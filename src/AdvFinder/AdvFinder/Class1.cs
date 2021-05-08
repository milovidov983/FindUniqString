using BaseAbstractions;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AdvFinder
{
    public class IndexData
    {
        public int[] Indexes { get; set; }

        public IndexData()
        {
            Indexes = Enumerable.Range(0, 1024).Select(_ => -1).ToArray();
        }

        public int GetPosition(int i)
        {
            return Indexes[i];
        }

        public void SavePosition(int i, int position)
        {
            Indexes[i] = position;
        }
    }

    public class Bag
    {
        public int Next { get; set; }
        public string Hash { get; set; }
        public int Count { get; set; }
    }

    public static class Utils
    {
        public static byte[] ComputeSha256Hash(byte[] rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                return sha256Hash.ComputeHash(rawData);
            }
        }

        

        public static int ComputeIndex(byte[] rawData)
        {
            int hash = 5381;


            foreach (var item in rawData)
            {
                hash = ((hash << 5) + hash) + item; /* hash * 33 + c */
            }

            return hash % 1024;
        }

    }

    public interface IBagFile {
        byte[] GetHash(int index);
        int Count(int index);
        void Increment(int index);
        int GetNext(int index);
        int Create(byte[] hash);
        void Bind(int parentIndex, int childIndex);
    }

    public class AdvFinder {
        private IFile file;
        private IndexData dataIndex;
        private IBagFile bagFile;
        private int bufferSize = 512;
        private int hashSize = 256;

        public int Find(string fileName) {
            int index = -1;

            byte[] buffer = new byte[bufferSize];
            int currentBufferIndex = 0;
            byte[] sha256 = new byte[hashSize];
            while (true) {
                index++;

                if (file.IsEOF(index))
                {
                    sha256 = Compute(sha256,buffer);
                    Save(sha256);
                    break;
                }

                if (file.IsLineBreak(index))
                {
                    sha256 = Compute(sha256, buffer);
                    Save(sha256);

                    sha256 = new byte[hashSize];
                    buffer = new byte[bufferSize];
                    currentBufferIndex = 0;
                } else
                {
                    if (currentBufferIndex < bufferSize)
                    {
                        buffer[currentBufferIndex] = file.GetCurrentByte(index);
                        currentBufferIndex++;
                    } else
                    {
                        sha256 = Compute(sha256, buffer);
                        buffer = new byte[bufferSize];
                        currentBufferIndex = 0;
                    }
                }
            }


            return 0;
        }

        private byte[] Compute(byte[] sha256, byte[] buffer)
        {
            return Utils.ComputeSha256Hash(sha256.Concat(buffer).ToArray());
        }

        private void Save(byte[] data)
        {
            throw new NotImplementedException();
        }
    }


}
