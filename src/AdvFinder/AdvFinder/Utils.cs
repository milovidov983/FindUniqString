using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AdvFinder {
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
                hash = ((hash << 5) + hash) + item;
            }
            return hash % IndexData.Size;
        }
        public static byte[]  xx(Bag bag) {
            byte[] buffer;

            using (var ms = new MemoryStream()) {
                using (var bw = new BinaryWriter(ms, Encoding.ASCII)) {
                    bw.Write(bag.Hash);
                    bw.Write(bag.Count);
                    bw.Write(bag.Next);
                }

                buffer = ms.ToArray();
                return buffer;
            }
        }
        public static FileStream Create(string name) {
            return new FileStream(name, FileMode.Create);
        }
        public static FileStream Open(string name) {
            return new FileStream(name, FileMode.Open);
        }
        private const int blockSizeByte = 40;
        public static byte[] ReadData(this FileStream reader, int index) {
            reader.Position = index;
            byte[] tmp1 = new byte[blockSizeByte];
            reader.Read(tmp1, 0, blockSizeByte);
            return tmp1;
        }
        public static int WriteData(this FileStream stream, int index, byte[] data) {
            stream.Position = index;
            stream.Write(data, 0, data.Length);
            return index + data.Length;
        }

    }
}
