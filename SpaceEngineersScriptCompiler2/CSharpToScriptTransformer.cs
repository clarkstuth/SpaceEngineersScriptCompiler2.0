using System.Collections.Generic;
using System.IO;

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
            var files = sourceDirectory.GetFiles("SingleScriptProgram.cs");

            var analyzedClasses = new Dictionary<FileInfo, AnalyzedClass>();
            foreach (var fileInfo in files)
            {
                var syntaxTree = syntaxTreeParser.parseFile(fileInfo);
                foreach (var namespaceToClassTree in classLocator.FindClasses(syntaxTree))
                {
                    analyzedClasses.Add(fileInfo, classAnalyzer.Analyze(namespaceToClassTree.Key, namespaceToClassTree.Value));
                }
            }

            foreach (var fileAndClass in analyzedClasses)
            {
                var fileName = outputDirectory.FullName + Path.DirectorySeparatorChar + fileAndClass.Key.Name;
                using (var fileInfo = new FileInfo(fileName).Create())
                using (var writer = new StreamWriter(fileInfo))
                {
                    writer.Write(scriptGenerator.Generate(fileAndClass.Value));
                }
            }
        }
    }
}