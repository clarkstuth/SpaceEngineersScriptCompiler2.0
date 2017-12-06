using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SpaceEngineersScriptCompiler2
{
    public class AnalyzedClass
    {
        public bool Main { get; set; }
        public ClassDeclarationSyntax ClassTree { get; set; }
        public string ClassShortName { get; set; }
        public string Namespace { get; set; }
    }
}