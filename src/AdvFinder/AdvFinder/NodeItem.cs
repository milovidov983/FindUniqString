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


        private byte[] tmp = new byte[SizeBytes];
        private byte[] cTmp = new byte[sizeof(long)];
        private byte[] nTmp = new byte[sizeof(long)];
        public ReadOnlySpan<byte> GetBytes() {
            System.Buffer.BlockCopy(Hash, 0, tmp, 0, Hash.Length * sizeof(byte));

            BitConverter.TryWriteBytes(cTmp, Count);
            System.Buffer.BlockCopy(cTmp, 0, tmp, 
                Hash.Length * sizeof(byte),
                cTmp.Length * sizeof(byte));

            BitConverter.TryWriteBytes(nTmp, Next);
            System.Buffer.BlockCopy(nTmp, 0, tmp, 
                Hash.Length * sizeof(byte) + cTmp.Length * sizeof(byte),
                nTmp.Length * sizeof(byte));

            return tmp.AsSpan();
        }

        public const int CountSize = sizeof(long);
        public const int CountOffset = NodeItem.HashSize;

        public const int NextSize = sizeof(long);
        public const int NextOffset = NodeItem.HashSize + CountSize;

    }
}
