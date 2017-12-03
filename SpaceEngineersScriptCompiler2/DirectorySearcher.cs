using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceEngineersScriptCompiler2.Tests;

namespace SpaceEngineersScriptCompiler2
{
    public class DirectorySearcher
    {
        private readonly SyntaxTreeParser SyntaxTreeParser = new SyntaxTreeParser();


        public List<FileMetadata> search(DirectoryInfo testDir)
        {
            var files = testDir.EnumerateFiles().Select(f =>
            {
                var metadata = SyntaxTreeParser.parseFile(f);
                metadata.Update();
                return metadata;
            }).ToList();
            return files.FindAll(f => f.HasMain);
        }
    }
}