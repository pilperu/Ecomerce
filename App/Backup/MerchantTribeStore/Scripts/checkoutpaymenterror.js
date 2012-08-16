function CleanCC() {
    var notclean = $('#cccardnumber').val();
    $.post('~/checkout/CleanCreditCard',
   { "CardNumber": notclean },
    function (data) {
        $('#cccardnumber').val(data.CardNumber);
    },
   "json");
}


      function LoadRegionsWithSelection(regionlist, countryid, selectedregion) {
            $.post('~/estimateshipping/getregions/' + countryid,
          { "regionid": selectedregion },
          function (data) {
              regionlist.html(data.Regions);
              $('#tempshippingregion').val('');
              $('#tempbillingregion').val('');
              BindStateDropDownLists();
          },
         "json"
         );
      } 
      

// Document Ready Function
$(document).ready(function () {

    $('#cccardnumber').change(function () { CleanCC(); });
    $('#billingcountryname').change(function () {
         LoadRegionsWithSelection($('#billingstate'), $('#billingcountryname option:selected').val(), $('#tempbillingregion').val());
    });

});        // End Document Ready
        

