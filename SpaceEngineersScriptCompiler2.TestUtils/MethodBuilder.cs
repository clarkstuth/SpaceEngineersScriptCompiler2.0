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