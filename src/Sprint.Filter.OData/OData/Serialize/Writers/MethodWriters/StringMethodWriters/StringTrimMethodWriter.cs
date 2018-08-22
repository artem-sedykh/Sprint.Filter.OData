﻿using System;
using System.Linq.Expressions;
// ReSharper disable once CheckNamespace

namespace Sprint.Filter.OData.Serialize.Writers
{
    internal class StringTrimMethodWriter:IMethodWriter
    {
        public int Priority { get; set; }
        public bool CanHandle(MethodCallExpression expression)
        {
            return expression.Method.DeclaringType == typeof(string) && expression.Method.Name == "Trim";
        }

        public string Write(MethodCallExpression expression, Func<Expression, string> writer)
        {
            var obj = expression.Object;

            return $"trim({writer(obj)})";
        }
    }
}
