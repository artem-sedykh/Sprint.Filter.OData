using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Sprint.Filter.OData.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Parameter | AttributeTargets.Struct | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ODataFilterBinderAttribute : CustomModelBinderAttribute
    {
        public bool NullDefaultValue { get; set; }

        public override IModelBinder GetBinder()
        {
            return new ODataFilterBinder(NullDefaultValue);
        }
    }

    internal class ODataFilterBinder : DefaultModelBinder
    {
        private readonly bool _nullDefaultValue;

        public ODataFilterBinder(bool nullDefaultValue)
        {
            _nullDefaultValue = nullDefaultValue;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var key = bindingContext.ModelName;

            if (typeof(Expression).IsAssignableFrom(bindingContext.ModelType))
            {
                var type = bindingContext.ModelType.GetGenericArguments()[0].GetGenericArguments()[0];

                if (!bindingContext.ValueProvider.ContainsPrefix(key))
                    return _nullDefaultValue ? null : DefaultLambda(type);

                var query = bindingContext.ValueProvider.GetValue(key).AttemptedValue;

                var lambda = Filter.Deserialize(type, query);

                return lambda ?? (_nullDefaultValue ? null : DefaultLambda(type));
            }

            throw new NotSupportedException("expected type Expression<Func<TModel,bool>>");
        }

        private LambdaExpression DefaultLambda(Type modelType)
        {
            return Expression.Lambda(typeof (Func<,>).MakeGenericType(modelType, typeof (bool)), Expression.Constant(true), Expression.Parameter(modelType, "t"));
        }
    }
}
