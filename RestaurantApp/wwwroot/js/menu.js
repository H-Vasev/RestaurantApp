$(document).ready(function () {
    $('.add-to-cart').click(function (e) {
        e.preventDefault();

        var productId = $(this).data('id');
        $.ajax({
            url: '/Menu/AddToCart',
            type: 'POST',
            data: {
                id: productId,
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },
            success: function (response) {

                updateCartCount();

                var messageHtml = '<div id="successResult" class="customAlertMessage alert alert-success text-center mt-5" > Product added to basket successfully! </div>';
                $('#message-container').html(messageHtml);

                setTimeout(function () {
                    $('#successResult').fadeOut('slow', function () {
                        $(this).remove();
                    });
                }, 3000);
            },
            error: function (xhr,error) {
                if (xhr.status == 401) { 
                    window.location.href = '/Account/Login'; 
                } else {
                    console.error("Error adding to cart: ", error);
                }
            }
        });

        function updateCartCount() {
            $.ajax({
                url: '/ShoppingCart/GetCartItemsCount',
                type: 'GET',
                success: function (response) {
                    $('#cartItemCount').text(response.count);
                },
                error: function (error) {
                    console.error("Error updating cart count: ", error);
                }
            });
        }
    });
});