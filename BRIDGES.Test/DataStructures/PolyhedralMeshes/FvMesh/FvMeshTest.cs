using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.DataStructures.PolyhedralMeshes.FaceVertexMesh;

using Euc3D = BRIDGES.Geometry.Euclidean3D;


namespace BRIDGES.Test.DataStructures.PolyhedralMeshes.FvMesh
{
    /// <summary>
    /// Class testing the members of the <see cref="FvMesh{TPosition}"/> data structure.
    /// </summary>
    [TestClass]
    public class FvMeshTest
    {
        #region Test Fields

        private static FvMesh<Euc3D.Point> _parallelepiped = new FvMesh<Euc3D.Point>();


        /******************** MSTest Processes ********************/

        /// <summary>
        /// Initialises the fields of the test class.
        /// </summary>
        /// <param name="context"> Context of the test. </param>
        [ClassInitialize]
        public static void InitialiseClass(TestContext context)
        {
            _parallelepiped = InitialiseParalelepiped();
        }

        /// <summary>
        /// Cleans the fields of the test class.
        /// </summary>
        [ClassCleanup]
        public static void CleanUpClass()
        {
            /* Dispose of the meshes ? */
        }


        /******************** Helpers ********************/

        /// <summary>
        /// Generates a parallelepiped in three-dimensional euclidean space.
        /// </summary>
        /// <returns></returns>
        private static FvMesh<Euc3D.Point> InitialiseParalelepiped()
        {
            FvMesh<Euc3D.Point> parallelepiped = new FvMesh<Euc3D.Point>();

            // Create vertice's position
            Euc3D.Point p0 = new Euc3D.Point(3, 1, 2);
            Euc3D.Point p1 = new Euc3D.Point(8, 1, 2);
            Euc3D.Point p2 = new Euc3D.Point(8, 4, 1);
            Euc3D.Point p3 = new Euc3D.Point(3, 4, 1);
            Euc3D.Point p4 = new Euc3D.Point(2, 2.5, 5);
            Euc3D.Point p5 = new Euc3D.Point(7, 2.5, 5);
            Euc3D.Point p6 = new Euc3D.Point(7, 5.5, 4);
            Euc3D.Point p7 = new Euc3D.Point(2, 5.5, 4);

            // Add vertices
            FvVertex<Euc3D.Point> v0 = parallelepiped.AddVertex(p0);
            FvVertex<Euc3D.Point> v1 = parallelepiped.AddVertex(p1);
            FvVertex<Euc3D.Point> v2 = parallelepiped.AddVertex(p2);
            FvVertex<Euc3D.Point> v3 = parallelepiped.AddVertex(p3);
            FvVertex<Euc3D.Point> v4 = parallelepiped.AddVertex(p4);
            FvVertex<Euc3D.Point> v5 = parallelepiped.AddVertex(p5);
            FvVertex<Euc3D.Point> v6 = parallelepiped.AddVertex(p6);
            FvVertex<Euc3D.Point> v7 = parallelepiped.AddVertex(p7);

            // Add faces
            parallelepiped.AddFace(v0, v1, v2, v3);

            parallelepiped.AddFace(v1, v0, v4, v5);
            parallelepiped.AddFace(v2, v1, v5, v6);
            parallelepiped.AddFace(v3, v2, v6, v7);
            parallelepiped.AddFace(v0, v3, v7, v4);

            parallelepiped.AddFace(v7, v6, v5, v4);

            return parallelepiped;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Tests the <see cref="FvMesh{TPosition}.VertexCount"/> property.
        /// </summary>
        [TestMethod("Property VertexCount")]
        public void VertexCount()
        {
            // Arrange

            //Act
            int vertexCount = _parallelepiped.VertexCount;

            // Assert
            Assert.AreEqual(8, vertexCount);
        }

        /// <summary>
        /// Tests the <see cref="FvMesh{TPosition}.EdgeCount"/> property.
        /// </summary>
        [TestMethod("Property EdgeCount")]
        public void EdgeCount()
        {
            // Arrange

            //Act
            int edgeCount = _parallelepiped.EdgeCount;

            // Assert
            Assert.AreEqual(12, edgeCount);
        }

        /// <summary>
        /// Tests the <see cref="FvMesh{TPosition}.FaceCount"/> property.
        /// </summary>
        [TestMethod("Property FaceCount")]
        public void FaceCount()
        {
            // Arrange

            //Act
            int faceCount = _parallelepiped.FaceCount;

            // Assert
            Assert.AreEqual(6, faceCount);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Tests the initialisation of the <see cref="FvMesh{TPosition}"/>.
        /// </summary>
        [TestMethod("Constructor()")]
        public void Constructor()
        {
            // Arrange
            FvMesh<Euc3D.Point> parallelepiped = new FvMesh<Euc3D.Point>();

            //Act
            int vertexCount = parallelepiped.VertexCount;
            int edgeCount = parallelepiped.EdgeCount;
            int faceCount = parallelepiped.FaceCount;

            // Assert
            Assert.AreEqual(0, vertexCount);
            Assert.AreEqual(0, edgeCount);
            Assert.AreEqual(0, faceCount);
        }

        #endregion


        #region Override : Mesh<T,HeVertex<T>,HeEdge<T>,HeFace<T>>

        /******************** On Vertices ********************/

        /// <summary>
        /// Tests the method <see cref="FvMesh{TPosition}.AddVertex(TPosition)"/>.
        /// </summary>
        [TestMethod("Method AddVertex(TPosition)")]
        public void AddVertex_TPosition()
        {
            // Arrange
            FvMesh<Euc3D.Point> mesh = new FvMesh<Euc3D.Point>();

            Euc3D.Point point = new Euc3D.Point(1.0, 2.0, 4.0);

            // Act
            FvVertex<Euc3D.Point> vertex = mesh.AddVertex(point);

            // Assert
            Assert.AreEqual(1, mesh.VertexCount);
            Assert.IsTrue(point.Equals(vertex.Position));
        }


        /// <summary>
        /// Tests the method <see cref="FvMesh{TPosition}.GetVertex(int)"/>.
        /// </summary>
        [TestMethod("Method GetVertex(Int)")]
        public void GetVertex_Int()
        {
            // Arrange
            int existingIndex = 4;
            Euc3D.Point point = new Euc3D.Point(2, 2.5, 5);

            int absentIndex = 10;
            bool throwsException = false;

            // Act
            FvVertex<Euc3D.Point> existingVertex = _parallelepiped.GetVertex(existingIndex);
            Euc3D.Point position = existingVertex.Position;

            FvVertex<Euc3D.Point> absentVertex = default;
            try { absentVertex = _parallelepiped.GetVertex(absentIndex); }
            catch (KeyNotFoundException) { throwsException = true; }

            // Assert
            Assert.AreEqual(existingIndex, existingVertex.Index);
            Assert.IsTrue(point.Equals(position));

            Assert.IsTrue(throwsException);
            Assert.IsTrue(absentVertex is null);
        }

        /// <summary>
        /// Tests the method <see cref="FvMesh{TPosition}.TryGetVertex(int)"/>.
        /// </summary>
        [TestMethod("Method TryGetVertex(Int)")]
        public void TryGetVertex_Int()
        {
            // Arrange
            int existingIndex = 6;
            Euc3D.Point point = new Euc3D.Point(7, 5.5, 4);

            int absentIndex = 12;

            // Act
            FvVertex<Euc3D.Point> vertex = _parallelepiped.TryGetVertex(existingIndex);
            Euc3D.Point position = vertex.Position;

            FvVertex<Euc3D.Point> absentVertex = _parallelepiped.TryGetVertex(absentIndex);

            // Assert
            Assert.AreEqual(existingIndex, vertex.Index);
            Assert.IsTrue(point.Equals(position));

            Assert.IsTrue(absentVertex is null);
        }

        /// <summary>
        /// Tests the method <see cref="FvMesh{TPosition}.GetVertices()"/>.
        /// </summary>
        [TestMethod("Method GetVertices()")]
        public void GetVertices()
        {
            // Arrange
            Euc3D.Point[] points = new Euc3D.Point[8];

            points[0] = new Euc3D.Point(3, 1, 2);
            points[1] = new Euc3D.Point(8, 1, 2);
            points[2] = new Euc3D.Point(8, 4, 1);
            points[3] = new Euc3D.Point(3, 4, 1);
            points[4] = new Euc3D.Point(2, 2.5, 5);
            points[5] = new Euc3D.Point(7, 2.5, 5);
            points[6] = new Euc3D.Point(7, 5.5, 4);
            points[7] = new Euc3D.Point(2, 5.5, 4);

            // Act
            IReadOnlyList<FvVertex<Euc3D.Point>> vertices = _parallelepiped.GetVertices();

            // Arrange
            Assert.AreEqual(points.Length, vertices.Count);

            for (int i = 0; i < points.Length; i++)
            {
                Assert.IsTrue(points[i].Equals(vertices[i].Position));
            }
        }


        /// <summary>
        /// Tests the method <see cref="FvMesh{TPosition}.RemoveVertex(FvVertex{TPosition})"/>.
        /// </summary>
        [TestMethod("Method RemoveVertex(FvVertex)")]
        public void RemoveVertex_FvVertex()
        {
            // Arrange
            FvMesh<Euc3D.Point> heMesh = _parallelepiped.Clone() as FvMesh<Euc3D.Point>;

            FvVertex<Euc3D.Point> vertex = heMesh.GetVertex(4);

            // Act
            heMesh.RemoveVertex(vertex);

            // Assert
            Assert.AreEqual(7, heMesh.VertexCount);
            Assert.AreEqual(9, heMesh.EdgeCount);
            Assert.AreEqual(3, heMesh.FaceCount);
        }


        /******************** On Edges ********************/

        /// <summary>
        /// Tests the method <see cref="FvMesh{TPosition}.AddEdge(FvVertex{TPosition}, FvVertex{TPosition})"/>.
        /// </summary>
        [TestMethod("Method AddEdge(FvVertex,FvVertex)")]
        public void AddEdge_HeVertex_FvVertex()
        {
            // Arrange
            FvMesh<Euc3D.Point> heMesh = new FvMesh<Euc3D.Point>();

            Euc3D.Point pointA = new Euc3D.Point(1.0, 2.0, 4.0);
            Euc3D.Point pointB = new Euc3D.Point(3.0, 5.0, 6.0);

            // Act
            FvVertex<Euc3D.Point> vertexA = heMesh.AddVertex(pointA);
            FvVertex<Euc3D.Point> vertexB = heMesh.AddVertex(pointB);

            FvEdge<Euc3D.Point> edge = heMesh.AddEdge(vertexA, vertexB);

            // Assert
            Assert.AreEqual(2, heMesh.VertexCount);
            Assert.AreEqual(1, heMesh.EdgeCount);
            Assert.AreEqual(0, heMesh.FaceCount);

            Assert.AreEqual(0, edge.Index);
            Assert.IsTrue(vertexA.Equals(edge.StartVertex));
            Assert.IsTrue(vertexB.Equals(edge.EndVertex));
        }


        /// <summary>
        /// Tests the method <see cref="FvMesh{TPosition}.GetEdge(int)"/>.
        /// </summary>
        [TestMethod("Method GetEdge(Int)")]
        public void GetEdge_Int()
        {
            // Arrange
            int existingIndex = 5;
            Euc3D.Point startPoint = new Euc3D.Point(2, 2.5, 5);
            Euc3D.Point endPoint = new Euc3D.Point(7, 2.5, 5);

            int absentIndex = 15;
            bool throwsException = false;

            // Act
            FvEdge<Euc3D.Point> existingEdge = _parallelepiped.GetEdge(existingIndex);
            FvVertex<Euc3D.Point> startVertex = existingEdge.StartVertex;
            FvVertex<Euc3D.Point> endVertex = existingEdge.EndVertex;

            FvEdge<Euc3D.Point> absentEdge = default;
            try { absentEdge = _parallelepiped.GetEdge(absentIndex); }
            catch (KeyNotFoundException) { throwsException = true; }

            // Assert
            Assert.AreEqual(existingIndex, existingEdge.Index);
            Assert.IsTrue(startPoint.Equals(startVertex.Position));
            Assert.IsTrue(endPoint.Equals(endVertex.Position));

            Assert.IsTrue(throwsException);
            Assert.IsTrue(absentEdge is null);
        }

        /// <summary>
        /// Tests the method <see cref="FvMesh{TPosition}.TryGetEdge(int)"/>.
        /// </summary>
        [TestMethod("Method TryGetEdge(Int)")]
        public void TryGetEdge_Int()
        {
            // Arrange
            int existingIndex = 11;
            Euc3D.Point startPoint = new Euc3D.Point(2, 5.5, 4);
            Euc3D.Point endPoint = new Euc3D.Point(2, 2.5, 5);

            int absentIndex = 12;

            // Act
            FvEdge<Euc3D.Point> existingEdge = _parallelepiped.TryGetEdge(existingIndex);
            FvVertex<Euc3D.Point> startVertex = existingEdge.StartVertex;
            FvVertex<Euc3D.Point> endVertex = existingEdge.EndVertex;

            FvEdge<Euc3D.Point> absentEdge = _parallelepiped.TryGetEdge(absentIndex);

            // Assert
            Assert.AreEqual(existingIndex, existingEdge.Index);
            Assert.IsTrue(startPoint.Equals(startVertex.Position));
            Assert.IsTrue(endPoint.Equals(endVertex.Position));

            Assert.IsTrue(absentEdge is null);
        }

        /// <summary>
        /// Tests the method <see cref="FvMesh{TPosition}.GetEdges()"/>.
        /// </summary>
        [TestMethod("Method GetEdges()")]
        public void GetEdges()
        {
            // Arrange
            Euc3D.Point p0 = new Euc3D.Point(3, 1, 2);
            Euc3D.Point p1 = new Euc3D.Point(8, 1, 2);
            Euc3D.Point p2 = new Euc3D.Point(8, 4, 1);
            Euc3D.Point p3 = new Euc3D.Point(3, 4, 1);
            Euc3D.Point p4 = new Euc3D.Point(2, 2.5, 5);
            Euc3D.Point p5 = new Euc3D.Point(7, 2.5, 5);
            Euc3D.Point p6 = new Euc3D.Point(7, 5.5, 4);
            Euc3D.Point p7 = new Euc3D.Point(2, 5.5, 4);

            // Act
            IReadOnlyList<FvEdge<Euc3D.Point>> edges = _parallelepiped.GetEdges();

            // Arrange
            Assert.AreEqual(12, edges.Count);

            for (int i = 0; i < edges.Count; i++)
            {
                Assert.AreEqual(i, edges[i].Index);
            }

            Assert.IsTrue(p0.Equals(edges[3].EndVertex.Position));
            Assert.IsTrue(p1.Equals(edges[1].StartVertex.Position));
            Assert.IsTrue(p2.Equals(edges[8].EndVertex.Position));
            Assert.IsTrue(p3.Equals(edges[3].StartVertex.Position));
            Assert.IsTrue(p4.Equals(edges[11].EndVertex.Position));
            Assert.IsTrue(p5.Equals(edges[7].StartVertex.Position));
            Assert.IsTrue(p6.Equals(edges[7].EndVertex.Position));
            Assert.IsTrue(p7.Equals(edges[10].StartVertex.Position));
        }


        /// <summary>
        /// Tests the method <see cref="FvMesh{TPosition}.RemoveEdge(FvEdge{TPosition})"/>.
        /// </summary>
        [TestMethod("Method RemoveEdge(FvEdge)")]
        public void RemoveEdge_FvEdge()
        {
            // Arrange
            FvMesh<Euc3D.Point> heMesh = _parallelepiped.Clone() as FvMesh<Euc3D.Point>;

            FvEdge<Euc3D.Point> edge = heMesh.GetEdge(5);

            // Act
            heMesh.RemoveEdge(edge);

            // Assert
            Assert.AreEqual(8, heMesh.VertexCount);
            Assert.AreEqual(11, heMesh.EdgeCount);
            Assert.AreEqual(4, heMesh.FaceCount);
        }


        /// <summary>
        /// Tests the method <see cref="FvMesh{TPosition}.EraseEdge(FvEdge{TPosition})"/>.
        /// </summary>
        [TestMethod("Method EraseEdge(FvEdge)")]
        public void EraseEdge_FvEdge()
        {
            // Arrange
            FvMesh<Euc3D.Point> mesh = _parallelepiped.Clone() as FvMesh<Euc3D.Point>;

            FvEdge<Euc3D.Point> edge = mesh.GetEdge(4);

            // Act
            mesh.EraseEdge(edge);

            // Assert
            Assert.AreEqual(8, mesh.VertexCount);
            Assert.AreEqual(11, mesh.EdgeCount);
            Assert.AreEqual(4, mesh.FaceCount);
        }


        /******************** On Faces ********************/

        /// <summary>
        /// Tests the method <see cref="FvMesh{TPosition}.AddFace(List{FvVertex{TPosition}})"/>.
        /// </summary>
        [TestMethod("Method AddFace(List<FvVertex>)")]
        public void AddEdge_FvVertexList()
        {
            // Arrange
            FvMesh<Euc3D.Point> mesh = new FvMesh<Euc3D.Point>();

            Euc3D.Point[] points = new Euc3D.Point[4];
            points[0] = new Euc3D.Point(1.0, 0.0, 2.0);
            points[1] = new Euc3D.Point(4.0, 1.0, 3.0);
            points[2] = new Euc3D.Point(5.0, 4.0, 4.0);
            points[3] = new Euc3D.Point(2.0, 3.0, 3.0);

            // Act
            List<FvVertex<Euc3D.Point>> vertices = new List<FvVertex<Euc3D.Point>>();
            for (int i = 0; i < 4; i++) { vertices.Add(mesh.AddVertex(points[i])); }

            FvFace<Euc3D.Point> face = mesh.AddFace(vertices);

            IReadOnlyList<FvVertex<Euc3D.Point>> faceVertices = face.FaceVertices();
            IReadOnlyList<FvEdge<Euc3D.Point>> faceEdges = face.FaceEdges();

            // Assert
            Assert.AreEqual(4, mesh.VertexCount);
            Assert.AreEqual(4, mesh.EdgeCount);
            Assert.AreEqual(1, mesh.FaceCount);

            Assert.AreEqual(0, face.Index);
            for (int i_V = 0; i_V < 4; i_V++) { Assert.IsTrue(faceVertices[i_V].Equals(vertices[i_V])); }
            for (int i_V = 0; i_V < 4; i_V++) { Assert.AreEqual(i_V, faceEdges[i_V].Index); }

        }


        /// <summary>
        /// Tests the method <see cref="FvMesh{TPosition}.GetFace(int)"/>.
        /// </summary>
        [TestMethod("Method GetFace(Int)")]
        public void GetFace_Int()
        {
            // Arrange
            int existingIndex = 4;
            FvEdge<Euc3D.Point>[] edges = new FvEdge<Euc3D.Point>[4];
            edges[0] = _parallelepiped.GetEdge(3);
            edges[1] = _parallelepiped.GetEdge(10);
            edges[2] = _parallelepiped.GetEdge(11);
            edges[3] = _parallelepiped.GetEdge(4);

            int absentIndex = 6;
            bool throwsException = false;

            // Act
            FvFace<Euc3D.Point> existingFace = _parallelepiped.GetFace(existingIndex);
            IReadOnlyList<FvEdge<Euc3D.Point>> faceEdges = existingFace.FaceEdges();

            FvFace<Euc3D.Point> absentFace = default;
            try { absentFace = _parallelepiped.GetFace(absentIndex); }
            catch (KeyNotFoundException) { throwsException = true; }

            // Assert
            Assert.AreEqual(existingIndex, existingFace.Index);
            for (int i_E = 0; i_E < 4; i_E++) { Assert.IsTrue(edges[i_E].Equals(faceEdges[i_E])); }

            Assert.IsTrue(throwsException);
            Assert.IsTrue(absentFace is null);
        }

        /// <summary>
        /// Tests the method <see cref="FvMesh{TPosition}.TryGetFace(int)"/>.
        /// </summary>
        [TestMethod("Method TryGetFace(Int)")]
        public void TryGetFace_Int()
        {
            // Arrange
            int existingIndex = 0;
            FvEdge<Euc3D.Point>[] edges = new FvEdge<Euc3D.Point>[4];
            edges[0] = _parallelepiped.GetEdge(0);
            edges[1] = _parallelepiped.GetEdge(1);
            edges[2] = _parallelepiped.GetEdge(2);
            edges[3] = _parallelepiped.GetEdge(3);

            int absentIndex = 12;

            // Act
            FvFace<Euc3D.Point> existingFace = _parallelepiped.TryGetFace(existingIndex);
            IReadOnlyList<FvEdge<Euc3D.Point>> faceEdges = existingFace.FaceEdges();

            FvFace<Euc3D.Point> absentFace = _parallelepiped.TryGetFace(absentIndex);

            // Assert
            Assert.AreEqual(existingIndex, existingFace.Index);
            for (int i_E = 0; i_E < 4; i_E++) { Assert.IsTrue(edges[i_E].Equals(faceEdges[i_E])); }

            Assert.IsTrue(absentFace is null);
        }

        /// <summary>
        /// Tests the method <see cref="FvMesh{TPosition}.GetFaces()"/>.
        /// </summary>
        [TestMethod("Method GetFaces()")]
        public void GetFaces()
        {
            // Arrange
            FvEdge<Euc3D.Point>[] edges = new FvEdge<Euc3D.Point>[6];
            edges[0] = _parallelepiped.GetEdge(0);
            edges[1] = _parallelepiped.GetEdge(0);
            edges[2] = _parallelepiped.GetEdge(1);
            edges[3] = _parallelepiped.GetEdge(2);
            edges[4] = _parallelepiped.GetEdge(3);
            edges[5] = _parallelepiped.GetEdge(9);

            // Act
            IReadOnlyList<FvFace<Euc3D.Point>> faces = _parallelepiped.GetFaces();

            FvEdge<Euc3D.Point>[] facesFirstEdge = new FvEdge<Euc3D.Point>[6];
            for (int i_F = 0; i_F < 6; i_F++)
            {
                IReadOnlyList<FvEdge<Euc3D.Point>> faceEdges = faces[i_F].FaceEdges();
                facesFirstEdge[i_F] = faceEdges[0];
            }

            // Arrange
            Assert.AreEqual(6, faces.Count);

            for (int i = 0; i < 6; i++) { Assert.AreEqual(i, faces[i].Index); }
            for (int i_F = 0; i_F < 6; i_F++) { Assert.IsTrue(edges[i_F].Equals(facesFirstEdge[i_F])); }
        }


        /// <summary>
        /// Tests the method <see cref="FvMesh{TPosition}.RemoveFace(FvFace{TPosition})"/>.
        /// </summary>
        [TestMethod("Method RemoveFace(FvFace)")]
        public void RemoveFace_FvFace()
        {
            // Arrange
            FvMesh<Euc3D.Point> heMesh = _parallelepiped.Clone() as FvMesh<Euc3D.Point>;

            FvFace<Euc3D.Point> face = heMesh.GetFace(0);

            // Act
            heMesh.RemoveFace(face);

            // Assert
            Assert.AreEqual(8, heMesh.VertexCount);
            Assert.AreEqual(12, heMesh.EdgeCount);
            Assert.AreEqual(5, heMesh.FaceCount);
        }


        /// <summary>
        /// Tests the method <see cref="FvMesh{TPosition}.EraseFace(FvFace{TPosition})"/>.
        /// </summary>
        [TestMethod("Method EraseFace(FvFace)")]
        public void EraseFace_FvFace()
        {
            // Arrange
            FvMesh<Euc3D.Point> heMesh = _parallelepiped.Clone() as FvMesh<Euc3D.Point>;

            FvFace<Euc3D.Point> face = heMesh.GetFace(4);

            // Act
            heMesh.EraseFace(face);

            // Assert
            Assert.AreEqual(8, heMesh.VertexCount);
            Assert.AreEqual(12, heMesh.EdgeCount);
            Assert.AreEqual(5, heMesh.FaceCount);
        }

        #endregion


        /********** Missing **********/

        // CleanMesh(Bool)
        // Clone()
        // ToHalfedgeMesh

        /********** Too quantitative **********/

        // RemoveVertex_HeVertex()
        // RemoveEdge_HeEdge()
        // EraseEdge_HeEdge()
        // RemoveFace_HeFace()
        // EraseFace_HeFace()
    }
}
