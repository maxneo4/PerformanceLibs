using System;
using System.Diagnostics;
using System.IO;

namespace Neo.PerformanceInside
{
    public class PerformanceLite
    {
        #region Fields

        readonly Stopwatch _stopWatch;
        string _outPutFolder;
        string _reportName;
        string _tab;
        string _headers;
        string _row;
        string _pathReport;

        #endregion

        #region Constructor

        public PerformanceLite(string outPutFolder, string reportName)
        {
            _stopWatch = new Stopwatch();
            _outPutFolder = outPutFolder;
            _tab = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            _headers = string.Format("Description{0}Minutes{0}Seconds{0}MiliSeconds{0}TotalMiliSeconds{1}", _tab, Environment.NewLine);
            _row = "{1}{0}{2}{0}{3}{0}{4}{0}{5}"+Environment.NewLine;
            _reportName = string.Format("{0}_{1}.csv", reportName, DateTime.Now.ToString("ddMMMyyyy HH-mm"));
            _pathReport = Path.Combine(_outPutFolder, _reportName);
        }

    #endregion

    #region Public methods 

    public void BeginCount()
    {
        File.AppendAllText(_pathReport, _headers);
        _stopWatch.Start();
    }

    public void CountTime(string description)
    {
        _stopWatch.Stop();
        string row = string.Format(_row, _tab, description, _stopWatch.Elapsed.Minutes, _stopWatch.Elapsed.Seconds, _stopWatch.Elapsed.Milliseconds, _stopWatch.ElapsedMilliseconds);
        File.AppendAllText(_pathReport, row);
        _stopWatch.Restart();
    }

    public void OpenReport()
    {
        Process.Start(_pathReport);
    }
    #endregion

    }
}
