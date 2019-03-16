// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) MMTest
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

#pragma warning disable SA1306 // Field names must begin with lower-case letter

namespace ConsoleApp1
{
    public struct DirectedEdge<T>
    {
        public long Vertex2;

        public T Metadata;
    }
}
