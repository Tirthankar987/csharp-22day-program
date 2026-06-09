//using System;
//using System.Collections.Generic;
//// Measure current managed memory used by the application.
//// &#39;true&#39; asks the CLR to perform a collection before measuring.
//long before = GC.GetTotalMemory(true);
//Console.WriteLine($" Memory Before Allocation: { before / 1024}KB");

//// Create an empty list that will hold patient names.
//var patients = new List<string>();
//// Create 100,000 patient records.
//// The underscore (_) is a digit separator for readability.
//for (int i = 0; i < 50_000; i++)
//{
//    patients.Add($"Patient -{i}");

//}
//// Measure memory again after creating the objects.
//long after = GC.GetTotalMemory(true);
//Console.WriteLine($" Memory After Allocation: { after / 1024}KB");
//// Calculate approximately how much additional memory was allocated.
//Console.WriteLine($" Allocated Approx: { (after - before) / 1024}KB");
//// Remove the reference to the list.
//// The objects are NOT deleted here.
//// They simply become eligible for garbage collection.
//patients = null;
//// Request the Garbage Collector to run.
//// In real production applications, developers rarely call this directly.
//GC.Collect();
//// Wait for any pending finalizers to complete.
//GC.WaitForPendingFinalizers();
//// Run GC again to ensure cleanup has completed.
//GC.Collect();
//// Measure memory after garbage collection.
//long cleaned = GC.GetTotalMemory(true);
//Console.WriteLine($" Memory After Cleanup: { cleaned / 1024}KB");
//// Compare current memory usage with the starting point.
//Console.WriteLine($" Difference From Start: { (cleaned - before) / 1024}KB");


using System;
using System.Collections.Generic;

Console.WriteLine("=== Garbage Collection Demo ===");

long before = GC.GetTotalMemory(true);

Console.WriteLine($"Memory Before Allocation: {before / 1024} KB");

CreatePatients();

GC.Collect();
GC.WaitForPendingFinalizers();
GC.Collect();

long after = GC.GetTotalMemory(true);

Console.WriteLine($"Memory After Cleanup: {after / 1024} KB");
Console.WriteLine($"Difference From Start: {(after - before) / 1024} KB");

static void CreatePatients()
{
    var patients = new List<string>();

    for (int i = 0; i < 50_000; i++)
    {
        patients.Add($"Patient-{i}");
    }

    long during = GC.GetTotalMemory(true);

    Console.WriteLine($"Memory During Allocation: {during / 1024} KB");
}



