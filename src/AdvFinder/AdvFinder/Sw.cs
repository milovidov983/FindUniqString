using System;
using System.Diagnostics;

namespace AdvFinder {
	public partial class AdvancedFinder {
		public class Sw {
            private System.Diagnostics.Stopwatch sw = new Stopwatch();
            private System.Diagnostics.Stopwatch shortSw = new Stopwatch();
            private int counter = 0;
            private long msTotal = 0;
            private long min = 0;
            private long max = 0;
			private string name;
            

			public Sw(string name) {
                this.name = name;
			}
            public void Restart() {
                shortSw.Restart();
                sw.Start();
            }
          
            public void Stop() {
                counter++;
                sw.Stop();
                shortSw.Stop();

                min = Math.Min(min, shortSw.ElapsedMilliseconds);
                max = Math.Max(max, shortSw.ElapsedMilliseconds);
            }

            public (int c, long ms, long min, long max) GetResult() {
                return (counter, msTotal, min, max);
			}

			public override string ToString() {
                return $"{name} AvgMs = {(double)sw.ElapsedMilliseconds / counter} ms, TotalMs = {sw.ElapsedMilliseconds} ms, Max = {max} ms, Min = {min} ms, Count = {counter}";
			}

		}
    }
}
