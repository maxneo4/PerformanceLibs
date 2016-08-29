using System;
using System.Diagnostics;

namespace Neo.PerformanceInside
{
    public class StopMemory
    {
        #region Constants

        internal const double BYTES_BY_KB = 1024d;
        private PerformanceCounter clrMemoryCounter;

        #endregion

        #region Constructor

        public StopMemory()
        {
            try { 
            clrMemoryCounter = new PerformanceCounter(".NET CLR Memory", "# Bytes in all Heaps",
                Process.GetCurrentProcess().ProcessName);
            }catch
            {
            }
        }

        #endregion

        #region Fields
        double _beforeMemory,
          _afterMemory;
        #endregion

        #region Properties
        public double Memory { get; private set; }
        public double CurrentMemory { get; private set; }

        public bool UseMemoryPerformanceCounter {
            get {
                return clrMemoryCounter != null;
            }
        }
        #endregion

        #region Public methods
        public void Start()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            _beforeMemory = GetCurrentMemory();            
        }

        public void Restart()
        {
            Memory = 0;
            Start();
        }

        public void Stop()
        {
            _afterMemory = GetCurrentMemory();                     
            Memory += (_afterMemory - _beforeMemory);
        }
        #endregion

        #region Private methods

        private double GetCurrentMemory()
        {
            CurrentMemory = UseMemoryPerformanceCounter?
                Math.Round(clrMemoryCounter.NextSample().RawValue / BYTES_BY_KB):
                Process.GetCurrentProcess().PrivateMemorySize64 / BYTES_BY_KB;
            return CurrentMemory;
        }

        #endregion
    }
}
