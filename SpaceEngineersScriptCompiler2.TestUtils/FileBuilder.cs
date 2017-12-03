using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceEngineersScriptCompiler2.TestUtils
{
    public class FileBuilder : ContentGenerator
    {
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int RandomFileLength = 5;
        private static readonly Random Random = new Random();


        public FileBuilder()
        {
            FileName = Enumerable.Repeat(Chars, RandomFileLength).Select(s => s[Random.Next(RandomFileLength)])
                .ToString();
            Classes = new List<ClassBuilder>();
        }

        public string FileName { get; set; }

        public string Namespace { get; set; }
        public List<ClassBuilder> Classes { get; set; }

        public string GetContents()
        {
            var classString = Classes.Select(cb => cb.GetContents()).Aggregate("", (s1, s2) => s1 + " " + s2);
            return $"namespace {Namespace} {{ {classString} }}";
        }
    }
}