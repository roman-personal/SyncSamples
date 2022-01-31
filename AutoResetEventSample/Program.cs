using System;
using System.Threading;
using System.Threading.Tasks;

namespace AutoResetEventSample {
    // Пример использования AutoResetEvent для согласования работы потоков
    internal class Program {
        static int value = 0;
        static AutoResetEvent event1;
        static AutoResetEvent event2;

        static void Main(string[] args) {
            using (event1 = new AutoResetEvent(true))
            using (event2 = new AutoResetEvent(false)) {
                var tasks = new Task[] { Task.Run(Method1), Task.Run(Method2) };
                Task.WaitAll(tasks);
            }
            Console.WriteLine("Done!");
        }

        static void Method1() {
            int count = 100;
            while (count-- > 0) {
                event1.WaitOne();
                value++;
                event2.Set();
            }
        }

        static void Method2() {
            int count = 100;
            while (count-- > 0) {
                event2.WaitOne();
                if (value % 2 == 0)
                    Console.WriteLine(value);
                event1.Set();
            }
        }
    }
}
