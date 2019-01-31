using System;
using System.Collections.Generic;
using System.Text;
using Apocalibs.CodeFlow.Fluent.Contracts;

namespace Apocalibs.CodeFlow.Fluent
{
    class InEnumerableStatement<TScope, TEnumerable> : IInEnumerableStatement<TScope, TEnumerable> where TScope : CodeBlock<TScope>, new()
    {
        private readonly TScope _scope;
        private Func<IReadonlyScope, IEnumerable<TEnumerable>> _enumerableGetter;

        public DoLoop<TScope> Do { get; private set; }

        public InEnumerableStatement(TScope scope)
        {
            _scope = scope;
        }

        public IDoLoop<TScope> In(Func<IReadonlyScope, IEnumerable<TEnumerable>> statement)
        {
            _enumerableGetter = statement;

            Do = new DoLoop<TScope>(_scope);

            return Do;
        }

        public IDoLoop<TScope> In(string variableName)
        {
            return In((s) => s.GetVar<IEnumerable<TEnumerable>>(variableName));
        }

        public IEnumerable<TEnumerable> Enumerable => _enumerableGetter(_scope);
    }
}
