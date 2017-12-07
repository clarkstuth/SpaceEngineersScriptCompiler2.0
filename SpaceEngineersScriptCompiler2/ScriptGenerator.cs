using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SpaceEngineersScriptCompiler2
{
    public class ScriptGenerator
    {
        private readonly Regex classDefinitionPattern = new Regex("^\\s*class\\s*Program\\s*{");

        public string Generate(AnalyzedClass analyzedClass, ISet<AnalyzedClass> dependencies)
        {
            var sb = new StringBuilder();

            sb.Append(RemoveOriginalClassDefinition(analyzedClass.ClassTree.GetText().ToString()));
            sb.Replace(analyzedClass.ClassTree.Identifier.ToString(), "Program");
            AddDependencies(dependencies, sb);

            return Regex.Replace(sb.ToString(), "^\\s*class\\s*Program\\s*{", "");
        }

        private string RemoveOriginalClassDefinition(string originalClass)
        {
            var removedMainClass = classDefinitionPattern.Replace(originalClass, "");
            return removedMainClass.Substring(0, removedMainClass.LastIndexOf("}") - 1);
        }

        private static void AddDependencies(ISet<AnalyzedClass> dependencies, StringBuilder sb)
        {
            foreach (var dependency in dependencies)
            {
                sb.Append(dependency.ClassTree.WithModifiers(dependency.ClassTree.Modifiers).ToString());
            }
        }
    }
}