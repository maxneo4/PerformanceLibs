using Microsoft.VisualStudio.TestTools.UnitTesting;
using PerformanceInside;
using System.IO;

namespace Neo.PerformanceInside.Test
{
    [TestClass]
    public  class PerformanceMeasureTest
    {
        
        [TestCleanup]
        public void Initialize()
        {
            PerformanceMeasure.Reset();
            PerformanceReport.Clear();
        }

        #region ArrayTest
        [TestMethod]
        public void MeasureProcessStringArrayTest()
        {
            //given
            string[] inputs = DataGenerator.GenerateArray(() => Path.GetRandomFileName(), 3500);
            //when
            PerformanceReport.AddHeaderData("base de datos", "Abengoa");
            PerformanceReport.AddHeaderData("Version", "10.6");
            PerformanceReport.AddHeaderData("Languages", 2500);
            PerformanceMeasure.CountTime(typeof(PerformanceMeasureTest), () => Process(inputs));
            string performaceReport = PerformanceReport.GetReport();
            //then
            Assert.IsNotNull(performaceReport);
            Assert.IsTrue(performaceReport.Contains("<base de datos : Abengoa> <Version : 10.6> <Languages : 2500> "));
            Assert.IsTrue(performaceReport.Contains("<length inputs : 3500> "));            
            Assert.IsTrue(performaceReport.Contains("MeasureProcessStringArrayTest"));
        }       

        private void Process(string[] inputs)
        {
            PerformanceReport.AddCustomData("length inputs", inputs.Length);
            string result = "";
            for (int i = 0; i < inputs.Length; i++)
            {
                result += inputs[i];
            }
        }
        #endregion

        #region ArrayFunctionTest
        [TestMethod]
        public void MeasureProcessStringArrayFunctionTest()
        {
            //given
            string[] inputs = DataGenerator.GenerateArray(() => Path.GetRandomFileName(), 3500);
            string result = null;
            //when
            PerformanceReport.AddHeaderData("base de datos", "Abengoa");
            PerformanceReport.AddHeaderData("Version", "10.6");
            PerformanceReport.AddHeaderData("Languages", 2500);
            PerformanceMeasure.CountTimeAndMemory(typeof(PerformanceMeasureTest), () => result = Acumulate(inputs));
            string performaceReport = PerformanceReport.GetReport();
            //then
            Assert.IsNotNull(performaceReport);
            Assert.IsTrue(performaceReport.Contains("<base de datos : Abengoa> <Version : 10.6> <Languages : 2500> "));
            Assert.IsTrue(performaceReport.Contains("<length inputs : 3500> "));            
            Assert.IsTrue(performaceReport.Contains("MeasureProcessStringArrayFunctionTest"));
        }

        private static string Acumulate(string[] inputs)
        {
            PerformanceReport.AddCustomData("length inputs", inputs.Length);
            string result = "";
            for (int i = 0; i < inputs.Length; i++)
            {
                result += inputs[i];
            }
            return result;
        }
        #endregion


        #region Every
        [TestMethod]
        public void MeasureEveryIterationInProcessStringTest()
        {
            //given
            string[] inputs = DataGenerator.GenerateArray(() => Path.GetRandomFileName(), 6);
            //when           
            
            ProcessInputs(inputs, 2);

            //then
            string performaceReport = PerformanceReport.GetReport();
            Assert.IsTrue(performaceReport.Split('\n').Length == 3 + 3);
        }

        [TestMethod]
        public void MeasureEveryIterationInProcessStringResidueTest()
        {
            //given
            string[] inputs = DataGenerator.GenerateArray(() => Path.GetRandomFileName(), 11);
            //when           

            ProcessInputs(inputs, 3);

            //then
            string performaceReport = PerformanceReport.GetReport();
            Assert.IsTrue(performaceReport.Split('\n').Length == 4 + 3);
        }

        [TestMethod]
        public void MeasureEveryIterationInProcessStringAcumulativeTest()
        {
            //given
            string[] inputs = DataGenerator.GenerateArray(() => Path.GetRandomFileName(), 5000);
            //when           

            PerformanceMeasure.CountTimeAndMemory(this, () => ProcessInputs(inputs, 100));//100

            //then
            string performaceReport = PerformanceReport.GetReport();
            Assert.IsTrue(performaceReport.Split('\n').Length == 50 + 3);
        }

        private static string _result;

        private static void ProcessInputs(string[] inputs, int every)
        {            
            string result = "";
            for (int i = 0; i < inputs.Length; i++)
            {
                AddString(inputs, i);
                PerformanceMeasure.CountTime(typeof(PerformanceMeasureTest), () => AddString(inputs, i), every);
            }
        }

        private static string AddString(string[] inputs, int i)
        {
            inputs[i] = inputs[i].Replace("1", "A");
            inputs[i] = inputs[i] + "ABC" + i + i.ToString() + "hi";
            _result += inputs[i];            
            return _result;
        }
        #endregion

        #region Interfaces implementation

        [TestMethod]
        public void MeasureICustomProcessTest()
        {
            //given            
            
            //when
            RunCustomProcess();
            //then

            Assert.IsNotNull(PerformanceReport.GetReport());
        }
        private void RunCustomProcess()
        {            
            IProcess[] process = new IProcess[] { new ProcessA(), new ProcessB() };
            foreach (IProcess processitem in process)
            {
                PerformanceMeasure.CountTime(processitem, ()=>processitem.Run());
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
