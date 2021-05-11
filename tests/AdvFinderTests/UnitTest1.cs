using AdvFinder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace AdvFinderTests {
    public class UnitTest1 {
        

        [Fact]
        public void PerformanceTest() {
            var advf = new AdvancedFinder();

            
            System.Diagnostics.Stopwatch sw = new();
            sw.Start();

            var result = advf.Find(@"examples/10mb10s.txt");
            

            System.Diagnostics.Debug.WriteLine($"\n\n{sw.ElapsedMilliseconds} ms\n\n");

            Assert.Equal(2, result);
        }
    }

}  