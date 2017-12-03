using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceEngineersScriptCompiler2.TestUtils;

namespace SpaceEngineersScriptCompiler2.Tests
{
    [TestClass]
    public class DirectorySearcherTest
    {
        private readonly FileSystemFixture fileSystemFixture = new FileSystemFixture();

        private readonly DirectorySearcher searcher = new DirectorySearcher();
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
        public void NoScriptsReturnsEmptyList()
        {
            var files = searcher.search(testDir);

            Assert.IsTrue(files.Count == 0);
        }

        [TestMethod]
        public void SingleFileIsReturned()
        {
            var testFile = new FileBuilder
            {
                FileName = "file.cs",
                Classes = {new ClassBuilder {MainMethod = true}}
            };
            fileSystemFixture.WriteTestFile(testFile);

            var files = searcher.search(testDir);

            Assert.AreEqual(1, files.Count);
            Assert.AreEqual(testFile.FileName, files[0].FileName);
        }

        [TestMethod]
        public void MultipleFilesAreReturned()
        {
            var file = new FileBuilder
            {
                FileName = "valid.cs",
                Classes = {new ClassBuilder {MainMethod = true, ClassName = "ValidClass"}}
            };
            fileSystemFixture.WriteTestFile(file);
            fileSystemFixture.WriteTestFile("invalid.cs",
                "namespace Invalid { class InvalidClass { public void NotMain() {} } }");

            var files = searcher.search(testDir);

            Assert.AreEqual(2, files.Count);
            Assert.AreEqual("invalid.cs", files[0].FileName);
            Assert.AreEqual(file.FileName, files[1].FileName);
        }

        [TestMethod]
        public void AbleToTellIfClassFeaturesMainMethod()
        {
            var file = new FileBuilder
            {
                FileName = "valid.cs",
                Classes = {new ClassBuilder {MainMethod = true, ClassName = "ValidClass"}}
            };
            fileSystemFixture.WriteTestFile(file);
            fileSystemFixture.WriteTestFile("invalid.cs",
                "namespace Invalid { class InvalidClass { public void NotMain() {} } }");

            var files = searcher.search(testDir);

            Assert.IsFalse(files[0].HasMain);
            Assert.IsTrue(files[1].HasMain);
        }

        [TestMethod]
        public void CapableOfReadingClassesInFilesFile()
        {
            var file = new FileBuilder
            {
                FileName = "valid.cs",
                Namespace = "some.lamespace.to.read",
                Classes = {new ClassBuilder {MainMethod = true, ClassName = "ValidClass"}}
            };
            var file2 = new FileBuilder
            {
                FileName = "also_valid.cs",
                Namespace = "another.lamespace.to.read",
                Classes = { new ClassBuilder { MainMethod = true, ClassName = "AnotherClass" } }
            };
            fileSystemFixture.WriteTestFile(file);
            fileSystemFixture.WriteTestFile(file2);

            var files = searcher.search(testDir);

            Assert.AreEqual(2, files.Count);
            Assert.AreEqual(1, files[0].Classes.Keys.Count);
            Assert.AreEqual("another.lamespace.to.read.AnotherClass", files[0].Classes.Keys.First());
            Assert.AreEqual(1, files[1].Classes.Keys.Count);
            Assert.AreEqual("some.lamespace.to.read.ValidClass", files[1].Classes.Keys.First());
        }
    }
}