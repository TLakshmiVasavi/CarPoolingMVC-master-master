$(document).on('click', '#edit', function () {
    $('label').hide();
    $('input').show();
    $(this).hide();
    $("#save").show();
    $("#cancel").show();
});

$(document).on('click', "#cancel", function () {
    $('label').show();
    $('input').hide();
    $("#edit").show();
    $("#save").hide();
    $("#cancel").hide();
});

$(document).ready(function () {
    $("#save").hide();
    $("#cancel").hide();
    $('input').hide();
});