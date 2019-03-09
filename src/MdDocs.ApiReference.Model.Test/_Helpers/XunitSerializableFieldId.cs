using System;
using Grynwald.MdDocs.ApiReference.Model;
using Xunit.Abstractions;

namespace Grynwald.MdDocs.ApiReference.Test.Model
{
    /// <summary>
    /// Adapter to make <see cref="FieldId"/> serializable by xunit
    /// </summary>
    class XunitSerializableFieldId : IXunitSerializable
    {
        public FieldId FieldId { get; private set; }


        // parameterless constructor required by xunit
        public XunitSerializableFieldId()
        { }

        public XunitSerializableFieldId(FieldId fieldId)
        {
            FieldId = fieldId ?? throw new ArgumentNullException(nameof(fieldId));
        }


        public void Deserialize(IXunitSerializationInfo info)
        {
            FieldId = new FieldId(
                definingType: info.GetValue<XunitSerializableTypeId>(nameof(FieldId.DefiningType)),
                name: info.GetValue<string>(nameof(FieldId.Name))
            );
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(FieldId.DefiningType), new XunitSerializableTypeId(FieldId.DefiningType));
            info.AddValue(nameof(FieldId.Name), FieldId.Name);
        }

        public static implicit operator FieldId(XunitSerializableFieldId serializable) => serializable?.FieldId;
    }
}
