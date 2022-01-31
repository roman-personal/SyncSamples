using System;
using System.Threading;
using System.Threading.Tasks;

namespace MonitorSample {
    // Пример использования Monitor для предотвращения состояния гонки
    internal class Program {
        static int value = 0;
        static object sync = new object();

        static void Main(string[] args) {
            var tasks = new Task[] { Task.Run(Method1), Task.Run(Method2) };
            Task.WaitAll(tasks);
            Console.WriteLine("Done!");
        }

        static void Method1() {
            int count = 100;
            while (count-- > 0) {
                Monitor.Enter(sync);
                value++;
                Monitor.Exit(sync);
                Thread.Sleep(3);
            }
        }

        static void Method2() {
            int count = 100;
            while (count-- > 0) {
                Monitor.Enter(sync);
                if (value % 2 == 0)
                    Console.WriteLine(value);
                Monitor.Exit(sync);
                Thread.Sleep(1);
            }
        }
    }
}
