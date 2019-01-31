using Apocalibs.CodeFlow.Fluent.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Apocalibs.CodeFlow.Fluent
{
    public interface IInEnumerableStatement<TScope, TEnumerable> where TScope : CodeBlock<TScope>, new()
    {
        IDoLoop<TScope> In(Func<IReadonlyScope, IEnumerable<TEnumerable>> statement);
        IDoLoop<TScope> In(string variableName);
    }
}