/* Jeff O'Hara
 * 5-15-26
 * A C# console application that allows users to create, view, search, and manage incident reports.
 * the application uses two classes: IncidentReport to represent individual reports and IncidentManager to handle the list of reports and related actions.
 */

using System;

namespace IncidentReportTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            // UPDATE ADDED 05/26/2026:
            // Create the authentication manager first.
            // The user must log in before accessing the incident tracker menu.
            AuthManager authManager = new AuthManager();

            bool loginSuccessful = authManager.Login();

            if (!loginSuccessful)
            {
                return;
            }

            IncidentManager manager = new IncidentManager();

            bool running = true;

            while (running)
            {
                Console.WriteLine("\n================================");
                Console.WriteLine("     INCIDENT REPORT TRACKER");
                Console.WriteLine("================================");
                Console.WriteLine($"Logged in as: {authManager.CurrentUser.Username}");
                Console.WriteLine($"Role: {authManager.CurrentUser.Role}");
                Console.WriteLine();
                Console.WriteLine("1. Add incident report");
                Console.WriteLine("2. View all incident reports");
                Console.WriteLine("3. Search incident reports");
                Console.WriteLine("4. Mark incident as resolved");
                Console.WriteLine("5. Delete incident report");
                Console.WriteLine("6. View incident dashboard");
                Console.WriteLine("7. Exit");
                Console.Write("\nEnter your choice, such as 1 or 2: ");

                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    manager.AddIncident();
                }
                else if (choice == "2")
                {
                    manager.ViewAllIncidents();
                }
                else if (choice == "3")
                {
                    manager.SearchIncident();
                }
                else if (choice == "4")
                {
                    manager.MarkIncidentResolved();
                }
                else if (choice == "5")
                {
                    manager.DeleteIncident();
                }
                else if (choice == "6")
                {
                    manager.ViewIncidentDashboard();
                }
                else if (choice == "7")
                {
                    running = false;
                    Console.WriteLine("Exiting Incident Report Tracker.");
                }
                else
                {
                    Console.WriteLine("Invalid option. Please choose 1 through 7.");
                }
            }
        }
    }
}