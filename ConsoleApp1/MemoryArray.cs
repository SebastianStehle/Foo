// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) MMTest
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System.Collections.Generic;

namespace ConsoleApp1
{
    public sealed class MemoryArray<T> : IArray<T> where T : struct
    {
        private readonly List<T> inner = new List<T>(1024);

        public T this[int index]
        {
            get
            {
                return inner[index];
            }
            set
            {
                inner[index] = value;
            }
        }

        public long Length
        {
            get { return inner.Count; }
        }

        public void Clear()
        {
            inner.Clear();
        }

        public void Remove(long index)
        {
            inner.RemoveAt((int)index);
        }

        public void RemoveRange(long index, int count)
        {
            inner.RemoveRange((int)index, count);
        }

        public void Add(T value)
        {
            inner.Add(value);
        }

        public void Add(ref T value)
        {
            inner.Add(value);
        }

        public void AddRange(params T[] value)
        {
            inner.AddRange(value);
        }

        public void Insert(long index, T value)
        {
            inner.Insert((int)index, value);
        }

        public void Insert(long index, ref T value)
        {
            inner.Insert((int)index, value);
        }

        public void InsertRange(long index, params T[] value)
        {
            inner.InsertRange((int)index, value);
        }

        public void Set(long index, T value)
        {
            inner[(int)index] = value;
        }

        public void Set(long index, ref T value)
        {
            inner[(int)index] = value;
        }

        public T[] ToArray()
        {
            return inner.ToArray();
        }

        public bool TryGet(long index, out T value)
        {
            if (index >= 0 && index < inner.Count)
            {
                value = inner[(int)index];
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }
    }
}
