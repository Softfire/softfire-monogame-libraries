using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Microsoft.Xna.Framework;
using Softfire.MonoGame.DB;
using Softfire.MonoGame.LOG;

namespace Softfire.MonoGame.NTWK.Lobby
{
    public class Lobby<T> where T : LobbyUser
    {
        /// <summary>
        /// Elapsed Time.
        /// </summary>
        private double ElapsedTime { get; set; }

        /// <summary>
        /// Delta Time.
        /// </summary>
        private double DeltaTime { get; set; }

        /// <summary>
        /// Logger.
        /// </summary>
        private Logger Logger { get; }

        /// <summary>
        /// Stopwatch Timer.
        /// For use when MonoGame GameTime is not being used in the Update Method.
        /// </summary>
        private Stopwatch Timer { get; }

        /// <summary>
        /// Previous Update Time In Milliseconds.
        /// For use when MonoGame GameTime is not being used in the Update Method.
        /// </summary>
        private double PreviousUpdateTimeInMilliseconds { get; set; }

        /// <summary>
        ///  Lobby Update Frequency In Seocnds.
        /// </summary>
        private const double LobbyUpdateFrequencyInSeconds = 30;

        /// <summary>
        ///  Lobby Name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Lobby Settings.
        /// </summary>
        public LobbySettings Settings { get; }
        
        /// <summary>
        ///  Lobby Rooms.
        /// </summary>
        public Dictionary<Guid, LobbyRoom<T>> Rooms { get; }

        /// <summary>
        /// Lobby Users.
        /// </summary>
        public Dictionary<Guid, T> Users { get; }

        /// <summary>
        ///  Lobby Banned Text List.
        /// </summary>
        public Dictionary<string, LobbyBanText> BannedText { get; }

        #region Lobby User Enums

        /// <summary>
        /// Lobby User Registration Results.
        /// </summary>
        public enum LobbyUserRegistrationResults : byte
        {
            Failure,
            Success,
            DeniedUserNull,
            DeniedFirstNameNullOrWhitespace,
            DeniedFirstNameTooShort,
            DeniedFirstNameTooLong,
            DeniedLastNameNullOrWhitespace,
            DeniedLastNameTooShort,
            DeniedLastNameTooLong,
            DeniedEmailAddressNullOrWhitespace,
            DeniedUserNameNullOrWhitespace,
            DeniedUserNameTooShort,
            DeniedUserNameTooLong,
            DeniedPasswordNullOrWhitespace,
            DeniedPasswordTooShort,
            DeniedPasswordTooLong,
            DeniedBannedFirstName,
            DeniedBannedLastName,
            DeniedBannedUsername,
            DeniedUsernameInUse,
            DeniedEmailAddressInUse
        }

        /// <summary>
        /// Lobby User Login Results.
        /// </summary>
        public enum LobbyUserLoginResults : byte
        {
            Failure,
            Success,
            Denied
        }

        /// <summary>
        /// Lobby User Add To Lobby Results.
        /// </summary>
        public enum LobbyUserAddToLobbyResults : byte
        {
            Failure,
            Success,
            DeniedUserNull,
            DeniedFirstNameNullOrWhitespace,
            DeniedLastNameNullOrWhitespace,
            DeniedEmailAddressNullOrWhitespace,
            DeniedUserNameNullOrWhitespace,
            DeniedBannedFirstName,
            DeniedBannedLastName,
            DeniedBannedUsername
        }

        #endregion

        #region Lobby Room Enums

        /// <summary>
        /// Lobby Room Action Results.
        /// </summary>
        public enum LobbyRoomRegistrationResults
        {
            Failure,
            Success,
            Denied
        }

        public enum LobbyRoomCreationResults
        {
            Failure,
            Success,
            DeniedUserNull,
            DeniedRoomNameNullOrWhitespace,
            DeniedAdminPasswordNullOrWhitespace,
            DeniedBannedRoomName
        }

        /// <summary>
        /// Lobby Room Removal Results.
        /// </summary>
        public enum LobbyRoomRemovalResults
        {
            Failure,
            Success,
            DeniedUserNull,
            DeniedRoomNull,
            DeniedRoomNotFound,
            DeniedInsufficientPermissionLevel
        }

        #endregion
        
        /// <summary>
        /// MySQL Database Connection.
        /// </summary>
        private MySqlDBConnect MySqlDBConnect { get; }

        /// <summary>
        /// Lobby Constructor.
        /// </summary>
        /// <param name="isHeadless">Indicates wether the lobby requires a graphics device and MonoGame. Intaken as a <see cref="bool"/>. Use Update() if true otherwise use Update(GameTime gametime).</param>
        /// <param name="name">The lobby name. Intaken as a <see cref="string"/>.</param>
        /// <param name="logFilePath">Intakes a file path for logs relative to the calling application.</param>
        public Lobby(bool isHeadless, string name, string logFilePath = @"Config\Logs\Lobby")
        {
            Settings = new LobbySettings();

            if (isHeadless)
            {
                Timer = new Stopwatch();
                Timer.Start();
            }

            //TODO: Inject DB info another way.
            MySqlDBConnect = new MySqlDBConnect("host", "username", "password", "database");

            Name = name;

            Users = new Dictionary<Guid, T>();
            Rooms = new Dictionary<Guid, LobbyRoom<T>>();
            BannedText = new Dictionary<string, LobbyBanText>();
            Logger = new Logger(logFilePath);
        }

        #region User Commands

