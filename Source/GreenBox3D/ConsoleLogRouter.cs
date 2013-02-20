// GreenBox3D
// 
// Copyright (c) 2013 The GreenBox Development Inc.
// Copyright (c) 2013 Mono.Xna Team and Contributors
// 
// Licensed under MIT license terms.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox3D
{
    public class ConsoleLogRouter : ILogRouter
    {
        #region Public Methods and Operators

        public void Output(LogLevel level, string text)
        {
            Console.WriteLine("{0} {1}", GetPrefix(level), text);
        }

        public void OutputEx(LogLevel level, string text, Exception exception)
        {
            Console.WriteLine("{0} {1}\n", GetPrefix(level), text);
        }

        #endregion

        #region Methods

        private static string GetPrefix(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Message:
                    return "[MESSAGE]";
                case LogLevel.Warning:
                    return "[WARN]";
                case LogLevel.Error:
                    return "[ERROR]";
            }

            return "[LOG]"; // Never should happen
        }

        #endregion
    }
}