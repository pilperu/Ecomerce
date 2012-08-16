
function DeleteButton(fileName, itemToClose) {

    $.ajax({ type: 'POST',
        url: '/bvadmin/configuration/themeseditbuttons_delete.aspx',
        data: { themeid: $('#themeidfield').val(),
            buttonname: fileName
        },
        dataType: "json",
        success: function (data) {
            if (data.result === true) {
                $(itemToClose).slideUp();
            }
        },
        error: function (data) {
        }
    });
}

// Document Ready Function
$(document).ready(function () {

    $(".deleteitem").click(function () { DeleteButton($(this).attr('title'), $(this).parent()); return false; });

});      // End Document Ready
        

