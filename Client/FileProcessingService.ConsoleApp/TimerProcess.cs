using System.Threading;

namespace FileProcessingService.ConsoleApp
{
    public class TimerProcess
    {
        private const long TimerInterval = 10000;

        private static object _locker = new object();
        private static Timer _timer;

        public void Start()
        {
            _timer = new Timer(Callback, null, 0, TimerInterval);
        }

        public void Stop()
        {
            _timer.Dispose();
        }

        public void Callback(object state)
        {
            var hasLock = false;

            try
            {
                Monitor.TryEnter(_locker, ref hasLock);
                if (!hasLock)
                {
                    return;
                }
                _timer.Change(Timeout.Infinite, Timeout.Infinite);

                // Do something
            }
            finally
            {
                if (hasLock)
                {
                    Monitor.Exit(_locker);
                    _timer.Change(TimerInterval, TimerInterval);
                }
            }
        }
    }
}
