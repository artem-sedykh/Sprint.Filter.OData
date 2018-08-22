using System;
// ReSharper disable once CheckNamespace
namespace Sprint.Filter.OData.Serialize.Writers
{
    public abstract class ValueWriterBase<T> : IValueWriter
    {
        public bool Handles(Type type)
        {
            return typeof(T) == type;
        }

        public abstract string Write(object value);
    }

    public interface IValueWriter
    {
        bool Handles(Type type);

        string Write(object value);
    }
}
