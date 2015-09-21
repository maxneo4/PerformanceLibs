using Microsoft.VisualStudio.TestTools.UnitTesting;
using PerformanceInside;
using System.IO;

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
            Assert.IsTrue(performaceReport.Contains("Void MeasureProcessStringArrayTest()"));
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
            PerformanceMeasure.CountTime(typeof(PerformanceMeasureTest), () => result = Acumulate(inputs));
            string performaceReport = PerformanceReport.GetReport();
            //then
            Assert.IsNotNull(performaceReport);
            Assert.IsTrue(performaceReport.Contains("<base de datos : Abengoa> <Version : 10.6> <Languages : 2500> "));
            Assert.IsTrue(performaceReport.Contains("<length inputs : 3500> "));            
            Assert.IsTrue(performaceReport.Contains("Void MeasureProcessStringArrayFunctionTest()"));
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
            string[] inputs = DataGenerator.GenerateArray(() => Path.GetRandomFileName(), 3500);
            //when           
            
            ProcessInputs(inputs);

            //then
            string performaceReport = PerformanceReport.GetReport();
            Assert.IsNotNull(performaceReport);
        }

        private static string _result;

        private static void ProcessInputs(string[] inputs)
        {            
            string result = "";
            for (int i = 0; i < inputs.Length; i++)
            {
                PerformanceMeasure.CountTime(null, () => AddString(inputs, i));
            }
        }

        private static string AddString(string[] inputs, int i)
        {
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
