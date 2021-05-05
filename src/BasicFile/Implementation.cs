using BaseAbstractions;
using System;
using System.IO;

namespace BasicFile {
	public class Implementation : IFile, IDisposable {
		readonly StreamReader sr;
		byte[] tmp = new byte[1];

		public Implementation(string path) {
			sr = new StreamReader(path);
		}

		public void Dispose() {
			sr?.Close();
			sr?.Dispose();
		}

		public char GetCurrentChar(int idx) {
			throw new NotImplementedException();
		}

		public bool IsEOF(int idx) {
			throw new NotImplementedException();
		}

		public bool IsLineBreak(int idx) {
			throw new NotImplementedException();
		}

		public int MoveIndexToNewLine(int idx) {
			throw new NotImplementedException();
		}
	}
}
