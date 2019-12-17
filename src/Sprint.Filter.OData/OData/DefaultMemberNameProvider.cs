using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Sprint.Filter.OData
{
    internal class DefaultMemberNameProvider : IMemberNameProvider
    {
        private static readonly ConcurrentDictionary<MemberInfo, string> KnownMemberNames = new ConcurrentDictionary<MemberInfo, string>();
        private static readonly ConcurrentDictionary<string, MemberInfo> KnownAliasNames = new ConcurrentDictionary<string, MemberInfo>();

        public MemberInfo ResolveAlias(Type type, string alias)
        {
            var key = type.AssemblyQualifiedName + alias;

            return KnownAliasNames.GetOrAdd(key, s => ResolveAliasInternal(type, alias));
        }

        public string ResolveName(MemberInfo member)
        {
            var result = KnownMemberNames.GetOrAdd(member, ResolveNameInternal);

            return result;
        }

        private static string ResolveNameInternal(MemberInfo member)
        {
            var dataMember = member.GetCustomAttributes(typeof(DataMemberAttribute), true)
                .OfType<DataMemberAttribute>()
                .FirstOrDefault();

            if (dataMember?.Name != null)
            {
                return dataMember.Name;
            }

            var xmlElement = member.GetCustomAttributes(typeof(XmlElementAttribute), true)
                .OfType<XmlElementAttribute>()
                .FirstOrDefault();

            if (xmlElement?.ElementName != null)
                return xmlElement.ElementName;

            var xmlAttribute = member.GetCustomAttributes(typeof(XmlAttributeAttribute), true)
                .OfType<XmlAttributeAttribute>()
                .FirstOrDefault();

            if (xmlAttribute?.AttributeName != null)
                return xmlAttribute.AttributeName;
            
            return member.Name;
        }

        private static MemberInfo ResolveAliasInternal(Type type, string alias)
        {
            var members = GetMembers(type).ToArray();

            foreach (var member in members)
            {
                if (HasAliasAttribute(alias, member))
                {
                    return member.MemberType == MemberTypes.Field ? CheckFrontingProperty(member) : member;
                }

                if (member.Name == alias)
                {
                    return member;
                }
            }

            return null;
        }

        private static MemberInfo CheckFrontingProperty(MemberInfo field)
        {
            var declaringType = field.DeclaringType;

            var correspondingProperty = declaringType?.GetProperties().FirstOrDefault(x => string.Equals(x.Name, field.Name.Replace("_", string.Empty), StringComparison.OrdinalIgnoreCase));

            return correspondingProperty ?? field;
        }

        private static IEnumerable<MemberInfo> GetMembers(Type type)
        {
            if (type.IsInterface)
            {
                var propertyInfos = new List<MemberInfo>();

                var considered = new List<Type>();
                var queue = new Queue<Type>();
                considered.Add(type);
                queue.Enqueue(type);
                while (queue.Count > 0)
                {
                    var subType = queue.Dequeue();
                    foreach (var subInterface in subType.GetInterfaces()
                        .Where(x => !considered.Contains(x)))
                    {
                        considered.Add(subInterface);
                        queue.Enqueue(subInterface);
                    }

                    var typeProperties = subType.GetMembers(
                        BindingFlags.FlattenHierarchy
                        | BindingFlags.Public
                        | BindingFlags.Instance);

                    var newPropertyInfos = typeProperties
                        .Where(x => !propertyInfos.Contains(x));

                    propertyInfos.InsertRange(0, newPropertyInfos);
                }

                return propertyInfos.ToArray();
            }

            var members = type.GetMembers(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            return members;
        }

        private static bool HasAliasAttribute(string alias, MemberInfo member)
        {
            var attributes = member.GetCustomAttributes(true);
            var dataMember = attributes.OfType<DataMemberAttribute>().FirstOrDefault();

            if (dataMember != null && dataMember.Name == alias)
            {
                return true;
            }

            var xmlElement = attributes.OfType<XmlElementAttribute>()
                .FirstOrDefault();
            if (xmlElement != null && xmlElement.ElementName == alias)
            {
                return true;
            }

            var xmlAttribute = attributes.OfType<XmlAttributeAttribute>()
                .FirstOrDefault();

            return xmlAttribute != null && xmlAttribute.AttributeName == alias;
        }
    }
}
