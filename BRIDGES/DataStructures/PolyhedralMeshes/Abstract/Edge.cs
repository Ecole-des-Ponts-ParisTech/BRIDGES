using System;
using System.Collections.Generic;

namespace BRIDGES.DataStructures.PolyhedralMeshes.Abstract
{
    /// <summary>
    /// Abstract class for an edge in a polyhedral mesh data structure.
    /// </summary>
    /// <typeparam name="TPosition"> Type for the position of the vertex.</typeparam>
    /// <typeparam name="TVertex"> Type of vertex for the mesh. </typeparam>
    /// <typeparam name="TEdge"> Type of vertex for the mesh. </typeparam>
    /// <typeparam name="TFace"> Type of vertex for the mesh.</typeparam>
    public abstract class Edge<TPosition, TVertex, TEdge, TFace> : IEdge<TPosition>,
        IEquatable<TEdge>
        where TPosition : IEquatable<TPosition>
        where TVertex : Vertex<TPosition, TVertex, TEdge, TFace>
        where TEdge : Edge<TPosition, TVertex, TEdge, TFace>
        where TFace : Face<TPosition, TVertex, TEdge, TFace>
    {
        #region Properties

        /// <inheritdoc/>
        public int Index { get; internal set; }


        /// <summary>
        /// Gets the start vertex of the current edge.
        /// </summary>
        public TVertex StartVertex { get; private protected set; }

        /// <summary>
        /// Gets the end vertex of the current edge.
        /// </summary>
        public TVertex EndVertex { get; private protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Edge{TPosition, TVertex, TEdge, TFace}"/> class from its index, and its start and end vertex.
        /// </summary>
        /// <param name="index"> Index of the new edge in the mesh. </param>
        /// <param name="startVertex"> Start vertex for the new edge. </param>
        /// <param name="endVertex"> End vertex for the new edge. </param>
        internal Edge(int index, TVertex startVertex, TVertex endVertex)
        {
            // Initialise properties
            Index = index;

            StartVertex = startVertex;
            EndVertex = endVertex;
        }

        #endregion

        #region Virtual Methods

        /******************** For this Edge ********************/

        /// <inheritdoc/>
        public virtual bool Equals(TEdge edge)
        {
            return Index == edge.Index
                && StartVertex.Equals(edge.StartVertex)
                && EndVertex.Equals(edge.EndVertex);
        }

        #endregion

        #region Abstract Methods

        /******************** For this Edge ********************/

        /// <summary>
        /// Evaluates whether the edge is on a boundary.
        /// </summary>
        /// <returns> <see langword="true"/> if the edge is on a boundary, <see langword="false"/> otherwise.</returns>
        public abstract bool IsBoundary();


        /// <summary>
        /// Identifies the faces around the current edge.
        /// </summary>
        /// <returns> The list of adjacent faces. An empty list can be returned. </returns>
        public abstract IReadOnlyList<TFace> AdjacentFaces();


        /// <summary>
        /// Unsets all the fields of the current edge.
        /// </summary>
        internal abstract void Unset();

        #endregion


        #region Override : Object

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is TEdge edge ? this.Equals(edge) : false;
        }

        /// <inheritdoc cref="object.GetHashCode()"/>
        public override int GetHashCode()
        {
            int hashCode = 1631021372;
            hashCode = hashCode * -1521134295 + Index.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<TVertex>.Default.GetHashCode(StartVertex);
            hashCode = hashCode * -1521134295 + EqualityComparer<TVertex>.Default.GetHashCode(EndVertex);
            return hashCode;
        }

        /// <inheritdoc cref="object.ToString()"/>
        public override string ToString()
        {
            return $"Edge {Index} from vertex {StartVertex.Index} to {EndVertex.Index}.";
        }

        #endregion


        #region Explicit : IEdge<TPosition>

        /******************** Properties ********************/

        IVertex<TPosition> IEdge<TPosition>.StartVertex
        {
            get { return StartVertex; }
        }

        IVertex<TPosition> IEdge<TPosition>.EndVertex
        {
            get { return EndVertex; }
        }


        /******************** Methods ********************/

        IReadOnlyList<IFace<TPosition>> IEdge<TPosition>.AdjacentFaces()
        {
            return AdjacentFaces();
        }

        #endregion
    }
}
