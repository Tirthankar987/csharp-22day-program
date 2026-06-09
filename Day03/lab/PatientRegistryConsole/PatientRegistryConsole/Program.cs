using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;

namespace PatientRegistryConsole
{
    class Program
    {
        // Main data storage using List<string> - dynamic and flexible
        static List<string> patients = new List<string>();
        static List<(string,string,DateTime)> PatientNameAudit = new List<(string,string,DateTime)>();
            
        static void Main(string[] args)
        {
            Console.Title = "HCA Patient Registry - Day 4: Arrays & Lists";

            Console.WriteLine("===========================================");
            Console.WriteLine("     HCA HEALTHCARE PATIENT REGISTRY");
            Console.WriteLine("         Day 4 - Data Structures Demo");
            Console.WriteLine("===========================================\n");

            // Show limitation of array first
            DemonstrateArrayLimitation();

            // Main loop - keeps asking until user chooses to exit
            while (true)
            {
                DisplayMenu();
                string choice = Console.ReadLine()?.Trim();

                Console.Clear(); // Clear screen for clean look after each operation

                switch (choice)
                {
                    case "1":
                        AddPatient();
                        break;
                    case "2":
                        RemovePatient();
                        break;
                    case "3":
                        SearchPatient();
                        break;
                    case "4":
                        UpdatePatient();
                        break;
                    case "5":
                        DisplayAllPatients();
                        break;
                    case "6":
                        SortPatients();
                        break;
                    case "8":
                        DisplayPatientNameAudit();
                        break;
                    case "7":
                        Console.WriteLine("Thank you for using HCA Patient Registry!");
                        Console.WriteLine("Goodbye!");
                        return; // Exits the application

                    default:
                        Console.WriteLine("Invalid choice! Please enter a number from 1 to 7.");
                        break;
                }

                // After every operation, ask to continue
                Console.WriteLine("\nPress any key to return to the main menu...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void DemonstrateArrayLimitation()
        {
            Console.WriteLine("DEMO: Limitation of Arrays (Fixed Size)\n");
            string[] doctors = new string[4]; // Fixed size of 4

            doctors[0] = "Dr. Anderson";
            doctors[1] = "Dr. Patel";
            doctors[2] = "Dr. Garcia";
            doctors[3] = "Dr. Kim";

            Console.WriteLine("We created an array with exactly 4 doctors:");
            for (int i = 0; i < doctors.Length; i++)
            {
                Console.WriteLine($"  {i + 1}. {doctors[i]}");
            }

            Console.WriteLine("\nProblem: If a 5th doctor joins, we cannot easily add them!");
            Console.WriteLine("→ Arrays have fixed size → hard to grow/shrink.\n");
            Console.WriteLine("Solution: Use List<string> → can grow dynamically!\n");

            Console.WriteLine("Press any key to start using List<string> for patients...");
            Console.ReadKey();
            Console.Clear();
        }

        static void DisplayMenu()
        {
            Console.WriteLine("MAIN MENU");
            Console.WriteLine($"Current Registered Patients: {patients.Count}");
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("1. Add Patient");
            Console.WriteLine("2. Remove Patient");
            Console.WriteLine("3. Search Patient");
            Console.WriteLine("4. Update Patient");
            Console.WriteLine("5. Display All Patients");
            Console.WriteLine("6. Sort Patients (A-Z)");
            Console.WriteLine("7. Exit Application");
            Console.WriteLine("8. Display All Patients Audits");
            Console.Write("\nEnter your choice (1-7): ");
        }

        static void AddPatient()
        {
            Console.Write("Enter patient name: ");
            string name = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Error: Patient name cannot be empty!");
            }
            else if (patients.Contains(name, StringComparer.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Patient '{name}' is already registered.");
            }
            else
            {
                patients.Add(name);
                Console.WriteLine($"Success: Patient '{name}' added!");
            }
        }

        static void RemovePatient()
        {
            if (patients.Count == 0)
            {
                Console.WriteLine("No patients to remove.");
                return;
            }

            DisplayAllPatients();
            Console.Write("\nEnter patient name to remove: ");
            string name = Console.ReadLine()?.Trim();

            if (patients.Remove(name))
            {
                Console.WriteLine($"Success: Patient '{name}' removed.");
            }
            else
            {
                Console.WriteLine($"Patient '{name}' not found.");
            }
        }

        static void SearchPatient()
        {
            if (patients.Count == 0)
            {
                Console.WriteLine("No patients registered yet.");
                return;
            }

            Console.Write("Enter name (or part of name) to search: ");
            string search = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(search))
            {
                Console.WriteLine("Search text cannot be empty.");
                return;
            }

            var results = patients
                .Select((patient, index) => new { patient, index })
                .Where(x => x.patient.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            if (results.Any())
            {
                Console.WriteLine($"\nFound {results.Count} matching patient(s):");
                foreach (var result in results)
                {
                    Console.WriteLine($"  {result.index + 1}. {result.patient}");
                }
            }
            else
            {
                Console.WriteLine("No patients found matching your search.");
            }
        }

        static void UpdatePatient()
        {
            if (patients.Count == 0)
            {
                Console.WriteLine("No patients to update.");
                return;
            }

            DisplayAllPatients();
            Console.Write("\nEnter current patient name to update: ");
            string oldName = Console.ReadLine()?.Trim();

            int index = patients.FindIndex(p => p.Equals(oldName, StringComparison.OrdinalIgnoreCase));

            if (index == -1)
            {
                Console.WriteLine("Patient not found.");
                return;
            }

            Console.Write("Enter new name: ");
            string newName = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(newName))
            {
                Console.WriteLine("New name cannot be empty.");
            }
            else
            {
                patients[index] = newName;
                Console.WriteLine($"Success: Updated '{oldName}' → '{newName}'");
                PatientNameAudit.Add((oldName, newName,DateTime.UtcNow));
                //foreach(var patient in PatientNameAudit)
                //{
                //    Console.WriteLine(patient);
                //}
                
            }
        }
        static void DisplayPatientNameAudit()
        {
            foreach (var patient in PatientNameAudit)
            {
                Console.WriteLine(patient);
            }
        }
        static void DisplayAllPatients()
        {
            Console.WriteLine($"\nREGISTERED PATIENTS ({patients.Count})");
            Console.WriteLine("---------------------------------");

            if (patients.Count == 0)
            {
                Console.WriteLine("No patients registered yet.");
                return;
            }

            // Using foreach - clean and safe iteration
            for (int i = 0; i < patients.Count; i++)
            {
                Console.WriteLine($"  {i + 1}. {patients[i]}");
            }
        }

        static void SortPatients()
        {
            if (patients.Count == 0)
            {
                Console.WriteLine("No patients to sort.");
                return;
            }

            patients = patients.OrderBy(p => p, StringComparer.OrdinalIgnoreCase).ToList();
            Console.WriteLine("Patients sorted alphabetically (A-Z)!");
            DisplayAllPatients();
        }
    }
}
