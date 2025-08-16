document.addEventListener('DOMContentLoaded', function() {
	const matrixInput = document.getElementById('matrix-input');
	const errorMessageDisplay = document.getElementById('error-message');
	const solveButton = document.getElementById('solve-button');
	const resultsDisplay = document.getElementById('results-display');

	// Event listener for matrix input validation
	matrixInput.addEventListener('input', validateMatrixFormat);

	// Initial validation call to set button state on page load
	validateMatrixFormat();

	/**
	 * Validates the format of the matrix entered in the textarea.
	 * Sets 'invalid' class and displays error message if format is incorrect.
	 * Also controls the state (enabled/disabled, shadowed/unshadowed) of the solve button.
	 */
	function validateMatrixFormat()
	{
		const inputText = matrixInput.value.trim();
		let isValid = true;
		let message = '';

		// Check if results are currently visible and input is changing
		if (resultsDisplay.innerHTML.trim() !== '' && !resultsDisplay.classList.contains('animate-out')) {
			// If results are present, trigger fade-out animation
			resultsDisplay.classList.add('animate-out');
			// Listen for the end of the animation to clear content
			resultsDisplay.addEventListener('animationend', function handler() {
				resultsDisplay.innerHTML = ''; // Clear content after animation
				resultsDisplay.classList.remove('animate-out'); // Remove animation class
				resultsDisplay.removeEventListener('animationend', handler); // Remove listener to prevent multiple calls
			}, { once: true }); // Ensure the event listener is removed after it fires once
		}


		if (inputText === '')
		{
			isValid = false;
			matrixInput.classList.remove('invalid');
			// If input is empty, ensure results are cleared immediately if not already fading out
			if (!resultsDisplay.classList.contains('animate-out')) {
				resultsDisplay.innerHTML = '';
				resultsDisplay.classList.remove('animate-in'); // Also remove 'animate-in' if it was there
				resultsDisplay.style.opacity = '0'; // Reset for future animations
				resultsDisplay.style.transform = 'translateY(10px)'; // Reset for future animations
			}
		}
		else
		{
			// Split the input into rows using semicolons OR newlines as delimiters
			const rows = inputText.split(/[;\n]+/).map(row => row.trim()).filter(row => row !== '');

			if (rows.length === 0)
			{
				isValid = false;
				message = 'Matrix is empty or contains only delimiters.';
			}
			else
			{
				let expectedColumnCount = -1;

				for (let i = 0; i < rows.length; i++)
				{
					const row = rows[i];
					// Split the row into elements using one or more spaces OR commas as delimiters
					const elements = row.split(/[\s,]+/).filter(el => el !== '');

					if (elements.length === 0)
					{
						isValid = false;
						message = `Row ${i + 1} is empty.`;
						break;
					}

					for (let j = 0; j < elements.length; j++)
					{
						if (isNaN(parseFloat(elements[j])))
						{
							isValid = false;
							message = `Error: Value "${elements[j]}" in row ${i + 1} is not a valid number.`;
							break;
						}
					}
					if (!isValid) break;

					if (expectedColumnCount === -1)
					{
						expectedColumnCount = elements.length;
					}
					else if (elements.length !== expectedColumnCount)
					{
						isValid = false;
						message = `Error: Row ${i + 1} has ${elements.length} elements, but expected ${expectedColumnCount}. Matrix is not rectangular.`;
						break;
					}
				}

				if (isValid && rows.length !== expectedColumnCount)
				{
					isValid = false;
					message = `Error: Matrix is ${rows.length}x${expectedColumnCount}. It must be a square matrix (N x N).`;
				}
			}
		}

		// Apply styles and display message based on validation result
		if (isValid)
		{
			matrixInput.classList.remove('invalid');
			errorMessageDisplay.textContent = '';
			solveButton.classList.remove('disabled'); // Enable button
			solveButton.disabled = false;
		}
		else
		{
			// Only add 'invalid' class (red border) if there's actual content that's malformed
			if (inputText !== '')
			{
				matrixInput.classList.add('invalid');
			}
			else
			{
				matrixInput.classList.remove('invalid');
			}
			errorMessageDisplay.textContent = message; // Display the specific error message (or empty string if no message)
			solveButton.classList.add('disabled'); // Disable button
			solveButton.disabled = true;
		}
	}
});
