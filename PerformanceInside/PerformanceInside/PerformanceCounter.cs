using System;
using System.Reflection;

namespace PerformanceInside
{
    public class PerformanceCounter
    {
        public TimeSpan TimeSpan { get; internal set; }        
        public MethodBase Method { get; internal set; }
        public MethodBase EnvironmentMethod { get; internal set; }
        public Type SourceType { get; internal set; }
    }
}
