using System;
using System.Collections.Generic;


namespace BRIDGES.DataStructures.PolyhedralMeshes.Abstract
{
    /// <summary>
    /// Abstract class for a face in a polyhedral mesh data structure.
    /// </summary>
    /// <typeparam name="TPosition"> Type for the position of the vertex.</typeparam>
    /// <typeparam name="TVertex"> Type of vertex for the mesh. </typeparam>
    /// <typeparam name="TEdge"> Type of vertex for the mesh. </typeparam>
    /// <typeparam name="TFace"> Type of vertex for the mesh.</typeparam>
    public abstract class Face<TPosition, TVertex, TEdge, TFace> : IFace<TPosition>,
        IEquatable<TFace>
        where TPosition : IEquatable<TPosition>
        where TVertex : Vertex<TPosition, TVertex, TEdge, TFace>
        where TEdge : Edge<TPosition, TVertex, TEdge, TFace>
        where TFace : Face<TPosition, TVertex, TEdge, TFace>
    {
        #region Properties

        /// <inheritdoc/>
        public int Index { get; internal set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Face{TPosition, TVertex, TEdge, TFace}"/> class from its index.
        /// </summary>
        /// <param name="index"> Index of the new face in the mesh. </param>
        internal Face(int index)
        {
            Index = index;
        }

        #endregion

        #region Virtual Methods

        /******************** For this Face ********************/

        /// <inheritdoc/>
        public virtual bool Equals(TFace face)
        {
            // Same index.
            if (Index != face.Index) { return false; }

            IReadOnlyList<TVertex> thisFaceVertices = FaceVertices();
            IReadOnlyList<TVertex> otherFaceVertices = face.FaceVertices();

            // Same number of vertices
            int vertexCound = thisFaceVertices.Count;
            if (thisFaceVertices.Count != otherFaceVertices.Count) { return false; }

            // Same vertices
            for (int i_FV = 0; i_FV < vertexCound; i_FV++)
            {
                if (!thisFaceVertices[i_FV].Equals(otherFaceVertices[i_FV])) { return false; }
            }

            return true;
        }

        #endregion

        #region Abstract Methods

        /******************** On this Face ********************/

        /// <summary>
        /// Identifies the vertices around the current face.
        /// </summary>
        /// <returns> The ordered list of face vertices. </returns>
        public abstract IReadOnlyList<TVertex> FaceVertices();

        /// <summary>
        /// Identifies the edges around the current face.
        /// </summary>
        /// <returns> The ordered list of face edges. </returns>
        public abstract IReadOnlyList<TEdge> FaceEdges();

        /// <summary>
        /// Identifies the list of faces around the current face.
        /// </summary>
        /// <returns> The ordered list of faces. An empty list can be returned. </returns>
        public abstract IReadOnlyList<TFace> AdjacentFaces();


        /// <summary>
        /// Unsets all the fields of the current face.
        /// </summary>
        internal abstract void Unset();

        #endregion


        #region Override : Object

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is TFace face && Equals(face);
        }

        /// <inheritdoc cref="object.GetHashCode()"/>
        public override int GetHashCode()
        {
            return -2134847229 + Index.GetHashCode();
        }

        /// <inheritdoc cref="object.ToString()"/>
        public override string ToString()
        {
            IReadOnlyList<IVertex<TPosition>> faceVertices = FaceVertices();

            string text = $"Face {Index} comprising the vertices (";

            for (int i_FV = 0; i_FV < faceVertices.Count - 1; i_FV++)
            {
                text += faceVertices[i_FV].Index + ",";
            }
            text += faceVertices[faceVertices.Count - 1].Index + ").";

            return text;
        }

        #endregion


        #region Explicit : IFace<TPosition>

        /******************** Methods ********************/

        IReadOnlyList<IVertex<TPosition>> IFace<TPosition>.FaceVertices()
        {
            return FaceVertices();
        }

        IReadOnlyList<IEdge<TPosition>> IFace<TPosition>.FaceEdges()
        {
            return FaceEdges();
        }

        IReadOnlyList<IFace<TPosition>> IFace<TPosition>.AdjacentFaces()
        {
            return AdjacentFaces();
        }

        #endregion
    }
}
