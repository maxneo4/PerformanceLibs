using System.Windows.Forms;
using Neo.JsonSerializer;

namespace PerformanceMeasureStadistics.Serializer
{
    public class ClipBoardReportSerializer :IReportSerializer
    {
        public void SerializeReport(string path = null, params Report[] reports)
        {
            Clipboard.SetText(reports.ToJson());
        }
    }
}
