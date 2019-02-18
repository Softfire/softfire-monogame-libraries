using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Softfire.MonoGame.IO.Parsers.CommandLine
{
    /// <summary>
    /// A command line argument class.
    /// </summary>
    public sealed class IOCommandLineArgument
    {
        /// <summary>
        /// Argument Identifier.
        /// The string used to access the argument.
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// Argument Description.
        /// Details regarding the argument's usage.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Argument Syntax.
        /// The form in which to write the argument.
        /// </summary>
        public string Syntax { get; }

        /// <summary>
        /// Argument Is Required?
        /// </summary>
        public bool IsRequired { get; }

        /// <summary>
        /// Argument Values.
        /// Values assigned to the argument.
        /// </summary>
        private List<string> Values { get; }

        /// <summary>
        /// Arguments Suffixes.
        /// Available suffixes for arguments.
        /// </summary>
        private static Regex SuffixesRegex { get; } = new Regex(@"(?<name>^\w+)(?:=)([""]?)(?<value>[\w `~!@#$%^&*()\-+=\[\]\\{}|;':,./<>?]+)\1$");
        
        /// <summary>
        /// IO Command Line Argument.
        /// Used by the IOCommandLineParser to define an argument.
        /// Generally suffixed with '=' and sometimes ':'.
        /// </summary>
        /// <param name="identifier">The argument's principal identifier.</param>
        /// <param name="description">A description of the argument's usage.</param>
        /// <param name="syntax">The form in which to write the argument</param>
        /// <param name="isRequired">Whether the argument is required.</param>
        public IOCommandLineArgument(string identifier, string description, string syntax, bool isRequired)
        {
            Identifier = identifier;
            Description = description;
            Syntax = syntax;
            IsRequired = isRequired;

            Values = new List<string>();
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
        /// <returns>Returns a read-only collection of the argument's values.</returns>
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
        /// Parse.
        /// </summary>
        /// <param name="argument">The argument to parse.</param>
        /// <returns>Returns a Match indicating whether the argument was parsed successfully along with any matches, if found.</returns>
        public static Match Parse(string argument)
        {
            return SuffixesRegex.Match(argument);
        }
    }
}