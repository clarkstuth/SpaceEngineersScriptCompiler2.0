using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SpaceEngineersScriptCompiler2
{
    public class AnalyzedClass
    {
        public bool HasMain { get; set; }
        public ClassDeclarationSyntax ClassTree { get; set; }
        public string ClassShortName { get; set; }
        public string Namespace { get; set; }
        public ISet<string> ReferencedObjectsWithoutNamespace { get; set; }

        public AnalyzedClass()
        {
            ReferencedObjectsWithoutNamespace = new HashSet<string>();
        }
    }
}