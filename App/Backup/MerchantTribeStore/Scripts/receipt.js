function CheckChanged() {
    var chk = $('#chksetpassword');
    if (chk.attr('checked')) {
        $('#firstpasswordform').show();
    }
    else {
        $('#firstpasswordform').hide();
    }
}

function SetFirstPassword() {
    $('#changing').show();
    $('#passwordmessage').html('');
    
    var emailfield = $('#email').val();
    var passwordfield = $('#password').val();
    var orderbvinfield = $('#orderbvin').val();

    $.post('../account/setfirstpassword',
            { "email": emailfield,
                "password": passwordfield,
                "orderbvin": orderbvinfield
            },
            function (data) {
                $('#changing').hide();
                if (data.Success == "True" || data.Success == true) {
                    $('#passwordmessage').html('<div class="flash-message-success">Your password is now set. Thank You!</div>');
                }
                else {
                    $('#passwordmessage').html('<div class="flash-message-warning">' + data.Messages + '</div>');
                }
            },
            "json")
            .error(function () {
                $('#changing').hide();
                $('#passwordmessage').html('<div class="flash-message-failure">Ajax error. contact administrator</div>');
            })
            .complete(function () { $('#changing').hide(); });
}

// Document Ready Function
$(document).ready(function () {
    $('#chksetpassword').click(function () { CheckChanged(); return true; });
    $('#setpasswordbutton').click(function () { SetFirstPassword(); return false; });
    CheckChanged();       
});             // End Document Ready
        

