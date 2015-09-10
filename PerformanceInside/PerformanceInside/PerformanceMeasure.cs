using System;
using System.Collections.Generic;
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
        Dictionary<string, Action> _cacheExpressionMethod;
        static PerformanceMeasure _performanceMeasure;            

        #endregion

        #region Properties

        public PerformanceCounter PerformanceCounter { get { return _performanceCounter; } }

        #endregion

        private PerformanceMeasure()
        {
            _stopWatch = new Stopwatch();
            _performanceCounter = new PerformanceCounter();
            _cacheExpressionMethod = new Dictionary<string, Action>();      
        }

        public static PerformanceMeasure GetPerformanceMeasure()
        {
            return _performanceMeasure ?? (_performanceMeasure = new PerformanceMeasure());
        }

        public void TakePerformanceMeasure(Expression<Action> actionCallExp)
        {
            MethodCallExpression methodCallExp = (MethodCallExpression)actionCallExp.Body;
            Action action = _cacheExpressionMethod.ContainsKey(methodCallExp.Method.Name)?
            _cacheExpressionMethod[methodCallExp.Method.Name] : _cacheExpressionMethod[methodCallExp.Method.Name] = actionCallExp.Compile();            
            Run(action);            
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

        public void StartMemoryMeasure()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            _beforeMemory = Process.GetCurrentProcess().VirtualMemorySize64;
            
        }

        public void StopMemoryMeasure()
        {  
            _afterMemory = Process.GetCurrentProcess().VirtualMemorySize64;
            _performanceCounter.Memory = (_afterMemory - _beforeMemory)/ BYTES_BY_MEGA; 
        }
    }
}
