using System;
using Xunit;

namespace BasicFileTests {
	public class Tests {
		[Fact]
		public void DataSetIsEqual_IsEquals_True() {
			var file = new BasicFile.Implementation(@".\examples\aa.txt");

			var result = file.IsEquals(0, 1);

			Assert.True(result);
		}
		[Fact]
		public void DataSetIsNotEqual_IsEquals_False() {
			var file = new BasicFile.Implementation(@".\examples\ab.txt");

			var result = file.IsEquals(0, 1);

			Assert.False(result);
		}
		[Fact]
		public void IsEof_False() {
			var file = new BasicFile.Implementation(@".\examples\aa.txt");

			var result = file.IsEOF(3);

			Assert.False(result);
		}
		[Fact]
		public void IsEof_True() {
			var file = new BasicFile.Implementation(@".\examples\aa.txt");

			var result = file.IsEOF(5);

			Assert.True(result);
		}
		[Fact]
		public void GetChar_CharIsCorrect() {
			var file = new BasicFile.Implementation(@".\examples\ab.txt");

			var result = file.GetCurrentChar(4);

			Assert.Equal('b', result);
		}
		[Fact]
		public void IsLineBreak_True() {
			var file = new BasicFile.Implementation(@".\examples\a-nextline.txt");
			var index = file.MoveIndexToNewLine(0);

			var result = file.IsLineBreak(index);

			Assert.True(result, $"Current char is '{file.GetCurrentChar(index)}' ");
		}
		[Fact]
		public void IsLineBreak_False() {
			var file = new BasicFile.Implementation(@".\examples\a-nextline.txt");
			var index = 0;

			var result = file.IsLineBreak(index);

			Assert.False(result, $"Current char is '{file.GetCurrentChar(index)}' ");
		}
	}
}
