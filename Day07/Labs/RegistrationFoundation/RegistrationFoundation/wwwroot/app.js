const doctorAvailability = {
    "Dr. Mehta": true,
    "Dr. Sharma": false,
    "Dr. Khan": true
};

document.getElementById("checkBtn").addEventListener("click", function () {
    const doctor = document.getElementById("doctor").value;
    if (doctor === "") { alert("Please select a doctor first"); return; }
    alert(doctor + (doctorAvailability[doctor] ? " is AVAILABLE" : " is NOT available"));
});

document.getElementById("patientForm").addEventListener("submit", function (e) {
    e.preventDefault();
    const name = document.getElementById("name").value;
    const email = document.getElementById("email").value;
    const out = document.getElementById("output");
    out.style.display = "block";
    out.innerHTML = `<h3>Registered</h3><p>${name} - ${email}</p>`;
});
