using System;

namespace Compare.Contracts
{
    public interface IComparisonBuilder
    {
        IComparisonBuilder SetupDefaultComparison<T>(Action<IComparisonConfiguration<T>> comparisonConfiguration);

        IComparisonBuilder SetupComparisonOf<T>(Action<IClassComparisonBuilder<T>> classComparisonBuilder);
    }
}
