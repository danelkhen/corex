using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Corex.Threading
{
    public class ReaderWriterLockThreadSafe<T>
    {
        public ReaderWriterLockThreadSafe(T value)
        {
            Value = value;
            Timeout = 1000;
        }

        T Value;
        ReaderWriterLock Lock = new ReaderWriterLock();
        public int Timeout { get; set; }
        public void Read(Action<T> action)
        {
            Lock.AcquireReaderLock(Timeout);
            try
            {
                action(Value);
            }
            finally
            {
                Lock.ReleaseReaderLock();
            }
        }
        public void UpgradeOrWrite(Action<T> action)
        {
            if (!Lock.IsWriterLockHeld)
            {
                Write(action);
                return;
            }
            Upgrade(action);
        }

        private void Upgrade(Action<T> action)
        {
            var cookie = Lock.UpgradeToWriterLock(Timeout);
            try
            {
                action(Value);
            }
            finally
            {
                Lock.DowngradeFromWriterLock(ref cookie);
            }
        }
        public void Write(Action<T> action)
        {
            LockCookie cookie;
            bool hasCookie;
            if (Lock.IsWriterLockHeld)
            {
                cookie = Lock.UpgradeToWriterLock(Timeout);
                hasCookie = true;
            }
            else
            {
                Lock.AcquireWriterLock(Timeout);
                cookie = new LockCookie();
                hasCookie = false;
            }
            try
            {
                action(Value);
            }
            finally
            {
                if (hasCookie)
                    Lock.DowngradeFromWriterLock(ref cookie);
                else
                    Lock.ReleaseWriterLock();
            }
        }
    }
}
