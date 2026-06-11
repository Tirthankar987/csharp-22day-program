// ===============================================
// Live Mother & Fetus Heart Rate Chart (Canvas)
// ===============================================

// Canvas setup
const canvas = document.getElementById("heartChart");
const ctx = canvas.getContext("2d");

// Chart configuration
const MAX_POINTS = 60;   // visible points on screen
const UPDATE_INTERVAL = 1000; // 1 second

// Heart rate arrays
let motherData = [];
let fetusData = [];

// Initial values
let motherHR = 75;
let fetusHR = 140;

// Thresholds
const MOTHER_MIN = 60;
const MOTHER_MAX = 100;

const FETUS_MIN = 120;
const FETUS_MAX = 160;

// Utility functions
function randomChange(range) {
    return Math.floor(Math.random() * (range * 2 + 1)) - range;
}

function clamp(value, min, max) {
    return Math.min(Math.max(value, min), max);
}

// Generate next data point
function updateVitals() {
    motherHR = clamp(motherHR + randomChange(3), 50, 140);
    fetusHR = clamp(fetusHR + randomChange(5), 100, 190);

    motherData.push(motherHR);
    fetusData.push(fetusHR);

    if (motherData.length > MAX_POINTS) motherData.shift();
    if (fetusData.length > MAX_POINTS) fetusData.shift();
}

// Draw chart
function drawChart() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);

    drawThreshold(MOTHER_MIN, "#555");
    drawThreshold(MOTHER_MAX, "#555");
    drawThreshold(FETUS_MIN, "#444");
    drawThreshold(FETUS_MAX, "#444");

    drawLine(motherData, "#00ff6a"); // Mother
    drawLine(fetusData, "#00bfff");  // Fetus
}

// Draw a line
function drawLine(data, color) {
    ctx.beginPath();
    ctx.strokeStyle = color;
    ctx.lineWidth = 2;

    data.forEach((value, index) => {
        const x = (index / MAX_POINTS) * canvas.width;
        const y = canvas.height - (value / 200) * canvas.height;

        if (index === 0) ctx.moveTo(x, y);
        else ctx.lineTo(x, y);
    });

    ctx.stroke();
}

// Draw threshold line
function drawThreshold(value, color) {
    const y = canvas.height - (value / 200) * canvas.height;
    ctx.beginPath();
    ctx.strokeStyle = color;
    ctx.setLineDash([5, 5]);
    ctx.moveTo(0, y);
    ctx.lineTo(canvas.width, y);
    ctx.stroke();
    ctx.setLineDash([]);
}

// Update alert text
function updateStatus() {
    const motherStatus = document.getElementById("motherStatus");
    const fetusStatus = document.getElementById("fetusStatus");

    motherStatus.className = "";
    fetusStatus.className = "";

    if (motherHR < MOTHER_MIN || motherHR > MOTHER_MAX) {
        motherStatus.textContent = `Mother: ALERT (${motherHR} bpm)`;
        motherStatus.classList.add("critical");
    } else {
        motherStatus.textContent = `Mother: Stable (${motherHR} bpm)`;
    }

    if (fetusHR < FETUS_MIN || fetusHR > FETUS_MAX) {
        fetusStatus.textContent = `Fetus: ALERT (${fetusHR} bpm)`;
        fetusStatus.classList.add("critical");
    } else {
        fetusStatus.textContent = `Fetus: Stable (${fetusHR} bpm)`;
    }
}

// Main loop
setInterval(() => {
    updateVitals();
    drawChart();
    updateStatus();
}, UPDATE_INTERVAL);
