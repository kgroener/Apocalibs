namespace Compare.Contracts
{
    public interface IComparisonConfigurationConstraints<T> : IComparisonConfiguration<T>
    {
        void Else(IComparisonResult result);
    }
}
