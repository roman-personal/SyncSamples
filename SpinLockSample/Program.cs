using System;
using System.Threading;
using System.Threading.Tasks;

namespace SpinLockSample {
    // Использование SpinLock для предотвращения состояния гонки
    internal class Program {
        static int value = 0;
        static SpinLock spinlock = new SpinLock();

        static void Main(string[] args) {
            var tasks = new Task[] { Task.Run(Method1), Task.Run(Method2) };
            Task.WaitAll(tasks);
            Console.WriteLine("Done!");
        }

        static void Method1() {
            int count = 100;
            while (count-- > 0) {
                bool lockTaken = false;
                try {
                    spinlock.Enter(ref lockTaken);
                    value++;
                }
                finally {
                    if (lockTaken)
                        spinlock.Exit();
                }
                Thread.Sleep(3);
            }
        }

        static void Method2() {
            int count = 100;
            while (count-- > 0) {
                int localValue;
                bool lockTaken = false;
                try {
                    spinlock.Enter(ref lockTaken);
                    localValue = value;
                }
                finally {
                    if (lockTaken)
                        spinlock.Exit();
                }
                if (localValue % 2 == 0)
                    Console.WriteLine(localValue);
                Thread.Sleep(1);
            }
        }
    }
}
