using System;
using System.IO;
using System.Reflection;

namespace VsTestRunner
{
    public static class Paths
    {
        public static string OpenCoverPath
        {
            get
            {
                string openCoverPath = GetPackagePath("OpenCover");
                return Path.Combine(openCoverPath, "OpenCover.Console.exe");
            }
        }

        public static string ReportGeneratorPath
        {
            get
            {
                string reportGeneratorPath = GetPackagePath("ReportGenerator");
                return Path.Combine(reportGeneratorPath, "ReportGenerator.exe");
            }
        }

        private static string GetPackagePath(string package)
        {
            string installLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string[] openCoverPaths = Directory.GetDirectories(installLocation, $"{package}*");

            string packagePath = null;

            if (openCoverPaths.Length == 1)
            {
                packagePath = openCoverPaths[0];
            }
            else if (openCoverPaths.Length > 1)
            {
                throw new Exception($"Multiple versions of {package} found.");
            }
            else
            {
                throw new Exception($"{package} not found.");
            }

            return packagePath;
        }
    }
}
