namespace Sprint.Filter.OData.Serialize.Writers
{
    internal class StringValueWriter : ValueWriterBase<string>
    {
        public override string Write(object value)
        {
            return string.Format("'{0}'", value);
        }
    }
}
