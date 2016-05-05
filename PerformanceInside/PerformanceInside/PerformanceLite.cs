using System;
using System.Diagnostics;

namespace Neo.PerformanceInside
{
    public class PerformanceLite
    {
        #region static Fields
        public static string OutPutFolder
        {
            get; set;
        }
        #endregion

        #region Fields

        Stopwatch _stopWatch;    
        string _tab;
        string _headers;
        string _row;
        PerformanceLiteFileOutPut _outPutWriter;
        IOutPut _alternativeOutPutWriter;

        #endregion

        #region Constructor
                
        public PerformanceLite(string reportName, Action<string> alternativeActionIfErrorFile = null)
        {
            if (alternativeActionIfErrorFile == null)
                alternativeActionIfErrorFile = (s) => Trace.Write(s);
            _outPutWriter = new PerformanceLiteFileOutPut(OutPutFolder, reportName);            
            _alternativeOutPutWriter = new PerformanceLiteActionOutPut(alternativeActionIfErrorFile);
            _stopWatch = new Stopwatch();
            _tab = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            _headers = string.Format("Description{0}Minutes{0}Seconds{0}MiliSeconds{0}TotalMiliSeconds{1}", _tab, Environment.NewLine);
            _row = "{1}{0}{2}{0}{3}{0}{4}{0}{5}" + Environment.NewLine;
        }  

        #endregion

        #region Public methods 

        public void BeginCount()
        {
            Write(_headers);
            _stopWatch.Start();
        }

        public void CountTime(string description)
        {
            _stopWatch.Stop();
            string row = string.Format(_row, _tab, description, _stopWatch.Elapsed.Minutes, _stopWatch.Elapsed.Seconds, _stopWatch.Elapsed.Milliseconds, _stopWatch.ElapsedMilliseconds);
            Write(row);
            _stopWatch.Restart();
        }

        public void OpenReport()
        {
            _outPutWriter.OpenReport();
        }

        #endregion

        #region Private methods

        private void Write(string text)
        {
            if (!_outPutWriter.Write(text))
                _alternativeOutPutWriter.Write(text);
        }

        #endregion
    }
}
