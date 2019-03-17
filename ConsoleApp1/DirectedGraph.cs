// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) MMTest
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System;

namespace ConsoleApp1
{
    public sealed partial class DirectedGraph<TVertexArray, TVertex, TEdgeArray, TEdge> : IGraph<TVertex, TEdge>
        where TVertex : struct
        where TVertexArray : IArray<GraphVertex<TVertex>>
        where TEdge : struct
        where TEdgeArray : IArray<GraphEdge<TEdge>>
    {
        private const long VertexDeleted = -1;
        private readonly TVertexArray vertices;
        private readonly TEdgeArray edges;

        public DirectedGraph(TVertexArray vertices, TEdgeArray edges)
        {
            this.vertices = vertices;

            this.edges = edges;
        }

        public long AddVertex(ref TVertex data)
        {
            vertices.Add(new GraphVertex<TVertex> { Value = data });

            return vertices.Length - 1;
        }

        public void AddEdge(long v1, long v2, ref TEdge data)
        {
            if (!vertices.TryGet(v2, out _) || !vertices.TryGet(v1, out var vertex))
            {
                throw new IndexOutOfRangeException();
            }

            var edge = default(GraphEdge<TEdge>);

            if (vertex.EdgeLength == 0)
            {
                edge.Value = data;
                edge.Vertex1 = v1;
                edge.Vertex2 = v2;
                edges.Add(ref edge);

                vertex.EdgeLength++;
                vertex.EdgeOffset = edges.Length - 1;
                vertices.Set(v1, ref vertex);
            }
            else
            {
                for (var i = 0; i < vertex.EdgeLength; i++)
                {
                    var index = i + vertex.EdgeOffset;

                    if (edges.TryGet(i, out edge) && edge.Vertex2 == v2)
                    {
                        edge.Value = data;
                        edges.Set(index, edge);

                        return;
                    }
                }

                edge.Value = data;
                edge.Vertex1 = v1;
                edge.Vertex2 = v2;
                edges.Insert(vertex.EdgeOffset + vertex.EdgeLength, ref edge);

                vertex.EdgeLength++;
                vertices.Set(v1, ref vertex);

                UpdateOffsetsAfter(v1, ref vertex, -1);
            }
        }

        public bool TryRemoveVertex(long id)
        {
            if (vertices.TryGet(id, out var vertex) && vertex.EdgeOffset != VertexDeleted)
            {
                edges.RemoveRange(vertex.EdgeOffset, (int)vertex.EdgeLength);

                vertex.EdgeOffset = VertexDeleted;
                vertices.Set(id, vertex);

                return true;
            }

            return false;
        }

        public bool TryRemoveEdge(long v1, long v2)
        {
            if (vertices.TryGet(v2, out _) && vertices.TryGet(v1, out var vertex))
            {
                var edge = default(GraphEdge<TEdge>);

                for (var i = 0; i < vertex.EdgeLength; i++)
                {
                    var index = i + vertex.EdgeOffset;

                    if (edges.TryGet(i, out edge) && edge.Vertex2 == v2)
                    {
                        edges.Remove(index);

                        vertex.EdgeLength--;
                        vertices.Set(v1, vertex);

                        UpdateOffsetsAfter(v1, ref vertex, -1);

                        return true;
                    }
                }
            }

            return false;
        }

        public bool TryGetVertex(long v, out TVertex data)
        {
            if (vertices.TryGet(v, out var vertex))
            {
                data = vertex.Value;
                return true;
            }

            data = default(TVertex);

            return false;
        }

        public bool TryGetEdge(long v1, long v2, out TEdge data)
        {
            if (vertices.TryGet(v2, out _) && vertices.TryGet(v1, out var vertex))
            {
                var edge = default(GraphEdge<TEdge>);

                for (var i = 0; i < vertex.EdgeLength; i++)
                {
                    var index = i + vertex.EdgeOffset;

                    if (edges.TryGet(i, out edge) && edge.Vertex2 == v2)
                    {
                        data = edge.Value;

                        return true;
                    }
                }
            }

            data = default(TEdge);

            return false;
        }

        private void UpdateOffsetsAfter(long id, ref GraphVertex<TVertex> vertex, int offset)
        {
            for (var i = vertex.EdgeOffset + vertex.EdgeLength; i < edges.Length;)
            {
                vertices.TryGet(id, out vertex);
                vertex.EdgeOffset += offset;
                vertices.Set(id, vertex);

                i += vertex.EdgeLength;
            }
        }
    }
}
