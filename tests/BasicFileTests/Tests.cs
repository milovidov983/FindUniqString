using System;
using Xunit;

namespace BasicFileTests {
	
	public class Tests {
		[Fact]
		public void DataSetIsEqual_IsEquals_True() {
			var file = new BasicFile.Implementation(@"examples/aa1.txt");

			var result = file.IsEquals(0, 1);

			Assert.True(result);
		}
		[Fact]
		public void DataSetIsNotEqual_IsEquals_False() {
			var file = new BasicFile.Implementation(@"examples/ab1.txt");

			var result = file.IsEquals(0, 1);

			Assert.False(result);
		}
		[Fact]
		public void IsEof_False() {
			var file = new BasicFile.Implementation(@"examples/aa2.txt");

			var result = file.IsEOF(1);

			Assert.False(result);
		}
		[Fact]
		public void IsEof_True() {
			var file = new BasicFile.Implementation(@"examples/aa3.txt");

			var result = file.IsEOF(3);

			Assert.True(result);
		}
		[Fact]
		public void GetChar_CharIsCorrect() {
			var file = new BasicFile.Implementation(@"examples/ab2.txt");

			var result1 = file.GetCurrentChar(1);
			var result2 = file.GetCurrentChar(0);

			Assert.Equal('b', result1);
			Assert.Equal('a', result2);
		}
		[Fact]
		public void IsLineBreak_True() {
			var file = new BasicFile.Implementation(@".\examples\a-nextline1.txt");
			var index = file.MoveIndexToNewLine(0);

			var result = file.IsLineBreak(index);

			Assert.True(result, $"Current char is '{file.GetCurrentChar(index)}' ");
		}
		[Fact]
		public void IsLineBreak_False() {
			var file = new BasicFile.Implementation(@".\examples\a-nextline2.txt");
			var index = 0;

			var result = file.IsLineBreak(index);

			Assert.False(result, $"Current char is '{file.GetCurrentChar(index)}' ");
		}
	}
}
