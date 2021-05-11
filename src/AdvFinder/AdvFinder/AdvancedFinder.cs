using BaseAbstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdvFinder {
	public partial class AdvancedFinder {
		private IFile inputFile;
		private IndexData HashDataFileIndexes;
		private IHashFileManager fileManager;
		private readonly int readStringBufferSize = 512;
		private readonly List<byte> readStringBuffer;

		public AdvancedFinder() {
			readStringBuffer = new(readStringBufferSize + 1);
		}
		private Sw swCreateHashDataFile = new Sw(nameof(CreateHashDataFile));

		public int Find(string fName) {
			inputFile = new BasicFile.ImplementationBuffer(fName);
			fileManager = new HashFileManager();
			HashDataFileIndexes = new IndexData(inputFile.Length);

			swCreateHashDataFile.Restart();
			CreateHashDataFile();
			swCreateHashDataFile.Stop();


			int counter = 0;
			foreach (var node in fileManager.GetAll()) {
				if (node.Count == 1) {
					counter++;
				}
			}


			PrintDebugStatistics();

			return counter;
		}

		private void PrintDebugStatistics() {
			System.Diagnostics.Debug.WriteLine($"{swCreateHashDataFile}");
			System.Diagnostics.Debug.WriteLine($"{swReadNextString}");
			System.Diagnostics.Debug.WriteLine($"{swSaveNextHash}");

		}
		private Sw swReadNextString = new Sw(nameof(ReadNextString));
		private Sw swSaveNextHash = new Sw(nameof(SaveNextHash));

		private void CreateHashDataFile() {
			int index = 0;

			while (true) {
				if (index == -1 || inputFile.IsEOF(index)) {
					break;
				}
				swReadNextString.Restart();
				var (h, i) = ReadNextString(index);
				index = i;
				swReadNextString.Stop();

				swSaveNextHash.Restart();
				SaveNextHash(h);
				swSaveNextHash.Stop();
			}
			fileManager.Close();
		}


		private (byte[], int) ReadNextString(int idx) {
			readStringBuffer.Clear();
			while (!inputFile.IsEOF(idx) && !inputFile.IsLineBreak(idx)) {
				var c = inputFile.GetCurrentByte(idx);
				readStringBuffer.Add(c);
				if (readStringBuffer.Count >= readStringBufferSize) {
					var intermedateHash = Utils.ComputeSha256Hash(readStringBuffer.ToArray());
					readStringBuffer.Clear();
					readStringBuffer.AddRange(intermedateHash);
				}
				idx++;
			}
			int resultIndex = inputFile.IsEOF(idx) ? -1 : idx;
			if (resultIndex != -1 && inputFile.IsLineBreak(idx)) {
				resultIndex++;
			}
			var hash = Utils.ComputeSha256Hash(readStringBuffer.ToArray());
			return (hash, resultIndex);
		}


		private void SaveNextHash(byte[] hash) {
			
			var idx = Utils.ComputeIndex(hash);

			var currentPosition = HashDataFileIndexes.GetPosition(idx);
			if (currentPosition == -1) {
				long newPosition = fileManager.SaveNew(hash);
				HashDataFileIndexes.SavePosition(idx, newPosition);
			} else {
				while (true) {

					(NodeItem storedNode, long storedNodePos, bool isEquals)
						= fileManager.GetEqualOrLastNode(currentPosition, hash);

					if (isEquals) {
						storedNode.Count++;
						fileManager.Update(storedNodePos, storedNode);
						break;
					} else if (storedNode.Next == -1) {
						storedNode.Next = fileManager.SaveNew(hash);
						fileManager.Update(storedNodePos, storedNode);
						break;
					} else {
						currentPosition = storedNode.Next;
					}
				}
			}
		}
	}
}
