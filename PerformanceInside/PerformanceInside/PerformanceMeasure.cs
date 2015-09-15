using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

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
        Dictionary<DictionaryMultipleKeys, Action> _cacheExpressionMethod;
        static PerformanceMeasure _performanceMeasure;            

        #endregion

        #region Properties

        public PerformanceCounter PerformanceCounter { get { return _performanceCounter; } }
        public double MemoryCounter { get; private set; }

        #endregion

        private PerformanceMeasure()
        {
            _stopWatch = new Stopwatch();
            _performanceCounter = new PerformanceCounter();
            _cacheExpressionMethod = new Dictionary<DictionaryMultipleKeys, Action>();      
        }

        public static PerformanceMeasure GetPerformanceMeasure()
        {
            return _performanceMeasure ?? (_performanceMeasure = new PerformanceMeasure());
        }

        public void TakePerformanceMeasure(object sourceObject, Expression < Action> actionCallExp)
        {            
            MethodCallExpression methodCallExp = (MethodCallExpression)actionCallExp.Body;
            DictionaryMultipleKeys key = new DictionaryMultipleKeys(sourceObject, methodCallExp.Method.Name);
            Action action = _cacheExpressionMethod.ContainsKey(key)?
            _cacheExpressionMethod[key] : _cacheExpressionMethod[key] = actionCallExp.Compile();            
            Run(action);
            PerformanceCounter.SourceType = sourceObject!=null? 
                (sourceObject is Type? (Type)sourceObject : 
                sourceObject.GetType()) : null;
            _performanceCounter.Method = methodCallExp.Method;
            _performanceCounter.EnvironmentMethod = new StackTrace().GetFrame(1).GetMethod();           
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
            MemoryCounter = (_afterMemory - _beforeMemory)/ BYTES_BY_MEGA; 
        }
        
    }
}
