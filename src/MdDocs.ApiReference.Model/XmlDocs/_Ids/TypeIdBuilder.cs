#nullable disable

using System;
using System.Collections.Generic;

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    internal abstract class TypeIdBuilder
    {
        private class OuterTypeIdBuilder : TypeIdBuilder
        {
            private NamespaceIdBuilder m_NamespaceBuilder;
            private readonly string m_CurrentName;

            public OuterTypeIdBuilder()
            { }

            private OuterTypeIdBuilder(NamespaceIdBuilder namespaceBuilder, string currentName)
            {
                m_NamespaceBuilder = namespaceBuilder;
                m_CurrentName = currentName;
            }


            public override TypeIdBuilder AddNameSegment(string name)
            {
                if (m_CurrentName != null)
                {
                    m_NamespaceBuilder.AddNameSegment(m_CurrentName);
                }

                return new OuterTypeIdBuilder(m_NamespaceBuilder, name);
            }

            public override TypeIdBuilder SetArity(int arity)
            {
                if (arity < 0)
                    throw new ArgumentOutOfRangeException(nameof(arity));

                if (m_CurrentName == null)
                    throw new InvalidOperationException();

                if (arity == 0)
                    return this;

                // once the first name segment with an arity is added, all following
                // names are names of nested types
                // (there can't be any type parameters on namespaces)
                return new NestedTypeIdBuilder(ToTypeId(arity));
            }

            public override TypeIdBuilder SetTypeArguments(IReadOnlyList<TypeId> typeArguments)
            {
                if (typeArguments is null)
                    throw new ArgumentNullException(nameof(typeArguments));

                if (m_CurrentName == null)
                    throw new InvalidOperationException();

                if (typeArguments.Count == 0)
                    return this;

                // once type arguments added, all following
                // names are names of nested types
                // (there can't be any type parameters on namespaces)
                return new NestedTypeIdBuilder(ToTypeId(typeArguments));
            }

            public override TypeIdBuilder BeginNestedType()
            {
                return new NestedTypeIdBuilder(ToTypeId());
            }

            public override TypeId ToTypeId() => ToTypeId(0);


            private TypeId ToTypeId(int arity)
            {
                if (arity == 0)
                {
                    return new SimpleTypeId(m_NamespaceBuilder.ToNamespaceId(), m_CurrentName);
                }
                else
                {
                    return new GenericTypeId(m_NamespaceBuilder.ToNamespaceId(), m_CurrentName, arity);
                }
            }

            private TypeId ToTypeId(IReadOnlyList<TypeId> typeArguments)
            {
                if (typeArguments.Count == 0)
                {
                    return new SimpleTypeId(m_NamespaceBuilder.ToNamespaceId(), m_CurrentName);
                }
                else
                {
                    return new GenericTypeInstanceId(m_NamespaceBuilder.ToNamespaceId(), m_CurrentName, typeArguments);
                }

            }
        }


        private class NestedTypeIdBuilder : TypeIdBuilder
        {
            private readonly TypeId m_DeclaringType;
            private readonly string m_Name;
            private readonly int m_Arity;
            private readonly IReadOnlyList<TypeId> m_TypeArguments;


            public NestedTypeIdBuilder(TypeId declaringType)
            {
                m_DeclaringType = declaringType;
            }

            private NestedTypeIdBuilder(TypeId declaringType, string name, int arity)
            {
                m_DeclaringType = declaringType;
                m_Name = name;
                m_Arity = arity;
            }

            private NestedTypeIdBuilder(TypeId declaringType, string name, IReadOnlyList<TypeId> typeArguments)
            {
                m_DeclaringType = declaringType;
                m_Name = name;
                m_TypeArguments = typeArguments;
            }


            public override TypeIdBuilder AddNameSegment(string name)
            {
                if (m_Name == null)
                {
                    return new NestedTypeIdBuilder(m_DeclaringType, name, 0);
                }
                else
                {
                    return new NestedTypeIdBuilder(ToTypeId(), name, 0);
                }
            }

            public override TypeIdBuilder BeginNestedType()
            {
                return new NestedTypeIdBuilder(ToTypeId());
            }

            public override TypeIdBuilder SetArity(int arity)
            {
                if (m_Name == null)
                    throw new InvalidOperationException();

                if (m_Arity == arity)
                    return this;

                return new NestedTypeIdBuilder(m_DeclaringType, m_Name, arity);
            }

            public override TypeIdBuilder SetTypeArguments(IReadOnlyList<TypeId> typeArguments)
            {
                return new NestedTypeIdBuilder(m_DeclaringType, m_Name, typeArguments);
            }

            public override TypeId ToTypeId()
            {
                if (m_Name == null)
                {
                    return m_DeclaringType;
                }
                else
                {
                    if (m_TypeArguments != null)
                    {
                        return new GenericTypeInstanceId(m_DeclaringType, m_Name, m_TypeArguments);
                    }
                    else if (m_Arity == 0)
                    {
                        return new SimpleTypeId(m_DeclaringType, m_Name);
                    }
                    else
                    {
                        return new GenericTypeId(m_DeclaringType, m_Name, m_Arity);
                    }

                }
            }

        }

        public static TypeIdBuilder Create() => new OuterTypeIdBuilder();


        public abstract TypeIdBuilder AddNameSegment(string name);

        public abstract TypeIdBuilder SetArity(int arity);

        public abstract TypeIdBuilder SetTypeArguments(IReadOnlyList<TypeId> typeArguments);

        public abstract TypeIdBuilder BeginNestedType();

        public abstract TypeId ToTypeId();
    }
}
