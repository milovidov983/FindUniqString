using AdvFinder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace AdvFinderTests {
    public class UnitTest1 {
        [Fact]
        public void Test1() {
            var advf = new AFinder();

            var r = advf.Find2(@"examples/100kb.txt");

            Console.WriteLine();

        }

        [Fact]
        public void Create_Two_Bag_AfterCorrect_Read_All_Bags() {
            var fileCreator = 
        }
    }

}  