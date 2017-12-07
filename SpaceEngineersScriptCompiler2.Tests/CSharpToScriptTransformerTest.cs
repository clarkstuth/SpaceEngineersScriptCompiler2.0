using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceEngineersScriptCompiler2.Tests.Fixtures;

namespace SpaceEngineersScriptCompiler2.Tests
{
    [TestClass]
    public class CSharpToScriptTransformerTest
    {
        private readonly CSharpToScriptTransformer transformer = new CSharpToScriptTransformer();

        private readonly CodeExampleFixture codeExampleFixture = new CodeExampleFixture();

        private DirectoryInfo singleScriptProgramDir;
        private const string SINGLE_SCRIPT_OUTPUT = "SingleScriptProgram.cs";

        private DirectoryInfo singleProgramTwoScriptsDir;

        [TestInitialize]
        public void Setup()
        {
            CodeExampleFixture.CodeOutputDirectory.Create();

            singleScriptProgramDir = GetProgramDir("SingleScriptProgram");
            singleProgramTwoScriptsDir = GetProgramDir("SingleProgramTwoScripts");
        }

        [TestCleanup]
        public void Cleanup()
        {
            foreach (var fileInfo in CodeExampleFixture.CodeOutputDirectory.GetFiles())
            {
                fileInfo.Delete();
            }
        }

        private DirectoryInfo GetProgramDir(string name)
        {
            return CodeExampleFixture.CodeExampleDirectory.GetDirectories(name).First();
        }

        private FileInfo GetOutputScriptFileInfo(string name)
        {
            return CodeExampleFixture.CodeOutputDirectory.GetFiles(name).First();
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
            transformer.Transform(singleScriptProgramDir, CodeExampleFixture.CodeOutputDirectory);

            Assert.IsTrue(GetOutputScriptFileInfo(SINGLE_SCRIPT_OUTPUT).Exists);
        }

        [TestMethod]
        public void ChangesClassConstructorNameToProgram()
        {
            transformer.Transform(singleScriptProgramDir, CodeExampleFixture.CodeOutputDirectory);

            var code = GetOutputScript(SINGLE_SCRIPT_OUTPUT);
            Assert.IsTrue(Regex.IsMatch(code, "public\\s+Program"));
        }

        [TestMethod]
        public void RemoveNamespaceFromClass()
        {
            transformer.Transform(singleScriptProgramDir, CodeExampleFixture.CodeOutputDirectory);

            var code = GetOutputScript(SINGLE_SCRIPT_OUTPUT);
            Console.WriteLine(code);
            Console.WriteLine(CodeExampleFixture.CodeOutputDirectory);
            Assert.IsFalse(Regex.IsMatch(code, "namespace\\s+{"));
        }

        [TestMethod]
        public void RemoveOriginalClassDefinition()
        {
            transformer.Transform(singleScriptProgramDir, CodeExampleFixture.CodeOutputDirectory);

            var code = GetOutputScript(SINGLE_SCRIPT_OUTPUT);
            Assert.IsFalse(Regex.IsMatch(code, "class|SingleScriptProgram"));
        }

        [TestMethod]
        public void OnlyOutputOneScriptWhenTwoSourceFilesButOneProgram()
        {
            transformer.Transform(singleProgramTwoScriptsDir, CodeExampleFixture.CodeOutputDirectory);

            Assert.AreEqual(1, CodeExampleFixture.CodeOutputDirectory.GetFiles().Length);
        }

        [TestMethod]
        public void IncludeSecondScriptClassDependencyInsideScript()
        {
            transformer.Transform(singleProgramTwoScriptsDir, CodeExampleFixture.CodeOutputDirectory);

            var code = GetOutputScript("TwoScriptProgram.cs");

            Assert.IsTrue(Regex.IsMatch(code, "class Dependency\\s+{\\s+public\\s+void\\s+doSomething\\(\\)\\s+{\\s+}\\s+}"));
        }
    }
}