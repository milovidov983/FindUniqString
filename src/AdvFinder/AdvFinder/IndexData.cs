using System;
using System.Linq;

namespace AdvFinder {
    public class IndexData
    {
        public static int Size;
        public long[] Indexes { get; set; }

        public IndexData(long inFileSize)
        {
            Size = inFileSize switch {
                < 512 * 1024 => 2048, // < 512 kb
                < 1024 * 1024 => 4096, // < 1mb
                < 1024 * 1024 * 10 => 10000, // < 10mb
                _ => 100000
            };
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
