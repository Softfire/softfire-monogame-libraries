using System;
using System.Collections.Generic;
using Softfire.MonoGame.LOG;

namespace Softfire.MonoGame.NTWK.Lobby
{
    public class LobbyUser : IComparable<LobbyUser>
    {
        /// <summary>
        /// Logger.
        /// </summary>
        private Logger Logger { get; }

        /// <summary>
        /// Uinque Id.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Internal First Name Field.
        /// </summary>
        private string _firstName;

        /// <summary>
        /// Internal Last Name Field.
        /// </summary>
        private string _lastName;

        /// <summary>
        /// Internal Email Address Field.
        /// </summary>
        private string _emailAddress;

        /// <summary>
        /// Internal User Name Field.
        /// </summary>
        private string _userName;

        /// <summary>
        /// Internal Screen Name Field.
        /// </summary>
        private string _screenName;

        /// <summary>
        /// First Name.
        /// </summary>
        public string FirstName
        {
            get => _firstName;
            set => _firstName = string.IsNullOrWhiteSpace(value) == false ? value : "INVALID_FIRST_NAME";
        }

        /// <summary>
        /// Last Name.
        /// </summary>
        public string LastName
        {
            get => _lastName;
            set => _lastName = string.IsNullOrWhiteSpace(value) == false ? value : "INVALID_LAST_NAME";
        }

        /// <summary>
        /// Email Address.
        /// </summary>
        public string EmailAddress
        {
            get => _emailAddress;
            set => _emailAddress = string.IsNullOrWhiteSpace(value) == false ? value : "INVALID_EMAIL_ADDRESS";
        }

        /// <summary>
        /// User Name.
        /// </summary>
        public string UserName
        {
            get => _userName;
            set => _userName = string.IsNullOrWhiteSpace(value) == false ? value : "INVALID_USERNAME";
        }

        /// <summary>
        /// Screen Name.
        /// </summary>
        public string ScreenName
        {
            get => _screenName;
            set => _screenName = string.IsNullOrWhiteSpace(value) == false ? value : UserName;
        }

        /// <summary>
        /// Karma.
        /// </summary>
        public double Karma { get; internal set; }

        /// <summary>
        /// Coins.
        /// </summary>
        public double Coins { get; internal set; }

        /// <summary>
        /// Strikes.
        /// </summary>
        public int Strikes { get; internal set; }

        /// <summary>
        /// Last Logon Date/Time.
        /// </summary>
        public DateTime LastLogonDateTime { get; internal set; }

        /// <summary>
        /// Ban Date/Time.
        /// </summary>
        public DateTime BanDateTime { get; internal set; }

        /// <summary>
        /// Ban End Date/Time.
        /// </summary>
        public DateTime BanEndDateTime { get; internal set; }

        /// <summary>
        /// Ban Reason.
        /// </summary>
        public string BanReason { get; internal set; }

        /// <summary>
        /// Lobby User Add Friend Results.
        /// </summary>
        public enum LobbyUserAddFriendResults
        {
            Failure,
            Success,
            DeniedUserNull,
            DeniedUserAlreadyOnFriendsList
        }

        /// <summary>
        /// Lobby User Remove Friend Results.
        /// </summary>
        public enum LobbyUserRemoveFriendResults
        {
            Failure,
            Success,
            DeniedUserNull,
            DeniedUserNotOnFriendsList
        }

        /// <summary>
        /// Lobby User Add Karma Results.
        /// </summary>
        public enum LobbyUserAddKarmaResults
        {
            Failure,
            Success,
            DeniedUserNull,
            DeniedAmountLessThanOrEqualToZero,
            DeniedMembershipStatusNotHighEnough
        }

        /// <summary>
        /// Lobby User Remove Karma Results.
        /// </summary>
        public enum LobbyUserRemoveKarmaResults
        {
            Failure,
            Success,
            DeniedUserNull,
            DeniedReasonNullOrWhitespace,
            DeniedAmountLessThanOrEqualToZero,
            DeniedMembershipStatusNotHighEnough
        }

        /// <summary>
        /// Lobby User Add Coin Results.
        /// </summary>
        public enum LobbyUserAddCoinResults
        {
            Failure,
            Success,
            DeniedUserNull,
            DeniedAmountLessThanOrEqualToZero,
            DeniedMembershipStatusNotHighEnough
        }

        /// <summary>
        /// Lobby User Gifte Coin Results.
        /// </summary>
        public enum LobbyUserGiftCoinResults
        {
            Failure,
            Success,
            DeniedUserNull,
            DeniedAmountLessThanOrEqualToZero,
            DeniedMembershipStatusNotHighEnough,
            DeniedAmountGreaterThanWhatIsAvailable
        }
        
        /// <summary>
        /// Lobby User Remove Coin Results.
        /// </summary>
        public enum LobbyUserRemoveCoinResults
        {
            Failure,
            Success,
            DeniedUserNull,
            DeniedReasonNullOrWhitespace,
            DeniedAmountLessThanOrEqualToZero,
            DeniedMembershipStatusNotHighEnough
        }

        /// <summary>
        /// LobbyUser Add Strike Results.
        /// </summary>
        public enum LobbyUserAddStrikeResults
        {
            Failure,
            Success,
            DeniedUserNull,
            DeniedReasonNullOrWhitespace,
            DeniedMembershipStatusNotHighEnough
        }

        /// <summary>
        /// LobbyUser Remove Strike Results.
        /// </summary>
        public enum LobbyUserRemoveStrikeResults
        {
            Failure,
            Success,
            DeniedUserNull,
            DeniedReasonNullOrWhitespace,
            DeniedMembershipStatusNotHighEnough
        }

        /// <summary>
        /// LobbyUser Add Ban Results.
        /// </summary>
        public enum LobbyUserAddBanResults
        {
            Failure,
            Success,
            DeniedUserNull,
            DeniedReasonNullOrWhitespace,
            DeniedEndDateTimeLessThanOrEqualToDateTimeNow,
            DeniedMembershipStatusNotHighEnough
        }

