// -----------------------------
// BED DATA (Mock backend)
// -----------------------------
let beds = [
    { bedNumber: 1, patientName: "Anita Sharma", heartRate: 78, spo2: 98 },
    { bedNumber: 2, patientName: "Ramesh Kumar", heartRate: 110, spo2: 92 },
    { bedNumber: 3, patientName: "Sunita Verma", heartRate: 130, spo2: 85 },
    { bedNumber: 4, patientName: "Amit Singh", heartRate: 82, spo2: 96 },
    { bedNumber: 5, patientName: "Neha Joshi", heartRate: 105, spo2: 90 }
];


// -----------------------------
// FUNCTION: Determine bed status
// -----------------------------
function getBedStatus(heartRate, spo2) {

    if (heartRate > 120 || spo2 < 90) {
        return "critical";
    }

    if (heartRate > 100 || spo2 < 95) {
        return "observation";
    }

    return "stable";
}


// -----------------------------
// FUNCTION: Simulate live vitals
// -----------------------------
function updateVitalsRandomly() {

    for (let i = 0; i < beds.length; i++) {

        // Random heart rate change (-5 to +5)
        beds[i].heartRate += Math.floor(Math.random() * 11) - 5;

        // Keep heart rate within realistic limits
        if (beds[i].heartRate < 60) beds[i].heartRate = 60;
        if (beds[i].heartRate > 150) beds[i].heartRate = 150;

        // Random SpO2 change (-2 to +2)
        beds[i].spo2 += Math.floor(Math.random() * 5) - 2;

        // Keep SpO2 within realistic limits
        if (beds[i].spo2 < 80) beds[i].spo2 = 80;
        if (beds[i].spo2 > 100) beds[i].spo2 = 100;
    }
}


// -----------------------------
// FUNCTION: Render beds on screen
// -----------------------------
function renderBeds() {

    let container = document.getElementById("bedContainer");
    container.innerHTML = "";

    for (let i = 0; i < beds.length; i++) {

        let bed = beds[i];
        let bedDiv = document.createElement("div");

        bedDiv.classList.add("bed");

        // Determine bed status using vitals
        let status = getBedStatus(bed.heartRate, bed.spo2);
        bedDiv.classList.add(status);

        bedDiv.innerHTML = `
            Bed ${bed.bedNumber}
            <div class="tooltip">
                <b>Patient:</b> ${bed.patientName}<br>
                <b>Heart Rate:</b> ${bed.heartRate} bpm<br>
                <b>SpO₂:</b> ${bed.spo2} %
            </div>
        `;

        container.appendChild(bedDiv);
    }
}


// -----------------------------
// LIVE UPDATE LOOP (Every 5 sec)
// -----------------------------
function startLiveMonitoring() {

    // Initial render
    renderBeds();

    // Update vitals + UI every 5 seconds
    setInterval(function () {

        updateVitalsRandomly(); // simulate device input
        renderBeds();           // refresh UI

    }, 5000);
}


// -----------------------------
// START MONITORING ON PAGE LOAD
// -----------------------------
startLiveMonitoring();


