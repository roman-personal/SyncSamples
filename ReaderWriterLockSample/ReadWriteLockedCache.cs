using System;
using System.Collections.Generic;
using System.Threading;

namespace ReaderWriterLockSample {
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
            cacheLock.EnterReadLock();
            try {
                return innerCache[key];
            }
            finally {
                cacheLock.ExitReadLock();
            }
        }

        public void Add(int key, string value) {
            cacheLock.EnterWriteLock();
            try {
                innerCache.Add(key, value);
            }
            finally {
                cacheLock.ExitWriteLock();
            }
        }

        public void Modify(int key, string value) {
            cacheLock.EnterUpgradeableReadLock();
            try {
                if (innerCache[key] != value) {
                    cacheLock.EnterWriteLock();
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
