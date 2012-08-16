$(document).ready(function () {

    var tabContainers = $('div.tabs > div');

    $('ul.tabnavigation a').click(function () {
        tabContainers.hide().filter(this.hash).show();

        $('ul.tabnavigation li').removeClass('selected');
        $(this).parent().addClass('selected');

        return false;
    }).filter(':first').click();
});
