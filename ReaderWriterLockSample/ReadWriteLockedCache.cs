using System;
using System.Collections.Generic;
using System.Threading;

namespace ReaderWriterLockSample {
    // Потокобезопасная коллекция с использованием ReaderWriterLockSlim
    internal class ReadWriteLockedCache : IDisposable {
        ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();
        readonly Dictionary<int, string> innerCache = new Dictionary<int, string>();

        public bool Contains(int key) {
            cacheLock.EnterReadLock();
            try {
                return innerCache.ContainsKey(key);
            }
            finally {
                cacheLock.ExitReadLock();
            }
        }

        public string Read(int key) {
            cacheLock.EnterReadLock(); // Блокировка чтения
            try {
                return innerCache[key];
            }
            finally {
                cacheLock.ExitReadLock();
            }
        }

        public void Add(int key, string value) {
            cacheLock.EnterWriteLock(); // Блокировка записи
            try {
                innerCache.Add(key, value);
            }
            finally {
                cacheLock.ExitWriteLock();
            }
        }

        public void Modify(int key, string value) {
            cacheLock.EnterUpgradeableReadLock(); // Обновляемая блокировка на чтение
            try {
                if (innerCache[key] != value) {
                    cacheLock.EnterWriteLock(); // Повышение уровня блокировки
                    try {
                        innerCache[key] = value;
                    }
                    finally {
                        cacheLock.ExitWriteLock();
                    }
                }
            }
            finally {
                cacheLock.ExitUpgradeableReadLock();
            }
        }

        public void Dispose() {
            cacheLock?.Dispose();
            cacheLock = null;
        }
    }
}
