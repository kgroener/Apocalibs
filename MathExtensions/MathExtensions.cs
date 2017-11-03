using System;

namespace Apocalibs.Extensions.Mathematics
{
    public static class MathExtensions
    {
        public static double Clip(this double value, double min, double max)
        {
            return value < min ? min : value > max ? max : value;
        }
    }
}
