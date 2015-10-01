using System.Text;

namespace Neo.PerformanceInside
{
    internal class PerformanceReportWritter
    {

        #region Constants

        static string tab = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;
        internal static  readonly string reportColumnHeaders = string.Format("CallerMethod{0}Source{0}Seconds{0}Miliseconds{0}Ticks{0}Iteration{0}Memory(Kb){0}CustomData", tab);

        #endregion

        internal static void AddDataToStringBuilder(StringBuilder stringBuilder, string key, object value)
        {
            stringBuilder.Append("<").Append(key).Append(" : ").Append(value).Append("> ");
        }

        internal static void AddPerformanceCounterToStrigBuilder(StringBuilder stringBuilder, PerformanceCounter performanceCounter)
        {
            
            stringBuilder.Append(performanceCounter.CallerMethod).Append(tab).Append(performanceCounter.Source).Append(tab)
                .Append(performanceCounter.TimeSpan.Seconds).Append(tab).
                Append(performanceCounter.TimeSpan.Milliseconds).Append(tab).Append(performanceCounter.TimeSpan.Ticks).Append(tab).Append(performanceCounter.Iteration).Append(tab).
                Append(performanceCounter.Memory).Append(tab).Append(performanceCounter._customData).AppendLine();
        }
    }
}
