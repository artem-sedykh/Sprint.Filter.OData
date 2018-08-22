// ReSharper disable once CheckNamespace
namespace Sprint.Filter.OData.Serialize.Writers
{
    internal class BooleanValueWriter : ValueWriterBase<bool>
    {
        public override string Write(object value)
        {
            var boolean = (bool)value;

            return boolean ? "true" : "false";
        }
    }
}
