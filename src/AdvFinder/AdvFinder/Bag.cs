using System;

namespace AdvFinder {
    public class Bag
    {
        public Bag(ReadOnlySpan<byte> result) {
            var t = result.ToArray();
            Hash = result.Slice(0, 32).ToArray();
            Count = BitConverter.ToInt32(result.Slice(32, 4));
            Next = BitConverter.ToInt32(result.Slice(36, 4));
        }

        public Bag() {
        }

        public byte[] Hash;
        public int Next = -1;
        public int Count = 1;

        public const int BlockSizeByte = 40;

        public override string ToString() {
            return $"H:[{Hash}] N:[{Next}] C:[{Count}]";
        }
    }
}
