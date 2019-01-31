using System;

namespace Apocalibs.Core.Types
{
    /// <summary>
    /// Represents a value from 0 to 1
    /// </summary>
    public struct Scalar
    {
        private double _value;

        public Scalar(double value)
        {
            _value = value;

            ValidateValue(value);
        }

        private void ValidateValue(double value)
        {
            if (value < 0 || value > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "value must be between 0 and 1");
            }
        }

        public static implicit operator double(Scalar f)
        {
            return f._value;
        }

        public static implicit operator Scalar(double d)
        {
            return new Scalar(d);
        }

        public static Scalar operator +(Scalar a, Scalar b)
        {
            return new Scalar(a._value + b._value);
        }

        public static Scalar operator -(Scalar a, Scalar b)
        {
            return new Scalar(a._value - b._value);
        }

        public static Scalar operator *(Scalar a, Scalar b)
        {
            return new Scalar(a._value * b._value);
        }

        public static Scalar operator /(Scalar a, Scalar b)
        {
            return new Scalar(a._value / b._value);
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}
