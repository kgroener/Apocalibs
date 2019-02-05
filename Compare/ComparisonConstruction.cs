using Compare.Contracts;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Compare
{
    internal class ComparisonConstruction : IComparisonConstruction
    {
        private readonly List<IComparisonConfiguration> _defaultComparisonConfigurations;
        private readonly List<MemberInfo> _ignorableProperties;
        private readonly Dictionary<MemberInfo, List<IComparisonConfiguration>> _propertyComparisonConfigurations;


        public ComparisonConstruction()
        {

        }

        public IEnumerable<IComparisonResult> Compare<T>(T a, T b)
        {
            throw new NotImplementedException();
        }
    }
}
