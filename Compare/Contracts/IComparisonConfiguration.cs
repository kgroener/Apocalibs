using System;

namespace Compare.Contracts
{
    public interface IComparisonConfiguration<T>
    {
        IComparisonConfigurationConstraints<T> When(Func<T, T, bool> comparison, IComparisonResult action);
    }

    internal interface IComparisonConfiguration
    {
        bool CanCompare<T>();

        IComparisonResult Compare(string propertyName, object a, object b);
    }
}
