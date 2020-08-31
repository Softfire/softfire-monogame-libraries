using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Softfire.MonoGame.IO.V2.Parsers.CommandLine
{
    /// <summary>
    /// A command line option class.
    /// </summary>
    public sealed class IOCommandLineOption
    {
        /// <summary>
        /// Option Identifier.
        /// The string used to access the option.
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// Option Description.
        /// Details regarding the option's usage.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Option Syntax.
        /// The form in which to write the option.
        /// </summary>
        public string Syntax { get; }

        /// <summary>
        /// Option Values.
        /// Values assigned to the option.
        /// </summary>
        public List<string> Values { get; }

        /// <summary>
        /// Option Suffixes.
        /// Available suffixes for options.
        /// </summary>
        private static Regex PrefixesRegex { get; } = new Regex(@"(?:-{1,2})(?<name>\w+)(?:[:=])([""]?)(?<value>[\w `~!@#$%^&*()\-+=\[\]\\{}|;':,./<>?]+)\1$");

        /// <summary>
        /// IO Command Line Option.
        /// Used by the IOCommandLineParser to define an option.
        /// Generally prefixed with '--'.
        /// </summary>
        /// <param name="identifier">The option's principal identifier.</param>
        /// <param name="description">A description of the option's usage.</param>
        /// <param name="syntax">The form in which to write the option.</param>
        public IOCommandLineOption(string identifier, string description, string syntax)
        {
            Identifier = identifier;
            Description = description;
            Syntax = syntax;

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
        /// <returns>Returns a read-only collection of the option's values.</returns>
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
        /// <param name="option">The option to parse.</param>
        /// <returns>Returns a Match indicating whether the option was parsed successfully along with any matches, if found.</returns>
        public static Match Parse(string option)
        {
            return PrefixesRegex.Match(option);
        }
    }
}