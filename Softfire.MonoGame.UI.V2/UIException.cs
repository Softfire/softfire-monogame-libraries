using System;
using Softfire.MonoGame.LOG;

namespace Softfire.MonoGame.UI
{
    public sealed class UIException : Exception
    {
        private static Logger Logger { get; } = new Logger(@"Config\Logs\UI");

        public UIException(LogTypes logType, string message)
        {
            Logger.Write(logType, message, useInlineLayout: false);
        }
    }
}