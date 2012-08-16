function LoadNews() {
    $('#changing').show();    
    $.ajax(
                        { 
                            url: 'http://merchanttribe.com/news',
                            data: {
                                version: '1.0'
                            },
                            dataType: "html",
                            success: function (data) {
                                $('#changing').hide();
                                $('#newsfeed').html(data);
                            },
                            error: function () {                                
                                $('#changing').hide();
                                $('#newsfeed').html('Could not load news at the moment. Will try again later.');
                            },

                        });
}

// Document Ready Function
$(document).ready(function () {
    LoadNews();
});// End Document Ready
        

