using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PerformanceMeasureStadistics;
using PerformanceMeasureStadistics.Serializer;

namespace Neo.PerformanceMeasureStadistics.Test
{
    [TestClass]
    public class PerformanceMeasurerTest
    {
        [TestMethod]
        public void CountTest()
        {
            //Given
            PerformanceMeasurer<IAdapter> performanceMeasurer = new PerformanceMeasurer<IAdapter>("reporte esperado");
            Report reportExpect = new Report("reporte esperado", 2);
            reportExpect.MeasuredCodes[0] = new MeasuredCode()
            {
                Name = "EntityAdapter",
                Seconds = 0,
                MiliSeconds = 0,
                Ticks = 2500,
                CustomMeasures = new List<CustomMeasure>()
                {
                    new CustomMeasure(){ Description = "catalogos de Entity", Measure = 34}
                }
            };
            reportExpect.MeasuredCodes[1] = new MeasuredCode()
            {
                Name = "CalendarAdapter",
                Seconds = 0,
                MiliSeconds = 0,
                Ticks = 2500,
                CustomMeasures = new List<CustomMeasure>()
                {
                    new CustomMeasure(){ Description = "catalogos de Calendar", Measure = 22}
                }
            };
           
            //When
            performanceMeasurer.OnPreMeasure<EntityAdapter>(OnPreMeasureEntity);
            performanceMeasurer.OnPreMeasure<CalendarAdapter>(OnPreMeasureCalendar);
            performanceMeasurer.SetUpAction(AccionAdapter, new EntityAdapter(), new CalendarAdapter());
            Report reportResult = performanceMeasurer.Run();

            IReportSerializer reportSerializer = new ClipBoardReportSerializer();
            reportSerializer.SerializeReport(string.Empty, reportResult);

            //Then
            Assert.AreEqual(reportResult.MeasuredCodes.Length, reportExpect.MeasuredCodes.Length);
            for (int i = 0; i < reportResult.MeasuredCodes.Length; i++)
            {
                MeasuredCode measuredCode = reportResult.MeasuredCodes[i];
                MeasuredCode measuredCodeExpect = reportExpect.MeasuredCodes[i];
                CustomMeasure customMeasure = measuredCode.CustomMeasures[0];
                CustomMeasure customMeasureExpect = measuredCodeExpect.CustomMeasures[0];

                Assert.AreEqual(measuredCode.Name, measuredCodeExpect.Name);
                Assert.AreEqual(customMeasure.Description, customMeasureExpect.Description);
                Assert.AreEqual(customMeasure.Measure, customMeasureExpect.Measure);
                Assert.IsTrue(measuredCode.Ticks > 0);
            }
        }

        #region Private methods

        private const string Value = "some..";

        void AccionAdapter(IAdapter adapter)
        {
            adapter.Define(Value);
            adapter.Append(Value);
        }

        void OnPreMeasureEntity(List<CustomMeasure> customMeasures)
        {
            customMeasures.Add(new CustomMeasure(){ Description = "catalogos de Entity", Measure = 34});
        }

        void OnPreMeasureCalendar(List<CustomMeasure> customMeasures)
        {
            customMeasures.Add(new CustomMeasure(){ Description = "catalogos de Calendar", Measure = 22});
        }

        #endregion

        #region Classes

        interface IAdapter
        {
             void Define(string s);

             void Append(string s);
        }

        class EntityAdapter : IAdapter
        {
            public void Define(string s)
            {
                DoAnyThing();
            }
            
            public void Append(string s)
            {
                DoAnyThing();
            }

            internal static void DoAnyThing()
            {
                for (int i = 0; i < 100000; i++)
                {
                    string dummy = "hello" + "5" + 5;
                }
            }
        }

        class CalendarAdapter : IAdapter
        {
            public void Define(string s)
            {
                EntityAdapter.DoAnyThing();
            }

            public void Append(string s)
            {
                EntityAdapter.DoAnyThing();
            }
        }

        #endregion
    }
}
