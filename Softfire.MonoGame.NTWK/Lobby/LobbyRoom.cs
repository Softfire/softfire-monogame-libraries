using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Softfire.MonoGame.LOG;

namespace Softfire.MonoGame.NTWK.Lobby
{
    public class LobbyRoom<T> where T : LobbyUser
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
        /// Parent Lobby.
        /// </summary>
        public Lobby<T> ParentLobby { private get; set; }

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
        private double PreviosUpdateTimeInMilliseconds { get; set; }

        /// <summary>
        /// Lidgren Lobby Room Update Frequency In Seocnds.
        /// </summary>
        private const double LobbyRoomUpdateFrequencyInSeconds = 1;

        /// <summary>
        /// Room Id.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Room Name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Room Owner.
        /// </summary>
        public T RoomOwner { get; private set; }

        /// <summary>
        /// Room Access Password.
        /// </summary>
        private string AccessPassword { get; set; }

        /// <summary>
        /// Room Admin Password.
        /// </summary>
        private string AdminPassword { get; set; }

        /// <summary>
        /// Is Flagged For Deletion?
        /// </summary>
        public bool IsFlaggedForDeletion { get; private set; }

        /// <summary>
        /// Room Visibilities.
        /// </summary>
        public List<Visibilities> VisibilityLevels { get; private set; }

        /// <summary>
        /// Room Visiblity.
        /// </summary>
        public enum Visibilities
        {
            Guests,
            Members,
            Private,
            VIP,
            Staff,
            Admin
        }

        /// <summary>
        /// Lobby Room Set Room Owner Results.
        /// </summary>
        public enum LobbyRoomSetRoomOwnerResults
        {
            Failure,
            Success,
            DeniedUserNull,
            DeniedAdminPasswordNullOrWhitespace,
            DeniedPasswordsDoNotMatch
        }

        /// <summary>
        /// Lobby Room Set Room Name Results.
        /// </summary>
        public enum LobbyRoomSetRoomNameResults
        {
            Failure,
            Success,
            DeniedUserNull,
            DeniedRoomNameNull,
            DeniedAdminPasswordNullOrWhitespace,
            DeniedPasswordsDoNotMatch
        }

        /// <summary>
        /// Lobby Room Set Room Access Password Results.
        /// </summary>
        public enum LobbyRoomSetRoomAccessPasswordResults
        {
            Failure,
            Success,
            DeniedUserNull,
            DeniedAccessPasswordNullOrWhitespace,
            DeniedAdminPasswordNullOrWhitespace,
            DeniedPasswordsDoNotMatch
        }

        /// <summary>
        /// Lobby Room Set Room Admin Password Results.
        /// </summary>
        public enum LobbyRoomSetRoomAdminPasswordResults
        {
            Failure,
            Success,
            DeniedUserNull,
            DeniedCurrentAdminPasswordNullOrWhitespace,
            DeniedNewAdminPasswordNullOrWhitespace,
            DeniedPasswordsDoNotMatch
        }

        /// <summary>
        /// Lobby Room Login Results.
        /// </summary>
        public enum LobbyRoomLoginResults
        {
            Failure,
            Success,
            DeniedUserNull,
            DeniedUserAlreadyLoggedIn,
            DeniedUserBanned,
            DeniedAccessPasswordNullOrWhitespace,
            DeniedPasswordsDoNotMatch
        }

        /// <summary>
        /// Lobby Room Post Results.
        /// </summary>
        public enum LobbyRoomPostResults
        {
            Failure,
            Success,
            DeniedUserNull,
            DeniedMessageNullOrWhitespace
        }

        /// <summary>
        /// Lobby Room Check Users List For User Results.
        /// </summary>
        public enum LobbyRoomCheckListForUserResults
        {
            False,
            True,
            DeniedUserNull,
            Failure
        }

        /// <summary>
        /// Lobby Room Add User To List Results.
        /// </summary>
        public enum LobbyRoomAddUserToListResults
        {
            Failure,
            Success,
            DeniedUserNull,
            DeniedUserBanned,
            DeniedUserNotRoomOwner
        }

        /// <summary>
        /// Lobby Room Remove User From List Results.
        /// </summary>
        public enum LobbyRoomRemoveUserFromListResults
        {
            Failure,
            Success,
            DeniedUserNull,
            DeniedUserBanned,
            DeniedUserNotRoomOwner
        }

        /// <summary>
        /// Room Chat Log.
        /// </summary>
        public List<Tuple<DateTime, T, string>> ChatLog { get; }

        /// <summary>
        /// Room Users List.
        /// </summary>
        private Dictionary<T, bool> UsersList { get; }

        /// <summary>
        /// Lidgren Lobby Room Constructor.
        /// </summary>
        /// <param name="id">Server generated unique id.</param>
        /// <param name="roomName">The room name. Intaken as a <see cref="string"/>.</param>
        /// <param name="user">The requesting user. User becomes the Room Owner.</param>
        /// <param name="adminPassword">The admin password to modify the room once created. Intaken as a <see cref="string"/>.</param>
        /// <param name="accessPassword">The access password. Used by users to access the room. Intaken as a <see cref="string"/>.</param>
        /// <param name="logFilePath">Intakes a file path for logs relative to the calling application.</param>
        public LobbyRoom(Guid id, string roomName, T user, string adminPassword, string accessPassword = null, string logFilePath = @"Config\Logs\Lobby\Rooms")
        {
            Logger = new Logger(logFilePath);
            
            Id = id;
            Name = roomName;
            RoomOwner = user;
            AdminPassword = adminPassword;
            AccessPassword = accessPassword;

            IsFlaggedForDeletion = false;

            ChatLog = new List<Tuple<DateTime, T, string>>();
            UsersList = new Dictionary<T, bool>();

            VisibilityLevels = new List<Visibilities>(7);

            // Privatize room if AccessPassword is not null or whitespace.
            if (!string.IsNullOrWhiteSpace(AccessPassword))
            {
                VisibilityLevels.Add(Visibilities.Private);
            }
        }

