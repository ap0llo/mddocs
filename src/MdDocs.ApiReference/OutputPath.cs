using System;
using System.IO;

namespace Grynwald.MdDocs.ApiReference
{
    public sealed class OutputPath : IEquatable<OutputPath>, IEquatable<string>
    {        
        public string Value { get; }


        public OutputPath(params string[] paths) : this(Path.Combine(paths))
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="OutputPath"/>
        /// </summary>
        /// <param name="value">
        /// The path of the file to create links relative to. 
        /// Paths returns by <see cref="GetRelativePathTo"/> will be relative to the
        /// parent directory of the specified file
        /// </param>
        public OutputPath(string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("Value must not be empty", nameof(value));

            Value = Path.GetFullPath(value).Normalize();
        }


        public string GetRelativePathTo(string absolutePath)
        {
            var pathUri = new Uri(absolutePath);
            var relativeToUri = new Uri(Value);

            return relativeToUri.MakeRelativeUri(pathUri).ToString();
        }

        public string GetRelativePathTo(OutputPath absolutePath) => GetRelativePathTo(absolutePath.Value);

        public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case OutputPath outputPath:
                    return Equals(outputPath);

                case string str:
                    return Equals(str);

                default:
                    return false;
            }
        }

        public bool Equals(OutputPath other) => 
            other != null && StringComparer.OrdinalIgnoreCase.Equals(Value, other.Value);

        public bool Equals(string other) =>
            other != null && StringComparer.OrdinalIgnoreCase.Equals(Value, other);

        public static implicit operator string(OutputPath instance) => instance?.Value;
    }
}
