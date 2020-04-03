$(document).ready(function () {
    if (location.pathname == "/" || location.pathname == "/user/login" || location.pathname.toLowerCase() == "/user/signup") {
        $(".user").hide();
    }
    else {
        $(".user").show();
    }
});

$(document).ready(function () {

    $(".form-group .form-control").blur(function () {
        if ($(this).val() != "") {
            $(this).addClass('active');
            $(this).sibilings("label").addClass('active');

        } else {
            $(this).sibilings("label").removeClass('active');
        }
    });
});