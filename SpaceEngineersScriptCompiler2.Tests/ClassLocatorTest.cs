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
            var syntaxTree = roslynFixture.CreateSyntaxTree("namespace A.Memespace { class AClass {}}");
            
            var namespaceToNodeMap = classLocator.FindClasses(syntaxTree);

            Assert.IsNotNull(namespaceToNodeMap.Keys.First());
            Assert.AreEqual("AClass", namespaceToNodeMap.First().Value.Identifier.Text);
        }

        [TestMethod]
        public void FindsMultipleClasses()
        {
            var syntaxTree = roslynFixture.CreateSyntaxTree("namespace A.Bemespace { class AClass {} class ZClass {}}");

            var namespaceToNodeMap = classLocator.FindClasses(syntaxTree);

            Assert.AreEqual(2, namespaceToNodeMap.Count);
            CollectionAssert.Contains(namespaceToNodeMap.Keys, "A.Bemespace.AClass");
            CollectionAssert.Contains(namespaceToNodeMap.Keys, "A.Bemespace.ZClass");
        }

        [TestMethod]
        public void IgnoresInnerClasses()
        {
            var syntaxTree = roslynFixture.CreateSyntaxTree("namespace A.Bemespace { class AClass { class PClass {} } class ZClass {}}");
            
            var namespaceToNodeMap = classLocator.FindClasses(syntaxTree);

            Assert.AreEqual(2, namespaceToNodeMap.Count);
            CollectionAssert.Contains(namespaceToNodeMap.Keys, "A.Bemespace.AClass");
            CollectionAssert.Contains(namespaceToNodeMap.Keys, "A.Bemespace.ZClass");
        }

    }
}