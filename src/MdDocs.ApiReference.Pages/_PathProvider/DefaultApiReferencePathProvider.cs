using System;
using System.IO;
using Grynwald.MdDocs.ApiReference.Model;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    public class DefaultApiReferencePathProvider : IApiReferencePathProvider
    {
        private static readonly char[] s_SplitChars = ".".ToCharArray();


        public string GetPath(ConstructorsPage page) => Path.Combine(GetTypeDirRelative(page.Model.TypeDocumentation), "Constructors.md");

        public string GetPath(FieldPage page) => Path.Combine(GetTypeDirRelative(page.Model.TypeDocumentation), "Fields", $"{page.Model.Name}.md");

        public string GetPath(EventPage page) => Path.Combine(GetTypeDirRelative(page.Model.TypeDocumentation), "Events", $"{page.Model.Name}.md");

        public string GetPath(IndexerPage page) => Path.Combine(GetTypeDirRelative(page.Model.TypeDocumentation), "Indexers", $"{page.Model.Name}.md");

        public string GetPath(PropertyPage page) => Path.Combine(GetTypeDirRelative(page.Model.TypeDocumentation), "Properties", $"{page.Model.Name}.md");

        public string GetPath(MethodPage page) => Path.Combine(GetTypeDirRelative(page.Model.TypeDocumentation), "Methods", $"{page.Model.Name}.md");

        public string GetPath(OperatorPage page) => Path.Combine(GetTypeDirRelative(page.Model.TypeDocumentation), "Operators", $"{page.Model.Kind}.md");

        public string GetPath(NamespacePage page) => Path.Combine(GetNamespaceDirRelative(page.Model), "Namespace.md");

        public string GetPath(TypePage page) => Path.Combine(GetTypeDirRelative(page.Model), "Type.md");


        protected string GetTypeDirRelative(TypeDocumentation type)
        {
            var dirName = type.TypeId.Name;
            if (type.TypeId is GenericTypeInstanceId genericTypeInstance)
            {
                dirName += "-" + genericTypeInstance.TypeArguments.Count;
            }
            else if (type.TypeId is GenericTypeId genericType)
            {
                dirName += "-" + genericType.Arity;
            }

            return Path.Combine(GetNamespaceDirRelative(type.NamespaceDocumentation), dirName);
        }

        protected string GetNamespaceDirRelative(NamespaceDocumentation namespaceDocumentation) =>
           String.Join("/", namespaceDocumentation.Name.Split(s_SplitChars));
    }
}
