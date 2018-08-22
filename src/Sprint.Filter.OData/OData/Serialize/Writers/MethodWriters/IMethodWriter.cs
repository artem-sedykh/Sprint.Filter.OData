using System;
using System.Linq.Expressions;
// ReSharper disable once CheckNamespace

namespace Sprint.Filter.OData.Serialize.Writers
{
    internal interface IMethodWriter
    {
        int Priority { get; set; }

        bool CanHandle(MethodCallExpression expression);

        string Write(MethodCallExpression expression, Func<Expression, string> writer);
    }
}
