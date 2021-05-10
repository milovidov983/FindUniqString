using BaseAbstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AdvFinder {
    public class AdvancedFinderDiag {
        private IFile inputFile;
        private IndexData dataIndex;
        private IHashFileManager fileManager;
        private readonly int bufferSize = 512;
   

        public int Find(string fName) {
            inputFile = new BasicFile.Implementation(fName);
            fileManager = new HashFileManaager();
            dataIndex = new IndexData();
            FillBagFile();


            int counter = 0;

            foreach (var node in fileManager.GetAll()) {
                if (node.Count == 1) {
                    counter++;
                }
            }

            var empty = 0;
            foreach(var i in dataIndex.Indexes) {
                if(i == -1) {
                    empty++;
                }
            }

            System.Diagnostics.Debug.WriteLine($"Empty bags {empty}");

            return counter;
        }

        long totalLoop = 0;

        long totalComputeIndex = 0;
        long computeIndexMax = 0;

        long totalSaveNew1 = 0;
        long totalSaveNew1Max = 0;
        long counterSaveNew1 = 0;

        long totalSaveNew2 = 0;
        long totalSaveNew2Max = 0;
        long counterSaveNew2 = 0;

        long totatGetEqualOrLastNode = 0;
        long getEqualOrLastNodeMax = 0;
        long counterGetEqualOrLastNode = 0;

        long totatUpdate1 = 0;
        long totatUpdate1Max = 0;
        long counterUpdate1 = 0;

        long totatUpdate2 = 0;
        long totatUpdate2Max = 0;
        long counterUpdate2 = 0;
        private void FillBagFile() {
            int index = 0;
            long totalRead = 0;
            long totalReadMax = 0;
            long totatSave = 0;
            long totatSaveMax = 0;
            var sw = new System.Diagnostics.Stopwatch();
            while (true) {
                if (index == -1 || inputFile.IsEOF(index)) {
                    break;
                }

                sw.Restart();

                var (h, i) = ReadNextString(index);
                index = i;

                sw.Stop();
                totalRead += sw.ElapsedMilliseconds;
                totalReadMax = Math.Max(totalReadMax, sw.ElapsedMilliseconds);


                sw.Restart();

                SaveNextHash(h);

                sw.Stop();
                totatSave += sw.ElapsedMilliseconds;
                totatSaveMax = Math.Max(totatSaveMax, sw.ElapsedMilliseconds);

                totalLoop++;
            }
            fileManager.Close();
            var avg1 = (double) totalRead / totalLoop;
            var avg2 = (double) totatSave / totalLoop;
            System.Diagnostics.Debug.WriteLine($"ReadNextString {avg1} max {totalReadMax}");
            System.Diagnostics.Debug.WriteLine($"SaveNextHash {avg2} max {totatSaveMax}");
            System.Diagnostics.Debug.WriteLine($"totalLoop {totalLoop}");

            //long totalComputeIndex = 0;
            //long totalSaveNew1 = 0;
            //long totalSaveNew2 = 0;
            //long counterSaveNew1 = 0;
            //long counterSaveNew2 = 0;
            //long totatGetEqualOrLastNode = 0;
            //long counterGetEqualOrLastNode = 0;
            //long totatUpdate1 = 0;
            //long counterUpdate1 = 0;
            //long totatUpdate2 = 0;
            //long counterUpdate2 = 0;
            System.Diagnostics.Debug.WriteLine($"Avg ComputeIndex: {(double)totalComputeIndex/ totalLoop} max: {computeIndexMax} iterations: {totalLoop}");
            System.Diagnostics.Debug.WriteLine($"Avg SaveNew1: {(double)totalSaveNew1 / counterSaveNew1} max: {totalSaveNew1Max}  iterations: {counterSaveNew1}");
            System.Diagnostics.Debug.WriteLine($"Avg SaveNew2: {(double)totalSaveNew2 / counterSaveNew2} max: {totalSaveNew2Max}  iterations: {counterSaveNew2}");
            System.Diagnostics.Debug.WriteLine($"Avg GetEqualOrLastNode: {(double)totatGetEqualOrLastNode / counterGetEqualOrLastNode} max: {getEqualOrLastNodeMax}  iterations: {counterGetEqualOrLastNode}");
            System.Diagnostics.Debug.WriteLine($"Avg Update1: {(double)totatUpdate1 / counterUpdate1} max: {totatUpdate1Max} iterations: {counterUpdate1}");
            System.Diagnostics.Debug.WriteLine($"Avg Update2: {(double)totatUpdate2 / counterUpdate2} max: {totatUpdate2Max} iterations: {counterUpdate2}");
            
        }

        private (byte[], int) ReadNextString(int idx) {
            List<byte> buffer = new();
            while (!inputFile.IsEOF(idx) && !inputFile.IsLineBreak(idx)) {
                var c = inputFile.GetCurrentByte(idx);
                buffer.Add(c);
                if (buffer.Count >= bufferSize) {
                    var intermedateHash = Utils.ComputeSha256Hash(buffer.ToArray());
                    buffer.Clear();
                    buffer.AddRange(intermedateHash);
                }
                idx++;
            }
            int resultIndex = inputFile.IsEOF(idx) ? -1 : idx;
            if(resultIndex != -1 && inputFile.IsLineBreak(idx)) {
                resultIndex++;
            }
            var hash = Utils.ComputeSha256Hash(buffer.ToArray());
            return (hash, resultIndex);
        }


        Stopwatch ssw = new System.Diagnostics.Stopwatch();
        private void SaveNextHash(byte[] hash) {
            ssw.Restart();


            var idx = Utils.ComputeIndex(hash);


            ssw.Stop();
            totalComputeIndex += ssw.ElapsedMilliseconds;
            computeIndexMax = Math.Max(computeIndexMax, ssw.ElapsedMilliseconds);


            var currentPosition = dataIndex.GetPosition(idx);
            if (currentPosition == -1) {
                ssw.Restart();

                long newPosition = fileManager.SaveNew(hash);

                ssw.Stop();
                totalSaveNew1 += ssw.ElapsedMilliseconds;
                counterSaveNew1++;
                totalSaveNew1Max = Math.Max(totalSaveNew1Max, ssw.ElapsedMilliseconds);

                dataIndex.SavePosition(idx, newPosition);
            } else {
                while (true) {
                    ssw.Restart();
                    
                    (NodeItem storedNode, long storedNodePos) = fileManager.GetEqualOrLastNode(currentPosition, hash);
                    
                    ssw.Stop();
                    totatGetEqualOrLastNode += ssw.ElapsedMilliseconds;
                    getEqualOrLastNodeMax = Math.Max(getEqualOrLastNodeMax, ssw.ElapsedMilliseconds);
                    counterGetEqualOrLastNode++;

                    if (Enumerable.SequenceEqual(storedNode.Hash, hash)) {
                        storedNode.Count++;

                        ssw.Restart();

                        fileManager.Update(storedNodePos, storedNode);

                        ssw.Stop();
                        totatUpdate1 += ssw.ElapsedMilliseconds;
                        totatUpdate1Max = Math.Max(totatUpdate1Max, ssw.ElapsedMilliseconds);
                        counterUpdate1++;

                        break;
                    } else if (storedNode.Next == -1) {
                        ssw.Restart();

                        storedNode.Next = fileManager.SaveNew(hash);

                        ssw.Stop();
                        totalSaveNew2 += ssw.ElapsedMilliseconds;
                        totalSaveNew2Max = Math.Max(totalSaveNew2Max, ssw.ElapsedMilliseconds);
                        counterSaveNew2++;

                        ssw.Restart();

                        fileManager.Update(storedNodePos, storedNode);


                        ssw.Stop();
                        totatUpdate2 += ssw.ElapsedMilliseconds;
                        totatUpdate2Max = Math.Max(totatUpdate2Max, ssw.ElapsedMilliseconds);
                        counterUpdate2++;

                        break;
                    } else {
                        currentPosition = storedNode.Next;
                    }
                }
            }
        }
    }
}
