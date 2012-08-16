
function UpdateCustomFiles() {

    $('#jsonoutput').html('<div class="flash-message-info">Saving. Please Wait...</div>').fadeIn(200);
    $('#updateonly').hide();
    $.ajax({ type: 'POST',
        url: '/bvadmin/configuration/themeseditheaderfooter_update.aspx',
        data: { themeid: $('#themeidfield').val(),
            headerhtml: $('#headerhtml').serialize().replace('headerhtml=', ''),
            footerhtml: $('#footerhtml').serialize().replace('footerhtml=', '')
             },
        dataType: "json",
        success: function (data) {            
            if (data.result === true) {
                $('#jsonoutput').html('<div class="flash-message-success">Changes Saved</div>').fadeIn(200).fadeOut(5000);
                $('#updateonly').show();
            }
            else {
                $('#jsonoutput').html('<div class="flash-message-warning">Unable to update Files</div>');
                $('#updateonly').show();
            }
        },
        error: function (data) {
            $('#jsonoutput').html('<div class="flash-message-error">Could not update Files</div>');
            $('#updateonly').show();
        }
    });
}

// Document Ready Function
$(document).ready(function () {

    $("#updateonly").click(function () { UpdateCustomFiles(); return false; });

});      // End Document Ready
        

