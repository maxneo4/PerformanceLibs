using Microsoft.VisualStudio.TestTools.UnitTesting;
using PerformanceInside;
using System.IO;
using System;

namespace Neo.PerformanceInside.Test
{
    [TestClass]
    public  class PerformanceMeasureTest
    {

        #region ArrayTest
        [TestMethod]
        public void MeasureProcessStringArrayTest()
        {
            //given
            string[] inputs = DataGenerator.GenerateArray(() => Path.GetRandomFileName(), 3500);
            PerformanceMeasure performanceMeasure = PerformanceMeasure.GetPerformanceMeasure();
            //when
            performanceMeasure.TakePerformanceMeasure(typeof(PerformanceMeasureTest), () => Process(inputs));
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
        #endregion

        #region Every
        [TestMethod]
        public void MeasureEveryIterationInProcessStringTest()
        {
            //given
            string[] inputs = DataGenerator.GenerateArray(() => Path.GetRandomFileName(), 3500);
            //when
            PerformanceMeasure performanceMeasure = PerformanceMeasure.GetPerformanceMeasure();
            performanceMeasure.StartMemoryMeasure();
            ProcessInputs(inputs);
            performanceMeasure.StopMemoryMeasure();
            //then
            Assert.IsNotNull(performanceMeasure);
        }

        private static string _result;

        private static void ProcessInputs(string[] inputs)
        {
            PerformanceMeasure performanceMeasure = PerformanceMeasure.GetPerformanceMeasure();
            string result = "";
            for (int i = 0; i < inputs.Length; i++)
            {
                performanceMeasure.TakePerformanceMeasure(null, () => AddString(inputs, i));
            }
        }

        private static void AddString(string[] inputs, int i)
        {
            _result += inputs[i];
        }
        #endregion

        #region Interfaces implementation

        [TestMethod]
        public void MeasureICustomProcessTest()
        {
            //given            
            PerformanceMeasure performanceMeasure = PerformanceMeasure.GetPerformanceMeasure();
            //when
            RunCustomProcess();
            //then
            Type cl = performanceMeasure.PerformanceCounter.Method.ReflectedType;
            Assert.IsNotNull(performanceMeasure.PerformanceCounter);
        }
        private void RunCustomProcess()
        {
            PerformanceMeasure performanceMeasure = PerformanceMeasure.GetPerformanceMeasure();
            IProcess[] process = new IProcess[] { new ProcessA(), new ProcessB() };
            foreach (IProcess processitem in process)
            {
                performanceMeasure.TakePerformanceMeasure(processitem, ()=>processitem.Run());
            }
        }

        interface IProcess
        {
            void Run();
        }

        class ProcessA : IProcess
        {
            public void Run()
            {
                for (int i = 0; i < 5000; i++)
                {

                }
            }
        }

        class ProcessB : IProcess
        {
            public void Run()
            {
                for (int i = 0; i < 50000; i++)
                {

                }
            }
        }

        #endregion
    }
}
