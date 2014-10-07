using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Sprint.Filter.OData.Deserialize;

namespace Sprint.Filter.OData
{
    public static class Filter
    {        
        public static Expression<Func<TModel, bool>> Deserialize<TModel>(string query) where TModel : class
        {
            if (String.IsNullOrWhiteSpace(query))
                return model => true;

            var expressionLexer = new ExpressionLexer(query);

            var expression = expressionLexer.BuildLambdaExpression();

            var translator = new Translator();

            var expr = translator.Translate<TModel, bool>(expression);

            return expr;
        }

        public static string Serialize<TModel, TResult>(Expression<Func<TModel, TResult>> expression)
        {
            var translator = new Serialize.Translator();

            var query = translator.Translate(expression);

            return query;
        }
        
        public static Expression<Func<TResult>> Invoke<TResult>([NotNull]string query)
        {
            var expressionLexer = new ExpressionLexer(query);

            var expression = expressionLexer.BuildLambdaExpression();

            var translator = new Translator();

            return translator.Translate<TResult>(expression);
        }
    }
}
