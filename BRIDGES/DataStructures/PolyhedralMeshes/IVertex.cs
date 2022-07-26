using System;
using System.Collections.Generic;
using System.Text;

namespace BRIDGES.DataStructures.PolyhedralMeshes
{
    /// <summary>
    /// Abstract class for an vertex in a polyhedral mesh data structure.
    /// </summary>
    /// <typeparam name="TPosition"> Type of the vertex position. </typeparam>
    public interface IVertex<TPosition>
        where TPosition : IEquatable<TPosition>
    {
        #region Properties

        /// <summary>
        /// Gets the index of the current vertex in the mesh.
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Gets or sets the position of the current vertex.
        /// </summary>
        TPosition Position { get; set; }

        #endregion

        #region Methods

        /******************** For this Vertex ********************/

        /// <summary>
        /// Evaluates whether the vertex is on a boundary.
        /// </summary>
        /// <returns> <see langword="true"/> if the vertex is not connected or if at least one edge is a boundary edge, <see langword="false"/> otherwise. </returns>
        bool IsBoundary();

        /// <summary>
        /// Evaluates whether the vertex is connected to any edge. 
        /// </summary>
        /// <returns> <see langword="true"/> if the vertex has at least one connected edge, <see langword="false"/> otherwise. </returns>
        bool IsConnected();

        /// <summary>
        /// Determines the number of edges connected to the current vertex.
        /// </summary>
        /// <returns> The number of edges connected to the current vertex </returns>
        int Valence();


        /******************** On Vertices ********************/

        /// <summary>
        /// Identifies the vertices directly connected to the current vertex with an edge.
        /// </summary>
        /// <returns> The list of connected vertices. An empty list can be returned. </returns>
        IReadOnlyList<IVertex<TPosition>> NeighbourVertices();


        /******************** On Edges ********************/

        /// <summary>
        /// Identifies the edges connected to the current vertex. 
        /// </summary>
        /// <returns> The list of connected edges. An empty list can be returned. </returns>
        IReadOnlyList<IEdge<TPosition>> ConnectedEdges();


        /******************** On Faces ********************/

        /// <summary>
        /// Identifies the faces around the current vertex.
        /// </summary>
        /// <returns> The list of adjacent faces. An empty list can be returned. </returns>
        IReadOnlyList<IFace<TPosition>> AdjacentFaces();

        #endregion
    }
}