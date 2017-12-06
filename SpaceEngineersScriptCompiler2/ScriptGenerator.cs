﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpaceEngineersScriptCompiler2
{
    public class ScriptGenerator
    {
        public string Generate(AnalyzedClass analyzedClass)
        {
            var script = analyzedClass.ClassTree.GetText().ToString();
            script = script.Replace(analyzedClass.ClassTree.Identifier.ToString(), "Program");
            script = RemoveOriginalClassDefinition(script);

            return script;
        }

        private static string RemoveOriginalClassDefinition(string script)
        {
            script = Regex.Replace(script, "^\\s*class\\s*Program\\s*{", "");
            return script.Substring(0, script.LastIndexOf("}") - 1);
        }

        
    }
}