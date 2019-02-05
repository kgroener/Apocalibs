using Compare.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compare
{
    internal class ComparisonConfiguration<T> : IComparisonConfigurationConstraints<T>, IComparisonConfiguration
    {
        private class Comparison
        {
            private readonly Func<T, T, bool> _comparison;

            public Comparison(Func<T, T, bool> comparison, IComparisonResult action)
            {
                _comparison = comparison;
                Result = action;
            }

            public bool Compare(T a, T b)
            {
                return _comparison(a, b);
            }

            public IComparisonResult Result { get; }
        }


        private IComparisonResult _elseResult;
        private List<Comparison> _comparisons;

        public ComparisonConfiguration()
        {
            _comparisons = new List<Comparison>();
        }

        public bool CanCompare<T1>()
        {
            return typeof(T1) == typeof(T);
        }

        public IComparisonResult Compare(string propertyName, object a, object b)
        {
            var defaultResult = _comparisons.All(c => c.Result.Result != ComparisonResultType.Equal)
                ? ComparisonResult.Equal()
                : ComparisonResult.Error($"Values of {propertyName} are not equal: ({a} - ({b}))");

            return _comparisons.FirstOrDefault(c => c.Compare((T)a, (T)b))?.Result
                ?? _elseResult
                ?? defaultResult;
        }

        public void Else(IComparisonResult result)
        {
            _elseResult = result;
        }

        public IComparisonConfigurationConstraints<T> When(Func<T, T, bool> comparison, IComparisonResult action)
        {
            _comparisons.Add(new Comparison(comparison, action));

            return this;
        }
    }
}
