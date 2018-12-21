namespace MdDoc.Model
{
    public static class MethodIdExtensions
    {
        public static OperatorKind? GetOperatorKind(this MethodId methodId) =>
            OperatorMethodNames.GetOperatorKind(methodId.Name);

        public static bool IsConstructor(this MethodId methodId) =>
            (methodId.Name == ".cctor" || methodId.Name == ".ctor");
    }
}
