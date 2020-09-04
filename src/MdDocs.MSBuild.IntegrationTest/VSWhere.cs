using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Grynwald.MdDocs.MSBuild.IntegrationTest
{
    /// <summary>
    /// Wrapper around vswhere.exe that enables discovery of Visual Studio installation
    /// </summary>
    internal static class VSWhere
    {

        private static string VSWherePath => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
            @"Microsoft Visual Studio\Installer\vswhere.exe");


        public static string GetMSBuildPath(int msbuildVersion)
        {
            if (msbuildVersion < 0)
                throw new ArgumentOutOfRangeException(nameof(msbuildVersion));

            var installationPath = RunVSWhere(
                "-requires", "Microsoft.Component.MSBuild",
                "-property", "installationPath",
                "-version", $"[{msbuildVersion}.0, )");

            installationPath = installationPath.Trim('\r', '\n');

            if (String.IsNullOrEmpty(installationPath))
                throw new VSWhereException($"Failed to locate a Visual Studio installation for version {msbuildVersion}. Installation path is empty");

            if (!Directory.Exists(installationPath))
                throw new VSWhereException($"Visual Studio installation path '{installationPath}' does not exist");


            var msbuildPath = msbuildVersion >= 16
                ? Path.Combine(installationPath, @"MSBuild\Current\Bin\MSBuild.exe")
                : Path.Combine(installationPath, $@"MSBuild\{msbuildVersion}.0\Bin\MSBuild.exe");

            if (!File.Exists(msbuildPath))
                throw new VSWhereException($"Failed to locate MSBuild, '{msbuildPath}' does not exist");

            return msbuildPath;
        }


        private static string RunVSWhere(params string[] args)
        {
            if (!File.Exists(VSWherePath))
                throw new VSWhereException($"vswhere.exe not found (expected at '{VSWherePath}')");

            var startInfo = new ProcessStartInfo()
            {
                FileName = VSWherePath,
                Arguments = String.Join(" ", args.Select(x => $"\"{x}\"")),
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            };

            var stdOutBuilder = new StringBuilder();

            var process = Process.Start(startInfo);

            process.OutputDataReceived += (s, e) => stdOutBuilder.AppendLine(e.Data ?? "");

            process.BeginOutputReadLine();

            process.WaitForExit();
            process.CancelOutputRead();

            if (process.ExitCode != 0)
                throw new VSWhereException($"vswhere.exe completed with exit code {process.ExitCode}");

            return stdOutBuilder.ToString();
        }
    }
}
