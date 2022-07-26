using System;
using System.Collections.Generic;


namespace BRIDGES.DataStructures.PolyhedralMeshes
{
    /// <summary>
    /// Abstract class for an edge in a polyhedral mesh data structure.
    /// </summary>
    /// <typeparam name="TPosition"> Type of the vertex position. </typeparam>
    public interface IEdge<TPosition>
        where TPosition : IEquatable<TPosition>
    {
        #region Properties

        /// <summary>
        /// Gets the index of the current edge in the mesh.
        /// </summary>
        int Index { get; }


        /// <summary>
        /// Gets the start vertex of the current edge.
        /// </summary>
        IVertex<TPosition> StartVertex { get; }

        /// <summary>
        /// Gets the end vertex of the current edge.
        /// </summary>
        IVertex<TPosition> EndVertex { get; }

        #endregion

        #region Methods

        /******************** For this Edge ********************/

        /// <summary>
        /// Evaluates whether the vertex is on a boundary.
        /// </summary>
        /// <returns> <see langword="true"/> if the edge is on a boundary, <see langword="false"/> otherwise.</returns>
        bool IsBoundary();

        /// <summary>
        /// Identifies the faces around the current edge.
        /// </summary>
        /// <returns> The list of adjacent faces. An empty list can be returned. </returns>
        IReadOnlyList<IFace<TPosition>> AdjacentFaces();

        #endregion
    }
}
