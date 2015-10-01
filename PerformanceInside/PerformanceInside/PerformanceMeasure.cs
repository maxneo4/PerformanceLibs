using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Neo.PerformanceInside
{
    public class PerformanceMeasure
    {               

        #region Fields

        Stopwatch _stopWatch;
        StopMemory _stopMemory;
        internal PerformanceCounter _currentPerformanceCounter;
        internal List<PerformanceCounter> _perfomanceCounters;
                
        #endregion
                
        internal static Dictionary<DictionaryMultipleKeys, PerformanceMeasure> _performanceMeasureByDelegate;
        internal static PerformanceMeasure _currentPerformanceMeasure;
        internal static PerformanceMeasure GetPerformanceMeasure(object source, Delegate function)
        {
            DictionaryMultipleKeys func = new DictionaryMultipleKeys(source, function);
            _currentPerformanceMeasure = _performanceMeasureByDelegate.ContainsKey(func) ? _performanceMeasureByDelegate[func] : (_performanceMeasureByDelegate[func] = new PerformanceMeasure());
            return _currentPerformanceMeasure;
       }

        #region Constructor

        static PerformanceMeasure()
        {
            Enabled = true;
            _performanceMeasureByDelegate = new Dictionary<DictionaryMultipleKeys, PerformanceMeasure>();
        }

        private PerformanceMeasure()
        {
            _stopWatch = new Stopwatch();
            _stopMemory = new StopMemory();            
            _perfomanceCounters = new List<PerformanceCounter>();                           
        }

        #endregion

        public static bool Enabled { get; set; }
        internal readonly int[] DEF_ITERATIONPACK = new[] { 1 };

        public static void CountTime(object sourceObject, Action action, int[] iterationPack = null, [CallerMemberName]string caller = "None")
        {
            if (Enabled)
            {
                PerformanceMeasure performanceMeasure = GetPerformanceMeasure(sourceObject, action);
                performanceMeasure.TakePerformanceMeasure(sourceObject, action, iterationPack, caller);
            }
            else action();
        }

        public static void CountTime(object sourceObject, Func<object> func, int[] iterationPack = null, [CallerMemberName]string caller = "None")
        {
            if (Enabled)
            {
                PerformanceMeasure performanceMeasure = GetPerformanceMeasure(sourceObject, func);
                performanceMeasure.TakePerformanceMeasure(sourceObject, func, iterationPack, caller);
            }
            else func();
        }

        public static void CountTimeAndMemory(object sourceObject, Action action, int[] iterationPack = null, [CallerMemberName]string caller = "None")
        {
            if (Enabled)
            {
                PerformanceMeasure performanceMeasure = GetPerformanceMeasure(sourceObject, action);
                performanceMeasure.TakePerformanceMeasure(sourceObject, action, iterationPack, caller, true);
            }
            else action();
        }

        public static void CountTimeAndMemory(object sourceObject, Func<object> func, int[] iterationPack = null, [CallerMemberName]string caller = "None")
        {
            if (Enabled)
            {
                PerformanceMeasure performanceMeasure = GetPerformanceMeasure(sourceObject, func);
                performanceMeasure.TakePerformanceMeasure(sourceObject, func, iterationPack, caller, true);
            }
            else func();
        }

        public static void Reset()
        {
            _performanceMeasureByDelegate = new Dictionary<DictionaryMultipleKeys, PerformanceMeasure>();
        }

        #region Private methods

        private void TakePerformanceMeasure(object sourceObject, Delegate func, int[] iterationPack, string caller, bool measureMemory = false)
        {
            iterationPack = iterationPack?? DEF_ITERATIONPACK;
            IteratePerformanceCounter(iterationPack);
            if (measureMemory) RunAndTakeMemory( ()=>Run(func) );
            else Run(func);
            _currentPerformanceCounter.FillData(sourceObject, caller);
        }

        private void IteratePerformanceCounter(int[] iterationPack)
        {
            bool isTheFirstCounter = _currentPerformanceCounter == null;
            if (isTheFirstCounter || _currentPerformanceCounter.Iteration % iterationPack[_currentPerformanceCounter.PackagePosition] == 0)
            {
                int iterationInitial = isTheFirstCounter ? 0 : _currentPerformanceCounter.Iteration;
                int packagePosition = isTheFirstCounter ? 0 :
                    _currentPerformanceCounter.PackagePosition < iterationPack.Length - 1 ?
                    _currentPerformanceCounter.PackagePosition + 1 : _currentPerformanceCounter.PackagePosition;
                TimeSpan currentTimeSpan = isTheFirstCounter? new TimeSpan() : _currentPerformanceCounter.TimeSpan;

                _currentPerformanceCounter = new PerformanceCounter(iterationInitial);
                _currentPerformanceCounter.PackagePosition = packagePosition;                
                _currentPerformanceCounter.TimeSpan = currentTimeSpan;
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
            action();
            _stopMemory.Stop();
            _currentPerformanceCounter.Memory += _stopMemory.Memory;
        }

        #endregion
    }
}
