using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceEngineersScriptCompiler2.TestUtils;

namespace SpaceEngineersScriptCompiler2.Tests
{
    [TestClass]
    public class ClassLocatorTest
    {
        private readonly RoslynFixture roslynFixture = new RoslynFixture();
        private readonly ClassLocator classLocator = new ClassLocator();

        [TestMethod]
        public void CanLocateSingleClassWithNamespace()
        {
            var syntaxTree = roslynFixture.CreateSyntaxTree("namespace A.Memespace { class AClass {  } }");

            var namespaceToNodeMap = classLocator.FindClasses(syntaxTree);
            
            Assert.AreEqual(1, namespaceToNodeMap.Keys.Count);
            Assert.AreEqual("A.Memespace.AClass", namespaceToNodeMap.Keys.First());
        }

        [TestMethod]
        public void LocatesStartingClassNodeAlongWithNamespace()
        {
            var file = new FileBuilder
            {
                Namespace = "A.Memespace",
                Classes = { new ClassBuilder { ClassName = "AClass" } }
            };
            var syntaxTree = roslynFixture.CreateSyntaxTree(file);

            var namespaceToNodeMap = classLocator.FindClasses(syntaxTree);

            Assert.IsNotNull(namespaceToNodeMap.Keys.First());
            Assert.AreEqual("AClass", namespaceToNodeMap.First().Value.Identifier.Text);
        }

        [TestMethod]
        public void FindsMultipleClasses()
        {
            var file = new FileBuilder
            {
                Namespace = "A.Bemespace",
                Classes = { new ClassBuilder { ClassName = "AClass" }, new ClassBuilder { ClassName = "ZClass"} }
            };
            var syntaxTree = roslynFixture.CreateSyntaxTree(file);

            var namespaceToNodeMap = classLocator.FindClasses(syntaxTree);

            Assert.AreEqual(2, namespaceToNodeMap.Count);
            CollectionAssert.Contains(namespaceToNodeMap.Keys, "A.Bemespace.AClass");
            CollectionAssert.Contains(namespaceToNodeMap.Keys, "A.Bemespace.ZClass");
        }

        [TestMethod]
        public void IgnoresInnerClasses()
        {
            var file = new FileBuilder
            {
                Namespace = "A.Bemespace",
                Classes =
                {
                    new ClassBuilder {ClassName = "AClass", Classes = {new ClassBuilder {ClassName = "P.Class"}}},
                    new ClassBuilder {ClassName = "ZClass"}
                }
            };
            var syntaxTree = roslynFixture.CreateSyntaxTree(file);

            var namespaceToNodeMap = classLocator.FindClasses(syntaxTree);

            Assert.AreEqual(2, namespaceToNodeMap.Count);
            CollectionAssert.Contains(namespaceToNodeMap.Keys, "A.Bemespace.AClass");
            CollectionAssert.Contains(namespaceToNodeMap.Keys, "A.Bemespace.ZClass");
        }

    }
}