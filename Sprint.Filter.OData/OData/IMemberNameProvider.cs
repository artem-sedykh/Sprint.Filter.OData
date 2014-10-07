using System;
using System.Reflection;

namespace Sprint.Filter.OData
{
    public interface IMemberNameProvider
    {
        MemberInfo ResolveAlias(Type type, string alias);

        string ResolveName(MemberInfo member);
    }
}
