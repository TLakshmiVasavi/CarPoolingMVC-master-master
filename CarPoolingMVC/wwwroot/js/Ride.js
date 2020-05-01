$(document).ready(function () {
    $("#second").hide();
});

$(document).on('change', ".checkbox", function () {
    var url;
        if (this.checked) {
        url = "/Ride/BookRide/";
    }
        else {
        url = "/Ride/OfferRide/";
}
window.location.href = url;
});

$(document).on('click', ".btn-group>.btn", function () {
    $(this).addClass('active').siblings().removeClass('active');
    $(this).addClass("active");
});

$(document).ready(function () {

    $(".form-group .form-control").blur(function () {
        if ($(this).val() != "") {
            $(this).addClass('active');
            $(this).sibilings().find("label").addClass('active');

        } else {
            $(this).sibilings("label").removeClass('active');
        }
    });
});

$(document).on('click', ".next", function () {
    $("#first").hide();
    $("#second").show();
});

function AddStop()
{
    var x = $("#count").val();
    x++;
    alert(x);
    var div = document.createElement("div");
    var label = document.createElement("label");
    var input = document.createElement("input");
    var inputgroup = document.createElement("div");
    var a = document.createElement("a");
    var i = document.createElement("i");
    var span = document.createElement("span");
    div.id = "stop" + x;
    a.classList.add("add_input");
    div.classList.add("form-group");
    label.classList.add("control-label");
    label.innerHTML = "Stop " + x;
    label.setAttribute("for", "Route_ViaPoints_" + x + "__Location");
    input.classList.add("form-control");
    input.type = "text";
    input.setAttribute("data-val", "true");
    input.setAttribute("data-val-required", "The Location field is required.");
    input.id = "Route_ViaPoints_" + x + "__Location";
    input.name = "Route.ViaPoints[" + x + "].Location";
    input.value = "";
    inputgroup.classList.add("input-group");
    i.classList.add("glyphicon");
    i.classList.add("glyphicon-plus");
    span.classList.add("text-danger");
    span.classList.add("field-validation-valid");
    span.setAttribute("data-valmsg-for", "Route.ViaPoints[" + x + "].Location");
    span.setAttribute("data-valmsg-replace", "true");
    var inputgroupaddon = document.createElement("div");
    inputgroupaddon.classList.add("input-group-addon");
    a.appendChild(i);
    inputgroup.appendChild(input);
    inputgroup.appendChild(inputgroupaddon);
    inputgroupaddon.appendChild(a);
    div.appendChild(label);
    div.appendChild(inputgroup);
    div.appendChild(span);
    $("#stops").append(div);   // Append new elements
    $("#count").val(x);
    if (x == 1) {
        $("#add_stop").hide();
    } else {
        $("#stop" + (x - 1)).children("div").children("div").remove()
    }
}

$(document).on('click', ".input-group-addon", AddStop);

$(document).on('click', "#add_stop", AddStop);

//
function addDots() {
    var count = Math.floor(($("#stops").outerHeight() - $(".dot").outerHeight() - $(".glyphicon-map-marker").outerHeight()) / ($(".dot").outerHeight()));
//    $(".dots").outerHeight($("#stops").outerHeight());
    for (var i = 0; i < count; i++) {
        $(".dots").insertBefore(".glyphicon", '<div class="dot"></div>')
    }
}

$(document).on('change', "#HasVehicle", function () {
    if ($("#HasVehicle").val()=="1")
    {
        $("#VehicleDetails").show();
    }
    else {
        $("#VehicleDetails").hide();
    }
});

$(document).on('change', "#VehicleType", function setVehicle() {
    if ($("#Vehicle_Type").val() == 0) {
        $("#vehicleCapacity").show();
    }
    else {
        $("#vehicleCapacity").hide();
        $("#Vehicle_Capacity").val(2);
    }
});

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

function getDate(date) {
    var d = new Date(date);
    var date = d.getDate();
    var month = d.getMonth() + 1;
    var year = d.getFullYear();
    var dateStr = date + "/" + month + "/" + year;
    return dateStr;
}