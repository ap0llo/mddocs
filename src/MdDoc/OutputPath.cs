using System;

namespace MdDoc
{
    sealed class OutputPath
    {        
        public string Value { get; }

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

            Value = value;
        }


        public string GetRelativePathTo(string absolutePath)
        {
            var pathUri = new Uri(absolutePath);
            var relativeToUri = new Uri(Value);

            return relativeToUri.MakeRelativeUri(pathUri).ToString();
        }

        public string GetRelativePathTo(OutputPath absolutePath) => GetRelativePathTo(absolutePath.Value);


        public static implicit operator string(OutputPath instance) => instance?.Value;
    }
}
