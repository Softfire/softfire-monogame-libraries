using System;
namespace Softfire.MonoGame.LOG.ConsoleColorProfiles
{
    public static class LoggerConsoleColorProfile
    {
        /// <summary>
        /// Info Foreground Color.
        /// </summary>
        public static ConsoleColor InfoForegroundColor { get; set; } = ConsoleColor.Cyan;

        /// <summary>
        /// Info Background Color.
        /// </summary>
        public static ConsoleColor InfoBackgroundColor { get; set; } = ConsoleColor.Black;

        /// <summary>
        /// Console Foreground Color.
        /// </summary>
        public static ConsoleColor ConsoleForegroundColor { get; set; } = ConsoleColor.White;

        /// <summary>
        /// Console Background Color.
        /// </summary>
        public static ConsoleColor ConsoleBackgroundColor { get; set; } = ConsoleColor.Black;

        /// <summary>
        /// Debug Foreground Color.
        /// </summary>
        public static ConsoleColor DebugForegroundColor { get; set; } = ConsoleColor.Magenta;

        /// <summary>
        /// Debug Background Color.
        /// </summary>
        public static ConsoleColor DebugBackgroundColor { get; set; } = ConsoleColor.Black;

        /// <summary>
        /// Warning Foreground Color.
        /// </summary>
        public static ConsoleColor WarningForegroundColor { get; set; } = ConsoleColor.Yellow;

        /// <summary>
        /// Warning Background Color.
        /// </summary>
        public static ConsoleColor WarningBackgroundColor { get; set; } = ConsoleColor.Black;

        /// <summary>
        /// Error Foreground Color.
        /// </summary>
        public static ConsoleColor ErrorForegroundColor { get; set; } = ConsoleColor.Red;

        /// <summary>
        /// Error Background Color.
        /// </summary>
        public static ConsoleColor ErrorBackgroundColor { get; set; } = ConsoleColor.Black;

        /// <summary>
        /// Special Foreground Color.
        /// </summary>
        public static ConsoleColor SpecialForegroundColor { get; set; } = ConsoleColor.DarkYellow;

        /// <summary>
        /// Special Background Color.
        /// </summary>
        public static ConsoleColor SpecialBackgroundColor { get; set; } = ConsoleColor.Black;

        /// <summary>
        /// Print To Console.
        /// </summary>
        /// <param name="coloredText">An array of LoggerColoredText objects.</param>
        /// <param name="printNewLine">A bool indicating whether to print text and follow with a new line (\n) or not.</param>
        public static void PrintToConsole(LoggerConsoleColoredText[] coloredText, bool printNewLine)
        {
            if (coloredText != null &&
                coloredText.Length > 0)
            {
                var originalForegroundColor = Console.ForegroundColor;
                var originalBackgroundColor = Console.BackgroundColor;

                foreach (var text in coloredText)
                {
                    switch (text.LogType)
                    {
                        case LogTypes.Info:
                            Console.ForegroundColor = InfoForegroundColor;
                            Console.BackgroundColor = InfoBackgroundColor;
                            break;
                        case LogTypes.Console:
                            Console.ForegroundColor = ConsoleForegroundColor;
                            Console.BackgroundColor = ConsoleBackgroundColor;
                            break;
                        case LogTypes.Debug:
                            Console.ForegroundColor = DebugForegroundColor;
                            Console.BackgroundColor = DebugBackgroundColor;
                            break;
                        case LogTypes.Warning:
                            Console.ForegroundColor = WarningForegroundColor;
                            Console.BackgroundColor = WarningBackgroundColor;
                            break;
                        case LogTypes.Error:
                            Console.ForegroundColor = ErrorForegroundColor;
                            Console.BackgroundColor = ErrorBackgroundColor;
                            break;
                        case LogTypes.Special:
                            Console.ForegroundColor = SpecialForegroundColor;
                            Console.BackgroundColor = SpecialBackgroundColor;
                            break;
                    }

                    if (printNewLine)
                    {
                        Console.WriteLine(text.Text);
                    }
                    else
                    {
                        Console.Write(text.Text);
                    }
                }

                Console.ForegroundColor = originalForegroundColor;
                Console.BackgroundColor = originalBackgroundColor;
            }
        }

