using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ChatApp.Match
{
    public struct WriteLock : IDisposable
    {
        ReaderWriterLockSlim obj;
        public WriteLock(ReaderWriterLockSlim lockObj)
        {
            obj = lockObj;
            lockObj.EnterWriteLock();
        }
        public void Dispose()
        {
            obj.ExitWriteLock();
        }
    }
}
