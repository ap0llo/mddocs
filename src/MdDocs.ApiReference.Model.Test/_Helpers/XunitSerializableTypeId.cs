using System;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Model;
using Xunit.Abstractions;

namespace Grynwald.MdDocs.ApiReference.Test.Model
{
    /// <summary>
    /// Adapter to make <see cref="TypeIdId"/> serializable by xunit
    /// </summary>
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
            var namespaceName = info.GetValue<string>(nameof(TypeId.Namespace));
            var name = info.GetValue<string>(nameof(TypeId.Name));

            var type = info.GetValue<string>("type");
            switch (type)
            {
                case nameof(SimpleTypeId):
                    TypeId = new SimpleTypeId(namespaceName, name);
                    break;

                case nameof(GenericTypeId):
                    var arity = info.GetValue<int>(nameof(GenericTypeId.Arity));
                    TypeId = new GenericTypeId(namespaceName, name, arity);
                    break;

                case nameof(GenericTypeInstanceId):
                    var typeArguments = info.GetValue<XunitSerializableTypeId[]>(nameof(GenericTypeInstanceId.TypeArguments));
                    TypeId = new GenericTypeInstanceId(namespaceName, name, typeArguments.Select(x => x.TypeId).ToArray());
                    break;

                case nameof(ArrayTypeId):
                    var elementType = info.GetValue<XunitSerializableTypeId>(nameof(ArrayTypeId.ElementType));
                    var dimensions = info.GetValue<int>(nameof(ArrayTypeId.Dimensions));
                    TypeId = new ArrayTypeId(elementType, dimensions);
                    break;

                case nameof(GenericTypeParameterId):
                    var index = info.GetValue<int>(nameof(GenericTypeParameterId.Index));
                    var definingMemberKind = info.GetValue<string>(nameof(GenericTypeParameterId.DefiningMemberKind));
                    TypeId = new GenericTypeParameterId(Enum.Parse<GenericTypeParameterId.MemberKind>(definingMemberKind), index);
                    break;

                case nameof(ByReferenceTypeId):
                    var byReferenceElementType = info.GetValue<XunitSerializableTypeId>(nameof(ByReferenceTypeId.ElementType));
                    TypeId = new ByReferenceTypeId(byReferenceElementType);
                    break;
                    
                default:
                    throw new NotImplementedException();
            }
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(TypeId.Namespace), TypeId.Namespace.Name);
            info.AddValue(nameof(TypeId.Name), TypeId.Name);

            switch (TypeId)
            {
                case SimpleTypeId simpleType:
                    info.AddValue("type", nameof(SimpleTypeId));
                    break;

                case GenericTypeId genericType:
                    info.AddValue("type", nameof(GenericTypeId));
                    info.AddValue(nameof(GenericTypeId.Arity), genericType.Arity);
                    break;

                case GenericTypeInstanceId typeInstance:
                    info.AddValue("type", nameof(GenericTypeInstanceId));
                    info.AddValue(nameof(GenericTypeInstanceId.TypeArguments), typeInstance.TypeArguments.Select(x => new XunitSerializableTypeId(x)).ToArray());
                    break;

                case ArrayTypeId arrayType:
                    info.AddValue("type", nameof(ArrayTypeId));
                    info.AddValue(nameof(ArrayTypeId.ElementType), new XunitSerializableTypeId(arrayType.ElementType));
                    info.AddValue(nameof(ArrayTypeId.Dimensions), arrayType.Dimensions);
                    break;

                case GenericTypeParameterId typeParameter:
                    info.AddValue("type", nameof(GenericTypeParameterId));
                    info.AddValue(nameof(GenericTypeParameterId.DefiningMemberKind), typeParameter.DefiningMemberKind.ToString());
                    info.AddValue(nameof(GenericTypeParameterId.Index), typeParameter.Index);
                    break;

                case ByReferenceTypeId byReferenceTypeId:
                    info.AddValue("type", nameof(ByReferenceTypeId));
                    info.AddValue(nameof(ByReferenceTypeId.ElementType), new XunitSerializableTypeId(byReferenceTypeId.ElementType));
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public static implicit operator TypeId(XunitSerializableTypeId serializable) => serializable?.TypeId;
    }
}
