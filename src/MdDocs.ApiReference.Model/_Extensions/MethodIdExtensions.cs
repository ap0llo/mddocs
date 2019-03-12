namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Extension methods for <see cref="MethodId"/>.
    /// </summary>
    public static class MethodIdExtensions
    {
        /// <summary>
        /// Gets the <see cref="OperatorKind"/> for a method if the method is an operator overload.
        /// </summary>
        /// <returns>Returns the kind of operator the method implements or <c>null</c> if the method is not an operator overload.</returns>
        /// <seealso cref="OperatorKind"/>
        public static OperatorKind? GetOperatorKind(this MethodId methodId) =>
            OperatorMethodNames.GetOperatorKind(methodId.Name);

        /// <summary>
        /// Determines whether the specified method id refers to a constructor.
        /// </summary>
        /// <returns>Returns <c>true</c> if the specified method refers to a constructor, otherwise returns <c>false</c>.</returns>
        /// <seealso cref="OperatorKind"/>
        public static bool IsConstructor(this MethodId methodId) =>
            methodId.Name == ".cctor" || methodId.Name == ".ctor";
    }
}
