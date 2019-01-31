using Apocalibs.CodeFlow.Fluent.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apocalibs.CodeFlow.Fluent
{
    internal class Assignment : IExecutableCodeFlow
    {
        private readonly Action _assignment;

        public Assignment(Action assignment)
        {
            _assignment = assignment;
        }

        public void Execute()
        {
            _assignment();
        }
    }
}
