// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) MMTest
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

namespace ConsoleApp1
{
    public interface IArray<T> where T : struct
    {
        T this[int index] { get; set; }

        long Length { get; }

        void Clear();

        void Remove(long index);

        void RemoveRange(long index, int count);

        void Add(T value);

        void Add(ref T value);

        void AddRange(params T[] value);

        void Insert(long index, T value);

        void Insert(long index, ref T value);

        void InsertRange(long index, params T[] value);

        void Set(long index, T value);

        void Set(long index, ref T value);

        T[] ToArray();

        bool TryGet(long index, out T value);
    }
}