using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Sprint.Filter.OData.Serialize;

namespace Sprint.Filter.OData.Test.QueryProviderTests
{
    public abstract class QueryProvider : IQueryProvider
    {
        private readonly Translator _queryTranslator;
        private readonly Uri _address;
        protected QueryProvider(Uri address)
        {
            _queryTranslator = new Translator();
            _queryNodes = new List<Func<string>>();
            _address = address;
        }

        private readonly List<Func<string>> _queryNodes;

        IQueryable<S> IQueryProvider.CreateQuery<S>(Expression expression)
        {

            var methodCallExpression = (MethodCallExpression)expression;

            var methodName = methodCallExpression.Method.Name;

            switch (methodName.ToLower())
            {
                case "select":
                {
                    var lambda = methodCallExpression.Arguments[1];

                    if (lambda.NodeType == ExpressionType.Quote)
                        _queryNodes.Add(() => String.Format("$select={0}", _queryTranslator.Translate(((UnaryExpression)lambda).Operand)));
                    else
                        _queryNodes.Add(() => String.Format("$select={0}", _queryTranslator.Translate(lambda)));

                    break;
                }
                case "where":
                {
                    var lambda = methodCallExpression.Arguments[1];
                    
                    if(lambda.NodeType == ExpressionType.Quote)
                        _queryNodes.Add(() =>String.Format("$filter={0}",_queryTranslator.Translate(((UnaryExpression)lambda).Operand)));
                    else
                        _queryNodes.Add(() => String.Format("$filter={0}",_queryTranslator.Translate(lambda)));

                    break;                    
                }
                default:
                {
                    throw new NotSupportedException();
                }
            }
            
            return new ODataQuery<S>(expression, this);
        }

        IQueryable IQueryProvider.CreateQuery(Expression expression)
        {
            var elementType = TypeSystem.GetElementType(expression.Type);
            try
            {
                return null;
                // return (IQueryable)Activator.CreateInstance(typeof(Query<>).MakeGenericType(elementType), new object[] { this, expression });
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        S IQueryProvider.Execute<S>(Expression expression)
        {
            return (S)this.Execute(expression);
        }

        object IQueryProvider.Execute(Expression expression)
        {
            return Execute(expression);
        }

        public virtual string GetQueryText(Expression expression)
        {
            var query = String.Join("&", _queryNodes.Select(x => x()));

            return String.Format("{0}?{1}", _address, query);   
            
        }
        public abstract object Execute(Expression expression);
    }
}
