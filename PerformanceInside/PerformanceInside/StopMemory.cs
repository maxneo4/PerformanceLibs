﻿using System;
using System.Diagnostics;

namespace PerformanceInside
{
    public class StopMemory
    {
        #region Constants

        internal const double BYTES_BY_MEGA = 1048576d;

        #endregion

        #region Fields
        long _beforeMemory,
          _afterMemory;
        #endregion

        #region Properties
        public double Memory { get; private set; }
        #endregion

        #region Public methods
        public void Start()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            _beforeMemory = Process.GetCurrentProcess().VirtualMemorySize64;
        }

        public void Restart()
        {
            Memory = 0;
            Start();
        }

        public void Stop()
        {
            _afterMemory = Process.GetCurrentProcess().VirtualMemorySize64;
            Memory += (_afterMemory - _beforeMemory) / BYTES_BY_MEGA;
        } 
        #endregion
    }
}