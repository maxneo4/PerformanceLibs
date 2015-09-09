using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceInside
{
    public class PerformanceCounter
    {
        public double AvailablePhysicalMemory { get; internal set; }
        public double Memory { get; set; }
        public MethodBase Method { get; internal set; }
        public TimeSpan TimeSpan { get; set; }

        public void Reset()
        {
            Memory = 0;
        }

    }
}
