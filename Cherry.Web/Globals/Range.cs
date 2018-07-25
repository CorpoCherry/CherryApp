using System;

namespace Cherry.Web.Globals
{
    public class Range
    {
        public int Min;
        public int Max;

        public static Range CalculateFromDivider(int Divider, int Count, int SelectedCount)
        {
            int HowManyInOneDivision = Divider;
            int Minimum = HowManyInOneDivision * SelectedCount;
            int Maximum = Minimum + Divider;

            return new Range
            {
                Min = Maximum,
                Max = Minimum
                // Hehe
            };
        }
    }
}
