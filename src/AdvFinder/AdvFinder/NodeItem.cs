using System;
using System.Collections.Generic;

namespace AdvFinder {
    public class NodeItem
    {
        public byte[] Hash;
        public long Next = -1;
        public long Count = 1;

        public override string ToString() {
            return $"Hash:{string.Join("",Hash)}, Next:{Next}, Count:{Count};";
        }

        public const int HashSize = 32;
        public const long SizeBytes = HashSize + sizeof(long) * 2;

        public IEnumerable<byte> GetBytes() {
            foreach(var b in Hash) {
                yield return b;
            }
            var count = BitConverter.GetBytes(Count);
            foreach (var c in count) {
                yield return c;
            }
            var next = BitConverter.GetBytes(Next);
            foreach (var n in next) {
                yield return n;
            }
            yield break;
        }
    }
}
