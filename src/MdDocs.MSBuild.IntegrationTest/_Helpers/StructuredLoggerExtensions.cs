using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Logging.StructuredLogger;

namespace Grynwald.MdDocs.MSBuild.IntegrationTest
{
    /// <summary>
    /// Extension methods for types from the MSBuild StructuredLogger library
    /// </summary>
    /// <seealso href="https://msbuildlog.com/#api" />
    internal static class StructuredLoggerExtensions
    {
        public static IEnumerable<Target> GetTargets(this Build build, string? name = null)
        {
            var targets = build.Children
                .OfType<Project>()
                .SelectMany(x => x.Children.OfType<Target>());


            if (!String.IsNullOrEmpty(name))
            {
                targets = targets.Where(x => StringComparer.Ordinal.Equals(x.Name, name));
            }

            return targets;
        }

        public static IEnumerable<Task> GetTasks(this Target target, string? name = null)
        {
            var tasks = target.Children.OfType<Task>();

            if (!String.IsNullOrEmpty(name))
            {
                tasks = tasks.Where(x => x.Name == name);
            }

            return tasks;
        }
    }
}
