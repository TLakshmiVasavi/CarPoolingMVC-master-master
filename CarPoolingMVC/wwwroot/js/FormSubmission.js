$(document).ready(function () {

        $("#SignUpForm").submit(function () {
        event.preventDefault();
        alert("starting...")
        var url = $(this).attr("action");
        $.ajax({
            url: "https://localhost:5001/api/UserApi/SignUp",
            type: "POST",
            //dataType: "JSON",
            data: new FormData(this),
            processData: false,
            contentType: false,
            success: function (s) {
                console.log(s)
                alert("success");
            },
            error: function (e) {
                console.log(e)
                alert("error");
            }
        });
    });

    $("#LoginForm").on('submit', function (e) {
        e.preventDefault();
        alert("Login");
        var formData = new FormData(document.querySelector("form"));
        var user = {};
        for (var [key, value] of formData.entries()) {
            user[key] = value;
        }
        $.ajax({
            type: "POST",
            data: JSON.stringify(user),
            processData: false,
            url: "https://localhost:5001/api/UserApi/Login",
            contentType: "application/json",
            dataType:"json",
            success: function (s) {
                //console.log(s)
                alert("success");
                //window.location.href = "/Home/Index";
            },
            error: function (e) {
                //$.each(e, function (x) {
                //    console.log(x + " " + e[x])
                //});
                //console.log(e)
                alert("error");
            }
        });
    });

    $("#Balance").on('load',function () {
        alert("Balance");
        $.ajax({
            type: 'GET',
            url: "https://localhost:5001/api/UserApi/GetBalance?userId=tlakshmivasavi005@gmail.com",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (e) {
                $.each(e, function (ele) {
                    if (document.getElementById(ele) != null) {
                        $('label[for="' + ele + '"]')[0].textContent = e[ele];
                    }
                });
                alert("success");
            },
            error: function (e) {
                console.log(e);
                alert("error");
            }
        });
    });

    $("#UserProfile").on('load',function () {
        alert("User Profile");
            $.ajax({
                type: 'GET',
                url: "https://localhost:5001/api/UserApi/GetUser?userId=tlakshmivasavi005@gmail.com",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (e) {
                    $.each(e, function (ele) {
                        if (document.getElementById(ele) != null) {
                            $('label[for="' + ele + '"]')[0].textContent = e[ele];
                        }
                    });
                    alert("success");
                },
                error: function (e) {
                    console.log(e);
                    alert("error");
                }
            });
    });

    $("#UserProfile").on('submit', function () {
        alert("userProfile");
        $.ajax({
            type: 'POST',
            url: "https://localhost:5001/api/UserApi/GetUser?userId=tlakshmivasavi005@gmail.com",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (e) {
                $.each(e, function (ele) {
                    if (document.getElementById(ele) != null) {
                        $('label[for="' + ele + '"]')[0].textContent = e[ele];
                    }
                });
                alert("success");
            },
            error: function (e) {
                console.log(e);
                alert("error");
            }
        });
    });

    $("#AddVehicle").on('submit', function () {
        alert("AddVehicle");
        var formData = new FormData(document.querySelector("form"));
        var Vehicle = {};
        for (var [key, value] of formData.entries()) {
            Vehicle[key] = value;
        }
        $.ajax({
            type: "POST",
            data: JSON.stringify(Vehicle),
            processData: false,
            url: "https://localhost:5001/api/UserApi/AddVehicle",
            contentType: "application/json",
            success: function (s) {
                console.log(s)
                alert("success");
                window.location.href = "/Home/Index";
            },
            error: function (e) {
                console.log(e)
            }
        });
    });

    $("#UpdateBalance").on('submit',function () {
        alert("balance");
        var formData = new FormData(document.querySelector("form"));
        var user = {};

        for (var [key, value] of formData.entries()) {
            user[key] = value;
        }
        $.ajax({
            type: "POST",
            data: JSON.stringify(user),
            processData: false,
            url: "https://localhost:5001/api/UserApi/AddAmount",
            contentType: "application/json",
            success: function (s) {
                console.log(s)
                alert("Added Successfully");
                window.location.href = "/User/GetProfile";
            },
            error: function (e) {
                console.log(e)
            }

        });
    });    

    $("#RideRequests").on('load', function () {
        alert("ride request");
        $.ajax({
            type: "GET",
            data: JSON.stringify(user),
            url: "https://localhost:5001/api/RideApi/ViewRequests?rideId=" +rideId,
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
                        '< p >' + item.Source + '</p > ' +
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
                        '< p >' + item.Destination + '</p > ' +
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

    $("#OfferedRides").on('load',function () {
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
                        '<p>' + item.route.source + '</p>' +
                        '</div>' +
                        '<div class="col-md-4">' +
                        '<div class="dot darkviolet"></div>' +
                        '<div class="dot"></div>' +
                        '<div class="dot"></div>' +
                        '<span class="glyphicon glyphicon-map-marker darkviolet"></span>' +
                        '</div>' +
                        '<div class="col-md-4">' +
                        '<small>To</small>' +
                        '<p>' + item.route.destination + '</p>' +
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

    $("#BookRide").on('submit',function () {
        var formData = new FormData(document.querySelector("form"));
        var user = {};
        for (var [key, value] of formData.entries()) {
            user[key] = value;
        }
        $.ajax({
            type: "POST",
            data: JSON.stringify(user),
            processData: false,
            url: "https://localhost:5001/api/RideApi/BookRide?userId=" + document.cookie,
            contentType: "application/json",
            success: function (data) {
                console.log(data);

                alert("success");
            },
            error: function (e) {
                console.log(e)
                alert("error");
            }
        });
    });

    $("#OfferRide").on('submit',function () {
        var formData = new FormData(document.querySelector("form"));
        var user = {};
        for (var [key, value] of formData.entries()) {
            user[key] = value;
        }
        $.ajax({
            type: "POST",
            data: JSON.stringify(user),
            processData: false,
            url: "https://localhost:5001/api/RideApi/OfferRide?userId=" + document.cookie,
            contentType: "application/json",
            success: function (s) {
                console.log(s)
                alert("success");
                window.location.href = "/Home/Index";
            },
            error: function (e) {
                console.log(e)
            }

        });
    });

    $("#MyBookings").on('load',function () {
        $.ajax({
            type: "GET",
            url: "https://localhost:5001/api/RideApi/GetBookings?userId=" + document.cookie,
            contentType: "application/json",
            success: function (data) {
                var form = $("#Bookings");
                $.each(data, function (index, item) {
                    $("#Bookings").append(
                        '<div class="col-md-4 p-3 mb-5 bg-white rounded shadow" >' +
                        '<div class="row">' +
                        '<div class="col-md-8">' +
                        '<h2>' + item.providerName + '</h2>' +
                        '</div>' +
                        '<div class="col-md-4">' +
                        '<img src="#">' +
                        '</div>' +
                        '</div>' +
                        '<div class="row">' +
                        '<div class="row">' +
                        '<div class="col-md-4">' +
                        '<small>From</small>' +
                        '<p>' + item.pickUp + '</p>' +
                        '</div>' +
                        '<div class="col-md-4">' +
                        '<div class="dot darkviolet"></div>' +
                        '<div class="dot"></div>' +
                        '<div class="dot"></div>' +
                        '<span class="glyphicon glyphicon-map-marker darkviolet"></span>' +
                        '</div>' +
                        '<div class="col-md-4">' +
                        '<small>To</small>' +
                        '<p>' + item.drop + '</p>' +
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
                        '<small>Price</small>' +
                        '<p>' + item.cost + '</p>' +
                        '</div>' +
                        '<div class="col-md-4 col-md-offset-4">' +
                        '<small>Seats Available</small>' +
                        '<p>' + item.noOfSeats + '</p>' +
                        '</div>' +
                        '</div>' +
                        '<div class="row">' +
                        '<div class="col-md-4">' +
                        '<small>Vehicle</small>' +
                        '<p>' + item.vehicleType + '</p>' +
                        '</div>' +
                        '<div class="col-md-4 col-md-offset-4">' +
                        '<small>Vehicle Number</small>' +
                        '<p>' + item.vehicleNumber + '</p>' +
                        '</div>' +
                        '</div>' +
                        '</div>' +
                        '</div>');
                });
                alert("success,mybooking");
            },
            error: function (e) {
                alert("error");
            }
        });
    });

    $("#AvailableRides").on('load',function () {
        var data = '@Html.Raw(Model)';
        $.ajax({
            type: "POST",
            data: data,
            url: "https://localhost:5001/api/RideApi/BookRide?userId=" + document.cookie,
            contentType: "application/json",
            success: function (data) {
                $.each(data, function (index, item) {
                    $("form").append(
                        '<button type="submit" name="RideId" value=' + item.id + '>' +
                        '<div class="col-md-4 p-3 mb-5 bg-white rounded shadow" >' +
                        '<div class="row">' +
                        '<div class="row">' +
                        '<div class="col-md-4">' +
                        '<small>From</small>' +
                        '<p>' + item.route.source + '</p>' +
                        '</div>' +
                        '<div class="col-md-4">' +
                        '<div class="dot darkviolet"></div>' +
                        '<div class="dot"></div>' +
                        '<div class="dot"></div>' +
                        '<span class="glyphicon glyphicon-map-marker darkviolet"></span>' +
                        '</div>' +
                        '<div class="col-md-4">' +
                        '<small>To</small>' +
                        '<p>' + item.route.destination + '</p>' +
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

        $("button").click(function () {
            var formData = new FormData($(this));
            var user = {};
            for (var [key, value] of formData.entries()) {
                user[key] = value;
            }
            $.ajax({
                type: "POST",
                data: JSON.stringify(user),
                processData: false,
                url: "https://localhost:5001/api/RideApi/RequestRide?userId=" + document.cookie + "/rideId=" + $(this).val(),
                contentType: "application/json",
                success: function (data) {
                    console.log(data);
                    alert("success");
                },
                error: function (e) {
                    console.log(e)
                    alert("error");
                }
            });
        });
    });

});