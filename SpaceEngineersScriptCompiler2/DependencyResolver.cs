using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace SpaceEngineersScriptCompiler2
{
    public class DependencyResolver
    {
        private readonly Dictionary<string, AnalyzedClass> fullClassToAnalyzedClass =
            new Dictionary<string, AnalyzedClass>();

        public DependencyResolver(List<AnalyzedClass> allAnalyzedClasses)
        {
            allAnalyzedClasses.ForEach(c => fullClassToAnalyzedClass.Add(c.Namespace + "." + c.ClassShortName, c));
        }

        public ISet<AnalyzedClass> ResolveDependencies(AnalyzedClass @class)
        {
            return fullClassToAnalyzedClass
                .Where(kv => @class.ReferencedObjectsWithoutNamespace.Contains(kv.Value.ClassShortName))
                .Select(kv => kv.Value)
                .ToImmutableHashSet();


        }
    }
}