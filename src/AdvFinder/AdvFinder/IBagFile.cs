using System;
using System.IO;

namespace AdvFinder {
    public interface IBagFile {
        //byte[] GetHash(int index);
        //int Count(int index);
        //void Increment(int index);
        //int GetNext(int index);
        //int Create(byte[] hash);
        //void Bind(int parentIndex, int childIndex);
        int SaveNewBag(byte[] hash);
        Bag GetBag(int pos);
        void UpdateBag(int pos, Bag bag);
    }

    public class BagFile : IBagFile, IDisposable {
        private int endPos = 0;
        readonly FileStream bagFile;

        public BagFile() {
            bagFile = new FileStream(Guid.NewGuid().ToString("N"), FileMode.Create);
        }

        public Bag GetBag(int pos) {
            byte[] tmp1 = new byte[Bag.BlockSizeByte];
            bagFile.Position = pos;
            bagFile.Read(tmp1, 0, Bag.BlockSizeByte);
            return new Bag(tmp1);
        }
    
        public int SaveNewBag(byte[] hash) {
            var b = new Bag { Hash = hash };

            System.Diagnostics.Debug.WriteLine(b);
            System.Diagnostics.Debug.WriteLine($"SaveNewBag: endpos:[{endPos}]");

            var tmp = Utils.ToArray(b);
            bagFile.Position = endPos;
            bagFile.Write(tmp, 0, hash.Length);
            endPos = endPos + tmp.Length;
            return endPos - tmp.Length;
        }

        public void UpdateBag(int pos, Bag bag) {
            var array = Utils.ToArray(bag);
            bagFile.Position = pos;
            bagFile.Write(array, 0, array.Length);
        }

        public void Dispose() {
            bagFile?.Close();
        }
    }
}
