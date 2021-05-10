using System;
using System.Linq;

namespace AdvFinder {
    public class IndexData
    {
        //public const int Size = 2048; // test size 2048
        public const int Size = 1024;
        public long[] Indexes { get; set; }

        public IndexData()
        {
            Indexes = Enumerable.Range(0, Size).Select(_ => -1L).ToArray();
        }

        public long GetPosition(int i)
        {
            return Indexes[i];
        }

        public void SavePosition(int i, long position)
        {
            Indexes[i] = position;
        }
    }
}
