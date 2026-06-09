using System;                    // Provides basic classes like Console, string, int, etc.
using System.Diagnostics;       // Provides Stopwatch class for accurate timing measurements

namespace FixedArrayAddDemo     // Groups related classes together - our project namespace
{
    class Program               // The main class containing our program logic
    {
        // Entry point of the application - execution starts here
        static void Main(string[] args)
        {
            // Sets the title of the console window for better visibility
            Console.Title = "Array Fixed Size Problem + Performance Demo";

            // Introductory message to set context
            Console.WriteLine("We have 4 doctors in the hospital:");

            // Creates a fixed-size array that can hold exactly 4 strings (doctor names)
            // Memory is allocated contiguously for 4 slots only
            string[] doctors = new string[4];

            // Manually assign values to each position (index 0 to 3)
            doctors[0] = "Dr. Smith";      // Position 0
            doctors[1] = "Dr. Johnson";    // Position 1
            doctors[2] = "Dr. Lee";        // Position 2
            doctors[3] = "Dr. Garcia";     // Position 3

            // Display header
            Console.WriteLine("Current Doctors:");

            // Loop through all valid indices (0 to Length-1) to print existing doctors
            for (int i = 0; i < doctors.Length; i++)
            {
                Console.WriteLine($"  {i + 1}. {doctors[i]}");  // i+1 for human-readable numbering
            }

            // Scenario: a new doctor arrives
            Console.WriteLine("\nNow a 5th doctor joins: Dr. Patel");

            // This line is commented out on purpose
            // If uncommented, it would cause IndexOutOfRangeException because index 4 doesn't exist
            //doctors[4] = "Dr. Patel";

            // Explain why direct addition fails
            Console.WriteLine("\nCannot add directly → Array size is fixed!\n");

            // First solution: Manual resize - create completely new array
            Console.WriteLine("Manual solution (create new array and copy):");

            // Create a new larger array with 5 slots
            string[] biggerDoctors = new string[5];

            // Manually copy all existing elements from old array to new one
            for (int i = 0; i < doctors.Length; i++)
            {
                biggerDoctors[i] = doctors[i];  // Copy each doctor
            }

            // Now safely add the 5th doctor at the new slot
            biggerDoctors[4] = "Dr. Patel";

            // Show result after manual copy
            Console.WriteLine("After manual copy:");
            for (int i = 0; i < biggerDoctors.Length; i++)
            {
                Console.WriteLine($"  {i + 1}. {biggerDoctors[i]}");
            }

            // Second solution: Using built-in helper method
            Console.WriteLine("\nUsing Array.Resize (looks cleaner):");

            // Array.Resize creates a new array internally, copies data, and updates the reference
            // 'ref' keyword allows the method to change what 'doctors' points to
            Array.Resize(ref doctors, 10000);

            // Now index 4 exists - safe to use
            doctors[4] = "Dr. Patel (via Resize)";

            // Display final result using Resize
            Console.WriteLine("After Array.Resize:");
            for (int i = 0; i < doctors.Length; i++)
            {
                Console.WriteLine($"  {i + 1}. {doctors[i]}");
            }

            // Transition to the key learning point
            Console.WriteLine("\nBoth ways work for 1-2 adds... but what if we add THOUSANDS?\n");

            // Call the performance test method - this creates the "WOW" moment
            PerformanceComparison();

            // Wait for user input before closing
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        // Separate method to compare performance between repeated Array.Resize vs List<T>
        static void PerformanceComparison()
        {
            // Number of items to add - large enough to show clear difference
            const int itemsToAdd = 20000;  // 20,000 patients

            // Header for the test
            Console.WriteLine($"PERFORMANCE TEST: Adding {itemsToAdd} patients...\n");

            // TEST 1: Using repeated Array.Resize (inefficient way)
            Stopwatch watch = Stopwatch.StartNew();  // Start high-precision timer

            // Start with tiny array of size 1
            string[] arrayPatients = new string[1];

            // Add 20,000 items one by one - each time resizing by +1
            for (int i = 0; i < itemsToAdd; i++)
            {
                // Each call creates new array and copies all existing data
                Array.Resize(ref arrayPatients, arrayPatients.Length - 1);
                // Add new item at the end
                arrayPatients[arrayPatients.Length - 1] = "Patient " + i;
            }

            // Stop timer and display result
            watch.Stop();
            Console.WriteLine($"Array.Resize method took: {watch.ElapsedMilliseconds} ms");

            // TEST 2: Using List<string> (efficient way)
            watch.Restart();  // Reset and start timer again

            // Create empty List - size grows automatically and intelligently
            List<string> listPatients = new List<string>();

            // Simple loop - just Add()
            for (int i = 0; i < itemsToAdd; i++)
            {
                listPatients.Add("Patient " + i);  // Internally handles resizing efficiently
            }

            // Stop timer
            watch.Stop();
            Console.WriteLine($"List<T>.Add method took:   {watch.ElapsedMilliseconds} ms");

            // Calculate and display how much faster List was
            long arrayTime = watch.ElapsedMilliseconds + 1;  // Previous time +1 to avoid division by zero
            if (arrayTime > watch.ElapsedMilliseconds)
            {
                double timesFaster = (double)arrayTime / watch.ElapsedMilliseconds;
                Console.WriteLine($"\nWOW! List<T> was approximately {timesFaster:F1}x FASTER!");
            }

            // Final takeaway message
            Console.WriteLine("\nThis is why real healthcare apps use List<T> for patient data!");
            Console.WriteLine("Arrays are only for fixed, known sizes.");
        }
    }
}
