using System;
// ReSharper disable once CheckNamespace

namespace Sprint.Filter.OData.Serialize.Writers
{
    public class EnumValueWriter : IValueWriter
    {
        public bool Handles(Type type)
        {
            return type==typeof(Enum) || type.IsEnum;
        }

        public string Write(object value)
        {
            var enumType = value.GetType();

            return $"{enumType.FullName}'{value}'";
        }
    }
}
