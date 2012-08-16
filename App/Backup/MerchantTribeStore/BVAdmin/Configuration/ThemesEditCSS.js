
function UpdateCSS() {
    
    $('#jsonoutput').html('<div class="flash-message-info">Saving. Please Wait...</div>').fadeIn(200);
    $('#updatecssonly').hide();    
    $.ajax({ type: 'POST',
        url: '/bvadmin/configuration/themeseditcss_update.aspx',
        data: { themeid: $('#themeidfield').val(),
            css: $('#EditForm').serialize().replace('EditForm=','')
        },
        dataType: "json",
        success: function (data) {            
            if (data.result === true) {
                $('#jsonoutput').html('<div class="flash-message-success">Changes Saved</div>').fadeIn(200).fadeOut(5000);
                $('#updatecssonly').show();
            }
            else {
                $('#jsonoutput').html('<div class="flash-message-warning">Unable to update CSS</div>');
                $('#updatecssonly').show();
            }
        },
        error: function (data) {
            $('#jsonoutput').html('<div class="flash-message-error">Could not update CSS</div>');
            $('#updatecssonly').show();
        }
    });
}



// Document Ready Function
$(document).ready(function () {


    $("#updatecssonly").click(function () { UpdateCSS(); return false; });


});                    // End Document Ready
        

