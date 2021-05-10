using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdvFinder {
    public sealed class HashFileFileStream  {
        private readonly string fileName;
        public FileStream fs;

        public HashFileFileStream(string fileName) {
            this.fileName = fileName;
            fs = new FileStream(fileName, FileMode.OpenOrCreate);
        }

      

        public long WriteData(long? pos, NodeItem data) {
            fs.Seek(pos ?? fs.Length, SeekOrigin.Begin);
            var bytes = data.GetBytes();
            fs.Write(bytes);

            return fs.Position;
        }

        readonly byte[] readBuffer = new byte[NodeItem.SizeBytes];
        public (NodeItem storedNode, long storedNodePos, bool isEquals)  GetEqualOrLastNode(long startPosition, byte[] hash) { 
            if (startPosition > fs.Length) {
                return default;
            }
            long next = startPosition;

            while (true) {
                fs.Seek(next, SeekOrigin.Begin);
                fs.Read(readBuffer, 0, (int)NodeItem.SizeBytes);

                Span<byte> storedHash = readBuffer.AsSpan().Slice(0, NodeItem.HashSize);

                Span<byte> countSpan = readBuffer.AsSpan().Slice(NodeItem.CountOffset, NodeItem.CountSize);
                long count = BitConverter.ToInt32(countSpan);

                Span<byte> nextSpan = readBuffer.AsSpan().Slice(NodeItem.NextOffset, NodeItem.NextSize);
                next = BitConverter.ToInt32(nextSpan);

                bool? isEquals = null;
                if (next == -1 || SeqEqual(storedHash, hash, out isEquals)) {
                    var node = new NodeItem {
                        Hash = storedHash.ToArray(),
                        Count = count,
                        Next = next
                    };
                    
                    return (
                            storedNode: node,
                            storedNodePos: fs.Position - NodeItem.SizeBytes,
                            isEquals: isEquals ?? SeqEqual(storedHash, hash, out _)
                        );
                }
            }
        }

        private bool SeqEqual(ReadOnlySpan<byte> r, ReadOnlySpan<byte> l, out bool? result) {
            for(var i = 0; i < r.Length; i++) {
                if(r[i] != l[i]) {
                    result = false;
                    return false;
                }
            }
            result = true;
            return true;
        }


        public IEnumerable<NodeItem> GetAll() {
            using BinaryReader reader = new(File.Open(fileName, FileMode.Open));

            var tmpHash = new byte[NodeItem.HashSize];
            while (reader.BaseStream.Length > reader.BaseStream.Position) {
                for (int i = 0; i < NodeItem.HashSize; i++) {
                    tmpHash[i] = reader.ReadByte();
                }
                long count = reader.ReadInt64();
                long next = reader.ReadInt64();


                yield return new NodeItem {
                    Count = count,
                    Hash = tmpHash.ToArray(),
                    Next = next
                };
            }
            yield break;
        }
    }
}
