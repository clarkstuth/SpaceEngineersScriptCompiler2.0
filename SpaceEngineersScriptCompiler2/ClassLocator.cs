using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SpaceEngineersScriptCompiler2
{
    public class ClassLocator : SyntaxWalker
    {
        public Dictionary<string, ClassDeclarationSyntax> FindClasses(SyntaxTree syntaxTree)
        {
            var walker = new ClassSortingSyntaxWalker();
            walker.FindClasses(syntaxTree);
            return walker.ClassMap;
        }

        private class ClassSortingSyntaxWalker : SyntaxWalker
        {
            private string @namespace;
            internal readonly Dictionary<string, ClassDeclarationSyntax> ClassMap = new Dictionary<string, ClassDeclarationSyntax>();

            public void FindClasses(SyntaxTree syntaxTree)
            {
                Visit(syntaxTree.GetRoot());
            }

            public override void Visit(SyntaxNode node)
            {
                if (node is ClassDeclarationSyntax)
                {
                    var classNode = node as ClassDeclarationSyntax;
                    var className = string.Concat(@namespace, ".", FindIdentifierToken(classNode).Text);

                    ClassMap.Add(className, classNode);
                    return;
                }

                if (node is NamespaceDeclarationSyntax)
                {
                    @namespace = (node as NamespaceDeclarationSyntax).Name.ToString();
                }
                base.Visit(node);
            }

            private static SyntaxToken FindIdentifierToken(SyntaxNode node)
            {
                return node.ChildTokens().FirstOrDefault(t => t.Kind() == SyntaxKind.IdentifierToken);

            }

        }

    }
}