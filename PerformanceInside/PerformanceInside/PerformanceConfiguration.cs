using System;

namespace Neo.PerformanceInside
{
    public static class PerformanceConfiguration
    {
        public static string ReportName { get; set; }
        public static string ParentFolderReport { get; set; }
        public static bool EnabledMeasure { get { return true; }
        set { AutoOpenReport = value; } }
        public static bool AutoOpenReport { get; set; }
        public static bool CopyReportToClipboard { get; set; }

        static PerformanceConfiguration()
        {
            EnabledMeasure = true;
            AutoOpenReport = true;
            CopyReportToClipboard = true;
            ParentFolderReport = @"C:\";//Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ReportName = "PerformanceReport";
        }
    }
}
