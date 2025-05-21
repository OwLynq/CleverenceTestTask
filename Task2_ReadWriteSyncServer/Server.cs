namespace Task2_ReadWriteSyncServer
{
    public static class Server
    {
        private static int count = 0;
        private static ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();

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
