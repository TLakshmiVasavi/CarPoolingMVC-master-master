function ViaPoint() {
    $("#AddViaPoint").hide();
    var formdata = $('form').serialize();
    alert(formdata);
    $.ajax({
        type: 'POST',
        data: formdata,
        url: "/Ride/AddViaPoint",
    }).done(function (res) {
        alert("success" + formdata);
        $("#Partial").html(res);

    }).fail(function (res) {
        alert("Failed" + formdata);
    });
}

function SaveDetails() {
    $("#AddViaPoint").show();
    var formdata = $('form').serialize();
    alert(formdata);
    $.ajax({
        type: 'POST',
        data: formdata,
        url: "/Ride/Save",
    }).done(function (res) {
        alert("success" + formdata);
        $("#ViaPoints").html(res);
        $("#Partial").html("");
    }).fail(function (res) {
        alert("Failed" + formdata);
    });
}
function cancelled() {
    $("#NewViaPoint").remove();
    $("#AddViaPoint").show();
}

function show() {
    $("#VehicleDetails").show();
}
function hide() {
    $("#VehicleDetails").hide();
}
function setVehicle()
{
    if ($("#Vehicle_Type").val() == 0)
    {
        $("#vehicleCapacity").show();
    }
    else
    {
        $("#vehicleCapacity").hide();
        $("#Vehicle_Capacity").val(2);
    }  
}
function setCapacity() {
    if ($("#Type").val() == 0) {
        $("#capacity").show();
    }
    else {
        $("#capacity").hide();
        $("#Capacity").val(2);
    }
}
function setNoOfSeats()
{
    if ($("#VehicleType").val() == 0) {
        $("#noOfSeats").show();
    }
    else {
        $("#noOfSeats").hide();
        $("#NoOfSeats").val(1);
    }  
}