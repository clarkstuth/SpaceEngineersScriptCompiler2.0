using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace SpaceEngineersScriptCompiler2
{
    public class SyntaxTreeParser
    {
        public SyntaxTree parseFile(FileInfo file)
        {
            using (var stream = file.OpenRead())
            using (var reader = new StreamReader(stream))
            {
                return CSharpSyntaxTree.ParseText(reader.ReadToEnd());
            }
        }
    }
}