using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SpaceEngineersScriptCompiler2
{
    public class ClassLocator : SyntaxWalker
    {
        private string nameSpace;
        private readonly Dictionary<string, ClassDeclarationSyntax> classMap = new Dictionary<string, ClassDeclarationSyntax>();

        public Dictionary<string, ClassDeclarationSyntax> FindClasses(SyntaxTree syntaxTree)
        {
            Visit(syntaxTree.GetRoot());

            return classMap;
        }

        public override void Visit(SyntaxNode node)
        {
            if (node is ClassDeclarationSyntax)
            {
                var classNode = node as ClassDeclarationSyntax;
                var className = string.Concat(nameSpace, ".", FindIdentifierToken(classNode).Text);

                classMap.Add(className, classNode);
                return;
            }

            if (node is NamespaceDeclarationSyntax)
            {
                nameSpace = (node as NamespaceDeclarationSyntax).Name.ToString();
                Console.WriteLine(nameSpace);
            }
            base.Visit(node);
        }

        private static SyntaxToken FindIdentifierToken(SyntaxNode node)
        {
            return node.ChildTokens().FirstOrDefault(t => t.Kind() == SyntaxKind.IdentifierToken);

        }

    }
}