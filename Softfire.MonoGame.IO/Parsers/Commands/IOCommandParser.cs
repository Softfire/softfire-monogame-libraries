using System.Collections.Generic;

namespace Softfire.MonoGame.IO.Parsers.Commands
{
    /// <summary>
    /// A command parser class.
    /// </summary>
    public class IOCommandParser
    {
        /// <summary>
        /// Commands.
        /// </summary>
        private Dictionary<string, IOCommandCommand> Commands { get; }

        /// <summary>
        /// The current command.
        /// </summary>
        public IOCommandCommand CurrentCommand { get; private set; }

        /// <summary>
        /// A command parser.
        /// </summary>
        public IOCommandParser()
        {
            Commands = new Dictionary<string, IOCommandCommand>();
        }

        /// <summary>
        /// Add Command.
        /// </summary>
        /// <param name="identifier">The command's principal identifier.</param>
        /// <param name="description">A short description of the command's usage.</param>
        /// <param name="syntax">Details regarding how the command should be used/written.</param>
        /// <param name="numberOfRequiredVariables">The required number of variables required following the command.</param>
        /// <returns>Returns a bool indicating whether the command was added.</returns>
        public bool AddCommand(string identifier, string description, string syntax, int numberOfRequiredVariables)
        {
            var result = false;

            if (!Commands.ContainsKey(identifier))
            {
                Commands.Add(identifier, new IOCommandCommand(identifier, description, syntax, numberOfRequiredVariables));
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Add Command Variant.
        /// Adds a variant identifier for an already existing command.
        /// </summary>
        /// <param name="identifier">The command's principal identifier. Intaken as a <see cref="string"/>.</param>
        /// <param name="variantIdentifier">The command's variant identifier. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a bool indicating whether the variant was added successfully.</returns>
        public bool AddCommandVariant(string identifier, string variantIdentifier)
        {
            var result = false;
            IOCommandCommand command;

            if ((command = GetCommand(identifier)) != null)
            {
                Commands.Add(variantIdentifier, command);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Get Command.
        /// </summary>
        /// <param name="identifier">The command's identifier.</param>
        /// <returns>Returns the requested command or null, if not found.</returns>
        public IOCommandCommand GetCommand(string identifier)
        {
            IOCommandCommand result = null;

            if (Commands.ContainsKey(identifier))
            {
                result = Commands[identifier];
            }

            return result;
        }

        /// <summary>
        /// Remove Command.
        /// </summary>
        /// <param name="identifier">The command's principal identifier.</param>
        /// <returns>Returns a bool indicating whether the command was removed.</returns>
        public bool RemoveCommand(string identifier)
        {
            var result = false;

            if (Commands.ContainsKey(identifier))
            {
                result = Commands.Remove(identifier);
            }

            return result;
        }

        /// <summary>
        /// Parse.
        /// </summary>
        /// <param name="input">Input to parse.</param>
        /// <param name="isCaseSensitive">A bool indicating whether the input is case sensitive. Default is true.</param>
        /// <param name="toLower">A bool indicating that if input is case insensitive then input will be converted to lowercase if set to true or to uppercase if set to false.</param>
        /// <returns>Returns a bool indicating whether the input string contained the required amount of variables for the command.</returns>
        public void Parse(string input, bool isCaseSensitive = true, bool toLower = true)
        {
            // Clear previous command.
            CurrentCommand?.ClearValues();
            CurrentCommand = null;

            if (!string.IsNullOrWhiteSpace(input))
            {
                var result = IOCommandCommand.Parse(input);

                if (result.Success)
                {
                    var identifier = result.Groups["command"].Value;
                    var values = result.Groups["values"].Value.Split(' ');

                    if (!isCaseSensitive)
                    {
                        identifier = toLower ? identifier.ToLower() : identifier.ToUpper();
                    }

                    if ((CurrentCommand = GetCommand(identifier)) != null &
                        !IOCommandCommand.DisplayHelp)
                    {
                        foreach (var value in values)
                        {
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                CurrentCommand.AddValue(value);
                            }
                        }
                    }
                }
            }
        }
    }
}