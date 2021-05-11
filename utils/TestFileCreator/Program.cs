using System;
using System.IO;
using System.Linq;
using System.Text;

namespace TestFileCreator {
    class Program {
        readonly static FileStream tmp = new FileStream("10mb10s.txt", FileMode.Create);
        private static float fileSizeMb;
        private static int strSize = 10;

        static void Main(string[] args) {
            var uniqStringCount = 2;//Convert.ToInt32(args[0]);
            fileSizeMb = 10.0F; //Convert.ToInt32(args[1]);

            
            int index = FillUniq(uniqStringCount);
            int cloneCounter = 2;
            byte[] byteArray = null;
            while (true)
            {
                if (IsEof(index))
                {
                    if (cloneCounter == 1)
                    {
                        tmp.SetLength(index - byteArray.Length);
                    }
                    break;
                }
                else
                {
                    if (cloneCounter == 2)
                    {
                        byteArray = CreateString();
                        index = InsertString(byteArray, index);
                        if (index > 0)
                        {
                            cloneCounter = 1;
                        }
                    }
                    else if (cloneCounter == 1)
                    {
                        index = InsertString(byteArray, index);
                        if (index > 0)
                        {
                            cloneCounter = 2;
                        }
                    }

                }

            }

            tmp.Close();

        }

        private static bool IsEof(int index)
        {
            return fileSizeMb * 1000000 < index;
        }

        private static void DeleteLastString(int index)
        {
            Console.WriteLine($"DeleteLastString = {index}");
        }

        private static int FillUniq(int uniqStringCount)
        {
            int idx = 0;
            foreach(var i in Enumerable.Range(0, uniqStringCount))
            {
                var bytes = CreateString("UNIQ_");
                idx = InsertString(bytes, idx);
                if (idx < 0)
                {
                    Console.WriteLine("FillUniq overflow");
                    return -1;
                }
            }
            return idx;
        }

        private static int InsertString(byte[] bytes, int index)
        {
            if(IsEof(index))
            {
                return -1;
            }
            
            try
            {

                // Write the data to the file, byte by byte.
                for (int i = 0; i < bytes.Length; i++)
                {
                    tmp.WriteByte(bytes[i]);
                }
            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }

            return index + bytes.Length;
        }

        private static byte[] CreateString(string prefix = null)
        {
            var body = string.Join("", Enumerable.Range(0, strSize).Select(_ => Guid.NewGuid().ToString()));
            var str = $"{prefix}{body}\n";
            return Encoding.ASCII.GetBytes(str);

        }
    }
}
