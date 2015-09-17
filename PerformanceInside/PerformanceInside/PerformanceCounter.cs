using System;
using System.Reflection;
using System.Text;

namespace PerformanceInside
{
    public class PerformanceCounter
    {
        internal StringBuilder _customData;

        public TimeSpan TimeSpan { get; internal set; }        
        public MethodBase Method { get; internal set; }
        public MethodBase EnvironmentMethod { get; internal set; }
        public Type SourceType { get; internal set; }
        public double Memory { get; internal set; }
        public int Iteration { get; internal set; }

        public PerformanceCounter()
        {
            _customData = new StringBuilder();
        }
    }
}
