$(document).ready(function () {
    $('.gallery-item a').click(function (e) {
        e.preventDefault();
        var imageUrl = $(this).data('img-url');
        var modalBody = $('#imageModal .modal-body');
        modalBody.empty();
        modalBody.append(`<img src="${imageUrl}" class="img-fluid">`);
        $('#imageModal').modal('show');
    });
});

$('#imageModal').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget);
    var id = button.data('id');
    var viewsCounterSelector = `#views-count-${id}`;
    var csrfToken = $('input[name = "__RequestVerificationToken"]').val();

    $.ajax({
        url: '/Gallery/IncrementImageCount',
        type: 'POST',
        data: {
            id: id,
            __RequestVerificationToken: csrfToken
        }
    }).done(function (response) {
        $(viewsCounterSelector).html(`<i class="fas fa-eye"></i> ${response.viewCount}`);
    }).fail(function (error) {
        console.error('Error incrementing views count', error);
    });
});