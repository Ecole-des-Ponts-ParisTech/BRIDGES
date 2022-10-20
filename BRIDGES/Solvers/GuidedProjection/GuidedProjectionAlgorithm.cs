using System;
using System.Collections.Generic;

using BRIDGES.LinearAlgebra.Vectors;
using BRIDGES.LinearAlgebra.Matrices.Sparse;
using BRIDGES.LinearAlgebra.Matrices.Storage;

using BRIDGES.Solvers.GuidedProjection.Interfaces;


namespace BRIDGES.Solvers.GuidedProjection
{
    /// <summary>
    /// Class defining a Guided Projection Algorithm solver.<br/>
    /// The algorithm is described in <see href="https://doi.org/10.1145/2601097.2601213"/>.
    /// </summary>
    public class GuidedProjectionAlgorithm
    {
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

        #endregion

        #region Properties

        /// <summary>
        /// Gets the vector containing all the components of the <see cref="GuidedProjectionAlgorithm"/>.
        /// </summary>
        public Vector X { get { return _x; } }


        /**************************************** Settings ****************************************/

        /// <summary>
        /// Gets or sets the number of iteration after which the solver is stopped.
        /// </summary>
        public int MaxIteration { get; set; }

        /// <summary>
        /// Gets the number of executed iteration.
        /// </summary>
        public int IterationCount { get; internal set; }


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
            IterationCount = 0;

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
        /// Creates and adds a new <see cref="Energy"/> to the list.
        /// </summary>
        /// <param name="energyType"> Energy type defining the energy locally. </param>
        /// <param name="variables"> Variables composing the local vector xReduced. </param>
        /// <param name="weight">  Weight for the energy. </param>
        /// <returns> The new energy. </returns>
        public Energy AddEnergy(IEnergyType energyType, List<(VariableSet, int)> variables, double weight = 1.0)
        {
            int energyIndex = _energies.Count;

            Energy energy = new Energy(energyType, variables, weight);
            _energies.Add(energy);
            return energy;
        }


        /******************** For Quadratic Constraints ********************/

        /// <summary>
        /// Creates and adds a new <see cref="QuadraticConstraint"/> to the list.
        /// </summary>
        /// <param name="constraintType"> Quadratic constraint type defining the constraint locally. </param>
        /// <param name="variables"> Variables composing the local vector xReduced. </param>
        /// <param name="weight">  Weight for the constraint. </param>
        /// <returns> The new constraint. </returns>
        public QuadraticConstraint AddConstraint(IQuadraticConstraintType constraintType, List<(VariableSet, int)> variables, double weight = 1.0)
        {
            int constraintIndex = _constraints.Count;

            QuadraticConstraint constraint = new QuadraticConstraint(constraintType, variables, weight);
            _constraints.Add(constraint);
            return constraint;
        }

        /// <summary>
        /// Creates and adds a new <see cref="LinearisedConstraint"/> to the list.
        /// </summary>
        /// <param name="constraintType"> Quadratic constraint type defining the constraint locally. </param>
        /// <param name="variables"> Variables composing the local vector xReduced. </param>
        /// <param name="weight">  Weight for the constraint. </param>
        /// <returns> The new constraint. </returns>
        public LinearisedConstraint AddConstraint(ILinearisedConstraintType constraintType, List<(VariableSet, int)> variables, double weight = 1.0)
        {
            int constraintIndex = _constraints.Count;

            LinearisedConstraint constraint = new LinearisedConstraint(constraintType, variables, weight);
            _constraints.Add(constraint);
            return constraint;
        }

        /**************************************** For Solving ****************************************/

        /// <summary>
        /// Initialise the solver for the <see cref="GuidedProjectionAlgorithm"/>.
        /// </summary>
        public void InitialiseSolver()
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


            /******************** Create global Hi and global Bi (Constraint) ********************/

            // Create global Hi and global Bi
            for (int i_Cstr = 0; i_Cstr < _constraints.Count; i_Cstr++)
            {
                if(_constraints[i_Cstr] is LinearisedConstraint linCstr) 
                {
                    double[] xReduced = linCstr.GetXReduced(ref _x);

                    linCstr.UpdateLocal(xReduced);
                }

                _constraints[i_Cstr].Globalise();
            }


            /******************** Create global Ki (Energy) ********************/

            // Create global Ki
            for (int i_Energy = 0; i_Energy < _energies.Count; i_Energy++)
            {
                _energies[i_Energy].Globalise();
            }
        }

