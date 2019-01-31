using System;
using System.Collections.Generic;
using System.Text;

namespace Apocalibs.CodeFlow.Fluent.Contracts
{
    public interface IReadonlyScope
    {
        T GetVar<T>(string variableName);
    }
}
