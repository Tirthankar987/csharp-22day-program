using System;

namespace ArraysTrainingDemo
{
    public class ArrayExceptionsExample
    {
        public static void Run()
        {
            Console.Clear();
            Console.WriteLine("EXAMPLE 5: Common Array Problems & Safe Code");
            Console.WriteLine("============================================");

            int[] visits = new int[3];  // Only indices 0, 1, 2
            visits[0] = 5;
            visits[1] = 8;
            visits[2] = 3;

            Console.WriteLine("Safe display using Length:");
            for (int i = 0; i < visits.Length; i++)
            {
                Console.WriteLine($"  Patient visits: {visits[i]}");
            }

            Console.WriteLine("\nWarning: visits[3] would cause IndexOutOfRangeException!");
            // Uncomment to see crash:
            // Console.WriteLine(visits[3]);

            Console.WriteLine("\nSafe practice: Always use < array.Length in loops.");

            Console.WriteLine("\nArrays are excellent when size never changes.");
            Console.WriteLine("But when size varies → we need List<T> (coming next!).");

            Console.WriteLine("\nPress any key to finish...");
            Console.ReadKey();
        }
    }
}
