function Validate()
{
    var return_val = true;
    if ($('#HasVehicle').is(':checked')) {

        if ($('#Vehicle_Model').val().trim() == '') {
            $('#Vehicle_Model').next('span').show();
            return_val = false;
        } else {
            $('#Vehicle_Model').next('span').hide();
        }
        alert(return_val);
        if ($('#Vehicle_Capacity').val().trim() == '') {
            $('#Vehicle_Capacity').next('span').show();
            return_val = false;
        } else {
            $('#Vehicle_Capacity').next('span').hide();
        }
        alert(return_val);
        if ($('#Vehicle_Number').val().trim() == '') {
            $('#Vehicle_Number').next('span').show();
            return_val = false;
        } else {
            $('#Vehicle_Number').next('span').hide();
        }
        alert(return_val);
        if (!return_val) {
            event.preventDefault();
        }
    }
}