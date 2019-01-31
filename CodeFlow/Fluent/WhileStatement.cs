using System;
using Apocalibs.CodeFlow.Fluent.Contracts;

namespace Apocalibs.CodeFlow.Fluent
{
    internal class WhileStatement<TScope> : IExecutableCodeFlow where TScope : CodeBlock<TScope>, new()
    {
        private readonly CodeBlock<TScope> _scope;
        private readonly Predicate<IReadonlyScope> _predicate;

        public WhileStatement(TScope scope, System.Predicate<Contracts.IReadonlyScope> predicate)
        {
            _scope = scope;
            _predicate = predicate;
            Do = new DoLoop<TScope>(scope);
        }

        public DoLoop<TScope> Do { get; private set; }

        public void Execute()
        {
            while(_predicate(_scope))
            {
                Do.Execute();
            }
        }
    }
}