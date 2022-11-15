using System;
using System.Collections.Generic;

using BRIDGES.DataStructures.PolyhedralMeshes.Abstract;


namespace BRIDGES.DataStructures.PolyhedralMeshes.HalfedgeMesh
{
    /// <summary>
    /// Class for a vertex in a polyhedral halfedge mesh data structure.
    /// </summary>
    /// <typeparam name="TPosition"> Type for the position of the vertex. </typeparam>
    public class HeVertex<TPosition> : Vertex<TPosition, HeVertex<TPosition>, HeEdge<TPosition>, HeFace<TPosition>>
        where TPosition : IEquatable<TPosition>
    {
        #region Properties

        /// <summary>
        /// An outgoing halfedge of the current vertex.
        /// </summary>
        /// <remarks> If the current vertex is on the boundary, the outgoing halfedge must be the boundary one.</remarks>
        public HeHalfedge<TPosition> OutgoingHalfedge { get; internal set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="HeVertex{TPosition}"/> class from its index and position.
        /// </summary>
        /// <param name="index"> Index of the new vertex in the mesh. </param>
        /// <param name="position"> Position of the vertex. </param>
        internal HeVertex(int index, TPosition position)
            : base(index, position)
        {
            OutgoingHalfedge = null;
        }

        #endregion

        #region Methods

        /******************** On Halfedges ********************/

        /// <summary>
        /// Identifies the halfedges whose start is the current vertex. 
        /// </summary>
        /// <returns> The list of outgoing halfedges. An empty list can be returned. </returns>
        public IReadOnlyList<HeHalfedge<TPosition>> OutgoingHalfedges()
        {
            List<HeHalfedge<TPosition>> result = new List<HeHalfedge<TPosition>>();

            // If the vertex is not connected
            if (OutgoingHalfedge is null) { return result; }


            HeHalfedge<TPosition> firstOutgoing = OutgoingHalfedge;
            result.Add(firstOutgoing);

            HeHalfedge<TPosition> outgoing = firstOutgoing.PrevHalfedge.PairHalfedge;

            while (!firstOutgoing.Equals(outgoing))
            {
                result.Add(outgoing);
                outgoing = outgoing.PrevHalfedge.PairHalfedge;
            }

            return result;
        }


        /// <summary>
        /// Identifies the halfedges whose end is the current vertex. 
        /// </summary>
        /// <returns> The list of incomming halfedges. An empty list can be returned. </returns>
        public IReadOnlyList<HeHalfedge<TPosition>> IncomingHalfedges()
        {
            List<HeHalfedge<TPosition>> result = new List<HeHalfedge<TPosition>>();

            // If the vertex is not connected
            if (OutgoingHalfedge is null) { return result; }

            HeHalfedge<TPosition> firstIncoming = OutgoingHalfedge.PairHalfedge;
            result.Add(firstIncoming);

            HeHalfedge<TPosition> incoming = firstIncoming.PairHalfedge.PrevHalfedge;

            while (!firstIncoming.Equals(incoming))
            {
                result.Add(incoming);
                incoming = incoming.PairHalfedge.PrevHalfedge;
            }
            return result;
        }

        #endregion


        #region Override : Object

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is HeVertex<TPosition> vertex && Equals(vertex);
        }

        /// <inheritdoc cref="object.GetHashCode()"/>
        public override int GetHashCode()
        {
            int hashCode = 2096401715;
            hashCode = hashCode * -1521134295 + Index.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<TPosition>.Default.GetHashCode(Position);
            return hashCode;
        }

        /// <inheritdoc cref="object.ToString()"/>
        public override string ToString()
        {
            return $"HeVertex {Index} at {Position}.";
        }

        #endregion

        #region Override : Vertex<T,HeVertex<T>,HeEdge<T>,HeFace<T>>

        /******************** For this Vertex ********************/

        /// <inheritdoc/>
        public override bool IsBoundary()
        {
            if (OutgoingHalfedge is null) { return true; }

            else if (OutgoingHalfedge.IsBoundary()) { return true; }

            else { return false; }
        }

        /// <inheritdoc/>
        public override bool IsConnected()
        {
            return !(OutgoingHalfedge is null);
        }

        /// <inheritdoc/>
        public override int Valence()
        {
            return OutgoingHalfedges().Count;
        }


        /// <inheritdoc/>
        internal override void Unset()
        {
            // Instanciate properties
            OutgoingHalfedge = null;

            // Initialise properties
            Index = -1;
            Position = default;
        }


        /******************** On Vertices ********************/

        /// <inheritdoc/>
        public override IReadOnlyList<HeVertex<TPosition>> NeighbourVertices()
        {
            IReadOnlyList<HeHalfedge<TPosition>> outgoings = OutgoingHalfedges();

            int edgeValency = outgoings.Count;
            HeVertex<TPosition>[] result = new HeVertex<TPosition>[edgeValency];

            for (int i_OHe = 0; i_OHe < edgeValency; i_OHe++)
            {
                result[i_OHe] = outgoings[i_OHe].EndVertex;
            }

            return result;
        }


        /******************** On Edges ********************/

        /// <inheritdoc/>
        public override IReadOnlyList<HeEdge<TPosition>> ConnectedEdges()
        {
            List<HeEdge<TPosition>> result = new List<HeEdge<TPosition>>();

            // If the vertex is not connected
            if (OutgoingHalfedge is null) { return result; }


            HeHalfedge<TPosition> firstOutgoing = OutgoingHalfedge;
            HeEdge<TPosition> firstEdge = firstOutgoing.GetEdge();
            result.Add(firstEdge);

            HeHalfedge<TPosition> outgoing = firstOutgoing.PrevHalfedge.PairHalfedge;

            while (!firstOutgoing.Equals(outgoing))
            {
                HeEdge<TPosition> edge = outgoing.GetEdge();

                result.Add(edge);
                outgoing = outgoing.PrevHalfedge.PairHalfedge;
            }

            return result;
        }


        /******************** On Faces ********************/

        /// <inheritdoc/>
        public override IReadOnlyList<HeFace<TPosition>> AdjacentFaces()
        {
            IReadOnlyList<HeHalfedge<TPosition>> outgoings = OutgoingHalfedges();

            int edgeValency = outgoings.Count;
            List<HeFace<TPosition>> result = new List<HeFace<TPosition>>(edgeValency);

            for (int i_OHe = 0; i_OHe < edgeValency; i_OHe++)
            {
                HeHalfedge<TPosition> outgoing = outgoings[i_OHe];

                if (!(outgoing.AdjacentFace is null)) { result.Add(outgoing.AdjacentFace); }
            }

            return result;
        }

        #endregion
    }
}
