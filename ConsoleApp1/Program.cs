// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) MMTest
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System;
using System.IO;

namespace ConsoleApp1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var array = new MemoryMappedArray<int>(new FileInfo("ff"));
            array.Add(1);
            array.Add(12);

            var count = array.Length;

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

        private void TestSomething(Action<MemoryMappedArray<int>> action)
        {
            var file = new FileInfo("Test.mm");

            using (var array = new MemoryMappedArray<int>(file))
            {
                action(array);
            }
        }
    }
}
