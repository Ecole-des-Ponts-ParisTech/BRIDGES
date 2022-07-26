using System;
using System.Collections.Generic;

using BRIDGES.DataStructures.PolyhedralMeshes.Abstract;


namespace BRIDGES.DataStructures.PolyhedralMeshes.FaceVertexMesh
{
    /// <summary>
    /// Class for a face in a polyhedral face-vertex mesh data structure.
    /// </summary>
    /// <typeparam name="TPosition"> Type for the position of the vertex. </typeparam>
    public class FvFace<TPosition> : Face<TPosition, FvVertex<TPosition>, FvEdge<TPosition>, FvFace<TPosition>>
        where TPosition : IEquatable<TPosition>
    {
        #region Fields

        /// <summary>
        /// Ordered list of the <see cref="FvVertex{TPosition}"/> around the current <see cref="FvFace{TPosition}"/>.
        /// </summary>
        /// <remarks> This should never return an empty list. </remarks>
        internal List<FvVertex<TPosition>> _faceVertices;

        /// <summary>
        /// Ordered list of the <see cref="FvEdge{TPosition}"/> bounding the current <see cref="FvFace{TPosition}"/>.
        /// </summary>
        /// <remarks> This should never return an empty list. <br/>
        /// This is not necessary in the face-vertex mesh data structure, but it allows simplifies and speeds up methods. </remarks>
        internal List<FvEdge<TPosition>> _faceEdges;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="FvFace{TVector}"/> class.
        /// </summary>
        /// <param name="index"> Index of the added edge in the mesh. </param>
        /// <param name="faceVertices"> Ordered list of face's vertex. </param>
        /// <param name="faceEdges">  Ordered list of face's edges. </param>
        internal FvFace(int index, List<FvVertex<TPosition>> faceVertices, List<FvEdge<TPosition>> faceEdges)
            : base(index)
        {
            // Initialise fields
            _faceVertices = faceVertices;
            _faceEdges = faceEdges;
        }

        #endregion


        #region Override : Object

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is FvFace<TPosition> face && Equals(face);
        }

        /// <inheritdoc cref="object.GetHashCode()"/>
        public override int GetHashCode()
        {
            int hashCode = 1104277913;
            hashCode = hashCode * -1521134295 + Index.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<FvVertex<TPosition>>>.Default.GetHashCode(_faceVertices);
            return hashCode;
        }

        /// <inheritdoc cref="object.ToString()"/>
        public override string ToString()
        {
            IReadOnlyList<IVertex<TPosition>> faceVertices = FaceVertices();

            string text = $"FvFace {Index} comprising the vertices (";

            for (int i_FV = 0; i_FV < faceVertices.Count - 1; i_FV++)
            {
                text += faceVertices[i_FV].Index + ",";
            }
            text += faceVertices[faceVertices.Count - 1].Index + ").";

            return text;
        }

        #endregion

        #region Override : Face<T,FvVertex<T>,FvEdge<T>,FvFace<T>>

        /******************** Methods - On this Face ********************/

        /// <inheritdoc/>
        internal override void Unset()
        {
            // Unset Fields
            _faceVertices = null;

            // Unset Properties
            Index = -1;
        }


        /******************** Methods - On Vertices ********************/

        /// <inheritdoc/>
        public override IReadOnlyList<FvVertex<TPosition>> FaceVertices()
        {
            return _faceVertices;
        }


        /******************** Methods - On Edges ********************/

        /// <inheritdoc/>
        public override IReadOnlyList<FvEdge<TPosition>> FaceEdges()
        {
            return _faceEdges;
        }


        /******************** Methods - On Faces ********************/

        /// <inheritdoc/>
        public override IReadOnlyList<FvFace<TPosition>> AdjacentFaces()
        {
            int edgeCount = _faceEdges.Count;

            List<FvFace<TPosition>> result = new List<FvFace<TPosition>>(edgeCount);

            for (int i_FE = 0; i_FE < edgeCount; i_FE++)
            {
                FvEdge<TPosition> edge = _faceEdges[i_FE];

                IReadOnlyList<FvFace<TPosition>> edgeFaces = edge.AdjacentFaces();
                for (int i_EF = 0; i_EF < edgeFaces.Count; i_EF++)
                {
                    FvFace<TPosition> edgeFace = edgeFaces[i_EF];

                    if (edgeFace.Equals(this)) { continue; }
                    else if (!result.Contains(edgeFace)) { result.Add(edgeFace); }
                }
            }

            return result;
        }

        #endregion
    }
}
