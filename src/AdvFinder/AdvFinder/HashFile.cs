using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvFinder {
    public class HashFile {
        private readonly string fileName;

        public HashFile(string fileName) {
            this.fileName = fileName;
        }

        public long WriteData(long? pos, NodeItem data) {
            using BinaryWriter writer = new(File.Open(fileName, FileMode.OpenOrCreate));
            writer.BaseStream.Position = pos ?? writer.BaseStream.Length;
            writer.Write(data.Hash);
            writer.Write(data.Count);
            writer.Write(data.Next);
            return writer.BaseStream.Position;
        }

        private readonly byte[] tmp = new byte[NodeItem.HashSize];

        /// <summary>
        ///  TODO можно оптимизировать не создавать каждый раз Bag и сравнивать
        ///  А открыв файл однажды пройтись по позициям указанным в next и найти нужный hash -> FindEqual
        /// </summary>
        public NodeItem ReadData(long pos) {
            using BinaryReader reader = new(File.Open(fileName, FileMode.Open));
           
            if (pos > reader.BaseStream.Length) {
                System.Diagnostics.Debug.WriteLine($"ReadData Pos:{pos} EOF");
                return null;
            }
            reader.BaseStream.Position = pos;


            for (int i = 0; i < NodeItem.HashSize; i++) {
                tmp[i] = reader.ReadByte();
            }
            long count = reader.ReadInt64();
            long next = reader.ReadInt64();


            var node = new NodeItem {
                Count = count,
                Hash = tmp.ToArray(),
                Next = next
            };

            return node;
        }

        public (NodeItem node, long pos) FindEqual(long pos, NodeItem node) {
            throw new NotImplementedException();
        }

        public IEnumerable<NodeItem> GetAll() {
            using BinaryReader reader = new(File.Open(fileName, FileMode.Open));
            while (reader.BaseStream.Length > reader.BaseStream.Position) {
                for (int i = 0; i < NodeItem.HashSize; i++) {
                    tmp[i] = reader.ReadByte();
                }
                long count = reader.ReadInt64();
                long next = reader.ReadInt64();


                yield return new NodeItem {
                    Count = count,
                    Hash = tmp.ToArray(),
                    Next = next
                };
            }
            yield break;
        }
    }
}
