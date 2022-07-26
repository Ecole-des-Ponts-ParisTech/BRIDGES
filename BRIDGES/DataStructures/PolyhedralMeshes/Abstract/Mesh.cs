using System;
using System.Collections.Generic;
using System.Text;


namespace BRIDGES.DataStructures.PolyhedralMeshes.Abstract
{
    /// <summary>
    /// Abstract class for a polyhedral mesh data structure.
    /// </summary>
    /// <typeparam name="TPosition"> Type for the position of the vertex.</typeparam>
    /// <typeparam name="TVertex"> Type of vertex for the mesh. </typeparam>
    /// <typeparam name="TEdge"> Type of vertex for the mesh. </typeparam>
    /// <typeparam name="TFace"> Type of vertex for the mesh.</typeparam>
    public abstract class Mesh<TPosition, TVertex, TEdge, TFace> : IMesh<TPosition>,
        ICloneable
        where TPosition : IEquatable<TPosition>
        where TVertex : Vertex<TPosition, TVertex, TEdge, TFace>
        where TEdge : Edge<TPosition, TVertex, TEdge, TFace>
        where TFace : Face<TPosition, TVertex, TEdge, TFace>
    {
        #region Abstract Properties

        /// <summary>
        /// Gets the number of vertices in the current mesh.
        /// </summary>
        public abstract int VertexCount { get; }

        /// <summary>
        /// Gets the number of edges in the current mesh.
        /// </summary>
        public abstract int EdgeCount { get; }

        /// <summary>
        /// Gets the number of faces in the current mesh.
        /// </summary>
        public abstract int FaceCount { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Mesh{TPosition, TVertex, TEdge, TFace}"/> class.
        /// </summary>
        protected Mesh()
        {

        }

        #endregion

        #region Methods

        /******************** On Vertices ********************/

        /// <summary>
        /// Removes the vertex at the given index in the mesh by keeping the mesh manifold.
        /// </summary>
        /// <param name="index"> Index of the vertex to remove. </param>
        public void RemoveVertex(int index)
        {
            TVertex vertex = GetVertex(index);

            RemoveVertex(vertex);
        }


        /// <summary>
        /// Erases the vertex at the given index in the mesh. Every reference to this vertex should be deleted before it is erased.
        /// </summary>
        /// <param name="index"> Index of the vertex to erase. </param>
        public void EraseVertex(int index)
        {
            TVertex vertex = GetVertex(index);

            EraseVertex(vertex);
        }

        /******************** On Edges ********************/

        /// <summary>
        /// Adds a new edge to the current mesh.
        /// </summary>
        /// <param name="startIndex"> Index of the start vertex for the new edge. </param>
        /// <param name="endIndex"> Index of the end vertex for the new edge. </param>
        /// <returns> The new edge if it was added, <see langword="null"/> otherwise. </returns>
        protected TEdge AddEdge(int startIndex, int endIndex)
        {
            TVertex startVertex = GetVertex(startIndex);
            TVertex endVertex = GetVertex(endIndex);

            return AddEdge(startVertex, endVertex);
        }

        /// <summary>
        /// Looks for the edge between two vertices.
        /// </summary>
        /// <param name="indexA"> Index of the first end vertex of the edge. </param>
        /// <param name="indexB"> Index of the second end vertex of the edge. </param>
        /// <returns> The edge if it exists, <see langword="null"/> otherwise.</returns>
        public TEdge EdgeBetween(int indexA, int indexB)
        {
            TVertex vertexA = GetVertex(indexA);
            TVertex vertexB = GetVertex(indexB);

            return EdgeBetween(vertexA, vertexB);
        }

        /// <summary>
        /// Removes the edge at the given index in the mesh by keeping the mesh manifold.
        /// </summary>
        /// <param name="index"> Index of the edge to remove. </param>
        public void RemoveEdge(int index)
        {
            TEdge edge = GetEdge(index);

            RemoveEdge(edge);
        }


        /// <summary>
        /// Erases the edge at the given index in the mesh. The mesh may be non-manifold after this operation.
        /// </summary>
        /// <param name="index"> Index of the edge to erase. </param>
        /// <exception cref="KeyNotFoundException"> The given edge index doesn't exist in the mesh. </exception>
        public void EraseEdge(int index)
        {
            TEdge edge = GetEdge(index);

            EraseEdge(edge);
        }


        /******************** On Faces ********************/

        /// <summary>
        /// Adds a new triangular face to the current mesh.
        /// </summary>
        /// <param name="indexA"> Index of the first vertex for the new face. </param>
        /// <param name="indexB"> Index of the second vertex for the new face. </param>
        /// <param name="indexC"> Index of the third vertex for the new face. </param>
        /// <returns> The newly created triangular face. </returns>
        public TFace AddFace(int indexA, int indexB, int indexC)
        {
            TVertex vertexA = GetVertex(indexA);
            TVertex vertexB = GetVertex(indexB);
            TVertex vertexC = GetVertex(indexC);

            return AddFace(new List<TVertex> { vertexA, vertexB, vertexC});
        }

        /// <summary>
        /// Adds a new quadrangular face to the current mesh.
        /// </summary>
        /// <param name="indexA"> Index of the first vertex for the new face. </param>
        /// <param name="indexB"> Index of the second vertex for the new face. </param>
        /// <param name="indexC"> Index of the third vertex for the new face. </param>
        /// <param name="indexD"> Index of the fourth vertex for the new face. </param>
        /// <returns> The newly created quadrangular face. </returns>
        public TFace AddFace(int indexA, int indexB, int indexC, int indexD)
        {
            TVertex vertexA = GetVertex(indexA);
            TVertex vertexB = GetVertex(indexB);
            TVertex vertexC = GetVertex(indexC);
            TVertex vertexD = GetVertex(indexD);

            return AddFace(new List<TVertex> { vertexA, vertexB, vertexC, vertexD });
        }

        /// <summary>
        /// Adds a new face to the current mesh.
        /// </summary>
        /// <param name="indices"> Ordered list of the face's vertex indices. </param>
        /// <returns> The newly created face. </returns>
        public TFace AddFace(List<int> indices)
        {
            List<TVertex> vertices = new List<TVertex>();
            for (int i = 0; i < indices.Count; i++)
            {
                TVertex vertex = GetVertex(indices[i]);
                vertices.Add(vertex);
            }

            return AddFace(vertices);
        }

        /// <summary>
        /// Adds a new triangular face to the current mesh.
        /// </summary>
        /// <param name="vertexA"> First vertex for the new face. </param>
        /// <param name="vertexB"> Second vertex for the new face. </param>
        /// <param name="vertexC"> Third vertex for the new face. </param>
        /// <returns> The newly created triangular face. </returns>
        public TFace AddFace(TVertex vertexA, TVertex vertexB, TVertex vertexC)
        {
            return AddFace(new List<TVertex> { vertexA, vertexB, vertexC });
        }

        /// <summary>
        /// Adds a new quadrangular face to the current mesh.
        /// </summary>
        /// <param name="vertexA"> First vertex for the new face. </param>
        /// <param name="vertexB"> Second vertex for the new face. </param>
        /// <param name="vertexC"> Third vertex for the new face. </param>
        /// <param name="vertexD"> Fourth vertex for the new face. </param>
        /// <returns> The newly created quadrangular face. </returns>
        public TFace AddFace(TVertex vertexA, TVertex vertexB, TVertex vertexC, TVertex vertexD)
        {
            return AddFace(new List<TVertex> { vertexA, vertexB, vertexC, vertexD });
        }


        /// <summary>
        /// Removes the face at the given index in the mesh by keeping the mesh manifold.
        /// </summary>
        /// <param name="index"> Index of the face to remove. </param>
        /// <exception cref="KeyNotFoundException"> The given face index doesn't exist in the mesh. </exception>
        public void RemoveFace(int index)
        {
            TFace face = GetFace(index);

            RemoveFace(face);
        }


        /// <summary>
        /// Erases the face at the given index in the mesh. The mesh may be non-manifold after this operation.
        /// </summary>
        /// <param name="index"> Index of the face to erase. </param>
        /// <exception cref="KeyNotFoundException"> The given face index doesn't exist in the mesh. </exception>
        public void EraseFace(int index)
        {
            TFace face = GetFace(index);

            EraseFace(face);
        }

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Looks for the edge between <paramref name="vertexA"/> and <paramref name="vertexB"/>.
        /// </summary>
        /// <param name="vertexA"> First end vertex of the edge. </param>
        /// <param name="vertexB"> Second end vertex of the edge. </param>
        /// <returns> The edge if it exists, <see langword="null"/> otherwise.</returns>
        public virtual TEdge EdgeBetween(TVertex vertexA, TVertex vertexB)
        {
            IReadOnlyList<TEdge> connectedEdges = vertexA.ConnectedEdges();
            for (int i_CE = 0; i_CE < connectedEdges.Count; i_CE++)
            {
                TEdge connectedEdge = connectedEdges[i_CE];

                if (connectedEdge.StartVertex.Equals(vertexB)) { return connectedEdge; }
                else if (connectedEdge.EndVertex.Equals(vertexB)) { return connectedEdge; }
            }

            return null;
        }

        #endregion

        #region Abstract Methods

        /******************** For this Mesh ********************/

        /// <summary>
        /// Cleans the current mesh by reindexing the faces, edges and vertices.
        /// </summary>
        /// <param name="cullIsolated"> Evaluates whether isolated members should be removed in the process.</param>
        public abstract void CleanMesh(bool cullIsolated = true);

        /// <summary>
        ///  Creates a new object that is a deep copy of the current instance.
        /// </summary>
        /// <returns> The new object that is a deep copy of the current instance. </returns>
        public abstract object Clone();

        /******************** On Vertices ********************/

        /// <summary>
        /// Adds a vertex in the mesh from its position.
        /// </summary>
        /// <param name="position"> Position of the vertex. </param>
        /// <returns> The new vertex if it was added, <see langword="null"/> otherwise. </returns>
        public abstract TVertex AddVertex(TPosition position);


        /// <summary>
        /// Returns the vertex at the given index in the mesh.
        /// </summary>
        /// <returns> The vertex at the given index in the mesh. </returns>
        public abstract TVertex GetVertex(int index);

        /// <summary>
        /// Returns the vertex at the given index in the mesh if it exists, <see langword="null"/> otherwise. 
        /// </summary>
        /// <param name="index"> The index of the vertex to look for. </param>
        /// <returns> The vertex if it exists, <see langword="null"/> otherwise. </returns>
        public abstract TVertex TryGetVertex(int index);

        /// <summary>
        /// Returns the list of vertices of the current mesh.
        /// </summary>
        /// <remarks> 
        /// If some vertices were removed from the mesh, the index of the vertex in the returned list might not match the vertex index in the mesh. <br/>
        /// The index of the vertices in the mesh is accessible through the Index property.
        /// </remarks>
        /// <returns> List of vertices of the mesh. </returns>
        public abstract IReadOnlyList<TVertex> GetVertices();


        /// <summary>
        /// Removes the vertex from the mesh by keeping the mesh manifold.
        /// </summary>
        /// <param name="vertex"> Vertex to remove. </param>
        public abstract void RemoveVertex(TVertex vertex);


        /// <summary>
        /// Erases the vertex from the mesh. Every reference to this vertex should be deleted before it is erased.
        /// </summary>
        /// <param name="vertex"> The vertex to erase. </param>
        protected abstract void EraseVertex(TVertex vertex);


        /******************** On Edges ********************/

        /// <summary>
        /// Adds a new edge to the current mesh from its end vertices.
        /// </summary>
        /// <remarks> This method does not manage the connectivity within the mesh. </remarks>
        /// <param name="startVertex"> Start vertex for the new edge. </param>
        /// <param name="endVertex"> End vertex for the new edge. </param>
        /// <returns> The new edge if it was added, <see langword="null"/> otherwise. </returns>
        internal abstract TEdge AddEdge(TVertex startVertex, TVertex endVertex);


        /// <summary>
        /// Returns the edge at the given index in the mesh.
        /// </summary>
        /// <returns> The edge at the given index in the mesh. </returns>
        public abstract TEdge GetEdge(int index);

        /// <summary>
        /// Returns the edge at the given index in the mesh if it exists, <see langword="null"/> otherwise. 
        /// </summary>
        /// <param name="index"> The index of the edge to look for. </param>
        /// <returns> The edge if it exists, <see langword="null"/> otherwise. </returns>
        public abstract TEdge TryGetEdge(int index);

        /// <summary>
        /// Returns the list of edges of the current mesh.
        /// </summary>
        /// <remarks> 
        /// If some edges were removed from the mesh, the index of the edge in the returned list might not match the edge index in the mesh. <br/>
        /// The index of the edges in the mesh is accessible through the Index property.
        /// </remarks>
        /// <returns> List of edges of the mesh. </returns>
        public abstract IReadOnlyList<TEdge> GetEdges();


        /// <summary>
        /// Removes the edge from the mesh by keeping the mesh manifold.
        /// </summary>
        /// <param name="edge"> Edge to remove. </param>
        public abstract void RemoveEdge(TEdge edge);


        /// <summary>
        /// Erases the edge from the mesh. The mesh may not be manifold after this operation.
        /// </summary>
        /// <param name="edge"> Edge to erase. </param>
        public abstract void EraseEdge(TEdge edge);


        /******************** On Faces ********************/

        /// <summary>
        /// Adds a new face to the current mesh from its vertices.
        /// </summary>
        /// <param name="vertices"> Ordered list of the face's vertex. </param>
        /// <returns> The new face if it was added, <see langword="null"/> otherwise. </returns>
        public abstract TFace AddFace(List<TVertex> vertices);


        /// <summary>
        /// Returns the face at the given index in the mesh.
        /// </summary>
        /// <returns> The face at the given index in the mesh. </returns>
        /// <exception cref="KeyNotFoundException"> The given face index doesn't exist in the mesh. </exception>
        public abstract TFace GetFace(int index);

        /// <summary>
        /// Returns the face at the given index in the mesh if it exists, <see langword="null"/> otherwise. 
        /// </summary>
        /// <param name="index"> The index of the face to look for. </param>
        /// <returns> The face if it exists, <see langword="null"/> otherwise. </returns>
        public abstract TFace TryGetFace(int index);

        /// <summary>
        /// Returns the list of faces of the current mesh.
        /// </summary>
        /// <remarks> 
        /// If some faces were removed from the mesh, the index of the face in the returned list might not match the face index in the mesh. <br/>
        /// The index of the faces in the mesh is accessible through the Index property.
        /// </remarks>
        /// <returns> List of faces of the mesh. </returns>
        public abstract IReadOnlyList<TFace> GetFaces();


        /// <summary>
        /// Removes the face from the mesh by keeping the mesh manifold.
        /// </summary>
        /// <param name="face"> Face to remove. </param>
        public abstract void RemoveFace(TFace face);


        /// <summary>
        /// Erases the face from the mesh. The mesh may not be manifold after this operation.
        /// </summary>
        /// <param name="face"> Face to erase. </param>
        public abstract void EraseFace(TFace face);

        #endregion


        #region Override : Object

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"Mesh with {VertexCount} vertices, {EdgeCount} edges, {FaceCount} faces.";
        }

        #endregion


        #region Explicit : IMesh<TPosition>

        /******************** Methods ********************/

        IVertex<TPosition> IMesh<TPosition>.AddVertex(TPosition position)
        {
            return AddVertex(position);
        }

        IVertex<TPosition> IMesh<TPosition>.GetVertex(int index)
        {
            return GetVertex(index);
        }

        IVertex<TPosition> IMesh<TPosition>.TryGetVertex(int index)
        {
            return TryGetVertex(index);
        }

        IReadOnlyList<IVertex<TPosition>> IMesh<TPosition>.GetVertices()
        {
            return GetVertices();
        }


        IEdge<TPosition> IMesh<TPosition>.AddEdge(int startIndex, int endIndex)
        {
            return AddEdge(startIndex, endIndex);
        }

        IEdge<TPosition> IMesh<TPosition>.GetEdge(int index)
        {
            return GetEdge(index);
        }

        IEdge<TPosition> IMesh<TPosition>.TryGetEdge(int index)
        {
            return TryGetEdge(index);
        }

        IReadOnlyList<IEdge<TPosition>> IMesh<TPosition>.GetEdges()
        {
            return GetEdges();
        }

        IEdge<TPosition> IMesh<TPosition>.EdgeBetween(int indexA, int indexB)
        {
            return EdgeBetween(indexA, indexB);
        }


        IFace<TPosition> IMesh<TPosition>.AddFace(int indexA, int indexB, int indexC)
        {
            return AddFace(indexA, indexB, indexC);
        }

        IFace<TPosition> IMesh<TPosition>.AddFace(int indexA, int indexB, int indexC, int indexD)
        {
            return AddFace(indexA, indexB, indexC, indexD);
        }

        IFace<TPosition> IMesh<TPosition>.AddFace(List<int> indices)
        {
            return AddFace(indices);
        }

        IFace<TPosition> IMesh<TPosition>.GetFace(int index)
        {
            return GetFace(index);
        }

        IFace<TPosition> IMesh<TPosition>.TryGetFace(int index)
        {
            return TryGetFace(index);
        }

        IReadOnlyList<IFace<TPosition>> IMesh<TPosition>.GetFaces()
        {
            return GetFaces();
        }

        #endregion
    }
}