        /// <summary>
        /// Print To Console.
        /// </summary>
        /// <param name="coloredText">A LoggerColoredText object.</param>
        /// <param name="printNewLine">A bool indicating whether to print text and follow with a new line (\n) or not.</param>
        public static void PrintToConsole(LoggerConsoleColoredText coloredText, bool printNewLine = true)
        {
            if (coloredText != null)
            {
                var originalForegroundColor = Console.ForegroundColor;
                var originalBackgroundColor = Console.BackgroundColor;

                switch (coloredText.LogType)
                {
                    case LogTypes.Info:
                        Console.ForegroundColor = InfoForegroundColor;
                        Console.BackgroundColor = InfoBackgroundColor;
                        break;
                    case LogTypes.Console:
                        Console.ForegroundColor = ConsoleForegroundColor;
                        Console.BackgroundColor = ConsoleBackgroundColor;
                        break;
                    case LogTypes.Debug:
                        Console.ForegroundColor = DebugForegroundColor;
                        Console.BackgroundColor = DebugBackgroundColor;
                        break;
                    case LogTypes.Warning:
                        Console.ForegroundColor = WarningForegroundColor;
                        Console.BackgroundColor = WarningBackgroundColor;
                        break;
                    case LogTypes.Error:
                        Console.ForegroundColor = ErrorForegroundColor;
                        Console.BackgroundColor = ErrorBackgroundColor;
                        break;
                    case LogTypes.Special:
                        Console.ForegroundColor = SpecialForegroundColor;
                        Console.BackgroundColor = SpecialBackgroundColor;
                        break;
                }

                if (printNewLine)
                {
                    Console.WriteLine(coloredText.Text);
                }
                else
                {
                    Console.Write(coloredText.Text);
                }

                Console.ForegroundColor = originalForegroundColor;
                Console.BackgroundColor = originalBackgroundColor;
            }
        }

        /// <summary>
        /// Print To Console.
        /// </summary>
        /// <param name="logType">The log type. Info, Console, Debug, Warning, Error or Special.</param>
        /// <param name="text">The string to print to console.</param>
        /// <param name="printNewLine">A bool indicating whether to print text and follow with a new line (\n) or not.</param>
        public static void PrintToConsole(LogTypes logType, string text, bool printNewLine = true)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                var originalForegroundColor = Console.ForegroundColor;
                var originalBackgroundColor = Console.BackgroundColor;

                switch (logType)
                {
                    case LogTypes.Info:
                        Console.ForegroundColor = InfoForegroundColor;
                        Console.BackgroundColor = InfoBackgroundColor;
                        break;
                    case LogTypes.Console:
                        Console.ForegroundColor = ConsoleForegroundColor;
                        Console.BackgroundColor = ConsoleBackgroundColor;
                        break;
                    case LogTypes.Debug:
                        Console.ForegroundColor = DebugForegroundColor;
                        Console.BackgroundColor = DebugBackgroundColor;
                        break;
                    case LogTypes.Warning:
                        Console.ForegroundColor = WarningForegroundColor;
                        Console.BackgroundColor = WarningBackgroundColor;
                        break;
                    case LogTypes.Error:
                        Console.ForegroundColor = ErrorForegroundColor;
                        Console.BackgroundColor = ErrorBackgroundColor;
                        break;
                    case LogTypes.Special:
                        Console.ForegroundColor = SpecialForegroundColor;
                        Console.BackgroundColor = SpecialBackgroundColor;
                        break;
                }

                if (printNewLine)
                {
                    Console.WriteLine(text);
                }
                else
                {
                    Console.Write(text);
                }

                Console.ForegroundColor = originalForegroundColor;
                Console.BackgroundColor = originalBackgroundColor;
            }
        }
    }
}