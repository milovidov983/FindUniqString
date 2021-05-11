using BaseAbstractions;
using System;
using System.IO;

namespace BasicFile {
	public sealed class ImplementationBuffer : IFile, IDisposable {
		readonly FileStream reader;
		readonly long len;
		readonly byte[] tmp1 = new byte[1];
		readonly byte[] tmp2 = new byte[1];
	

		public ImplementationBuffer(string path) {
			reader = new FileStream($"{System.IO.Directory.GetCurrentDirectory()}/{path}", FileMode.Open);
			len = reader.Length;
		}

        public long Length => len;

        public void Dispose() {
			reader?.Close();
			reader?.Dispose();
		}

		const int sizeBuffer = 1024;
		byte[] buffer = new byte[sizeBuffer];
		int startBufferPos = -1;
		int endBufferPos = -1;


		public byte GetCurrentByte(int idx)
		{
			if(startBufferPos <= idx && idx < endBufferPos) {
				return  buffer[idx - startBufferPos];
			} else {
				FillBuffer(idx);
			}
			return  buffer[idx - startBufferPos];

		}
		private void FillBuffer(int idx) {
			int count = len - idx > sizeBuffer ? sizeBuffer : (int)len - idx;
			startBufferPos = idx;
			endBufferPos = idx + count;

			reader.Position = idx;
			reader.Read(buffer, 0, count);
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
