using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace PerformanceInside
{
    public class PerformanceMeasure
    {

        #region Constants

        internal const double BYTES_BY_MEGA = 1048576d;

        #endregion

        #region Fields

        Stopwatch _stopWatch;
        long _beforeMemory,
            _afterMemory;
        PerformanceCounter _performanceCounter;
        static PerformanceMeasure _performanceMeasure;     

        #endregion

        #region Properties

        public PerformanceCounter PerformanceCounter { get { return _performanceCounter; } }

        #endregion

        private PerformanceMeasure()
        {
            _stopWatch = new Stopwatch();
            _performanceCounter = new PerformanceCounter();            
        }

        public static PerformanceMeasure GetPerformanceMeasure()
        {
            return _performanceMeasure ?? (_performanceMeasure = new PerformanceMeasure());
        }

        public void TakePerformanceMeasure(Expression<Action> actionCallExp)
        {
            MethodCallExpression methodCallExp = (MethodCallExpression)actionCallExp.Body;
            Action action = actionCallExp.Compile();
           // StartMeasure();
            Run(action);
           // StopMeasure();
            _performanceCounter.Method = methodCallExp.Method;
            _performanceCounter.EnvironmentMethod = new StackTrace(1).GetFrame(1).GetMethod();
        }

        private void Run(Action action)
        {
            _stopWatch.Restart();
            action.Invoke();
            _stopWatch.Stop();
            _performanceCounter.TimeSpan = _stopWatch.Elapsed;
        }

        private void StartMeasure()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            _beforeMemory = Process.GetCurrentProcess().VirtualMemorySize64;
            
        }

        private void StopMeasure()
        {  
            _afterMemory = Process.GetCurrentProcess().VirtualMemorySize64;
            _performanceCounter.Memory = (_afterMemory - _beforeMemory)/ BYTES_BY_MEGA; 
        }
    }
}
