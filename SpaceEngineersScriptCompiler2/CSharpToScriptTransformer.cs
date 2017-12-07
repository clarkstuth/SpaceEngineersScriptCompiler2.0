using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SpaceEngineersScriptCompiler2
{
    public class CSharpToScriptTransformer
    {
        private readonly SyntaxTreeParser syntaxTreeParser = new SyntaxTreeParser();
        private readonly ClassAnalyzer classAnalyzer = new ClassAnalyzer();
        private readonly ClassLocator classLocator = new ClassLocator();
        private readonly ScriptGenerator scriptGenerator = new ScriptGenerator();

        public void Transform(DirectoryInfo sourceDirectory, DirectoryInfo outputDirectory)
        {
            var files = sourceDirectory.GetFiles("*.cs");

            var analyzedClasses = new Dictionary<FileInfo, AnalyzedClass>();
            foreach (var fileInfo in files)
            {
                var syntaxTree = syntaxTreeParser.parseFile(fileInfo);
                foreach (var namespaceToClassTree in classLocator.FindClasses(syntaxTree))
                {
                    analyzedClasses.Add(fileInfo,
                        classAnalyzer.Analyze(namespaceToClassTree.Key, namespaceToClassTree.Value));
                }
            }

            var dependencyResolver = new DependencyResolver(analyzedClasses.Values.ToList());
            foreach (var fileAndClass in analyzedClasses.Where(a => a.Value.HasMain))
            {
                var fileName = outputDirectory.FullName + Path.DirectorySeparatorChar + fileAndClass.Key.Name;
                var dependencies = dependencyResolver.ResolveDependencies(fileAndClass.Value);
                using (var fileInfo = new FileInfo(fileName).Create())
                using (var writer = new StreamWriter(fileInfo))
                {
                    writer.Write(scriptGenerator.Generate(fileAndClass.Value, dependencies));
                }
            }
        }

    }
}