using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceEngineersScriptCompiler2.TestUtils;

namespace SpaceEngineersScriptCompiler2.Tests
{
    [TestClass]
    public class ClassAnalyzerTest
    {
        private ClassAnalyzer classAnalyzer = new ClassAnalyzer();
        private RoslynFixture roslynFixture = new RoslynFixture();

        [TestMethod]
        public void ResultShouldContainClassTreeReferenceAndNamespace()
        {
            var syntaxTree = roslynFixture.CreateSyntaxTree("class MyClass {}");
            var classTree = roslynFixture.FindFirstClassDeclaration(syntaxTree);
            var @namespace = "MyNamespace";

            var analyzedClass = classAnalyzer.Analyze(@namespace, classTree);
            
            Assert.AreSame(classTree, analyzedClass.ClassTree);
            Assert.AreEqual(@namespace, analyzedClass.Namespace);
        }

        [TestMethod]
        public void ResultShouldIndicateIfTheClassHasAMainMethod()
        {
            var syntaxTree = roslynFixture.CreateSyntaxTree("class MyClass { public void Main(string Argument){} }");
            var classTree = roslynFixture.FindFirstClassDeclaration(syntaxTree);
            var @namespace = "MyNamespace";

            var analyzedClass = classAnalyzer.Analyze(@namespace, classTree);

            Assert.IsTrue(analyzedClass.HasMain);
        }

        [TestMethod]
        public void ClassThatDoesNotHaveMain()
        {
            var syntaxTree = roslynFixture.CreateSyntaxTree("class MyClass { public void NotMain(string Argument){} }");
            var classTree = roslynFixture.FindFirstClassDeclaration(syntaxTree);
            var @namespace = "MyNamespace";

            var analyzedClass = classAnalyzer.Analyze(@namespace, classTree);

            Assert.IsFalse(analyzedClass.HasMain);
        }

    }
}