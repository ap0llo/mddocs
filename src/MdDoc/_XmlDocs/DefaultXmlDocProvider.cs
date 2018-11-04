using Grynwald.MarkdownGenerator;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MdDoc
{
    class DefaultXmlDocProvider : IXmlDocProvider
    {
        private readonly ModuleDefinition m_Module;
        private readonly string m_XmlDocsPath;
        public readonly Dictionary<MemberReference, MemberDocumentation> m_MemberDocumentation = new Dictionary<MemberReference, MemberDocumentation>();


        public DefaultXmlDocProvider(ModuleDefinition module, string xmlDocsPath)
        {
            m_Module = module;
            m_XmlDocsPath = xmlDocsPath;

            Load();
        }


        public MemberDocumentation TryGetDocumentation(MemberReference member)
        {
            return m_MemberDocumentation.TryGetValue(member, out var documentation) ? documentation : MemberDocumentation.Empty;
        }


        private void Load()
        {
            var namesByMemberReference = GetXmlDocMemberNames();
            var docsByName = LoadXmlDocumentation();

            // check if the xml docs contain a member we did not find
            var namesInAssembly = namesByMemberReference.Values.ToHashSet();            
            var unrecognizedNames = docsByName.Keys.Where(x => !namesInAssembly.Contains(x)).ToArray();
            foreach (var name in unrecognizedNames)
            {
                Console.WriteLine($"WARNING: Unrecignized name '{name}' in XML docs");
            }


            foreach(var memberReference in namesByMemberReference.Keys)
            {
                var name = namesByMemberReference[memberReference];

                if(docsByName.ContainsKey(name))
                {
                    m_MemberDocumentation.Add(memberReference, new MemberDocumentation(docsByName[name]));
                }
            }
        }


        private IDictionary<MemberReference, string> GetXmlDocMemberNames()
        {
            var result = new Dictionary<MemberReference, string>();

            foreach (var type in m_Module.Types)
            {
                result.Add(type, $"T:{type.FullName}");


                foreach (var property in type.Properties)
                {
                    result.Add(property, GetXmlDocName(property));
                }

                foreach (var field in type.Fields)
                {
                    result.Add(field, GetXmlDocName(field));
                }

                foreach (var method in type.Methods)
                {
                    result.Add(method, GetXmlDocName(method));
                }            

                //TODO: Events
            }

            return result;
        }

        private string GetXmlDocName(MethodDefinition method)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("M:");

            stringBuilder.Append(method.DeclaringType.FullName);
            stringBuilder.Append(".");

            if(method.IsConstructor)
            {
                stringBuilder.Append("#ctor");
            }
            else
            {
                stringBuilder.Append(method.Name);
            }


            if(method.HasParameters)
            { 
                stringBuilder.Append("(");

                var first = true;
                foreach(var parameter in method.Parameters)
                {
                    if(!first)
                    {
                        stringBuilder.Append(",");
                    }
                    stringBuilder.Append(GetXmlDocName(parameter.ParameterType));
                    first = false;

                }
                stringBuilder.Append(")");
            }

            //TODO: Other operatos
            if(method.IsSpecialName && method.Name == "op_Implicit")
            {
                stringBuilder.Append("~");
                stringBuilder.Append(GetXmlDocName(method.ReturnType));
            }

            return stringBuilder.ToString();
        }

        private string GetXmlDocName(PropertyDefinition property)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("P:");

            stringBuilder.Append(property.DeclaringType.FullName);
            stringBuilder.Append(".");
            stringBuilder.Append(property.Name);

            if (property.HasParameters)
            {
                stringBuilder.Append("(");

                var first = true;
                foreach (var parameter in property.Parameters)
                {
                    if (!first)
                    {
                        stringBuilder.Append(",");
                    }
                    stringBuilder.Append(GetXmlDocName(parameter.ParameterType));
                    first = false;

                }
                stringBuilder.Append(")");
            }

            return stringBuilder.ToString();
        }

        private string GetXmlDocName(FieldDefinition field)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("F:");

            stringBuilder.Append(field.DeclaringType.FullName);
            stringBuilder.Append(".");
            stringBuilder.Append(field.Name);

            return stringBuilder.ToString();
        }

        private string GetXmlDocName(TypeReference type)
        {

            if (type is GenericInstanceType genericType && genericType.HasGenericArguments)
            {
                var arguments = genericType.GenericArguments;

                var typeName = genericType.Name.Replace($"`{arguments.Count}", "");

                var stringBuilder = new StringBuilder();

                stringBuilder.Append(type.Namespace);
                stringBuilder.Append(".");
                stringBuilder.Append(typeName);
                stringBuilder.Append("{");

                var first = true;
                foreach (var typeArgument in arguments)
                {
                    if (!first)
                    {
                        stringBuilder.Append(",");
                    }
                    stringBuilder.Append(GetXmlDocName(typeArgument));
                    first = false;

                }

                stringBuilder.Append("}");
  
                return stringBuilder.ToString();

            }

            return type.FullName;

        }


        private IDictionary<string, XElement> LoadXmlDocumentation()
        {
            var document = XDocument.Load(m_XmlDocsPath);

            return document.Root
                .Descendants("member")
                .ToDictionary(x => x.Attribute("name").Value);

        }
    }
}
