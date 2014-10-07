using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Sprint.Filter.OData.Serialize.Writers
{
    internal interface IMethodWriter
    {
        int Priority { get; set; }

        bool CanHandle([NotNull]MethodCallExpression expression);

        string Write([NotNull]MethodCallExpression expression, [NotNull]Func<Expression, string> writer);
    }
}
