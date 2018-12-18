using MdDoc.Model.XmlDocs;
using System;
using Xunit.Abstractions;

namespace MdDoc.Test.Model.XmlDocs
{
    class XunitSerializableTypeId : IXunitSerializable
    {
        public TypeId TypeId { get; private set; }


        // parameterless constructor required by xunit
        public XunitSerializableTypeId()
        { }

        public XunitSerializableTypeId(TypeId typeId)
        {
            TypeId = typeId ?? throw new ArgumentNullException(nameof(typeId));
        }


        public void Deserialize(IXunitSerializationInfo info)
        {
            TypeId = new TypeId(
                namespaceName: info.GetValue<string>(nameof(TypeId.NamespaceName)),
                name: info.GetValue<string>(nameof(TypeId.Name)),
                arity: info.GetValue<int>(nameof(TypeId.Arity))
            );
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(TypeId.NamespaceName), TypeId.NamespaceName);
            info.AddValue(nameof(TypeId.Name), TypeId.Name);
            info.AddValue(nameof(TypeId.Arity), TypeId.Arity);
        }

        public static implicit operator TypeId(XunitSerializableTypeId serializable) => serializable?.TypeId; 
    }
}
