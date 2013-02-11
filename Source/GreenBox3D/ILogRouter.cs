using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox3D
{
    public interface ILogRouter
    {
        void Output(LogLevel level, string text);
        void OutputEx(LogLevel level, string text, Exception exception);
    }
}
