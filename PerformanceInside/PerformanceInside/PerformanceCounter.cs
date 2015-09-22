using System;
using System.Diagnostics;
using System.Text;

namespace PerformanceInside
{
    public class PerformanceCounter
    {
        internal StringBuilder _customData;

        public TimeSpan TimeSpan { get; internal set; }  
        public string EnvironmentMethod { get; internal set; }
        public Type SourceType { get; internal set; }
        public double Memory { get; internal set; }
        public int Iteration { get; internal set; }

        public PerformanceCounter()
        {
            _customData = new StringBuilder();
        }

        internal void FillData(object sourceObject, string caller)
        {
            SourceType = sourceObject != null ?
                (sourceObject is Type ? (Type)sourceObject :
                sourceObject.GetType()) : null;                                  
            EnvironmentMethod = caller;
        }
    }
}
