using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;

namespace PerformanceInside
{
    public class PerformanceMeasure
    {

        #region Constants

        internal const double BYTES_BY_MEGA = 1048576d;
        internal const string reportColumnHeaders = "SourceType\tEnvironmentMethod\tMethod\tSeconds\tMiliseconds\tIteration\tMemory\tCustomData";

        #endregion

        #region Fields

        Stopwatch _stopWatch;
        long _beforeMemory,
            _afterMemory;
        PerformanceCounter _performanceCounter;
        Dictionary<DictionaryMultipleKeys, Action> _cacheExpressionMethod;
        

        static StringBuilder _headerData;
        static StringBuilder _reportData;
        static PerformanceMeasure _performanceMeasure;

        #endregion

        #region Properties

        public PerformanceCounter PerformanceCounter { get { return _performanceCounter; } }
        public double MemoryCounter { get; private set; }

        #endregion

        #region Constructor

        private PerformanceMeasure()
        {
            _stopWatch = new Stopwatch();
            _performanceCounter = new PerformanceCounter();
            _cacheExpressionMethod = new Dictionary<DictionaryMultipleKeys, Action>();            
        }

        static PerformanceMeasure()
        {
            _headerData = new StringBuilder();
            _reportData = new StringBuilder();
        }

        #endregion

        public static void CountTime(object sourceObject, Expression<Action> actionCallExp)
        {
            PerformanceMeasure performanceMeasure = GetPerformanceMeasure();
            performanceMeasure.TakePerformanceMeasure(sourceObject, actionCallExp);
        }        

        public static void AddcustomData(string key, object value)
        {
            PerformanceMeasure performanceMeasure = GetPerformanceMeasure();
            AddDataToStringBuilder(performanceMeasure.PerformanceCounter._customData, key, value);
        }

        public static void AddHeaderData(string key, object value)
        {
            AddDataToStringBuilder(_headerData, key, value);
        }

        public static string GetReport()
        {
            _reportData.AppendLine(_headerData.ToString());
            _reportData.AppendLine(reportColumnHeaders);
            AddPerformanceCounterToStrigBuilder(_reportData, GetPerformanceMeasure().PerformanceCounter);
            return _reportData.ToString();
        }

        #region Private methods

        private static void AddPerformanceCounterToStrigBuilder(StringBuilder stringBuilder, PerformanceCounter performanceCounter)
        {
            const string tab = "\t";
            stringBuilder.Append(performanceCounter.SourceType).Append(tab).Append(performanceCounter.EnvironmentMethod).Append(tab).
                Append(performanceCounter.Method).Append(tab).Append(performanceCounter.TimeSpan.Seconds).Append(tab).
                Append(performanceCounter.TimeSpan.Milliseconds).Append(tab).Append(performanceCounter.Iteration).Append(tab).
                Append(performanceCounter.Memory).Append(tab).Append(performanceCounter._customData).AppendLine();
        }

        private static void AddDataToStringBuilder(StringBuilder stringBuilder, string key, object value)
        {
            stringBuilder.Append("<").Append(key).Append(" : ").Append(value).Append("> ");
        }

        private static PerformanceMeasure GetPerformanceMeasure()
        {
            return _performanceMeasure ?? (_performanceMeasure = new PerformanceMeasure());
        }

        private void TakePerformanceMeasure(object sourceObject, Expression < Action> actionCallExp)
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
            _performanceCounter.EnvironmentMethod = new StackTrace(1).GetFrame(1).GetMethod();           
        }

        private void Run(Action action)
        {
            _stopWatch.Restart();
            action.Invoke();
            _stopWatch.Stop();
            _performanceCounter.TimeSpan = _stopWatch.Elapsed;
        }

        private void StartMemoryMeasure()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            _beforeMemory = Process.GetCurrentProcess().VirtualMemorySize64;
            
        }

        private void StopMemoryMeasure()
        {  
            _afterMemory = Process.GetCurrentProcess().VirtualMemorySize64;
            MemoryCounter = (_afterMemory - _beforeMemory)/ BYTES_BY_MEGA; 
        }

        #endregion
    }
}
