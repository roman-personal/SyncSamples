using System;
using System.Threading;
using System.Threading.Tasks;

namespace RaceConditionSample {
    // Пример race condition
    // Доступ к переменной int value не контролируется и поэтому программа может напечатать нечетные числа
    internal class Program {
        static int value = 0;

        static void Main(string[] args) {
            var tasks = new Task[] { Task.Run(Method1), Task.Run(Method2) };
            Task.WaitAll(tasks);
            Console.WriteLine("Done!");
        }

        static void Method1() {
            int count = 100;
            while (count-- > 0) {
                value++;
                Thread.Sleep(3);
            }
        }

        static void Method2() {
            int count = 100;
            while (count-- > 0) {
                if (value % 2 == 0)
                    Console.WriteLine(value);
                Thread.Sleep(1);
            }
        }
    }
}
