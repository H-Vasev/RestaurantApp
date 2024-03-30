$(document).ready(function () {
    setTimeout(function () {
        $('#dateMessage').fadeOut('slow', function () {
            $(this).remove();
        })
    }, 3000);
});