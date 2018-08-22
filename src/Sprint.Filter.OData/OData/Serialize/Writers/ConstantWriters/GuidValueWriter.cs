using System;
// ReSharper disable once CheckNamespace

namespace Sprint.Filter.OData.Serialize.Writers
{
    internal class GuidValueWriter : ValueWriterBase<Guid>
    {
        public override string Write(object value)
        {
            return $"guid'{value}'";
        }
    }
}
