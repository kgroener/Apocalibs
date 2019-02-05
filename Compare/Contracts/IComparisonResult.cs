namespace Compare.Contracts
{
    public interface IComparisonResult
    {
        string Message { get; }

        ComparisonResultType Result { get; }
    }
}
