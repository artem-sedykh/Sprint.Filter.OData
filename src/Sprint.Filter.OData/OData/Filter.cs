using System;
using System.Linq.Expressions;
using Sprint.Filter.OData.Deserialize;

namespace Sprint.Filter.OData
{
    public static class Filter
    {
        public static IMemberNameProvider DefaultMemberNameProvider = new DefaultMemberNameProvider();

        public static LambdaExpression Deserialize(Type modelType, string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return null;

            var expressionLexer = new ExpressionLexer(query);

            var expression = expressionLexer.BuildLambdaExpression();

            var translator = new QueryTranslator(DefaultMemberNameProvider);

            var expr = translator.Translate(expression, modelType);

            return expr;
        }

        public static Expression<Func<TModel, bool>> Deserialize<TModel>(string query) where TModel : class
        {
            if (string.IsNullOrWhiteSpace(query))
                return model => true;

            var expressionLexer = new ExpressionLexer(query);

            var expression = expressionLexer.BuildLambdaExpression();

            var translator = new QueryTranslator(DefaultMemberNameProvider);

            var expr = translator.Translate<TModel, bool>(expression);

            return expr;
        }

        public static string Serialize<TModel>(Expression<Func<TModel, bool>> expression)
        {
            var translator = new Serialize.QueryTranslator();

            var query = translator.Translate(expression);

            return query;
        }

        public static string Serialize(LambdaExpression expression)
        {
            var translator = new Serialize.QueryTranslator();

            var query = translator.Translate(expression);

            return query;
        }

        public static string Serialize<TModel, TResult>(Expression<Func<TModel, TResult>> expression)
        {
            var translator = new Serialize.QueryTranslator();

            var query = translator.Translate(expression);

            return query;
        }

        public static Expression<Func<TResult>> Invoke<TResult>(string query)
        {
            var expressionLexer = new ExpressionLexer(query);

            var expression = expressionLexer.BuildLambdaExpression();

            var translator = new QueryTranslator(DefaultMemberNameProvider);

            return translator.Translate<TResult>(expression);
        }
    }
}
