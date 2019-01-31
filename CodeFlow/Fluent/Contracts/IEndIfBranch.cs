using System;
using System.Collections.Generic;
using System.Text;

namespace Apocalibs.CodeFlow.Fluent.Contracts
{
    public interface IEndIfBranch<TScope> where TScope : CodeBlock<TScope>, new()
    {
        TScope EndIf();
    }
}
