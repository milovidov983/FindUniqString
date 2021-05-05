using BaseAbstractions;
using System;
using System.IO;

namespace BasicFile {
	public sealed class Implementation : IFile, IDisposable {
		readonly StreamReader reader;
		readonly long len;
		readonly char[] tmp1 = new char[1];
		readonly char[] tmp2 = new char[1];

		public Implementation(string path) {
			reader = new StreamReader(path);
			len = reader.BaseStream.Length;
		}

		public void Dispose() {
			reader?.Close();
			reader?.Dispose();
		}

		public char GetCurrentChar(int idx) {
			reader.BaseStream.Flush();
			reader.BaseStream.Seek(idx, SeekOrigin.Begin);
			reader.Read(tmp1, 0, 1);
			return tmp1[0];
		}

		public bool IsEOF(int idx) {
			return idx >= len;
		}

		public bool IsEquals(int index1, int index2) {
			reader.BaseStream.Seek(index1, SeekOrigin.Begin);
			reader.Read(tmp1, 0, 1);
			reader.BaseStream.Seek(index2, SeekOrigin.Begin);
			reader.Read(tmp2, 0, 1);

			return tmp1[0] == tmp2[0];
		}

		public bool IsLineBreak(int idx) {
			var c = GetCurrentChar(idx);
			return c == '\n';
		}

		public int MoveIndexToNewLine(int idx) {
			while(!IsEOF(idx) && !IsLineBreak(idx)) {
				idx++;
			}
			if (IsEOF(idx)) {
				return -1;
			}
			return idx;
		}

		
	}
}
