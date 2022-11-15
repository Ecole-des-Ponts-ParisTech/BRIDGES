using System;
using System.Collections.Generic;


namespace BRIDGES.DataStructures.PolyhedralMeshes
{
    /// <summary>
    /// Abstract class for a polyhedral mesh data structure.
    /// </summary>
    /// <typeparam name="TPosition"> Type of the vertex position. </typeparam>
    public interface IMesh<TPosition>
        where TPosition : IEquatable<TPosition>
    {
        #region Properties

        /// <summary>
        /// Gets the number of vertices in the current mesh.
        /// </summary>
        int VertexCount { get; }

        /// <summary>
        /// Gets the number of edges in the current mesh.
        /// </summary>
        int EdgeCount { get; }

        /// <summary>
        /// Gets the number of faces in the current mesh.
        /// </summary>
        int FaceCount { get; }

        #endregion

        #region Methods

        /******************** On Vertices ********************/

        /// <summary>
        /// Adds a vertex in the mesh.
        /// </summary>
        /// <param name="position"> Position of the vertex. </param>
        /// <returns> The new vertex if it was added, <see langword="null"/> otherwise. </returns>
        IVertex<TPosition> AddVertex(TPosition position);


        /// <summary>
        /// Returns the vertex at the given index in the mesh.
        /// </summary>
        /// <returns> The vertex at the given index in the mesh. </returns>
        IVertex<TPosition> GetVertex(int index);

        /// <summary>
        /// Returns the vertex at the given index in the mesh if it exists, <see langword="null"/> otherwise. 
        /// </summary>
        /// <param name="index"> The index of the vertex to look for. </param>
        /// <returns> The vertex if it exists, <see langword="null"/> otherwise. </returns>
        IVertex<TPosition> TryGetVertex(int index);

        /// <summary>
        /// Returns the list of vertices of the current mesh.
        /// </summary>
        /// <remarks> 
        /// If some vertices were removed from the mesh, the index of the vertex in the returned list might not match the vertex index in the mesh. <br/>
        /// The index of the vertices in the mesh is accessible through <see cref="IVertex{T}.Index"/>.
        /// </remarks>
        /// <returns> List of vertices of the mesh. </returns>
        IReadOnlyList<IVertex<TPosition>> GetVertices();


        /// <summary>
        /// Removes the vertex at the given index in the mesh by keeping the mesh manifold.
        /// </summary>
        /// <param name="index"> Index of the vertex to remove. </param>
        void RemoveVertex(int index);


        /// <summary>
        /// Erases the vertex at the given index in the mesh. Every reference to this vertex should be deleted before it is erased.
        /// </summary>
        /// <param name="index"> Index of the vertex to erase. </param>
        void EraseVertex(int index);


        /******************** On Edges ********************/

        /// <summary>
        /// Adds a new edge to the current mesh.
        /// </summary>
        /// <param name="startIndex"> Index of the start vertex for the new edge. </param>
        /// <param name="endIndex"> Index of the end vertex for the new edge. </param>
        /// <returns> The new edge if it was added, <see langword="null"/> otherwise. </returns>
        IEdge<TPosition> AddEdge(int startIndex, int endIndex);


        /// <summary>
        /// Returns the edge at the given index in the mesh.
        /// </summary>
        /// <returns> The edge at the given index in the mesh. </returns>
        IEdge<TPosition> GetEdge(int index);

        /// <summary>
        /// Returns the edge at the given index in the mesh if it exists, <see langword="null"/> otherwise. 
        /// </summary>
        /// <param name="index"> The index of the edge to look for. </param>
        /// <returns> The edge if it exists, <see langword="null"/> otherwise. </returns>
        IEdge<TPosition> TryGetEdge(int index);

        /// <summary>
        /// Returns the list of edges of the current mesh.
        /// </summary>
        /// <remarks> 
        /// If some edges were removed from the mesh, the index of the edge in the returned list might not match the edge index in the mesh. <br/>
        /// The index of the edges in the mesh is accessible through <see cref="IEdge{T}.Index"/>.
        /// </remarks>
        /// <returns> List of edges of the mesh. </returns>
        IReadOnlyList<IEdge<TPosition>> GetEdges();

        /// <summary>
        /// Looks for the edge between two vertices.
        /// </summary>
        /// <param name="indexA"> Index of the first end vertex of the edge. </param>
        /// <param name="indexB"> Index of the second end vertex of the edge. </param>
        /// <returns> The edge if it exists, <see langword="null"/> otherwise.</returns>
        IEdge<TPosition> EdgeBetween(int indexA, int indexB);


        /// <summary>
        /// Removes the edge at the given index in the mesh by keeping the mesh manifold.
        /// </summary>
        /// <param name="index"> Index of the edge to remove. </param>
        void RemoveEdge(int index);


        /// <summary>
        /// Erases the edge at the given index in the mesh. The mesh may be non-manifold after this operation.
        /// </summary>
        /// <param name="index"> Index of the edge to erase. </param>
        /// <exception cref="KeyNotFoundException"> The given edge index doesn't exist in the mesh. </exception>
        void EraseEdge(int index);


        /******************** On Faces ********************/

        /// <summary>
        /// Adds a new triangular face to the current mesh.
        /// </summary>
        /// <param name="indexA"> Index of the first vertex for the new face. </param>
        /// <param name="indexB"> Index of the second vertex for the new face. </param>
        /// <param name="indexC"> Index of the third vertex for the new face. </param>
        /// <returns> The newly created triangular face. </returns>
        IFace<TPosition> AddFace(int indexA, int indexB, int indexC);

        /// <summary>
        /// Adds a new quadrangular face to the current mesh.
        /// </summary>
        /// <param name="indexA"> Index of the first vertex for the new face. </param>
        /// <param name="indexB"> Index of the second vertex for the new face. </param>
        /// <param name="indexC"> Index of the third vertex for the new face. </param>
        /// <param name="indexD"> Index of the fourth vertex for the new face. </param>
        /// <returns> The newly created quadrangular face. </returns>
        IFace<TPosition> AddFace(int indexA, int indexB, int indexC, int indexD);

        /// <summary>
        /// Adds a new face to the current mesh.
        /// </summary>
        /// <param name="indices"> Ordered list of the face's vertex indices. </param>
        /// <returns> The newly created face. </returns>
        IFace<TPosition> AddFace(List<int> indices);


        /// <summary>
        /// Returns the face at the given index in the mesh.
        /// </summary>
        /// <returns> The face at the given index in the mesh. </returns>
        /// <exception cref="KeyNotFoundException"> The given face index doesn't exist in the mesh. </exception>
        IFace<TPosition> GetFace(int index);

        /// <summary>
        /// Returns the face at the given index in the mesh if it exists, <see langword="null"/> otherwise. 
        /// </summary>
        /// <param name="index"> The index of the face to look for. </param>
        /// <returns> The face if it exists, <see langword="null"/> otherwise. </returns>
        IFace<TPosition> TryGetFace(int index);

        /// <summary>
        /// Returns the list of faces of the current mesh.
        /// </summary>
        /// <remarks> 
        /// If some faces were removed from the mesh, the index of the face in the returned list might not match the face index in the mesh. <br/>
        /// The index of the faces in the mesh is accessible through <see cref="IFace{T}.Index"/>.
        /// </remarks>
        /// <returns> List of faces of the mesh. </returns>
        IReadOnlyList<IFace<TPosition>> GetFaces();


        /// <summary>
        /// Removes the face at the given index in the mesh by keeping the mesh manifold.
        /// </summary>
        /// <param name="index"> Index of the face to remove. </param>
        /// <exception cref="KeyNotFoundException"> The given face index doesn't exist in the mesh. </exception>
        void RemoveFace(int index);


        /// <summary>
        /// Erases the face at the given index in the mesh. The mesh may be non-manifold after this operation.
        /// </summary>
        /// <param name="index"> Index of the face to erase. </param>
        /// <exception cref="KeyNotFoundException"> The given face index doesn't exist in the mesh. </exception>
        void EraseFace(int index);


        /******************** On this Mesh ********************/

        /// <summary>
        /// Cleans the current mesh by reindexing the faces, edges and vertices.
        /// </summary>
        /// <param name="cullIsolated"> Evaluates whether isolated members should be removed in the process.</param>
        void CleanMesh(bool cullIsolated = true);

        #endregion
    }
}
