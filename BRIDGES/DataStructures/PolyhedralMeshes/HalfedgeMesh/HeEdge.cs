using System;
using System.Collections.Generic;

using BRIDGES.DataStructures.PolyhedralMeshes.Abstract;


namespace BRIDGES.DataStructures.PolyhedralMeshes.HalfedgeMesh
{
    /// <summary>
    /// Class for an edge in a polyhedral halfedge mesh data structure.
    /// </summary>
    /// <remarks>
    /// In a polyhedral halfedge mesh data structure, edges are represented by the halfedge whose indices are pair. <br/>
    /// Edges have the same behaviour as in other polyhedral mesh data structures but each edge (with index i) can be mapped to a pair of halfedge (with indices 2i and 2i+1).
    /// </remarks>
    /// <typeparam name="TPosition"> Type for the position of the vertex. </typeparam>
    public class HeEdge<TPosition> : Edge<TPosition, HeVertex<TPosition>, HeEdge<TPosition>, HeFace<TPosition>>
        where TPosition : IEquatable<TPosition>
    {
        #region Fields

        /// <summary>
        /// The halfedge whose index is twice the index of the current edge.
        /// </summary>
        private HeHalfedge<TPosition> _halfedge;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="HeEdge{TPosition}"/> class.
        /// </summary>
        /// <param name="halfedge"> Halfedge whose index is twice the index of the current edge. </param>
        internal HeEdge(HeHalfedge<TPosition> halfedge)
            : base(halfedge.Index / 2, halfedge.StartVertex, halfedge.EndVertex)
        {
            // Verification
            if( 2 * (halfedge.Index / 2) != halfedge.Index) { throw new ArgumentException("The index of the given halfedge is not pair."); }

            // Initialise fields
            _halfedge = halfedge;
        }

        #endregion


        #region Override : Object

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is HeEdge<TPosition> edge ? this.Equals(edge) : false;
        }

        /// <inheritdoc cref="object.GetHashCode()"/>
        public override int GetHashCode()
        {
            int hashCode = 2018062386;
            hashCode = hashCode * -1521134295 + Index.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<HeVertex<TPosition>>.Default.GetHashCode(StartVertex);
            hashCode = hashCode * -1521134295 + EqualityComparer<HeVertex<TPosition>>.Default.GetHashCode(EndVertex);
            return hashCode;
        }

        /// <inheritdoc cref="object.ToString()"/>
        public override string ToString()
        {
            return $"HeEdge {Index} from vertex {StartVertex.Index} to {EndVertex.Index}.";
        }

        #endregion

        #region Override : Edge<T,HeVertex<T>,HeEdge<T>,HeFace<T>>

        /******************** For this Edges ********************/

        /// <inheritdoc/>
        public override bool IsBoundary()
        {
            return _halfedge.IsBoundary() && _halfedge.PairHalfedge.IsBoundary();
        }

        /// <inheritdoc/>
        internal override void Unset()
        {
            // Unset fields
            _halfedge = null;

            // Unset properties
            Index = -1;
            StartVertex = null;
            EndVertex = null;
        }


        /******************** On Faces ********************/

        /// <inheritdoc/>
        public override IReadOnlyList<HeFace<TPosition>> AdjacentFaces()
        {
            List<HeFace<TPosition>> result = new List<HeFace<TPosition>>();

            if (!(_halfedge.AdjacentFace is null)) { result.Add(_halfedge.AdjacentFace); }

            if (!(_halfedge.PairHalfedge.AdjacentFace is null)) { result.Add(_halfedge.PairHalfedge.AdjacentFace); }

            return result;

        }

        #endregion
    }
}
