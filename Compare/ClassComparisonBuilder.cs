using Compare.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Compare
{
    internal class ClassComparisonBuilder<TClass> : IClassComparisonBuilder<TClass>
    {
        private List<MemberInfo> _ignorableProperties;
        private Dictionary<MemberInfo, List<IComparisonConfiguration>> _propertyComparisonConfigurations;

        public ReadOnlyDictionary<MemberInfo, List<IComparisonConfiguration>> PropertyComparisons => new ReadOnlyDictionary<MemberInfo, List<IComparisonConfiguration>>(_propertyComparisonConfigurations);

        public IEnumerable<MemberInfo> IgnorableProperties => _ignorableProperties;

        public ClassComparisonBuilder()
        {
            _ignorableProperties = new List<MemberInfo>();
            _propertyComparisonConfigurations = new Dictionary<MemberInfo, List<IComparisonConfiguration>>();
        }

        public IClassComparisonBuilder<TClass> IgnoreProperty<TProperty>(Expression<Func<TClass, TProperty>> expression)
        {
            var propertyExpression = GetPropertyExpression(expression);

            _ignorableProperties.Add(propertyExpression.Member);

            return this;
        }

        public IClassComparisonBuilder<TClass> SetupPropertyComparison<TProperty>(Expression<Func<TClass, TProperty>> expression, Action<IComparisonConfiguration<TProperty>> comparisonConfiguration)
        {
            var propertyExpression = GetPropertyExpression(expression);
            if (!_propertyComparisonConfigurations.ContainsKey(propertyExpression.Member))
            {
                _propertyComparisonConfigurations[propertyExpression.Member] = new List<IComparisonConfiguration>();
            }

            var configuration = new ComparisonConfiguration<TProperty>();

            comparisonConfiguration(configuration);

            _propertyComparisonConfigurations[propertyExpression.Member].Add(configuration);

            return this;
        }

        private static MemberExpression GetPropertyExpression<TProperty>(Expression<Func<TClass, TProperty>> expression)
        {
            var lambdaExpression = expression as LambdaExpression;

            if (lambdaExpression == null)
            {
                throw new ArgumentException("Expression should be a lambda expression");
            }

            var propertyExpression = lambdaExpression.Body as MemberExpression;

            if (propertyExpression == null)
            {
                throw new ArgumentException("Expression should supply a member");
            }

            return propertyExpression;
        }
    }
}
