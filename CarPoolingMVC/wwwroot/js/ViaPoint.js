function ViaPoint()
{
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

function SaveDetails()
{
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
function cancelled()
{
    $("#NewViaPoint").remove();
    $("#AddViaPoint").show();
}