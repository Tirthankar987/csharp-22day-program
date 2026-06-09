using System;                       // For Console operations
using System.Collections.Generic;   // Required for Queue<T>

class Program
{
    static void Main(string[] args)
    {
        // Create a Queue for patient tokens - FIFO: first patient gets first token served
        Queue<int> tokens = new Queue<int>();

        // Variable to track the next token number to assign
        int nextToken = 1;

        // Welcome and instructions
        Console.WriteLine("=== HCA Patient Token Queue System ===");
        Console.WriteLine("Tokens are served in arrival order (FIFO).\n");

        // Main loop - continues until user exits
        while (true)
        {
            // Show menu
            Console.WriteLine("Choose an action:");
            Console.WriteLine("1: New patient arrives (assign token)");
            Console.WriteLine("2: Call next patient (serve token)");
            Console.WriteLine("3: View current waiting queue");
            Console.WriteLine("4: Exit");
            Console.Write("Enter choice (1-4): ");

            string choice = Console.ReadLine();

            // Option 1: New patient - assign token
            if (choice == "1")
            {
                // Add the next token number to the end of the queue
                tokens.Enqueue(nextToken);

                // Show the assigned token
                Console.WriteLine($"Patient received token: {nextToken}");

                // Increment for the next patient
                nextToken++;

                Console.WriteLine();
            }
            // Option 2: Serve next patient
            else if (choice == "2")
            {
                // Check if there are patients waiting
                if (tokens.Count > 0)
                {
                    // Remove and get the first token (front of queue)
                    int calledToken = tokens.Dequeue();

                    Console.WriteLine($"Now serving token number: {calledToken}");
                    Console.WriteLine("Please proceed to counter.\n");
                }
                else
                {
                    // No one in queue
                    Console.WriteLine("No patients currently waiting.\n");
                }
            }
            // Option 3: Display queue
            else if (choice == "3")
            {
                Console.WriteLine($"Patients waiting: {tokens.Count}");

                if (tokens.Count == 0)
                {
                    Console.WriteLine("(Queue is empty)\n");
                }
                else
                {
                    Console.WriteLine("Tokens in order:");
                    // foreach safely iterates without modifying queue
                    foreach (int token in tokens)
                    {
                        Console.WriteLine($"  Token {token}");
                    }
                    Console.WriteLine();
                }
            }
            // Option 4: Exit
            else if (choice == "4")
            {
                Console.WriteLine("Token system closed. Thank you!");
                break;
            }
            // Invalid input
            else
            {
                Console.WriteLine("Invalid choice. Please enter 1-4.\n");
            }
        }
    }
}
