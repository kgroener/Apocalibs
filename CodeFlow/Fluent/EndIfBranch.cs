using Apocalibs.CodeFlow.Fluent.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apocalibs.CodeFlow.Fluent
{
    internal class EndIfBranch<TScope> : IEndIfBranch<TScope> where TScope : CodeBlock<TScope>, new()
    {
        protected readonly TScope _scope;

        public EndIfBranch(TScope scope)
        {
            _scope = scope;
        }

        public TScope EndIf()
        {
            return _scope;
        }
    }
}
