function connectToSignalR() {
    console.log("connecting to SignalR...");
    var conn = new signalR.HubConnectionBuilder().withUrl("/hub").build();
    conn.on("DoSomethingReallyReallyCool", displayNewVehicleNotification);
    conn.start().then(function () {
        console.log("SignalR started! Yay!");
    }).catch(function (err) {
        console.log("Something went wrong starting SignalR:");
        console.log(err);
    });
}

function displayNewVehicleNotification(user, json) {
    const data = JSON.parse(json);
    console.log(data);
    const $div = $("#signalr-notifications");
    const $alert = $(`
<div>NEW CAR ALERT!
${data.ManufacturerName} ${data.ModelName} (${data.Color}, ${data.Year})
Price: ${data.Price} ${data.CurrencyCode}
<a href="/vehicles/details/${data.Registration}">Click here for more info...</a>
</div>`);
    $div.prepend($alert);
    window.setTimeout(function() {
        $alert.fadeOut(2000, function() { $alert.remove(); });
    }, 5000);
}

$(document).ready(connectToSignalR);

/*
 * {"Price":12345,"CurrencyCode":"EUR","Registration":"SIGNALR3","ManufacturerName":"DMC","ModelName":"DELOREAN","Year":0,"Color":"Silver","ListedAt":"0001-01-01T00:00:00+00:00"}
 * */