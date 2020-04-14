$(document).on('click', '#edit', function () {
    $('label').hide();
    $('input').show();
    $(this).hide();
    $("#save").show();
    $("#cancel").show();
    $('select').show();
});

$(document).on('click', "#cancel", function () {
    $('label').show();
    $('input').hide();
    $("#edit").show();
    $("#save").hide();
    $("#cancel").hide();
    $('select').hide();
});

$(document).ready(function () {
    $('select').hide();
    $("#save").hide();
    $("#cancel").hide();
    $('input').hide();
});