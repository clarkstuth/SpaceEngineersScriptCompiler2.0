using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SpaceEngineersScriptCompiler2.Tests
{
    [TestClass]
    public class CSharpToScriptTransformerTest
    {
        private DirectoryInfo CodeExampleDirectory;

        private DirectoryInfo CodeOutputDirectory =
            new DirectoryInfo(Path.GetTempPath()).CreateSubdirectory("TestCodeOutput");

        private readonly CSharpToScriptTransformer transformer = new CSharpToScriptTransformer();

        private DirectoryInfo singleScriptProgramDir;
        private const string SINGLE_SCRIPT_OUTPUT = "SingleScriptProgram.cs";

        private DirectoryInfo singleProgramTwoScriptsDir;

        [TestInitialize]
        public void Setup()
        {
            CodeExampleDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent
                .GetDirectories("CodeExamples").First();
            if (CodeExampleDirectory.Exists == false)
            {
                throw new FileNotFoundException("CodeExampleDirectory directory not found:" +
                                                CodeExampleDirectory.FullName);
            }

            CodeOutputDirectory.Create();

            singleScriptProgramDir = GetProgramDir("SingleScriptProgram");
            singleProgramTwoScriptsDir = GetProgramDir("SingleProgramTwoScripts");
        }

        [TestCleanup]
        public void Cleanup()
        {
            foreach (var fileInfo in CodeOutputDirectory.GetFiles())
            {
                fileInfo.Delete();
            }
        }

        private DirectoryInfo GetProgramDir(string name)
        {
            return CodeExampleDirectory.GetDirectories(name).First();
        }


        private FileInfo GetOutputScriptFileInfo(string name)
        {
            return CodeOutputDirectory.GetFiles(name).First();
        }

        private string GetOutputScript(string name)
        {
            using (var stream = GetOutputScriptFileInfo(name).OpenRead())
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        [TestMethod]
        public void CanFindOneScriptProgram()
        {
            transformer.Transform(singleScriptProgramDir, CodeOutputDirectory);

            Assert.IsTrue(GetOutputScriptFileInfo(SINGLE_SCRIPT_OUTPUT).Exists);
        }

        [TestMethod]
        public void ChangesClassConstructorNameToProgram()
        {
            transformer.Transform(singleScriptProgramDir, CodeOutputDirectory);

            var code = GetOutputScript(SINGLE_SCRIPT_OUTPUT);
            Assert.IsTrue(Regex.IsMatch(code, "public\\s+Program"));
        }

        [TestMethod]
        public void RemoveNamespaceFromClass()
        {
            transformer.Transform(singleScriptProgramDir, CodeOutputDirectory);

            var code = GetOutputScript(SINGLE_SCRIPT_OUTPUT);
            Console.WriteLine(code);
            Console.WriteLine(CodeOutputDirectory);
            Assert.IsFalse(Regex.IsMatch(code, "namespace\\s+{"));
        }

        [TestMethod]
        public void RemoveOriginalClassDefinition()
        {
            transformer.Transform(singleScriptProgramDir, CodeOutputDirectory);

            var code = GetOutputScript(SINGLE_SCRIPT_OUTPUT);
            Assert.IsFalse(Regex.IsMatch(code, "class|SingleScriptProgram"));
        }

        [TestMethod]
        public void OnlyOutputOneScriptWhenTwoSourceFilesButOneProgram()
        {
            transformer.Transform(singleProgramTwoScriptsDir, CodeOutputDirectory);

            Assert.AreEqual(1, CodeOutputDirectory.GetFiles().Length);
        }

        [TestMethod]
        public void IncludeSecondScriptClassDependencyInsideScript()
        {
            transformer.Transform(singleProgramTwoScriptsDir, CodeOutputDirectory);

            var code = GetOutputScript("TwoScriptProgram.cs");
            Assert.IsTrue(Regex.IsMatch(code, "class\\s+Dependency"));
        }
    }
}