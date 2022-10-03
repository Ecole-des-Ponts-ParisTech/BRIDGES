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
        /// List of constraints sets for the <see cref="GuidedProjectionAlgorithm"/>.
        /// </summary>
        private List<IConstraintSet> _constraintSets;

        /// <summary>
        /// List of energies sets for the <see cref="GuidedProjectionAlgorithm"/>.
        /// </summary>
        private List<IEnergySet> _energySets;

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
            _constraintSets = new List<IConstraintSet>();
            _energySets = new List<IEnergySet>();

            // Initialize Iteration Properties
            MaxIteration = maxIteration;
            IterationCount = 0;

            // Initialize Solving Properties
            Epsilon = epsilon;
        }

        #endregion

        #region Methods

        /**************************************** For Variables ****************************************/

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
            if (setIndex == 0) { firstRank = 0;}
            else
            {
                VariableSet previousSet = _variableSets[setIndex - 1];
                firstRank = previousSet.FirstRank + (previousSet.VariableCount * previousSet.VariableDimension);
            }

            VariableSet newSet = new VariableSet(setIndex, firstRank, setCapacity, variableDimension);
            _variableSets.Add(newSet);
            return newSet;
        }


        /**************************************** For Quadratic Constraints ****************************************/

        /// <summary>
        /// Creates a new <see cref="QuadraticConstraintSet"/> and adds it after the other constraint sets.
        /// </summary>
        /// <param name="constraintType"> Instance of a quadratic constraint type for the set. </param>
        /// <param name="setCapacity"> Indicative number of quadratic constraints that the set can initially store. </param>
        /// <returns> The newly created <see cref="QuadraticConstraintSet"/>. </returns>
        public QuadraticConstraintSet AddQuadraticConstraintSet(IQuadraticConstraintType constraintType, int setCapacity)
        {
            int setIndex = _constraintSets.Count;

            QuadraticConstraintSet quadCstrSet = new QuadraticConstraintSet(constraintType, setIndex, setCapacity);
            _constraintSets.Add(quadCstrSet);
            return quadCstrSet;
        }

        /// <summary>
        /// Creates a new <see cref="LinearisedConstraintSet"/> and adds it after the other constraint sets.
        /// </summary>
        /// <param name="constraintType"> Instance of a linearised constraint type for the set. </param>
        /// <param name="setCapacity"> Indicative number of linearised constraints that the set can initially store. </param>
        /// <returns> The newly created <see cref="LinearisedConstraintSet"/>.</returns>
        public LinearisedConstraintSet AddLinearisedConstraintSet(ILinearisedConstraintType constraintType, int setCapacity)
        {
            int setIndex = _constraintSets.Count;

            LinearisedConstraintSet linCstrSet = new LinearisedConstraintSet(constraintType, setIndex, setCapacity);
            _constraintSets.Add(linCstrSet);
            return linCstrSet;
        }


        /**************************************** For Energies ****************************************/

        /// <summary>
        /// Creates a new <see cref="EnergySet{TEnergy}"/> and adds it after the other sets.
        /// </summary>
        /// <typeparam name="TEnergy"> Energy type. </typeparam>
        /// <param name="energyType"> Instance of an energy type for the set. </param>
        /// <param name="setCapacity"> Indicative number of energies that the new set can initially store. </param>
        /// <returns> The newly created <see cref="EnergySet{TEnergy}"/>. </returns>
        public EnergySet<TEnergy> AddEnergySet<TEnergy>(TEnergy energyType, int setCapacity)
            where TEnergy : IEnergyType, new()
        {
            int setIndex = _energySets.Count;

            EnergySet<TEnergy> energySet = new EnergySet<TEnergy>(energyType, setIndex, setCapacity);
            _energySets.Add(energySet);
            return energySet;
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
            for (int i_CstrSet = 0; i_CstrSet < _constraintSets.Count; i_CstrSet++)
            {
                _constraintSets[i_CstrSet].ComputeAndGlobalise();           
            }


            /******************** Create global Ki (Energy) ********************/

            // Create global Ki
            for (int i_EnergySet = 0; i_EnergySet < _energySets.Count; i_EnergySet++)
            {
                _energySets[i_EnergySet].ComputeAndGlobalise();
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
            for (int i_CstrSet = 0; i_CstrSet < _constraintSets.Count; i_CstrSet++)
            {
                IConstraintSet cstrSet = _constraintSets[i_CstrSet];

                for (int i_Cstr = 0; i_Cstr < cstrSet.ConstraintCount; i_Cstr++)
                {
                    IConstraint cstr = cstrSet[i_Cstr];

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
                    if (!(cstr.Ci is null)) 
                    {
                        double Ci = (double)cstr.Ci;
                        vect_r[constraintCount] -= cstr.Weight * Ci; 
                    }
                        

                    constraintCount++;
                }
            }


            /******************** Iterate on the energies to create K and s ********************/

            DictionaryOfKeys dok_K = new DictionaryOfKeys();
            DenseVector vect_s = new DenseVector(_x.Size);

            int energyCount = 0;
            for (int i_EnergySet = 0; i_EnergySet < _energySets.Count; i_EnergySet++)
            {
                IEnergySet energySet = _energySets[i_EnergySet];

                for (int i_Energy = 0; i_Energy < energySet.EnergyCount; i_Energy++)
                {
                    IEnergy energy = energySet[i_Energy];

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

                    /******************** Use of Ci ********************/
                    if (!(energy.Si is null)) 
                    {
                        double Si = (double)energy.Si;
                        vect_s[energyCount] += energy.Weight * Si; 
                    }


                    energyCount++;
                }
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
                    RHS = CompressedColumn.TransposeMultiply(sparse_H, vect_r) ;
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
            foreach ((int,int) key in matrix._values.Keys) // *.Keys is an O(1) operation
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
