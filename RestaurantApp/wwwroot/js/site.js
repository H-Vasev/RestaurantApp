document.addEventListener('DOMContentLoaded', function () {
	var navToggle = document.querySelector('.mobile-nav-toggle');
	var navMenu = document.querySelector('.navbar ul');
	navToggle.addEventListener('click', function () {
		if (navMenu.style.display === 'block') {
			navMenu.style.display = 'none';
		} else {
			navMenu.style.display = 'block';
		}
	});
});