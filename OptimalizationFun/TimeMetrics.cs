using System;
using System.Diagnostics;

namespace OptimalizationFun
{
    public class TimeMetrics : IDisposable
    {
        private readonly string _message;
        private readonly Stopwatch _watch;

        public TimeMetrics(string message)
        {
            _message = message;
            _watch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            _watch.Stop();
            Console.WriteLine("{0} {1}ms.", _message, _watch.ElapsedMilliseconds);
        }
    }
}