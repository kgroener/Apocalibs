using Apocalibs.CodeFlow.Fluent.Contracts;
using System;

namespace Apocalibs.CodeFlow.Fluent
{
    internal class ThenBranch<TScope> : IThenBranch<TScope> where TScope : CodeBlock<TScope>, new()
    {
        private TScope _scope;
        private Action<CodeBlock<TScope>> _action;

        public ThenBranch(TScope scope)
        {
            _scope = scope;
        }

        public void Execute()
        {
            var innerScope = _scope.CreateInnerScope();
            _action(innerScope);
            innerScope.Execute();
        }

        public IElseBranch<TScope> Then(Action<CodeBlock<TScope>> action)
        {
            _action = action;

            Else = new ElseBranch<TScope>(_scope);

            return Else;
        }

        public ElseBranch<TScope> Else { get; private set; }
    }
}
