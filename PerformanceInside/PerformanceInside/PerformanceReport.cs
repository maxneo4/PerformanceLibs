using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Neo.PerformanceInside
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
            if (PerformanceMeasure._currentPerformanceMeasure == null) return;
            PerformanceMeasure performanceMeasure = PerformanceMeasure._currentPerformanceMeasure;
            PerformanceReportWritter.AddDataToStringBuilder(performanceMeasure._currentPerformanceCounter._customData, key, value);
        }

        public static string GenerateReport()
        {
            if (!PerformanceConfiguration.EnabledMeasure)
                return null;
            FillReportData();
            string report = _reportData.ToString();
            string nameReport = string.Format("{0}_{1}.csv", PerformanceConfiguration.ReportName, DateTime.Now.ToString("ddMMMyyyy HH-mm"));
            string pathReport = Path.Combine(PerformanceConfiguration.ParentFolderReport, nameReport);
            File.AppendAllText(pathReport, report);
            if (PerformanceConfiguration.AutoOpenReport)
                System.Diagnostics.Process.Start(pathReport);
            if (PerformanceConfiguration.CopyReportToClipboard)
                SetReportToClipboard(report);
            return report;
        }

        private static void FillReportData()
        {
            _reportData.AppendLine(_headerData.ToString());
            _reportData.AppendLine(PerformanceReportWritter.reportColumnHeaders);
            foreach (KeyValuePair<DictionaryMultipleKeys, PerformanceMeasure> performanceMeasure in PerformanceMeasure._performanceMeasureByDelegate)
                foreach (PerformanceCounter performanceCounter in performanceMeasure.Value._perfomanceCounters)
                    PerformanceReportWritter.AddPerformanceCounterToStrigBuilder(_reportData, performanceCounter);
        }

        private static void SetReportToClipboard(string report)
        {            
            report = report.Replace(PerformanceReportWritter.tab, "\t");
            try
            { Clipboard.SetText(report); }
            catch { } //Si se intenta correr en un hilo que no sea el de los windows forms           
        }    
        
        public static void Clear()
        {
            _headerData.Clear();
            _reportData.Clear();
        }   
    }
}
