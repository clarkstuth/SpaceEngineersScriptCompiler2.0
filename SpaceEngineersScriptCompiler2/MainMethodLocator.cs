using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SpaceEngineersScriptCompiler2
{
    public class MainMethodLocator
    {
        public Optional<MethodDeclarationSyntax> LocateMain(SyntaxTree tree)
        {
            var result = tree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().FirstOrDefault(s => s.Identifier.Text == "Main");
            return result == null ? new Optional<MethodDeclarationSyntax>() : new Optional<MethodDeclarationSyntax>(result);
        }
        
    }
}