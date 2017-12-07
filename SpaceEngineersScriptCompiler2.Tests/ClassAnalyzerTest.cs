using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceEngineersScriptCompiler2.Tests.Fixtures;

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
            var syntaxTree = roslynFixture.CreateSyntaxTree("class MyClass { public void Main(string argument){} }");
            var classTree = roslynFixture.FindFirstClassDeclaration(syntaxTree);
            var @namespace = "MyNamespace";

            var analyzedClass = classAnalyzer.Analyze(@namespace, classTree);

            Assert.IsTrue(analyzedClass.HasMain);
        }

        [TestMethod]
        public void ClassThatDoesNotHaveMain()
        {
            var syntaxTree = roslynFixture.CreateSyntaxTree("class MyClass { public void NotMain(string argument){} }");
            var classTree = roslynFixture.FindFirstClassDeclaration(syntaxTree);
            var @namespace = "MyNamespace";

            var analyzedClass = classAnalyzer.Analyze(@namespace, classTree);

            Assert.IsFalse(analyzedClass.HasMain);
        }

        [TestMethod]
        public void ExtractReferencedObject()
        {
            var syntaxTree =
                roslynFixture.CreateSyntaxTree(
                    "class MyClass { public void Main(string argument){var object = new AnotherObject();} }");
            var classTree = roslynFixture.FindFirstClassDeclaration(syntaxTree);
            var @namespace = "ProjectNamespace";

            var analyzedClass = classAnalyzer.Analyze(@namespace, classTree);

            Assert.IsTrue(analyzedClass.ReferencedObjectsWithoutNamespace.Contains("AnotherObject"));
        }

        [TestMethod]
        public void ExtractMultipleReferencedObject()
        {
            var syntaxTree =
                roslynFixture.CreateSyntaxTree(
                    "class MyClass { public void Main(string argument){var object = new AnotherObject(); var object2 = new YetAnotherObject();} }");
            var classTree = roslynFixture.FindFirstClassDeclaration(syntaxTree);
            var @namespace = "ProjectNamespace";

            var analyzedClass = classAnalyzer.Analyze(@namespace, classTree);


            Assert.IsTrue(analyzedClass.ReferencedObjectsWithoutNamespace.Contains("AnotherObject"));
            Assert.IsTrue(analyzedClass.ReferencedObjectsWithoutNamespace.Contains("YetAnotherObject"));
        }

        [TestMethod]
        public void AddClassShortName()
        {
            var syntaxTree = roslynFixture.CreateSyntaxTree("class MyClass {}");
            var classTree = roslynFixture.FindFirstClassDeclaration(syntaxTree);
            var @namespace = "MyNamespace";

            var analyzedClass = classAnalyzer.Analyze(@namespace, classTree);

            Assert.AreEqual("MyClass",  analyzedClass.ClassShortName);
        }

        [TestMethod]
        public void DifferentClassShortName()
        {
            var syntaxTree = roslynFixture.CreateSyntaxTree("class AnotherClass {}");
            var classTree = roslynFixture.FindFirstClassDeclaration(syntaxTree);
            var @namespace = "MyNamespace";

            var analyzedClass = classAnalyzer.Analyze(@namespace, classTree);

            Assert.AreEqual("AnotherClass", analyzedClass.ClassShortName);
        }

    }
}