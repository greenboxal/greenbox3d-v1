using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox3D
{
    public interface ILogger
    {
        void LogMessage(string format, params object[] args);
        void LogWarning(string format, params object[] args);
        void LogError(string format, params object[] args);
    }
}
