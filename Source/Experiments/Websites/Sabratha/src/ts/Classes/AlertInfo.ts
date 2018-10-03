export class AlertInfo {
    static GetRandomAlertInfo(): AlertInfo {
        let value: number = Math.random() * 100;

        if (value < 25) {
            return new AlertInfo("alert-success", "Success");
        }

        if (value < 50) {
            return new AlertInfo("alert-info", "Info");
        }

        if (value < 75) {
            return new AlertInfo("alert-warning", "Warning");
        }

        return new AlertInfo("alert-danger", "Danger!");
    }


    constructor(public BootstrapType: string, public Name: string) { }
}