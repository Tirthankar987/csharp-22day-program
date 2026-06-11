// -----------------------------
// BED DATA (Mock backend data)
// -----------------------------
let beds = [
    { bedNumber: 1, isOccupied: false },
    { bedNumber: 2, isOccupied: true },
    { bedNumber: 3, isOccupied: false },
    { bedNumber: 4, isOccupied: true },
    { bedNumber: 5, isOccupied: false },
    { bedNumber: 6, isOccupied: false },
    { bedNumber: 7, isOccupied: true },
    { bedNumber: 8, isOccupied: false },
    { bedNumber: 9, isOccupied: true },
    { bedNumber: 10, isOccupied: false },
    { bedNumber: 11, isOccupied: false },
    { bedNumber: 12, isOccupied: false }
];


// -----------------------------
// FUNCTION: Render beds on screen
// -----------------------------
function renderBeds() {

    let container = document.getElementById("bedContainer");
    let counter = document.getElementById("counter");

    // Clear existing beds
    container.innerHTML = "";
    let count = 0;
    //counter.innerHTML = "";
    //container.innerHTML += "Total" + beds.length;

    // Loop through all beds
    for (let i = 0; i < beds.length; i++) {

        let bed = beds[i];

        // Create a div for each bed
        let bedDiv = document.createElement("div");

        // Assign common bed class
        bedDiv.classList.add("bed");

        // Condition to decide color
        if (bed.isOccupied) {
            bedDiv.classList.add("occupied");
            count++;
            bedDiv.innerText = "Bed " + bed.bedNumber + "\nOccupied";

        } else {
            bedDiv.classList.add("available");
            //total++;
            bedDiv.innerText = "Bed " + bed.bedNumber + "\nAvailable";
        }

        //counter.InnerHTML+ = beds.length;
        // Click event to toggle bed status
        bedDiv.onclick = function () {
            if (!bed.isOccupied) {
                bed.isOccupied = !bed.isOccupied;
                renderBeds(); // Re-render UI
            }

        };

        // Add bed to container

        container.appendChild(bedDiv);

    }
   
    counter.innerHTML = `
        Total Beds: ${beds.length}
        Occupied: ${count} 
       `;
}


// -----------------------------
// INITIAL LOAD
// -----------------------------
renderBeds();
