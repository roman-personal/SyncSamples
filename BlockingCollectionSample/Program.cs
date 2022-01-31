using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace BlockingCollectionSample {
    // Пример использования блокирующей коллекции
    // Продьюсер ограничивается количеством элементов в очереди
    // Консьюмеры могут завершиться только если продьюсер закончил добавление и в очереди больше нет элементов
    internal class Program {
        const int MaxQueueSize = 6; // не более 6-ти элементов в очереди
        static BlockingCollection<int> queue;

        static void Main(string[] args) {
            using (queue = new BlockingCollection<int>(MaxQueueSize)) {
                var tasks = new Task[] {
                    Task.Run(ConsumeData),
                    Task.Run(ConsumeData),
                    Task.Run(ConsumeData)
                };
                ProduceData();
                Task.WaitAll(tasks);
            }
            Console.WriteLine("Done!");
        }

        // Добавлять элементы останавливаясь если достигнуто ограничение
        static void ProduceData() {
            for (int i = 0; i < 1000; i++)
                queue.Add(i + 1);
            queue.CompleteAdding();
        }

        // Брать элементы пока не завершено добавление и есть что брать
        static void ConsumeData() {
            int local;
            while (!queue.IsCompleted) {
                if (queue.TryTake(out local)) {
                    Console.WriteLine($"item {local}, count {queue.Count}");
                }
            }
        }
    }
}
