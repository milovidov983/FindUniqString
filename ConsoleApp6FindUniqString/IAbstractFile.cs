namespace ConsoleApp6FindUniqString {
	public interface IAbstractFile {
		char GetCurrentChar(int idx);
		bool IsEOF(int idx);
		bool IsLineBreak(int idx);
		int MoveIndexToNewLine(int idx);
	}
}