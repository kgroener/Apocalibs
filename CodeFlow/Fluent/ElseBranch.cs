using Apocalibs.CodeFlow.Fluent.Contracts;
using System;

namespace Apocalibs.CodeFlow.Fluent
{
    internal class ElseBranch<TScope> : EndIfBranch<TScope>, IExecutableCodeFlow, IElseBranch<TScope> where TScope : CodeBlock<TScope>, new()
    {
        private Action<TScope> _elseBranch;

        public ElseBranch(TScope scope) : base(scope)
        {
        }

        public IEndIfBranch<TScope> Else(Action<TScope> elseBranch)
        {
            _elseBranch = elseBranch;

            return this;
        }

        public void Execute()
        {
            var innerScope = _scope.CreateInnerScope();
            _elseBranch?.Invoke(innerScope);
            innerScope.Execute();
        }
    }
}