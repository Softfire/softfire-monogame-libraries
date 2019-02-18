using System.Text.RegularExpressions;

namespace Softfire.MonoGame.IO.Parsers.CommandLine
{
    /// <summary>
    /// A command line flag.
    /// </summary>
    public sealed class IOCommandLineFlag
    {
        /// <summary>
        /// Flag Identifier.
        /// The string used to access the flag.
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// Flag Description.
        /// Details regarding the flag's usage.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Flag Syntax.
        /// The form in which to write the flag.
        /// </summary>
        public string Syntax { get; }

        /// <summary>
        /// Flag Is Required?
        /// </summary>
        public bool IsRequired { get; }

        /// <summary>
        /// Flag Is Present?
        /// </summary>
        public bool IsPresent { get; set; }

        /// <summary>
        /// Flag Prefixes.
        /// Available prefixes for flags.
        /// </summary>
        private static Regex PrefixesRegex { get; } = new Regex(@"(?:/)(?<flag>\S+) ?$");
        
        /// <summary>
        /// IO Command Line Flag.
        /// Used by the IOCommandLineParser to define a flag.
        /// Generally prefixed with '/'.
        /// </summary>
        /// <param name="identifier">The flag's principal identifier.</param>
        /// <param name="description">A description of the flag's usage.</param>
        /// <param name="syntax">The form in which to write the flag</param>
        /// <param name="isRequired">Whether the flag is required.</param>
        public IOCommandLineFlag(string identifier, string description, string syntax, bool isRequired)
        {
            Identifier = identifier;
            Description = description;
            Syntax = syntax;
            IsRequired = isRequired;
            IsPresent = false;
        }

        /// <summary>
        /// Parse.
        /// </summary>
        /// <param name="flag">The flag to parse.</param>
        /// <returns>Returns a Match indicating whether the flag was parsed successfully along with any matches, if found.</returns>
        public static Match Parse(string flag)
        {
            return PrefixesRegex.Match(flag);
        }
    }
}