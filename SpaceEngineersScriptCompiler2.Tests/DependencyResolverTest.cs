using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SpaceEngineersScriptCompiler2.Tests
{
    [TestClass]
    public class DependencyResolverTest
    {
        private DependencyResolver GetDependencyResolver(params AnalyzedClass[] analyzedClasses)
        {
            return new DependencyResolver(analyzedClasses.ToList());
        }

        [TestMethod]
        public void ResolveClassDependency()
        {
            var dependantClass = new AnalyzedClass {Namespace = "Namespace", ClassShortName = "ReferencedClass"};
            var classToResolve = new AnalyzedClass
            {
                Namespace = "Namespace",
                ClassShortName = "Program",
                ReferencedObjectsWithoutNamespace = {"ReferencedClass"}
            };

            var dependencies = GetDependencyResolver(dependantClass, classToResolve)
                .ResolveDependencies(classToResolve);

            Assert.IsTrue(new HashSet<AnalyzedClass> {dependantClass}.SetEquals(dependencies));
        }

        [TestMethod]
        public void ResolveMultipleClassDependencies()
        {
            var dependantClass = new AnalyzedClass {Namespace = "Namespace", ClassShortName = "ReferencedClass"};
            var dependantClass2 = new AnalyzedClass {Namespace = "Namespace", ClassShortName = "ReferencedClass2"};
            var classToResolve = new AnalyzedClass
            {
                Namespace = "Namespace",
                ClassShortName = "Program",
                ReferencedObjectsWithoutNamespace = {"ReferencedClass", "ReferencedClass2"}
            };

            var dependencies = GetDependencyResolver(dependantClass, classToResolve, dependantClass2)
                .ResolveDependencies(classToResolve);

            Assert.IsTrue(new HashSet<AnalyzedClass> {dependantClass, dependantClass2}.SetEquals(dependencies));
        }

        [TestMethod]
        public void DoNotResolveAClassThatIsNotReferenced()
        {
            var dependantClass = new AnalyzedClass {Namespace = "Namespace", ClassShortName = "ReferencedClass"};
            var dependantClass2 = new AnalyzedClass {Namespace = "Namespace", ClassShortName = "ReferencedClass2"};
            var notReferencedClass = new AnalyzedClass {Namespace = "Namespace", ClassShortName = "NotReferencedClass"};
            var classToResolve = new AnalyzedClass
            {
                Namespace = "Namespace",
                ClassShortName = "Program",
                ReferencedObjectsWithoutNamespace = {"ReferencedClass", "ReferencedClass2"}
            };

            var dependencies =
                GetDependencyResolver(dependantClass, classToResolve, dependantClass2, notReferencedClass)
                    .ResolveDependencies(classToResolve);

            Assert.IsTrue(new HashSet<AnalyzedClass> {dependantClass, dependantClass2}.SetEquals(dependencies));
        }
    }
}