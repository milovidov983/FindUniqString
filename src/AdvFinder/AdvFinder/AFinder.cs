using BaseAbstractions;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdvFinder {


    public class AFinder {
        private IFile inputFile;
        private IndexData dataIndex;
        private IHashFileManager fileManager;
        private int bufferSize = 512;
        


        public int Find2(string fName) {
            inputFile = new BasicFile.Implementation(fName);
            fileManager = new HashFileManaager();
            dataIndex = new IndexData();

            FillBagFile();

            int counter = 0;

            foreach (var node in fileManager.GetAll()) {
                if (node.Count == 1) {
                    counter++;
                    //using BinaryWriter writer = new(File.Open($"111h{counter}", FileMode.OpenOrCreate));
                    //writer.Write(node.Hash);
                }
            }
            return counter;
        }

        private void FillBagFile() {
            int index = 0;
            while (true) {
                if (index == -1 || inputFile.IsEOF(index)) {
                    break;
                }
                var (h, i) = ReadNextString(index);
                index = i;
                SaveNextHash(h);
            }
        }

        private (byte[], int) ReadNextString(int idx) {
            List<byte> buffer = new();
            while (!inputFile.IsEOF(idx) && !inputFile.IsLineBreak(idx)) {
                var c = inputFile.GetCurrentByte(idx);
                buffer.Add(c);
                if (buffer.Count >= bufferSize) {
                    var intermedateHash = Utils.ComputeSha256Hash(buffer.ToArray());
                    buffer.Clear();
                    buffer.AddRange(intermedateHash);
                }
                idx++;
            }
            int resultIndex = inputFile.IsEOF(idx) ? -1 : idx;
            if(resultIndex != -1 && inputFile.IsLineBreak(idx)) {
                resultIndex++;
            }
            var hash = Utils.ComputeSha256Hash(buffer.ToArray());
            return (hash, resultIndex);
        }

        private void SaveNextHash(byte[] hash) {
            var idx = Utils.ComputeIndex(hash);
            var pos = dataIndex.GetPosition(idx);

            if (pos == -1) {
                long index = fileManager.SaveNew(hash);
                dataIndex.SavePosition(idx, index);
            } else {
                while (true) {
                    NodeItem storedBag = fileManager.Get(pos);
                    if (Enumerable.SequenceEqual(storedBag.Hash, hash)) {
                        storedBag.Count++;
                        fileManager.Update(pos, storedBag);
                        break;
                    } else if (storedBag.Next == -1) {
                        storedBag.Next = fileManager.SaveNew(hash);
                        break;
                    } else {
                        pos = storedBag.Next;
                    }
                }
            }
        }
    }
}
