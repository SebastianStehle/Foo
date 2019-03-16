// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) MMTest
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

#pragma warning disable SA1649 // File name must match first type name

namespace ConsoleApp1
{
    public static class DirectedGraph
    {
        public static DirectedGraph<TVertexArray, TVertex, TEdgeArray, TEdge> Create<TVertexArray, TVertex, TEdgeArray, TEdge>(
                TVertexArray vertices, TEdgeArray edges)
            where TVertex : struct
            where TVertexArray : IArray<DirectedVertex<TVertex>>
            where TEdge : struct
            where TEdgeArray : IArray<DirectedEdge<TEdge>>
        {
            return new DirectedGraph<TVertexArray, TVertex, TEdgeArray, TEdge>(vertices, edges);
        }
    }
}
