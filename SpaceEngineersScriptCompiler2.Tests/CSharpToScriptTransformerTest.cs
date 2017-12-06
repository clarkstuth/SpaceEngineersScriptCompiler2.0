using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
        private string singleScriptOutput = "SingleScriptProgram.cs";

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

            Assert.IsTrue(GetOutputScriptFileInfo(singleScriptOutput).Exists);
        }

        [TestMethod]
        public void ChangesClassConstructorNameToProgram()
        {
            transformer.Transform(singleScriptProgramDir, CodeOutputDirectory);

            var code = GetOutputScript(singleScriptOutput);
            StringAssert.Contains(code, "public Program()");
        }

        [TestMethod]
        public void RemoveNamespaceFromClass()
        {
            transformer.Transform(singleScriptProgramDir, CodeOutputDirectory);

            var code = GetOutputScript(singleScriptOutput);
            Console.WriteLine(code);
            Console.WriteLine(CodeOutputDirectory);
            Assert.IsFalse(code.Contains("namespace"));
        }

        [TestMethod]
        public void RemoveOriginalClassDefinition()
        {
            transformer.Transform(singleScriptProgramDir, CodeOutputDirectory);

            var code = GetOutputScript(singleScriptOutput);
            
            Assert.IsFalse(Regex.IsMatch(code, "class|SingleScriptProgram"));
        }

    }
}