using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SpaceEngineersScriptCompiler2
{
    public class FileMetadata
    {
        public SyntaxTree SyntaxTree { get; }
        public String FileName { get; }

        public MethodDeclarationSyntax MainNode { get; private set; }
        public bool HasMain => MainNode != null;

        private readonly MainMethodLocator mainMethodLocator = new MainMethodLocator();

        public FileMetadata(string fileName,SyntaxTree syntaxTree)
        {
            this.FileName = fileName;
            SyntaxTree = syntaxTree;
        }

        public void Update()
        {
            var mainNode = mainMethodLocator.LocateMain(SyntaxTree);
            if (mainNode.HasValue)
            {
                MainNode = mainNode.Value;
            }
        }

        
    }
}
