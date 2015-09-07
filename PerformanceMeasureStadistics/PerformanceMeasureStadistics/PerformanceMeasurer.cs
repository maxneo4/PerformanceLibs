using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PerformanceMeasureStadistics
{
    public class PerformanceMeasurer<T>
    {
        #region Fields

        private Action<T> _action;
        private T[] _inputs;
        private Stopwatch _stopwatch;
        private readonly Dictionary<Type, Action<List<CustomMeasure>>> _dictionaryCustomPreMeasures;
        private readonly Dictionary<Type, Action<List<CustomMeasure>>> _dictionaryCustomPostMeasures;
        private readonly string _performanceMeasureDescription;

        #endregion

        #region Constructor

        public PerformanceMeasurer(string performanceMeasureDescription)
        {
            _performanceMeasureDescription = performanceMeasureDescription;
            _dictionaryCustomPreMeasures = new Dictionary<Type, Action<List<CustomMeasure>>>();
            _dictionaryCustomPostMeasures = new Dictionary<Type, Action<List<CustomMeasure>>>();
        }

        #endregion

        public Report Run()
        {
            Report report = new Report(_performanceMeasureDescription, _inputs.Length);
            MeasureActionWithInputs(report);
            return report;
        }
        
        public void SetUpAction(Action<T> action, params T[] inputs)
        {
            _action = action;
            _inputs = inputs;
            _stopwatch = new Stopwatch();
        }

        #region Private methods

        private void MeasureActionWithInputs(Report report)
        {
            for (int i = 0; i < _inputs.Length; i++)
            {
                T input = _inputs[i];
                MeasuredCode measuredCode = new MeasuredCode();
                SetNameInMeasuredCode(input, measuredCode);
                RunAndMeasureAction(input, measuredCode);
                report.MeasuredCodes[i] = measuredCode;
            }
            report.End();
        }
        
        private void RunAndMeasureAction(T input, MeasuredCode measuredCode)
        {
            PreMeasureAction(input, measuredCode);
            _stopwatch.Restart();
            _action(input);
            _stopwatch.Stop();
            PostMeasureAction(input, measuredCode);
            SetTimeSpanInMeasureCode(measuredCode);
        }

        private void SetTimeSpanInMeasureCode(MeasuredCode measuredCode)
        {
            TimeSpan result = _stopwatch.Elapsed;
            measuredCode.MiliSeconds = result.Milliseconds;
            measuredCode.Seconds = result.Seconds;
            measuredCode.Ticks = result.Ticks;
        }

        private void PreMeasureAction(T input, MeasuredCode measuredCode)
        {
            if (_dictionaryCustomPreMeasures.ContainsKey(input.GetType()))
            {
                Action<List<CustomMeasure>> onPreMeasure = _dictionaryCustomPreMeasures[input.GetType()];
                onPreMeasure(measuredCode.CustomMeasures);
            }
        }

        private void PostMeasureAction(T input, MeasuredCode measuredCode)
        {
            if (_dictionaryCustomPostMeasures.ContainsKey(input.GetType()))
            {
                Action<List<CustomMeasure>> onPreMeasure = _dictionaryCustomPostMeasures[input.GetType()];
                onPreMeasure(measuredCode.CustomMeasures);
            }
        }

        private static void SetNameInMeasuredCode(T input, MeasuredCode measuredCode)
        {
            Type inputType = input.GetType();
            measuredCode.Name = inputType.Name;
        }

        #endregion

        public void OnPreMeasure<M>(Action<List<CustomMeasure>> onMeasure)
        {
            _dictionaryCustomPreMeasures[typeof (M)] = onMeasure;
        }

        public void OnPostMeasure<M>(Action<List<CustomMeasure>> onMeasure)
        {
            _dictionaryCustomPostMeasures[typeof(M)] = onMeasure;
        }
    }
}
