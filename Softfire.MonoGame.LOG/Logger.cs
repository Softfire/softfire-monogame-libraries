using System;
using System.Collections.Generic;
using System.IO;
using System.Security;

namespace Softfire.MonoGame.LOG
{
    /// <summary>
    /// Log Types.
    /// </summary>
    public enum LogTypes
    {
        Info,
        Console,
        Debug,
        Warning,
        Error,
        Special
    }

    /// <summary>
    /// Logger.
    /// Logs text to a log file.
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Log File Path.
        /// Relative to calling application's location.
        /// </summary>
        private string LogFilePath { get; set; }

        /// <summary>
        /// Record Log.
        /// An Internal list used to store log entries for later output.
        /// </summary>
        private List<Tuple<DateTime, LogTypes, string, bool>> RecordLog { get; }
        
        /// <summary>
        /// Logger Constructor.
        /// </summary>
        /// <param name="logFilePath">Intakes a log file path for any written logs.</param>
        public Logger(string logFilePath)
        {
            LogFilePath = logFilePath;

            RecordLog = new List<Tuple<DateTime, LogTypes, string, bool>>();
        }

        /// <summary>
        /// Records to an internal Records Log.
        /// </summary>
        /// <param name="logType">The log type.</param>
        /// <param name="text">Intakes a string.</param>
        /// <param name="useInlineLayout">Intakes a bool indicating whether to use inline or formatted output in the log. Default is true.</param>
        public void Record(LogTypes logType, string text, bool useInlineLayout = true)
        {
            if (string.IsNullOrWhiteSpace(text) == false)
            {
                RecordLog.Add(new Tuple<DateTime, LogTypes, string, bool>(DateTime.Now, logType, text, useInlineLayout));
            }
        }

