using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceEngineersScriptCompiler2.TestUtils;

namespace SpaceEngineersScriptCompiler2.Tests
{
    [TestClass]
    public class SyntaxTreeParserTest
    {
        private readonly FileSystemFixture fileSystemFixture = new FileSystemFixture();
        private readonly SyntaxTreeParser syntaxTreeParser = new SyntaxTreeParser();

        [TestInitialize]
        public void Setup()
        {
            fileSystemFixture.CreateTestDirectory();
        }

        [TestCleanup]
        public void Cleanup()
        {
            fileSystemFixture.RemoveTestDirectory();
        }

        [TestMethod]
        public void CanParseCSharpSyntax()
        {
            const string code =
                "namespace something { class somethingelse { public void methodOne() {} public void methodTwo() {} }}";
            var testFile = fileSystemFixture.WriteTestFile("test.txt", code);

            var syntaxTree = syntaxTreeParser.parseFile(testFile);

            Assert.IsNotNull(syntaxTree);

            var i = syntaxTree.GetRoot().DescendantNodes().First() as NamespaceDeclarationSyntax;
            Assert.IsInstanceOfType(i, typeof(NamespaceDeclarationSyntax));
        }

        [TestMethod]
        public void NonCSharpTreesAreNotParsed()
        {
            const string content = "I'm not a CSharp file at all!";
            var testFile = fileSystemFixture.WriteTestFile("test.txt", content);

            var syntaxTree = syntaxTreeParser.parseFile(testFile);

            Assert.AreEqual(0, syntaxTree.GetRoot().DescendantNodes().Count());
        }
    }
}