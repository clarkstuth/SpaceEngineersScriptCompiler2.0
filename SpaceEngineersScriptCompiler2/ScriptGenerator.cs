using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SpaceEngineersScriptCompiler2
{
    public class ScriptGenerator
    {
        private readonly Regex classDefinitionPattern = new Regex("^\\s*class\\s*Program\\s*{", RegexOptions.Multiline);

        public string Generate(AnalyzedClass analyzedClass, ISet<AnalyzedClass> dependencies)
        {
            var script = StandardizeClassNameToProgram(analyzedClass.ClassTree.GetText().ToString(), analyzedClass.ClassShortName);
            var sb = new StringBuilder(RemoveOriginalClassDefinition(script));
            AddDependencies(dependencies, sb);
            return sb.ToString();
        }

        private string StandardizeClassNameToProgram(string script, string className)
        {
            return script.Replace(className, "Program");
        }

        private string RemoveOriginalClassDefinition(string originalScript)
        {
            var removedMainClass = classDefinitionPattern.Replace(originalScript, "");
            return removedMainClass.Substring(0, removedMainClass.LastIndexOf("}") - 1);
        }

        private static void AddDependencies(ISet<AnalyzedClass> dependencies, StringBuilder sb)
        {
            foreach (var dependency in dependencies)
            {
                sb.Append(dependency.ClassTree.WithModifiers(dependency.ClassTree.Modifiers));
            }
        }
    }
}