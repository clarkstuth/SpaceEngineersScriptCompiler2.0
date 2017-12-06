using System;
using System.IO;
using System.Linq;

namespace SpaceEngineersScriptCompiler2.Tests.Fixtures
{
    public class CodeExampleFixture
    {
        public static readonly DirectoryInfo CodeExampleDirectory =
            new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent
                .GetDirectories("CodeExamples").First();

        public static readonly DirectoryInfo CodeOutputDirectory =
            new DirectoryInfo(Path.GetTempPath()).CreateSubdirectory("TestCodeOutput");

    }
}