using System;
using System.Collections.Generic;
using System.Text;

namespace Apocalibs.CodeFlow.Fluent.Contracts
{
    public interface IDoLoop<TScope> where TScope : CodeBlock<TScope>, new()
    {
        TScope Do(Action<TScope> body);
    }
}
