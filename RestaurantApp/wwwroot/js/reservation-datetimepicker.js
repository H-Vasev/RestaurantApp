$(document).ready(function () {
	$.ajax({
		url: '/Reservation/AllFullyBookedReservation',
		method: 'GET',
		success: function (disabledDates) {
			initializeDateTimePicker(disabledDates);
		},
		error: function (error) {
			console.error("Error: ", error);
		}
	});
});

function initializeDateTimePicker(disabledDates) {
	$('#datetimepicker').datetimepicker({
		format: 'd-m-Y H:i',
		allowTimes: [
			'19:00', '20:00', '21:00', '22:00'
		],
		step: 60,
		minDate: 0,
		minTime: '19:00',
		maxTime: '22:00',
		defaultTime: '19:00',
		beforeShowDay: function (date) {
			var dateString = date.toISOString().slice(0, 10);
			if (disabledDates.includes(dateString)) {
				return [false, "", "Unavailable"];
			} else {
				return [true, "", "Available"];
			}
		}
	});
}