using System;
using System.Collections.Generic;
using System.Text;

namespace BaseAbstractions {
	public interface IFile {
		char GetCurrentChar(int idx);
		bool IsEOF(int idx);
		bool IsLineBreak(int idx);
		int MoveIndexToNewLine(int idx);
	}
}
