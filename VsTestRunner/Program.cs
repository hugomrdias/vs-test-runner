using System;
using System.Collections.Generic;
using System.Linq;
using Vstack.Common.Extensions;

namespace VsTestRunner
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            args = args.Select(arg => arg.ToLower()).ToArray();

            bool x64 = args.Contains("--x64");
            bool release = args.Contains("--release");
            bool coverage = args.Contains("--coverage");

            IEnumerable<string> testBinaries = TestDiscoverer.DiscoverTests(release);
            int exitCode = TestRunner.RunTests(testBinaries, x64, coverage);

            Console.WriteLine($"Exiting with code {exitCode}.");
            Environment.Exit(exitCode);
        }
    }
}
