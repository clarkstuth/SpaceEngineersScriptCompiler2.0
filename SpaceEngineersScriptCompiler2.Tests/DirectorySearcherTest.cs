using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceEngineersScriptCompiler2.TestUtils;

namespace SpaceEngineersScriptCompiler2.Tests
{
    [TestClass]
    public class DirectorySearcherTest
    {
        private readonly FileSystemFixture fileSystemFixture = new FileSystemFixture();

        private readonly DirectorySearcher locator = new DirectorySearcher();
        private DirectoryInfo testDir;

        [TestInitialize]
        public void Setup()
        {
            testDir = fileSystemFixture.CreateTestDirectory();
        }

        [TestCleanup]
        public void CleanUp()
        {
            fileSystemFixture.RemoveTestDirectory();
        }

        [TestMethod]
        public void NoScriptsWithMainMethodReturnsEmptyList()
        {
            var files = locator.search(testDir);

            Assert.IsTrue(files.Count == 0);
        }

        [TestMethod]
        public void SingleFileWithQualifyingScriptIsReturned()
        {
            var testFile = new FileBuilder
            {
                FileName = "file.cs",
                Classes = {new ClassBuilder {MainMethod = true}}
            };
            fileSystemFixture.WriteTestFile(testFile);

            var files = locator.search(testDir);
            
            Assert.AreEqual(1, files.Count);
            Assert.AreEqual(testFile.FileName, files[0].FileName);
        }

        [TestMethod]
        public void OnlyPickUpFilesWithMainMethods()
        {
            FileBuilder file = new FileBuilder
            {
                FileName = "valid.cs",
                Classes = {new ClassBuilder {MainMethod = true, ClassName = "ValidClass"}}
            };
            fileSystemFixture.WriteTestFile(file);
            fileSystemFixture.WriteTestFile("invalid.cs", "namespace Invalid { class InvalidClass { public void NotMain() {} } }");

            var files = locator.search(testDir);
            
            Assert.AreEqual(1, files.Count);
            Assert.AreEqual(file.FileName, files[0].FileName);
        }
    }
}