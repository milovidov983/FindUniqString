using BaseAbstractions;
using System;
using System.IO;

namespace BasicFile {
	public sealed class Implementation : IFile, IDisposable {
		readonly FileStream reader;
		readonly long len;
		readonly byte[] tmp1 = new byte[1];
		readonly byte[] tmp2 = new byte[1];
		readonly char[] charArray1 = new char[2];


		public Implementation(string path) {
			reader = new FileStream(path, FileMode.Open);
			len = reader.Length;
		}

		public void Dispose() {
			reader?.Close();
			reader?.Dispose();
		}

		public char GetCurrentChar(int idx) {
			reader.Position = idx;
			reader.Read(tmp1, 0, 1);

			var c = Convert.ToChar(tmp1[0]);

			return c;

		}

		public bool IsEOF(int idx) {
			return idx >= len;
		}

		public bool IsEquals(int index1, int index2) {
			reader.Position = index1;
			reader.Read(tmp1, 0, 1);
			reader.Position = index2;
			//reader.BaseStream.Seek(index2, SeekOrigin.Begin);
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
