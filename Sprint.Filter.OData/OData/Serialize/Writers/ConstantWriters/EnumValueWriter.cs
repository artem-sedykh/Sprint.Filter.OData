using System;

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

            return string.Format("{0}'{1}'", enumType.FullName, value);
        }
    }
}
