using System;
using System.Linq.Expressions;

namespace Compare.Contracts
{
    public interface IClassComparisonBuilder<TClass>
    {
        IClassComparisonBuilder<TClass> IgnoreProperty<TProperty>(Expression<Func<TClass, TProperty>> propertyExpression);
        IClassComparisonBuilder<TClass> SetupPropertyComparison<TProperty>(Expression<Func<TClass, TProperty>> propertyExpression, Action<IComparisonConfiguration<TProperty>> comparisonConfiguration);
    }
}
