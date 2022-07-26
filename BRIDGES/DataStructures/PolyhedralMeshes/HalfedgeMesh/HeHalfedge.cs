using System;
using System.Collections.Generic;

using BRIDGES.DataStructures.PolyhedralMeshes.Abstract;


namespace BRIDGES.DataStructures.PolyhedralMeshes.HalfedgeMesh
{
    /// <summary>
    /// Class for a halfedge in a polyhedral halfedge mesh data structure.
    /// </summary>
    /// <typeparam name="TPosition"> Type for the position of the vertex. </typeparam>
    public class HeHalfedge<TPosition> : IEquatable<HeHalfedge<TPosition>>
        where TPosition : IEquatable<TPosition>
    {
        #region Fields

        /// <summary>
        /// Edge whose index is half the current halfedge's index (rounded towards zero).
        /// </summary>
        private HeEdge<TPosition> _edge;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the index of the current edge in the mesh.
        /// </summary>
        public int Index { get; internal set; }


        /// <summary>
        /// Gets the start vertex of the current edge.
        /// </summary>
        public HeVertex<TPosition> StartVertex { get; private protected set; }

        /// <summary>
        /// Gets the end vertex of the current edge.
        /// </summary>
        public HeVertex<TPosition> EndVertex { get; private protected set; }


        /// <summary>
        /// Gets the previous halfedge of the current halfedge.
        /// </summary>
        public HeHalfedge<TPosition> PrevHalfedge { get; internal set; }

        /// <summary>
        /// Gets the next halfedge of the current halfedge.
        /// </summary>
        public HeHalfedge<TPosition> NextHalfedge { get; internal set; }

        /// <summary>
        /// Gets the pair halfedge of the current halfedge.
        /// </summary>
        public HeHalfedge<TPosition> PairHalfedge { get; internal set; }


        /// <summary>
        /// Gets the adjacent face of the current halfedge.
        /// </summary>
        public HeFace<TPosition> AdjacentFace { get; internal set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HeHalfedge{TPosition}"/> class.
        /// </summary>
        /// <param name="index"> Index of the added halfedge in the mesh. </param>
        /// <param name="startVertex"> Start vertex of the halfedge. </param>
        /// <param name="endVertex"> End vertex of the halfedge. </param>
        internal HeHalfedge(int index, HeVertex<TPosition> startVertex, HeVertex<TPosition> endVertex)
        {
            // Instanciate properties
            PrevHalfedge = null;
            NextHalfedge = null;
            PairHalfedge = null;

            AdjacentFace = null;

            // Initialise properties
            Index = index;

            StartVertex = startVertex;
            EndVertex = endVertex;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="HeHalfedge{TPosition}"/> class.
        /// </summary>
        /// <param name="index"> Index of the added halfedge in the mesh. </param>
        /// <param name="startVertex"> Start vertex of the halfedge. </param>
        /// <param name="endVertex"> End vertex of the halfedge. </param>
        /// <param name="adjacentFace"> Adjacent face of the halfedge. </param>
        internal HeHalfedge(int index, HeVertex<TPosition> startVertex, HeVertex<TPosition> endVertex, HeFace<TPosition> adjacentFace)
        {
            // Instanciate properties
            PrevHalfedge = null;
            NextHalfedge = null;
            PairHalfedge = null;

            // Initialise properties
            Index = index;

            StartVertex = startVertex;
            EndVertex = endVertex;

            AdjacentFace = adjacentFace;
        }

        #endregion

        #region Methods

        /******************** For this Halfedge ********************/

        /// <summary>
        /// Evaluates whether the halfedge is on a boundary.
        /// </summary>
        /// <returns> <see langword="true"/> if the halfedge is on a boundary, <see langword="false"/> otherwise.</returns>
        public bool IsBoundary()
        {
            return AdjacentFace is null;
        }


        /// <summary>
        /// Unsets all the fields of the current halfedge.
        /// </summary>
        internal void Unset()
        {
            // Unset fields
            if (!(_edge is null))
            {
                _edge.Unset();
                _edge = null;
            }

            // Unset properties
            Index = -1;

            StartVertex = null;
            EndVertex = null;

            PrevHalfedge = null;
            NextHalfedge = null;
            PairHalfedge = null;

            AdjacentFace = null;
        }


        /// <inheritdoc/>
        public bool Equals(HeHalfedge<TPosition> halfedge)
        {
            return Index == halfedge.Index
                && StartVertex == halfedge.StartVertex
                && EndVertex == halfedge.EndVertex;
        }


        /******************** On Edges ********************/

        /// <summary>
        /// Identifies the edge representing the current halfedge and its pair.
        /// It is the edge whose index is half the current halfedge's index (rounded towards zero).
        /// </summary>
        /// <returns> The edge representing the current halfedge. </returns>
        public HeEdge<TPosition> GetEdge()
        {
            // If the corresponding edge was alredday created.
            if (!(_edge is null)) { return _edge; }

            // Otherwise, create the corresponding edge.
            int edgeIndex = Index / 2;
            int parity = Index - (2 * edgeIndex);

            // If the current halfedge index is pair.
            if (parity == 0) { _edge = new HeEdge<TPosition>(this); }
            // If the current halfedge index is odd.
            else { _edge = new HeEdge<TPosition>(PairHalfedge); }

            return _edge;
        }

        #endregion


        #region Override : Object

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is HeHalfedge<TPosition> halfedge && Equals(halfedge);
        }

        /// <inheritdoc cref="object.GetHashCode()"/>
        public override int GetHashCode()
        {
            int hashCode = 1631021372;
            hashCode = hashCode * -1521134295 + Index.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<HeVertex<TPosition>>.Default.GetHashCode(StartVertex);
            hashCode = hashCode * -1521134295 + EqualityComparer<HeVertex<TPosition>>.Default.GetHashCode(EndVertex);
            return hashCode;
        }

        /// <inheritdoc cref="object.ToString()"/>
        public override string ToString()
        {
            return $"HeHalfedge {Index} from vertex {StartVertex.Index} to {EndVertex.Index}.";
        }

        #endregion
    }
}
