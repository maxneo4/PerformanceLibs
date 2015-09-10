using Microsoft.VisualStudio.TestTools.UnitTesting;
using PerformanceInside;
using System.IO;

namespace Neo.PerformanceInside.Test
{
    [TestClass]
    public  class PerformanceMeasureTest
    {

        [TestMethod]
        public void MeasureProcessStringArrayTest()
        {
            //given
            string[] inputs = DataGenerator.GenerateArray(() => Path.GetRandomFileName(), 3500);
            PerformanceMeasure performanceMeasure = PerformanceMeasure.GetPerformanceMeasure();
            //when
            performanceMeasure.TakePerformanceMeasure(()=> Process(inputs));            
            //then
            Assert.IsNotNull(performanceMeasure.PerformanceCounter);
        }

        private static void Process(string[] inputs)
        {
            string result = "";
            for (int i = 0; i < inputs.Length; i++)
            {
                result += inputs[i];
            }
        }

        [TestMethod]
        public void MeasureEveryIterationInProcessStringTest()
        {
            //given
            string[] inputs = DataGenerator.GenerateArray(() => Path.GetRandomFileName(), 3500);
            //when
            PerformanceMeasure performanceMeasure = PerformanceMeasure.GetPerformanceMeasure();
            performanceMeasure.StartMemoryMeasure();
            ProcessA(inputs);
            performanceMeasure.StopMemoryMeasure();
            //then
            Assert.IsNotNull(performanceMeasure);
        }

        private static string _result;

        private static void ProcessA(string[] inputs)
        {
            PerformanceMeasure performanceMeasure = PerformanceMeasure.GetPerformanceMeasure();
            string result = "";
            for (int i = 0; i < inputs.Length; i++)
            {
                performanceMeasure.TakePerformanceMeasure(() => AddString(inputs, i));                
            }
        }

        private static void AddString(string[] inputs, int i)
        {
            _result += inputs[i];           
        }
    }
}
