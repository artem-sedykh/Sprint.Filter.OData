// ReSharper disable once CheckNamespace
namespace Sprint.Filter.OData.Serialize.Writers
{
    internal class IntegerValueWriter<T> : ValueWriterBase<T>
    {
        public override string Write(object value)
        {
            return value.ToString();
        }
    }
}