        #region Admin Commands

        /// <summary>
        /// Set Room Owner.
        /// Sets the room owner to one of Type T.
        /// </summary>
        /// <param name="requestingUser">The user requesting to set the room owner.</param>
        /// <param name="userToSetAsOwner">The user to be set as the room owner.</param>
        /// <param name="adminPassword">The room's admin password. If permissions allow, the room's admin password will be updated by the one provided.</param>
        /// <returns>Returns an enum of LobbyRoomSetRoomOwnerResults specifying the result.</returns>
        public LobbyRoomSetRoomOwnerResults SetRoomOwner(T requestingUser, T userToSetAsOwner, string adminPassword)
        {
            try
            {
                #region Checks

                // Check requesting user for null.
                if (requestingUser == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Setting room owner failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(SetRoomOwner)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(requestingUser)}{Environment.NewLine}" +
                                                          $"Message: Requesting user object was null.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomSetRoomOwnerResults.DeniedUserNull;
                }

                // Check user to set as room owner for null.
                if (userToSetAsOwner == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Setting room owner failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(SetRoomOwner)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(userToSetAsOwner)}{Environment.NewLine}" +
                                                          $"Message: User object to set as room owner was null.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomSetRoomOwnerResults.DeniedUserNull;
                }

                // Check permissions level.
                if (requestingUser.MembershipStatus >= ParentLobby.Settings.SetRoomOwnerMinimumMembershipLevel)
                {
                    // Set owner.
                    RoomOwner = userToSetAsOwner;

                    // Set admin password.
                    if (!AuthenticateAdminCredentials(adminPassword))
                    {
                        AdminPassword = adminPassword;
                    }

                    // Write to log.
                    Logger.Write(LogTypes.Info, $"Setting room owner succeeded.{Environment.NewLine}" +
                                                       $"Source: {nameof(SetRoomOwner)}{Environment.NewLine}" +
                                                       $"Variables: {nameof(userToSetAsOwner.MembershipStatus)}{Environment.NewLine}" +
                                                       $"Message: User is {ParentLobby.Settings.SetRoomOwnerMinimumMembershipLevel.ToString()} or higher.{Environment.NewLine}",
                                                       useInlineLayout: false);

                    // Return success code.
                    return LobbyRoomSetRoomOwnerResults.Success;
                }

