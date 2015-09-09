using Microsoft.VisualStudio.TestTools.UnitTesting;
using PerformanceInside;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo.PerformanceInside.Test
{
    [TestClass]
    public  class PerformanceMeasureTest
    {

        [TestMethod]
        public void GenerateArrayFromBuildObjectTest()
        {
            //given
            string[] inputs = DataGenerator.GenerateArray(() => Path.GetRandomFileName(), 3500);
            PerformanceMeasure performanceMeasure = PerformanceMeasure.GetPerformanceMeasure();
            //when
            performanceMeasure.BeginMeasure();
            Process(inputs);
            PerformanceCounter performanceCounter = performanceMeasure.EndMeasure();
            //then
            Assert.IsNotNull(performanceCounter);
        }

        private static void Process(string[] inputs)
        {
            string result = "";
            for (int i = 0; i < inputs.Length; i++)
            {
                result += inputs[i];
            }
        }
    }
}
