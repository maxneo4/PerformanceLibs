namespace PerformanceMeasureStadistics
{
    public class Report
    {

        #region Fields

        public string Description { get; private set; }
        private int _totalSeconds;
        private int _totalMiliSeconds;

        #endregion
        
        public MeasuredCode[] MeasuredCodes { get; private set; }

        public int TotalSeconds 
        {
            get { return _totalSeconds; }
        }

        public int TotalMiliSeconds
        {
            get { return _totalMiliSeconds; }
        }

        #region Constructor


        public Report(string description, int measureCodesLength)
        {
            MeasuredCodes = new MeasuredCode[measureCodesLength];
            Description = description;
        }

        #endregion

        public void End()
        {
            _totalSeconds = 0;
            _totalMiliSeconds = 0;
            foreach (MeasuredCode measuredCode in MeasuredCodes)
            {
                _totalSeconds += measuredCode.Seconds;
                _totalMiliSeconds += measuredCode.MiliSeconds;
            }
        }
    }
}
