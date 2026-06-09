using System;                       // For Console and basic types
using System.Collections.Generic;   // Required for Stack<T>

class Program
{
    static void Main(string[] args)
    {
        // Create a Stack to store previous versions of the diagnosis note
        // Stack follows LIFO: last added note is first to be undone
        Stack<string> noteHistory = new Stack<string>();

        // Current note being edited - starts empty
        string currentNote = "";

        // Welcome message to explain the app
        Console.WriteLine("=== HCA Diagnosis Note Editor with Undo ===");
        Console.WriteLine("You can add notes and undo mistakes.\n");

        // Main loop - keeps running until user chooses to exit
        while (true)
        {
            // Display menu options
            Console.WriteLine("Choose an action:");
            Console.WriteLine("1: Add new note (saves current as history)");
            Console.WriteLine("2: Undo last change");
            Console.WriteLine("3: View current note");
            Console.WriteLine("4: Exit");
            Console.Write("Enter choice (1-4): ");

            // Read user input
            string choice = Console.ReadLine();

            // Handle option 1: Add new note
            if (choice == "1")
            {
                // Before changing the note, save the current version to history
                // This allows undo later

                // Ask for the new note text
                Console.Write("Enter new diagnosis note: ");
                currentNote = Console.ReadLine();
                noteHistory.Push(currentNote);

                // Confirm addition
                Console.WriteLine("New note saved.\n");
            }
            // Handle option 2: Undo
            else if (choice == "2")
            {
                // Check if there is any history to undo
                if (noteHistory.Count > 0)
                {
                    // Pop removes and returns the most recent saved note
                    currentNote = noteHistory.Pop();
                    Console.WriteLine("Undo successful - previous note restored.\n");
                }
                else
                {
                    // No history means nothing to undo
                    Console.WriteLine("Nothing to undo - no previous notes saved.\n");
                }
            }
            // Handle option 3: View current note
            else if (choice == "3")
            {
                Console.WriteLine("Current diagnosis note:");
                if (noteHistory.Count > 0)
                {
                    Console.WriteLine(noteHistory.Peek());
                }
                Console.WriteLine();
            }
            // Handle option 4: Exit the program
            else if (choice == "4")
            {
                Console.WriteLine("Goodbye! Final note saved.");
                break;  // Exits the while loop and ends program
            }
            // Invalid choice handling
            else
            {
                Console.WriteLine("Invalid choice. Please enter 1, 2, 3, or 4.\n");
            }
        }
    }
}

