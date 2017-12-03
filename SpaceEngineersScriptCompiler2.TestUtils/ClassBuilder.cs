using System.Collections.Generic;
using System.Linq;

namespace SpaceEngineersScriptCompiler2.TestUtils
{
    public class ClassBuilder : ContentGenerator
    {
        public string ClassName { get; set; }
        public List<MethodBuilder> Methods { get; set; }
        public List<ClassBuilder> Classes { get; set; }

        public ClassBuilder()
        {
            Methods = new List<MethodBuilder>();
            Classes = new List<ClassBuilder>();
            ClassName = "Program";
        }


        public bool MainMethod
        {
            get { return Methods.Exists(m => m.MethodName == "Main"); }
            set => WithMainMethod();
        }

        public string GetContents()
        {
            var methods = Methods.Select(m => m.GetContents()).Aggregate("", (m1, m2) => m1 + " " + m2);
            return $"class {ClassName} {{ {methods} }}";
        }

        public ClassBuilder WithMainMethod()
        {
            Methods.Add(new MethodBuilder {MethodName = "Main"});

            return this;
        }
    }
}