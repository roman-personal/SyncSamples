using System;
using System.Threading;
using System.Threading.Tasks;

namespace InterlockedSample {
    internal class Program {
        static int value = 0;

        static void Main(string[] args) {
            var tasks = new Task[] { Task.Run(DoIncrement), Task.Run(DoIncrement) };
            Task.WaitAll(tasks);
            Console.WriteLine($"Done! Value: {value}");
        }

        static void DoIncrement() {
            int count = 5000;
            while (count-- > 0)
                Interlocked.Increment(ref value);
        }
    }
}