        /// <summary>
        /// Generate Unique Lobby User Id.
        /// </summary>
        /// <returns>Returns a unique user id as a Guid.</returns>
        public Guid GenerateUniqueLobbyUserId()
        {
            var id = NetCommon.GenerateUniqueId();

            // Ensure there are no duplicate user ids.
            while (Users.ContainsKey(id))
            {
                id = NetCommon.GenerateUniqueId();
            }

            return id;
        }

        /// <summary>
        /// Lobby User Registration.
        /// </summary>
        /// <param name="user">A new user of Type T1.</param>
        /// <param name="password">New user's password.  Intaken as a <see cref="string"/>.</param>
        /// <param name="ipEndPoint">IPEndpoint of registering user.</param>
        /// <returns>Returns an enum of LobbyUserRegistrationResults indicating the result.</returns>
        public LobbyUserRegistrationResults LobbyUserRegistration(T user, string password, IPEndPoint ipEndPoint)
        {
            try
            {
                #region Check For Null Or Whitespace

                // Check User for null.
                if (user == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Lobby user registration failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(LobbyUserRegistration)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user)}{Environment.NewLine}" +
                                                          $"Message: User object was null.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRegistrationResults.DeniedUserNull;
                }

                // Check First Name for null or white space.
                if (string.IsNullOrWhiteSpace(user.FirstName))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"User registration request denied.{Environment.NewLine}" +
                                                          $"Source: {nameof(LobbyUserRegistration)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user.FirstName)}{Environment.NewLine}" +
                                                          $"Message: First Name was null or whitespace.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRegistrationResults.DeniedFirstNameNullOrWhitespace;
                }

