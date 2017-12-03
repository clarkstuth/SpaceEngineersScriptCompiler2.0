using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceEngineersScriptCompiler2.TestUtils
{
    public class FileBuilder : ContentGenerator
    {
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly Random Random = new Random();
        private const int RandomFileLength = 5;

        public string FileName { get; set; }

        public List<ClassBuilder> Classes { get; set; }
        
        public FileBuilder()
        {
            FileName = Enumerable.Repeat(Chars, RandomFileLength).Select(s => s[Random.Next(RandomFileLength)]).ToString();
            Classes = new List<ClassBuilder>();
        }
        
        public string GetContents()
        {
            var classString = Classes.Select(cb => cb.GetContents()).Aggregate("", (s1, s2) => s1 + " " + s2);
            return $"namespace Test {{ {classString} }}";
        }
    }
}