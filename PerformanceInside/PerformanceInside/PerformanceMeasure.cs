using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

namespace PerformanceInside
{
    public class PerformanceMeasure
    {       

        #region Fields

        Stopwatch _stopWatch;
        StopMemory _stopMemory;
        internal PerformanceCounter _currentPerformanceCounter;

        Dictionary<DictionaryMultipleKeys, Action> _cacheExpressionMethod;
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
            _cacheExpressionMethod = new Dictionary<DictionaryMultipleKeys, Action>();            
        }       

        #endregion

        public static void CountTime(object sourceObject, Expression<Action> actionCallExp)
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

        private void TakePerformanceMeasure(object sourceObject, Func<object> actionCallExp)
        {
            //MethodCallExpression methodCallExp = (MethodCallExpression)actionCallExp.Body;
            //DictionaryMultipleKeys key = new DictionaryMultipleKeys(sourceObject, methodCallExp.Method.Name);
            //Action action = _cacheExpressionMethod.ContainsKey(key) ?
            //_cacheExpressionMethod[key] : _cacheExpressionMethod[key] = actionCallExp.Compile();

            //Run(action);

            //_currentPerformanceCounter.FillData(sourceObject, methodCallExp.Method);
        }

        private void TakePerformanceMeasure(object sourceObject, Expression<Action> actionCallExp)
        {
            MethodCallExpression methodCallExp = (MethodCallExpression)actionCallExp.Body;
            DictionaryMultipleKeys key = new DictionaryMultipleKeys(sourceObject, methodCallExp.Method.Name);
            Action action = _cacheExpressionMethod.ContainsKey(key) ?
            _cacheExpressionMethod[key] : _cacheExpressionMethod[key] = actionCallExp.Compile();

            Run(action);

            _currentPerformanceCounter.FillData(sourceObject, methodCallExp.Method);
        }        

        private void Run(Action action)
        {
            _stopWatch.Restart();
            action.Invoke();
            _stopWatch.Stop();
            _currentPerformanceCounter.TimeSpan = _stopWatch.Elapsed;
        }        

        #endregion
    }
}
