$(document).ready(function () {

    function Validate() {
        var return_val = true;
        if ($('#HasVehicle').is(':checked')) {

            if ($('#Vehicle_Model').val().trim() == '') {
                $('#Vehicle_Model').next('span').show();
                return_val = false;
            } else {
                $('#Vehicle_Model').next('span').hide();
            }
            if ($('#Vehicle_Capacity').val().trim() == '') {
                $('#Vehicle_Capacity').next('span').show();
                return_val = false;
            } else {
                $('#Vehicle_Capacity').next('span').hide();
            }
            if ($('#Vehicle_Number').val().trim() == '') {
                $('#Vehicle_Number').next('span').show();
                return_val = false;
            } else {
                $('#Vehicle_Number').next('span').hide();
            }
            return return_val;
        }
    }

    function GetImage() {
        $.ajax({
            type: 'GET',
            url: "https://localhost:5001/api/UserApi/GetImage?userId="+document.cookie,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (e) {
                $("#userPic").attr("src", "data:image/jpeg;base64," + e)
            },
            error: function (e) {
                console.log(e);
                alert("error");
            }
        });
    }

    $("#SignUpForm").submit(function () {
        if (Validate()) {
            event.preventDefault();
            $.ajax({
                url: "https://localhost:5001/api/UserApi/SignUp",
                type: "POST",
                data: new FormData(this),
                processData: false,
                contentType: false,
                success: function (s) {
                    GetImage();
                    window.location.href = "/home/index";
                },
                error: function (e) {
                    console.log(e)
                    alert("error");
                }
            });
        }
    });

    $("#LoginForm").on('submit', function (e) {
        e.preventDefault();
        var formData = new FormData(document.querySelector("form"));
        var user = {};
        for (var [key, value] of formData.entries()) {
            user[key] = value;
        }
        $.ajax({
            type: "POST",
            data: JSON.stringify(user),
            url: "https://localhost:5001/api/UserApi/Login",
            contentType: "application/json",
            dataType: "json",
            success: function (value) {
                document.cookie=$("#Id").val()
                GetImage();
                window.location.href = "/Home/Index";
            },
            error: function (e) {
                alert("error");
            }
        });
    });//

    $("#UserProfile").on('submit', function () {
        $.ajax({
            type: 'POST',
            url: "https://localhost:5001/api/UserApi/GetUser?userId="+$.cookie("userId"),
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
                console.log(e);
                window.location.href = "/Home/Error";
            }
        });
    });

    $("#AddVehicle").on('submit', function () {
        var formData = new FormData(document.querySelector("form"));
        var Vehicle = {};
        for (var [key, value] of formData.entries()) {
            Vehicle[key] = value;
        }
        $.ajax({
            type: "POST",
            data: JSON.stringify(Vehicle),
            processData: false,
            url: "https://localhost:5001/api/UserApi/AddVehicle?userId="+$.cookie("userId"),
            contentType: "application/json",
            success: function (s) {
                console.log(s)
                alert("success");
                window.location.href = "/Home/Index";
            },
            error: function (e) {
                console.log(e);
                window.location.href = "/Home/Error";
            }
        });
    });

    $("#UpdateBalance").on('submit', function () {
        var formData = new FormData(document.querySelector("form"));
        var user = {};
        for (var [key, value] of formData.entries()) {
            user[key] = value;
        }
        $.ajax({
            type: "POST",
            data: JSON.stringify(user),
            processData: false,
            url: "https://localhost:5001/api/UserApi/AddAmount?userId=" + $.cookie("userId"),
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

    $("#BookRide").on('submit', function () {
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

    $("#OfferRide").on('submit', function () {
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

    $("#MyBookings").on('load', function () {
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

    $("#AvailableRides").on('load', function () {
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