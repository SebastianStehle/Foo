// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) MMTest
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using ConsoleApp1;

namespace XUnitTestProject1
{
    public class DirectedGraphTest
    {
        private readonly DirectedGraph<MemoryArray<DirectedVertex<Empty>>, Empty, MemoryArray<DirectedEdge<Empty>>, Empty> sut;

        public DirectedGraphTest()
        {
            sut = new DirectedGraph<MemoryArray<DirectedVertex<Empty>>, Empty, MemoryArray<DirectedEdge<Empty>>, Empty>(
                new MemoryArray<DirectedVertex<Empty>>(),
                new MemoryArray<DirectedEdge<Empty>>());
        }
    }
}
