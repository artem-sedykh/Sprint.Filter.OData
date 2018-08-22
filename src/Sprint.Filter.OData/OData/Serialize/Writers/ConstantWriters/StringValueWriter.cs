// ReSharper disable once CheckNamespace
namespace Sprint.Filter.OData.Serialize.Writers
{
    internal class StringValueWriter : ValueWriterBase<string>
    {
        public override string Write(object value)
        {
            return $"'{value}'";
        }
    }
}
