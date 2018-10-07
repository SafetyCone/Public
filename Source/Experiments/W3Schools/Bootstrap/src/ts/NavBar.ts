const button1: HTMLButtonElement = document.querySelector("#button1");
const button2: HTMLButtonElement = document.querySelector("#button2");

let button2Enabled: boolean = true;
SetButton2State();

button1.onclick = () => {
    button2Enabled = !button2Enabled;
    SetButton2State();
}

function SetButton2State() {
    if (button2Enabled) {
        button2.classList.remove("disabled");
        button1.textContent = "Disable Btn 2";
    }
    else {
        button2.classList.add("disabled");
        button1.textContent = "Activate Btn 2";
    }
}