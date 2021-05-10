using BaseAbstractions;
using System.Collections.Generic;
using System.Linq;

namespace AdvFinder {
    public class AFinder {
        private IFile inputFile;
        private IndexData dataIndex;
        private IBagFile bagFile;
        private int bufferSize = 512;
        

        public int Find2(string fName) {
            inputFile = new BasicFile.Implementation(fName);
            bagFile = new BagFile();
            dataIndex = new IndexData();

            FillBagFile();

            return 1;
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
            List<byte> buffer = new List<byte>();
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
                int index = bagFile.SaveNewBag(hash);
                dataIndex.SavePosition(idx, index);
            } else {
                while (true) {
                    Bag storedBag = bagFile.GetBag(pos);
                    if (Enumerable.SequenceEqual(storedBag.Hash, hash)) {
                        storedBag.Count++;
                        bagFile.UpdateBag(pos, storedBag);
                        break;
                    } else if (storedBag.Next == -1) {
                        storedBag.Next = bagFile.SaveNewBag(hash);
                        break;
                    } else {
                        pos = storedBag.Next;
                    }
                }
            }
        }


        //public int Find(string fileName) {
        //    int index = -1;

        //    byte[] buffer = new byte[bufferSize];
        //    int currentBufferIndex = 0;
        //    byte[] sha256 = new byte[hashSize];
        //    while (true) {
        //        index++;

        //        if (inputFile.IsEOF(index))
        //        {
        //            sha256 = Compute(sha256,buffer);
        //            Save(sha256);
        //            break;
        //        }

        //        if (inputFile.IsLineBreak(index))
        //        {
        //            sha256 = Compute(sha256, buffer);
        //            Save(sha256);

        //            sha256 = new byte[hashSize];
        //            buffer = new byte[bufferSize];
        //            currentBufferIndex = 0;
        //        } else
        //        {
        //            if (currentBufferIndex < bufferSize)
        //            {
        //                buffer[currentBufferIndex] = inputFile.GetCurrentByte(index);
        //                currentBufferIndex++;
        //            } else
        //            {
        //                sha256 = Compute(sha256, buffer);
        //                buffer = new byte[bufferSize];
        //                currentBufferIndex = 0;
        //            }
        //        }
        //    }


        //    return 0;
        //}

        //private byte[] Compute(byte[] sha256, byte[] buffer)
        //{
        //    return Utils.ComputeSha256Hash(sha256.Concat(buffer).ToArray());
        //}

        //private void Save(byte[] data)
        //{
        //    var index = Utils.ComputeIndex(data);
        //    var position = dataIndex.GetPosition(index);
        //    if(position == -1)
        //    {
        //        var newPos = bagFile.Create(data);
        //        dataIndex.SavePosition(index, newPos);
        //        return;
        //    }
        //    while(true)
        //    {
        //        var hash = bagFile.GetHash(position);
        //        if(IsEquals(data, hash))
        //        {
        //            bagFile.Increment(position);
        //            break;
        //        } else {
        //            var nextPos = bagFile.GetNext(position);
        //            if(nextPos == -1)
        //            {
        //                var newPos = bagFile.Create(data);
        //                bagFile.Bind(position, newPos);
        //                break;
        //            }
        //        }
        //    }
        //}

        //private bool IsEquals(byte[] data, byte[] hash)
        //{
        //    return Enumerable.SequenceEqual(data, hash);
        //}
    }
}
