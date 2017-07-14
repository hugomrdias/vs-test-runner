using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using Vstack.Common.Extensions;

namespace VsTestRunner
{
    [Serializable]
    internal class TestDiscoverer
    {
        public static IEnumerable<string> DiscoverTests(bool release)
        {
            string binFolder = $@"bin\{(release ? "release" : "debug")}";

            return Directory.GetDirectories(Environment.CurrentDirectory, "*", SearchOption.AllDirectories)
                .Where(path => path.ToLower().EndsWith(binFolder))
                .SelectMany(path => DiscoverTests(path))
                .ToList();
        }

        private static IEnumerable<string> DiscoverTests(string path)
        {
            return Directory.GetFiles(path, "*.dll")
                .Where(filePath => IsTestAssembly(filePath))
                .ToList();
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Needs to be instance method.")]
        [SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Reflection.Assembly.LoadFrom", Justification = "No other option.")]
        private static bool IsTestAssembly(string filePath)
        {
            AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(filePath);

            return assembly.MainModule.AssemblyReferences
                .Any(reference => reference.FullName == typeof(TestClassAttribute).Assembly.FullName);
        }
    }
}
