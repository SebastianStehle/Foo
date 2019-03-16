// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) MMTest
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System;
using System.IO;
using ConsoleApp1;

namespace XUnitTestProject1
{
    public class MemoryMappedArrayTest : ArrayTestBase, IDisposable
    {
        private readonly FileInfo fileInfo = new FileInfo(Guid.NewGuid().ToString());
        private readonly MemoryMappedArray<int> array;

        public MemoryMappedArrayTest()
        {
            Sut = array = new MemoryMappedArray<int>(fileInfo);
        }

        public void Dispose()
        {
            array.Dispose();

            fileInfo.Delete();
        }
    }
}