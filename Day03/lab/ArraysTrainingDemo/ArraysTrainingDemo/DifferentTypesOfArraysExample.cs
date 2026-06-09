using System;

namespace ArraysTrainingDemo
{
    public class DifferentTypesOfArraysExample
    {
        public static void Run()
        {
            Console.Clear();
            Console.WriteLine("EXAMPLE 4: Arrays with Different Data Types");
            Console.WriteLine("==========================================");

            // Double array
            double[] heartRates = { 72.5, 88.0, 65.3, 94.1 };

            // Boolean array
            bool[] insuranceApproved = { true, false, true, true };

            // Char array
            char[] severity = { 'M', 'H', 'L', 'M' };  // M=Medium, H=High, L=Low

            Console.WriteLine("Heart Rates (bpm):");
            foreach (double rate in heartRates)
            {
                Console.WriteLine("  " + rate);
            }

            Console.WriteLine("\nInsurance Status:");
            for (int i = 0; i < insuranceApproved.Length; i++)
            {
                Console.WriteLine($"  Patient {i + 1}: {(insuranceApproved[i] ? "Approved" : "Pending")}");
            }

            Console.WriteLine("\nCase Severity:");
            foreach (char s in severity)
            {
                Console.WriteLine("  Level: " + s);
            }

            Console.WriteLine("\nAll types stored contiguously in memory.");
            Console.WriteLine("Press any key for exceptions example...");
            Console.ReadKey();
        }
    }
}
