using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Neo.PerformanceInside
{
    public class PerformanceReport
    {       
        #region Fields

        internal static StringBuilder _headerData;
        internal static StringBuilder _reportData;

        public static bool AutoOpenReport { get; set; }

        #endregion

        #region Constructor

        static PerformanceReport()
        {
            _headerData = new StringBuilder();
            _reportData = new StringBuilder();
            AutoOpenReport = true;
        }

        #endregion

        public static void AddHeaderData(string key, object value)
        {
            PerformanceReportWritter.AddDataToStringBuilder(_headerData, key, value);
        }

        public static void AddCustomData(string key, object value)
        {
            if (PerformanceMeasure._currentPerformanceMeasure == null) return;
            PerformanceMeasure performanceMeasure = PerformanceMeasure._currentPerformanceMeasure;
            PerformanceReportWritter.AddDataToStringBuilder(performanceMeasure._currentPerformanceCounter._customData, key, value);
        }

        public static string GenerateReport(string fileReportPath = null)
        {
            _reportData.AppendLine(_headerData.ToString());
            _reportData.AppendLine(PerformanceReportWritter.reportColumnHeaders);
            foreach (KeyValuePair<Delegate, PerformanceMeasure> performanceMeasure in PerformanceMeasure._performanceMeasureByDelegate)            
                foreach (PerformanceCounter performanceCounter in performanceMeasure.Value._perfomanceCounters)
                    PerformanceReportWritter.AddPerformanceCounterToStrigBuilder(_reportData, performanceCounter);
            string report = _reportData.ToString();
            string pathReport = fileReportPath ?? "PeformanceReport.csv";
            File.AppendAllText(pathReport, report);
            if (AutoOpenReport)
                System.Diagnostics.Process.Start(pathReport);
            return report;
        }     
        
        public static void Clear()
        {
            _headerData.Clear();
            _reportData.Clear();
        }   
    }
}
