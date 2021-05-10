using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdvFinder {
    public sealed class HashFile  {
        private readonly string fileName;
     

        public HashFile(string fileName) {
            this.fileName = fileName;
        }

        BinaryWriter w;
        BinaryReader r;
        BinaryWriter GetWriter() {
            if(r != null) {
                r.Close();
                r.Dispose();
                r = null;
            }
            if (w != null) {
                return w;
            }
            w = new(File.Open(fileName, FileMode.OpenOrCreate));
            return w;
        }
        BinaryReader GetReader() {
            if (w != null) {
                w.Close();
                w.Dispose();
                w = null;
            }
            if (r != null) {
                return r;
            }
            r = new(File.Open(fileName, FileMode.Open));
            return r;
        }

        public long WriteData(long? pos, NodeItem data) {
            //using BinaryWriter writer = new(File.Open(fileName, FileMode.OpenOrCreate));
            BinaryWriter writer = GetWriter();
            writer.BaseStream.Position = pos ?? writer.BaseStream.Length;
            writer.Write(data.Hash);
            writer.Write(data.Count);
            writer.Write(data.Next);
            return writer.BaseStream.Position;
        }

        private readonly byte[] tmpHash = new byte[NodeItem.HashSize];

        /// <summary>
        ///  TODO можно оптимизировать не создавать каждый раз Bag и сравнивать
        ///  А открыв файл однажды пройтись по позициям указанным в next и найти нужный hash -> FindEqual
        /// </summary>
        public NodeItem ReadData(long pos) {
            //using BinaryReader reader = new(File.Open(fileName, FileMode.Open));
            BinaryReader reader = GetReader();
           
            if (pos > reader.BaseStream.Length) {
                
                return null;
            }
            reader.BaseStream.Position = pos;


            for (int i = 0; i < NodeItem.HashSize; i++) {
                tmpHash[i] = reader.ReadByte();
            }
            long count = reader.ReadInt64();
            long next = reader.ReadInt64();


            var node = new NodeItem {
                Count = count,
                Hash = tmpHash.ToArray(),
                Next = next
            };

            return node;
        }

        public (NodeItem storedNode, long storedNodePos)  GetEqualOrLastNode(long startPosition, byte[] hash) {
            //using BinaryReader reader = new(File.Open(fileName, FileMode.Open));
            BinaryReader reader = GetReader();


            if (startPosition > reader.BaseStream.Length) {
                return default;
            }
            long next = startPosition;
            var counter = 0;
            while (true) {
                counter++;
                maxStep = Math.Max(maxStep, counter);
                reader.BaseStream.Position = next;

                var tmp = reader.ReadBytes(NodeItem.HashSize);

                long count = reader.ReadInt64();
                next = reader.ReadInt64();

                if (next == -1 || Enumerable.SequenceEqual(tmp, hash)) {
                    return (
                            storedNode: new NodeItem {
                                Count = count,
                                Hash = tmp,
                                Next = next
                            },
                            storedNodePos: reader.BaseStream.Position - NodeItem.SizeBytes
                        );
                }
            }
        }

        public IEnumerable<NodeItem> GetAll() {
            //using BinaryReader reader = new(File.Open(fileName, FileMode.Open));
            BinaryReader reader = GetReader();


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

        public int maxStep = 0;
        
    }
}
