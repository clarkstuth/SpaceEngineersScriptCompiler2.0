using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SpaceEngineersScriptCompiler2
{
    public class FileMetadata
    {
        private readonly MainMethodLocator mainMethodLocator = new MainMethodLocator();
        private ClassLocator classLocator = new ClassLocator();

        public SyntaxTree SyntaxTree { get; }
        public string FileName { get; }

        public MethodDeclarationSyntax MainNode { get; private set; }
        public bool HasMain => MainNode != null;
        
        public Dictionary<string, ClassDeclarationSyntax> Classes { get; private set; }

        public FileMetadata(string fileName, SyntaxTree syntaxTree)
        {
            Classes = new Dictionary<string, ClassDeclarationSyntax>();
            FileName = fileName;
            SyntaxTree = syntaxTree;
        }

        public void Update()
        {
            var mainNode = mainMethodLocator.LocateMain(SyntaxTree);
            if (mainNode.HasValue)
                MainNode = mainNode.Value;

            Classes = classLocator.FindClasses(SyntaxTree);
        }




    }
}