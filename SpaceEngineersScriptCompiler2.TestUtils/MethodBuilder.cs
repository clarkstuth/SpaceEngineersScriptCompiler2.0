using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceEngineersScriptCompiler2.TestUtils
{
    public class MethodBuilder : ContentGenerator
    {
        public string MethodName { get; set; }
        
        public string GetContents()
        {
            return $"public void {MethodName}() {{ }}";
        }
    }
}