                // Check admin password for null or whitespace.
                if (string.IsNullOrWhiteSpace(adminPassword))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Setting room owner failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(SetRoomOwner)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(adminPassword)}{Environment.NewLine}" +
                                                          $"Message: Admin password was null or whitespace.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomSetRoomOwnerResults.DeniedAdminPasswordNullOrWhitespace;
                }

                // Authenticate password.
                if (!AuthenticateAdminCredentials(adminPassword))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Setting room owner failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(SetRoomOwner)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(AdminPassword)} - {nameof(adminPassword)}{Environment.NewLine}" +
                                                          $"Message: Passwords did not match.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomSetRoomOwnerResults.DeniedPasswordsDoNotMatch;
                }

                #endregion

                // Set owner.
                RoomOwner = userToSetAsOwner;

                // Write to log.
                Logger.Write(LogTypes.Info, $"Setting room owner succeeded.{Environment.NewLine}" +
                                                   $"Source: {nameof(SetRoomOwner)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(userToSetAsOwner)}:{userToSetAsOwner.UserName}{Environment.NewLine}" +
                                                   $"Variables: {nameof(adminPassword)}{Environment.NewLine}",
                                                   useInlineLayout: false);

                // Return success code.
                return LobbyRoomSetRoomOwnerResults.Success;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                        $"Source: {nameof(SetRoomOwner)}{Environment.NewLine}" +
                                                        $"Message: {ex}{Environment.NewLine}",
                                                        useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomSetRoomOwnerResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                                $"Source: {nameof(SetRoomOwner)}{Environment.NewLine}" +
                                                $"Variables: ({nameof(userToSetAsOwner)}:{userToSetAsOwner?.UserName}){Environment.NewLine}" +
                                                $"Variables: ({nameof(adminPassword)}:{adminPassword}){Environment.NewLine}",
                                                useInlineLayout: false);

            // Return failure code.
            return LobbyRoomSetRoomOwnerResults.Failure;
        }

        /// <summary>
        /// Set Room Name.
        /// </summary>
        /// <param name="user">The user requesting to set the room name.</param>
        /// <param name="roomName">The new room name to set.</param>
        /// <param name="adminPassword">The room's admin password.</param>
        /// <returns>Returns an enum of LobbyRoomSetRoomNameResults specifying the result.</returns>
        public LobbyRoomSetRoomNameResults SetRoomName(T user, string roomName, string adminPassword)
        {
            try
            {
                #region Checks

                // Check user for null.
                if (user == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Setting room name failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(SetRoomName)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user)}{Environment.NewLine}" +
                                                          $"Message: User object was null.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomSetRoomNameResults.DeniedUserNull;
                }

                // Check room name for null or whitespace.
                if (string.IsNullOrWhiteSpace(roomName))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Setting room name failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(SetRoomName)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(roomName)}{Environment.NewLine}" +
                                                          $"Message: Room name object was null.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomSetRoomNameResults.DeniedRoomNameNull;
                }

                // Check permissions level.
                if (user.MembershipStatus >= ParentLobby.Settings.SetRoomNameMinimumMembershipLevel)
                {
                    // Set room name.
                    Name = roomName;

                    // Set admin password.
                    AdminPassword = adminPassword;

                    // Write to log.
                    Logger.Write(LogTypes.Info, $"Setting room name succeeded.{Environment.NewLine}" +
                                                       $"Source: {nameof(SetRoomName)}{Environment.NewLine}" +
                                                       $"Variables: {nameof(user.MembershipStatus)}{Environment.NewLine}" +
                                                       $"Message: User is {ParentLobby.Settings.SetRoomNameMinimumMembershipLevel.ToString()} or higher.{Environment.NewLine}",
                                                       useInlineLayout: false);

                    // Return success code.
                    return LobbyRoomSetRoomNameResults.Success;
                }

                // Check admin password for null or whitespace.
                if (string.IsNullOrWhiteSpace(adminPassword))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Setting room name failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(SetRoomName)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(adminPassword)}{Environment.NewLine}" +
                                                          $"Message: Admin password was null or whitespace.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomSetRoomNameResults.DeniedAdminPasswordNullOrWhitespace;
                }

                // Authenticate password.
                if (!AdminPassword.Equals(adminPassword))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Setting room owner failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(SetRoomOwner)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(AdminPassword)} - {nameof(adminPassword)}{Environment.NewLine}" +
                                                          $"Message: Passwords did not match.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomSetRoomNameResults.DeniedPasswordsDoNotMatch;
                }

                #endregion

                // Set room name.
                Name = roomName;

                // Write to log.
                Logger.Write(LogTypes.Info, $"Setting room name succeeded.{Environment.NewLine}" +
                                                   $"Source: {nameof(SetRoomName)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(user)}:{user.UserName}{Environment.NewLine}" +
                                                   $"Variables: {nameof(roomName)}:{roomName}{Environment.NewLine}" +
                                                   $"Variables: {nameof(adminPassword)}{Environment.NewLine}",
                                                   useInlineLayout: false);

                // Return success code.
                return LobbyRoomSetRoomNameResults.Success;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                        $"Source: {nameof(SetRoomName)}{Environment.NewLine}" +
                                                        $"Message: {ex}{Environment.NewLine}",
                                                        useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomSetRoomNameResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                                $"Source: {nameof(SetRoomOwner)}{Environment.NewLine}" +
                                                $"Variables: ({nameof(user)}:{user?.UserName}){Environment.NewLine}" +
                                                $"Variables: ({nameof(roomName)}:{roomName}){Environment.NewLine}" +
                                                $"Variables: ({nameof(adminPassword)}:{adminPassword}){Environment.NewLine}",
                                                useInlineLayout: false);

            // Return failure code.
            return LobbyRoomSetRoomNameResults.Failure;
        }

        /// <summary>
        /// Set Room Access Password.
        /// </summary>
        /// <param name="user">The user requesting to set the room access password.</param>
        /// <param name="accessPassword">The new access password to set.</param>
        /// <param name="adminPassword">The room's admin password.</param>
        /// <returns>Returns an enum of LobbyRoomSetRoomAccessPasswordResults specifying the result.</returns>
        public LobbyRoomSetRoomAccessPasswordResults SetRoomAccessPassword(T user, string accessPassword, string adminPassword)
        {
            try
            {
                #region Checks

                // Check user for null.
                if (user == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Setting room access password failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(SetRoomAccessPassword)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user)}{Environment.NewLine}" +
                                                          $"Message: User object was null.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomSetRoomAccessPasswordResults.DeniedUserNull;
                }

                // Check permissions level.
                if (user.MembershipStatus >= ParentLobby.Settings.SetRoomAccessPasswordMinimumMembershipLevel)
                {
                    // Set access password.
                    AccessPassword = accessPassword;

                    // Write to log.
                    Logger.Write(LogTypes.Info, $"Setting room access password succeeded.{Environment.NewLine}" +
                                                       $"Source: {nameof(SetRoomOwner)}{Environment.NewLine}" +
                                                       $"Variables: {nameof(user.MembershipStatus)}{Environment.NewLine}" +
                                                       $"Message: User is {ParentLobby.Settings.SetRoomAccessPasswordMinimumMembershipLevel.ToString()} or higher.{Environment.NewLine}",
                                                       useInlineLayout: false);

                    // Return success code.
                    return LobbyRoomSetRoomAccessPasswordResults.Success;
                }

                // Check access password for null or whitespace.
                if (string.IsNullOrWhiteSpace(accessPassword))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Setting room access password failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(SetRoomAccessPassword)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(accessPassword)}{Environment.NewLine}" +
                                                          $"Message: Access password was null or whitespace.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomSetRoomAccessPasswordResults.DeniedAccessPasswordNullOrWhitespace;
                }

                // Check admin password for null or whitespace.
                if (string.IsNullOrWhiteSpace(adminPassword))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Setting room access password failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(SetRoomAccessPassword)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(adminPassword)}{Environment.NewLine}" +
                                                          $"Message: Admin password was null or whitespace.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomSetRoomAccessPasswordResults.DeniedAdminPasswordNullOrWhitespace;
                }

                // Authenticate password.
                if (!AuthenticateAdminCredentials(adminPassword))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Setting room access password failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(SetRoomAccessPassword)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(AdminPassword)} != {nameof(adminPassword)}{Environment.NewLine}" +
                                                          $"Message: Passwords did not match.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomSetRoomAccessPasswordResults.DeniedPasswordsDoNotMatch;
                }

                #endregion

                // Set access password.
                AccessPassword = accessPassword;

                // Write to log.
                Logger.Write(LogTypes.Info, $"Setting room access password succeeded.{Environment.NewLine}" +
                                                   $"Source: {nameof(SetRoomAccessPassword)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(user)}:{user.UserName}{Environment.NewLine}" +
                                                   $"Variables: {nameof(accessPassword)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(adminPassword)}{Environment.NewLine}",
                                                   useInlineLayout: false);

                // Return success code.
                return LobbyRoomSetRoomAccessPasswordResults.Success;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                        $"Source: {nameof(SetRoomAccessPassword)}{Environment.NewLine}" +
                                                        $"Message: {ex}{Environment.NewLine}",
                                                        useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomSetRoomAccessPasswordResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                                $"Source: {nameof(SetRoomAccessPassword)}{Environment.NewLine}" +
                                                $"Variables: ({nameof(user)}:{user?.UserName}){Environment.NewLine}" +
                                                $"Variables: ({nameof(accessPassword)}:{accessPassword}){Environment.NewLine}" +
                                                $"Variables: ({nameof(adminPassword)}:{adminPassword}){Environment.NewLine}",
                                                useInlineLayout: false);

            // Return failure code.
            return LobbyRoomSetRoomAccessPasswordResults.Failure;
        }

        /// <summary>
        /// Set Room Admin Password.
        /// </summary>
        /// <param name="user">The user requesting to set the room admin password.</param>
        /// <param name="currentAdminPassword">The current admin password</param>
        /// <param name="newAdminPassword">The new admin password to set.</param>
        /// <returns>Returns an enum of LobbyRoomSetRoomAdminPasswordResults specifying the result.</returns>
        public LobbyRoomSetRoomAdminPasswordResults SetRoomAdminPassword(T user, string currentAdminPassword, string newAdminPassword)
        {
            try
            {
                #region Checks

                // Check user for null.
                if (user == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Setting room admin password failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(SetRoomAdminPassword)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user)}{Environment.NewLine}" +
                                                          $"Message: User object was null.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomSetRoomAdminPasswordResults.DeniedUserNull;
                }

                // Check permissions level.
                if (user.MembershipStatus >= ParentLobby.Settings.SetRoomAdminPasswordMinimumMembershipLevel)
                {
                    // Set access password.
                    AdminPassword = newAdminPassword;

                    // Write to log.
                    Logger.Write(LogTypes.Info, $"Setting room admin password succeeded.{Environment.NewLine}" +
                                                       $"Source: {nameof(SetRoomAdminPassword)}{Environment.NewLine}" +
                                                       $"Variables: {nameof(user.MembershipStatus)}{Environment.NewLine}" +
                                                       $"Message: User is {ParentLobby.Settings.SetRoomAdminPasswordMinimumMembershipLevel} or higher.{Environment.NewLine}",
                                                       useInlineLayout: false);

                    // Return success code.
                    return LobbyRoomSetRoomAdminPasswordResults.Success;
                }

                // Check current password for null or whitespace.
                if (string.IsNullOrWhiteSpace(currentAdminPassword))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Setting room admin password failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(SetRoomAdminPassword)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(currentAdminPassword)}{Environment.NewLine}" +
                                                          $"Message: Current password was null or whitespace.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomSetRoomAdminPasswordResults.DeniedCurrentAdminPasswordNullOrWhitespace;
                }

                // Check new password for null or whitespace.
                if (string.IsNullOrWhiteSpace(newAdminPassword))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Setting room admin password failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(SetRoomAdminPassword)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(newAdminPassword)}{Environment.NewLine}" +
                                                          $"Message: New password was null or whitespace.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomSetRoomAdminPasswordResults.DeniedNewAdminPasswordNullOrWhitespace;
                }

                // Authenticate password.
                if (!AuthenticateAdminCredentials(currentAdminPassword))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Setting room access password failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(SetRoomAdminPassword)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(AdminPassword)} != {nameof(currentAdminPassword)}{Environment.NewLine}" +
                                                          $"Message: Passwords did not match.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomSetRoomAdminPasswordResults.DeniedPasswordsDoNotMatch;
                }

                #endregion

                // Set access password.
                AdminPassword = newAdminPassword;

                // Write to log.
                Logger.Write(LogTypes.Info, $"Setting room access password succeeded.{Environment.NewLine}" +
                                                   $"Source: {nameof(SetRoomAdminPassword)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(user)}:{user.UserName}{Environment.NewLine}" +
                                                   $"Variables: {nameof(currentAdminPassword)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(newAdminPassword)}{Environment.NewLine}",
                                                   useInlineLayout: false);

                // Return success code.
                return LobbyRoomSetRoomAdminPasswordResults.Success;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                        $"Source: {nameof(SetRoomAdminPassword)}{Environment.NewLine}" +
                                                        $"Message: {ex}{Environment.NewLine}",
                                                        useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomSetRoomAdminPasswordResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                                $"Source: {nameof(SetRoomAdminPassword)}{Environment.NewLine}" +
                                                $"Variables: ({nameof(user)}:{user?.UserName}){Environment.NewLine}" +
                                                $"Variables: ({nameof(currentAdminPassword)}:{currentAdminPassword}){Environment.NewLine}" +
                                                $"Variables: ({nameof(newAdminPassword)}:{newAdminPassword}){Environment.NewLine}",
                                                useInlineLayout: false);

            // Return failure code.
            return LobbyRoomSetRoomAdminPasswordResults.Failure;
        }

        #endregion

        #region Room Commands

        /// <summary>
        /// Login.
        /// Login to room.
        /// </summary>
        /// <param name="user">The user requesting to login.</param>
        /// <param name="accessPassword">The roo'ms access password.</param>
        /// <returns>Returns an enum of LobbyRoomLoginResults specifying the result.</returns>
        public LobbyRoomLoginResults Login(T user, string accessPassword = null)
        {
            try
            {
                #region Checks

                // Check user for null.
                if (user == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Login failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(Login)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(user)}{Environment.NewLine}" +
                                                   $"Message: User object was null.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomLoginResults.DeniedUserNull;
                }

                // Check user membership for ban.
                if (user.MembershipStatus == LobbyUser.Memberships.Banned)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Login failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(Login)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(user)}:{user.UserName}{Environment.NewLine}" +
                                                   $"Message: User is banned.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomLoginResults.DeniedUserBanned;
                }

                // Check if already logged in.
                if (CheckUsersListForUser(user).Item1 == LobbyRoomCheckListForUserResults.True)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Login failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(Login)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(user)}:{user.UserName}{Environment.NewLine}" +
                                                   $"Message: User is already logged in.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomLoginResults.DeniedUserAlreadyLoggedIn;
                }

                // Check for vip room and user is on vip list.
                if (VisibilityLevels.Contains(Visibilities.VIP) &&
                    CheckUsersListForUser(user).Item2)
                {
                    // Add user to room.
                    UsersList.Add(user, true);

                    // Write to log.
                    Logger.Write(LogTypes.Info, $"Login successful.{Environment.NewLine}" +
                                                       $"Source: {nameof(Login)}{Environment.NewLine}" +
                                                       $"Variables: {nameof(user)}:{user.UserName}{Environment.NewLine}" +
                                                       $"Variables: {nameof(LobbyRoom<T>)}:{Name}{Environment.NewLine}" +
                                                       $"Variables: {nameof(Visibilities.VIP)}{Environment.NewLine}",
                                                       useInlineLayout: false);

                    // Return success code.
                    return LobbyRoomLoginResults.Success;
                }

                // Check for private room.
                if (VisibilityLevels.Contains(Visibilities.Private))
                {
                    // Check for access password null or whitespace.
                    if (string.IsNullOrWhiteSpace(accessPassword))
                    {
                        // Write to log.
                        Logger.Write(LogTypes.Warning, $"Login failed.{Environment.NewLine}" +
                                                       $"Source: {nameof(Login)}{Environment.NewLine}" +
                                                       $"Variables: {nameof(user)}:{user.UserName}{Environment.NewLine}" +
                                                       $"Variables: {nameof(accessPassword)}{Environment.NewLine}" +
                                                       $"Message: Access password was null or whitespace.{Environment.NewLine}",
                                                       useInlineLayout: false);

                        // Return failure code.
                        return LobbyRoomLoginResults.DeniedAccessPasswordNullOrWhitespace;
                    }

                    // Authenticate access password.
                    if (!AuthenticateAccessPassword(accessPassword))
                    {
                        // Write to log.
                        Logger.Write(LogTypes.Warning, $"Login failed.{Environment.NewLine}" +
                                                       $"Source: {nameof(Login)}{Environment.NewLine}" +
                                                       $"Variables: {nameof(user)}:{user.UserName}{Environment.NewLine}" +
                                                       $"Variables: {nameof(accessPassword)}{Environment.NewLine}" +
                                                       $"Message: Passwords do not match.{Environment.NewLine}",
                                                       useInlineLayout: false);

                        // Return failure code.
                        return LobbyRoomLoginResults.DeniedPasswordsDoNotMatch;
                    }

                    // Add user to room.
                    UsersList.Add(user, false);

                    // Write to log.
                    Logger.Write(LogTypes.Info, $"Login successful.{Environment.NewLine}" +
                                                $"Source: {nameof(Login)}{Environment.NewLine}" +
                                                $"Variables: {nameof(user)}:{user.UserName}{Environment.NewLine}" +
                                                $"Variables: {nameof(LobbyRoom<T>)}:{Name}{Environment.NewLine}",
                                                useInlineLayout: false);

                    // Return success code.
                    return LobbyRoomLoginResults.Success;
                }

                // Check permissions.
                if (VisibilityLevels.Contains(Visibilities.Guests) & user.MembershipStatus == LobbyUser.Memberships.Guest ||
                    VisibilityLevels.Contains(Visibilities.Members) & user.MembershipStatus == LobbyUser.Memberships.Member ||
                    VisibilityLevels.Contains(Visibilities.Staff) & user.MembershipStatus == LobbyUser.Memberships.Staff ||
                    VisibilityLevels.Contains(Visibilities.Admin) & user.MembershipStatus == LobbyUser.Memberships.Admin)
                {
                    // Add user to room.
                    UsersList.Add(user, false);

                    // Write to log.
                    Logger.Write(LogTypes.Info, $"Login successful.{Environment.NewLine}" +
                                                $"Source: {nameof(Login)}{Environment.NewLine}" +
                                                $"Variables: {nameof(user)}:{user.UserName}{Environment.NewLine}" +
                                                $"Variables: {nameof(LobbyRoom<T>)}:{Name}{Environment.NewLine}",
                                                useInlineLayout: false);

                    // Return success code.
                    return LobbyRoomLoginResults.Success;
                }

                #endregion
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                 $"Source: {nameof(Login)}{Environment.NewLine}" +
                                                 $"Message: {ex}{Environment.NewLine}",
                                                 useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomLoginResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                         $"Source: {nameof(Login)}{Environment.NewLine}" +
                                         $"Variables: ({nameof(user)}:{user?.UserName}){Environment.NewLine}" +
                                         $"Variables: ({nameof(accessPassword)}:{accessPassword}){Environment.NewLine}",
                                         useInlineLayout: false);

            // Return failure code.
            return LobbyRoomLoginResults.Failure;
        }

        /// <summary>
        /// Post.
        /// Post to chat.
        /// </summary>
        /// <param name="user">The user requesting to post to chat.</param>
        /// <param name="message">The message to post to chat.</param>
        /// <returns>Returns an enum of LobbyRoomPostResults specifying the result.</returns>
        public LobbyRoomPostResults Post(T user, string message)
        {
            try
            {
                #region Checks

                // Check user for null.
                if (user == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Login failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(Post)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user)}{Environment.NewLine}" +
                                                          $"Message: User object was null.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomPostResults.DeniedUserNull;
                }

                // Check message for null or whitespace.
                if (string.IsNullOrWhiteSpace(message))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Login failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(Post)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(user)}:{user.UserName}{Environment.NewLine}" +
                                                          $"Variables: {nameof(message)}{Environment.NewLine}" +
                                                          $"Message: Message was null or whitespace.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomPostResults.DeniedMessageNullOrWhitespace;
                }

                #endregion

                // Add message to chat log.
                ChatLog.Add(new Tuple<DateTime, T, string>(DateTime.Now, user, message));

                // Write to log.
                Logger.Write(LogTypes.Info, $"Post successful.{Environment.NewLine}" +
                                                   $"Source: {nameof(Post)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(user)}:{user.UserName}{Environment.NewLine}" +
                                                   $"Variables: {nameof(LobbyRoom<T>)}:{Name}{Environment.NewLine}" +
                                                   $"Variables: {nameof(message)}:{message}{Environment.NewLine}",
                                                   useInlineLayout: false);

                // Return success code.
                return LobbyRoomPostResults.Success;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                        $"Source: {nameof(Post)}{Environment.NewLine}" +
                                                        $"Message: {ex}{Environment.NewLine}",
                                                        useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomPostResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                                $"Source: {nameof(Post)}{Environment.NewLine}" +
                                                $"Variables: ({nameof(user)}:{user?.UserName}){Environment.NewLine}" +
                                                $"Variables: ({nameof(message)}:{message}){Environment.NewLine}",
                                                useInlineLayout: false);

            // Return failure code.
            return LobbyRoomPostResults.Failure;
        }

        /// <summary>
        /// Add User To Users List.
        /// </summary>
        /// <param name="requestingUser">The user requesting to add a user to the room's user list.</param>
        /// <param name="userToAdd">The user to add to the room's user list.</param>
        /// <param name="isVip">Is user to be added as a Vip? Allows user to enter private room without entering a password.</param>
        /// <returns>Returns an enum of LobbyRoomAddUserToListResults specifying the result.</returns>
        public LobbyRoomAddUserToListResults AddUserToUsersList(T requestingUser, T userToAdd, bool isVip)
        {
            // Call add user method.
            return AddUserToList(UsersList, requestingUser, userToAdd, isVip);
        }

        /// <summary>
        /// Add User To List.
        /// </summary>
        /// <param name="list">The list to add the user to.</param>
        /// <param name="requestingUser">The user requesting to add a user to the list.</param>
        /// <param name="userToAdd">The user to add to the list.</param>
        /// <param name="isVip">Is user to be added as a Vip? Allows user to enter private room without entering a password.</param>
        /// <returns>Returns an enum of LobbyRoomAddUserToListResults specifying the result.</returns>
        private LobbyRoomAddUserToListResults AddUserToList(IDictionary<T, bool> list, T requestingUser, T userToAdd, bool isVip)
        {
            try
            {
                #region Checks

                // Check requesting user for null.
                if (requestingUser == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Adding user to list failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(AddUserToList)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(requestingUser)}{Environment.NewLine}" +
                                                          $"Message: Requesting user object was null.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomAddUserToListResults.DeniedUserNull;
                }

                // Check user to add for null.
                if (userToAdd == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Adding user to list failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(AddUserToList)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(userToAdd)}{Environment.NewLine}" +
                                                          $"Message: User object to add was null.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomAddUserToListResults.DeniedUserNull;
                }

                // Check for ban.
                if (requestingUser.MembershipStatus == LobbyUser.Memberships.Banned)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Adding user to list failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(AddUserToList)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(requestingUser)}{Environment.NewLine}" +
                                                          $"Message: User is banned.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomAddUserToListResults.DeniedUserBanned;
                }

                // Check for ban.
                if (userToAdd.MembershipStatus == LobbyUser.Memberships.Banned)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Adding user to list failed.{Environment.NewLine}" +
                                                          $"Source: {nameof(AddUserToList)}{Environment.NewLine}" +
                                                          $"Variables: {nameof(userToAdd)}{Environment.NewLine}" +
                                                          $"Message: User is banned.{Environment.NewLine}",
                                                          useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomAddUserToListResults.DeniedUserBanned;
                }

                // Check if user is already on list.
                if (CheckListForUser(list, userToAdd).Item1 == LobbyRoomCheckListForUserResults.True)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Adding user to list failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(AddUserToList)}{Environment.NewLine}" +
                                                   $"Variables: ({nameof(userToAdd)}:{userToAdd.UserName}){Environment.NewLine}" +
                                                   $"Message: User is already in list.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomAddUserToListResults.Failure;
                }

                // Check permissions.
                if (requestingUser.MembershipStatus >= ParentLobby.Settings.AddUserToRoomUsersListMinimumMembershipLevel)
                {
                    // Add user to list.
                    list.Add(userToAdd, isVip);

                    // Write to log.
                    Logger.Write(LogTypes.Info, $"Added user to list successfully.{Environment.NewLine}" +
                                                $"Source: {nameof(AddUserToList)}{Environment.NewLine}" +
                                                $"Variables: ({nameof(userToAdd)}:{userToAdd.UserName}){Environment.NewLine}",
                                                useInlineLayout: false);

                    // Return success code.
                    return LobbyRoomAddUserToListResults.Success;
                }

                // Check if room owner.
                if (!RoomOwner.Equals(requestingUser))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Adding user to list failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(AddUserToList)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(requestingUser)}{Environment.NewLine}" +
                                                   $"Message: User is not room owner.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomAddUserToListResults.DeniedUserNotRoomOwner;
                }

                #endregion

                // Add user to list.
                list.Add(userToAdd, isVip);

                // Write to log.
                Logger.Write(LogTypes.Info, $"Added user to list successfully.{Environment.NewLine}" +
                                            $"Source: {nameof(AddUserToList)}{Environment.NewLine}" +
                                            $"Variables: ({nameof(requestingUser)}:{requestingUser.UserName}){Environment.NewLine}" +
                                            $"Variables: ({nameof(userToAdd)}:{userToAdd.UserName}){Environment.NewLine}",
                                            useInlineLayout: false);

                // Return success code.
                return LobbyRoomAddUserToListResults.Success;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                 $"Source: {nameof(AddUserToList)}{Environment.NewLine}" +
                                                 $"Message: {ex}{Environment.NewLine}",
                                                 useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomAddUserToListResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                         $"Source: {nameof(AddUserToList)}{Environment.NewLine}" +
                                         $"Variables: ({nameof(requestingUser)}:{requestingUser?.UserName}){Environment.NewLine}" +
                                         $"Variables: ({nameof(userToAdd)}:{userToAdd?.UserName}){Environment.NewLine}",
                                         useInlineLayout: false);

            // Return failure code.
            return LobbyRoomAddUserToListResults.Failure;
        }

        /// <summary>
        /// Check Users List For User.
        /// </summary>
        /// <param name="user">The user to check for.</param>
        /// <returns>Returns an enum of LobbyRoomCheckListForUserResults specifying the result and a boolean indicating whether the user is a Vip.</returns>
        public (LobbyRoomCheckListForUserResults, bool) CheckUsersListForUser(T user)
        {
            // Call check method.
            return CheckListForUser(UsersList, user);
        }

        /// <summary>
        /// Check List For User.
        /// </summary>
        /// <param name="list">The list to check in.</param>
        /// <param name="user">The user to check for.</param>
        /// <returns>Returns an enum of LobbyRoomCheckListForUserResults specifying the result and a boolean indicating whether the user is a Vip.</returns>
        private (LobbyRoomCheckListForUserResults, bool) CheckListForUser(IDictionary<T, bool> list, T user)
        {
            try
            {
                #region Checks

                // Check user for null.
                if (user == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"User check failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(CheckListForUser)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(user)}{Environment.NewLine}" +
                                                   $"Message: User object was null.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return (LobbyRoomCheckListForUserResults.DeniedUserNull, false);
                }

                // Check if user is on list.
                if (!list.ContainsKey(user))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Info, $"User check successful. User not found.{Environment.NewLine}" +
                                                $"Source: {nameof(CheckListForUser)}{Environment.NewLine}" +
                                                $"Variables: {nameof(user)}:{user.UserName}{Environment.NewLine}" +
                                                $"Variables: {nameof(LobbyRoom<T>)}:{Name}{Environment.NewLine}",
                                                useInlineLayout: false);

                    // Return failure code.
                    return (LobbyRoomCheckListForUserResults.False, false);
                }

                #endregion

                // Write to log.
                Logger.Write(LogTypes.Info, $"User check successful. User found.{Environment.NewLine}" +
                                            $"Source: {nameof(CheckListForUser)}{Environment.NewLine}" +
                                            $"Variables: {nameof(user)}:{user.UserName}{Environment.NewLine}" +
                                            $"Variables: {nameof(LobbyRoom<T>)}:{Name}{Environment.NewLine}",
                                            useInlineLayout: false);

                // Return success code.
                return (LobbyRoomCheckListForUserResults.True, list[user]);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                 $"Source: {nameof(CheckListForUser)}{Environment.NewLine}" +
                                                 $"Message: {ex}{Environment.NewLine}",
                                                 useInlineLayout: false);

                    // Return failure code.
                    return (LobbyRoomCheckListForUserResults.Failure, false);
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                         $"Source: {nameof(CheckListForUser)}{Environment.NewLine}" +
                                         $"Variables: ({nameof(user)}:{user?.UserName}){Environment.NewLine}",
                                         useInlineLayout: false);

            // Return failure code.
            return (LobbyRoomCheckListForUserResults.Failure, false);
        }

        /// <summary>
        /// Remove User From Users List.
        /// </summary>
        /// <param name="requestingUser">The user requesting to remove a user from the room's user list.</param>
        /// <param name="userToRemove">The user to remove.</param>
        /// <returns>Returns an enum of LobbyRoomRemoveUserFromListResults specifying the result.</returns>
        public LobbyRoomRemoveUserFromListResults RemoveUserFromUsersList(T requestingUser, T userToRemove)
        {
            // Call removal method.
            return RemoveUserFromList(UsersList, requestingUser, userToRemove);
        }

        /// <summary>
        /// Remove User From List.
        /// </summary>
        /// <param name="list">The list to remove a user from.</param>
        /// <param name="requestingUser">The user requesting to remove a user from the list.</param>
        /// <param name="userToRemove">The user to remove.</param>
        /// <returns>Returns an enum of LobbyRoomRemoveUserFromListResults specifying the result.</returns>
        private LobbyRoomRemoveUserFromListResults RemoveUserFromList(IDictionary<T, bool> list, T requestingUser, T userToRemove)
        {
            try
            {
                #region Checks

                // Check requesting user for null.
                if (requestingUser == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Removing user from list failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(RemoveUserFromList)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(requestingUser)}{Environment.NewLine}" +
                                                   $"Message: Requesting user object was null.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomRemoveUserFromListResults.DeniedUserNull;
                }

                // Check user to remove for null.
                if (userToRemove == null)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Removing user from list failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(RemoveUserFromList)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(userToRemove)}{Environment.NewLine}" +
                                                   $"Message: User object to remove was null.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomRemoveUserFromListResults.DeniedUserNull;
                }

                // Check for ban.
                if (requestingUser.MembershipStatus == LobbyUser.Memberships.Banned)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Removing user from list failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(RemoveUserFromList)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(requestingUser)}{Environment.NewLine}" +
                                                   $"Message: User is banned.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomRemoveUserFromListResults.DeniedUserBanned;
                }

                // Check for ban.
                if (userToRemove.MembershipStatus == LobbyUser.Memberships.Banned)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Removing user from list failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(RemoveUserFromList)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(userToRemove)}{Environment.NewLine}" +
                                                   $"Message: User is banned.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomRemoveUserFromListResults.DeniedUserBanned;
                }

                // Check if user is on list.
                if (CheckListForUser(list, userToRemove).Item1 == LobbyRoomCheckListForUserResults.False)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Removing user from list failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(RemoveUserFromList)}{Environment.NewLine}" +
                                                   $"Variables: ({nameof(userToRemove)}:{userToRemove.UserName}){Environment.NewLine}" +
                                                   $"Message: User is not in list.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomRemoveUserFromListResults.Failure;
                }

                // Check permissions.
                if (requestingUser.MembershipStatus >= ParentLobby.Settings.AddUserToRoomUsersListMinimumMembershipLevel)
                {
                    // Add user to list.
                    list.Remove(userToRemove);

                    // Write to log.
                    Logger.Write(LogTypes.Info, $"Removed user from list successfully.{Environment.NewLine}" +
                                                $"Source: {nameof(RemoveUserFromList)}{Environment.NewLine}" +
                                                $"Variables: ({nameof(userToRemove)}:{userToRemove.UserName}){Environment.NewLine}",
                                                useInlineLayout: false);

                    // Return success code.
                    return LobbyRoomRemoveUserFromListResults.Success;
                }

                // Check if room owner.
                if (!RoomOwner.Equals(requestingUser))
                {
                    // Write to log.
                    Logger.Write(LogTypes.Warning, $"Removing user from list failed.{Environment.NewLine}" +
                                                   $"Source: {nameof(RemoveUserFromList)}{Environment.NewLine}" +
                                                   $"Variables: {nameof(requestingUser)}{Environment.NewLine}" +
                                                   $"Message: User is not room owner.{Environment.NewLine}",
                                                   useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomRemoveUserFromListResults.DeniedUserNotRoomOwner;
                }

                #endregion

                // Add user to list.
                list.Remove(userToRemove);

                // Write to log.
                Logger.Write(LogTypes.Info, $"Removed user from list successfully.{Environment.NewLine}" +
                                            $"Source: {nameof(RemoveUserFromList)}{Environment.NewLine}" +
                                            $"Variables: ({nameof(requestingUser)}:{requestingUser.UserName}){Environment.NewLine}" +
                                            $"Variables: ({nameof(userToRemove)}:{userToRemove.UserName}){Environment.NewLine}",
                                            useInlineLayout: false);

                // Return success code.
                return LobbyRoomRemoveUserFromListResults.Success;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    // Write to log.
                    Logger.Write(LogTypes.Error, $"Error in received data.{Environment.NewLine}" +
                                                 $"Source: {nameof(RemoveUserFromList)}{Environment.NewLine}" +
                                                 $"Message: {ex}{Environment.NewLine}",
                                                 useInlineLayout: false);

                    // Return failure code.
                    return LobbyRoomRemoveUserFromListResults.Failure;
                }
            }

            // Write to log.
            Logger.Write(LogTypes.Error, $"Error! Something went wrong.{Environment.NewLine}" +
                                         $"Source: {nameof(RemoveUserFromList)}{Environment.NewLine}" +
                                         $"Variables: ({nameof(requestingUser)}:{requestingUser?.UserName}){Environment.NewLine}" +
                                         $"Variables: ({nameof(userToRemove)}:{userToRemove?.UserName}){Environment.NewLine}",
                                         useInlineLayout: false);

            // Return failure code.
            return LobbyRoomRemoveUserFromListResults.Failure;
        }

        /// <summary>
        /// Get Lobby Room Users List Count.
        /// </summary>
        /// <returns>Returns the number of users in the room as an int.</returns>
        public int GetLobbyRoomUsersCount()
        {
            return UsersList.Count;
        }

        #endregion

        /// <summary>
        /// Authenticate Admin Credentials.
        /// </summary>
        /// <param name="adminPassword">Admin password to check. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a bool indicating whether the passwords matched.</returns>
        public bool AuthenticateAdminCredentials(string adminPassword)
        {
            return !string.IsNullOrWhiteSpace(adminPassword) && AdminPassword.Equals(adminPassword);
        }

        /// <summary>
        /// Authenticate Access Credentials.
        /// </summary>
        /// <param name="accessPassword">Access password to check. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a bool indicating whether the passwords matched.</returns>
        public bool AuthenticateAccessPassword(string accessPassword)
        {
            return !string.IsNullOrWhiteSpace(accessPassword) && AccessPassword.Equals(accessPassword);
        }

        /// <summary>
        /// Lidgren Lobby Room Update Method.
        /// </summary>
        public void Update()
        {
            PreviosUpdateTimeInMilliseconds = DeltaTime;
            ElapsedTime += DeltaTime = Timer.ElapsedMilliseconds - PreviosUpdateTimeInMilliseconds;

            if (ElapsedTime >= LobbyRoomUpdateFrequencyInSeconds)
            {

            }
        }

        /// <summary>
        /// Lidgren Lobby Room Update Method.
        /// </summary>
        /// <param name="gameTime">MonoGame GameTime.</param>
        public void Update(GameTime gameTime)
        {
            ElapsedTime += DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            if (ElapsedTime >= LobbyRoomUpdateFrequencyInSeconds)
            {

            }
        }
    }
}