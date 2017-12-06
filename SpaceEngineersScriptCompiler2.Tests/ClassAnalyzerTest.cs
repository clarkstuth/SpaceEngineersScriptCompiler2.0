using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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



    }
}