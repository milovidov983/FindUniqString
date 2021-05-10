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
            var bytes = data.GetBytes().ToArray();
            fs.Write(bytes, 0, bytes.Length);
            fs.Flush();

            return fs.Position;
        }



        public (NodeItem storedNode, long storedNodePos)  GetEqualOrLastNode(long startPosition, byte[] hash) { 
            if (startPosition > fs.Length) {
                return default;
            }
            long next = startPosition;

            while (true) {
                fs.Seek(next, SeekOrigin.Begin);

                var tmp = new byte[NodeItem.SizeBytes];
                fs.Read(tmp, 0, (int)NodeItem.SizeBytes);
                var storedHash = tmp.AsSpan().Slice(0, 32).ToArray();

                long count = BitConverter.ToInt32(tmp.AsSpan().Slice(32, 8));
                next = BitConverter.ToInt32(tmp.AsSpan().Slice(40, 8));

                if (next == -1 || Enumerable.SequenceEqual(storedHash, hash)) {
                    return (
                            storedNode: new NodeItem {
                                Count = count,
                                Hash = storedHash,
                                Next = next
                            },
                            storedNodePos: fs.Position - NodeItem.SizeBytes
                        );
                }
            }
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
