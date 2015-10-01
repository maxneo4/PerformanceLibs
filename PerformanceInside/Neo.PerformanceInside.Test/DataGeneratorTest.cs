using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;

namespace Neo.PerformanceInside.Test
{
    [TestClass]
    public class DataGeneratorTest
    {
        [TestMethod]
        public void GenerateArrayFromBuildObjectTest()
        {
            //given
            Func<string> buildString = new Func<string>(()=> Path.GetRandomFileName());
            //when
            string[] randomStrings = DataGenerator.GenerateArray(buildString, 200);
            //then
            Assert.AreEqual(200, randomStrings.Length);
            foreach (string item in randomStrings)
                Assert.IsNotNull(item);
        }

        [TestMethod]
        public void GenerateArrayFromBuildObjectFromPositionTest()
        {
            //given
            Func<int, string> buildString = new Func<int, string>((int i) => i.ToString());
            //when
            string[] generatedStrings = DataGenerator.GenerateArray(buildString, 200);
            //then
            Assert.AreEqual(200, generatedStrings.Length);
            for (int i = 0; i < generatedStrings.Length; i++)
                Assert.AreEqual(i.ToString(), generatedStrings[i]);
        }

        [TestMethod]
        public void GenerateArrayFromSampleObjectsTest()
        {
            //given
            string[] sampleObjects = new string[] { "A", "B", "C" };
            //when
            string[] generatedStrings = DataGenerator.GenerateArray(sampleObjects, 200);
            //then
            Assert.AreEqual(200, generatedStrings.Length);
            Assert.AreEqual("A", generatedStrings[0]);
            Assert.AreEqual("B", generatedStrings[1]);
            Assert.AreEqual("C", generatedStrings[2]);
            Assert.AreEqual("A", generatedStrings[6]);
            Assert.AreEqual("B", generatedStrings[7]);
            Assert.AreEqual("C", generatedStrings[8]);
        }

        [TestMethod]
        public void GeneratEnumerableFromSampleObjectsTest()
        {
            //given
            string[] sampleObjects = new string[] { "A", "B", "C" };
            //when
            IEnumerable<string> generatedStrings = DataGenerator.GenerateEnumerable(sampleObjects, 6);
            IEnumerator<string> enumerator = generatedStrings.GetEnumerator();
            //then
            enumerator.MoveNext();
            Assert.AreEqual("A", enumerator.Current);
            enumerator.MoveNext();
            Assert.AreEqual("B", enumerator.Current);
            enumerator.MoveNext();
            Assert.AreEqual("C", enumerator.Current);
            enumerator.MoveNext();
            Assert.AreEqual("A", enumerator.Current);
            enumerator.MoveNext();
            Assert.AreEqual("B", enumerator.Current);
            enumerator.MoveNext();
            Assert.AreEqual("C", enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());
        }
    }
}
