using System;
using System.Reflection;

namespace PerformanceInside
{
    public class PerformanceCounter
    {
        public TimeSpan TimeSpan { get; set; }
        public double Memory { get; set; }
        public MethodBase Method { get; internal set; }
        public MethodBase EnvironmentMethod { get; internal set; }  
    }
}
