using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox3D
{
    public class NullLogger : ILogger
    {
        public void LogMessage(string format, params object[] args)
        {
        }

        public void LogWarning(string format, params object[] args)
        {
        }

        public void LogError(string format, params object[] args)
        {
        }
    }
}
