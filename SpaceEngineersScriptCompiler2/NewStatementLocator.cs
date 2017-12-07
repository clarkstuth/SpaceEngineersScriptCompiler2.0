using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SpaceEngineersScriptCompiler2
{
    public class NewStatementLocator
    {
        public Dictionary<string, ObjectCreationExpressionSyntax> mapClassNamesToNewStatements(SyntaxTree syntaxTree)
        {
            var walker = new NewStatementSyntaxWalker();
            walker.FindObjectCreationExpressions(syntaxTree.GetRoot());
            return walker.ObjectCreations;
        }

        private class NewStatementSyntaxWalker : SyntaxWalker
        {
            public readonly Dictionary<string, ObjectCreationExpressionSyntax> ObjectCreations =
                new Dictionary<string, ObjectCreationExpressionSyntax>();

            public void FindObjectCreationExpressions(SyntaxNode node)
            {
                Visit(node);
            }

            public override void Visit(SyntaxNode node)
            {
                if (node is ObjectCreationExpressionSyntax)
                {
                    var objNode = node as ObjectCreationExpressionSyntax;
                    var name = objNode.Type.ToString();

                    ObjectCreations.Add(name, objNode);
                }

                base.Visit(node);
            }
        }
    }
}