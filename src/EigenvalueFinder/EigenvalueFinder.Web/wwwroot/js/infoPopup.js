document.addEventListener('DOMContentLoaded', function() {
	const infoIcon = document.getElementById('info-icon');
	const infoPopup = document.getElementById('info-popup');

	infoIcon.addEventListener('click', toggleInfoPopup); // Toggle popup on click
	document.addEventListener('click', closeInfoPopupOutside); // Close popup when clicking outside

	/**
	 * Toggles the visibility of the info popup.
	 */
	function toggleInfoPopup(event) {
		event.stopPropagation(); // Prevent this click from immediately closing the popup via document listener
		if (infoPopup.style.opacity === '1') {
			infoPopup.style.opacity = '0';
			infoPopup.style.visibility = 'hidden';
		} else {
			infoPopup.style.opacity = '1';
			infoPopup.style.visibility = 'visible';
		}
	}

	/**
	 * Closes the info popup if a click occurs outside of it or the info icon.
	 */
	function closeInfoPopupOutside(event) {
		if (infoPopup.style.opacity === '1' && !infoPopup.contains(event.target) && event.target !== infoIcon) {
			infoPopup.style.opacity = '0';
			infoPopup.style.visibility = 'hidden';
		}
	}
});
