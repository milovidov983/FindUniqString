using BaseAbstractions;
using System;
using System.IO;

namespace BasicFile {
	public sealed class Implementation : IFile, IDisposable {
		readonly FileStream reader;
		readonly long len;
		readonly byte[] tmp1 = new byte[1];
		readonly byte[] tmp2 = new byte[1];
	

		public Implementation(string path) {
			reader = new FileStream($"{System.IO.Directory.GetCurrentDirectory()}/{path}", FileMode.Open);
			len = reader.Length;
		}

        public long Length => len;

        public void Dispose() {
			reader?.Close();
			reader?.Dispose();
		}

		public byte GetCurrentByte(int idx)
		{
			reader.Position = idx;
			reader.Read(tmp1, 0, 1);

			return tmp1[0];
		}

		public char GetCurrentChar(int idx) {
			return Convert.ToChar(GetCurrentByte(idx));
		}

		public bool IsEOF(int idx) {
			return idx >= len;
		}

		public bool IsEquals(int index1, int index2) {
			reader.Position = index1;
			reader.Read(tmp1, 0, 1);

			reader.Position = index2;
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
