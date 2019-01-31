using Apocalibs.CodeFlow.Fluent.Contracts;
using System;

namespace Apocalibs.CodeFlow.Fluent
{
    internal class DoLoop<TScope> : IDoLoop<TScope>, IExecutableCodeFlow where TScope : CodeBlock<TScope>, new()
    {
        private readonly TScope _scope;
        private Action<TScope> _body;

        public DoLoop(TScope scope)
        {
            _scope = scope;
        }

        public TScope Do(Action<TScope> body)
        {
            _body = body;

            return _scope;
        }

        public void Execute()
        {
            var innerScope = _scope.CreateInnerScope();
            _body?.Invoke(innerScope);
            innerScope.Execute();
        }
    }
}