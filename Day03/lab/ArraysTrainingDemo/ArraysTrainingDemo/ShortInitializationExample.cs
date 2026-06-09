using System;

namespace ArraysTrainingDemo
{
    public class ShortInitializationExample
    {
        public static void Run()
        {
            Console.Clear();
            Console.WriteLine("EXAMPLE 3: Short Array Initialization");
            Console.WriteLine("=====================================");

            // Short syntax - size determined automatically
            int[] roomNumbers = { 301, 302, 303, 304, 305 };

            // Another example
            string[] shifts = { "Morning", "Evening", "Night" };

            Console.WriteLine("ICU Room Numbers:");
            foreach (int room in roomNumbers)
            {
                Console.WriteLine("  Room: " + room);
            }

            Console.WriteLine("\nShift Schedule:");
            foreach (string shift in shifts)
            {
                Console.WriteLine("  Shift: " + shift);
            }

            Console.WriteLine("\nThis is clean when data is fixed.");
            Console.WriteLine("Press any key for next example...");
            Console.ReadKey();
        }
    }
}
