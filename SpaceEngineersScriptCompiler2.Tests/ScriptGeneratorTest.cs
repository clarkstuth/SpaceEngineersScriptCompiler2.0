using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceEngineersScriptCompiler2.Tests.Fixtures;

namespace SpaceEngineersScriptCompiler2.Tests
{
    [TestClass]
    public class ScriptGeneratorTest
    {
        private RoslynFixture roslynFixture = new RoslynFixture();
        private ScriptGenerator scriptGenerator = new ScriptGenerator();

        [TestMethod]
        public void ShouldReplaceConstructorNameWithProgram()
        {
            var code = "class MyClass { public MyClass() {} }";
            var syntaxTree = roslynFixture.CreateSyntaxTree(code);
            var classTree = roslynFixture.FindFirstClassDeclaration(syntaxTree);

            var script = scriptGenerator.Generate(new AnalyzedClass {ClassTree = classTree, ClassShortName = "MyClass"},
                new HashSet<AnalyzedClass>());

            StringAssert.Contains(script, "public Program() {}");
        }

        [TestMethod]
        public void ShouldReplaceConstructorNameIrregardlessOfSpacing()
        {
            var code = "class AnotherClass { public     AnotherClass()       {} }";
            var syntaxTree = roslynFixture.CreateSyntaxTree(code);
            var classTree = roslynFixture.FindFirstClassDeclaration(syntaxTree);

            var script = scriptGenerator.Generate(
                new AnalyzedClass {ClassTree = classTree, ClassShortName = "AnotherClass"},
                new HashSet<AnalyzedClass>());

            Assert.IsTrue(Regex.IsMatch(script, "public\\s+Program"));
        }

        [TestMethod]
        public void ShouldRemoveOriginalClassDefinition()
        {
            var code = "class MyClass { public MyClass() {  } public HasMain() {} }";
            var syntaxTree = roslynFixture.CreateSyntaxTree(code);
            var classTree = roslynFixture.FindFirstClassDeclaration(syntaxTree);

            var script = scriptGenerator.Generate(new AnalyzedClass {ClassTree = classTree, ClassShortName = "MyClass"},
                new HashSet<AnalyzedClass>());
            Console.WriteLine(script);

            Assert.IsFalse(Regex.IsMatch(script, "class\\s+\\w+\\s*{"));
            Assert.AreEqual(2, script.Count(c => c == '}'));
        }

        [TestMethod]
        public void AddOneDependencyToTheOutput()
        {
            var code = "class MyClass { public MyClass() { var object = new MyObject(); } public HasMain() {} }";
            var syntaxTree = roslynFixture.CreateSyntaxTree(code);
            var classTree = roslynFixture.FindFirstClassDeclaration(syntaxTree);

            var dependencyCode = "class MyObject { }";
            var dependencySyntaxTree = roslynFixture.CreateSyntaxTree(dependencyCode);
            var dependencyClassTree = roslynFixture.FindFirstClassDeclaration(dependencySyntaxTree);
            var dependencies = new HashSet<AnalyzedClass> {new AnalyzedClass {ClassTree = dependencyClassTree}};

            var script =
                scriptGenerator.Generate(new AnalyzedClass
                    {
                        ClassTree = classTree, ClassShortName = "MyClass"
                    },
                    dependencies);

            Console.WriteLine(script);

            StringAssert.Contains(script, "var object = new MyObject();");
            StringAssert.Contains(script, dependencyCode);
        }
    }
}