using MdDoc.Model.XmlDocs;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace MdDoc.Test.Model.XmlDocs
{
    class XunitSerializableMethodId : IXunitSerializable
    {
        public MethodId MethodId { get; private set; }


        // parameterless constructor required by xunit
        public XunitSerializableMethodId()
        { }

        public XunitSerializableMethodId(MethodId methodId)
        {
            MethodId = methodId ?? throw new ArgumentNullException(nameof(methodId));
        }

        
        public void Deserialize(IXunitSerializationInfo info)
        {
            var definingType = info.GetValue<XunitSerializableTypeId>(nameof(MethodId.DefiningType));

            var methodName = info.GetValue<string>(nameof(MethodId.Name));

            var arity = info.GetValue<int>(nameof(MethodId.Arity));

            var parameterCount = info.GetValue<int>("parameterCount");
            var parameters = new List<TypeId>(parameterCount);
            for (int i = 0; i < parameterCount; i++)
            {
                parameters.Add(info.GetValue<XunitSerializableTypeId>($"parameter{i}"));
            }

            var returnType = info.GetValue<XunitSerializableTypeId>(nameof(MethodId.ReturnType));

            MethodId = new MethodId(definingType, methodName, arity, parameters, returnType);
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(MethodId.DefiningType), new XunitSerializableTypeId(MethodId.DefiningType));

            info.AddValue(nameof(MethodId.Name), MethodId.Name);

            info.AddValue(nameof(MethodId.Arity), MethodId.Arity);

            info.AddValue("parameterCount", MethodId.Parameters.Count);
            for(int i = 0; i < MethodId.Parameters.Count; i++)
            {
                info.AddValue($"parameter{i}", new XunitSerializableTypeId(MethodId.Parameters[i]));
            }            
        }

        public static implicit operator MethodId(XunitSerializableMethodId serializable) => serializable?.MethodId;
    }
}
