import { AlertInfo } from "./Classes/AlertInfo";


const alertList: HTMLDivElement = document.querySelector("#AlertList");

const addAlertButton: HTMLButtonElement = document.querySelector("#AddAlertButton");
addAlertButton.onclick = () => {
    let newAlert = CreateAlert();
    alertList.appendChild(newAlert);
}

const deleteAlertsButton: HTMLButtonElement = document.querySelector("#DeleteAlertsButton");
deleteAlertsButton.onclick = () => {
    let numberOfAlerts = alertList.childNodes.length;
    for (let index = 0; index < numberOfAlerts; index++) {
        alertList.removeChild(alertList.children[0]);
    }
}

function CreateAlert(): HTMLDivElement {
    let alertInfo: AlertInfo = AlertInfo.GetRandomAlertInfo();

    let newAlert: HTMLDivElement = document.createElement("div");
    newAlert.classList.add("alert");
    newAlert.classList.add("alert-dismissable");
    newAlert.classList.add(alertInfo.BootstrapType);
    
    let closeLink: HTMLAnchorElement = document.createElement("a");
    closeLink.href = "#";
    closeLink.classList.add("close");
    closeLink.setAttribute("data-dismiss", "alert");
    // let closeLinkDataDismissAttr: Attr = document.createAttribute("data-dismiss");
    // closeLinkDataDismissAttr.value = "alert";
    // closeLink.attributes.setNamedItem(closeLinkDataDismissAttr);
    closeLink.innerHTML = "&times;";
    newAlert.appendChild(closeLink);

    let paragraph: HTMLParagraphElement = document.createElement("p");
    paragraph.innerHTML = `<strong>${alertInfo.Name} </strong> An alert was successfully added.`;

    newAlert.appendChild(paragraph);

    return newAlert;
}

function CreateAlertUsingTimer(): void {
    setTimeout(() => {
        CreateAlertUsingTimer();
    }, 2000);

    let newAlert = CreateAlert();
    alertList.appendChild(newAlert);
}

CreateAlertUsingTimer();