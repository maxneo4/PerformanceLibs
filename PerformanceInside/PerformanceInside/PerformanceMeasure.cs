using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.VisualBasic.Devices;

namespace PerformanceInside
{
    public class PerformanceMeasure
    {

        #region Fields

        Stopwatch _stopWatch;
        long _beforeMemory,
            _afterMemory;
        PerformanceCounter _performanceCounter;
        ComputerInfo _computerInfo; 
        #endregion

        private PerformanceMeasure()
        {
            _stopWatch = new Stopwatch();
            _performanceCounter = new PerformanceCounter();
            _computerInfo = new ComputerInfo();
        }

        public static PerformanceMeasure GetPerformanceMeasure()
        {
            return new PerformanceMeasure();
        }

        public void BeginMeasure()
        {
            BeginMemoryMeasure();
            _stopWatch.Restart();
        }

        private void BeginMemoryMeasure()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            _beforeMemory = Process.GetCurrentProcess().VirtualMemorySize64;
        }

        public PerformanceCounter EndMeasure()
        {
            _stopWatch.Stop();
            _afterMemory = Process.GetCurrentProcess().VirtualMemorySize64;
            _performanceCounter.Memory = (_afterMemory - _beforeMemory)/ 1048576d;
            _performanceCounter.TimeSpan = _stopWatch.Elapsed;
            _performanceCounter.AvailablePhysicalMemory = _computerInfo.AvailablePhysicalMemory/ 1048576d;
            _performanceCounter.Method = new StackTrace(0).GetFrame(1).GetMethod();
            return _performanceCounter;
        }
    }
}
