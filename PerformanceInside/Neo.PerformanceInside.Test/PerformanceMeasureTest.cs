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
            PerformanceMeasure.AddHeaderData("base de datos", "Abengoa");
            PerformanceMeasure.AddHeaderData("Version", "10.6");
            PerformanceMeasure.AddHeaderData("Languages", 2500);
            PerformanceMeasure.CountTime(typeof(PerformanceMeasureTest), () => Process(inputs));
            //then
            string performaceReport = PerformanceMeasure.GetReport();
            Assert.IsNotNull(performaceReport);
            Assert.IsTrue(performaceReport.Contains("<base de datos : Abengoa> <Version : 10.6> <Languages : 2500> "));
            Assert.IsTrue(performaceReport.Contains("<length inputs : 3500> "));

        }

        private static void Process(string[] inputs)
        {
            PerformanceMeasure.AddcustomData("length inputs", inputs.Length);
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
            
            ProcessInputs(inputs);

            //then
            string performaceReport = PerformanceMeasure.GetReport();
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

            Assert.IsNotNull(PerformanceMeasure.GetReport());
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
