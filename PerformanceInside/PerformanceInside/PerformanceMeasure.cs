using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace PerformanceInside
{
    public class PerformanceMeasure
    {       

        #region Fields

        Stopwatch _stopWatch;
        StopMemory _stopMemory;
        internal PerformanceCounter _currentPerformanceCounter;
        internal List<PerformanceCounter> _perfomanceCounters;
                
        #endregion
                
        static PerformanceMeasure _performanceMeasure;
        internal static PerformanceMeasure GetPerformanceMeasure()
        {
            return _performanceMeasure ?? (_performanceMeasure = new PerformanceMeasure());
        }

        #region Constructor

        private PerformanceMeasure()
        {
            _stopWatch = new Stopwatch();
            _stopMemory = new StopMemory();            
            _perfomanceCounters = new List<PerformanceCounter>();                           
        }       

        #endregion

        public static void CountTime(object sourceObject, Action actionCallExp, int iterationPack = 1, [CallerMemberName]string caller = "None")
        {            
            PerformanceMeasure performanceMeasure = GetPerformanceMeasure();
            performanceMeasure.TakePerformanceMeasure(sourceObject, actionCallExp, iterationPack, caller);
        }

        public static void CountTime(object sourceObject, Func<object> actionCallExp, int iterationPack = 1, [CallerMemberName]string caller = "None")
        {
            PerformanceMeasure performanceMeasure = GetPerformanceMeasure();
            performanceMeasure.TakePerformanceMeasure(sourceObject, actionCallExp, iterationPack, caller);
        }

        public static void Reset()
        {
            _performanceMeasure = null;
        }

        #region Private methods

        private void TakePerformanceMeasure(object sourceObject, Delegate func, int iterationPack, string caller)
        {
            IteratePerformanceCounter(iterationPack);
            Run(func);
            _currentPerformanceCounter.FillData(sourceObject, caller);
        }

        private void IteratePerformanceCounter(int iterationPack)
        {
            bool isTheFirstCounter = _currentPerformanceCounter == null;
            if (isTheFirstCounter || _currentPerformanceCounter.Iteration % iterationPack == 0)
            {
                int iterationInitial = isTheFirstCounter ? 0 : _currentPerformanceCounter.Iteration;
                _currentPerformanceCounter = new PerformanceCounter(iterationInitial);
                _perfomanceCounters.Add(_currentPerformanceCounter);
            }
        }

        private void Run(Delegate func)
        {
            _stopWatch.Restart();
            func.DynamicInvoke();
            _stopWatch.Stop();
            _currentPerformanceCounter.TimeSpan += _stopWatch.Elapsed;
        }    

        #endregion
    }
}
