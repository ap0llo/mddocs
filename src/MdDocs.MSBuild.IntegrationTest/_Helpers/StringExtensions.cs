using System;
using System.IO;
using Grynwald.Utilities.IO;

namespace Grynwald.MdDocs.MSBuild.IntegrationTest
{
    internal static class StringExtensions
    {
        public static string PathCombine(this TemporaryDirectory temporaryDirectory, params string[] paths) =>
            PathCombine(temporaryDirectory.FullName, paths);

        public static string PathCombine(this string path1, params string[] paths)
        {
            var allPaths = new string[paths.Length + 1];
            Array.Copy(paths, 0, allPaths, 1, paths.Length);
            allPaths[0] = path1;

            return Path.Combine(allPaths);
        }

        public static string GetFullPath(this string path) => Path.GetFullPath(path);

        public static string GetDirectoryName(this string path) => Path.GetDirectoryName(path)!;

        public static string TrimEndingDirectorySeparator(this string path) => Path.TrimEndingDirectorySeparator(path);
    }
}
