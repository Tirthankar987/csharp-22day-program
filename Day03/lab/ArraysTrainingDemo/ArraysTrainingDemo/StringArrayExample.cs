using System;

namespace ArraysTrainingDemo
{
    public class StringArrayExample
    {
        public static void Run()
        {
            Console.Clear();
            Console.WriteLine("EXAMPLE 2: String Array - Patient Names");
            Console.WriteLine("=======================================");

            // Create fixed-size array for 5 patients
            string[] patients = new string[5];

            // Assign values
            patients[0] = "Ava Thompson";
            patients[1] = "Ethan Rodriguez";
            patients[2] = "Mia Hernandez";
            patients[3] = "Lucas Walker";
            patients[4] = "Charlotte Hall";

            // Display using foreach
            Console.WriteLine("Current Registered Patients:");
            foreach (string patient in patients)
            {
                Console.WriteLine("  - " + patient);
            }

            Console.WriteLine("\nWe had to decide maximum 5 patients in advance.");
            Console.WriteLine("Press any key for next example...");
            Console.ReadKey();
        }
    }
}
