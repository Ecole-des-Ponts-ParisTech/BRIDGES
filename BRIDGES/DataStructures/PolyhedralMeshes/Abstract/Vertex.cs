using System;
using System.Collections.Generic;


namespace BRIDGES.DataStructures.PolyhedralMeshes.Abstract
{
    /// <summary>
    /// Abstract class for a vertex in a polyhedral mesh data structure.
    /// </summary>
    /// <typeparam name="TPosition"> Type for the position of the vertex. </typeparam>
    /// <typeparam name="TVertex"> Type of vertex for the mesh. </typeparam>
    /// <typeparam name="TEdge"> Type of vertex for the mesh. </typeparam>
    /// <typeparam name="TFace"> Type of vertex for the mesh. </typeparam>
    public abstract class Vertex<TPosition, TVertex, TEdge, TFace> : IVertex<TPosition>,
        IEquatable<TVertex>
        where TPosition : IEquatable<TPosition>
        where TVertex : Vertex<TPosition, TVertex, TEdge, TFace>
        where TEdge : Edge<TPosition, TVertex, TEdge, TFace>
        where TFace : Face<TPosition, TVertex, TEdge, TFace>
    {
        #region Properties

        /// <inheritdoc/>
        public int Index { get; internal set; }

        /// <inheritdoc/>
        public TPosition Position { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Vertex{TPosition, TVertex, TEdge, TFace}"/> class from its index and position.
        /// </summary>
        /// <param name="index"> Index of the new vertex in the mesh. </param>
        /// <param name="position"> Position of the vertex. </param>
        internal Vertex(int index, TPosition position)
        {
            // Initialise properties
            Index = index;
            Position = position;
        }

        #endregion

        #region Virtual Methods

        /******************** For this Vertex ********************/

        /// <inheritdoc/>
        public virtual bool Equals(TVertex vertex)
        {
            return Index == vertex.Index
                && Position.Equals(vertex.Position);
        }

        #endregion

        #region Abstract Methods

        /******************** For this Vertex ********************/

        /// <inheritdoc/>
        public abstract bool IsBoundary();

        /// <inheritdoc/>
        public abstract bool IsConnected();

        /// <inheritdoc/>
        public abstract int Valence();


        /// <summary>
        /// Unsets all the fields of the current vertex.
        /// </summary>
        internal abstract void Unset();


        /******************** On Vertices ********************/

        /// <summary>
        /// Identifies the vertices directly connected to the current vertex with an edge.
        /// </summary>
        /// <returns> The list of connected vertices. An empty list can be returned. </returns>
        public abstract IReadOnlyList<TVertex> NeighbourVertices();


        /******************** On Edges ********************/

        /// <summary>
        /// Identifies the edges connected to the current vertex. 
        /// </summary>
        /// <returns> The list of connected edges. An empty list can be returned. </returns>
        public abstract IReadOnlyList<TEdge> ConnectedEdges();


        /******************** On Faces ********************/

        /// <summary>
        /// Identifies the faces around the current vertex.
        /// </summary>
        /// <returns> The list of adjacent faces. An empty list can be returned. </returns>
        public abstract IReadOnlyList<TFace> AdjacentFaces();

        #endregion


        #region Override : Object

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is TVertex vertex && Equals(vertex);
        }

        /// <inheritdoc cref="object.GetHashCode()"/>
        public override int GetHashCode()
        {
            int hashCode = 1665140013;
            hashCode = hashCode * -1521134295 + Index.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<TPosition>.Default.GetHashCode(Position);
            return hashCode;
        }

        /// <inheritdoc cref="object.ToString()"/>
        public override string ToString()
        {
            return $"Vertex {Index} at {Position}.";
        }

        #endregion


        #region Explicit : IVertex<TPosition>

        /******************** Methods ********************/

        IReadOnlyList<IVertex<TPosition>> IVertex<TPosition>.NeighbourVertices()
        {
            return NeighbourVertices();
        }

        IReadOnlyList<IEdge<TPosition>> IVertex<TPosition>.ConnectedEdges()
        {
            return ConnectedEdges();
        }

        IReadOnlyList<IFace<TPosition>> IVertex<TPosition>.AdjacentFaces()
        {
            return AdjacentFaces();
        }

        #endregion
    }
}
