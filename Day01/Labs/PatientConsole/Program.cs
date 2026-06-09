


//Console.Write("Enter patient weight (kg): ");

//string? raw = Console.ReadLine();

//try
//{
//    double weight = double.Parse(raw!);   // throws if not a number

//    if (weight <= 0)
//        throw new ArgumentException("Weight must be positive.");

//    Console.WriteLine($"Recorded weight: {weight} kg");
//}
//catch (FormatException)
//{
//    Console.WriteLine("Input was not a valid number.");
//}
//catch (ArgumentException ex)
//{
//    Console.WriteLine($"Invalid value: {ex.Message}");
//}
//finally
//{
//    Console.WriteLine("Weight entry step complete.");
//}

//Json

using Newtonsoft.Json;

//var patient = new
//{
//    Id = 1001,
//    Name = "A. Sharma",
//    Active = true
//};

//string json = JsonConvert.SerializeObject(patient, Formatting.Indented);

//Console.WriteLine(json);
using Bogus;   // NuGet package used to generate realistic fake data

namespace PatientConsole;

// Model representing a patient in our healthcare system
public class Patient
{
    public int PatientId { get; set; }
    public string Name { get; set; } = "";
    public int Age { get; set; }
    public string City { get; set; } = "";
    public bool Active { get; set; }

}

internal class Program
{
    static void Main()
    {
        // Create a fake-data generator for the Patient class
        // Faker<T> comes from the Bogus NuGet package
        var patientGenerator = new Faker<Patient>()

            // Generate a random PatientId between 1000 and 9999
            .RuleFor(p => p.PatientId,
                f => f.Random.Number(1000, 9999))

            // Generate a realistic full name
            .RuleFor(p => p.Name,
                f => f.Name.FullName())

               .RuleFor(p => p.Age,
                f => f.Random.Number(18, 90))


            // Generate a realistic city name
            .RuleFor(p => p.City,
                f => f.Address.City())

            // Randomly assign Active = true or false
            .RuleFor(p => p.Active,
                f => f.Random.Bool());
       
        // Generate 500 fake patient records
        // Generate() is a method provided by Bogus
        var patients = patientGenerator.Generate(500);

        Console.WriteLine($"Generated {patients.Count} fake patients");
        Console.WriteLine();

        // Display only the first 10 records
        // Imagine these 1000 records being used for testing
        foreach (var patient in patients.Take(10))
        {
            Console.WriteLine(
                $"ID: {patient.PatientId} | " +
                $"Name: {patient.Name} | " +
                $"Age: {patient.Age} | " +
                $"City: {patient.City} | " +
                $"Active: {patient.Active}");
        }

        Console.WriteLine();
        Console.WriteLine("Remaining 990 records were generated but not displayed.");
        //string json = JsonConvert.SerializeObject(patients, Formatting.Indented);

      


    }
}
