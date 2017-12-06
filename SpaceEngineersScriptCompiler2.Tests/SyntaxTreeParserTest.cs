using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SpaceEngineersScriptCompiler2.Tests
{
    [TestClass]
    public class SyntaxTreeParserTest
    {
        private readonly SyntaxTreeParser syntaxTreeParser = new SyntaxTreeParser();

        private readonly DirectoryInfo testDirectory =
            new DirectoryInfo(Path.GetTempPath()).CreateSubdirectory("SyntaxTreeParserTest");

        [TestInitialize]
        public void Setup()
        {
            testDirectory.Create();
        }

        [TestCleanup]
        public void Cleanup()
        {
            foreach (var fileInfo in testDirectory.GetFiles())
            {
                fileInfo.Delete();
            }
            testDirectory.Delete();
        }

        private FileInfo WriteTestFile(string filename, string content)
        {
            var file = new FileInfo(testDirectory.FullName + Path.DirectorySeparatorChar + filename);

            using (var stream = file.Create())
            using (var writer = new StreamWriter(stream))
            {
                writer.WriteLine(content);
            }

            return file;
        }

        [TestMethod]
        public void CanParseCSharpSyntax()
        {
            const string code =
                "namespace something { class somethingelse { public void methodOne() {} public void methodTwo() {} }}";
            var testFile = WriteTestFile("test.txt", code);

            var syntaxTree = syntaxTreeParser.parseFile(testFile);

            Assert.IsNotNull(syntaxTree);

            var i = syntaxTree.GetRoot().DescendantNodes().First() as NamespaceDeclarationSyntax;
            Assert.IsInstanceOfType(i, typeof(NamespaceDeclarationSyntax));
        }

        [TestMethod]
        public void NonCSharpTreesAreNotParsed()
        {
            const string content = "I'm not a CSharp file at all!";
            var testFile = WriteTestFile("test.txt", content);

            var syntaxTree = syntaxTreeParser.parseFile(testFile);

            Assert.AreEqual(0, syntaxTree.GetRoot().DescendantNodes().Count());
        }
    }
}