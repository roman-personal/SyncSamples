using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderWriterLockSample {
    internal class Program {
        static ReadWriteLockedCache cache;

        static void Main(string[] args) {
            using (cache = new ReadWriteLockedCache()) {
                var tasks = new Task[] { 
                    Task.Run(() => ReaderMethod(1)), 
                    Task.Run(() => ReaderMethod(2)), 
                    Task.Run(WriterMethod),
                    Task.Run(ModifierMethod)
                };
                Task.WaitAll(tasks);
            }
            Console.WriteLine("Done!");
        }

        static void ReaderMethod(int id) {
            StringBuilder sb = new StringBuilder();
            int count = 30;
            while (count-- > 0) {
                sb.Clear();
                sb.Append($"Reader {id}: ");
                for (int i = 0; i < 5; i++) {
                    if (cache.Contains(i + 1))
                        sb.Append($"{cache.Read(i + 1)} ");
                }
                Console.WriteLine(sb.ToString());
                Thread.Sleep(1);
            }
        }

        static void WriterMethod() {
            int count = 10;
            while (count-- > 0) {
                int key = 10 - count;
                cache.Add(key, $"sample{key}");
                Thread.Sleep(20);
            }
        }

        static void ModifierMethod() {
            int count = 5;
            while (count-- > 0) {
                Thread.Sleep(50);
                int key = 5 - count;
                cache.Modify(key, $"test{key}");
            }
        }

    }
}
