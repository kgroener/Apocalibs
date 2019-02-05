using Compare.Contracts;

namespace Compare
{
    internal class ComparisonResult : IComparisonResult
    {
        public string Message { get; private set; }

        public ComparisonResultType Result { get; private set; }

        public static IComparisonResult Equal()
        {
            return new ComparisonResult()
            {
                Message = "Values are equal",
                Result = ComparisonResultType.Equal
            };
        }

        public static IComparisonResult Error(string message)
        {
            return new ComparisonResult()
            {
                Message = message,
                Result = ComparisonResultType.Error
            };
        }

        public static IComparisonResult Warning(string message)
        {
            return new ComparisonResult()
            {
                Message = message,
                Result = ComparisonResultType.Warning
            };
        }
    }
}
