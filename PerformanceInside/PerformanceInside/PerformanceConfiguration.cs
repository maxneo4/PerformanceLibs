using System;

namespace Neo.PerformanceInside
{
    public static class PerformanceConfiguration
    {
        public static string ReportName { get; set; }
        public static string ParentFolderReport { get; set; }
        public static bool EnabledMeasure { get; set; }
        public static bool AutoOpenReport { get; set; }
        public static bool CopyReportToClipboard { get; set; }

        static PerformanceConfiguration()
        {
            EnabledMeasure = true;
            AutoOpenReport = true;
            CopyReportToClipboard = true;
            ParentFolderReport = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);//Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ReportName = "PerformanceReport";
        }
    }
}
