using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Filter.OData.Serialize.Writers
{
    internal interface IMethodWriter
    {
        string Write(MethodCallExpression expression, Translator translator);
    }
}
