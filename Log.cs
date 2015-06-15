using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hawk
{
    public class LogEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public LogEventArgs(string msg)
        {
            Message = msg;
        }
    }

    public class Logger
    {
        public static void Post(string msg)
        {
            // If there are any event handlers, invoke.
            if (evhlog != null)
            {
                LogEventArgs args = new LogEventArgs("[" + DateTime.Now.ToString("dd/MM/yy HH:mm:ss") + "] " + msg);
                evhlog(null, args);
            }

        }
        static public EventHandler evhlog = null;
    }
}
