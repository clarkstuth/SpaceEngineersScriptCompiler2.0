using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace SpaceEngineersScriptCompiler2.TestUtils
{
    public class RoslynFixture
    {
        public SyntaxTree CreateSyntaxTree(string text)
        {
            return CSharpSyntaxTree.ParseText(text);
        }

        public SyntaxTree CreateSyntaxTree(FileBuilder fileBuilder)
        {
            return CreateSyntaxTree(fileBuilder.GetContents());
        }
    }
}