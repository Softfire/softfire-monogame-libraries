namespace Softfire.MonoGame.NTWK.V2.Lobby
{
    public class LobbySettings
    {
        #region Lobby User Properties

        /// <summary>
        /// Lobby User First Name Minimum Length.
        /// </summary>
        public int FirstNameMinimumLength { get; }

        /// <summary>
        /// Lobby User First Name Maximum Length.
        /// </summary>
        public int FirstNameMaximumLength { get; }

        /// <summary>
        /// Lobby User Last Name Minimum Length.
        /// </summary>
        public int LastNameMinimumLength { get; }

        /// <summary>
        /// Lobby User Last Name Maximum Length.
        /// </summary>
        public int LastNameMaximumLength { get; }

        /// <summary>
        /// Lobby User User Name Minimum Length.
        /// </summary>
        public int UserNameMinimumLength { get; }

        /// <summary>
        /// Lobby User Last Name Maximum Length.
        /// </summary>
        public int UserNameMaximumLength { get; }

        /// <summary>
        /// Lobby User Password Minimum Length.
        /// </summary>
        public int PasswordMinimumLength { get; }

        /// <summary>
        /// Lobby User Password Maximum Length.
        /// </summary>
        public int PasswordMaximumLength { get; }

        #endregion

        #region Room Properties

        public LobbyUser.Memberships SetRoomOwnerMinimumMembershipLevel { get; set; } = LobbyUser.Memberships.Staff;

        public LobbyUser.Memberships SetRoomNameMinimumMembershipLevel { get; set; } = LobbyUser.Memberships.Staff;

        public LobbyUser.Memberships SetRoomAccessPasswordMinimumMembershipLevel { get; set; } = LobbyUser.Memberships.Staff;

        public LobbyUser.Memberships SetRoomAdminPasswordMinimumMembershipLevel { get; set; } = LobbyUser.Memberships.Staff;

        public LobbyUser.Memberships AddUserToRoomUsersListMinimumMembershipLevel { get; set; } = LobbyUser.Memberships.Staff;

        #endregion

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public LobbySettings()
        {
            
        }

        /// <summary>
        /// Secondary Constructor.
        /// </summary>
        /// <param name="firstNameMinimumLength"></param>
        /// <param name="firstNameMaximumLength"></param>
        /// <param name="lastNameMinimumLength"></param>
        /// <param name="lastNameMaximumLength"></param>
        /// <param name="userNameMinimumLength"></param>
        /// <param name="userNameMaximumLength"></param>
        /// <param name="passwordMinimumLength"></param>
        /// <param name="passwordMaximumLength"></param>
        public LobbySettings(int firstNameMinimumLength, int firstNameMaximumLength,
                             int lastNameMinimumLength, int lastNameMaximumLength,
                             int userNameMinimumLength, int userNameMaximumLength,
                             int passwordMinimumLength, int passwordMaximumLength)
        {
            FirstNameMinimumLength = firstNameMinimumLength;
            FirstNameMaximumLength = firstNameMaximumLength;
            LastNameMinimumLength = lastNameMinimumLength;
            LastNameMaximumLength = lastNameMaximumLength;
            UserNameMinimumLength = userNameMinimumLength;
            UserNameMaximumLength = userNameMaximumLength;
            PasswordMinimumLength = passwordMinimumLength;
            PasswordMaximumLength = passwordMaximumLength;
        }
    }
}