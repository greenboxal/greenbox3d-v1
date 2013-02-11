using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox3D
{
    public interface ILogger
    {
        void Message(string format, params object[] args);
        void MessageEx(string text, Exception exception);
        void Warning(string format, params object[] args);
        void WarningEx(string text, Exception exception);
        void Error(string format, params object[] args);
        void ErrorEx(string text, Exception exception);
    }
}
