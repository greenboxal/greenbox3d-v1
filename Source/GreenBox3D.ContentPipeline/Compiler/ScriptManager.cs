using System;
using System.Collections.Generic;

using IronRuby;

using Microsoft.Scripting.Hosting;

namespace GreenBox3D.ContentPipeline.Compiler
{
    public static class ScriptManager
    {
        public static ScriptEngine Engine { get; private set; }
        public static ScriptRuntime Runtime { get; private set; }

        static ScriptManager()
        {
            Engine = Ruby.CreateEngine();
            Runtime = Ruby.CreateRuntime();
        }

        public static ScriptScope CreateScope()
        {
            return Runtime.CreateScope();
        }

        public static void SetMethod(this ScriptScope scope, string name, Delegate method)
        {
            scope.SetVariable(name + "__delegate", method);
            Engine.Execute("def " + name + "(*args)\n" + name + "__delegate.invoke(*args)\nend", scope);
        }

        public static void SetMethodWithBlock(this ScriptScope scope, string name, Delegate method)
        {
            scope.SetVariable(name + "__delegate", method);
            Engine.Execute("def " + name + "(*args, &block)\nargs.push block\n" + name + "__delegate.invoke(*args)\nend", scope);
        }
    }
}
