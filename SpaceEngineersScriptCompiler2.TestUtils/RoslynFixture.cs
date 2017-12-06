using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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

        public ClassDeclarationSyntax FindFirstClassDeclaration(SyntaxTree syntaxTree)
        {
            try
            {
                return syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().First();
            }
            catch (Exception e)
            {
                throw new Exception("Unable to locate Class in SyntaxTree", e);
            }
        }

    }
}