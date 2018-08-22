using System;
using System.Collections;
using System.Linq;

namespace Sprint.Filter.Extensions
{
    internal static class TypeExtensions
    {
        private static readonly Type NullableType = typeof(Nullable<>);
        private static readonly Type QueryableType = typeof(IQueryable);
        private static readonly Type EnumerableType = typeof(IEnumerable);
        public static bool IsNullableType(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == NullableType;
        }

        public static bool IsIEnumerable(this Type type)
        {
            return EnumerableType.IsAssignableFrom(type);
        }

        public static bool IsIQueryable(this Type type)
        {
            return QueryableType.IsAssignableFrom(type);
        }

        public static Type ToNullable(this Type type)
        {
            return NullableType.MakeGenericType(type);
        }

        public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            if(interfaceTypes.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericType))            
                return true;            

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            var baseType = givenType.BaseType;
            if(baseType == null) return false;

            return IsAssignableToGenericType(baseType, genericType);
        }

        internal static bool IsOpenType(this Type type)
        {
            return type.IsGenericType && type.GetGenericArguments().Any(x => x.IsGenericParameter);
        }

        internal static Type[] GetTypeGenericArguments(this Type type)
        {
            return type.IsArray ? new[] { type.GetElementType() } : type.GenericTypeArguments;
        }
    }
}
