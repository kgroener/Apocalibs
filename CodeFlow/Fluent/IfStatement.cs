using System;
using System.Collections.Generic;
using System.Text;
using Apocalibs.CodeFlow.Fluent.Contracts;

namespace Apocalibs.CodeFlow.Fluent
{
    internal class IfStatement<TScope> : IExecutableCodeFlow where TScope : CodeBlock<TScope>, new()
    {
        private readonly TScope _scope;
        private readonly Predicate<IReadonlyScope> _predicate;
        private readonly ThenBranch<TScope> _thenBranch;
        private readonly List<IExecutableCodeFlow> _actions;

        public IfStatement(TScope scope, Predicate<IReadonlyScope> predicate)
        {
            _scope = scope;
            _predicate = predicate;
            _thenBranch = new ThenBranch<TScope>(scope);
            _actions = new List<IExecutableCodeFlow>();
        }

        public IThenBranch<TScope> Then => _thenBranch;

        public IEnumerable<IExecutableCodeFlow> Actions => throw new NotImplementedException();

        public void Execute()
        {
            if (_predicate(_scope))
            {
                _thenBranch.Execute();
            }
            else
            {
                _thenBranch.Else.Execute();
            }
        }
    }
}
