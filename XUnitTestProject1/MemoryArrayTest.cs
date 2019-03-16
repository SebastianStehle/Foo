// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) MMTest
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using ConsoleApp1;

namespace XUnitTestProject1
{
    public class MemoryArrayTest : ArrayTestBase
    {
        public MemoryArrayTest()
        {
            Sut = new MemoryArray<int>();
        }
    }
}
