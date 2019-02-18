using System.Collections.Generic;

namespace Softfire.MonoGame.IO.Parsers.CommandLine
{
    /// <summary>
    /// A command line parser.
    /// </summary>
    public sealed class IOCommandLineParser
    {
        /// <summary>
        /// Command Line Arguments.
        /// </summary>
        private Dictionary<string, IOCommandLineArgument> Arguments { get; }

        /// <summary>
        /// Command Line Flags.
        /// </summary>
        private Dictionary<string, IOCommandLineFlag> Flags { get; }

        /// <summary>
        /// Command Line Options.
        /// </summary>
        private Dictionary<string, IOCommandLineOption> Options { get; }

        /// <summary>
        /// IO Command Line Parser.
        /// Used in Main to parse command line arguments.
        /// </summary>
        public IOCommandLineParser()
        {
            Arguments = new Dictionary<string, IOCommandLineArgument>();
            Flags = new Dictionary<string, IOCommandLineFlag>();
            Options = new Dictionary<string, IOCommandLineOption>();
        }

        /// <summary>
        /// Add Argument.
        /// </summary>
        /// <param name="identifier">The argument's principal identifier.</param>
        /// <param name="description">A short description of the argument's usage.</param>
        /// <param name="syntax">Details regarding how the argument should be used/written.</param>
        /// <param name="isRequired">Whether the argument is required.</param>
        /// <returns>Returns a bool indicating whether the argument was added.</returns>
        public bool AddArgument(string identifier, string description, string syntax, bool isRequired)
        {
            var result = false;

            if (!Arguments.ContainsKey(identifier))
            {
                Arguments.Add(identifier, new IOCommandLineArgument(identifier, description, syntax, isRequired));
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Add Argument Variant.
        /// Adds a variant identifier for an already existing argument.
        /// </summary>
        /// <param name="identifier">The argument's principal identifier. Intaken as a <see cref="string"/>.</param>
        /// <param name="variantIdentifier">The argument's variant identifier. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a bool indicating whether the variant was added successfully.</returns>
        public bool AddArgumentVariant(string identifier, string variantIdentifier)
        {
            var result = false;
            IOCommandLineArgument argument;

            if ((argument = GetArgument(identifier)) != null)
            {
                Arguments.Add(variantIdentifier, argument);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Get Argument.
        /// </summary>
        /// <param name="identifier">The argument's identifier.</param>
        /// <returns>Returns the requested argument or null, if not found.</returns>
        public IOCommandLineArgument GetArgument(string identifier)
        {
            IOCommandLineArgument result = null;

            if (Arguments.ContainsKey(identifier))
            {
                result = Arguments[identifier];
            }

            return result;
        }

        /// <summary>
        /// Remove Argument.
        /// </summary>
        /// <param name="identifier">The argument's principal identifier.</param>
        /// <returns>Returns a bool indicating whether the argument was removed.</returns>
        public bool RemoveArgument(string identifier)
        {
            var result = false;

            if (Arguments.ContainsKey(identifier))
            {
                result = Arguments.Remove(identifier);
            }

            return result;
        }

        /// <summary>
        /// Parse Argument.
        /// </summary>
        /// <param name="argument">The string to parse.</param>
        /// <returns>Returns a bool indicating whether the argument was parsed successfully.</returns>
        private bool ParseArgument(string argument)
        {
            var result = IOCommandLineArgument.Parse(argument);

            if (result.Success)
            {
                var identifier = result.Groups["name"].Value;
                var value = result.Groups["value"].Value;

                if (Arguments.ContainsKey(identifier))
                {
                    Arguments[identifier].AddValue(value);
                }
            }

            return result.Success;
        }

        /// <summary>
        /// Add Flag.
        /// </summary>
        /// <param name="identifier">The flag's principal identifier.</param>
        /// <param name="description">A short description of the flag's usage.</param>
        /// <param name="syntax">Details regarding how the flag should be used/written.</param>
        /// <param name="isRequired">Whether the flag is required.</param>
        /// <returns>Returns a bool indicating whether the flag was added.</returns>
        public bool AddFlag(string identifier, string description, string syntax, bool isRequired)
        {
            var result = false;

            if (!Flags.ContainsKey(identifier))
            {
                Flags.Add(identifier, new IOCommandLineFlag(identifier, description, syntax, isRequired));
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Add Flag Variant.
        /// Adds a variant identifier for an already existing flag.
        /// </summary>
        /// <param name="identifier">The flag's principal identifier. Intaken as a <see cref="string"/>.</param>
        /// <param name="variantIdentifier">The flag's variant identifier. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a bool indicating whether the variant was added successfully.</returns>
        public bool AddFlagVariant(string identifier, string variantIdentifier)
        {
            var result = false;
            IOCommandLineFlag flag;

            if ((flag = GetFlag(identifier)) != null)
            {
                Flags.Add(variantIdentifier, flag);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Get Flag.
        /// </summary>
        /// <param name="identifier">The flag's identifier.</param>
        /// <returns>Returns the requested flag or null, if not found.</returns>
        public IOCommandLineFlag GetFlag(string identifier)
        {
            IOCommandLineFlag result = null;

            if (Flags.ContainsKey(identifier))
            {
                result = Flags[identifier];
            }

            return result;
        }

        /// <summary>
        /// Remove Flag.
        /// </summary>
        /// <param name="identifier">The flag's principal identifier.</param>
        /// <returns>Returns a bool indicating whether the flag was removed.</returns>
        public bool RemoveFlag(string identifier)
        {
            var result = false;

            if (Flags.ContainsKey(identifier))
            {
                result = Flags.Remove(identifier);
            }

            return result;
        }

        /// <summary>
        /// Parse Flag.
        /// </summary>
        /// <param name="flag">The string to parse.</param>
        /// <returns>Returns a bool indicating whether the flag was parsed successfully.</returns>
        private bool ParseFlag(string flag)
        {
            var result = IOCommandLineFlag.Parse(flag);

            if (result.Success)
            {
                var identifier = result.Groups["flag"].Value;

                if (Flags.ContainsKey(identifier))
                {
                    Flags[identifier].IsPresent = true;
                }
            }

            return result.Success;
        }

        /// <summary>
        /// Add Option.
        /// </summary>
        /// <param name="identifier">The option's principal identifier.</param>
        /// <param name="description">A short description of the option's usage.</param>
        /// <param name="syntax">Details regarding how the option should be used/written.</param>
        /// <returns>Returns a bool indicating whether the option was added.</returns>
        public bool AddOption(string identifier, string description, string syntax)
        {
            var result = false;

            if (!Options.ContainsKey(identifier))
            {
                Options.Add(identifier, new IOCommandLineOption(identifier, description, syntax));
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Add Option Variant.
        /// Adds a variant identifier for an already existing option.
        /// </summary>
        /// <param name="identifier">The option's principal identifier. Intaken as a <see cref="string"/>.</param>
        /// <param name="variantIdentifier">The option's variant identifier. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a bool indicating whether the variant was added successfully.</returns>
        public bool AddOptionVariant(string identifier, string variantIdentifier)
        {
            var result = false;
            IOCommandLineOption option;

            if ((option = GetOption(identifier)) != null)
            {
                Options.Add(variantIdentifier, option);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Get Option.
        /// </summary>
        /// <param name="identifier">The option's identifier.</param>
        /// <returns>Returns the requested option or null, if not found.</returns>
        public IOCommandLineOption GetOption(string identifier)
        {
            IOCommandLineOption result = null;

            if (Options.ContainsKey(identifier))
            {
                result = Options[identifier];
            }

            return result;
        }

        /// <summary>
        /// Remove Option.
        /// </summary>
        /// <param name="identifier">The option's principal identifier.</param>
        /// <returns>Returns a bool indicating whether the option was removed.</returns>
        public bool RemoveOption(string identifier)
        {
            var result = false;

            if (Options.ContainsKey(identifier))
            {
                result = Options.Remove(identifier);
            }

            return result;
        }

        /// <summary>
        /// Parse Option.
        /// </summary>
        /// <param name="option">The string to parse.</param>
        /// <returns>Returns a bool indicating whether the option was parsed successfully.</returns>
        private bool ParseOption(string option)
        {
            var result = IOCommandLineOption.Parse(option);

            if (result.Success)
            {
                var identifier = result.Groups["name"].Value;
                var value = result.Groups["value"].Value;

                if (Options.ContainsKey(identifier))
                {
                    Options[identifier].AddValue(value);
                }
            }

            return result.Success;
        }

        /// <summary>
        /// Parse.
        /// </summary>
        /// <param name="input">The input string to parse.</param>
        public void Parse(string input)
        {
            if (input != null)
            {
                ParseArgument(input);
                ParseFlag(input);
                ParseOption(input);
            }
        }
    }
}