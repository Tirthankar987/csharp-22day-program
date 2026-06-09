using System;
using System.Collections.Generic;

namespace ForeachLimitationDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== FOREACH LIMITATION DEMO ===");

            // Create a list of patients
            List<string> patients = new List<string>
            {
                "John",
                "Jane",
                "Alice",
                "Jane"
            };

            Console.WriteLine("\nInitial List:");
            PrintList(patients);

            // Example 1: Modifying list inside foreach (commented to avoid crash)
            
            //foreach (string p in patients)
            //{
            //    if (p == "Jane")
            //    {
            //        patients.Remove(p); // This will throw InvalidOperationException
            //    }
            //}
            

            Console.WriteLine("\nForeach example is commented because it will crash.");

            // Example 2: Correct way using for loop
            for (int i = patients.Count - 1; i >= 0; i--)
            {
                if (patients[i] == "Jane")
                {
                    patients.RemoveAt(i);
                }
            }

            Console.WriteLine("\nAfter removing 'Jane' using for loop:");
            PrintList(patients);

            Console.WriteLine("\nProgram completed safely.");
        }

        // Reusable method to print list contents
        static void PrintList(List<string> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }
    }
}

