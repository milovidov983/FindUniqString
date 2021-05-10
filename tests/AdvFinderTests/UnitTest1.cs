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

            //var result = advf.Find2(@"examples/100kb.txt");            
            var result = advf.Find2(@"examples/test.txt");

            Assert.Equal(2, result);
        }

      
    }

}  