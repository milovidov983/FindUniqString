using System;
using System.Collections.Generic;
using System.Text;

namespace BaseAbstractions {
	public interface IFile {
		char GetCurrentChar(int idx);
		byte GetCurrentByte(int idx);
		bool IsEOF(int idx);
		bool IsLineBreak(int idx);
		bool IsEquals(int index1, int index2);
		int MoveIndexToNewLine(int idx);
		long Length { get; }
	}
}
