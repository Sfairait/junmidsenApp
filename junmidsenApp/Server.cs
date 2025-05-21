using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace junmidsenApp
{
    using System;
    using System.Threading;

    /// <summary>
    /// ReaderWriterLockSlim - обеспечивает:
    /// Множественное чтение: потоки могут одновременно читать (EnterReadLock).
    /// Эксклюзивную запись: только один поток может писать (EnterWriteLock), и пока идет запись, все остальные операции (чтение и запись) блокируются.
    /// try-finally - гарантирует, что блокировка будет освобождена даже в случае исключения.
    /// Cтатическая реализация - класс и все его члены статические.
    /// </summary>
    public static class Server
    {
        private static int count = 0;
        private static readonly ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();

        public static int GetCount()
        {
            rwLock.EnterReadLock();
            try
            {
                return count;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static void AddToCount(int value)
        {
            rwLock.EnterWriteLock();
            try
            {
                count += value;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

    }
}
