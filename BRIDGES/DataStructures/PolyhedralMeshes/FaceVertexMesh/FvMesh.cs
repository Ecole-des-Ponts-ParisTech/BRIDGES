using System;
using System.Collections.Generic;

using BRIDGES.DataStructures.PolyhedralMeshes.Abstract;


namespace BRIDGES.DataStructures.PolyhedralMeshes.FaceVertexMesh
{
    /// <summary>
    /// Class for a polyhedral face-vertex mesh data structure.
    /// </summary>
    /// <typeparam name="TPosition"> Type for the position of the vertex. </typeparam>
    public class FvMesh<TPosition> : Mesh<TPosition, FvVertex<TPosition>, FvEdge<TPosition>, FvFace<TPosition>>
        where TPosition : IEquatable<TPosition>
    {
        #region Fields

        /// <summary>
        /// Dictionary containing the <see cref="FvVertex{TPosition}"/> of the current <see cref="FvMesh{TPosition}"/>.
        /// </summary>
        /// <remarks> Key : Index of the <see cref="FvVertex{TPosition}"/>; Value : Corresponding <see cref="FvVertex{TPosition}"/>. </remarks>
        internal Dictionary<int, FvVertex<TPosition>> _vertices;

        /// <summary>
        /// Index for a newly created vertex.
        /// </summary>
        /// <remarks> This may not match with <see cref="VertexCount"/> if vertices are removed from the mesh. </remarks>
        protected int _newVertexIndex;


        /// <summary>
        /// Dictionary containing the <see cref="FvEdge{TPosition}"/> of the current <see cref="FvMesh{TPosition}"/>.
        /// </summary>
        /// <remarks> Key : Index of the <see cref="FvEdge{TPosition}"/>; Value : Corresponding <see cref="FvEdge{TPosition}"/>. </remarks>
        internal Dictionary<int, FvEdge<TPosition>> _edges;

        /// <summary>
        /// Index for a newly created edge.
        /// </summary>
        /// <remarks> This may not match with <see cref="EdgeCount"/> if edges are removed from the mesh. </remarks>
        protected int _newEdgeIndex;


        /// <summary>
        /// Dictionary containing the <see cref="FvFace{TPosition}"/> of the current <see cref="FvMesh{TPosition}"/>.
        /// </summary>
        /// <remarks> Key : Index of the <see cref="FvFace{TPosition}"/>; Value : Corresponding <see cref="FvFace{TPosition}"/>. </remarks>
        internal Dictionary<int, FvFace<TPosition>> _faces;

        /// <summary>
        /// Index for a newly created face.
        /// </summary>
        /// <remarks> This may not match with <see cref="FaceCount"/> if faces are removed from the mesh. </remarks>
        protected int _newFaceIndex;

        #endregion

        #region Properties

        /// <inheritdoc/>
        public override int VertexCount 
        { 
            get { return _vertices.Count; } 
        }

        /// <inheritdoc/>
        public override int EdgeCount 
        { 
            get { return _edges.Count; }
        }

        /// <inheritdoc/>
        public override int FaceCount 
        { 
            get { return _faces.Count; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="FvMesh{TPosition}"/> class.
        /// </summary>
        public FvMesh()
            : base()
        {
            // Instanciate fields
            _vertices = new Dictionary<int, FvVertex<TPosition>>();
            _edges = new Dictionary<int, FvEdge<TPosition>>();
            _faces = new Dictionary<int, FvFace<TPosition>>();

            // Initialise fields
            _newVertexIndex = 0;
            _newEdgeIndex = 0;
            _newFaceIndex = 0;
        }
        
        /// <summary>
        /// Initialises a new instance of the <see cref="FvMesh{TPosition}"/> class from its fields.
        /// </summary>
        internal FvMesh(Dictionary<int, FvVertex<TPosition>> vertices, Dictionary<int, FvEdge<TPosition>> edges, Dictionary<int, FvFace<TPosition>> faces,
            int newVertexIndex, int newEdgeIndex, int newFaceIndex)
        {
            // Instanciate fields
            _vertices = vertices;
            _edges = edges;
            _faces = faces;

            // Initialise fields
            _newVertexIndex = newVertexIndex;
            _newEdgeIndex = newEdgeIndex;
            _newFaceIndex = newFaceIndex;
        }

        #endregion

        #region Methods

        /******************** On Meshes ********************/

        /// <summary>
        /// Creates a halfedge mesh from the current face-vertex mesh.
        /// </summary>
        /// <returns> Halfedge mesh which represents the topology and geometry of the current face-vertex mesh. </returns>
        public HalfedgeMesh.HeMesh<TPosition> ToHalfedgeMesh()
        {
            Dictionary<int, HalfedgeMesh.HeVertex<TPosition>> heVertices = new Dictionary<int, HalfedgeMesh.HeVertex<TPosition>>();
            Dictionary<int, HalfedgeMesh.HeHalfedge<TPosition>> heHalfedges = new Dictionary<int, HalfedgeMesh.HeHalfedge<TPosition>>();
            Dictionary<int, HalfedgeMesh.HeFace<TPosition>> heFaces = new Dictionary<int, HalfedgeMesh.HeFace<TPosition>>();


            // Add Vertices
            var vertexIndexEnumerator = _vertices.Keys.GetEnumerator();

            while (vertexIndexEnumerator.MoveNext())
            {
                int vertexIndex = vertexIndexEnumerator.Current;

                FvVertex<TPosition> vertex = GetVertex(vertexIndex);

                HalfedgeMesh.HeVertex<TPosition> heVertex = new HalfedgeMesh.HeVertex<TPosition>(vertexIndex, vertex.Position);

                heVertices.Add(vertexIndex, heVertex);
            }
            vertexIndexEnumerator.Dispose();


            // Add Halfedges
            var edgeIndexEnumerator = _edges.Keys.GetEnumerator();

            while (edgeIndexEnumerator.MoveNext())
            {
                int edgeIndex = edgeIndexEnumerator.Current;

                FvEdge<TPosition> fvEdge = GetEdge(edgeIndex);

                HalfedgeMesh.HeVertex<TPosition> heStartVertex = heVertices[fvEdge.StartVertex.Index];
                HalfedgeMesh.HeVertex<TPosition> heEndVertex = heVertices[fvEdge.EndVertex.Index];

                // Add the halfedge pair to the mesh.
                HalfedgeMesh.HeHalfedge<TPosition> halfedge = new HalfedgeMesh.HeHalfedge<TPosition>(2 * edgeIndex, heStartVertex, heEndVertex);
                HalfedgeMesh.HeHalfedge<TPosition> pairHalfedge = new HalfedgeMesh.HeHalfedge<TPosition>((2 * edgeIndex) + 1, heEndVertex, heStartVertex);

                halfedge.PairHalfedge = pairHalfedge;
                pairHalfedge.PairHalfedge = halfedge;

                heHalfedges.Add(2 * edgeIndex, halfedge);
                heHalfedges.Add((2 * edgeIndex) + 1, pairHalfedge);
            }
            edgeIndexEnumerator.Dispose();


            // Add Faces & Manage faces connectivity
            var faceIndexEnumerator = _faces.Keys.GetEnumerator();
            bool[] isHeAssigned = new bool[heHalfedges.Count];

            while (faceIndexEnumerator.MoveNext())
            {
                int faceIndex = faceIndexEnumerator.Current;

                FvFace<TPosition> face = GetFace(faceIndex);
                IReadOnlyList<FvEdge<TPosition>> faceEdges = face.FaceEdges();
                IReadOnlyList<FvVertex<TPosition>> faceVertices = face.FaceVertices();

                // Get the face halfedges
                HalfedgeMesh.HeHalfedge<TPosition>[] heFaceHalfedges = new HalfedgeMesh.HeHalfedge<TPosition>[faceEdges.Count];
                for (int i = 0; i < faceEdges.Count; i++)
                {
                    HalfedgeMesh.HeVertex<TPosition> heVertex = heVertices[faceVertices[i].Index];
                    HalfedgeMesh.HeHalfedge<TPosition> heHalfedge = heHalfedges[2 * faceEdges[i].Index];

                    if (heVertex.Equals(heHalfedge.StartVertex)) { heFaceHalfedges[i] = heHalfedge; }
                    else { heFaceHalfedges[i] = heHalfedge.PairHalfedge; }
                }

                // Create the face
                HalfedgeMesh.HeFace<TPosition> heFace = new HalfedgeMesh.HeFace<TPosition>(faceIndex, heFaceHalfedges[0]);

                heFaces.Add(faceIndex, heFace);

                // Manage face halfedges connectivity
                for (int i_He = 0; i_He < heFaceHalfedges.Length; i_He++)
                {
                    int i_PrevHe = (i_He + heFaceHalfedges.Length - 1) % (heFaceHalfedges.Length);
                    int i_NextHe = (i_He + 1) % (heFaceHalfedges.Length);

                    heFaceHalfedges[i_He].PrevHalfedge = heFaceHalfedges[i_PrevHe];
                    heFaceHalfedges[i_He].NextHalfedge = heFaceHalfedges[i_NextHe];

                    heFaceHalfedges[i_He].AdjacentFace = heFace;

                    isHeAssigned[heFaceHalfedges[i_He].Index] = true;
                }
            }
            faceIndexEnumerator.Dispose();


            // Manage boundary halfedges connectivity
            for (int i_He = 0; i_He < heHalfedges.Count; i_He++)
            {
                if (isHeAssigned[i_He]) { continue; }

                HalfedgeMesh.HeHalfedge<TPosition> heHalfedge = heHalfedges[i_He];
                
                // if the halfedge is non-manifold
                if (heHalfedge.PairHalfedge.IsBoundary()) 
                {
                    throw new NotImplementedException("A halfedge and its pair do not have an adjacent face. Hence the connectivity could not be managed.");
                }

                if (heHalfedge.PrevHalfedge is null)
                {
                    HalfedgeMesh.HeHalfedge<TPosition> hePrevHalfedge = heHalfedge.PairHalfedge.NextHalfedge.PairHalfedge;

                    while (!hePrevHalfedge.IsBoundary()) { hePrevHalfedge = hePrevHalfedge.NextHalfedge.PairHalfedge; }

                    heHalfedge.PrevHalfedge = hePrevHalfedge;
                    hePrevHalfedge.NextHalfedge = heHalfedge;
                }

                if (heHalfedge.NextHalfedge is null)
                {
                    HalfedgeMesh.HeHalfedge<TPosition> heNextHalfedge = heHalfedge.PairHalfedge.PrevHalfedge.PairHalfedge;

                    while (!heNextHalfedge.IsBoundary()) { heNextHalfedge = heNextHalfedge.PrevHalfedge.PairHalfedge; }

                    heHalfedge.NextHalfedge = heNextHalfedge;
                    heNextHalfedge.PrevHalfedge = heHalfedge;
                }

                isHeAssigned[i_He] = true;
            }

            return new HalfedgeMesh.HeMesh<TPosition>(heVertices, heHalfedges, heFaces, _newVertexIndex, 2 * _newEdgeIndex, _newFaceIndex);
        }

        #endregion


        #region Override : Object

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"FvMesh with {VertexCount} vertices, {EdgeCount} edges, {FaceCount} faces.";
        }

        #endregion

        #region Override : Mesh<T,FvVertex<T>,FvEdge<T>,FvFace<T>>

        /******************** Methods - For this Mesh ********************/

        /// <inheritdoc/>
        public override void CleanMesh(bool cullIsolated = true)
        {
            /* Isolated Face : Face with no adjacent faces.
             * Isolated Edge : Edge (and pair edge) with no adjacent faces.
             * Isolated Vertex : Vertex with connected edges.
             * 
             * Could be replaced by:
             * Isolated Edge : Edge (and pair edge) with no adjacent faces, and whose start and end vertices have a valency of 1.
             * Isolated Vertex : Vertex with connected edges.
             */

            /********** For Faces **********/

            if ((FaceCount != _newFaceIndex) || cullIsolated)
            {
                int newFaceIndex = 0;
                List<int> isolatedFaces = new List<int>();
                Dictionary<int, FvFace<TPosition>> newFaces = new Dictionary<int, FvFace<TPosition>>();

                foreach (int key in _faces.Keys)
                {
                    if (cullIsolated && _faces[key].AdjacentFaces().Count == 0)
                    {
                        isolatedFaces.Add(key);     // Marks for removal.
                        continue;                   // Avoids storing the face in the new dictionnary of faces.
                    }

                    newFaces.Add(newFaceIndex, _faces[key]);
                    _faces[key].Index = newFaceIndex;
                    newFaceIndex += 1;
                }

                // Remove isolated faces
                foreach (int key in isolatedFaces) { RemoveFace(key); }

                // Reconfigure mesh faces.
                _faces = newFaces;
                _newFaceIndex = FaceCount;
            }

            /********** For Edges **********/

            if (EdgeCount != _newEdgeIndex)
            {
                int newEdgeIndex = 0;
                List<int> isolatedEdges = new List<int>();
                Dictionary<int, FvEdge<TPosition>> newEdges = new Dictionary<int, FvEdge<TPosition>>();

                foreach (int key in _edges.Keys)
                {
                    if (cullIsolated && _edges[key].AdjacentFaces().Count == 0)
                    {
                        isolatedEdges.Add(key);     // Marks for removal.
                        continue;                   // Avoids storing the edge in the new dictionnary of edges.
                    }

                    newEdges.Add(newEdgeIndex, _edges[key]);
                    _edges[key].Index = newEdgeIndex;
                    newEdgeIndex++;
                }

                // Remove isolated edges
                foreach (int key in isolatedEdges) { RemoveEdge(key); }

                // Reconfigure mesh edges.
                _edges = newEdges;
                _newEdgeIndex = VertexCount;
            }

            /********** For Vertices **********/

            if (VertexCount != _newVertexIndex)
            {
                int newVertexIndex = 0;
                List<int> isolatedVertices = new List<int>();
                Dictionary<int, FvVertex<TPosition>> newVertices = new Dictionary<int, FvVertex<TPosition>>();

                foreach (int key in _vertices.Keys)
                {
                    if (cullIsolated && !_vertices[key].IsConnected())
                    {
                        isolatedVertices.Add(key);      // Marks for removal.
                        continue;                       // Avoids storing the vertex in the new dictionnary of vertices.
                    }

                    newVertices.Add(newVertexIndex, _vertices[key]);
                    _vertices[key].Index = newVertexIndex;
                    newVertexIndex++;
                }

                // Remove isolated vertices
                foreach (int key in isolatedVertices) { RemoveVertex(key); }

                _vertices = newVertices;
                _newVertexIndex = VertexCount;
            }

        }

        /// <inheritdoc/>
        public override object Clone()
        {
            FvMesh<TPosition> cloneFvMesh = new FvMesh<TPosition>();

            // Add Vertices
            var vertexIndexEnumerator = _vertices.Keys.GetEnumerator();

            while (vertexIndexEnumerator.MoveNext())
            {
                int vertexIndex = vertexIndexEnumerator.Current;

                FvVertex<TPosition> vertex = GetVertex(vertexIndex);

                FvVertex<TPosition> cloneVertex = new FvVertex<TPosition>(vertexIndex, vertex.Position);
                cloneFvMesh._vertices.Add(vertexIndex, cloneVertex);
            }
            vertexIndexEnumerator.Dispose();


            // Add Edges
            var edgeIndexEnumerator = _edges.Keys.GetEnumerator();

            while (edgeIndexEnumerator.MoveNext())
            {
                int edgeIndex = edgeIndexEnumerator.Current;

                FvEdge<TPosition> edge = GetEdge(edgeIndex);

                FvVertex<TPosition> cloneStartVertex = cloneFvMesh.GetVertex(edge.StartVertex.Index);
                FvVertex<TPosition> cloneEndVertex = cloneFvMesh.GetVertex(edge.EndVertex.Index);

                FvEdge<TPosition> cloneEdge = new FvEdge<TPosition>(edgeIndex, cloneStartVertex, cloneEndVertex);
                cloneFvMesh._edges.Add(edgeIndex, cloneEdge);
            }
            edgeIndexEnumerator.Dispose();


            // Add Faces
            var faceIndexEnumerator = _faces.Keys.GetEnumerator();

            while (faceIndexEnumerator.MoveNext())
            {
                int faceIndex = faceIndexEnumerator.Current;

                FvFace<TPosition> face = GetFace(faceIndex);

                IReadOnlyList<FvEdge<TPosition>> faceEdges = face.FaceEdges();
                List<FvEdge<TPosition>> cloneFaceEdges = new List<FvEdge<TPosition>>(faceEdges.Count);
                for (int i_FE = 0; i_FE < faceEdges.Count; i_FE++)
                {
                    cloneFaceEdges.Add(cloneFvMesh.GetEdge(faceEdges[i_FE].Index));
                }

                IReadOnlyList<FvVertex<TPosition>> faceVertices = face.FaceVertices();
                List<FvVertex<TPosition>> cloneFaceVertices = new List<FvVertex<TPosition>>(faceVertices.Count);
                for (int i_FV = 0; i_FV < faceVertices.Count; i_FV++)
                {
                    cloneFaceVertices.Add(cloneFvMesh.GetVertex(faceVertices[i_FV].Index));
                }

                FvFace<TPosition> cloneFace = new FvFace<TPosition>(faceIndex, cloneFaceVertices, cloneFaceEdges);
                cloneFvMesh._faces.Add(faceIndex, cloneFace);
            }
            faceIndexEnumerator.Dispose();


            // Manage the connectivity of the vertices (_connectedEdges).
            vertexIndexEnumerator = _vertices.Keys.GetEnumerator();

            while (vertexIndexEnumerator.MoveNext())
            {
                int vertexIndex = vertexIndexEnumerator.Current;

                FvVertex<TPosition> vertex = GetVertex(vertexIndex);
                IReadOnlyList<FvEdge<TPosition>> connectedEdges = vertex._connectedEdges;

                FvVertex<TPosition> cloneVertex = cloneFvMesh.GetVertex(vertexIndex);

                List<FvEdge<TPosition>> cloneConnectedEdges = new List<FvEdge<TPosition>>(connectedEdges.Count);
                for (int i_FE = 0; i_FE < connectedEdges.Count; i_FE++)
                {
                    cloneConnectedEdges.Add(cloneFvMesh.GetEdge(connectedEdges[i_FE].Index));
                }

                cloneVertex._connectedEdges = cloneConnectedEdges;
            }
            vertexIndexEnumerator.Dispose();


            // Manage the connectivity of edges (_adjacentFaces)
            edgeIndexEnumerator = _edges.Keys.GetEnumerator();

            while (edgeIndexEnumerator.MoveNext())
            {
                int edgeIndex = edgeIndexEnumerator.Current;

                FvEdge<TPosition> edge = GetEdge(edgeIndex);
                IReadOnlyList<FvFace<TPosition>> adjacentFaces = edge._adjacentFaces;

                FvEdge<TPosition> cloneEdge = cloneFvMesh.GetEdge(edgeIndex);

                List<FvFace<TPosition>> cloneAdjacentFaces = new List<FvFace<TPosition>>(adjacentFaces.Count);
                for (int i_FE = 0; i_FE < adjacentFaces.Count; i_FE++)
                {
                    cloneAdjacentFaces.Add(cloneFvMesh.GetFace(adjacentFaces[i_FE].Index));
                }

                cloneEdge._adjacentFaces= cloneAdjacentFaces;
            }
            edgeIndexEnumerator.Dispose();

            cloneFvMesh._newVertexIndex = _newVertexIndex;
            cloneFvMesh._newEdgeIndex = _newEdgeIndex;
            cloneFvMesh._newFaceIndex = _newFaceIndex;

            return cloneFvMesh;
        }

        /******************** Methods - On Vertices ********************/

        /// <inheritdoc/>
        public override FvVertex<TPosition> AddVertex(TPosition position)
        {
            // Creates new instance of vertex.
            FvVertex<TPosition> vertex = new FvVertex<TPosition>(_newVertexIndex, position);

            _vertices.Add(_newVertexIndex, vertex);

            _newVertexIndex += 1;

            return vertex;
        }


        /// <inheritdoc/>
        public override FvVertex<TPosition> GetVertex(int index)
        {
            return _vertices[index];
        }

        /// <inheritdoc/>
        public override FvVertex<TPosition> TryGetVertex(int index)
        {
            _vertices.TryGetValue(index, out FvVertex<TPosition> vertex);

            return vertex;
        }

        /// <inheritdoc/>
        public override IReadOnlyList<FvVertex<TPosition>> GetVertices()
        {
            FvVertex<TPosition>[] result = new FvVertex<TPosition>[VertexCount];

            int i_Vertex = 0;
            foreach (FvVertex<TPosition> vertex in _vertices.Values)
            {
                result[i_Vertex] = vertex;
                i_Vertex++;
            }

            return result;
        }


        /// <inheritdoc/>
        public override void RemoveVertex(FvVertex<TPosition> vertex)
        {
            IReadOnlyList<FvFace<TPosition>> adjacentFaces = vertex.AdjacentFaces();

            for (int i_AF = 0; i_AF < adjacentFaces.Count; i_AF++)
            {
                RemoveFace(adjacentFaces[i_AF]);
            }
        }


        /// <inheritdoc/>
        protected override void EraseVertex(FvVertex<TPosition> vertex)
        {
            // If the current vertex is still connected.
            if (vertex._connectedEdges.Count != 0)
            {
                throw new InvalidOperationException("The vertex cannot be erased if it is still connected.");
            }

            // Remove the vertex from the mesh.
            _vertices.Remove(vertex.Index);

            // Unset the current vertex.
            vertex.Unset();
        }


        /******************** Methods - On Edges ********************/

        /// <inheritdoc/>
        internal override FvEdge<TPosition> AddEdge(FvVertex<TPosition> startVertex, FvVertex<TPosition> endVertex)
        {
            // Verification : Avoid looping halfedges
            if (startVertex.Equals(endVertex)) { return null; }

            // Verifications : Avoid duplicate halfedges
            FvEdge<TPosition> exitingEdge = EdgeBetween(startVertex, endVertex);
            if (!(exitingEdge is null)) { return null; }


            FvEdge<TPosition> edge = new FvEdge<TPosition>(_newEdgeIndex, startVertex, endVertex);

            this._edges.Add(_newEdgeIndex, edge);

            _newEdgeIndex += 1;

            return edge;
        }


        /// <inheritdoc/>
        public override FvEdge<TPosition> GetEdge(int index)
        {
            return _edges[index];
        }

        /// <inheritdoc/>
        public override FvEdge<TPosition> TryGetEdge(int index)
        {
            _edges.TryGetValue(index, out FvEdge<TPosition> edge);

            return edge;
        }

        /// <inheritdoc/>
        public override IReadOnlyList<FvEdge<TPosition>> GetEdges()
        {
            FvEdge<TPosition>[] result = new FvEdge<TPosition>[EdgeCount];

            int i_Edge = 0;
            foreach (FvEdge<TPosition> edge in _edges.Values)
            {
                result[i_Edge] = edge;
                i_Edge++;
            }

            return result;
        }


        /// <inheritdoc/>
        public override void RemoveEdge(FvEdge<TPosition> edge)
        {
            IReadOnlyList<FvFace<TPosition>> adjacentFaces = edge.AdjacentFaces();

            if (edge.IsBoundary() && adjacentFaces.Count == 0)
            {
                FvVertex<TPosition> startVertex = edge.StartVertex;
                FvVertex<TPosition> endVertex = edge.StartVertex;

                EraseEdge(edge);

                // Manage start and end vertex
                if (!startVertex.IsConnected()) { EraseVertex(startVertex); }
                if (!endVertex.IsConnected()) { EraseVertex(endVertex); }
            }
            else
            {
                // Manage connection with adjacent face
                for (int i_AF = 0; i_AF < adjacentFaces.Count; i_AF++)
                {
                    RemoveFace(adjacentFaces[i_AF]);
                }
            }
        }


        /// <inheritdoc/>
        public override void EraseEdge(FvEdge<TPosition> edge)
        {
            // Manage connection with start and end vertices
            edge.StartVertex._connectedEdges.Remove(edge);
            edge.EndVertex._connectedEdges.Remove(edge);

            // Manage connection with adjacent face
            IReadOnlyList<FvFace<TPosition>> adjacentFaces = edge.AdjacentFaces();
            for (int i_AF = 0; i_AF < adjacentFaces.Count; i_AF++)
            {
                EraseFace(adjacentFaces[i_AF]);
            }

            // Remove the pair of edges from the mesh
            _edges.Remove(edge.Index);

            // Unset the pair of edges
            edge.Unset();
        }


        /******************** Methods - On Faces ********************/

        /// <inheritdoc/>
        public override FvFace<TPosition> AddFace(List<FvVertex<TPosition>> vertices)
        {
            // Verifications
            if (vertices.Count < 3) { throw new ArgumentOutOfRangeException("A face must have at least three vertices."); }
            foreach (FvVertex<TPosition> vertex in vertices)
            {
                if (!vertex.Equals(GetVertex(vertex.Index)))
                {
                    throw new ArgumentException("One of the input vertex does not belong to this mesh.");
                }
            }

            // Create the list of edges around the face
            List<FvEdge<TPosition>> edges = new List<FvEdge<TPosition>>();
            for (int i = 0; i < vertices.Count; i++)
            {
                int j = (i + 1) % (vertices.Count);
                FvEdge<TPosition> edge = EdgeBetween(vertices[i], vertices[j]);

                if (edge is null)
                {
                    FvVertex<TPosition> start = vertices[i];
                    FvVertex<TPosition> end = vertices[j];

                    edge = AddEdge(start, end);
                }
                edges.Add(edge);
            }


            // Should check if the face already exists and other things (vertex belonging to the right mesh, etc...)
            FvFace<TPosition> face = new FvFace<TPosition>(_newFaceIndex, vertices, edges);

            this._faces.Add(_newFaceIndex, face);

            _newFaceIndex += 1;

            return face;
        }


        /// <inheritdoc/>
        public override FvFace<TPosition> GetFace(int index)
        {
            return _faces[index];
        }

        /// <inheritdoc/>
        public override FvFace<TPosition> TryGetFace(int index)
        {
            _faces.TryGetValue(index, out FvFace<TPosition> face);

            return face;
        }

        /// <inheritdoc/>
        public override IReadOnlyList<FvFace<TPosition>> GetFaces()
        {
            FvFace<TPosition>[] result = new FvFace<TPosition>[FaceCount];

            int i_Face = 0;
            foreach (FvFace<TPosition> face in _faces.Values)
            {
                result[i_Face] = face;
                i_Face++;
            }

            return result;
        }


        /// <inheritdoc/>
        public override void RemoveFace(FvFace<TPosition> face)
        {
            // Manage connection with edges
            IReadOnlyList<FvEdge<TPosition>> faceEdges = face.FaceEdges();
            for (int i_FE = 0; i_FE < faceEdges.Count; i_FE++)
            {
                FvEdge<TPosition> edge = faceEdges[i_FE];
                if (edge.IsBoundary())
                {
                    edge._adjacentFaces = new List<FvFace<TPosition>>(); // Empty the list
                    EraseEdge(edge);
                }
                else
                {
                    edge._adjacentFaces.Remove(face);
                }
            }

            // Manage isolated vertices
            IReadOnlyList<FvVertex<TPosition>> faceVertices = face.FaceVertices();
            for (int i_FV = 0; i_FV < faceEdges.Count; i_FV++)
            {
                FvVertex<TPosition> vertex = faceVertices[i_FV];

                if (!vertex.IsConnected()) { EraseVertex(vertex); }
            }

            // Erase the face.
            EraseFace(face);
        }


        /// <inheritdoc/>
        public override void EraseFace(FvFace<TPosition> face)
        {
            // Manage connection with edges
            IReadOnlyList<FvEdge<TPosition>> faceEdges = face.FaceEdges();
            for (int i_FE = 0; i_FE < faceEdges.Count; i_FE++)
            {
                FvEdge<TPosition> edge = faceEdges[i_FE];

                edge._adjacentFaces?.Remove(face);
            }

            // Remove the face from the mesh
            _faces.Remove(face.Index);

            // Unset the face
            face.Unset();
        }

        #endregion
    }
}