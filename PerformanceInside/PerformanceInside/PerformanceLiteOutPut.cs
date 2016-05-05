using System;
using System.Diagnostics;
using System.IO;

namespace Neo.PerformanceInside
{

    #region Interface OutPut
    internal interface IOutPut
    {
        void Write(string text);
    }

    internal class PerformanceLiteActionOutPut : IOutPut
    {
        Action<string> _action;

        public PerformanceLiteActionOutPut(Action<string> action)
        {
            _action = action;
        }

        public void Write(string text)
        {
            _action(text);
        }
    }
    #endregion

    internal class PerformanceLiteFileOutPut
    {          
        string _reportName;  
        string _pathReport;        

        public PerformanceLiteFileOutPut(string outPutFolder, string reportName)
        {
            _reportName = string.Format("{0}_{1}.csv", reportName, DateTime.Now.ToString("ddMMMyyyy HH-mm"));
            _pathReport = Path.Combine(outPutFolder, _reportName);
        }

        public bool Write(string text)
        {
            try
            {
                File.AppendAllText(_pathReport, text);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void OpenReport()
        {
            Process.Start(_pathReport);
        }
    }    
}
