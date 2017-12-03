using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
