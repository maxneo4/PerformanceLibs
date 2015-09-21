using System;
using System.Diagnostics;

namespace PerformanceInside
{
    public class PerformanceMeasure
    {       

        #region Fields

        Stopwatch _stopWatch;
        StopMemory _stopMemory;
        internal PerformanceCounter _currentPerformanceCounter;
        
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
            _currentPerformanceCounter = new PerformanceCounter();                    
        }       

        #endregion

        public static void CountTime(object sourceObject, Action actionCallExp)
        {
            PerformanceMeasure performanceMeasure = GetPerformanceMeasure();
            performanceMeasure.TakePerformanceMeasure(sourceObject, actionCallExp);
        }

        public static void CountTime(object sourceObject, Func<object> actionCallExp)
        {
            PerformanceMeasure performanceMeasure = GetPerformanceMeasure();
            performanceMeasure.TakePerformanceMeasure(sourceObject, actionCallExp);
        }

        #region Private methods

        private void TakePerformanceMeasure(object sourceObject, Delegate func)
        {
            Run(func);
            _currentPerformanceCounter.FillData(sourceObject);
        }                      

        private void Run(Delegate func)
        {
            _stopWatch.Restart();
            func.DynamicInvoke();
            _stopWatch.Stop();
            _currentPerformanceCounter.TimeSpan = _stopWatch.Elapsed;
        }    

        #endregion
    }
}
