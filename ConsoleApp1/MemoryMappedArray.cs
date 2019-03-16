// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) MMTest
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

#pragma warning disable RECS0108 // Warns about static fields in generic types

namespace ConsoleApp1
{
    public sealed class MemoryMappedArray<T> : IDisposable, IArray<T> where T : struct
    {
        private const int CopySize = 1024 * 1024;
        private const int BucketCapacity = 4 * 1024;
        private static readonly int ItemSize = Marshal.SizeOf<T>();
        private static readonly int Offset = sizeof(long);
        private readonly FileInfo file;
        private MemoryMappedViewAccessor mappedView;
        private MemoryMappedFile mappedFile;
        private long length;
        private long lastPointer;

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= length)
                {
                    throw new IndexOutOfRangeException();
                }

                mappedView.Read(GetPosition(index), out T result);

                return result;
            }
            set
            {
                Set(index, ref value);
            }
        }

        public long Length
        {
            get { return length; }
        }

        public FileInfo File => file;

        public MemoryMappedArray(FileInfo file)
        {
            this.file = file;

            if (file.Exists)
            {
                mappedFile = MemoryMappedFile.CreateFromFile(file.FullName, FileMode.OpenOrCreate);
                mappedView = mappedFile.CreateViewAccessor();
            }
            else
            {
                mappedFile = MemoryMappedFile.CreateFromFile(file.FullName, FileMode.OpenOrCreate, file.Name, CopySize);
                mappedView = mappedFile.CreateViewAccessor();
            }

            length = mappedView.ReadInt64(0);

            lastPointer = GetPosition(length);
        }

        public void Dispose()
        {
            mappedView?.Dispose();
            mappedView = null;

            mappedFile?.Dispose();
            mappedFile = null;
        }

        public void Clear()
        {
            length = 0;
            lastPointer = GetPosition(length);

            mappedView.Write(0, length);
        }

        public bool TryGet(long index, out T value)
        {
            if (index >= 0 && index < length)
            {
                mappedView.Read(GetPosition(index), out value);
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }

        public void Add(T value)
        {
            Add(ref value);
        }

        public void Add(ref T value)
        {
            EnsureSize(lastPointer + ItemSize);

            mappedView.Write(lastPointer, ref value);

            IncrementLength();
        }

        public void AddRange(params T[] value)
        {
            EnsureSize(lastPointer + (value.Length * ItemSize));

            mappedView.WriteArray(lastPointer, value, 0, value.Length);

            IncrementLength(value.Length);
        }

        public void Set(long index, T value)
        {
            Set(index, ref value);
        }

        public void Set(long index, ref T value)
        {
            if (index < 0 || index >= length)
            {
                throw new IndexOutOfRangeException();
            }

            mappedView.Write(GetPosition(index), ref value);
        }

        public void Insert(long index, T value)
        {
            Insert(index, ref value);
        }

        public void Insert(long index, ref T value)
        {
            if (index < 0 || index > length)
            {
                throw new IndexOutOfRangeException();
            }

            if (index == length)
            {
                Add(ref value);
            }
            else
            {
                Shift(index, 1);

                mappedView.Write(GetPosition(index), ref value);

                IncrementLength();
            }
        }

        public void InsertRange(long index, params T[] value)
        {
            if (index < 0 || index > length)
            {
                throw new IndexOutOfRangeException();
            }

            if (index == length)
            {
                AddRange(value);
            }
            else
            {
                Shift(index, value.Length);

                mappedView.WriteArray(GetPosition(index), value, 0, value.Length);

                IncrementLength(value.Length);
            }
        }

        public void Remove(long index)
        {
            RemoveRange(index, 1);
        }

        public void RemoveRange(long index, int count)
        {
            if (index < 0 || index >= length || (index + count) > length)
            {
                throw new IndexOutOfRangeException();
            }

            if (index + count < length)
            {
                Shift(index + count, -count);
            }

            DecrementLength(count);
        }

        private void Shift(long index, long offset)
        {
            EnsureSize(lastPointer + (offset * ItemSize));

            var itemsToShift = length - index;
            var itemsOffset = offset * ItemSize;
            var bucketCapacity = CopySize / ItemSize;
            var buckets = (int)Math.Ceiling((double)itemsToShift / bucketCapacity);

            var temp = new T[bucketCapacity];

            if (offset > 0)
            {
                for (var bucket = buckets - 1; bucket >= 0; bucket--)
                {
                    var bucketIndex = (bucket * bucketCapacity) + index;
                    var bucketOffset = GetPosition(bucketIndex);
                    var bucketSize = (int)Math.Min(bucketCapacity, length - bucketIndex);

                    mappedView.ReadArray(bucketOffset, temp, 0, bucketSize);
                    mappedView.WriteArray(bucketOffset + itemsOffset, temp, 0, bucketSize);
                }
            }
            else
            {
                for (var bucket = 0; bucket < buckets; bucket++)
                {
                    var bucketIndex = (bucket * bucketCapacity) + index;
                    var bucketOffset = GetPosition(bucketIndex);
                    var bucketSize = (int)Math.Min(bucketCapacity, length - bucketIndex);

                    mappedView.ReadArray(bucketOffset, temp, 0, bucketSize);
                    mappedView.WriteArray(bucketOffset + itemsOffset, temp, 0, bucketSize);
                }
            }
        }

        public T[] ToArray()
        {
            var result = new T[length];

            mappedView.ReadArray(Offset, result, 0, (int)length);

            return result;
        }

        private void IncrementLength(int d = 1)
        {
            length += d;
            lastPointer = GetPosition(length);

            mappedView.Write(0, length);
        }

        private void DecrementLength(int d = 1)
        {
            length -= d;
            lastPointer = GetPosition(length);

            mappedView.Write(0, length);
        }

        private static long GetPosition(long index)
        {
            return (index * ItemSize) + Offset;
        }

        private void EnsureSize(long newSize)
        {
            if (newSize > mappedView.Capacity)
            {
                var capacity = mappedView.Capacity;

                mappedFile.Dispose();
                mappedView.Dispose();

                mappedFile = MemoryMappedFile.CreateFromFile(File.FullName, FileMode.OpenOrCreate, file.Name, capacity * 2);
                mappedView = mappedFile.CreateViewAccessor();
            }
        }
    }
}
