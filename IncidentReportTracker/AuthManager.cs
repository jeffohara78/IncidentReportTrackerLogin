using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;

namespace IncidentReportTracker
{
    // UPDATE ADDED 05/26/2026:
    // This class handles login and user account storage.
    public class AuthManager
    {
        private List<UserAccount> users = new List<UserAccount>();

        private string filePath = "users.json";

        public UserAccount CurrentUser { get; private set; }

        public AuthManager()
        {
            LoadUsersFromFile();

            // If no users exist yet, create one default account.
            if (users.Count == 0)
            {
                CreateDefaultUser();
            }
        }

        public bool Login()
        {
            Console.WriteLine("\n================================");
            Console.WriteLine("       INCIDENT TRACKER LOGIN");
            Console.WriteLine("================================");
            Console.WriteLine("Default practice account:");
            Console.WriteLine("Username: analyst");
            Console.WriteLine("Password: ChangeMe123!");
            Console.WriteLine();

            for (int attempts = 1; attempts <= 3; attempts++)
            {
                Console.Write("Username: ");
                string username = Console.ReadLine();

                Console.Write("Password: ");
                string password = Console.ReadLine();

                UserAccount user = users.Find(account => account.Username == username);

                if (user != null && VerifyPassword(password, user.PasswordSalt, user.PasswordHash))
                {
                    CurrentUser = user;

                    Console.WriteLine($"\nLogin successful. Welcome, {CurrentUser.Username}.");
                    Console.WriteLine($"Role: {CurrentUser.Role}");

                    return true;
                }

                Console.WriteLine($"Invalid login. Attempt {attempts} of 3.");
            }

            Console.WriteLine("Too many failed login attempts. Access denied.");
            return false;
        }

        private void CreateDefaultUser()
        {
            string username = "analyst";
            string plainTextPassword = "ChangeMe123!";
            string role = "Analyst";

            string salt = GenerateSalt();

            string hash = HashPassword(plainTextPassword, salt);

            UserAccount defaultUser = new UserAccount(username, hash, salt, role);

            users.Add(defaultUser);

            SaveUsersToFile();
        }

        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];

            RandomNumberGenerator.Fill(saltBytes);

            return Convert.ToBase64String(saltBytes);
        }

        private string HashPassword(string password, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);

            // PBKDF2 is stronger than storing or comparing plain-text passwords.
            using Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(
                password,
                saltBytes,
                100000,
                HashAlgorithmName.SHA256
            );

            byte[] hashBytes = pbkdf2.GetBytes(32);

            return Convert.ToBase64String(hashBytes);
        }

        private bool VerifyPassword(string enteredPassword, string storedSalt, string storedHash)
        {
            string enteredHash = HashPassword(enteredPassword, storedSalt);

            return enteredHash == storedHash;
        }

        private void SaveUsersToFile()
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string jsonData = JsonSerializer.Serialize(users, options);

            File.WriteAllText(filePath, jsonData);
        }

        private void LoadUsersFromFile()
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            string jsonData = File.ReadAllText(filePath);

            if (string.IsNullOrWhiteSpace(jsonData))
            {
                return;
            }

            users = JsonSerializer.Deserialize<List<UserAccount>>(jsonData);

            if (users == null)
            {
                users = new List<UserAccount>();
            }
        }
    }
}