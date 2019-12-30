using System;
using Grynwald.MdDocs.ApiReference.Model;
using Xunit.Abstractions;

namespace Grynwald.MdDocs.ApiReference.Test.Model
{
    /// <summary>
    /// Adapter to make <see cref="NamespaceId"/> serializable by xunit
    /// </summary>
    class XunitSerializableNamespaceId : IXunitSerializable
    {
        public NamespaceId NamespaceId { get; private set; }


        // parameterless constructor required by xunit
        public XunitSerializableNamespaceId()
        {
            NamespaceId = null!; // set by Serialize()
        }

        public XunitSerializableNamespaceId(NamespaceId namespaceId)
        {
            NamespaceId = namespaceId ?? throw new ArgumentNullException(nameof(namespaceId));
        }


        public void Deserialize(IXunitSerializationInfo info)
        {
            NamespaceId = new NamespaceId(
                name: info.GetValue<string>(nameof(NamespaceId.Name))
            );
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(NamespaceId.Name), NamespaceId.Name);
        }

        public static implicit operator NamespaceId(XunitSerializableNamespaceId serializable) => serializable?.NamespaceId!;
    }
}
