// =====================================================
// JavaScript– Hospital Front Desk Simulation
// Covers:
// Variables, Objects, Arrays, Functions, Conditions, Loops
// =====================================================


// -------------------------------
// VARIABLES
// -------------------------------
// Variables store simple values in memory
let hospitalName = "City Care Hospital";
let todayAppointmentCount = 3;


// -------------------------------
// OBJECT
// -------------------------------
// Object represents a real-world entity (Patient)
let patient = {
    name: "Anita Sharma",
    age: 62,
    hasInsurance: true
};


// -------------------------------
// ARRAY
// -------------------------------
// Array stores a list of doctors
let doctors = [
    "Dr. Mehta (Cardiology)",
    "Dr. Sharma (Orthopedic)",
    "Dr. Khan (General Physician)"
];


// -------------------------------
// FUNCTION + CONDITION
// -------------------------------
// Function checks if patient can book appointment
function isPatientEligible(age) {

    // Condition checks business rule
    if (age >= 18) {
        return "YES";
    } else {
        return "NO";
    }
}

// -------------------------------
// LOOP
// -------------------------------
// Loop through doctors list and format it
function getDoctorList() {

    let doctorList = "";

    // For loop processes multiple values
    for (let i = 0; i < doctors.length; i++) {
        doctorList += doctors[i] + "<br>";
    }

    return doctorList;
}


// -------------------------------
// MAIN FUNCTION (DOM Manipulation)
// -------------------------------
// This function is called when button is clicked
function runDemo() {

    // Get reference to output div from HTML
    let outputDiv = document.getElementById("output");

    // Call function to check eligibility
    let eligibility = isPatientEligible(patient.age);

    // Dynamically write HTML content to the page
    outputDiv.innerHTML = `
        <h3>Hospital Information</h3>
        <b>Hospital Name:</b> ${hospitalName}<br>
        <b>Today's Appointments:</b> ${todayAppointmentCount}<br><br>

        <h3>Patient Details</h3>
        <b>Name:</b> ${patient.name}<br>
        <b>Age:</b> ${patient.age}<br>
        <b>Has Insurance:</b> ${patient.hasInsurance}<br>
        <b>Eligible for Appointment:</b> ${eligibility}<br><br>

        <h3>Available Doctors</h3>
        ${getDoctorList()}
    `;
}
