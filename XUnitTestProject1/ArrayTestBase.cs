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
    public abstract class ArrayTestBase
    {
        protected IArray<int> Sut { get; set; }

        [Fact]
        public void Should_add_items()
        {
            var added = 3;

            Sut.Add(1);
            Sut.Add(2);
            Sut.Add(ref added);
            Sut.AddRange(4, 5);

            Assert.Equal(5, Sut.Length);
            Assert.Equal(new[] { 1, 2, 3, 4, 5 }, Sut.ToArray());
        }

        [Fact]
        public void Should_insert_items()
        {
            var inserted = 2;

            Sut.Add(1);
            Sut.Add(6);
            Sut.Insert(1, ref inserted);
            Sut.Insert(2, 3);
            Sut.InsertRange(3, 4, 5);

            Assert.Equal(6, Sut.Length);
            Assert.Equal(new[] { 1, 2, 3, 4, 5, 6 }, Sut.ToArray());
        }

        [Fact]
        public void Should_replace_item()
        {
            var replaced = 7;

            Sut.Add(1);
            Sut.Add(2);
            Sut.Add(3);

            Sut[0] = 9;
            Sut.Set(1, 8);
            Sut.Set(2, ref replaced);

            Assert.Equal(3, Sut.Length);
            Assert.Equal(new[] { 9, 8, 7 }, Sut.ToArray());
        }

        [Fact]
        public void Should_remove_last_item()
        {
            Sut.Add(1);
            Sut.Add(2);
            Sut.Add(3);
            Sut.Add(4);
            Sut.Remove(3);
            Sut.Remove(2);

            Assert.Equal(2, Sut.Length);
            Assert.Equal(new[] { 1, 2 }, Sut.ToArray());
        }

        [Fact]
        public void Should_remove_items()
        {
            Sut.Add(1);
            Sut.Add(2);
            Sut.Add(3);
            Sut.Add(4);
            Sut.Remove(1);
            Sut.Remove(1);

            Assert.Equal(2, Sut.Length);
            Assert.Equal(new[] { 1, 4 }, Sut.ToArray());
        }

        [Fact]
        public void Should_remove_item_range()
        {
            Sut.Add(1);
            Sut.Add(2);
            Sut.Add(3);
            Sut.Add(4);
            Sut.RemoveRange(1, 2);

            Assert.Equal(2, Sut.Length);
            Assert.Equal(new[] { 1, 4 }, Sut.ToArray());
        }

        [Fact]
        public void Should_clear_array()
        {
            Sut.Add(1);
            Sut.Add(2);
            Sut.Add(3);
            Sut.Add(4);
            Sut.Clear();

            Assert.Equal(0, Sut.Length);
            Assert.Empty(Sut.ToArray());
        }

        [Fact]
        public void Should_get_item_by_index()
        {
            Sut.Add(1);
            Sut.Add(2);
            Sut.Add(3);

            Assert.Equal(1, Sut[0]);
            Assert.Equal(2, Sut[1]);
            Assert.Equal(3, Sut[2]);
        }

        [Fact]
        public void Should_get_item_by_indexer()
        {
            Sut.Add(1);
            Sut.Add(2);
            Sut.Add(3);

            Assert.False(Sut.TryGet(-1, out _));
            Assert.False(Sut.TryGet(+3, out _));

            Assert.True(Sut.TryGet(0, out var s0));
            Assert.True(Sut.TryGet(1, out var s1));
            Assert.True(Sut.TryGet(2, out var s2));

            Assert.Equal(1, s0);
            Assert.Equal(2, s1);
            Assert.Equal(3, s2);
        }

        [Fact]
        public void Should_get_item_by_range()
        {
            var result = new int[2];

            Sut.Add(1);
            Sut.Add(2);
            Sut.Add(3);
            Sut.Add(4);

            Assert.False(Sut.TryGetRange(-1, 2, result));
            Assert.False(Sut.TryGetRange(+3, 2, result));

            Assert.True(Sut.TryGetRange(0, 2, result));
            Assert.Equal(new[] { 1, 2 }, result);

            Assert.True(Sut.TryGetRange(1, 2, result));
            Assert.Equal(new[] { 2, 3 }, result);

            Assert.True(Sut.TryGetRange(2, 2, result));
            Assert.Equal(new[] { 3, 4 }, result);
        }
    }
}