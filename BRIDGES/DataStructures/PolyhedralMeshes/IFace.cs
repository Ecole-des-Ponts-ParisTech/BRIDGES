using System;
using System.Collections.Generic;


namespace BRIDGES.DataStructures.PolyhedralMeshes
{
    /// <summary>
    /// Abstract class for an face in a polyhedral mesh data structure.
    /// </summary>
    /// <typeparam name="TPosition"> Type of the vertex position. </typeparam>
    public interface IFace<TPosition>
        where TPosition : IEquatable<TPosition>
    {
        #region Properties

        /// <summary>
        /// Gets the index of the current face in the mesh.
        /// </summary>
        int Index { get; }

        #endregion

        #region Methods

        /******************** On this Face ********************/

        /// <summary>
        /// Identifies the vertices around the current face.
        /// </summary>
        /// <returns> The ordered list of face vertices. </returns>
        IReadOnlyList<IVertex<TPosition>> FaceVertices();

        /// <summary>
        /// Identifies the edges around the current face.
        /// </summary>
        /// <returns> The ordered list of face edges. </returns>
        IReadOnlyList<IEdge<TPosition>> FaceEdges();

        /// <summary>
        /// Identifies the list of faces around the current face.
        /// </summary>
        /// <returns> The ordered list of faces. An empty list can be returned. </returns>
        IReadOnlyList<IFace<TPosition>> AdjacentFaces();

        #endregion
    }
}
