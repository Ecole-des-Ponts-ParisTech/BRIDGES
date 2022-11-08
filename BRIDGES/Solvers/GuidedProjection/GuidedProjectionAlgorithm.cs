using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using BRIDGES.LinearAlgebra.Vectors;
using BRIDGES.LinearAlgebra.Matrices;
using BRIDGES.LinearAlgebra.Matrices.Sparse;
using BRIDGES.LinearAlgebra.Matrices.Storage;

using BRIDGES.Solvers.GuidedProjection.Interfaces;
using System.Drawing;

namespace BRIDGES.Solvers.GuidedProjection
{
    /// <summary>
    /// Class defining a Guided Projection Algorithm solver.<br/>
    /// The algorithm is described in <see href="https://doi.org/10.1145/2601097.2601213"/>.
    /// </summary>
    public sealed class GuidedProjectionAlgorithm
    {
        #region Events

        /******************** For Weigths ********************/

        /// <summary>
        /// Event raised whenever <see cref="Energy"/> and <see cref="QuadraticConstraint"/> weights needs to be updated.
        /// </summary>
        private event Action<int> WeigthUpdate;

        /// <summary>
        /// Raises the event which updates the <see cref="Energy"/> and the <see cref="QuadraticConstraint"/> weights. 
        /// </summary>
        /// <param name="iteration"> Index of the current iteration. </param>
        private void OnWeigthUpdate(int iteration)
        {
            WeigthUpdate?.Invoke(iteration);
        }


        /******************** For Quadratic Constraints ********************/

        /// <summary>
        /// Event raised whenever the members of <see cref="LinearisedConstraint"/> needs to be updated.
        /// </summary>
        private event Action<DenseVector> ConstraintUpdate;

        /// <summary>
        /// Raises the event which updates the members of <see cref="LinearisedConstraint"/>.
        /// </summary>
        /// <param name="x"> Global vector X at the current iteration. </param>
        private void OnConstraintUpdate(in DenseVector x)
        {
            ConstraintUpdate?.Invoke(x);
        }

        #endregion

        #region Fields

        /// <summary>
        /// List of the variable sets for the <see cref="GuidedProjectionAlgorithm"/>.
        /// </summary>
        private List<VariableSet> _variableSets;

        /// <summary>
        /// List of the energies for the <see cref="GuidedProjectionAlgorithm"/>.
        /// </summary>
        private List<Energy> _energies;

        /// <summary>
        /// List of the constraints for the <see cref="GuidedProjectionAlgorithm"/>.
        /// </summary>
        private List<QuadraticConstraint> _constraints;

        /// <summary>
        /// Vector containing the variables of the <see cref="GuidedProjectionAlgorithm"/>.
        /// </summary>
        private DenseVector _x;

        /// <summary>
        /// Identity matrix multiplied by Epsilon*Epsilon.
        /// </summary>
        private CompressedColumn _epsEpsIdentity;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the vector containing all the components of the <see cref="GuidedProjectionAlgorithm"/>.
        /// </summary>
        public Vector X { get { return _x; } }


        /**************************************** Settings ****************************************/

        /// <summary>
        /// Gets or sets the maximum number of iteration after which the solver is stopped.
        /// </summary>
        public int MaxIteration { get; set; }

        /// <summary>
        /// Gets the zero-based index of the current iteration.
        /// </summary>
        public int IterationIndex { get; internal set; }


        /**************************************** For Solving ****************************************/

