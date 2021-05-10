using System;

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
    }
}
