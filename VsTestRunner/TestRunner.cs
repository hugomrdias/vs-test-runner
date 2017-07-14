using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Vstack.Common.Extensions;

namespace VsTestRunner
{
    internal static class TestRunner
    {
        public static int RunTests(IEnumerable<string> testBinaries, bool x64, bool coverage)
        {
            bool appveyor = Environment.GetEnvironmentVariable("APPVEYOR") == "True";
            string logger = appveyor ? "/logger:Appveyor" : string.Empty;

            string platform = x64 ? "/Platform:x64 /InIsolation" : string.Empty;

            string vstestArgs = $"{logger} {platform} {testBinaries.Join(" ")}";

            return coverage ?
                RunTestsWithCoverage(vstestArgs) : RunTests(vstestArgs);
        }

        private static int RunTests(string vstestArgs)
        {
            return RunProcess("vstest.console.exe", vstestArgs);
        }

        private static int RunTestsWithCoverage(string vstestArgs)
        {
            RecreateCoverageFolder();

            int testExitCode = RunProcess(
                Paths.OpenCoverPath,
                $@"-register:user -target:vstest.console.exe -targetargs:""{vstestArgs}"" -output:coverage\coverage.xml");

            RunProcess(Paths.ReportGeneratorPath, @"-reports:coverage/coverage.xml -targetdir:coverage");

            return testExitCode;
        }

        private static int RunProcess(string path, string args)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = path;
                process.StartInfo.Arguments = args;
                process.StartInfo.UseShellExecute = false;

                process.Start();
                process.WaitForExit();

                return process.ExitCode;
            }
        }

        private static void RecreateCoverageFolder()
        {
            if (Directory.Exists("coverage"))
            {
                Directory.Delete("coverage", true);
            }

            Directory.CreateDirectory("coverage");
        }
    }
}
