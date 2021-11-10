using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public static class WeightedRandomExtension
{
    public static int FindFirstIndexGreaterThanOrEqualTo<T>(
                this IList<T> sortedCollection, T key
            ) where T : IComparable<T> {
        int begin = 0;
        int end = sortedCollection.Count;
        while (end > begin) {
            int index = (begin + end) / 2;
            T el = sortedCollection[index];
            if (el.CompareTo(key) >= 0)
                end = index;
            else
                begin = index + 1;
        }
        return end;
    }

    /// <summary>
    /// Cumulative Density Function
    /// IN: [0.1, 0.2, 0.4, 0.3]
    /// OUT [0.1, 0.3, 0.7, 1.0]
    /// </summary>
    /// <returns>CDF array float[]</returns>
    public static float[] ComputeCDF(float[] weights)
    {
        float cdfSum = 0;
        float[] cdfArray = new float[weights.Length];

        for(int i = 0; i < weights.Length; i++)
        {
            cdfSum += weights[i];
            cdfArray[i] = cdfSum;
        }

        return cdfArray;
    }

    
    
    public static int GetRandomWeightedID(float[] weights)
    {
        return FindFirstIndexGreaterThanOrEqualTo(ComputeCDF(weights), Random.Range(0, weights.Sum()));
    }
}
