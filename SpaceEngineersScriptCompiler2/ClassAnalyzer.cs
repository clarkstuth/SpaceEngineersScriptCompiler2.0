using System.Linq;
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
                HasMain = HasMainMethod(syntaxTree)
            };
        }

        private bool HasMainMethod(ClassDeclarationSyntax syntax)
        {
            return syntax.DescendantNodes().OfType<MethodDeclarationSyntax>().Any(m => m.Identifier.ToString() == "Main");
        }
    }
}