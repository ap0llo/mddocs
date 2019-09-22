using Microsoft.Build.Framework;

namespace Grynwald.MdDocs.MSBuild
{
    internal static class TaskItemExtensions
    {
        public static string GetFullPath(this ITaskItem taskItem) => taskItem.GetMetadata("FullPath");
    }
}
