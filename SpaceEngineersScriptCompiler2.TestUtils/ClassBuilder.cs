using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceEngineersScriptCompiler2.TestUtils
{
    public class ClassBuilder : ContentGenerator
    {
        public string ClassName { get; set; }
        public List<MethodBuilder> Methods { get; set; }

        public ClassBuilder()
        {
            Methods = new List<MethodBuilder>();
            ClassName = "Program";
        }

        public bool MainMethod
        {
            get { return Methods.Exists(m => m.MethodName == "Main"); }
            set => WithMainMethod();
        }
        
        public ClassBuilder WithMainMethod()
        {
            Methods.Add(new MethodBuilder {MethodName = "Main"});

            return this;
        }

        public string GetContents()
        {
            var methods = Methods.Select(m => m.GetContents()).Aggregate("", (m1, m2) => m1 + " " + m2);
            return $"class {ClassName} {{ {methods} }}";

        }
    }
}