using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Apocalibs.CodeFlow.Fluent.Contracts
{
    public interface IElseBranch<TScope> :  IEndIfBranch<TScope> where TScope : CodeBlock<TScope>, new()
    {
        IEndIfBranch<TScope> Else(Action<TScope> elseBranch);
    }
}
