using System;
using System.Text;

namespace Neo.PerformanceInside
{
    internal class NeoPerformanceCounter
    {
        internal StringBuilder _customData;

        public TimeSpan TimeSpan { get; internal set; }  
        public string CallerMethod { get; internal set; }
        public string Source { get; internal set; }
        public double Memory { get; internal set; }
        public int Iteration { get; internal set; }

        internal int PackagePosition { get; set; }

        public NeoPerformanceCounter(int iterationInitial)
        {
            Iteration = iterationInitial;
            _customData = new StringBuilder();                        
        }

        internal void FillData(object sourceObject, string caller)
        {
            Source = sourceObject != null ?
                (sourceObject is Type? ((Type)sourceObject).Name :
                sourceObject is string ? sourceObject.ToString() :                
                sourceObject.GetType().Name.ToString()) : null;                                  
            CallerMethod = caller;
            Iteration++;
        }
    }
}
