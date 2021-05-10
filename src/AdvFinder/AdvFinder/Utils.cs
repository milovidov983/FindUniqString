using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AdvFinder {
    public static class Utils
    {
        public static byte[] ComputeSha256Hash(byte[] rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                return sha256Hash.ComputeHash(rawData);
            }
        }
        public static int ComputeIndex(byte[] rawData)
        {
            int hash = 5381;
            foreach (var item in rawData) {
                hash = ((hash << 5) + hash) + item;
            }
            return Math.Abs(hash % IndexData.Size);
        }
    }
}
