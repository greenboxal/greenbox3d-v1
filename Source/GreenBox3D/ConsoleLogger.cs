using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox3D
{
    public class ConsoleLogger : ILogger
    {
        public void LogMessage(string format, params object[] args)
        {
            Console.WriteLine("[Message] " + format, args);
        }

        public void LogWarning(string format, params object[] args)
        {
            Console.WriteLine("[Warning] " + format, args);
        }

        public void LogError(string format, params object[] args)
        {
            Console.Error.WriteLine("[Error] " + format, args);
        }
    }
}