        /// <summary>
        /// Write.
        /// Writes to a log file from the internal Records Log.
        /// </summary>
        /// <param name="fileName">Intakes a file name as a string. Leave off extension. Default extension is .log.</param>
        public void WriteRecords(string fileName = "LOG")
        {
            try
            {
                var filePath = $@"{LogFilePath}\{fileName}_{DateTime.Now:(yyyy-MM-dd)}.log";

                CreateDirectory(filePath);

                using (var logger = new StreamWriter(filePath, true))
                {
                    var line = string.Empty;

                    foreach (var log in RecordLog)
                    {
                        if (log.Item4)
                        {
                            line += $"{log.Item1} - {log.Item2} - {log.Item3}{Environment.NewLine}";
                        }
                        else
                        {
                            line += $"Type: {log.Item2}{Environment.NewLine}" +
                                    $"Time: {log.Item1}{Environment.NewLine}" +
                                    $"Message: {log.Item3}{Environment.NewLine}";
                        }
                    }

                    logger.Write(line);
                }
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is SecurityException ||
                    ex is ArgumentException ||
                    ex is UnauthorizedAccessException ||
                    ex is PathTooLongException ||
                    ex is NotSupportedException ||
                    ex is DirectoryNotFoundException ||
                    ex is IOException ||
                    ex is IndexOutOfRangeException)
                {
                    // TODO: Send Exceptions to central service.
                    throw;
                }
            }
        }

        /// <summary>
        /// Write.
        /// Writes to a log file.
        /// </summary>
        /// <param name="strings">Intakes a 2D string array. Preceed with LOG.LogTypes in [X, 0] and text in [X, 1].</param>
        /// <param name="fileName">Intakes a file name as a string. Leave off extension. Default extension is .log.</param>
        /// <param name="useInlineLayout">Intakes a bool indicating whether to use inline or formatted output in the log. Default is true.</param>
        public void Write(string[,] strings, string fileName = "LOG", bool useInlineLayout = true)
        {
            try
            {
                var filePath = $@"{LogFilePath}\{fileName}_{DateTime.Now:(yyyy-MM-dd)}.log";

                CreateDirectory(filePath);

                using (var logger = new StreamWriter(filePath, true))
                {
                    var line = string.Empty;

                    for (var i = 0; i < strings.GetLength(0); i++)
                    {
                        if (useInlineLayout)
                        {
                            line += strings[i, 0] + $" - {DateTime.Now:yyyy-MM-dd-HH:mm:ss:fff} - {strings[i, 1]}{Environment.NewLine}";
                        }
                        else
                        {
                            line += $"Type: {strings[i, 0]}{Environment.NewLine}" +
                                    $"Time: {DateTime.Now:yyyy-MM-dd-HH:mm:ss:fff}{Environment.NewLine}" +
                                    $"Message: {strings[i, 1]}{Environment.NewLine}";
                        }
                    }

                    logger.Write(line);
                }
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is SecurityException ||
                    ex is ArgumentException ||
                    ex is UnauthorizedAccessException ||
                    ex is PathTooLongException ||
                    ex is NotSupportedException ||
                    ex is DirectoryNotFoundException ||
                    ex is IOException ||
                    ex is IndexOutOfRangeException)
                {
                    // TODO: Send Exceptions to central service.
                    throw;
                }
            }
        }

        /// <summary>
        /// Write.
        /// Writes to a log file.
        /// </summary>
        /// <param name="logType">The log type.</param>
        /// <param name="strings">Intakes a string array.</param>
        /// <param name="fileName">Intakes a file name as a string. Leave off extension. Default extension is .log.</param>
        /// <param name="useInlineLayout">Intakes a bool indicating whether to use inline or formatted output in the log. Default is true.</param>
        public void Write(LogTypes logType, string[] strings, string fileName = "LOG", bool useInlineLayout = true)
        {
            try
            {
                var filePath = $@"{LogFilePath}\{fileName}_{DateTime.Now:(yyyy-MM-dd)}.log";

                CreateDirectory(filePath);

                using (var logger = new StreamWriter(filePath, true))
                {
                    var line = string.Empty;

                    for (var i = 0; i < strings.GetLength(0); i++)
                    {
                        if (useInlineLayout)
                        {
                            line += logType + $" - {DateTime.Now:yyyy-MM-dd-HH:mm:ss:fff} - {strings[i]}{Environment.NewLine}";
                        }
                        else
                        {
                            line += $"Type: {logType}{Environment.NewLine}" +
                                    $"Time: {DateTime.Now:yyyy-MM-dd-HH:mm:ss:fff}{Environment.NewLine}" +
                                    $"Message: {strings[i]}{Environment.NewLine}";
                        }
                    }

                    logger.Write(line);
                }
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is SecurityException ||
                    ex is ArgumentException ||
                    ex is UnauthorizedAccessException ||
                    ex is PathTooLongException ||
                    ex is NotSupportedException ||
                    ex is DirectoryNotFoundException ||
                    ex is IOException ||
                    ex is IndexOutOfRangeException)
                {
                    // TODO: Send Exceptions to central service.
                    throw;
                }
            }
        }

        /// <summary>
        /// Write.
        /// Writes to a log file.
        /// </summary>
        /// <param name="logType">The log type.</param>
        /// <param name="text">Intakes a string.</param>
        /// <param name="fileName">Intakes a file name as a string. Leave off extension. Default extension is .log.</param>
        /// <param name="useInlineLayout">Intakes a bool indicating whether to use inline or formatted output in the log. Default is true.</param>
        public void Write(LogTypes logType, string text, string fileName = "LOG", bool useInlineLayout = true)
        {
            try
            {
                var filePath = $@"{LogFilePath}\{fileName}_{DateTime.Now:(yyyy-MM-dd)}.log";

                CreateDirectory(filePath);

                using (var logger = new StreamWriter(filePath, true))
                {
                    var line = string.Empty;

                    if (useInlineLayout)
                    {
                        line += logType + $" - {DateTime.Now:yyyy-MM-dd-HH:mm:ss:fff} - {text}{Environment.NewLine}";
                    }
                    else
                    {
                        line += $"Type: {logType}{Environment.NewLine}" +
                                $"Time: {DateTime.Now:yyyy-MM-dd-HH:mm:ss:fff}{Environment.NewLine}" +
                                $"Message: {text}{Environment.NewLine}";
                    }

                    logger.Write(line);
                }
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is SecurityException ||
                    ex is ArgumentException ||
                    ex is UnauthorizedAccessException ||
                    ex is PathTooLongException ||
                    ex is NotSupportedException ||
                    ex is DirectoryNotFoundException ||
                    ex is IOException ||
                    ex is IndexOutOfRangeException)
                {
                    // TODO: Send Exceptions to central service.
                    throw;
                }
            }
        }

        /// <summary>
        /// Write.
        /// </summary>
        /// <param name="filePath">The file path. Leave off trailing slash.</param>
        /// <param name="logType">The log type.</param>
        /// <param name="text">Intakes a string.</param>
        /// <param name="fileName">Intakes a file name as a string. Leave off extension. Default extension is .log.</param>
        /// <param name="useInlineLayout">Intakes a bool indicating whether to use inline or formatted output in the log. Default is true.</param>
        public static void Write(string filePath, LogTypes logType, string text, string fileName = "LOG", bool useInlineLayout = true)
        {
            try
            {
                var localFilePath = $@"{filePath}\{fileName}_{DateTime.Now:(yyyy-MM-dd)}.log";

                CreateDirectory(localFilePath);

                using (var logger = new StreamWriter(localFilePath, true))
                {
                    var line = string.Empty;

                    if (useInlineLayout)
                    {
                        line += logType + $" - {DateTime.Now:yyyy-MM-dd-HH:mm:ss:fff} - {text}{Environment.NewLine}";
                    }
                    else
                    {
                        line += $"Type: {logType}{Environment.NewLine}" +
                                $"Time: {DateTime.Now:yyyy-MM-dd-HH:mm:ss:fff}{Environment.NewLine}" +
                                $"Message: {text}{Environment.NewLine}";
                    }

                    logger.Write(line);
                }
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is SecurityException ||
                    ex is ArgumentException ||
                    ex is UnauthorizedAccessException ||
                    ex is PathTooLongException ||
                    ex is NotSupportedException ||
                    ex is DirectoryNotFoundException ||
                    ex is IOException ||
                    ex is IndexOutOfRangeException)
                {
                    // TODO: Send Exceptions to central service.
                    throw;
                }
            }
        }

        /// <summary>
        /// Create Directory.
        /// Creates a directory to store the files being written by the current logger.
        /// </summary>
        /// <param name="filePath">The file path of where the log is to be created.</param>
        public static void CreateDirectory(string filePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filePath) == false)
                {
                    var fi = new FileInfo(filePath);

                    if (fi.Directory != null &&
                        fi.Directory.Exists == false &&
                        fi.DirectoryName != null)
                    {
                        Directory.CreateDirectory(fi.DirectoryName);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is SecurityException ||
                    ex is ArgumentException ||
                    ex is UnauthorizedAccessException ||
                    ex is PathTooLongException ||
                    ex is NotSupportedException ||
                    ex is DirectoryNotFoundException ||
                    ex is IOException)
                {
                    // TODO: Send Exception to a central service.
                    throw;
                }
            }
        }
    }
}