using System;
using System.IO.MemoryMappedFiles;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {

            var array = new MemoryMappedArray<int>(new System.IO.FileInfo("Foo.txt"));
            array.Add(1);
            array.Add(12);

            var count = array.Count;

            for (var i = 0; i < int.MaxValue; i++)
            {
                if (i % 1000000 == 0)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine(i);
                }

                array.Add(i);
            }

            Console.WriteLine(count);
            Console.WriteLine("Hello World!");

        }
    }
}
