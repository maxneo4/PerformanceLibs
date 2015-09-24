using System;
using System.Collections.Generic;
using System.Text;

namespace PerformanceInside
{
    public class PerformanceReport
    {       
        #region Fields

        internal static StringBuilder _headerData;
        internal static StringBuilder _reportData;

        #endregion

        #region Constructor

        static PerformanceReport()
        {
            _headerData = new StringBuilder();
            _reportData = new StringBuilder();
        }

        #endregion

        public static void AddHeaderData(string key, object value)
        {
            PerformanceReportWritter.AddDataToStringBuilder(_headerData, key, value);
        }

        public static void AddCustomData(string key, object value)
        {
            PerformanceMeasure performanceMeasure = PerformanceMeasure._currentPerformanceMeasure;
            PerformanceReportWritter.AddDataToStringBuilder(performanceMeasure._currentPerformanceCounter._customData, key, value);
        }

        public static string GetReport()
        {
            _reportData.AppendLine(_headerData.ToString());
            _reportData.AppendLine(PerformanceReportWritter.reportColumnHeaders);
            foreach (KeyValuePair<Delegate, PerformanceMeasure> performanceMeasure in PerformanceMeasure._performanceMeasureByDelegate)            
                foreach (PerformanceCounter performanceCounter in performanceMeasure.Value._perfomanceCounters)
                    PerformanceReportWritter.AddPerformanceCounterToStrigBuilder(_reportData, performanceCounter);
            return _reportData.ToString();
        }     
        
        public static void Clear()
        {
            _headerData.Clear();
            _reportData.Clear();
        }   
    }
}
