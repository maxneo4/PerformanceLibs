using System.Text;

namespace PerformanceInside
{
    internal class PerformanceReportWritter
    {
        internal static void AddDataToStringBuilder(StringBuilder stringBuilder, string key, object value)
        {
            stringBuilder.Append("<").Append(key).Append(" : ").Append(value).Append("> ");
        }

        internal static void AddPerformanceCounterToStrigBuilder(StringBuilder stringBuilder, PerformanceCounter performanceCounter)
        {
            const string tab = "\t";
            stringBuilder.Append(performanceCounter.SourceType).Append(tab).Append(performanceCounter.EnvironmentMethod).Append(tab)
                .Append(performanceCounter.TimeSpan.Seconds).Append(tab).
                Append(performanceCounter.TimeSpan.Milliseconds).Append(tab).Append(performanceCounter.Iteration).Append(tab).
                Append(performanceCounter.Memory).Append(tab).Append(performanceCounter._customData).AppendLine();
        }
    }
}