        /// <summary>
        /// Runs one iteration.
        /// </summary>
        public void DoOneIteration()
        {
            /******************** Iterate on the quadratic constraints to create H and r ********************/

            DictionaryOfKeys dok_H = new DictionaryOfKeys();
            DenseVector vect_r = new DenseVector(_x.Size);

            int constraintCount = 0;
            for (int i_Cstr = 0; i_Cstr < _constraints.Count; i_Cstr++)
            {
                QuadraticConstraint cstr = _constraints[i_Cstr];

                /******************** Use of Hi ********************/
                if (!(cstr.GlobalHi is null))
                {
                    Dictionary<int, double> HiX = Multiply(cstr.GlobalHi, _x);

                    // Iterate on the keys (RowIndex) of the sparse vector.
                    foreach (int key in HiX.Keys) // *.Keys is an O(1) operation
                    {
                        dok_H.Add(cstr.Weight * HiX[key], constraintCount, key); // Fill H with the contributions of HiX
                    }

                    vect_r[constraintCount] = 0.5 * cstr.Weight * TransposeMultiply(_x, HiX);
                }


                /******************** Use of Bi ********************/
                if (!(cstr.GlobalBi is null))
                {
                    Dictionary<int, double> Bi = cstr.GlobalBi;

                    // Iterate on the keys (RowIndex) of the sparse vector.
                    foreach (int key in Bi.Keys) // *.Keys is an O(1) operation.
                    {
                        // If value (RowIndex, ColumnIndex) already exists in dok_H, the value is added to the existing one.
                        // This is achieved trough *.ContainsKey() which is an O(1) operation.
                        dok_H.Add(cstr.Weight * Bi[key], constraintCount, key); // Fill H with the contributions of Bi
                    }
                }

                /******************** Use of Ci ********************/
                if (!(cstr.Ci == 0.0))
                {
                    double Ci = (double)cstr.Ci;
                    vect_r[constraintCount] -= cstr.Weight * Ci;
                }


                constraintCount++;
            }


            /******************** Iterate on the energies to create K and s ********************/

            DictionaryOfKeys dok_K = new DictionaryOfKeys();
            DenseVector vect_s = new DenseVector(_x.Size);

            int energyCount = 0;
            for (int i_Energy = 0; i_Energy < _energies.Count; i_Energy++)
            {
                Energy energy = _energies[i_Energy];

                /******************** Use of Ki ********************/
                if (!(energy.GlobalKi is null))
                {
                    Dictionary<int, double> Ki = energy.GlobalKi;

                    // Iterate on the keys (RowIndex) of the sparse vector.
                    foreach (int key in Ki.Keys) // *.Keys is an O(1) operation.
                    {
                        dok_K.Add(energy.Weight * Ki[key], energyCount, key);
                    }
                }

                /******************** Use of Si ********************/
                if (!(energy.Si == 0.0))
                {
                    double Si = (double)energy.Si;
                    vect_s[energyCount] += energy.Weight * Si;
                }


                energyCount++;
            }


            /******************** Solve the minimisation problem ********************/

            CompressedColumn LHS; // Left hand side of the equation
            DenseVector RHS; // Right hand side of the equation

            if (dok_H._values.Count != 0 && dok_K._values.Count != 0)
            {
                CompressedColumn sparse_H = new CompressedColumn(_x.Size, _x.Size, dok_H);
                CompressedColumn sparse_K = new CompressedColumn(_x.Size, _x.Size, dok_K);

                LHS = CompressedColumn.Add(CompressedColumn.TransposeMultiplySelf(sparse_H), CompressedColumn.TransposeMultiplySelf(sparse_K));
                RHS = DenseVector.Add(CompressedColumn.TransposeMultiply(sparse_H, vect_r), CompressedColumn.TransposeMultiply(sparse_K, vect_s));
            }
            else
            {
                if (dok_H._values.Count != 0)
                {
                    CompressedColumn sparse_H = new CompressedColumn(_x.Size, _x.Size, dok_H);

                    LHS = CompressedColumn.TransposeMultiplySelf(sparse_H);
                    RHS = CompressedColumn.TransposeMultiply(sparse_H, vect_r);
                }
                else
                {
                    CompressedColumn sparse_K = new CompressedColumn(_x.Size, _x.Size, dok_K);

                    LHS = CompressedColumn.TransposeMultiplySelf(sparse_K);
                    RHS = CompressedColumn.TransposeMultiply(sparse_K, vect_s);
                }
            }

            LHS = CompressedColumn.Add(LHS, CompressedColumn.Multiply(Epsilon * Epsilon, CompressedColumn.Identity(_x.Size)));
            RHS = DenseVector.Add(RHS, DenseVector.Multiply(Epsilon * Epsilon, _x));

            _x = LHS.SolveCholesky(RHS);

            /******************** Update Variables ********************/

            // Get the size X
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


        #region Helpers

        /**************************************** For Iteration ****************************************/

        private Dictionary<int, double> Multiply(DictionaryOfKeys matrix, DenseVector vector)
        {
            Dictionary<(int, int), double> values = matrix._values;

            Dictionary<int, double> product = new Dictionary<int, double>(values.Count);

            // Iterate on the keys (RowIndex, ColumnIndex) of the matrix
            foreach ((int, int) key in matrix._values.Keys) // *.Keys is an O(1) operation
            {
                if (product.ContainsKey(key.Item1)) // *.ContainsKey is an O(1) operation
                {
                    product[key.Item1] += values[key] * vector[key.Item2];
                }
                else { product.Add(key.Item1, values[key] * vector[key.Item2]); }
            }

            return product;
        }

        private double TransposeMultiply(Vector leftVector, Dictionary<int, double> rightSparseVector)
        {
            // Output of the method
            double dotProduct = 0;

            // Iterate on the keys (RowIndex) of the sparse vector.
            foreach (int key in rightSparseVector.Keys) // *.Keys is an O(1) operation
            {
                dotProduct += leftVector[key] * rightSparseVector[key];
            }

            return dotProduct;
        }


        #endregion
    }
}
