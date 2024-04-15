$(document).ready(function () {
    setTimeout(function () {
        $('#message').fadeOut('slow', function () {
            $(this).remove();
        })
    }, 3000);
});