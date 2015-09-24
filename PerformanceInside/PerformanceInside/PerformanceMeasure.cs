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
                
        internal static Dictionary<Delegate, PerformanceMeasure> _performanceMeasureByDelegate;
        internal static PerformanceMeasure _currentPerformanceMeasure;
        internal static PerformanceMeasure GetPerformanceMeasure(Delegate func)
        {
            _currentPerformanceMeasure = _performanceMeasureByDelegate.ContainsKey(func) ? _performanceMeasureByDelegate[func] : (_performanceMeasureByDelegate[func] = new PerformanceMeasure());
            return _currentPerformanceMeasure;
       }

        #region Constructor

        static PerformanceMeasure()
        {
            _performanceMeasureByDelegate = new Dictionary<Delegate, PerformanceMeasure>();
        }

        private PerformanceMeasure()
        {
            _stopWatch = new Stopwatch();
            _stopMemory = new StopMemory();            
            _perfomanceCounters = new List<PerformanceCounter>();                           
        }       

        #endregion

        public static void CountTime(object sourceObject, Action action, int iterationPack = 1, [CallerMemberName]string caller = "None")
        {            
            PerformanceMeasure performanceMeasure = GetPerformanceMeasure(action);
            performanceMeasure.TakePerformanceMeasure(sourceObject, action, iterationPack, caller);
        }

        public static void CountTime(object sourceObject, Func<object> func, int iterationPack = 1, [CallerMemberName]string caller = "None")
        {
            PerformanceMeasure performanceMeasure = GetPerformanceMeasure(func);
            performanceMeasure.TakePerformanceMeasure(sourceObject, func, iterationPack, caller);
        }

        public static void CountTimeAndMemory(object sourceObject, Action action, int iterationPack = 1, [CallerMemberName]string caller = "None")
        {
            PerformanceMeasure performanceMeasure = GetPerformanceMeasure(action);
            performanceMeasure.TakePerformanceMeasure(sourceObject, action, iterationPack, caller, true);
        }

        public static void CountTimeAndMemory(object sourceObject, Func<object> func, int iterationPack = 1, [CallerMemberName]string caller = "None")
        {
            PerformanceMeasure performanceMeasure = GetPerformanceMeasure(func);
            performanceMeasure.TakePerformanceMeasure(sourceObject, func, iterationPack, caller, true);
        }

        public static void Reset()
        {
            _performanceMeasureByDelegate = new Dictionary<Delegate, PerformanceMeasure>();
        }

        #region Private methods

        private void TakePerformanceMeasure(object sourceObject, Delegate func, int iterationPack, string caller, bool measureMemory = false)
        {
            IteratePerformanceCounter(iterationPack);
            if (measureMemory) RunAndTakeMemory( ()=>Run(func) );
            else Run(func);
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

        private void RunAndTakeMemory(Action action)
        {
            _stopMemory.Restart();
            action.Invoke();
            _stopMemory.Stop();
            _currentPerformanceCounter.Memory += _stopMemory.Memory;
        }

        #endregion
    }
}
