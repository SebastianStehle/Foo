// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) MMTest
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

namespace ConsoleApp1
{
    public interface IGraph<TVertex, TEdge> where TVertex : struct where TEdge : struct
    {
        void AddEdge(long v1, long v2, ref TEdge data);

        long AddVertex(ref TVertex data);

        bool TryRemoveEdge(long v1, long v2);

        bool TryRemoveVertex(long id);

        bool TryGetEdge(long v1, long v2, out TEdge data);

        bool TryGetVertex(long v, out TVertex data);
    }
}