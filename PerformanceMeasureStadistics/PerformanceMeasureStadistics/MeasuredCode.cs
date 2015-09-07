using System.Collections.Generic;

namespace PerformanceMeasureStadistics
{
    public class MeasuredCode
    {
        #region Constructor

        public MeasuredCode()
        {
            CustomMeasures = new List<CustomMeasure>();
        }

        #endregion

        public string Name { get; set; }
        public int Seconds { get; set; }
        public int MiliSeconds { get; set; }
        public long Ticks { get; set; }
        public List<CustomMeasure> CustomMeasures { get; set; }
    }
}
