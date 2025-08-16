document.addEventListener('DOMContentLoaded', function() {
	const infoInputIcon = document.getElementById('info-input-icon'); 
	const infoInputPopup = document.getElementById('info-input-popup');
	const aboutLink = document.getElementById('about-link');
	const infoAboutPopup = document.getElementById('info-about-popup');

	/**
	 * Toggles the visibility of a given info popup.
	 * @param {HTMLElement} popupElement - The popup element to toggle.
	 * @param {Event} event - The click event.
	 */
	function togglePopup(popupElement, event) {
		event.stopPropagation(); // Prevent this click from immediately closing the popup via document listener
		if (popupElement.style.opacity === '1') {
			popupElement.style.opacity = '0';
			popupElement.style.visibility = 'hidden';
		} else {
			popupElement.style.opacity = '1';
			popupElement.style.visibility = 'visible';
		}
	}

	/**
	 * Closes a specific info popup.
	 * @param {HTMLElement} popupElement - The popup element to close.
	 */
	function closePopup(popupElement) {
		popupElement.style.opacity = '0';
		popupElement.style.visibility = 'hidden';
	}

	// Event listener for the input info icon
	infoInputIcon.addEventListener('click', function(event) {
		togglePopup(infoInputPopup, event);
		closePopup(infoAboutPopup); // Close other popups when this one opens
	});

	// Event listener for the about link
	aboutLink.addEventListener('click', function(event) { //
		event.preventDefault(); // Prevent default link behavior
		togglePopup(infoAboutPopup, event);
		closePopup(infoInputPopup); // Close other popups when this one opens
	});

	// Close popups when clicking outside
	document.addEventListener('click', function(event) { //
		if (infoInputPopup.style.opacity === '1' && !infoInputPopup.contains(event.target) && event.target !== infoInputIcon) { //
			closePopup(infoInputPopup);
		}
		if (infoAboutPopup.style.opacity === '1' && !infoAboutPopup.contains(event.target) && event.target !== aboutLink) { //
			closePopup(infoAboutPopup);
		}
	});
});
