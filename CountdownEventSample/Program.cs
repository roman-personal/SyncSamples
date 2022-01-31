using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CountdownEventSample {
    // Ожидание окончания работы нескольких консьюмеров при помощи CountdownEvent
    internal class Program {
        const int ItemsCount = 10000;
        static ConcurrentQueue<int> queue;
        static CountdownEvent cde;

        static void Main(string[] args) {
            queue = new ConcurrentQueue<int>(Enumerable.Range(0, ItemsCount));
            using (cde = new CountdownEvent(ItemsCount)) {
                var tasks = new Task[] { Task.Run(Consumer), Task.Run(Consumer), Task.Run(Consumer), Task.Run(Consumer) };
                cde.Wait();
            }
            Console.WriteLine("Done!");
        }

        static void Consumer() {
            int local;
            while (queue.TryDequeue(out local)) {
                // do something with local value
                cde.Signal();
            }
        }
    }
}
