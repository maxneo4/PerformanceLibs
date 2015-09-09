using System;
using System.Diagnostics;
using Microsoft.VisualBasic.Devices;
using System.Linq.Expressions;

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

        public PerformanceCounter TakePerformanceCounter(Expression<Action> actionCallExp)
        {
            MethodCallExpression methodCallExp = (MethodCallExpression)actionCallExp.Body;
            Action action = actionCallExp.Compile();
            BeginMemoryMeasure();
            _stopWatch.Restart();
            action.Invoke();            
            _performanceCounter.Method = methodCallExp.Method;
            return EndMeasure();
        }

        private void BeginMemoryMeasure()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            _beforeMemory = Process.GetCurrentProcess().VirtualMemorySize64;
        }

        private PerformanceCounter EndMeasure()
        {
            _stopWatch.Stop();
            _afterMemory = Process.GetCurrentProcess().VirtualMemorySize64;
            _performanceCounter.Memory = (_afterMemory - _beforeMemory)/ 1048576d;
            _performanceCounter.TimeSpan = _stopWatch.Elapsed;
            _performanceCounter.AvailablePhysicalMemory = _computerInfo.AvailablePhysicalMemory/ 1048576d;
            //_performanceCounter.Method = new StackTrace(0).GetFrame(1).GetMethod();
            return _performanceCounter;
        }
    }
}
