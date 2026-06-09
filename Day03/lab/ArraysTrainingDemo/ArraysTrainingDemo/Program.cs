using System;

namespace ArraysTrainingDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "HCA Day 4: Arrays Training Demo";

            Console.WriteLine("========================================");
            Console.WriteLine("     HCA HEALTHCARE - ARRAYS TRAINING");
            Console.WriteLine("     Simple Step-by-Step Examples");
            Console.WriteLine("========================================\n");

            // FIRST EXAMPLE: Directly in Program.cs - Most Basic Integer Array
            Console.WriteLine("EXAMPLE 1: Basic Integer Array (Direct in Program.cs)");
            Console.WriteLine("=====================================================");

            // Create array of 5 integers - contiguous memory
            int[] patientIds = new int[5];

            // Fill the array
            patientIds[0] = 1001;  // Index 0
            patientIds[1] = 1002;
            patientIds[2] = 1003;
            patientIds[3] = 1004;
            patientIds[4] = 1005;  // Index 4

            // Display
            Console.WriteLine("Patient IDs in the array:");
            for (int i = 0; i < patientIds.Length; i++)
            {
                Console.WriteLine($"  Position {i}: ID {patientIds[i]}");
            }

            Console.WriteLine("\nThis array uses contiguous memory - fast access!");
            Console.WriteLine("\nPress any key to continue to other examples...");
            Console.ReadKey();
            Console.Clear();

            // Now run other examples from separate classes
            StringArrayExample.Run();
            ShortInitializationExample.Run();
            DifferentTypesOfArraysExample.Run();
            ArrayExceptionsExample.Run();

            Console.WriteLine("\nAll examples completed!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
