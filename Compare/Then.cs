using Compare.Contracts;
using System;

namespace Compare
{
    public static class Then
    {

        public static IComparisonResult SetError(string message)
        {
            throw new NotImplementedException();
        }

        public static IComparisonResult SetWarning(string message)
        {
            throw new NotImplementedException();
        }

        public static IComparisonResult MarkAsEqual { get; }
    }
}
