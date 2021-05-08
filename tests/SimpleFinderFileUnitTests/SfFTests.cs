using System;
using TestUtils;
using Xunit;

namespace SimpleFinderFileUnitTests {
	public class SfFTests {
		readonly SimpleFinder.Finder finder = new SimpleFinder.Finder();

		[Fact]
		public void PerformanceTest()
		{
			var testFile = Factory.CreateBasicFile(@"examples/01mb.tmp");

			//System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
			//sw.Start();

			//var uniqueCount = finder.Find(testFile);

			//System.Diagnostics.Debug.WriteLine($"\n\n{sw.ElapsedMilliseconds} ms\n\n");

			//Assert.Equal(2, uniqueCount);
		}

		[Fact]
		public void TwoString_NoUniq() {
			var testFile = Factory.CreateBasicFile(@"examples/aa1.txt");

			var uniqueCount = finder.Find(testFile);

			Assert.Equal(0, uniqueCount);
		}

		[Fact]
		public void TwoString_TwoUniq() {
			var testFile = Factory.CreateBasicFile(@"examples/ab1.txt");

			var uniqueCount = finder.Find(testFile);

			Assert.Equal(2, uniqueCount);
		}

		[Fact]
		public void ThreeString_OneUniqOnStart() {
			var testFile = Factory.CreateBasicFile(@"examples/baa1.txt");

			var uniqueCount = finder.Find(testFile);

			Assert.Equal(1, uniqueCount);
		}

		[Fact]
		public void ThreeString_OneUniqOnCenter() {
			var testFile = Factory.CreateBasicFile(@"examples/aba1.txt");

			var uniqueCount = finder.Find(testFile);

			Assert.Equal(1, uniqueCount);
		}
		[Fact]
		public void ThreeString_OneUniqOnEnd() {
			var testFile = Factory.CreateBasicFile(@"examples/aab1.txt");

			var uniqueCount = finder.Find(testFile);

			Assert.Equal(1, uniqueCount);
		}

		[Fact]
		public void OneString_OneUniq() {
			var testFile = Factory.CreateBasicFile(@"examples/a1.txt");

			var uniqueCount = finder.Find(testFile);

			Assert.Equal(1, uniqueCount);
		}

		[Fact]
		public void ThreeString_ThreeUniq() {
			var testFile = Factory.CreateBasicFile(@"examples/abc1.txt");

			var uniqueCount = finder.Find(testFile);

			Assert.Equal(3, uniqueCount);
		}
	}
}
