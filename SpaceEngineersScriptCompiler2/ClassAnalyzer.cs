using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SpaceEngineersScriptCompiler2
{
    public class ClassAnalyzer
    {
        public AnalyzedClass Analyze(string @namespace, ClassDeclarationSyntax syntaxTree)
        {
            return new AnalyzedClass
            {
                Namespace = @namespace,
                ClassTree = syntaxTree,
                Main = true
            };
        }
    }
}