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
            var advf = new AdvancedFinder();

            var result = advf.Find(@"examples/1mb.txt");            
            //var result = advf.Find2(@"examples/test.txt");

            Assert.Equal(2, result);
        }

        [Fact]
        public void PerformanceTest() {
            var advf = new AdvancedFinder();

            
            System.Diagnostics.Stopwatch sw = new();
            sw.Start();

            var result = advf.Find(@"examples/1mb.txt");
            

            System.Diagnostics.Debug.WriteLine($"\n\n{sw.ElapsedMilliseconds} ms\n\n");

            Assert.Equal(2, result);
        }
    }

}  