        /// <summary>
        /// Gets or sets the weight of the distance to the previous iteration.
        /// </summary>
        double Epsilon { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GuidedProjectionAlgorithm"/> class.
        /// </summary>
        /// <param name="epsilon"> The weights of the distance to the previous iteration. </param>
        /// <param name="maxIteration"> The iteration index after which the solver is stopped. </param>
        public GuidedProjectionAlgorithm(double epsilon, int maxIteration)
        {
            // Instanciate Fields
            _variableSets = new List<VariableSet>();

            _energies = new List<Energy>();

            _constraints = new List<QuadraticConstraint>();


            // Initialize Properties
            MaxIteration = maxIteration;
            IterationIndex = 0;

            Epsilon = epsilon;
        }

        #endregion

        #region Methods

        /******************** For Variables ********************/

        /// <summary>
        /// Creates a new <see cref="VariableSet"/> and adds it after the other ones.
        /// </summary>
        /// <param name="variableDimension"> The dimension of the variables contained in set. </param>
        /// <returns> The newly created <see cref="VariableSet"/>. </returns>
        public VariableSet AddVariableSet(int variableDimension)
        {
            int setIndex = _variableSets.Count;

            int firstRank;
            if (setIndex == 0) { firstRank = 0; }
            else
            {
                VariableSet previousSet = _variableSets[setIndex - 1];
                firstRank = previousSet.FirstRank + (previousSet.VariableCount * previousSet.VariableDimension);
            }

            VariableSet newSet = new VariableSet(setIndex, firstRank, variableDimension);
            _variableSets.Add(newSet);
            return newSet;
        }

        /// <summary>
        /// Creates a new <see cref="VariableSet"/> and adds it after the other ones.
        /// </summary>
        /// <param name="variableDimension"> The dimension of the variables contained in set. </param>
        /// <param name="setCapacity"> The indicative number of variables that the new set can initially store. </param>
        /// <returns> The newly created <see cref="VariableSet"/>. </returns>
        public VariableSet AddVariableSet(int variableDimension, int setCapacity)
        {
            int setIndex = _variableSets.Count;

            int firstRank;
            if (setIndex == 0) { firstRank = 0; }
            else
            {
                VariableSet previousSet = _variableSets[setIndex - 1];
                firstRank = previousSet.FirstRank + (previousSet.VariableCount * previousSet.VariableDimension);
            }

            VariableSet newSet = new VariableSet(setIndex, firstRank, variableDimension, setCapacity);
            _variableSets.Add(newSet);
            return newSet;
        }


        /******************** For Energies ********************/

        /// <summary>
        /// Creates a new <see cref="Energy"/> with a constant weight and adds it to the list.
        /// </summary>
        /// <param name="energyType"> Energy type defining the energy locally. </param>
        /// <param name="variables"> Variables composing the local vector xReduced. </param>
        /// <param name="weight">  Weight for the energy. </param>
        /// <returns> The new energy. </returns>
        public Energy AddEnergy(IEnergyType energyType, List<(VariableSet, int)> variables, double weight = 1.0)
        {
            Energy energy = new Energy(energyType, variables, weight);
            _energies.Add(energy);

            return energy;
        }

        /// <summary>
        /// Creates a new <see cref="Energy"/> with a varying weight and adds it to the list.
        /// </summary>
        /// <param name="energyType"> Energy type defining the energy locally. </param>
        /// <param name="variables">  Variables composing the local vector xReduced. </param>
        /// <param name="weightFunction"> Function computing the weight from the iteration index. </param>
        /// <returns></returns>
        public Energy AddEnergy(IEnergyType energyType, List<(VariableSet, int)> variables, Func<int,double> weightFunction)
        {
            Energy energy = new Energy(energyType, variables, 0.0);
            _energies.Add(energy);

            void energyWeightUpdater(int iteration) => energy.Weight = weightFunction(iteration);
            WeigthUpdate += energyWeightUpdater;

            return energy;
        }


        /******************** For Quadratic Constraints ********************/

        /// <summary>
        /// Creates a new <see cref="QuadraticConstraint"/> with a constant weight and adds it to the list.
        /// </summary>
        /// <param name="constraintType"> Quadratic constraint type defining the constraint locally. </param>
        /// <param name="variables"> Variables composing the local vector xReduced. </param>
        /// <param name="weight">  Weight for the constraint. </param>
        /// <returns> The new constraint. </returns>
        public QuadraticConstraint AddConstraint(IQuadraticConstraintType constraintType, List<(VariableSet, int)> variables, double weight = 1.0)
        {
            QuadraticConstraint constraint = new QuadraticConstraint(constraintType, variables, weight);
            _constraints.Add(constraint);

            return constraint;
        }

        /// <summary>
        /// Creates a new <see cref="QuadraticConstraint"/> with a varying weight and adds it to the list.
        /// </summary>
        /// <param name="constraintType"> Quadratic constraint type defining the constraint locally. </param>
        /// <param name="variables"> Variables composing the local vector xReduced. </param>
        /// <param name="weightFunction"> Function computing the weight from the iteration index. </param>
        /// <returns> The new constraint. </returns>
        public QuadraticConstraint AddConstraint(IQuadraticConstraintType constraintType, List<(VariableSet, int)> variables, Func<int, double> weightFunction)
        {
            QuadraticConstraint constraint = new QuadraticConstraint(constraintType, variables, 0.0);
            _constraints.Add(constraint);

            void constraintWeightUpdater(int iteration) => constraint.Weight = weightFunction(iteration);
            WeigthUpdate += constraintWeightUpdater;

            return constraint;
        }


        /******************** For Linearised Constraints ********************/

        /// <summary>
        /// Creates a new <see cref="LinearisedConstraint"/> with a constant weight and adds it to the list.
        /// </summary>
        /// <param name="constraintType"> Quadratic constraint type defining the constraint locally. </param>
        /// <param name="variables"> Variables composing the local vector xReduced. </param>
        /// <param name="weight">  Weight for the constraint. </param>
        /// <returns> The new constraint. </returns>
        public LinearisedConstraint AddConstraint(ILinearisedConstraintType constraintType, List<(VariableSet, int)> variables, double weight = 1.0)
        {
            LinearisedConstraint constraint = new LinearisedConstraint(constraintType, variables, weight);
            _constraints.Add(constraint);

            return constraint;
        }

        /// <summary>
        /// Creates a new <see cref="LinearisedConstraint"/> with a varying weight and adds it to the list.
        /// </summary>
        /// <param name="constraintType"> Quadratic constraint type defining the constraint locally. </param>
        /// <param name="variables"> Variables composing the local vector xReduced. </param>
        /// <param name="weightFunction"> Function computing the weight from the iteration index. </param>
        /// <returns> The new constraint. </returns>
        public LinearisedConstraint AddConstraint(ILinearisedConstraintType constraintType, List<(VariableSet, int)> variables, Func<int, double> weightFunction)
        {
            LinearisedConstraint constraint = new LinearisedConstraint(constraintType, variables, 0.0);
            _constraints.Add(constraint);

            void constraintWeightUpdater(int iteration) => constraint.Weight = weightFunction(iteration);
            WeigthUpdate += constraintWeightUpdater;

            return constraint;
        }


        /**************************************** For Solving ****************************************/

        /// <summary>
        /// Initialise the solver for the <see cref="GuidedProjectionAlgorithm"/>.
        /// </summary>
        public void InitialiseX()
        {
            /******************** Create global X (Variable) ********************/

            // Get the size X
            VariableSet lastVariableSet = _variableSets[_variableSets.Count - 1];
            int sizeX = lastVariableSet.FirstRank + (lastVariableSet.VariableCount * lastVariableSet.VariableDimension);

            // Create and fill X
            _x = new DenseVector(sizeX);
            for (int i_VariableSet = 0; i_VariableSet < _variableSets.Count; i_VariableSet++)
            {
                VariableSet variableSet = _variableSets[i_VariableSet];
                int componentCount = variableSet.VariableCount * variableSet.VariableDimension;
                for (int i_Component = 0; i_Component < componentCount; i_Component++)
                {
                    _x[variableSet.FirstRank + i_Component] = variableSet.GetComponent(i_Component);
                }
            }


            /******************** Create Utilities ********************/

            _epsEpsIdentity = CompressedColumn.Multiply(Epsilon * Epsilon, CompressedColumn.Identity(_x.Size));
        }

        /// <summary>
        /// Runs one iteration.
        /// </summary>
        /// <param name="useAsync"> Evaluates whether the iteration should use asynchronous programming or not. </param>
        public void RunIteration(bool useAsync)
        {
            /********** Iteration Updates **********/

            OnConstraintUpdate(_x);

            OnWeigthUpdate(IterationIndex);


            /********** Formulate and Solve the System  **********/

            if (useAsync)
            {
                var task = FormAndSolveSystem_Async();

                Task.WaitAll(task);

                _x = task.Result;
            }
            else
            {
                _x = FormAndSolveSystem();
            }

            /******************** Update Variables ********************/

            // Fill the VariableSet with the updated values
            for (int i_VariableSet = 0; i_VariableSet < _variableSets.Count; i_VariableSet++)
            {
                VariableSet variableSet = _variableSets[i_VariableSet];
                int componentCount = variableSet.VariableCount * variableSet.VariableDimension;
                for (int i_Component = 0; i_Component < componentCount; i_Component++)
                {
                    variableSet.SetComponent(i_Component, _x[variableSet.FirstRank + i_Component]);
                }
            }
        }

        #endregion

        #region Other Methods

        /// <summary>
        /// Compute the members of the system and solves it using Cholesky factorisation.
        /// </summary>
        /// <returns> The solution of the sytem. </returns>
        private DenseVector FormAndSolveSystem()
        {
            /******************** Iterate on the quadratic constraints to create H and r ********************/

            FormConstraintMembers(out CompressedColumn H, out DenseVector r);


            /******************** Iterate on the energies to create K and s ********************/

            FromEnergyMembers(out CompressedColumn K, out SparseVector s);


            /******************** Solve the minimisation problem ********************/

            CompressedColumn LHS; // Left hand side of the equation
            DenseVector RHS; // Right hand side of the equation

            if (H.NonZerosCount != 0 && K.NonZerosCount != 0)
            {
                CompressedColumn HtH = CompressedColumn.TransposeMultiplySelf(H);
                CompressedColumn KtK = CompressedColumn.TransposeMultiplySelf(K);

                LHS = CompressedColumn.Add(HtH, KtK);
                RHS = DenseVector.Add(CompressedColumn.TransposeMultiply(H, r), CompressedColumn.TransposeMultiply(K, s));
            }
            else
            {
                if (H.NonZerosCount != 0)
                {
                    LHS = CompressedColumn.TransposeMultiplySelf(H);
                    RHS = CompressedColumn.TransposeMultiply(H, r);
                }
                else if (K.NonZerosCount != 0)
                {
                    DenseVector tmp = new DenseVector(s.ToArray());

                    LHS = CompressedColumn.TransposeMultiplySelf(K);
                    RHS = CompressedColumn.TransposeMultiply(K, tmp);
                }
                else { throw new InvalidOperationException("The matrices H and K are empty."); }
            }

            LHS = CompressedColumn.Add(LHS, _epsEpsIdentity);
            RHS = DenseVector.Add(RHS, DenseVector.Multiply(Epsilon * Epsilon, _x));

            return LHS.SolveCholesky(RHS);
        }

        /// <summary>
        /// Compute the members of the system and solves it using Cholesky factorisation.
        /// </summary>
        /// <returns> The solution of the sytem. </returns>
        private async Task<DenseVector> FormAndSolveSystem_Async()
        {
            CompressedColumn LHS;   // Left hand side of the equation
            Vector RHS;        // Right hand side of the equation


            var task_FormConstraintMembers = FormConstraintMembers();
            var task_FormEnergyMembers = FormEnergyMembers();

            Task<(CompressedColumn Matrix, Vector Vector)> task_FormMember;
            Task<CompressedColumn> task_LHS; Task<Vector> task_RHS;

            Task finishedTask = await Task.WhenAny(task_FormConstraintMembers, task_FormEnergyMembers);
            if (finishedTask == task_FormConstraintMembers)
            {
                (CompressedColumn HtH, Vector Htr) = task_FormConstraintMembers.Result;

                task_LHS = AddMatrices(HtH, _epsEpsIdentity);
                task_RHS = AddVectors(Htr, DenseVector.Multiply(Epsilon * Epsilon, _x));

                task_FormMember = task_FormEnergyMembers;
            }
            else
            {
                (CompressedColumn KtK, Vector Kts) = task_FormEnergyMembers.Result;

                task_LHS = AddMatrices(KtK, _epsEpsIdentity);
                task_RHS = AddVectors(Kts, DenseVector.Multiply(Epsilon * Epsilon, _x));

                task_FormMember = task_FormConstraintMembers;
            }

            List<Task> activeTasks = new List<Task> { task_LHS, task_RHS, task_FormMember };

            while (activeTasks.Count > 0)
            {
                await Task.WhenAny(activeTasks);

                if (task_FormMember.IsCompleted && task_LHS.IsCompleted)
                {
                    activeTasks.Remove(task_FormMember); activeTasks.Remove(task_LHS);

                    CompressedColumn matrix = task_FormMember.Result.Matrix;
                    LHS = task_LHS.Result;

                    task_LHS = AddMatrices(LHS, matrix);
                }
                if (task_FormMember.IsCompleted && task_RHS.IsCompleted)
                {
                    activeTasks.Remove(task_FormMember); activeTasks.Remove(task_RHS);

                    Vector vector = task_FormMember.Result.Vector;
                    RHS = task_RHS.Result;

                    task_RHS = AddVectors(RHS, vector);
                }
            }

            Task.WaitAll(task_LHS, task_RHS);

            LHS = task_LHS.Result;
            RHS = task_RHS.Result;

            return LHS.SolveCholesky(RHS);
        }

        #endregion


        #region Helper - Synchronous

        /// <summary>
        /// Forms the system members derived from the constraints.
        /// </summary>
        /// <param name="H"> The matrix H. </param>
        /// <param name="r"> The vector r. </param>
        private void FormConstraintMembers(out CompressedColumn H, out DenseVector r)
        {
            DictionaryOfKeys dok_H = new DictionaryOfKeys();
            List<double> list_r = new List<double>(_constraints.Count);

            int constraintCount = 0;

            for (int i_Cstr = 0; i_Cstr < _constraints.Count; i_Cstr++)
            {
                QuadraticConstraint cstr = _constraints[i_Cstr];

                // Verifications
                if (cstr.Weight == 0d) { continue; }


                List<(VariableSet Set, int Index)> variables = cstr.variables;
                IQuadraticConstraintType constraintType = cstr.constraintType;

                int size = constraintType.LocalHi.ColumnCount;



                // Translating the local indices of the constraint defined on xReduced into global indices defined on x.
                List<int> rowIndex = new List<int>(size);
                for (int i_Variable = 0; i_Variable < variables.Count; i_Variable++)
                {
                    int startIndex = variables[i_Variable].Set.FirstRank + (variables[i_Variable].Set.VariableDimension * variables[i_Variable].Index);

                    for (int i_VarComp = 0; i_VarComp < variables[i_Variable].Set.VariableDimension; i_VarComp++)
                    {
                        rowIndex.Add(startIndex + i_VarComp);
                    }
                }


                /******************** Create xReduced ********************/

                double[] components = new double[size];
                for (int i_Comp = 0; i_Comp < size; i_Comp++)
                {
                    components[i_Comp] = _x[rowIndex[i_Comp]];
                }
                DenseVector xReduced = new DenseVector(components);


                /******************** Compute Temporary Values ********************/

                // Compute HiX
                DenseVector tmp_Vect = SparseMatrix.Multiply(constraintType.LocalHi, xReduced);

                // Compute XtHiX
                double tmp_Val = DenseVector.TransposeMultiply(xReduced, tmp_Vect);


                /******************** For r ********************/

                if (cstr.constraintType.Ci == 0.0) { list_r.Add(cstr.Weight * 0.5 * tmp_Val); }
                else { list_r.Add(cstr.Weight * (0.5 * tmp_Val - constraintType.Ci)); }


                /******************** For H ********************/

                if (!(cstr.constraintType.LocalBi is null))
                {
                    tmp_Vect = DenseVector.Add(tmp_Vect, cstr.constraintType.LocalBi);
                }

                for (int i_Comp = 0; i_Comp < size; i_Comp++)
                {
                    if (tmp_Vect[i_Comp] == 0d) { continue;  }
                    dok_H.Add(cstr.Weight * tmp_Vect[i_Comp], constraintCount, rowIndex[i_Comp]);
                }

                constraintCount++;
            }

            H = new CompressedColumn(constraintCount, _x.Size, dok_H);
            r = new DenseVector(list_r.ToArray());
        }

        /// <summary>
        /// Forms the system members derived from the energies.
        /// </summary>
        /// <param name="K"> The matrix K. </param>
        /// <param name="s"> The vector s. </param>
        private void FromEnergyMembers(out CompressedColumn K, out SparseVector s)
        {
            DictionaryOfKeys dok_K = new DictionaryOfKeys();
            Dictionary<int, double> dict_s = new Dictionary<int, double>(_energies.Count);

            int energyCount = 0;
            for (int i_Energy = 0; i_Energy < _energies.Count; i_Energy++)
            {
                Energy energy = _energies[i_Energy];

                // Verifications
                if (energy.Weight == 0d) { continue; }


                List<(VariableSet Set, int Index)> variables = energy.variables;
                IEnergyType energyType = energy.energyType;


                /******************** Create the Row Indices ********************/

                // Translating the local indices of the constraint defined on xReduced into global indices defined on x.
                List<int> rowIndex = new List<int>();
                for (int i_Variable = 0; i_Variable < energy.variables.Count; i_Variable++)
                {
                    int startIndex = variables[i_Variable].Set.FirstRank + (variables[i_Variable].Set.VariableDimension * variables[i_Variable].Index);

                    for (int i_Component = 0; i_Component < variables[i_Variable].Set.VariableDimension; i_Component++)
                    {
                        rowIndex.Add(startIndex + i_Component);
                    }
                }

                /******************** For s ********************/

                if (!(energyType.Si == 0.0))
                {
                    dict_s.Add(energyCount, energy.Weight * energy.energyType.Si);
                }

                /******************** For K ********************/

                foreach (var (RowIndex, Value) in energyType.LocalKi.GetNonZeros())
                {
                    dok_K.Add(energy.Weight * Value, energyCount, rowIndex[RowIndex]);
                }

                energyCount++;
            }

            K = new CompressedColumn(energyCount, _x.Size, dok_K);
            s = new SparseVector(energyCount, ref  dict_s);
        }

        #endregion

        #region Helpers - Asynchronous 

        /********** Constraint Members **********/

        private Task<(CompressedColumn HtH, Vector Htr)> FormConstraintMembers()
        {
            return Task.Run(() =>
            {
                System.Collections.Concurrent.ConcurrentBag<(SparseVector ColumnHt, double ValueR)> bag = new System.Collections.Concurrent.ConcurrentBag<(SparseVector ColumnHt, double ValueR)>();

                Parallel.For(0, _constraints.Count, (int i_Cstr) =>
                {
                    /******************** Initialise Iteration ********************/

                    QuadraticConstraint constraint = _constraints[i_Cstr];

                    if (constraint.Weight == 0d) { return; }


                    List<(VariableSet Set, int Index)> variables = constraint.variables;
                    IQuadraticConstraintType constraintType = constraint.constraintType;

                    int size = constraintType.LocalHi.ColumnCount;

                    /******************** Devise xReduced ********************/

                    DenseVector xReduced = DeviseXReduced(size, variables, out int[] rowIndices);


                    /******************** Compute Temporary Values ********************/

                    // Compute HiX
                    DenseVector tmp_Vect = SparseMatrix.Multiply(constraintType.LocalHi, xReduced);

                    // Compute XtHiX
                    double tmp_Val = DenseVector.TransposeMultiply(xReduced, tmp_Vect);


                    /******************** For r *******************/

                    if (constraintType.Ci == 0.0) { tmp_Val = constraint.Weight * 0.5 * tmp_Val; }
                    else { tmp_Val = constraint.Weight * (0.5 * tmp_Val - constraintType.Ci); }


                    /******************** For Ht *******************/


                    if (!(constraintType.LocalBi is null))
                    {
                        tmp_Vect = DenseVector.Add(tmp_Vect, constraintType.LocalBi);
                    }

                    Dictionary<int, double> components = new Dictionary<int, double>(size);
                    for (int i_Comp = 0; i_Comp < size; i_Comp++)
                    {
                        if (tmp_Vect[i_Comp] == 0d) { continue; }
                        components.Add(rowIndices[i_Comp], constraint.Weight * tmp_Vect[i_Comp]);
                    }

                    /******************** Finally *******************/

                    bag.Add((new SparseVector(X.Size, ref components), tmp_Val));

                });

                (CompressedColumn Ht, DenseVector r) = AssembleConstraintMembers(_x.Size, bag);

                (CompressedColumn HtH, DenseVector Htr) = MultiplyConstraintMembers(Ht, r);

                return (HtH, Htr as Vector);
            });
        }


        /// <summary>
        /// Devises the component of xReduced.
        /// </summary>
        /// <param name="size"> Size of xReduced. </param>
        /// <param name="variables"> Variables contained in xReduced. </param>
        /// <param name="rowIndices"> The row indices of the components composing xReduced. </param>
        /// <returns> The dense vector xReduced. </returns>
        private DenseVector DeviseXReduced(int size, List<(VariableSet Set, int Index)> variables, out int[] rowIndices)
        {
            /******************** Create the Row Indices ********************/

            // Translating the local indices of the constraint defined on xReduced into global indices defined on x.
            rowIndices = new int[size];

            int counter = 0;
            for (int i_Variable = 0; i_Variable < variables.Count; i_Variable++)
            {
                int startIndex = variables[i_Variable].Set.FirstRank + (variables[i_Variable].Set.VariableDimension * variables[i_Variable].Index);

                for (int i_VarComp = 0; i_VarComp < variables[i_Variable].Set.VariableDimension; i_VarComp++)
                {
                    rowIndices[counter] = startIndex + i_VarComp;
                    counter++;
                }
            }


            /******************** Create xReduced ********************/

            double[] components = new double[size];
            for (int i_Comp = 0; i_Comp < size; i_Comp++)
            {
                components[i_Comp] = _x[rowIndices[i_Comp]];
            }

            return new DenseVector(components);
        }


        /// <summary>
        /// Assemble the data to create the tranposed matrix Ht and the vector r
        /// </summary>
        /// <param name="size"> Size of the global vector x. </param>
        /// <param name="bag"> Collection containing the components of the constraint members. </param>
        /// <returns></returns>
        private (CompressedColumn Ht, DenseVector r) AssembleConstraintMembers(int size, System.Collections.Concurrent.ConcurrentBag<(SparseVector ColumnHt, double ValueR)> bag)
        {
            List<int> columnPointers = new List<int>();
            List<int> rowIndices = new List<int>();
            List<double> values = new List<double>();

            List<double> list_r = new List<double>();

            columnPointers.Add(0);
            foreach ((SparseVector ColumnHt, double ValueR) in bag)
            {
                foreach ((int RowIndex, double Value) in ColumnHt.GetNonZeros())
                {
                    rowIndices.Add(RowIndex);
                    values.Add(Value);
                }
                columnPointers.Add(values.Count);

                list_r.Add(ValueR);
            }

            CompressedColumn Ht = new CompressedColumn(size, bag.Count, columnPointers.ToArray(), rowIndices.ToArray(), values.ToArray());
            DenseVector r = new DenseVector(list_r.ToArray());

            return (Ht, r);
        }

        private (CompressedColumn HtH, DenseVector Htr) MultiplyConstraintMembers(CompressedColumn Ht, DenseVector r)
        {
            Task<CompressedColumn> task_HtH = Task.Run(() =>
            {
                CompressedColumn H = new CompressedColumn(Ht); H.Transpose();
                return CompressedColumn.Multiply(Ht, H);
            });

            Task<DenseVector> task_Htr = Task.Run(() =>
            {
                return CompressedColumn.Multiply(Ht, r);
            });

            Task.WaitAll(task_Htr, task_HtH);

            CompressedColumn HtH = task_HtH.Result;
            DenseVector Htr = task_Htr.Result;

            return (HtH, Htr);
        }


        /********** Energy Members **********/

        private Task<(CompressedColumn KtK, Vector Kts)> FormEnergyMembers()
        {
            return Task.Run(() => 
            {
                System.Collections.Concurrent.ConcurrentBag<(SparseVector ColumnKt, double ValueS)> bag = new System.Collections.Concurrent.ConcurrentBag<(SparseVector ColumnKt, double ValueS)>();

                Parallel.For(0, _energies.Count, (int i_Energy) => 
                {
                    Energy energy = _energies[i_Energy];

                    // Verifications
                    if (energy.Weight == 0d) { return; }

                    List<(VariableSet Set, int Index)> variables = energy.variables;
                    IEnergyType energyType = energy.energyType;

                    int size = energyType.LocalKi.Size;

                    /******************** Create the Row Indices ********************/

                    // Translating the local indices of the constraint defined on xReduced into global indices defined on x.
                    List<int> rowIndices = new List<int>();
                    for (int i_Variable = 0; i_Variable < energy.variables.Count; i_Variable++)
                    {
                        int startIndex = variables[i_Variable].Set.FirstRank + (variables[i_Variable].Set.VariableDimension * variables[i_Variable].Index);

                        for (int i_Component = 0; i_Component < variables[i_Variable].Set.VariableDimension; i_Component++)
                        {
                            rowIndices.Add(startIndex + i_Component);
                        }
                    }

                    /******************** For s ********************/

                    double tmp_Val = 0.0;
                    if (!(energyType.Si == 0.0))
                    {
                        tmp_Val = energy.Weight * energy.energyType.Si;
                    }

                    /******************** For Kt ********************/

                    Dictionary<int, double> components = new Dictionary<int, double>(size);
                    foreach (var (RowIndex, Value) in energyType.LocalKi.GetNonZeros())
                    {
                        components.Add(rowIndices[RowIndex], energy.Weight * Value);
                    }

                    /******************** Finally *******************/

                    bag.Add((new SparseVector(X.Size, ref components), tmp_Val));

                });

                (CompressedColumn Kt, SparseVector s) = AssembleEnergyMembers(_x.Size, bag);

                (CompressedColumn KtK, SparseVector Kts) = MultiplyEnergyMembers(Kt, s);

                return (KtK, Kts as Vector);
            });
        }


        private (CompressedColumn Kt, SparseVector s) AssembleEnergyMembers(int size, System.Collections.Concurrent.ConcurrentBag<(SparseVector ColumnKt, double ValueS)> bag)
        {
            List<int> columnPointers = new List<int>();
            List<int> rowIndices = new List<int>();
            List<double> values = new List<double>();

            Dictionary<int, double> dict_s = new Dictionary<int,double>();

            columnPointers.Add(0);
            int counter = 0;
            foreach ((SparseVector ColumnKt, double ValueS) in bag)
            {
                foreach ((int RowIndex, double Value) in ColumnKt.GetNonZeros())
                {
                    rowIndices.Add(RowIndex);
                    values.Add(Value);
                }
                columnPointers.Add(values.Count);

                if (ValueS != 0.0)
                dict_s.Add(counter, ValueS);

                counter++;
            }

            CompressedColumn Kt = new CompressedColumn(size, bag.Count, columnPointers.ToArray(), rowIndices.ToArray(), values.ToArray());
            SparseVector s = new SparseVector(bag.Count, ref dict_s);

            return (Kt, s);
        }

        private (CompressedColumn KtK, SparseVector Kts) MultiplyEnergyMembers(CompressedColumn Kt, SparseVector s)
        {
            Task<CompressedColumn> task_KtK = Task.Run(() =>
            {
                CompressedColumn K = new CompressedColumn(Kt); K.Transpose();
                return CompressedColumn.Multiply(Kt, K);
            });

            Task<SparseVector> task_Kts = Task.Run(() =>
            {
                return CompressedColumn.Multiply(Kt, s);
            });

            Task.WaitAll(task_KtK, task_Kts);

            CompressedColumn KtK = task_KtK.Result;
            SparseVector Kts = task_Kts.Result;

            return (KtK, Kts);
        }


        /********** Common Tasks **********/

        private Task<Vector> AddVectors(Vector left, Vector right)
        {
            return Task.Run(() => Vector.Add(left, right));
        }

        private Task<CompressedColumn> AddMatrices(CompressedColumn left, CompressedColumn right)
        {
            return Task.Run(() => CompressedColumn.Add(left, right));
        }


        #endregion
    }
}