        /// <summary>
        /// LobbyUser Remove Ban Results.
        /// </summary>
        public enum LobbyUserRemoveBanResults
        {
            Failure,
            Success,
            DeniedUserNull,
            DeniedReasonNullOrWhitespace,
            DeniedMembershipStatusNotHighEnough
        }

        /// <summary>
        /// Friends List.
        /// </summary>
        private List<LobbyUser> FriendsList { get; }

        /// <summary>
        /// Action Log.
        /// </summary>
        public List<Tuple<DateTime, LogType, ActionType, string>> ActionLog { get; }

        /// <summary>
        /// Log Types.
        /// </summary>
        public enum LogType
        {
            Info,
            StaffAction,
            Warning,
            Effect,
            Error
        }

        /// <summary>
        /// Action Types.
        /// </summary>
        public enum ActionType
        {
            AddFriend,
            RemoveFriend,
            AddKarma,
            RemoveKarma,
            AddCoin,
            GiftCoin,
            RemoveCoin,
            AddStrike,
            RemoveStrike,
            AddBan,
            RemoveBan
        }

        /// <summary>
        /// Membership Status.
        /// </summary>
        public Memberships MembershipStatus { get; internal set; }

        /// <summary>
        /// Memberships.
        /// </summary>
        public enum Memberships : byte
        {
            Banned,
            Guest,
            Trial,
            Member,
            Karmic,
            Moderator,
            Staff,
            Admin
        }

        /// <summary>
        /// Lobby User.
        /// New user Constructor.
        /// </summary>
        /// <param name="id">A unique id. Intaken as a Guid.</param>
        /// <param name="firstName">User's first name as a string.</param>
        /// <param name="lastName">User's last name as a string.</param>
        /// <param name="emailAddress">User's email address as a string.</param>
        /// <param name="userName">User's username as a string. Must be unique on server.</param>
        /// <param name="logFilePath">Intakes a file path for logs relative to the calling application.</param>
        public LobbyUser(Guid id, string firstName, string lastName, string emailAddress, string userName, string logFilePath = @"Config\Logs\Lobby\Users")
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            UserName = userName;
            ScreenName = userName;

            MembershipStatus = Memberships.Guest;

            Karma = 0;
            Coins = 0;
            Strikes = 0;

            FriendsList = new List<LobbyUser>();
            ActionLog = new List<Tuple<DateTime, LogType, ActionType, string>>();
            Logger = new Logger(logFilePath + $@"\{Id}");
        }

        #region User Commands

        /// <summary>
        /// Add Friend.
        /// </summary>
        /// <typeparam name="T">An object of Type LobbyUser.</typeparam>
        /// <param name="user">The user to add to the friends list.</param>
        /// <returns>Returns an enum of LobbyUserAddFriendResults.</returns>
        public LobbyUserAddFriendResults AddFriend<T>(T user) where T : LobbyUser
        {
            try
            {
                #region Checks

                // Check User for null.
                if (user == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Adding of friend failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(AddFriend)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user)}{Environment.NewLine}" +
                                                          $"Message: User object was null.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserAddFriendResults.DeniedUserNull;
                }

                // Check if user is already on list.
                if (FriendsList.Contains(user))
                {
                    // Add to action log.
                    ActionLog.Add(new Tuple<DateTime, LogType, ActionType, string>(DateTime.Now, LogType.Warning, ActionType.AddFriend, $"({user.ScreenName}) is already a friend."));

                    // Record to log.
                    Logger.Record(LogTypes.Info, $"Adding of friend failed.{Environment.NewLine}" +
                                                        $"Source: {nameof(AddFriend)}{Environment.NewLine}" +
                                                        $"Variables: {nameof(user)}{Environment.NewLine}" +
                                                        $"Message: User ({user.ScreenName}) is already on friends list.{Environment.NewLine}",
                                                        useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserAddFriendResults.DeniedUserAlreadyOnFriendsList;
                }
                
                #endregion
                
                // Add user to friend list.
                FriendsList.Add(user);

                // Add to action log.
                ActionLog.Add(new Tuple<DateTime, LogType, ActionType, string>(DateTime.Now, LogType.Info, ActionType.AddFriend, $"({user.ScreenName}) is now a friend."));

                // Write to log.
                Logger.Record(LogTypes.Info, $"Adding of friend succeeded.{Environment.NewLine}" +
                                                    $"Source: {nameof(AddFriend)}{Environment.NewLine}" +
                                                    $"User: ({user.ScreenName}){Environment.NewLine}",
                                                    useInlineLayout: false);

                // Return success code.
                return LobbyUserAddFriendResults.Success;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                        $"Source: {nameof(AddFriend)}{Environment.NewLine}" +
                                                        $"User: ({user?.Id}){Environment.NewLine}" +
                                                        $"Message: {ex}{Environment.NewLine}",
                                                        useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserAddFriendResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                                $"Source: {nameof(AddFriend)}{Environment.NewLine}" +
                                                $"User: ({user?.Id}){Environment.NewLine}",
                                                useInlineLayout: false);

