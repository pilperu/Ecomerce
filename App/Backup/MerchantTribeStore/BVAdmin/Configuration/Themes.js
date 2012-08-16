

// Document Ready Function
$(document).ready(function () {

    alert('test');

    $(".themeview").hover(
      function () {
          $(this).find(".controls").show();
      },
      function () {
          $(this).find(".controls").hide();
      }
    );
    
});        // End Document Ready
        