                // Check Last Name for null or white space.
                if (string.IsNullOrWhiteSpace(user.LastName))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"User registration request denied.{Environment.NewLine}" +
                                                          $"Source: {nameof(LobbyUserRegistration)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user.LastName)}{Environment.NewLine}" +
                                                          $"Message: Last Name was null or whitespace.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRegistrationResults.DeniedLastNameNullOrWhitespace;
                }

                // Check Email Address for null or white space.
                if (string.IsNullOrWhiteSpace(user.EmailAddress))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"User registration request denied.{Environment.NewLine}" +
                                                          $"Source: {nameof(LobbyUserRegistration)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user.EmailAddress)}{Environment.NewLine}" +
                                                          $"Message: Email Address was null or whitespace.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRegistrationResults.DeniedEmailAddressNullOrWhitespace;
                }

                // Check User Name for null or white space.
                if (string.IsNullOrWhiteSpace(user.UserName))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"User registration request denied.{Environment.NewLine}" +
                                                          $"Source: {nameof(LobbyUserRegistration)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user.UserName)}{Environment.NewLine}" +
                                                          $"Message: Username was null or whitespace.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRegistrationResults.DeniedUserNameNullOrWhitespace;
                }

                // Check Password for null or white space.
                if (string.IsNullOrWhiteSpace(password))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"User registration request denied.{Environment.NewLine}" +
                                                          $"Source: {nameof(LobbyUserRegistration)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(password)}{Environment.NewLine}" +
                                                          $"Message: Password was null or whitespace.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRegistrationResults.DeniedPasswordNullOrWhitespace;
                }

                // Check IPEndPoint for null.
                if (ipEndPoint == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"User registration request denied.{Environment.NewLine}" +
                                                          $"Source: {nameof(LobbyUserRegistration)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(ipEndPoint)}{Environment.NewLine}" +
                                                          $"Message: IPEndPoint was null.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRegistrationResults.DeniedPasswordNullOrWhitespace;
                }

                #endregion

                #region Check For Less Than Minimum Length

                // Check First Name for less than minimum length.
                if (user.FirstName.Length < Settings.FirstNameMinimumLength)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"User registration request denied.{Environment.NewLine}" +
                                                          $"Source: {nameof(LobbyUserRegistration)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user.FirstName)}{Environment.NewLine}" +
                                                          $"Message: First Name ({user.FirstName}) was too short (<{Settings.FirstNameMinimumLength}).{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRegistrationResults.DeniedFirstNameTooShort;
                }

                // Check Last Name for less than minimum length.
                if (user.LastName.Length < Settings.LastNameMinimumLength)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Receiving of user data failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(LobbyUserRegistration)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user.LastName)}{Environment.NewLine}" +
                                                          $"Message: Last Name ({user.LastName}) was too short (<{Settings.LastNameMinimumLength}).{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRegistrationResults.DeniedLastNameTooShort;
                }
                
                // Check User Name for less than minimum length.
                if (user.UserName.Length < Settings.UserNameMinimumLength)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Receiving of user data failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(LobbyUserRegistration)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user.UserName)}{Environment.NewLine}" +
                                                          $"Message: User Name ({user.UserName}) was too short (<{Settings.UserNameMinimumLength}).{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRegistrationResults.DeniedUserNameTooShort;
                }

                // Check Password for less than minimum length.
                if (password.Length < Settings.PasswordMinimumLength)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Receiving of user data failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(LobbyUserRegistration)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(password)}{Environment.NewLine}" +
                                                          $"Message: Password was too short (<{Settings.PasswordMinimumLength}).{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRegistrationResults.DeniedPasswordTooShort;
                }
                
                #endregion

                #region Check For Greater Than Maximum Length
                
                // Check First Name for greater than maximum length.
                if (user.FirstName.Length > Settings.FirstNameMaximumLength)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"User registration request denied.{Environment.NewLine}" +
                                                          $"Source: {nameof(LobbyUserRegistration)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user.FirstName)}{Environment.NewLine}" +
                                                          $"Message: First Name ({user.FirstName}) was too long (>{Settings.FirstNameMaximumLength}).{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRegistrationResults.DeniedFirstNameTooLong;
                }

                // Check Last Name for greater than maximum length.
                if (user.LastName.Length > Settings.LastNameMaximumLength)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Receiving of user data failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(LobbyUserRegistration)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user.LastName)}{Environment.NewLine}" +
                                                          $"Message: Last Name ({user.LastName}) was too long (>{Settings.LastNameMaximumLength}).{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRegistrationResults.DeniedLastNameTooLong;
                }

                // Check User Name for greater than maximum length.
                if (user.UserName.Length > Settings.UserNameMaximumLength)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Receiving of user data failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(LobbyUserRegistration)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user.UserName)}{Environment.NewLine}" +
                                                          $"Message: User Name ({user.UserName}) was too long (>{Settings.UserNameMaximumLength}).{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRegistrationResults.DeniedUserNameTooLong;
                }

                // Check Password for greater than maximum length.
                if (password.Length > Settings.PasswordMaximumLength)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Receiving of user data failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(LobbyUserRegistration)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(password)}{Environment.NewLine}" +
                                                          $"Message: Password was too long (>{Settings.PasswordMaximumLength}).{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRegistrationResults.DeniedPasswordTooLong;
                }

                #endregion

                #region Check For Banned Text

                // Check First Name text for ban.
                if (TextBanExists(user.FirstName))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"User registration request denied.{Environment.NewLine}" +
                                                          $"Source: {nameof(LobbyUserRegistration)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user.FirstName)}{Environment.NewLine}" +
                                                          $"Message: First Name ({user.FirstName}) contained banned text.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRegistrationResults.DeniedBannedFirstName;
                }

                // Check Last Name text for ban.
                if (TextBanExists(user.LastName))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"User registration request denied.{Environment.NewLine}" +
                                                          $"Source: {nameof(LobbyUserRegistration)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user.LastName)}{Environment.NewLine}" +
                                                          $"Message: Last Name ({user.LastName}) contained banned text.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRegistrationResults.DeniedBannedLastName;
                }

                // Check User Name text for ban.
                if (TextBanExists(user.UserName))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"User registration request denied.{Environment.NewLine}" +
                                                          $"Source: {nameof(LobbyUserRegistration)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user.UserName)}{Environment.NewLine}" +
                                                          $"Message: User Name ({user.UserName}) contained banned text.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRegistrationResults.DeniedBannedUsername;
                }
                
                #endregion
                
                // Duplicate Username Check
                if (LobbyUserExistsByUserName(user.UserName))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"User registration request denied.{Environment.NewLine}" +
                                                          $"Source: {nameof(LobbyUserRegistration)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user.UserName)}{Environment.NewLine}" +
                                                          $"Message: User Name ({user.UserName}) is unavailable.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRegistrationResults.DeniedUsernameInUse;
                }

                // Duplicate Email Address Check
                if (LobbyUserExistsByEmail(user.EmailAddress))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"User registration request denied.{Environment.NewLine}" +
                                                          $"Source: {nameof(LobbyUserRegistration)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user.EmailAddress)}{Environment.NewLine}" +
                                                          $"Message: Email Address ({user.EmailAddress}) is currently unavailable.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRegistrationResults.DeniedEmailAddressInUse;
                }

                // Insert User into Database.
                if (InsertNewLobbyUser(user.Id, user.FirstName, user.LastName, user.EmailAddress, user.UserName, password) != 1)
                {
                    Logger.Write(LogTypes.Warning, $"User insertion into database failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(LobbyUserRegistration)} - {nameof(InsertNewLobbyUser)}{Environment.NewLine}" +
                                                          $"IP/Port: ({ipEndPoint.Address}:{ipEndPoint.Port}){Environment.NewLine}" +
                                                          $"Id: ({user.Id}){Environment.NewLine}{Environment.NewLine}" +
                                                          $"User: ({user.UserName}){Environment.NewLine}" +
                                                          $"First Name: ({user.FirstName}){Environment.NewLine}" +
                                                          $"Last Name: ({user.LastName}){Environment.NewLine}" +
                                                          $"Email: ({user.EmailAddress}){Environment.NewLine}",
                                                          useInlineLayout: false);

                    return LobbyUserRegistrationResults.Failure;
                }

                // Insert User into Lobby.
                if (LobbyUserAddToLobby(user) != LobbyUserAddToLobbyResults.Success)
                {
                    Logger.Write(LogTypes.Error, $"Lobby user creation failed.{Environment.NewLine}" +
                                                        $"Source: {nameof(LobbyUserRegistration)} - {nameof(LobbyUserAddToLobby)}{Environment.NewLine}" +
                                                        $"IP/Port: ({ipEndPoint.Address}:{ipEndPoint.Port}){Environment.NewLine}" +
                                                        $"Id: ({user.Id}){Environment.NewLine}{Environment.NewLine}" +
                                                        $"User: ({user.UserName}){Environment.NewLine}" +
                                                        $"First Name: ({user.FirstName}){Environment.NewLine}" +
                                                        $"Last Name: ({user.LastName}){Environment.NewLine}" +
                                                        $"Email: ({user.EmailAddress}){Environment.NewLine}",
                                                        useInlineLayout: false);

                    return LobbyUserRegistrationResults.Failure;
                }

                // Write to log.
                Logger.Write(LogTypes.Info, $"User registration request approved.{Environment.NewLine}" +
                                                   $"Source: {nameof(LobbyUserRegistration)}{Environment.NewLine}" +
                                                   $"IP/Port: ({ipEndPoint.Address}:{ipEndPoint.Port}){Environment.NewLine}" +
                                                   $"Id: ({user.Id}){Environment.NewLine}{Environment.NewLine}" +
                                                   $"User: ({user.UserName}){Environment.NewLine}" +
                                                   $"First Name: ({user.FirstName}){Environment.NewLine}" +
                                                   $"Last Name: ({user.LastName}){Environment.NewLine}" +
                                                   $"Email: ({user.EmailAddress}){Environment.NewLine}",
                                                   useInlineLayout: false);

                // Return success code.
                return LobbyUserRegistrationResults.Success;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                        $"Source: {nameof(LobbyUserRegistration)}{Environment.NewLine}" +
                                                        $"IP/Port: ({ipEndPoint?.Address}:{ipEndPoint?.Port}){Environment.NewLine}" +
                                                        $"Id: ({user?.Id}){Environment.NewLine}{Environment.NewLine}" +
                                                        $"User: ({user?.UserName}){Environment.NewLine}" +
                                                        $"First Name: ({user?.FirstName}){Environment.NewLine}" +
                                                        $"Last Name: ({user?.LastName}){Environment.NewLine}" +
                                                        $"Email: ({user?.EmailAddress}){Environment.NewLine}" +
                                                        $"Message: {ex}{Environment.NewLine}",
                                                        useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRegistrationResults.Failure;
                }
            }
            
            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                                $"Source: {nameof(LobbyUserRegistration)}{Environment.NewLine}" +
                                                $"IP/Port: ({ipEndPoint?.Address}:{ipEndPoint?.Port}){Environment.NewLine}" +
                                                $"Id: ({user?.Id}){Environment.NewLine}{Environment.NewLine}" +
                                                $"User: ({user?.UserName}){Environment.NewLine}" +
                                                $"First Name: ({user?.FirstName}){Environment.NewLine}" +
                                                $"Last Name: ({user?.LastName}){Environment.NewLine}" +
                                                $"Email: ({user?.EmailAddress}){Environment.NewLine}",
                                                useInlineLayout: false);

            // Return failure code.
            return LobbyUserRegistrationResults.Failure;
        }

        /// <summary>
        /// Lobby User Add To Lobby.
        /// </summary>
        /// <param name="user">A new user of Type T1.</param>
        /// <returns>Returns a bool indicating whether the user was created.</returns>
        private LobbyUserAddToLobbyResults LobbyUserAddToLobby(T user)
        {
            try
            {
                #region Check For Null Or Whitespace

                // Check User for null.
                if (user == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Lobby user creation failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(LobbyUserAddToLobby)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user)}{Environment.NewLine}" +
                                                          $"Message: User object was null.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserAddToLobbyResults.DeniedUserNull;
                }

                // Check First Name for null or white space.
                if (string.IsNullOrWhiteSpace(user.FirstName))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Lobby user creation failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(LobbyUserAddToLobby)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user.FirstName)}{Environment.NewLine}" +
                                                          $"Message: First Name was null or whitespace.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserAddToLobbyResults.DeniedFirstNameNullOrWhitespace;
                }

                // Check Last Name for null or white space.
                if (string.IsNullOrWhiteSpace(user.LastName))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Lobby user creation failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(LobbyUserAddToLobby)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user.LastName)}{Environment.NewLine}" +
                                                          $"Message: Last Name was null or whitespace.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserAddToLobbyResults.DeniedLastNameNullOrWhitespace;
                }

                // Check Email Address for null or white space.
                if (string.IsNullOrWhiteSpace(user.EmailAddress))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Lobby user creation failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(LobbyUserAddToLobby)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user.EmailAddress)}{Environment.NewLine}" +
                                                          $"Message: Email Address was null or whitespace.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserAddToLobbyResults.DeniedEmailAddressNullOrWhitespace;
                }

                // Check User Name for null or white space.
                if (string.IsNullOrWhiteSpace(user.UserName))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Lobby user creation failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(LobbyUserAddToLobby)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user.UserName)}{Environment.NewLine}" +
                                                          $"Message: Username was null or whitespace.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserAddToLobbyResults.DeniedUserNameNullOrWhitespace;
                }

                #endregion

                #region Check For Banned Text

                // Check First Name text for ban.
                if (TextBanExists(user.FirstName))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Lobby user creation failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(LobbyUserAddToLobby)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user.FirstName)}{Environment.NewLine}" +
                                                          $"Message: First Name ({user.FirstName}) contained banned text.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserAddToLobbyResults.DeniedBannedFirstName;
                }

                // Check Last Name text for ban.
                if (TextBanExists(user.LastName))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Lobby user creation failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(LobbyUserAddToLobby)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user.LastName)}{Environment.NewLine}" +
                                                          $"Message: Last Name ({user.LastName}) contained banned text.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserAddToLobbyResults.DeniedBannedLastName;
                }

                // Check User Name text for ban.
                if (!TextBanExists(user.UserName))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Lobby user creation failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(LobbyUserAddToLobby)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user.UserName)}{Environment.NewLine}" +
                                                          $"Message: Username ({user.UserName}) contained banned text.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserAddToLobbyResults.DeniedBannedUsername;
                }
                
                #endregion
                
                // Add New Lobby User.
                Users.Add(user.Id, user);

                // Write to log.
                Logger.Write(LogTypes.Info, $"Lobby user creation success.{Environment.NewLine}" +
                                                   $"Source: {nameof(LobbyUserAddToLobby)}{Environment.NewLine}" +
                                                   $"User: ({user.Id}){Environment.NewLine}" +
                                                   $"First Name: ({user.FirstName}){Environment.NewLine}" +
                                                   $"Last Name: ({user.LastName}){Environment.NewLine}" +
                                                   $"Email: ({user.EmailAddress}){Environment.NewLine}",
                                                   useInlineLayout: false);

                // Return success code.
                return LobbyUserAddToLobbyResults.Success;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                        $"Source: {nameof(LobbyUserAddToLobby)}{Environment.NewLine}" +
                                                        $"User: ({user?.Id}){Environment.NewLine}" +
                                                        $"First Name: ({user?.FirstName}){Environment.NewLine}" +
                                                        $"Last Name: ({user?.LastName}){Environment.NewLine}" +
                                                        $"Email: ({user?.EmailAddress}){Environment.NewLine}" +
                                                        $"Message: {ex}{Environment.NewLine}",
                                                        useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserAddToLobbyResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                                $"Source: {nameof(LobbyUserAddToLobby)}{Environment.NewLine}" +
                                                $"User: ({user?.Id}){Environment.NewLine}" +
                                                $"First Name: ({user?.FirstName}){Environment.NewLine}" +
                                                $"Last Name: ({user?.LastName}){Environment.NewLine}" +
                                                $"Email: ({user?.EmailAddress}){Environment.NewLine}",
                                                useInlineLayout: false);

            // Return failure code.
            return LobbyUserAddToLobbyResults.Failure;
        }
        
        #endregion

        #region Room Commands
        
        /// <summary>
        /// Generate Unique Lobby Room Id.
        /// </summary>
        /// <returns>Returns a unique room id as a Guid.</returns>
        public Guid GenerateUniqueLobbyRoomId()
        {
            var id = NetCommon.GenerateUniqueId();

            // Ensure there are no duplicate user ids.
            while (Rooms.ContainsKey(id))
            {
                id = NetCommon.GenerateUniqueId();
            }

            return id;
        }

        public LobbyRoomRegistrationResults LobbyRoomRegistration(T user, string roomName, string adminPassword, string accessPassword = null)
        {
            try
            {
                //TODO: Check user for null and proper permissions.
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                        $"Source: {nameof(LobbyRoomRegistration)}{Environment.NewLine}" +
                                                        $"Room Name: ({roomName}){Environment.NewLine}" +
                                                        $"Message: {ex}{Environment.NewLine}", useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomRegistrationResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                                $"Source: {nameof(LobbyRoomRegistration)}{Environment.NewLine}" +
                                                $"Room Name: ({roomName}){Environment.NewLine}", useInlineLayout: false);

            // Return failure code.
            return LobbyRoomRegistrationResults.Failure;
        }

        /// <summary>
        /// Create Room.
        /// </summary>
        /// <param name="user">The requesting user. intaken as a LobbyUser.</param>
        /// <param name="roomName">the room name. Displayed in the Lobby. Checked against the Banned Text List. Intaken as a <see cref="string"/>.</param>
        /// <param name="adminPassword">The admin password. Used to modify the room settings. Checked against the Banned Text List. Intaken as a <see cref="string"/>.</param>
        /// <param name="accessPassword">The access password. Used to access to room from the Lobby. Checked against the Banned Text List. Intaken as a <see cref="string"/>.</param>
        /// <returns></returns>
        public LobbyRoomCreationResults CreateRoom(T user, string roomName, string adminPassword, string accessPassword = null)
        {
            try
            {
                #region Check For Null Or Whitespace

                // Check User for null.
                if (user == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Lobby room creation failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(CreateRoom)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user)}{Environment.NewLine}" +
                                                          $"Message: User object was null.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomCreationResults.DeniedUserNull;
                }

                // Check Room Name for null or whitespace.
                if (string.IsNullOrWhiteSpace(roomName))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Lobby room creation failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(CreateRoom)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(roomName)}{Environment.NewLine}" +
                                                          $"Id: ({user.Id}){Environment.NewLine}" +
                                                          $"User: ({user.UserName}){Environment.NewLine}" +
                                                          $"Message: Room Name was null or whitespace.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomCreationResults.DeniedRoomNameNullOrWhitespace;
                }

                // Check Admin Password for null or whitespace.
                if (string.IsNullOrWhiteSpace(adminPassword))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Lobby room creation failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(CreateRoom)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(adminPassword)}{Environment.NewLine}" +
                                                          $"Id: ({user.Id}){Environment.NewLine}" +
                                                          $"User: ({user.UserName}){Environment.NewLine}" +
                                                          $"Message: Admin Password was null or whitespace.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomCreationResults.DeniedAdminPasswordNullOrWhitespace;
                }

                #endregion
                
                // Check Room Name for banned text.
                if (TextBanExists(roomName))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Lobby room creation failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(CreateRoom)} - {nameof(TextBanExists)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(roomName)}{Environment.NewLine}" +
                                                          $"Message: Room Name ({roomName}) contained banned text.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomCreationResults.DeniedBannedRoomName;
                }

                // Generate a unique id for the room.
                var id = GenerateUniqueLobbyRoomId();
                
                // Add new Room.
                Rooms.Add(id, new LobbyRoom<T>(id, roomName, user, adminPassword, accessPassword));

                // Write to log.
                Logger.Write(LogTypes.Info, $"Lobby room creation success.{Environment.NewLine}" +
                                                   $"Source: {nameof(CreateRoom)}{Environment.NewLine}" +
                                                   $"Id: ({user.Id}){Environment.NewLine}" +
                                                   $"User: ({user.UserName}){Environment.NewLine}" +
                                                   $"Room Name: ({roomName}){Environment.NewLine}",
                                                   useInlineLayout: false);

                // Return success code.
                return LobbyRoomCreationResults.Success;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                        $"Source: {nameof(CreateRoom)}{Environment.NewLine}" +
                                                        $"Id: ({user?.Id}){Environment.NewLine}" +
                                                        $"User: ({user?.UserName}){Environment.NewLine}" +
                                                        $"Message: {ex}{Environment.NewLine}",
                                                        useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomCreationResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                                $"Source: {nameof(CreateRoom)}{Environment.NewLine}" +
                                                $"Id: ({user?.Id}){Environment.NewLine}" +
                                                $"User: ({user?.UserName}){Environment.NewLine}" +
                                                $"Room Name: ({roomName}){Environment.NewLine}",
                                                useInlineLayout: false);

            // Return failure code.
            return LobbyRoomCreationResults.Failure;
        }

        /// <summary>
        /// Remove Room.
        /// </summary>
        /// <param name="user">The requesting user. intaken as a LobbyUser.</param>
        /// <param name="room">The room to remove. Intaken as a LobbyRoom.</param>
        public LobbyRoomRemovalResults RemoveRoom(T user, LobbyRoom<T> room)
        {
            try
            {
                #region Check For Null Or Whitespace

                // Check User for null.
                if (user == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Lobby room removal failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(RemoveRoom)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user)}{Environment.NewLine}" +
                                                          $"Message: User object was null.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomRemovalResults.DeniedUserNull;
                }

                // Check Room for null.
                if (room == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Lobby room removal failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(RemoveRoom)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(room)}{Environment.NewLine}" +
                                                          $"Id: ({user.Id}){Environment.NewLine}" +
                                                          $"User: ({user.UserName}){Environment.NewLine}" +
                                                          $"Message: Room object was null.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomRemovalResults.DeniedRoomNull;
                }

                #endregion

                // Check Permission Levels.
                if (!user.Equals(room.RoomOwner) ||
                    user.MembershipStatus < LobbyUser.Memberships.Staff)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Lobby room removal failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(RemoveRoom)}{Environment.NewLine}" +
                                                          $"Id: ({user.Id}){Environment.NewLine}" +
                                                          $"User: ({user.UserName}){Environment.NewLine}" +
                                                          $"Message: Membership Status is {user.MembershipStatus}. Status or Room Owner, {LobbyUser.Memberships.Staff.ToString()} or higher required.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomRemovalResults.DeniedInsufficientPermissionLevel;
                }

                // Check for Room.
                if (!Rooms.ContainsKey(room.Id))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Lobby room removal failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(RemoveRoom)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(room.Id)}{Environment.NewLine}" +
                                                          $"Id: ({user.Id}){Environment.NewLine}" +
                                                          $"User: ({user.UserName}){Environment.NewLine}" +
                                                          $"Message: Room was not found.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomRemovalResults.DeniedRoomNotFound;
                }

                //TODO: Backup Chat Logs. Move Users to Lobby.
                // Remove the room.
                Rooms.Remove(room.Id);

                // Write to log.
                Logger.Write(LogTypes.Info, $"Lobby room removal failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(RemoveRoom)}{Environment.NewLine}" +
                                                   $"Id: ({user.Id}){Environment.NewLine}" +
                                                   $"User: ({user.UserName}){Environment.NewLine}" +
                                                   $"Message: Requested Room ({room.Id + ":" + room.Name}) was removed.{Environment.NewLine}",
                                                   useInlineLayout: false);

                // Return success code.
                return LobbyRoomRemovalResults.Success;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                        $"Source: {nameof(RemoveRoom)}{Environment.NewLine}" +
                                                        $"Message: {ex}{Environment.NewLine}",
                                                        useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomRemovalResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                                $"Source: {nameof(RemoveRoom)}{Environment.NewLine}" +
                                                $"Id: ({user?.Id}){Environment.NewLine}" +
                                                $"User: ({user?.UserName}){Environment.NewLine}" +
                                                $"Room: ({room?.Id}:{room?.Name})",
                                                useInlineLayout: false);

            // Return failure code.
            return LobbyRoomRemovalResults.Failure;
        }

        #endregion

        #region Text Commands

        /// <summary>
        /// Add Text Ban.
        /// </summary>
        /// <param name="text">The text to ban. Intaken as a <see cref="string"/>.</param>
        /// <param name="reason">The reason for the ban. Intaken as a <see cref="string"/>.</param>
        /// <param name="dateTime">The date/time of the ban. Intaken as DateTime.</param>
        /// <param name="expiryDateTime">The expiry date/time of the ban. Intaken as DateTime.</param>
        /// <returns>Returns a bool indicating whether the text was added.</returns>
        public bool AddTextBan(string text, string reason, DateTime dateTime, DateTime expiryDateTime)
        {
            try
            {
                #region Check For Null Or Whitespace

                // Check Text for null or whitespace.
                if (string.IsNullOrWhiteSpace(text))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Text ban failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(AddTextBan)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(text)}{Environment.NewLine}" +
                                                          $"Message: Text was null or whitespace.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return false;
                }

                // Check Reason for null or whitespace.
                if (string.IsNullOrWhiteSpace(reason))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Text ban failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(AddTextBan)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(reason)}{Environment.NewLine}" +
                                                          $"Message: Reason was null or whitespace.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return false;
                }

                #endregion

                // Check Text for banned text.
                if (TextBanExists(text))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Text ban failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(AddTextBan)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(reason)}{Environment.NewLine}" +
                                                          $"Message: Text is already banned.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return false;
                }

                // Add Text ban.
                BannedText.Add(text, new LobbyBanText(text, reason, dateTime, expiryDateTime));

                // Write to log.
                Logger.Write(LogTypes.Info, $"Text ban succeeded.{Environment.NewLine}" +
                                                   $"Source: {nameof(AddTextBan)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(text)}{Environment.NewLine}" +
                                                   $"Message: Text was banned.{Environment.NewLine}",
                                                   useInlineLayout: false);

                // Return success code.
                return true;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Text ban failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(AddTextBan)}{Environment.NewLine}" +
                                                          $"Message: Text was null or whitespace.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return false;
                }
            }
            
            // Write to log.
            Logger.Write(LogTypes.Warning, $"Error! Something went wrong.{Environment.NewLine}" +
                                                  $"Source: {nameof(AddTextBan)}{Environment.NewLine}" +
                                                  $"Text: {text}{Environment.NewLine}" +
                                                  $"Reason: {reason}{Environment.NewLine}",
                                                  useInlineLayout: false);

            // Return failure code.
            return false;
        }

        /// <summary>
        /// Determines whether a text ban exists.
        /// </summary>
        /// <param name="text">The text to check. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a bool indicating whether the text is banned.</returns>
        public bool TextBanExists(string text)
        {
            try
            {
                // Check for Text ban and return code.
                return BannedText.ContainsKey(text);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                        $"Source: {nameof(TextBanExists)}{Environment.NewLine}" +
                                                        $"Message: {ex}{Environment.NewLine}",
                                                        useInlineLayout: false);

                    // Return failure code.
                    return false;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                                $"Source: {nameof(TextBanExists)}{Environment.NewLine}",
                                                useInlineLayout: false);

            // Return failure code.
            return false;
        }

        /// <summary>
        /// Remove Text Ban.
        /// </summary>
        /// <param name="text">The text to be unbanned. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a bool indicating whether the text was removed.</returns>
        public bool RemoveTextBan(string text)
        {
            try
            {
                // Check Text for ban.
                if (!TextBanExists(text))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Text ban removal failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(RemoveTextBan)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(text)}{Environment.NewLine}" +
                                                          $"Text: {text}{Environment.NewLine}" +
                                                          $"Message: Text was not found.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return false;
                }

                // Remove Text ban.
                BannedText.Remove(text);

                // Write to log.
                Logger.Write(LogTypes.Info, $"Text ban removal succeeded.{Environment.NewLine}" +
                                                   $"Source: {nameof(RemoveTextBan)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(text)}{Environment.NewLine}" +
                                                   $"Text: {text}{Environment.NewLine}" +
                                                   $"Message: Text was unbanned.{Environment.NewLine}",
                                                   useInlineLayout: false);

                // Return success code.
                return true;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                        $"Source: {nameof(RemoveTextBan)}{Environment.NewLine}" +
                                                        $"Message: {ex}{Environment.NewLine}",
                                                        useInlineLayout: false);

                    // Return failure code.
                    return false;
                }
            }
            
            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                                $"Source: {nameof(RemoveTextBan)}{Environment.NewLine}",
                                                useInlineLayout: false);

            // Return failure code.
            return false;
        }

        #endregion

        #region Database Commands

        /// <summary>
        /// Determines whether the lobby user exists, by id.
        /// </summary>
        /// <param name="id">Guid to check for in the database.</param>
        /// <returns>Returns a bool indicating whether the Guid exists in an account already.</returns>
        public bool LobbyUserExists(Guid id)
        {
            using (MySqlDBConnect.Connection)
            {
                using (var command = MySqlDBConnect.Connection.CreateCommand())
                {
                    command.CommandText = "SELECT Count(*) FROM Users WHERE id=?id";
                    command.Parameters.AddWithValue("?id", id);

                    MySqlDBConnect.OpenConnection();

                    return Convert.ToInt32(command.ExecuteScalar()) > 0;
                }
            }
        }

        /// <summary>
        /// Determines whether the lobby user exists, by user name.
        /// </summary>
        /// <param name="userName">User name to check for in the database. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a bool indicating whether the user name exists in an account already.</returns>
        public bool LobbyUserExistsByUserName(string userName)
        {
            using (MySqlDBConnect.Connection)
            {
                using (var command = MySqlDBConnect.Connection.CreateCommand())
                {
                    command.CommandText = "SELECT Count(*) FROM Users WHERE user_name=?user_name";
                    command.Parameters.AddWithValue("?user_name", userName);

                    MySqlDBConnect.OpenConnection();

                    return Convert.ToInt32(command.ExecuteScalar()) > 0;
                }
            }
        }

        /// <summary>
        /// Determines whether the lobby user exists, by email.
        /// </summary>
        /// <param name="emailAddress">Email address to check for in the database. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a bool indicating whether the email address exists in an account already.</returns>
        public bool LobbyUserExistsByEmail(string emailAddress)
        {
            using (MySqlDBConnect.Connection)
            {
                using (var checkForDuplicateEmail = MySqlDBConnect.Connection.CreateCommand())
                {
                    checkForDuplicateEmail.CommandText = "SELECT Count(*) FROM Users WHERE email_address=?email_address";
                    checkForDuplicateEmail.Parameters.AddWithValue("?email_address", emailAddress);

                    MySqlDBConnect.OpenConnection();

                    return Convert.ToInt32(checkForDuplicateEmail.ExecuteScalar()) > 0;
                }
            }
        }

        /// <summary>
        /// Insert New Lobby User.
        /// </summary>
        /// <param name="id">Unique Guid id.</param>
        /// <param name="firstName">Registering user's first name as a string.</param>
        /// <param name="lastName">Registering user's last name as a string.</param>
        /// <param name="emailAddress">Registering user's email address as a string.</param>
        /// <param name="userName">Registering user's username as a string.</param>
        /// <param name="passwordHash">Registering user's password as a string.</param>
        /// <returns>Returns the number of rows affected by the insert.</returns>
        public int InsertNewLobbyUser(Guid id, string firstName, string lastName, string emailAddress, string userName, string passwordHash)
        {
            using (MySqlDBConnect.Connection)
            {
                if (!LobbyUserExists(id))
                {
                    using (var command = MySqlDBConnect.Connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO Users (id, first_name, last_name, user_name, user_pass_hash, screen_name, email_address)" +
                                              "VALUES (?id, ?first_name, ?last_name, ?user_name, ?user_pass_hash, ?screen_name, ?email_address)";
                        command.Parameters.AddWithValue("?id", id);
                        command.Parameters.AddWithValue("?first_name", firstName);
                        command.Parameters.AddWithValue("?last_name", lastName);
                        command.Parameters.AddWithValue("?user_name", userName);
                        command.Parameters.AddWithValue("?user_pass_hash", passwordHash);
                        command.Parameters.AddWithValue("?screen_name", userName);
                        command.Parameters.AddWithValue("?email_address", emailAddress);

                        MySqlDBConnect.OpenConnection();

                        return command.ExecuteNonQuery();
                    }
                }

                return -1;
            }
        }

        /// <summary>
        /// Insert New Lobby Room.
        /// </summary>
        /// <param name="id">Unique Guid id.</param>
        /// <param name="roomName"></param>
        /// <param name="ownerId"></param>
        /// <returns>Returns the number of rows affected by the insert.</returns>
        public int InsertNewLobbyRoom(Guid id, string roomName, Guid ownerId)
        {
            using (MySqlDBConnect.Connection)
            {
                if (!LobbyUserExists(id))
                {
                    using (var command = MySqlDBConnect.Connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO Rooms (id, room_name, owner_id)" +
                                              "VALUES (?id, ?room_name, ?owner_id)";
                        command.Parameters.AddWithValue("?id", id);
                        command.Parameters.AddWithValue("?room_name", roomName);
                        command.Parameters.AddWithValue("?owner_id", ownerId);

                        MySqlDBConnect.OpenConnection();

                        return command.ExecuteNonQuery();
                    }
                }

                return -1;
            }
        }

        /// <summary>
        /// Select Lobby User By Id.
        /// </summary>
        /// <param name="id">Unique Guid id.</param>
        /// <returns>Returns the requested LobbyUser otherwise null.</returns>
        public T SelectLobbyUserById(Guid id)
        {
            using (MySqlDBConnect.Connection)
            {
                using (var command = MySqlDBConnect.Connection.CreateCommand())
                {
                    command.CommandText = "SELECT id, first_name, last_name, user_name, screen_name, membership_status, email_address, karma, coins, strikes, last_logon_datetime, ban_datetime, ban_end_datetime, ban_reason" +
                                          "FROM Users WHERE id=?id";
                    command.Parameters.AddWithValue("?id", id);

                    MySqlDBConnect.OpenConnection();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return (T)new LobbyUser(reader.GetGuid("id"), reader.GetString("first_name"), reader.GetString("last_name"), reader.GetString("email_address"), reader.GetString("user_name"))
                            {
                                ScreenName = reader.GetString("screen_name"),
                                Karma = reader.GetDouble("karma"),
                                Coins = reader.GetDouble("coins"),
                                Strikes = reader.GetInt32("strikes"),
                                LastLogonDateTime = reader.GetDateTime("last_logon_datetime"),
                                BanDateTime = reader.GetDateTime("ban_datetime"),
                                BanEndDateTime = reader.GetDateTime("ban_end_datetime"),
                                BanReason = reader.GetString("ban_reason")
                            };
                        }

                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Select Lobby User By Email.
        /// </summary>
        /// <param name="emailAddress">Email address to search for in the database.</param>
        /// <returns>Returns the requested LobbyUser otherwise null.</returns>
        public T SelectLobbyUserByEmail(string emailAddress)
        {
            using (MySqlDBConnect.Connection)
            {
                using (var command = MySqlDBConnect.Connection.CreateCommand())
                {
                    command.CommandText = "SELECT id, first_name, last_name, user_name, screen_name, membership_status, email_address, karma, coins, strikes, last_logon_datetime, ban_datetime, ban_end_datetime, ban_reason" +
                                          "FROM Users WHERE email_address=?email_address";
                    command.Parameters.AddWithValue("?email_address", emailAddress);

                    MySqlDBConnect.OpenConnection();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return (T)new LobbyUser(reader.GetGuid("id"), reader.GetString("first_name"), reader.GetString("last_name"), reader.GetString("email_address"), reader.GetString("user_name"))
                            {
                                ScreenName = reader.GetString("screen_name"),
                                Karma = reader.GetDouble("karma"),
                                Coins = reader.GetDouble("coins"),
                                Strikes = reader.GetInt32("strikes"),
                                LastLogonDateTime = reader.GetDateTime("last_logon_datetime"),
                                BanDateTime = reader.GetDateTime("ban_datetime"),
                                BanEndDateTime = reader.GetDateTime("ban_end_datetime"),
                                BanReason = reader.GetString("ban_reason")
                            };
                        }

                        return null;
                    }
                }
            }

        }

        #endregion

        /// <summary>
        ///  Lobby Update Method.
        /// </summary>
        /// <param name="gameTime">MonoGame GameTime.</param>
        public void Update(GameTime gameTime = null)
        {
            if (gameTime == null)
            {
                PreviousUpdateTimeInMilliseconds = DeltaTime;
                ElapsedTime += DeltaTime = Timer.ElapsedMilliseconds - PreviousUpdateTimeInMilliseconds;
            }
            else
            {
                ElapsedTime += DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;
            }
            

            if (ElapsedTime >= LobbyUpdateFrequencyInSeconds)
            {
                ElapsedTime = 0;
            }
        }
    }
}