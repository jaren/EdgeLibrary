using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeLibrary
{
    /// <summary>
    /// Represents a range of values.
    /// </summary>
    public struct Range
    {
        public float Min;
        public float Max;

        private float temp;

        public Range(float min, float max)
        {
            Min = min;
            Max = max;
            temp = 0;

            reorder();
        }

        public Range(float number) : this(number, number) { }

        public static Range RangeWithDiffer(float number, float differ)
        {
            return new Range(number - differ/2, number + differ/2);
        }

        public float GetRandom(Random random)
        {
            reorder();
            return random.Next((int)Min, (int)Max);
        }

        private void reorder()
        {
            if (Min > Max)
            {
                temp = Min;
                Min = Max;
                Max = temp;
            }
        }
    }

    /// <summary>
    /// Represents a collection of a range of values.
    /// </summary>
    public struct RangeArray
    {
        public List<Range> Ranges;

        public static implicit operator List<Range>(RangeArray rangeArray)
        {
            return rangeArray.Ranges;
        }

        public RangeArray(params Range[] items)
        {
            Ranges = new List<Range>(items);
        }

        public float[] GetRandom(Random random)
        {
            float[] numbers = new float[Ranges.Count];

            for (int i = 0; i < Ranges.Count; i++)
            {
                numbers[i] = Ranges[i].GetRandom(random);
            }

            return numbers;
        }

        public float GetRandom(int index, Random random)
        {
            return Ranges[index].GetRandom(random);
        }
    }
}
