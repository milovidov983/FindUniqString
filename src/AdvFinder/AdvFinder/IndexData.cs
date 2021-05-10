using System;
using System.Linq;

namespace AdvFinder {
    public class IndexData
    {
        public const int Size = 1024;
        public int[] Indexes { get; set; }

        public IndexData()
        {
            Indexes = Enumerable.Range(0, Size).Select(_ => -1).ToArray();
        }

        public int GetPosition(int i)
        {
            return Indexes[Math.Abs(i)];
        }

        public void SavePosition(int i, int position)
        {
            Indexes[Math.Abs(i)] = position;
        }
    }
}
