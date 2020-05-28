$(document).ready(
    function () {
        $("#save").hide();
        $("#cancel").hide();

        $(document).on('click',"#edit", function (e) {
            $("#UserProfile :input").prop("disabled", false);
            $(this).hide();
            $("#save").show();
            $("#cancel").show();
        });

        $(document).on('click', "#cancel", function (e) {
            $("#UserProfile :input").prop("disabled", true);
            $("#edit").prop("disabled", false);
            $("#edit").show();
            $("#save").hide();
            $("#cancel").hide();
        });
    }
);