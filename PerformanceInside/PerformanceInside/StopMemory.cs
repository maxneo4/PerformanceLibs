using System;
using System.Diagnostics;

namespace Neo.PerformanceInside
{
    public class StopMemory
    {
        #region Constants

        internal const double BYTES_BY_KB = 1024d;

        #endregion

        #region Fields
        long _beforeMemory,
          _afterMemory;
        #endregion

        #region Properties
        public double Memory { get; private set; }
        public double CurrentMemory { get; private set; }
        #endregion

        #region Public methods
        public void Start()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            _beforeMemory = Process.GetCurrentProcess().VirtualMemorySize64;
            CurrentMemory = _beforeMemory / BYTES_BY_KB;
        }

        public void Restart()
        {
            Memory = 0;
            Start();
        }

        public void Stop()
        {
            _afterMemory = Process.GetCurrentProcess().VirtualMemorySize64;
            CurrentMemory = _afterMemory / BYTES_BY_KB;
            Memory += (_afterMemory - _beforeMemory) / BYTES_BY_KB;
        } 
        #endregion
    }
}
