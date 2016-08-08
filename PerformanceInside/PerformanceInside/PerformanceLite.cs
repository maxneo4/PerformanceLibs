using System;
using System.Collections.Generic;
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

        private static Dictionary<string,PerformanceLite> _instances = new Dictionary<string, PerformanceLite>();

        public static PerformanceLite GetInstance(string key)
        {
            return _instances[key];
        }

        public static void SetInstance(string key, PerformanceLite performanceLite)
        {
            _instances[key] = performanceLite;
        }
        #endregion

        #region Fields

        Stopwatch _stopWatch;
        StopMemory _stopMemory;        
        string _tab;
        string _headers;
        string _row;
        PerformanceLiteFileOutPut _outPutWriter;
        IOutPut _alternativeOutPutWriter;

        public bool CountMemory { get; set; }

        #endregion

        #region Constructor

        public PerformanceLite(string reportName, Action<string> alternativeActionIfErrorFile = null)
        {
            if (alternativeActionIfErrorFile == null)
                alternativeActionIfErrorFile = (s) => Trace.Write(s);
            _outPutWriter = new PerformanceLiteFileOutPut(OutPutFolder, reportName);            
            _alternativeOutPutWriter = new PerformanceLiteActionOutPut(alternativeActionIfErrorFile);
            _stopWatch = new Stopwatch();            
            _stopMemory = new StopMemory();
            _tab = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            _headers = string.Format("Description{0}Hours{0}Minutes{0}Seconds{0}MiliSeconds{0}TotalMiliSeconds{0}Memory(kb){1}", _tab, Environment.NewLine);
            _row = "{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}" + Environment.NewLine;
        }  

        #endregion

        #region Public methods 

        public void BeginCount()
        {
            Write(_headers);
            _stopWatch.Start();
            if(CountMemory)
                _stopMemory.Start();
        }

        public void CountTime(string description)
        {
            _stopWatch.Stop();
            if (CountMemory)
                _stopMemory.Stop();
            string row = string.Format(_row, _tab, description, _stopWatch.Elapsed.Hours, _stopWatch.Elapsed.Minutes, _stopWatch.Elapsed.Seconds, _stopWatch.Elapsed.Milliseconds, _stopWatch.ElapsedMilliseconds, _stopMemory.Memory);
            Write(row);
            _stopWatch.Restart();
            if (CountMemory)
                _stopMemory.Restart();
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
