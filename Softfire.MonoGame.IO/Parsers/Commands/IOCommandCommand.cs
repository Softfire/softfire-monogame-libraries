using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Softfire.MonoGame.IO.Parsers.Commands
{
    public class IOCommandCommand
    {
        /// <summary>
        /// Command Identifier.
        /// The string used to access the command.
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// Command Description.
        /// Details regarding the command's usage.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Command Syntax.
        /// The form in which to write the command.
        /// </summary>
        public string Syntax { get; }

        /// <summary>
        /// Number Of Required Variables.
        /// </summary>
        public int NumberOfRequiredVariables { get; }

        /// <summary>
        /// Display Help.
        /// </summary>
        public static bool DisplayHelp { get; set; }

        /// <summary>
        /// Command Values.
        /// Values assigned to the command.
        /// </summary>
        private List<string> Values { get; }

        /// <summary>
        /// Command Regex.
        /// </summary>
        private static Regex CommandRegex { get; } = new Regex(@"(?<command>^[\\.+~-]\w+)(?:[:= ]?)(?<values>'?[\w\s]+'? ?|[\\.+~-][\w]+ ?)*");

        /// <summary>
        /// Command Regex.
        /// </summary>
        private static Regex CommandHelpRegex { get; } = new Regex(@"(?<values>[\\.+~-]\bh\b|[\\.+~-]\bhelp\b)");

        /// <summary>
        /// IO Command Command.
        /// </summary>
        /// <param name="identifier">The argument's principal identifier.</param>
        /// <param name="description">A description of the argument's usage.</param>
        /// <param name="syntax">The form in which to write the argument</param>
        /// <param name="numberOfRequiredVariables">The number of variables that are required following the command.</param>
        public IOCommandCommand(string identifier, string description, string syntax, int numberOfRequiredVariables)
        {
            Identifier = identifier;
            Description = description;
            Syntax = syntax;
            NumberOfRequiredVariables = numberOfRequiredVariables;

            Values = new List<string>(NumberOfRequiredVariables);
        }

        /// <summary>
        /// Add Value.
        /// </summary>
        /// <param name="value">The string value to add.</param>
        /// <returns>Returns a bool indicating whether the value was added.</returns>
        public bool AddValue(string value)
        {
            var result = false;

            if (value != null)
            {
                Values.Add(value);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Get Values.
        /// </summary>
        /// <returns>Returns a read-only collection of the command's values.</returns>
        public ReadOnlyCollection<string> GetValues()
        {
            return Values.AsReadOnly();
        }

        /// <summary>
        /// Clear Values.
        /// </summary>
        public void ClearValues()
        {
            Values.Clear();
        }

        /// <summary>
        /// Parse Command.
        /// </summary>
        /// <param name="input">Input to prase.</param>
        /// <returns>Returns a Regex Match.</returns>
        public static Match Parse(string input)
        {
            var parsedInput = CommandRegex.Match(input);

            DisplayHelp = CommandHelpRegex.Match(parsedInput.Groups["values"].Value).Success;

            return parsedInput;
        }
    }
}