using Apocalibs.CodeFlow.Fluent.Contracts;

namespace Apocalibs.CodeFlow.Fluent
{
    internal class ForeachStatement<TScope, TEnumerable> : IExecutableCodeFlow where TScope : CodeBlock<TScope>, new()
    {
        private readonly TScope _scope;
        private readonly string _variableName;
        private readonly InEnumerableStatement<TScope, TEnumerable> _inEnumerableStatment;

        public ForeachStatement(TScope scope, string variableName)
        {
            _scope = scope.CreateInnerScope();
            _variableName = variableName;
            _inEnumerableStatment = new InEnumerableStatement<TScope, TEnumerable>(_scope);
        }

        public IInEnumerableStatement<TScope, TEnumerable> In => _inEnumerableStatment;

        public void Execute()
        {
            foreach (var item in _inEnumerableStatment.Enumerable)
            {
                _scope.SetVar(_variableName, () => item).Execute();
                _inEnumerableStatment.Do.Execute();
            }
        }
    }
}