            // Return failure code.
            return LobbyUserAddFriendResults.Failure;
        }

        /// <summary>
        /// Remove Friend.
        /// </summary>
        /// <typeparam name="T">An object of Type LobbyUser.</typeparam>
        /// <param name="user">The user to remove from the friends list.</param>
        /// <returns>Returns an enum of LobbyUserRemoveFriendResults.</returns>
        public LobbyUserRemoveFriendResults RemoveFriend<T>(T user) where T : LobbyUser
        {
            try
            {
                #region Checks

                // Check User for null.
                if (user == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Removal of friend failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(RemoveFriend)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user)}{Environment.NewLine}" +
                                                          $"Message: User object was null.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRemoveFriendResults.DeniedUserNull;
                }

                // Check if user is on list.
                if (FriendsList.Contains(user) == false)
                {
                    // Add to action log.
                    ActionLog.Add(new Tuple<DateTime, LogType, ActionType, string>(DateTime.Now, LogType.Warning, ActionType.RemoveFriend, $"({user.ScreenName}) is not on your friends list."));

                    // Record to log.
                    Logger.Record(LogTypes.Info, $"Remove friend failed.{Environment.NewLine}" +
                                                        $"Source: {nameof(RemoveFriend)}{Environment.NewLine}" +
                                                        $"Variables: {nameof(user)}{Environment.NewLine}" +
                                                        $"Message: User ({user.ScreenName}) is not on friends list.{Environment.NewLine}",
                                                        useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRemoveFriendResults.DeniedUserNotOnFriendsList;
                }

                #endregion

                // Remove user from friend list.
                FriendsList.Remove(user);

                // Add to action log.
                ActionLog.Add(new Tuple<DateTime, LogType, ActionType, string>(DateTime.Now, LogType.Info, ActionType.RemoveFriend, $"({user.ScreenName}) is no longer on your friends list."));

                // Write to log.
                Logger.Record(LogTypes.Info, $"Friend removed successfully.{Environment.NewLine}" +
                                                    $"Source: {nameof(RemoveFriend)}{Environment.NewLine}" +
                                                    $"User: ({user.ScreenName}){Environment.NewLine}",
                                                    useInlineLayout: false);

                // Return success code.
                return LobbyUserRemoveFriendResults.Success;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                        $"Source: {nameof(RemoveFriend)}{Environment.NewLine}" +
                                                        $"User: ({user?.Id}){Environment.NewLine}" +
                                                        $"Message: {ex}{Environment.NewLine}",
                                                        useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRemoveFriendResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                                $"Source: {nameof(RemoveFriend)}{Environment.NewLine}" +
                                                $"User: ({user?.Id}){Environment.NewLine}",
                                                useInlineLayout: false);

            // Return failure code.
            return LobbyUserRemoveFriendResults.Failure;
        }

        /// <summary>
        /// Add Karma.
        /// </summary>
        /// <typeparam name="T">An object of Type LobbyUser.</typeparam>
        /// <param name="user">The user receiving the induction of karma.</param>
        /// <param name="amount">The amount in which to increase the user's karma by.</param>
        /// <returns>Returns an enum of LobbyUserAddKarmaResults.</returns>
        public LobbyUserAddKarmaResults AddKarma<T>(T user, double amount = 1) where T : LobbyUser
        {
            try
            {
                #region Checks

                // Check User for null.
                if (user == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Adding of Karma failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(AddKarma)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user)}{Environment.NewLine}" +
                                                          $"Message: User object was null.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserAddKarmaResults.DeniedUserNull;
                }

                // Check if less than or equal to 0.
                if (amount <= 0)
                {
                    // Record to log.
                    Logger.Record(LogTypes.Warning, $"Adding of Karma failed.{Environment.NewLine}" +
                                                           $"Source: {nameof(AddKarma)}{Environment.NewLine}" +
                                                           $"Variables: {nameof(amount)}{Environment.NewLine}" +
                                                           $"Message: Amount was less than or equal to zero.{Environment.NewLine}",
                                                           useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserAddKarmaResults.DeniedAmountLessThanOrEqualToZero;
                }

                // Check membership level.
                if (MembershipStatus >= Memberships.Member)
                {
                    // Record to log.
                    Logger.Record(LogTypes.Warning, $"Adding of Karma failed.{Environment.NewLine}" +
                                                           $"Source: {nameof(AddKarma)}{Environment.NewLine}" +
                                                           $"Variables: {nameof(MembershipStatus)}{Environment.NewLine}" +
                                                           $"Message: Membership status is not {nameof(Memberships.Member)} or higher.{Environment.NewLine}",
                                                           useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserAddKarmaResults.DeniedMembershipStatusNotHighEnough;
                }

                #endregion

                // Add Karma.
                user.Karma += amount;

                // Log Action.
                ActionLog.Add(new Tuple<DateTime, LogType, ActionType, string>(DateTime.Now, LogType.Info, ActionType.AddKarma, $"Gave karma to {user.ScreenName}. +{amount}"));

                // Record to log.
                Logger.Record(LogTypes.Info, $"Karma added successfully.{Environment.NewLine}" +
                                                    $"Source: {nameof(AddKarma)}{Environment.NewLine}" +
                                                    $"User: ({user.ScreenName}){Environment.NewLine}" +
                                                    $"Amount: ({amount}+){Environment.NewLine}",
                                                    useInlineLayout: false);

                // Return success code.
                return LobbyUserAddKarmaResults.Success;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                        $"Source: {nameof(AddKarma)}{Environment.NewLine}" +
                                                        $"User: ({user?.Id}){Environment.NewLine}" +
                                                        $"Message: {ex}{Environment.NewLine}",
                                                        useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserAddKarmaResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                                $"Source: {nameof(AddKarma)}{Environment.NewLine}" +
                                                $"User: ({user?.Id}){Environment.NewLine}",
                                                useInlineLayout: false);

            // Return failure code.
            return LobbyUserAddKarmaResults.Failure;
        }

        /// <summary>
        /// Remove Karma.
        /// </summary>
        /// <typeparam name="T">An object of Type LobbyUser.</typeparam>
        /// <param name="user">The user to receiving the reducton in karma.</param>
        /// <param name="reason">The reason for the reduction in karma. Intaken as a string.</param>
        /// <param name="amount">The amount in which to reduce the user's karma by.</param>
        /// <returns>Returns an enum of LobbyUserRemoveKarmaResults.</returns>
        public LobbyUserRemoveKarmaResults RemoveKarma<T>(T user, string reason, double amount = 1) where T : LobbyUser
        {
            try
            {
                #region Checks

                // Check User for null.
                if (user == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Removal of Karma failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(RemoveKarma)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user)}{Environment.NewLine}" +
                                                          $"Message: User object was null.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRemoveKarmaResults.DeniedUserNull;
                }

                // Check reason for null.
                if (string.IsNullOrWhiteSpace(reason))
                {
                    // Record to log.
                    Logger.Record(LogTypes.Warning, $"Removal of Karma failed.{Environment.NewLine}" +
                                                           $"Source: {nameof(RemoveKarma)}{Environment.NewLine}" +
                                                           $"Variables: {nameof(reason)}{Environment.NewLine}" +
                                                           $"Message: Reason was null or whitespace.{Environment.NewLine}",
                                                           useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRemoveKarmaResults.DeniedReasonNullOrWhitespace;
                }

                // Check if less than or equal to 0.
                if (amount <= 0)
                {
                    // Record to log.
                    Logger.Record(LogTypes.Warning, $"Removal of Karma failed.{Environment.NewLine}" +
                                                           $"Source: {nameof(RemoveKarma)}{Environment.NewLine}" +
                                                           $"Variables: {nameof(amount)}{Environment.NewLine}" +
                                                           $"Message: Amount was less than or equal to zero.{Environment.NewLine}",
                                                           useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRemoveKarmaResults.DeniedAmountLessThanOrEqualToZero;
                }

                // Check membership level.
                if (MembershipStatus >= Memberships.Member)
                {
                    // Record to log.
                    Logger.Record(LogTypes.Warning, $"Removal of Karma failed.{Environment.NewLine}" +
                                                           $"Source: {nameof(RemoveKarma)}{Environment.NewLine}" +
                                                           $"Variables: {nameof(MembershipStatus)}{Environment.NewLine}" +
                                                           $"Message: Membership status is not {nameof(Memberships.Member)} or higher.{Environment.NewLine}",
                                                           useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRemoveKarmaResults.DeniedMembershipStatusNotHighEnough;
                }

                #endregion

                // Remove Karma.
                user.Karma -= amount;

                // Log Action.
                ActionLog.Add(new Tuple<DateTime, LogType, ActionType, string>(DateTime.Now, LogType.Info, ActionType.RemoveKarma, $"Removed karma from {user.ScreenName}. -{amount} Reason: {reason}"));

                // Record to log.
                Logger.Record(LogTypes.Info, $"Karma removed successfully.{Environment.NewLine}" +
                                                    $"Source: {nameof(RemoveKarma)}{Environment.NewLine}" +
                                                    $"User: ({user.ScreenName}){Environment.NewLine}" +
                                                    $"Amount: ({amount}){Environment.NewLine}" +
                                                    $"Reason: {reason}{Environment.NewLine}",
                                                    useInlineLayout: false);

                // Return success code.
                return LobbyUserRemoveKarmaResults.Success;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                        $"Source: {nameof(RemoveKarma)}{Environment.NewLine}" +
                                                        $"User: ({user?.Id}){Environment.NewLine}" +
                                                        $"Message: {ex}{Environment.NewLine}",
                                                        useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRemoveKarmaResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                                $"Source: {nameof(RemoveKarma)}{Environment.NewLine}" +
                                                $"User: ({user?.Id}){Environment.NewLine}",
                                                useInlineLayout: false);

            // Return failure code.
            return LobbyUserRemoveKarmaResults.Failure;
        }

        /// <summary>
        /// Add Coin.
        /// </summary>
        /// <typeparam name="T">An object of Type LobbyUser.</typeparam>
        /// <param name="user">The user to receive an induction of coin.</param>
        /// <param name="amount">The amount in which to increase the user's coin by.</param>
        /// <returns>Returns an enum of LobbyUserAddCoinResults.</returns>
        public LobbyUserAddCoinResults AddCoin<T>(T user, double amount = 1) where T : LobbyUser
        {
            try
            {
                #region Checks

                // Check User for null.
                if (user == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Adding of Coin(s) failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(AddCoin)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user)}{Environment.NewLine}" +
                                                          $"Message: User object was null.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserAddCoinResults.DeniedUserNull;
                }

                // Check if less than or equal to 0.
                if (amount <= 0)
                {
                    // Record to log.
                    Logger.Record(LogTypes.Warning, $"Adding of Coin(s) failed.{Environment.NewLine}" +
                                                           $"Source: {nameof(AddCoin)}{Environment.NewLine}" +
                                                           $"Variables: {nameof(amount)}{Environment.NewLine}" +
                                                           $"Message: Amount was less than or equal to zero.{Environment.NewLine}",
                                                           useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserAddCoinResults.DeniedAmountLessThanOrEqualToZero;
                }

                // Check membership level.
                if (MembershipStatus >= Memberships.Member)
                {
                    // Record to log.
                    Logger.Record(LogTypes.Warning, $"Adding of Coin(s) failed.{Environment.NewLine}" +
                                                           $"Source: {nameof(AddCoin)}{Environment.NewLine}" +
                                                           $"Variables: {nameof(MembershipStatus)}{Environment.NewLine}" +
                                                           $"Message: Membership status is not {nameof(Memberships.Member)} or higher.{Environment.NewLine}",
                                                           useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserAddCoinResults.DeniedMembershipStatusNotHighEnough;
                }

                #endregion

                // Add Coin(s).
                user.Coins += amount;

                // Log Action.
                ActionLog.Add(new Tuple<DateTime, LogType, ActionType, string>(DateTime.Now, LogType.Info, ActionType.AddCoin, $"Increased the number of coins of user: {user.ScreenName} from: {user.Coins - amount} to: {user.Coins + amount}."));

                // Record to log.
                Logger.Record(LogTypes.Info, $"Coin(s) added successfully.{Environment.NewLine}" +
                                                    $"Source: {nameof(AddCoin)}{Environment.NewLine}" +
                                                    $"User: ({user.ScreenName}){Environment.NewLine}" +
                                                    $"Amount: ({amount}){Environment.NewLine}",
                                                    useInlineLayout: false);

                // Return success code.
                return LobbyUserAddCoinResults.Success;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                        $"Source: {nameof(AddCoin)}{Environment.NewLine}" +
                                                        $"User: ({user?.Id}){Environment.NewLine}" +
                                                        $"Message: {ex}{Environment.NewLine}",
                                                        useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserAddCoinResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                                $"Source: {nameof(AddCoin)}{Environment.NewLine}" +
                                                $"User: ({user?.Id}){Environment.NewLine}",
                                                useInlineLayout: false);

            // Return failure code.
            return LobbyUserAddCoinResults.Failure;
        }

        /// <summary>
        /// Gift Coin.
        /// </summary>
        /// <typeparam name="T">An object of Type LobbyUser.</typeparam>
        /// <param name="user">The user to receive an induction of coin.</param>
        /// <param name="amount">The amount in which to increase the user's coin by.</param>
        /// <returns>Returns an enum of LobbyUserGiftCoinResults.</returns>
        public LobbyUserGiftCoinResults GiftCoin<T>(T user, double amount = 1) where T : LobbyUser
        {
            try
            {
                #region Checks

                // Check User for null.
                if (user == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Gifting of Coin(s) failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(GiftCoin)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user)}{Environment.NewLine}" +
                                                          $"Message: User object was null.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserGiftCoinResults.DeniedUserNull;
                }

                // Check if greater than current amount.
                if (amount > Coins)
                {
                    // Record to log.
                    Logger.Record(LogTypes.Warning, $"Gifting of Coin(s) failed.{Environment.NewLine}" +
                                                           $"Source: {nameof(GiftCoin)}{Environment.NewLine}" +
                                                           $"Variables: {nameof(amount)}/{nameof(Coins)}{Environment.NewLine}" +
                                                           $"Message: Amount ({amount}) was greater than what was available ({Coins}).{Environment.NewLine}",
                                                           useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserGiftCoinResults.DeniedAmountGreaterThanWhatIsAvailable;
                }

                // Check if less than or equal to 0.
                if (amount <= 0)
                {
                    // Record to log.
                    Logger.Record(LogTypes.Warning, $"Gifting of Coin(s) failed.{Environment.NewLine}" +
                                                           $"Source: {nameof(GiftCoin)}{Environment.NewLine}" +
                                                           $"Variables: {nameof(amount)}{Environment.NewLine}" +
                                                           $"Message: Amount was less than or equal to zero.{Environment.NewLine}",
                                                           useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserGiftCoinResults.DeniedAmountLessThanOrEqualToZero;
                }

                // Check membership level.
                if (MembershipStatus >= Memberships.Member)
                {
                    // Write to log.
                    Logger.Record(LogTypes.Warning, $"Gifting of Coin(s) failed.{Environment.NewLine}" +
                                                           $"Source: {nameof(GiftCoin)}{Environment.NewLine}" +
                                                           $"Variables: {nameof(amount)}{Environment.NewLine}" +
                                                           $"Message: Membership status is not {nameof(Memberships.Member)} or higher.{Environment.NewLine}",
                                                           useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserGiftCoinResults.DeniedMembershipStatusNotHighEnough;
                }

                #endregion

                // Gift Coin(s).
                Coins -= amount;
                user.Coins += amount;

                // Log Action.
                ActionLog.Add(new Tuple<DateTime, LogType, ActionType, string>(DateTime.Now, LogType.Info, ActionType.GiftCoin, $"Gifted {user.ScreenName} +{amount} {(amount > 1 ? "coins" : "coin")}!"));

                // Record to log.
                Logger.Record(LogTypes.Info, $"Coin(s) gifted successfully.{Environment.NewLine}" +
                                                    $"Source: {nameof(GiftCoin)}{Environment.NewLine}" +
                                                    $"User: ({user.ScreenName}){Environment.NewLine}" +
                                                    $"Amount: ({amount}){Environment.NewLine}",
                                                    useInlineLayout: false);

                // Return success code.
                return LobbyUserGiftCoinResults.Success;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                        $"Source: {nameof(GiftCoin)}{Environment.NewLine}" +
                                                        $"User: ({user?.Id}){Environment.NewLine}" +
                                                        $"Message: {ex}{Environment.NewLine}",
                                                        useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserGiftCoinResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                                $"Source: {nameof(GiftCoin)}{Environment.NewLine}" +
                                                $"User: ({user?.Id}){Environment.NewLine}",
                                                useInlineLayout: false);

            // Return failure code.
            return LobbyUserGiftCoinResults.Failure;
        }

        /// <summary>
        /// Remove Coin.
        /// </summary>
        /// <typeparam name="T">An object of Type LobbyUser.</typeparam>
        /// <param name="user">The user to receive a reduction in coin.</param>
        /// <param name="reason">The reason for the reduction in coin. Intaken as a string.</param>
        /// <param name="amount">The amount in which to decrease the user's coin by.</param>
        /// <returns>Returns an enum of LobbyUserRemoveCoinResults.</returns>
        public LobbyUserRemoveCoinResults RemoveCoin<T>(T user, string reason, double amount = 1) where T : LobbyUser
        {
            try
            {
                #region Checks

                // Check User for null.
                if (user == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Removal of Coin(s) failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(RemoveCoin)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user)}{Environment.NewLine}" +
                                                          $"Message: User object was null.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRemoveCoinResults.DeniedUserNull;
                }

                // Check for null of whitespace.
                if (string.IsNullOrWhiteSpace(reason))
                {
                    // Record to log.
                    Logger.Record(LogTypes.Warning, $"Removal of Coin(s) failed.{Environment.NewLine}" +
                                                           $"Source: {nameof(RemoveCoin)}{Environment.NewLine}" +
                                                           $"Variables: {nameof(reason)}{Environment.NewLine}" +
                                                           $"Message: Reason was null or whitespace.{Environment.NewLine}",
                                                           useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRemoveCoinResults.DeniedReasonNullOrWhitespace;
                }

                // Check if less than or equal to 0.
                if (amount <= 0)
                {
                    // Record to log.
                    Logger.Record(LogTypes.Warning, $"Removal of Coin(s) failed.{Environment.NewLine}" +
                                                           $"Source: {nameof(RemoveCoin)}{Environment.NewLine}" +
                                                           $"Variables: {nameof(amount)}{Environment.NewLine}" +
                                                           $"Message: Amount was less than or equal to zero.{Environment.NewLine}",
                                                           useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRemoveCoinResults.DeniedAmountLessThanOrEqualToZero;
                }

                // Check membership level.
                if (MembershipStatus >= Memberships.Member)
                {
                    // Record to log.
                    Logger.Record(LogTypes.Warning, $"Removal of Coin(s) failed.{Environment.NewLine}" +
                                                           $"Source: {nameof(RemoveCoin)}{Environment.NewLine}" +
                                                           $"Variables: {nameof(amount)}{Environment.NewLine}" +
                                                           $"Message: Membership status is not {nameof(Memberships.Member)} or higher.{Environment.NewLine}",
                                                           useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRemoveCoinResults.DeniedMembershipStatusNotHighEnough;
                }

                #endregion

                // Add Coin(s).
                user.Coins -= amount;

                // Log Action.
                ActionLog.Add(new Tuple<DateTime, LogType, ActionType, string>(DateTime.Now, LogType.Info, ActionType.RemoveCoin, $"Decreased the number of coins of user: {user.ScreenName} from: {user.Coins + amount} to: {user.Coins - amount}. Reason: {reason}"));

                // Record to log.
                Logger.Record(LogTypes.Info, $"Coin(s) removed successfully.{Environment.NewLine}" +
                                                    $"Source: {nameof(RemoveCoin)}{Environment.NewLine}" +
                                                    $"User: ({user.ScreenName}){Environment.NewLine}" +
                                                    $"Amount: ({amount}){Environment.NewLine}" +
                                                    $"Reason: {reason}{Environment.NewLine}",
                                                    useInlineLayout: false);

                // Return success code.
                return LobbyUserRemoveCoinResults.Success;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                        $"Source: {nameof(RemoveCoin)}{Environment.NewLine}" +
                                                        $"User: ({user?.Id}){Environment.NewLine}" +
                                                        $"Message: {ex}{Environment.NewLine}",
                                                        useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRemoveCoinResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                                $"Source: {nameof(RemoveCoin)}{Environment.NewLine}" +
                                                $"User: ({user?.Id}){Environment.NewLine}",
                                                useInlineLayout: false);

            // Return failure code.
            return LobbyUserRemoveCoinResults.Failure;
        }

        /// <summary>
        /// Add Strike.
        /// </summary>
        /// <typeparam name="T">An object of Type LobbyUser.</typeparam>
        /// <param name="user">The user to receive a strike.</param>
        /// <param name="reason">The reason for the strike. Intaken as a string.</param>
        /// <returns>Returns an enum of LobbyUserAddStrikeResults.</returns>
        public LobbyUserAddStrikeResults AddStrike<T>(T user, string reason) where T : LobbyUser
        {
            try
            {
                #region Checks

                // Check User for null.
                if (user == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Adding of Strike failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(AddStrike)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user)}{Environment.NewLine}" +
                                                          $"Message: User object was null.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserAddStrikeResults.DeniedUserNull;
                }

                // Check for null of whitespace.
                if (string.IsNullOrWhiteSpace(reason))
                {
                    // Record to log.
                    Logger.Record(LogTypes.Warning, $"Adding of Strike failed.{Environment.NewLine}" +
                                                           $"Source: {nameof(AddStrike)}{Environment.NewLine}" +
                                                           $"Variables: {nameof(reason)}{Environment.NewLine}" +
                                                           $"Message: Reason was null or whitespace.{Environment.NewLine}",
                                                           useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserAddStrikeResults.DeniedReasonNullOrWhitespace;
                }

                // Check membership level.
                if (MembershipStatus >= Memberships.Staff)
                {
                    // Record to log.
                    Logger.Record(LogTypes.Warning, $"Adding of Strike failed.{Environment.NewLine}" +
                                                           $"Source: {nameof(AddStrike)}{Environment.NewLine}" +
                                                           $"Variables: {nameof(MembershipStatus)}{Environment.NewLine}" +
                                                           $"Message: Membership status is not {nameof(Memberships.Staff)} or higher.{Environment.NewLine}",
                                                           useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserAddStrikeResults.DeniedMembershipStatusNotHighEnough;
                }

                #endregion

                // Add Coin(s).
                user.Strikes += 1;

                // Log Action.
                ActionLog.Add(new Tuple<DateTime, LogType, ActionType, string>(DateTime.Now, LogType.Info, ActionType.AddStrike, $"Strike given to {user.ScreenName}. Reason: {reason}"));

                // Record to log.
                Logger.Record(LogTypes.Info, $"Strike added successfully.{Environment.NewLine}" +
                                                    $"Source: {nameof(AddStrike)}{Environment.NewLine}" +
                                                    $"User: ({user.ScreenName}){Environment.NewLine}" +
                                                    $"Reason: {reason}{Environment.NewLine}",
                                                    useInlineLayout: false);

                // Return success code.
                return LobbyUserAddStrikeResults.Success;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                        $"Source: {nameof(AddStrike)}{Environment.NewLine}" +
                                                        $"User: ({user?.Id}){Environment.NewLine}" +
                                                        $"Message: {ex}{Environment.NewLine}",
                                                        useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserAddStrikeResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                                $"Source: {nameof(AddStrike)}{Environment.NewLine}" +
                                                $"User: ({user?.Id}){Environment.NewLine}",
                                                useInlineLayout: false);

            // Return failure code.
            return LobbyUserAddStrikeResults.Failure;
        }

        /// <summary>
        /// Remove Strike.
        /// </summary>
        /// <typeparam name="T">An object of Type LobbyUser.</typeparam>
        /// <param name="user">The user to receive a reduction in strikes.</param>
        /// <param name="reason">The reason for the reduction in strikes. Intaken as a string.</param>
        /// <returns>Returns an enum of LobbyUserRemoveStrikeResults.</returns>
        public LobbyUserRemoveStrikeResults RemoveStrike<T>(T user, string reason) where T : LobbyUser
        {
            try
            {
                #region Checks

                // Check User for null.
                if (user == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Removal of Strike failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(RemoveStrike)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user)}{Environment.NewLine}" +
                                                          $"Message: User object was null.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRemoveStrikeResults.DeniedUserNull;
                }

                // Check for null of whitespace.
                if (string.IsNullOrWhiteSpace(reason))
                {
                    // Record to log.
                    Logger.Record(LogTypes.Warning, $"Removal of Strike failed.{Environment.NewLine}" +
                                                           $"Source: {nameof(RemoveStrike)}{Environment.NewLine}" +
                                                           $"Variables: {nameof(reason)}{Environment.NewLine}" +
                                                           $"Message: Reason was null or whitespace.{Environment.NewLine}",
                                                           useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRemoveStrikeResults.DeniedReasonNullOrWhitespace;
                }

                // Check membership level.
                if (MembershipStatus >= Memberships.Staff)
                {
                    // Record to log.
                    Logger.Record(LogTypes.Warning, $"Removal of Strike failed.{Environment.NewLine}" +
                                                           $"Source: {nameof(RemoveStrike)}{Environment.NewLine}" +
                                                           $"Variables: {nameof(MembershipStatus)}{Environment.NewLine}" +
                                                           $"Message: Membership status is not {nameof(Memberships.Staff)} or higher.{Environment.NewLine}",
                                                           useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRemoveStrikeResults.DeniedMembershipStatusNotHighEnough;
                }

                #endregion

                // Add Coin(s).
                user.Strikes -= 1;

                // Log Action.
                ActionLog.Add(new Tuple<DateTime, LogType, ActionType, string>(DateTime.Now, LogType.Info, ActionType.RemoveStrike, $"Strike removed from {user.ScreenName}. Reason: {reason}"));

                // Record to log.
                Logger.Record(LogTypes.Info, $"Strike removed successfully.{Environment.NewLine}" +
                                                    $"Source: {nameof(RemoveStrike)}{Environment.NewLine}" +
                                                    $"User: ({user.ScreenName}){Environment.NewLine}" +
                                                    $"Reason: {reason}{Environment.NewLine}",
                                                    useInlineLayout: false);

                // Return success code.
                return LobbyUserRemoveStrikeResults.Success;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                        $"Source: {nameof(RemoveStrike)}{Environment.NewLine}" +
                                                        $"User: ({user?.Id}){Environment.NewLine}" +
                                                        $"Message: {ex}{Environment.NewLine}",
                                                        useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRemoveStrikeResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                                $"Source: {nameof(RemoveStrike)}{Environment.NewLine}" +
                                                $"User: ({user?.Id}){Environment.NewLine}",
                                                useInlineLayout: false);

            // Return failure code.
            return LobbyUserRemoveStrikeResults.Failure;
        }

        /// <summary>
        /// Add Ban.
        /// </summary>
        /// <typeparam name="T">An object of Type LobbyUser.</typeparam>
        /// <param name="user">The user to receive a banning.</param>
        /// <param name="endDateTime">The end date/time of the ban.</param>
        /// <param name="reason">The reason for the banning. Intaken as a string.</param>
        /// <returns>Returns an enum of LobbyUserAddBanResults.</returns>
        public LobbyUserAddBanResults AddBan<T>(T user, DateTime endDateTime, string reason) where T : LobbyUser
        {
            try
            {
                #region Checks

                // Check User for null.
                if (user == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Adding of ban failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(AddBan)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user)}{Environment.NewLine}" +
                                                          $"Message: User object was null.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserAddBanResults.DeniedUserNull;
                }

                // Check DateTime if less than or equal to DateTime.Now.
                if (endDateTime <= DateTime.Now)
                {
                    // Record to log.
                    Logger.Record(LogTypes.Warning, $"Adding of ban failed.{Environment.NewLine}" +
                                                           $"Source: {nameof(AddBan)}{Environment.NewLine}" +
                                                           $"Variables: {nameof(endDateTime)}{Environment.NewLine}" +
                                                           $"Message: DateTime was less than or equal to DateTime.Now.{Environment.NewLine}",
                                                           useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserAddBanResults.DeniedEndDateTimeLessThanOrEqualToDateTimeNow;
                }

                // Check for null of whitespace.
                if (string.IsNullOrWhiteSpace(reason))
                {
                    // Record to log.
                    Logger.Record(LogTypes.Warning, $"Adding of ban failed.{Environment.NewLine}" +
                                                           $"Source: {nameof(AddBan)}{Environment.NewLine}" +
                                                           $"Variables: {nameof(reason)}{Environment.NewLine}" +
                                                           $"Message: Reason was null or whitespace.{Environment.NewLine}",
                                                           useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserAddBanResults.DeniedReasonNullOrWhitespace;
                }

                // Check membership level.
                if (MembershipStatus >= Memberships.Staff)
                {
                    // Record to log.
                    Logger.Record(LogTypes.Warning, $"Adding of ban failed.{Environment.NewLine}" +
                                                           $"Source: {nameof(AddBan)}{Environment.NewLine}" +
                                                           $"Variables: {nameof(MembershipStatus)}{Environment.NewLine}" +
                                                           $"Message: Membership status is not {nameof(Memberships.Staff)} or higher.{Environment.NewLine}",
                                                           useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserAddBanResults.DeniedMembershipStatusNotHighEnough;
                }

                #endregion

                //TODO: Add Ban.
                user.MembershipStatus = Memberships.Banned;
                user.BanReason = reason;
                user.BanDateTime = DateTime.Now;
                user.BanEndDateTime = endDateTime;

                // Log Action.
                ActionLog.Add(new Tuple<DateTime, LogType, ActionType, string>(DateTime.Now, LogType.Info, ActionType.AddBan, $"{user.ScreenName} banned. Reason: {reason}"));

                // Record to log.
                Logger.Record(LogTypes.Info, $"User banned successfully.{Environment.NewLine}" +
                                                    $"Source: {nameof(AddBan)}{Environment.NewLine}" +
                                                    $"User: ({user.ScreenName}){Environment.NewLine}" +
                                                    $"Reason: {reason}{Environment.NewLine}",
                                                    useInlineLayout: false);

                // Return success code.
                return LobbyUserAddBanResults.Success;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                        $"Source: {nameof(AddBan)}{Environment.NewLine}" +
                                                        $"User: ({user?.Id}){Environment.NewLine}" +
                                                        $"Message: {ex}{Environment.NewLine}",
                                                        useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserAddBanResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                                $"Source: {nameof(AddBan)}{Environment.NewLine}" +
                                                $"User: ({user?.Id}){Environment.NewLine}",
                                                useInlineLayout: false);

            // Return failure code.
            return LobbyUserAddBanResults.Failure;
        }

        /// <summary>
        /// Remove Ban.
        /// </summary>
        /// <typeparam name="T">An object of Type LobbyUser.</typeparam>
        /// <param name="user">The user to receive an unbanning.</param>
        /// <param name="reason">The reason for the unbannig. Intaken as a string.</param>
        /// <returns>Returns an enum of LobbyUserRemoveBanResults.</returns>
        public LobbyUserRemoveBanResults RemoveBan<T>(T user, string reason) where T : LobbyUser
        {
            try
            {
                #region Checks

                // Check User for null.
                if (user == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Removal of ban failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(RemoveBan)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user)}{Environment.NewLine}" +
                                                          $"Message: User object was null.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRemoveBanResults.DeniedUserNull;
                }

                // Check for null of whitespace.
                if (string.IsNullOrWhiteSpace(reason))
                {
                    // Record to log.
                    Logger.Record(LogTypes.Warning, $"Removal of ban failed.{Environment.NewLine}" +
                                                           $"Source: {nameof(RemoveBan)}{Environment.NewLine}" +
                                                           $"Variables: {nameof(reason)}{Environment.NewLine}" +
                                                           $"Message: Reason was null or whitespace.{Environment.NewLine}",
                                                           useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRemoveBanResults.DeniedReasonNullOrWhitespace;
                }

                // Check membership level.
                if (MembershipStatus >= Memberships.Staff)
                {
                    // Record to log.
                    Logger.Record(LogTypes.Warning, $"Removal of ban failed.{Environment.NewLine}" +
                                                           $"Source: {nameof(RemoveBan)}{Environment.NewLine}" +
                                                           $"Variables: {nameof(MembershipStatus)}{Environment.NewLine}" +
                                                           $"Message: Membership status is not {nameof(Memberships.Staff)} or higher.{Environment.NewLine}",
                                                           useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRemoveBanResults.DeniedMembershipStatusNotHighEnough;
                }

                #endregion

                //TODO: Remove Ban.
                user.MembershipStatus = Memberships.Member;
                user.BanReason = reason;
                user.BanEndDateTime = DateTime.Now;

                // Log Action.
                ActionLog.Add(new Tuple<DateTime, LogType, ActionType, string>(DateTime.Now, LogType.Info, ActionType.RemoveBan, $"{user.ScreenName}'s ban removed. Reason: {reason}"));

                // Record to log.
                Logger.Record(LogTypes.Info, $"Ban removed successfully.{Environment.NewLine}" +
                                                    $"Source: {nameof(RemoveBan)}{Environment.NewLine}" +
                                                    $"User: ({user.ScreenName}){Environment.NewLine}" +
                                                    $"Reason: {reason}{Environment.NewLine}",
                                                    useInlineLayout: false);

                // Return success code.
                return LobbyUserRemoveBanResults.Success;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                        $"Source: {nameof(RemoveBan)}{Environment.NewLine}" +
                                                        $"User: ({user?.Id}){Environment.NewLine}" +
                                                        $"Message: {ex}{Environment.NewLine}",
                                                        useInlineLayout: false);

                    // Return failure code.
                    return LobbyUserRemoveBanResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                                $"Source: {nameof(RemoveBan)}{Environment.NewLine}" +
                                                $"User: ({user?.Id}){Environment.NewLine}",
                                                useInlineLayout: false);

            // Return failure code.
            return LobbyUserRemoveBanResults.Failure;
        }

        #endregion

        #region Overriden

        /// <summary>
        /// Equals.
        /// Overriden to compare Id.
        /// </summary>
        /// <param name="obj">The object to compare against.</param>
        /// <returns>Returns a bool indicating whether the objects are equal.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is LobbyUser user))
            {
                return false;
            }

            return Id.Equals(user.Id);
        }

        /// <summary>
        /// Get Hash Code.
        /// Overriden to include Id.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = (int)2166136261;
                hash = (hash * 16777619) ^ Id.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Compare To.
        /// Overriden to compare Usernames alphabetically.
        /// </summary>
        /// <param name="otherUser">The user to compare against.</param>
        /// <returns></returns>
        public int CompareTo(LobbyUser otherUser)
        {
            // Sort alphabetically.
            return string.Compare(UserName, otherUser.UserName, StringComparison.Ordinal);
        }

        /// <summary>
        /// To String.
        /// Overriden to provide more details.
        /// </summary>
        /// <returns>Returns a string containing the user's screen name, their membership status and their karma.</returns>
        public override string ToString()
        {
            return $"Screen Name: {ScreenName}{Environment.NewLine}" +
                   $"Membership:  {MembershipStatus}{Environment.NewLine}" +
                   $"Karma:       {Karma}{Environment.NewLine}";

        }

        /// <summary>
        /// == Operator for LobbyUser.
        /// Overloaded.
        /// </summary>
        /// <param name="userOne">The first user.</param>
        /// <param name="userTwo">The second user.</param>
        /// <returns>Returns a bool indicating whether the two users reference the same LobbyUser.</returns>
        public static bool operator ==(LobbyUser userOne, LobbyUser userTwo)
        {
            return userOne?.Equals(userTwo) ?? userTwo is null;
        }

        /// <summary>
        /// != Operator for LobbyUser.
        /// Overloaded.
        /// </summary>
        /// <param name="userOne">The first user.</param>
        /// <param name="userTwo">The second user.</param>
        /// <returns>Returns a bool indicating whether the two users reference the same LobbyUser.</returns>
        public static bool operator !=(LobbyUser userOne, LobbyUser userTwo)
        {
            return !(userOne == userTwo);
        }

        #endregion
    }
}