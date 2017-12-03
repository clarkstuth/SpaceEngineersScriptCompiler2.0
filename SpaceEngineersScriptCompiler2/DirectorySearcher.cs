using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SpaceEngineersScriptCompiler2
{
    public class DirectorySearcher
    {
        private readonly SyntaxTreeParser SyntaxTreeParser = new SyntaxTreeParser();


        public List<FileMetadata> search(DirectoryInfo testDir)
        {
            return testDir.EnumerateFiles().Select(f =>
            {
                var metadata = SyntaxTreeParser.parseFile(f);
                metadata.Update();
                return metadata;
            }).ToList();
        }
    }
}