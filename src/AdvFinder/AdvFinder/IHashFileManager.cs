﻿using System;
using System.Collections.Generic;
using System.IO;

namespace AdvFinder {
    public interface IHashFileManager {
        long SaveNew(byte[] hash);
        void Update(long pos, NodeItem node);
        IEnumerable<NodeItem> GetAll();
        (NodeItem storedNode, long storedNodePos) GetEqualOrLastNode(long pos, byte[] hash);
        string GetDiagData();
        void Close();
    }

    public class HashFileManaager : IHashFileManager {
        readonly HashFileFs file;
        public readonly string FileName = Guid.NewGuid().ToString("N")+".tmp";

        public HashFileManaager() {
            file = new HashFileFs(FileName);
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

        public void Close() {
            file.fs.Close();
        }
     
    }
}
