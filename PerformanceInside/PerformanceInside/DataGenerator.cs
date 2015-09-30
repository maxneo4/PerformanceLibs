using System;
using System.Collections.Generic;

namespace Neo.PerformanceInside
{
    public static class DataGenerator
    {

        public static T[] GenerateArray<T>(Func<T> buildObject, int length)
        {
            T[] arrayObjects = new T[length];
            for (int i = 0; i < length; i++)
                arrayObjects[i] = buildObject();
            return arrayObjects;
        }

        public static T[] GenerateArray<T>(Func<int,T> buildObjectFromPosition, int length)
        {
            T[] arrayObjects = new T[length];
            for (int i = 0; i < length; i++)
                arrayObjects[i] = buildObjectFromPosition(i);
            return arrayObjects;
        }

        public static T[] GenerateArray<T>(T[] sampleObjects, int length)
        {
            T[] arrayObjects = new T[length];
            int lengthSamples = sampleObjects.Length;
            for (int i = 0; i < length; i++)
                arrayObjects[i] = sampleObjects[i% lengthSamples];
            return arrayObjects;
        }

        public static IEnumerable<T> GenerateEnumerable<T>(T[] sampleObjects, int length)
        {
            int sampleObjectsLength = sampleObjects.Length;
            int count = -1;
            while (count < length -1 )
            {
                count++;
                yield return sampleObjects[count % sampleObjectsLength];
            }
        }
    }
}
