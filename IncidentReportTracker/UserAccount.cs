namespace IncidentReportTracker
{
    // UPDATE ADDED 05/26/2026:
    // This class represents one user account for the app.
    public class UserAccount
    {
        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public string Role { get; set; }

        // Needed for JSON loading.
        public UserAccount()
        {
        }

        public UserAccount(string username, string passwordHash, string passwordSalt, string role)
        {
            Username = username;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            Role = role;
        }
    }
}