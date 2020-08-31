using System;

namespace Softfire.MonoGame.LOG.V2.ConsoleColorProfiles
{
    /// <summary>
    /// A console logger colored text class.
    /// </summary>
    public sealed class LoggerConsoleColoredText
    {
        /// <summary>
        /// Log Type.
        /// </summary>
        public LogTypes LogType { get; }

        /// <summary>
        /// Foreground Color.
        /// </summary>
        public ConsoleColor ForegroundColor { get; }

        /// <summary>
        /// Background Color.
        /// </summary>
        public ConsoleColor BackgroundColor { get; }

        /// <summary>
        /// Text.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Logger Console Colored Text Constructor.
        /// </summary>
        /// <param name="logType">The log type.</param>
        /// <param name="text">The text to color.</param>
        /// <param name="foregroundColor">The text's foreground color.</param>
        /// <param name="backgroundColor">The text's background color.</param>
        public LoggerConsoleColoredText(LogTypes logType, string text, ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            LogType = logType;
            Text = text;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }

        /// <summary>
        /// Print To Console.
        /// </summary>
        public void PrintToConsole()
        {
            var originalForegroundColor = Console.ForegroundColor;
            var originalBackgroundColor = Console.BackgroundColor;

            Console.ForegroundColor = ForegroundColor;
            Console.BackgroundColor = BackgroundColor;

            Console.Write(Text);
            
            Console.ForegroundColor = originalForegroundColor;
            Console.BackgroundColor = originalBackgroundColor;
        }
    }
}