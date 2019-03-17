// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) MMTest
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using ConsoleApp1;
using Xunit;

namespace XUnitTestProject1
{
    public class DirectedGraphTest
    {
        private readonly DirectedGraph<MemoryArray<DirectedVertex<Empty>>, Empty, MemoryArray<DirectedEdge<Empty>>, Empty> sut;
        private Empty empty;

        public DirectedGraphTest()
        {
            sut = new DirectedGraph<MemoryArray<DirectedVertex<Empty>>, Empty, MemoryArray<DirectedEdge<Empty>>, Empty>(
                new MemoryArray<DirectedVertex<Empty>>(),
                new MemoryArray<DirectedEdge<Empty>>());
        }

        [Fact]
        public void Should_add_vertices()
        {
            var v1 = sut.AddVertex(ref empty);
            var v2 = sut.AddVertex(ref empty);

            Assert.Equal(0, v1);
            Assert.Equal(1, v2);

            Assert.True(sut.TryGetVertex(0, out _));
            Assert.True(sut.TryGetVertex(1, out _));

            Assert.False(sut.TryGetVertex(2, out _));
        }

        [Fact]
        public void Should_add_edges()
        {
            var v1 = sut.AddVertex(ref empty);
            var v2 = sut.AddVertex(ref empty);
            var v3 = sut.AddVertex(ref empty);

            sut.AddEdge(v1, v2, ref empty);
            sut.AddEdge(v2, v3, ref empty);
            sut.AddEdge(v1, v3, ref empty);
        }
    }
}
