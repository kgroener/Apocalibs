using System;

namespace Apocalibs.Core.Types
{
    /// <summary>
    /// Represents a value from 0 to 1
    /// </summary>
    public struct OneRange
    {
        private double _value;

        public OneRange(double value)
        {
            _value = value;

            ValidateValue(value);
        }

        private void ValidateValue(double value)
        {
            if (value < 0 || value > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Factor value must be between 0 and 1");
            }
        }

        public static implicit operator double(OneRange f)
        {
            return f._value;
        }

        public static implicit operator OneRange(double d)
        {
            return new OneRange(d);
        }

        public static OneRange operator +(OneRange a, OneRange b)
        {
            return new OneRange(a._value + b._value);
        }

        public static OneRange operator -(OneRange a, OneRange b)
        {
            return new OneRange(a._value - b._value);
        }

        public static OneRange operator *(OneRange a, OneRange b)
        {
            return new OneRange(a._value * b._value);
        }

        public static OneRange operator /(OneRange a, OneRange b)
        {
            return new OneRange(a._value / b._value);
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}
