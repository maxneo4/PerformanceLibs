namespace PerformanceMeasureStadistics.Serializer
{
    public interface IReportSerializer
    {
        void SerializeReport(string path = null, params Report[] reports);
    }
}
