// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) MMTest
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

namespace ConsoleApp1
{
    public interface IGraphEdgeEnumerator<TEdge> where TEdge : struct
    {
        bool MoveTo(long id);

        bool MoveNext(out GraphEdge<TEdge> edge);
    }
}
