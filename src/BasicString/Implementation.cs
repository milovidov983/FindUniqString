using BaseAbstractions;
using System;

namespace BasicString {
	public class Implementation : IFile {
		public readonly char newLineToken = '\n';
		private readonly string content;


		public Implementation(string content) {
			this.content = content;
		}

        public byte GetCurrentByte(int idx)
        {
            throw new NotImplementedException();
        }

        public char GetCurrentChar(int idx) {
			return content[idx];
		}

		public bool IsEOF(int idx) {
			return content.Length == idx;
		}

		public bool IsEquals(int index1, int index2) {
			return content[index1] == content[index2];
		}

		public bool IsLineBreak(int idx) {
			return content[idx] == newLineToken;
		}


		public int MoveIndexToNewLine(int idx) {
			int result = idx + 1;
			while (result < content.Length && content[result] != newLineToken) {
				result++;
			}
			return result;
		}
	}
}
