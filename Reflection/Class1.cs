using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Reflection
{
    public static class Class1
    {
        public static T Create<T>()
        {
            IDictionary<string, object> instance = new ExpandoObject();

            TypeBuilder typeBuilder = TypeBuilder.


            return (T)instance;

        }
    }
}
