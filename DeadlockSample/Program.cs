using System;
using System.Threading.Tasks;

namespace DeadlockSample {
    // Самый простой пример deadlock
    // Два потока пытаются получить блокировки двух ресурсов в разном порядке
    // В результате ни один, ни второй поток не могут продолжить выполнение, а программа не может завершиться
    internal class Program {
        static object _lockerA = new object();
        static object _lockerB = new object();
        static void Main(string[] args) {
            var tasks = new Task[] { Task.Run(Method1), Task.Run(Method2) };
            Task.WaitAll(tasks);
            Console.WriteLine("Done!");
        }

        static void Method1() {
            lock(_lockerA) {
                Console.WriteLine("Got lock A from Method1");
                lock (_lockerB) {
                    Console.WriteLine("Got lock B from Method1");
                    // do something
                }
            }
        }

        static void Method2() {
            lock (_lockerB) {
                Console.WriteLine("Got lock B from Method2");
                lock (_lockerA) {
                    Console.WriteLine("Got lock A from Method2");
                    // do something
                }
            }
        }
    }
}
