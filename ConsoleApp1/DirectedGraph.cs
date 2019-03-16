// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) MMTest
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

#pragma warning disable SA1306 // Field names must begin with lower-case letter

using System;

namespace ConsoleApp1
{
    public sealed class DirectedGraph<TVertexArray, TVertex, TEdgeArray, TEdge>
        where TVertex : struct
        where TVertexArray : IArray<DirectedVertex<TVertex>>
        where TEdge : struct
        where TEdgeArray : IArray<DirectedEdge<TEdge>>
    {
        private readonly TVertexArray vertices;
        private readonly TEdgeArray edges;
        private DirectedVertex<TVertex> vertex;
        private DirectedEdge<TEdge> edge;

        public DirectedGraph(TVertexArray vertices, TEdgeArray edges)
        {
            this.vertices = vertices;

            this.edges = edges;
        }

        public long AddVertex(ref TVertex data)
        {
            vertices.Add(new DirectedVertex<TVertex> { Metadata = data });

            return vertices.Length - 1;
        }

        public void AddEdge(long v1, long v2, ref TEdge data)
        {
            if (!vertices.TryGet(v2, out _) && !vertices.TryGet(v1, out vertex))
            {
                throw new IndexOutOfRangeException();
            }

            if (vertex.EdgeCount == 0)
            {
                edge.Metadata = data;
                edge.Vertex2 = v2;

                edges.Add(ref edge);

                vertex.EdgeCount++;
                vertex.EdgeFirst = edges.Length - 1;

                vertices.Set(v1, ref vertex);
            }
            else
            {
                for (var i = 0; i < vertex.EdgeCount; i++)
                {
                    var index = i + vertex.EdgeFirst;

                    if (edges.TryGet(i, out edge) && edge.Vertex2 == v2)
                    {
                        edge.Metadata = data;
                        edges.Set(index, edge);

                        return;
                    }
                }

                edge.Metadata = data;
                edge.Vertex2 = v2;

                edges.Insert(vertex.EdgeFirst + vertex.EdgeCount, ref edge);

                vertex.EdgeCount++;

                vertices.Set(v1, ref vertex);

                for (var i = vertex.EdgeFirst + vertex.EdgeCount; i < edges.Length;)
                {
                    vertices.TryGet(i, out vertex);
                    vertex.EdgeFirst++;
                    vertices.Set(i, vertex);

                    i += vertex.EdgeCount;
                }
            }
        }
    }
}
