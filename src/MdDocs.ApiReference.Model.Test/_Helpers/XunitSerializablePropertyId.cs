using System;
using System.Collections.Generic;
using Grynwald.MdDocs.ApiReference.Model;
using Xunit.Abstractions;

namespace Grynwald.MdDocs.ApiReference.Test.Model
{
    /// <summary>
    /// Adapter to make <see cref="PropertyId"/> serializable by xunit
    /// </summary>
    class XunitSerializablePropertyId : IXunitSerializable
    {
        public PropertyId PropertyId { get; private set; }


        // parameterless constructor required by xunit
        public XunitSerializablePropertyId()
        { }

        public XunitSerializablePropertyId(PropertyId propertyId)
        {
            PropertyId = propertyId ?? throw new ArgumentNullException(nameof(propertyId));
        }


        public void Deserialize(IXunitSerializationInfo info)
        {
            var definingType = info.GetValue<XunitSerializableTypeId>(nameof(PropertyId.DefiningType));

            var methodName = info.GetValue<string>(nameof(PropertyId.Name));

            var parameterCount = info.GetValue<int>("parameterCount");
            var parameters = new List<TypeId>(parameterCount);
            for (int i = 0; i < parameterCount; i++)
            {
                parameters.Add(info.GetValue<XunitSerializableTypeId>($"parameter{i}"));
            }

            PropertyId = new PropertyId(definingType, methodName, parameters);
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(PropertyId.DefiningType), new XunitSerializableTypeId(PropertyId.DefiningType));

            info.AddValue(nameof(PropertyId.Name), PropertyId.Name);

            info.AddValue("parameterCount", PropertyId.Parameters.Count);
            for (int i = 0; i < PropertyId.Parameters.Count; i++)
            {
                info.AddValue($"parameter{i}", new XunitSerializableTypeId(PropertyId.Parameters[i]));
            }
        }

        public static implicit operator PropertyId(XunitSerializablePropertyId serializable) => serializable?.PropertyId;
    }
}
