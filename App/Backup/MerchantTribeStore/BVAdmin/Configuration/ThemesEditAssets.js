
function DeleteAsset(fileName, itemToClose) {

    $.ajax({ type: 'POST',
        url: '/bvadmin/configuration/themeseditassets_delete.aspx',
        data: { themeid: $('#themeidfield').val(),
            assetname: fileName
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

    $(".deleteitem").click(function () { DeleteAsset($(this).attr('title'), $(this).parent()); return false; });

});      // End Document Ready
        

