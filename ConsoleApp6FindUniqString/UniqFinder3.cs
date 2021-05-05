using ConsoleApp6FindUniqString;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniqStringFinder {
	public class UniqFinder3 {
		private const int startIndex = -1;
		public int Find(IAbstractFile file) {
			int counter = 0,
				mainPtr = startIndex,
				secendPtr = startIndex,
				mainStartPtr = startIndex;

			while (true) {
				mainPtr++;
				secendPtr++;

				bool isMainPtrEof = file.IsEOF(mainPtr);
				bool isSecondPtrEof = file.IsEOF(secendPtr);
				if(isMainPtrEof && isSecondPtrEof) {
					break;
				}

				if (isSecondPtrEof) {
					mainPtr = file.MoveIndexToNewLine(mainPtr);
					if (file.IsEOF(mainPtr)) {
						break;
					}
					counter++;
					mainStartPtr = mainPtr;
					secendPtr = startIndex;
				} else if(isMainPtrEof 
					|| mainPtr == secendPtr 
					|| file.GetCurrentChar(mainPtr) != file.GetCurrentChar(secendPtr)) {
					mainPtr = mainStartPtr;
					secendPtr = file.MoveIndexToNewLine(secendPtr);
				} else if (file.IsLineBreak(mainPtr)) { // file[mainPtr] ==  file[secendPtr]
					mainStartPtr = mainPtr;
					secendPtr = startIndex;
				}
			}
			return counter;
		}
	}
}
