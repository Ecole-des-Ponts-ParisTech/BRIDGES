using System;
using System.Collections.Generic;

using BRIDGES.DataStructures.PolyhedralMeshes.Abstract;


namespace BRIDGES.DataStructures.PolyhedralMeshes.HalfedgeMesh
{
    /// <summary>
    /// Class for a face in a polyhedral halfedge mesh data structure.
    /// </summary>
    /// <typeparam name="TPosition"> Type for the position of the vertex. </typeparam>
    public class HeFace<TPosition> : Face<TPosition, HeVertex<TPosition>, HeEdge<TPosition>, HeFace<TPosition>>
        where TPosition : IEquatable<TPosition>
    {
        #region Properties

        /// <summary>
        /// Gets a halfedge around the current face.
        /// </summary>
        /// <remarks> This shall never be <see langword="null"/>. </remarks>
        public HeHalfedge<TPosition> FirstHalfedge { get; internal set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="HeFace{TPosition}"/> class.
        /// </summary>
        /// <param name="index"> Index of the face in the mesh. </param>
        /// <param name="firstHalfedge"> First halfedge belonging the new face. </param>
        internal HeFace(int index, HeHalfedge<TPosition> firstHalfedge)
            : base(index)
        {
            FirstHalfedge = firstHalfedge;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="HeFace{TPosition}"/> class.
        /// </summary>
        /// <param name="index"> Index of the face in the mesh. </param>
        internal HeFace(int index)
            : base(index)
        {
            this.FirstHalfedge = null;
        }


        #endregion

        #region Methods

        /******************** On Halfedges ********************/

        /// <summary>
        /// Identifies the halfedges around the current face.
        /// </summary>
        /// <returns> The ordered list of face halfedges. </returns>
        public IReadOnlyList<HeHalfedge<TPosition>> FaceHalfedges()
        {
            List<HeHalfedge<TPosition>> result = new List<HeHalfedge<TPosition>>();

            result.Add(FirstHalfedge);

            HeHalfedge<TPosition> halfedge = FirstHalfedge.NextHalfedge;
            while (!FirstHalfedge.Equals(halfedge))
            {
                result.Add(halfedge);
                halfedge = halfedge.NextHalfedge;
            }

            return result;
        }

        #endregion


        #region Override : Object

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is HeFace<TPosition> face ? this.Equals(face) : false;
        }

        /// <inheritdoc cref="object.GetHashCode()"/>
        public override int GetHashCode()
        {
            int hashCode = 1407940046;
            hashCode = hashCode * -1521134295 + Index.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<HeHalfedge<TPosition>>.Default.GetHashCode(FirstHalfedge);
            return hashCode;
        }

        /// <inheritdoc cref="object.ToString()"/>
        public override string ToString()
        {
            IReadOnlyList<IVertex<TPosition>> faceVertices = FaceVertices();

            string text = $"HeFace {Index} comprising the vertices (";

            for (int i_FV = 0; i_FV < faceVertices.Count - 1; i_FV++)
            {
                text += faceVertices[i_FV].Index + ",";
            }
            text += faceVertices[faceVertices.Count - 1].Index + ").";

            return text;
        }

        #endregion

        #region Override : Face<T,HeVertex<T>,HeEdge<T>,HeFace<T>>

        /******************** For this Face ********************/

        /// <summary>
        /// Unsets all the fields of the current face.
        /// </summary>
        internal override void Unset()
        {
            // Unset Properties
            Index = -1;
            FirstHalfedge = null;
        }


        /// <inheritdoc/>
        public override bool Equals(HeFace<TPosition> face)
        {
            return Index == face.Index
                && FirstHalfedge == face.FirstHalfedge;
        }


        /******************** On Vertices ********************/

        /// <inheritdoc/>
        public override IReadOnlyList<HeVertex<TPosition>> FaceVertices()
        {
            List<HeVertex<TPosition>> result = new List<HeVertex<TPosition>>();

            HeHalfedge<TPosition> firstHalfedge = FirstHalfedge;
            result.Add(firstHalfedge.StartVertex);

            HeHalfedge<TPosition> halfedge = firstHalfedge.NextHalfedge;
            while (!firstHalfedge.Equals(halfedge))
            {
                result.Add(halfedge.StartVertex);
                halfedge = halfedge.NextHalfedge;
            }

            return result;
        }


        /******************** On Edges ********************/

        /// <inheritdoc/>
        public override IReadOnlyList<HeEdge<TPosition>> FaceEdges()
        {
            List<HeEdge<TPosition>> result = new List<HeEdge<TPosition>>();

            HeHalfedge<TPosition> firstHalfedge = FirstHalfedge;
            HeEdge<TPosition> firstEdge = firstHalfedge.GetEdge();
            result.Add(firstEdge);

            HeHalfedge<TPosition> halfedge = firstHalfedge.NextHalfedge;

            while (!firstHalfedge.Equals(halfedge))
            {
                HeEdge<TPosition> edge = halfedge.GetEdge();

                result.Add(edge);
                halfedge = halfedge.NextHalfedge;
            }

            return result;
        }


        /******************** On Faces ********************/

        /// <inheritdoc/>
        public override IReadOnlyList<HeFace<TPosition>> AdjacentFaces()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
