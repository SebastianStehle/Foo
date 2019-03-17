// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) MMTest
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

namespace ConsoleApp1
{
    public sealed partial class DirectedGraph<TVertexArray, TVertex, TEdgeArray, TEdge>
        where TVertex : struct
        where TVertexArray : IArray<GraphVertex<TVertex>>
        where TEdge : struct
        where TEdgeArray : IArray<GraphEdge<TEdge>>
    {
        internal sealed class EdgeEnumerator : IGraphEdgeEnumerator<TEdge>
        {
            private readonly DirectedGraph<TVertexArray, TVertex, TEdgeArray, TEdge> graph;
            private readonly GraphEdge<TEdge>[] edges = new GraphEdge<TEdge>[10];
            private readonly GraphEdge<TEdge>[] edgesLong;
            private int index = -1;
            private GraphVertex<TVertex> vertex;

            public EdgeEnumerator(DirectedGraph<TVertexArray, TVertex, TEdgeArray, TEdge> graph)
            {
                this.graph = graph;
            }

            public bool MoveNext(ref GraphEdge<TEdge> edge)
            {
                var array = edgesLong ?? edges;

                if (index < vertex.EdgeLength)
                {
                    if (graph.edges.TryGet(vertex.EdgeOffset + index, out edge))
                    {
                        index++;
                        return true;
                    }
                }

                return false;
            }

            public bool MoveTo(long id)
            {
                index = -1;

                return graph.vertices.TryGet(id, out vertex);
            }
        }
    }
}
