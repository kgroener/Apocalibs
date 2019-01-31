using Apocalibs.CodeFlow.Fluent.Contracts;
using System;
using System.Collections.Generic;

namespace Apocalibs.CodeFlow.Fluent
{
    public abstract class CodeBlock<TScope> : IExecutableCodeFlow, IReadonlyScope where TScope : CodeBlock<TScope>, new()
    {
        private readonly Dictionary<string, object> _variables;
        private readonly List<IExecutableCodeFlow> _actions;
        private CodeBlock<TScope> _parent;


        public CodeBlock()
        {
            _variables = new Dictionary<string, object>();
            _actions = new List<IExecutableCodeFlow>();
        }


        public void Execute()
        {
            foreach (var action in _actions)
            {
                action.Execute();
            }
        }

        public T GetVar<T>(string variableName)
        {
            var variableExists = TryGetVariableScope(variableName, out var scope);

            if (variableExists)
            {
                return (T)scope._variables[variableName];
            }
            throw new ArgumentException($"Variable '{variableName}' is not defined in current scope");
        }

        public TScope SetVar<T>(string variableName, Func<T> returnValue)
        {
            var assignment = new Assignment(() =>
            {
                var variableExists = TryGetVariableScope(variableName, out var scope);

                if (variableExists)
                {
                    scope._variables[variableName] = returnValue();
                }
                else
                {
                    _variables[variableName] = returnValue();
                }
            });

            _actions.Add(assignment);

            return (TScope)this;
        }

        public IThenBranch<TScope> If(Predicate<IReadonlyScope> predicate)
        {
            var ifStatement = new IfStatement<TScope>((TScope)this, predicate);

            _actions.Add(ifStatement);

            return ifStatement.Then;
        }

        public IDoLoop<TScope> While(Predicate<IReadonlyScope> predicate)
        {
            var whileLoop = new WhileStatement<TScope>((TScope)this, predicate);

            _actions.Add(whileLoop);

            return whileLoop.Do;
        }

        public IInEnumerableStatement<TScope, TEnumerable> ForEach<TEnumerable>(string variableName)
        {
            var foreachLoop = new ForeachStatement<TScope, TEnumerable>((TScope)this, variableName);

            _actions.Add(foreachLoop);

            return foreachLoop.In;
        }


        internal TScope CreateInnerScope()
        {
            var scope = new TScope();
            scope.SetParentScope(this);

            return scope;
        }

        private bool TryGetVariableScope(string variableName, out CodeBlock<TScope> scope)
        {
            scope = this;
            var variableExists = false;
            while (scope != null && !(variableExists = scope._variables.ContainsKey(variableName)))
            {
                scope = scope._parent;
            }

            return variableExists;
        }

        private void SetParentScope(CodeBlock<TScope> parent)
        {
            _parent = parent;
        }
    }
}
