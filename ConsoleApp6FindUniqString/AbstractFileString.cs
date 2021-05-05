using System;

namespace ConsoleApp6FindUniqString {
	public class AbstractFileString : IAbstractFile {
		public readonly char newLineToken = '\n';
		private readonly string content;
		

		public AbstractFileString(string content) {
			this.content = content;
		}

		public char GetCurrentChar(int idx) {
			return content[idx];
		}

		public bool IsEOF(int idx) {
			return content.Length == idx;
		}

		public bool IsLineBreak(int idx) {
			return content[idx] == newLineToken;
		}


		public int MoveIndexToNewLine(int idx) {
			int result = idx + 1;
			while(result < content.Length && content[result] != newLineToken) {
				result++;
			}
			return result;
		}
	}
}
