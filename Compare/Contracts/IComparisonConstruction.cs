using System.Collections.Generic;

namespace Compare.Contracts
{
    public interface IComparisonConstruction
    {
        IEnumerable<IComparisonResult> Compare<T>(T a, T b);
    }
}
