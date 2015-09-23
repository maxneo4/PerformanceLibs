using System.Text;

namespace PerformanceInside
{
    public class PerformanceReport
    {
        #region Constants

        internal const string reportColumnHeaders = "SourceType\tEnvironmentMethod\tSeconds\tMiliseconds\tIteration\tMemory\tCustomData";

        #endregion

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
            PerformanceMeasure performanceMeasure = PerformanceMeasure.GetPerformanceMeasure();
            PerformanceReportWritter.AddDataToStringBuilder(performanceMeasure._currentPerformanceCounter._customData, key, value);
        }

        public static string GetReport()
        {
            _reportData.AppendLine(_headerData.ToString());
            _reportData.AppendLine(reportColumnHeaders);
            foreach (PerformanceCounter performanceCounter in PerformanceMeasure.GetPerformanceMeasure()._perfomanceCounters)
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
