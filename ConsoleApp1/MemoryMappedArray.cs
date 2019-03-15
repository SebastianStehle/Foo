using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace ConsoleApp1
{
    public struct MemoryMappedArray<T> where T : struct
    {
        private const int InitialCapacity = 1024 * 1024;
        private static readonly long ElementSize = Marshal.SizeOf<T>();
        private static readonly long Offset = sizeof(long);
        private MemoryMappedViewAccessor mappedView;
        private MemoryMappedFile mappedFile;
        private readonly FileInfo file;
        private long count;
        private long lastPointer;

        public T this[int index]
        {
            get
            {
                mappedView.Read(GetPosition(index), out T result);

                return result;
            }
            set
            {
                mappedView.Write(index * ElementSize, ref value);
            }
        }

        public long Count
        {
            get { return count; }
        }

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
                mappedFile = MemoryMappedFile.CreateFromFile(file.FullName, FileMode.OpenOrCreate, "Map", InitialCapacity);
                mappedView = mappedFile.CreateViewAccessor();
            }

            count = mappedView.ReadInt64(0);

            lastPointer = GetPosition(count);
        }

        public void Add(T value)
        {
            Add(ref value);
        }

        public void Add(ref T value)
        {
            var newSize = lastPointer + ElementSize;

            if (newSize > mappedView.Capacity)
            {
                var capacity = mappedView.Capacity;

                mappedFile.Dispose();
                mappedView.Dispose();

                mappedFile = MemoryMappedFile.CreateFromFile(file.FullName, FileMode.OpenOrCreate, "Map", capacity * 2);
                mappedView = mappedFile.CreateViewAccessor();
            }

            mappedView.Write(lastPointer, ref value);

            IncrementCount();
        }

        private void IncrementCount()
        {
            count++;

            lastPointer = GetPosition(count);

            mappedView.Write(0, count);
        }

        private static long GetPosition(long index)
        {
            return index * ElementSize + Offset;
        }
    }
}
