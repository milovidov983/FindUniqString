using System;
using System.Collections.Generic;
using System.IO;

namespace AdvFinder {
    public interface IHashFileManager {
        long SaveNew(byte[] hash);
        NodeItem Get(long pos);
        void Update(long pos, NodeItem node);
        IEnumerable<NodeItem> GetAll();
        (NodeItem storedNode, long storedNodePos) GetEqualOrLastNode(long pos, byte[] hash);
        string GetDiagData();
    }

    public class HashFileManaager : IHashFileManager {
        readonly HashFile file;
        public readonly string FileName = Guid.NewGuid().ToString("N")+".tmp";

        public HashFileManaager() {
            file = new HashFile(FileName);
        }

        public NodeItem Get(long pos) {
            return file.ReadData(pos);
        }

        public (NodeItem storedNode, long storedNodePos) GetEqualOrLastNode(long startPosition, byte[] hash) {
            return file.GetEqualOrLastNode(startPosition, hash);
        }


        public long SaveNew(byte[] hash) {
            NodeItem node = new() { Hash = hash };
            long pos = file.WriteData(null, node);
            return pos - NodeItem.SizeBytes;
        }

        public void Update(long pos, NodeItem node) {
            file.WriteData(pos, node);
        }

        public IEnumerable<NodeItem> GetAll() {
            return file.GetAll();
        }

        public string GetDiagData() {
            return $"max transition steps: {file.maxStep}";
        }

     
    }
}
