using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Apocalibs.CodeFlow.Fluent.Contracts
{
    public interface IThenBranch<TScope> : IExecutableCodeFlow where TScope : CodeBlock<TScope>, new()
    {
        IElseBranch<TScope> Then(Action<CodeBlock<TScope>> scope);
    }
}
