using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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
                HasMain = HasMainMethod(syntaxTree),
                ReferencedObjectsWithoutNamespace = GetObjectReferences(syntaxTree),
                ClassShortName = syntaxTree.Identifier.Text
            };
        }

        private bool HasMainMethod(SyntaxNode syntax)
        {
            return syntax.DescendantNodes().OfType<MethodDeclarationSyntax>()
                .Any(m => m.Identifier.ToString() == "Main");
        }

        private ISet<string> GetObjectReferences(SyntaxNode node)
        {
            return node.DescendantNodesAndSelf().OfType<ObjectCreationExpressionSyntax>().Select(s => s.Type.ToString())
                .ToImmutableHashSet();
        }
    }
}