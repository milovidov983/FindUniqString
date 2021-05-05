using BaseAbstractions;
using System;
using Xunit;

namespace SimpleFinderStringUnitTests {
	public static class Factory {
		public static IFile CreateBasicString(string content) {
			return new BasicString.Implementation(content);
		}
	}


	public class BasicStringTests {
		readonly SimpleFinder.Finder finder = new SimpleFinder.Finder();

		[Fact]
		public void TwoString_NoUniq() {
			var testFile = Factory.CreateBasicString(
@"a
a
"
);

			var uniqueCount = finder.Find(testFile);

			Assert.Equal(0, uniqueCount);
		}

		[Fact]
		public void TwoString_TwoUniq() {
			var testFile = Factory.CreateBasicString(
@"a
b
"
);

			var uniqueCount = finder.Find(testFile);

			Assert.Equal(2, uniqueCount);
		}

		[Fact]
		public void ThreeString_OneUniqOnStart() {
			var testFile = Factory.CreateBasicString(
@"b
a
a
"
);

			var uniqueCount = finder.Find(testFile);

			Assert.Equal(1, uniqueCount);
		}

		[Fact]
		public void ThreeString_OneUniqOnCenter() {
			var testFile = Factory.CreateBasicString(
@"a
b
a
"
);

			var uniqueCount = finder.Find(testFile);

			Assert.Equal(1, uniqueCount);
		}
		[Fact]
		public void ThreeString_OneUniqOnEnd() {
			var testFile = Factory.CreateBasicString(
@"a
a
b
"
);

			var uniqueCount = finder.Find(testFile);

			Assert.Equal(1, uniqueCount);
		}

		[Fact]
		public void OneString_OneUniq() {
			var testFile = Factory.CreateBasicString(
@"a
"
);
			var uniqueCount = finder.Find(testFile);

			Assert.Equal(1, uniqueCount);
		}

		[Fact]
		public void ThreeString_ThreeUniq() {
			var testFile = Factory.CreateBasicString(
@"a
b
c
"
);
			var uniqueCount = finder.Find(testFile);

			Assert.Equal(3, uniqueCount);
		}
	}
}
