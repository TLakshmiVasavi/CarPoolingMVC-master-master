$.noConflict();
$(document).ready(function () {
    if (location.pathname == "/" || location.pathname == "/user/login" || location.pathname.toLowerCase() == "/user/signup") {
        $(".user").hide();
    }
    else {
        if (location.pathname == "/Ride/OfferRide/" || location.pathname == "/Ride/OfferRide") {
            GetVehicleDetails();
        }
        if (location.pathname == "/User/GetProfile/" || location.pathname == "/User/GetProfile")
        {
            GetProfile();
        }
        $(".user").show();
    }

    function GetVehicleDetails() {
        alert("getting");
        $.ajax({
            type: "GET",
            url: "https://localhost:5001/api/UserApi/GetVehicles?userId=" + document.cookie,
            contentType: "application/json",
            success: function (data) {
                console.log(data);
                let dropdown = $("#Vehicles");
                if (data.length == 0) {
                    window.location.href = "/User/AddVehicle";
                }
                else {
                    $.each(data, function (index,item) {
                        localStorage.setItem(item.number, item.capacity);
                        let option = $('<option></option>');
                        option.val(item.number);
                        option.html(item.number);
                        dropdown.append(option);
                    });
                    for (i = 1; i <= localStorage.getItem($("#Vehicles").val()); i++) {
                        $("#NoOfSeats").append(" <button type='button' class='btn round-button'>" + i + "</button>");
                    }
                }
            },
            error: function (e) {
                alert("error");
            }
        })
    }

    $(document).on('change', "#Vehicles", function () {
        $("#NoOfSeats").empty();
        for (i = 1; i <= localStorage.getItem($("#Vehicles").val()); i++) {
            $("#NoOfSeats").append(" <button type='button' class='btn round-button'>" + i + "</button>");
        }
    })

    
        $(".form-group .form-control").blur(function () {
            if ($(this).val() != "") {
                $(this).addClass('active').siblings().find("label").addClass('active');
                $(this).addClass('active');
                //$(this).sibilings().find("label").addClass('active');

            } else {
                $(this).sibilings("label").removeClass('active');
            }
        });
    

    function GetBalance() {
        $.ajax({
            type: 'GET',
            url: "https://localhost:5001/api/UserApi/GetBalance?userId=" + document.cookie("userId"),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (e) {
                $.each(e, function (ele) {
                    if (document.getElementById(ele) != null) {
                        $('label[for="' + ele + '"]')[0].textContent = e[ele];
                    }
                });
            },
            error: function (e) {
                $("#ErrorDetails").html(e);
                window.location.href = "/Home/Error";
            }
        });
    }

    function GetProfile() {
        $.ajax({
            type: 'GET',
            url: "https://localhost:5001/api/UserApi/GetUser?userId=" + document.cookie,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (e) {
                console.log(e);
                $.each(e, function (name,value) {
                    $("#" + name.charAt(0).toUpperCase() + name.substr(1)).val(value);
                });
                $("#UserProfile :input").prop("disabled", true);
                $("#edit").prop("disabled", false);
            },
            error: function (e) {
                console.log(e);
                window.location.href = "/Home/Error";
            }
        });
    }

    $("#RideRequests").on('load', function () {
        alert("ride request");
        $.ajax({
            type: "GET",
            data: JSON.stringify(user),
            url: "https://localhost:5001/api/RideApi/ViewRequests?rideId=" + rideId,
            contentType: "application/json",
            success: function (data) {
                alert("success");
                $.each(data, function (index, item) {
                    $("#Requests").append(
                        '<div class="col-md-4 p-3 mb-5 bg-white rounded shadow" style="left:4%">' +
                        '<div class="row" style="margin-bottom:20px">' +
                        '<div class="col-md-8">' +
                        '<h2>' + item.Id + '</h2>' +
                        '</div>' +
                        '< div class="col-md-4" style = "top:12px" > ' +
                        '< img src = "#" /> ' +
                        '</div > ' +
                        '</div > ' +
                        '< div class="row" > ' +
                        '< div class="row" > ' +
                        '< div class="col-md-4" > ' +
                        '< small > From</small > ' +
                        '< p >' + item.from + '</p > ' +
                        '</div > ' +
                        '< div class="col-md-4" > ' +
                        '< div class="dot darkviolet" ></div > ' +
                        '< div class="dot" ></div > ' +
                        '< div class="dot" ></div > ' +
                        '< div class="dot" ></div > ' +
                        '< span class="glyphicon glyphicon-map-marker darkviolet" ></span > ' +
                        '</div > ' +
                        '< div class="col-md-4" > ' +
                        '< small > To</small > ' +
                        '< p >' + item.To + '</p > ' +
                        '</div > ' +
                        '</div > ' +
                        '< div class="row" > ' +
                        '< div class="col-md-4" > ' +
                        '< small > Cost</small > ' +
                        '< p >' + item.Cost + '</p > ' +
                        '</div > ' +
                        '< div class="col-md-4 col-md-offset-4" > ' +
                        '< small > Seats </small > ' +
                        '< p >' + item.NoOfSeats + '</p > ' +
                        '</div > ' +
                        '</div > ' +
                        '< div class="row" > ' +
                        '< div class="col-md-4" > ' +

                        '</div > ' +
                        '< div class="col-md-4 col-md-offset-4" > ' +

                        '</div > ' +
                        '</div > ' +
                        '</div > ' +
                        '</div > ')

                });
            },
            error: function (e) {
                console.log(e)
            }

        });
    });

    $("#OfferedRides").on('load', function () {
        $.ajax({
            type: "GET",
            url: "https://localhost:5001/api/RideApi/GetOfferedRides?userId=" + document.cookie,
            contentType: "application/json",
            success: function (data) {
                if (data == null) {
                    $("#Offered").append(
                        '<h3>No Rides</h3>'
                    );
                }
                $.each(data, function (index, item) {
                    $("#Offered").append(
                        '<button type="submit" name="RideId" value=' + item.id + '>' +
                        '<div class="col-md-4 p-3 mb-5 bg-white rounded shadow" >' +
                        '<div class="row">' +
                        '<div class="row">' +
                        '<div class="col-md-4">' +
                        '<small>From</small>' +
                        '<p>' + item.route.from + '</p>' +
                        '</div>' +
                        '<div class="col-md-4">' +
                        '<div class="dot darkviolet"></div>' +
                        '<div class="dot"></div>' +
                        '<div class="dot"></div>' +
                        '<span class="glyphicon glyphicon-map-marker darkviolet"></span>' +
                        '</div>' +
                        '<div class="col-md-4">' +
                        '<small>To</small>' +
                        '<p>' + item.route.To + '</p>' +
                        '</div>' +
                        '</div>' +
                        '<div class="row">' +
                        '<div class="col-md-4">' +
                        '<small>Date</small>' +
                        '<p>' + getDate(item.StartDate) + '</p>' +
                        '</div>' +
                        '<div class="col-md-4 col-md-offset-4">' +
                        '<small>Ride Status</small>' +
                        '</div>' +
                        '</div>' +
                        '<div class="row">' +
                        '<div class="col-md-4">' +
                        '<small>Vehicle</small>' +
                        '<p>' + item.vehicleId + '</p>' +
                        '</div>' +
                        '<div class="col-md-4 col-md-offset-4">' +
                        '<small>Seats Offered</small>' +
                        '<p>' + item.noOfOfferedSeats + '</p>' +
                        '</div>' +
                        '</div>' +
                        '</div>' +
                        '</div>' +
                        '</button>');
                });
            },
            error: function (e) {
                alert("error");
            }
        });
    });
});