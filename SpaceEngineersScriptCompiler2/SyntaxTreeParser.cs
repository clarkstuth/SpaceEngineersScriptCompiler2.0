using System.IO;
using Microsoft.CodeAnalysis.CSharp;

namespace SpaceEngineersScriptCompiler2
{
    public class SyntaxTreeParser
    {
        public FileMetadata parseFile(FileInfo file)
        {
            using (var stream = file.OpenRead())
            using (var reader = new StreamReader(stream))
            {
                return new FileMetadata(file.Name, CSharpSyntaxTree.ParseText(reader.ReadToEnd()));
            }
        }
    }
}