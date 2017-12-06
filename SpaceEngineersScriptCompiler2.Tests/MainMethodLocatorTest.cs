using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceEngineersScriptCompiler2.TestUtils;

namespace SpaceEngineersScriptCompiler2.Tests
{
    [TestClass]
    public class MainMethodLocatorTest
    {
        private readonly MainMethodLocator mainMethodLocator = new MainMethodLocator();
        private readonly RoslynFixture roslynFixture = new RoslynFixture();

        [TestMethod]
        public void FileWithMethodNotCalledMainDoesNotHaveMainSyntaxNode()
        {
            var syntaxTree = roslynFixture.CreateSyntaxTree(new FileBuilder
            {
                FileName = "NotMain.cs",
                Classes = {new ClassBuilder {Methods = {new MethodBuilder {MethodName = "NotMain"}}}}
            });

            var nodeOptional = mainMethodLocator.LocateMain(syntaxTree);

            Assert.IsFalse(nodeOptional.HasValue);
        }

        [TestMethod]
        public void FileWithMainMethodHasMainSyntaxNode()
        {
            var file = new FileBuilder
            {
                FileName = "NotMain.cs",
                Classes =
                {
                    new ClassBuilder
                    {
                        MainMethod = true,
                        Methods =
                        {
                            new MethodBuilder {MethodName = "AnotherMethod"}
                        }
                    }
                }
            };


            var nodeOptional = mainMethodLocator.LocateMain(roslynFixture.CreateSyntaxTree(file));

            Assert.IsTrue(nodeOptional.HasValue);
        }
    }
}