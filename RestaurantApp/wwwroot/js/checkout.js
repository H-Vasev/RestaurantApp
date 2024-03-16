$(document).ready(function () {
    let initialDeliveryCharge = parseFloat($('#deliveryOptions').val());
    let initialPrice = totalPrice + initialDeliveryCharge;

    $('#totalPrice').text('€ ' + initialPrice.toFixed(2));
    $('input[name="totalPrice"]').val(initialPrice);

    $('#deliveryOptions').change(function () {
        let charge = parseFloat($(this).val());
        let newTotalPrice = totalPrice + charge;

        $('#totalPrice').text('€ ' + newTotalPrice.toFixed(2));
        $('input[name="totalPrice"]').val(newTotalPrice);
    });
});
