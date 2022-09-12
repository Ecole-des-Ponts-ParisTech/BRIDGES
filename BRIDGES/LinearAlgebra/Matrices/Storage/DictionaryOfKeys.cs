using System;
using System.Collections.Generic;
using System.Text;

namespace BRIDGES.LinearAlgebra.Matrices.Storage
{
    /// <summary>
    /// Class defining a dictionary of keys storage for sparse matrix.
    /// </summary>
    public sealed class DictionaryOfKeys
    {
        #region Fields

        /// <summary>
        /// Values for the sparse matrix associated with there row-column pair.
        /// </summary>
        private Dictionary<(int, int), double> _values;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of elements in the <see cref="DictionaryOfKeys"/>.
        /// </summary>
        public int Count { get { return _values.Count; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="DictionaryOfKeys"/> class.
        /// </summary>
        public DictionaryOfKeys()
        {
            _values = new Dictionary<(int, int), double>();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DictionaryOfKeys"/> class.
        /// </summary>
        /// <param name="values"> Values of the new <see cref="DictionaryOfKeys"/>. </param>
        /// <param name="rows"> Row indices of the new <see cref="DictionaryOfKeys"/>. </param>
        /// <param name="columns"> Column indices of the new <see cref="DictionaryOfKeys"/>. </param>
        /// <exception cref="ArgumentException"> The input arrays should have the same length. </exception>
        public DictionaryOfKeys(double[] values, int[] rows, int[] columns)
        {
            if ((values.Length != rows.Length) | (values.Length != columns.Length))
            {
                throw new ArgumentException("The arrays should have the same length.");
            }

            _values = new Dictionary<(int, int), double>();
            for (int i = 0; i < values.Length; i++)
            {
                Add(values[i], rows[i], columns[i]);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Evaluates whether the <see cref="DictionaryOfKeys"/> contains an element at the given row and column index.
        /// </summary>
        /// <param name="row"> Row index.</param>
        /// <param name="column"> Column index. </param>
        /// <returns> <see langword="true"/> if the storage doesn't have element at the specified row and column index, <see langword="false"/> otherwise. </returns>
        public bool IsEmpty(int row, int column)
        {
            return (!_values.ContainsKey((row, column)));
        }


        /// <summary>
        /// Adds an element to the storage. If the storage contains an element at the given row and column, the value is added to the existing one.
        /// </summary>
        /// <param name="value"> Value to add. </param>
        /// <param name="row"> Row index of the value. </param>
        /// <param name="column"> Column index of the value. </param>
        public void Add(double value, int row, int column)
        {
            if (_values.ContainsKey((row, column))) // Complexity : O(1)
            {
                _values[(row, column)] += value;
            }
            else
            {
                _values.Add((row, column), value); // Complexity : O(1)
            }
        }

        /// <summary>
        /// Adds an element to the storage. If the storage contains an element at the given row and column, the value replaces the existing one.
        /// </summary>
        /// <param name="value"> Value to add. </param>
        /// <param name="row"> Row index of the value. </param>
        /// <param name="column"> Column index of the value. </param>
        public void AddOrReplace(double value, int row, int column)
        {
            if (_values.ContainsKey((row, column))) // Complexity : O(1)
            {
                _values[(row, column)] = value;
            }
            else
            {
                _values.Add((row, column), value); // Complexity : O(1)
            }
        }

        /// <summary>
        /// Replaces an element of the storage at the given row and column.
        /// </summary>
        /// <param name="value"> Value to replace with. </param>
        /// <param name="row"> Row index of the value. </param>
        /// <param name="column"> Column index of the value. </param>
        /// <exception cref="MethodAccessException"> No element exist at the given row and column. </exception>
        public void Replace(double value, int row, int column)
        {
            if (_values.ContainsKey((row, column))) // Complexity : O(1)
            {
                _values[(row, column)] = value;
            }
            else
            {
                throw new MethodAccessException("No element exist at the given row and column.");
            }
        }

        /// <summary>
        /// Removes an element of the storage at the given row and column.
        /// </summary>
        /// <param name="row"> Row index of the value. </param>
        /// <param name="column"> Column index of the value. </param>
        /// <exception cref="MethodAccessException"> No element exist at the given row and column. </exception>
        public void Remove(int row, int column)
        {
            if (!_values.Remove((row, column)))
            {
                throw new MethodAccessException("No element exist at the given row and column.");
            }
        }


        /// <summary>
        /// Removes all zeros in the storage.
        /// </summary>
        /// <param name="tolerance"> Tolerance around the zero. </param>
        public void Clean(double tolerance = Settings.AbsolutePrecision)
        {
            List<(int, int)> keys = new List<(int, int)>();

            foreach (KeyValuePair<(int,int), double> kvp in _values)
            {
                if (Math.Abs(kvp.Value) < tolerance) { keys.Add(kvp.Key); }
            }

            for (int i_K = 0; i_K < keys.Count; i_K++)
            {
                _values.Remove(keys[i_K]);
            }
        }
        
        /// <summary>
        /// Makes the storage symmetrical by applying the operation s1/2*(A^T+A)
        /// </summary>
        public void MakeSymmetric()
        {
            var result = new Dictionary<(int, int), double>();

            foreach (KeyValuePair<(int, int), double> kvp in _values)
            {
                var k_RC = kvp.Key;
                var k_CR = (k_RC.Item2, k_RC.Item1);

                // If the value is on the diagonal
                if (k_RC.Item1 == k_RC.Item2)
                {
                    result.Add(k_RC, kvp.Value);
                }
                // If the symmetrical value exists
                else if(_values.ContainsKey(k_CR))
                {
                    if(!result.ContainsKey(k_RC))
                    {
                        double val = 0.5 * (kvp.Value + _values[k_CR]);

                        result.Add(k_RC, val);
                        result.Add(k_CR, val);
                    }
                    else { continue; }
                    
                }
                else if(!result.ContainsKey(k_RC))
                {
                    double val = 0.5 * kvp.Value;

                    result.Add(k_RC, val);
                    result.Add(k_CR, val);
                }
            }

            _values = result;
        }

        #endregion

        #region Other Methods

        /// <summary>
        /// Returns an enumerator which reads through the non-zero components of the current <see cref="DictionaryOfKeys"/>. <br/>
        /// The <see cref="KeyValuePair{TKey, TValue}"/> represents is composed of the row-column pair and the component value.
        /// </summary>
        /// <returns> The enumerator of the <see cref="DictionaryOfKeys"/>. </returns>
        public IEnumerator<KeyValuePair<(int, int), double>> GetNonZeros()
        {
            return _values.GetEnumerator();
        }


        #endregion
    }
}
