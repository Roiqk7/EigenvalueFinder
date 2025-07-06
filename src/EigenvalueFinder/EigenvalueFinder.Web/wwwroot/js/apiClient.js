document.addEventListener('DOMContentLoaded', function() {
	const matrixInput = document.getElementById('matrix-input');
	const errorMessageDisplay = document.getElementById('error-message');
	const solveButton = document.getElementById('solve-button');
	const resultsDisplay = document.getElementById('results-display');
	solveButton.addEventListener('click', findEigenvalues);

	/**
	 * Parses the matrix from the textarea into a 2D array of numbers (array of arrays).\
	 * Assumes the input format has already been validated by validateMatrixFormat.js.
	 */
	function parseMatrixTo2D(inputText) {
		// Replace newlines with semicolons for consistent row splitting, then split by semicolons
		const rows = inputText.replace(/\n/g, ';').split(';').map(row => row.trim()).filter(row => row !== '');
		const matrix2D = [];
		for (const rowStr of rows) {
			// Split by spaces or commas for elements, then parse to float
			const elementsInRow = rowStr.split(/[\s,]+/).filter(el => el !== '').map(el => parseFloat(el));
			matrix2D.push(elementsInRow); // Push the array of elements as a row
		}
		return matrix2D;
	}

	/**
	 * Formats a complex number for display (a + bi).
	 */
	function formatComplex(complexNum) {
		const real = complexNum.real.toFixed(4);
		const imaginary = complexNum.imaginary.toFixed(4);

		if (imaginary === "0.0000") {
			return real;
		} else if (real === "0.0000") {
			return `${imaginary}i`;
		} else if (imaginary.startsWith("-")) {
			return `${real} - ${imaginary.substring(1)}i`;
		} else {
			return `${real} + ${imaginary}i`;
		}
	}

	/**
	 * Handles the click event for the "Find Eigenvalues" button.
	 * Sends the 2D matrix to the backend API and displays the results (eigenpairs).
	 */
	async function findEigenvalues() {
		if (solveButton.disabled) {
			return;
		}

		const inputText = matrixInput.value.trim();

		// Re-validate just before sending to ensure no last-second invalid input
		if (typeof validateMatrixFormat === 'function') {
			validateMatrixFormat(); // Re-evaluate button state
		}
		if (solveButton.disabled) {
			return;
		}

		const matrixData2D = parseMatrixTo2D(inputText);

		// --- Display a loading message ---
		errorMessageDisplay.textContent = ''; // Clear any previous error messages
		resultsDisplay.innerHTML = '<p>Calculating eigenvalues and eigenvectors...</p>';
		// Remove any existing animation class immediately to reset the state for new animation
		resultsDisplay.classList.remove('animate-in');
		// Ensure initial opacity and transform are set for re-triggering animation
		resultsDisplay.style.opacity = '0';
		resultsDisplay.style.transform = 'translateY(10px)';


		solveButton.disabled = true; // Disable button during calculation
		solveButton.classList.add('disabled');

		const apiEndpoint = "http://localhost:5017/api/Eigenvalue/calculate";

		try {
			const response = await fetch(apiEndpoint, {
				method: 'POST',
				headers: {
					'Content-Type': 'application/json'
				},
				body: JSON.stringify({ matrix: matrixData2D })
			});

			if (!response.ok) {
				const errorText = await response.text();
				let errorMessage = `Server error: ${response.status} ${response.statusText}`;
				try {
					const errorData = JSON.parse(errorText);
					errorMessage = errorData.title || errorData.message || errorData.detail || errorMessage;
				}
				catch (e) {
					errorMessage = errorText || errorMessage;
				}
				errorMessageDisplay.textContent = 'Error: ' + errorMessage;
				resultsDisplay.textContent = ''; // Clear loading message on error
				resultsDisplay.classList.remove('animate-in');
				resultsDisplay.style.opacity = '0';
				resultsDisplay.style.transform = 'translateY(10px)';
				return; // Exit the function here as an error occurred
			}

			const data = await response.json();

			if (data.eigenpairs && Array.isArray(data.eigenpairs)) {
				let eigenvalues = [];
				let eigenvectors = [];

				data.eigenpairs.forEach(pair => {
					// Collect eigenvalues
					if (pair.eigenvalue !== undefined) {
						eigenvalues.push(formatComplex(pair.eigenvalue));
					}

					// Collect eigenvectors
					if (pair.eigenvector && Array.isArray(pair.eigenvector)) {
						eigenvectors.push(`{${pair.eigenvector.map(v => formatComplex(v)).join(', ')}}`);
					}
				});

				let resultsHtml = '';

				if (eigenvalues.length > 0) {
					resultsHtml += '<h3>Eigenvalues:</h3>';
					resultsHtml += `<p>{${eigenvalues.join(', ')}}</p>`;
				} else {
					resultsHtml += '<p>No eigenvalues found.</p>';
				}

				if (eigenvectors.length > 0) {
					resultsHtml += '<h3>Eigenvectors:</h3>';
					resultsHtml += `<p>${eigenvectors.join('<br>')}</p>`;
				} else {
					resultsHtml += '<p>No eigenvectors found.</p>';
				}

				resultsDisplay.innerHTML = resultsHtml;

				// Trigger the animation after setting the content
				// A small setTimeout ensures the browser has time to render the initial state
				// before applying the animation class, making the animation re-trigger correctly.
				setTimeout(() => {
					resultsDisplay.classList.add('animate-in');
				}, 10); // A minimal delay (e.g., 10ms) is often sufficient

			}
			else {
				resultsDisplay.textContent = 'Error: Invalid response format from API. Expected "eigenpairs" array.';
			}

		}
		catch (error) {
			console.error('Fetch error:', error);
			errorMessageDisplay.textContent = 'Error: ' + error.message;
			resultsDisplay.textContent = ''; // Clear loading message on error
			// Ensure animation class is removed on error
			resultsDisplay.classList.remove('animate-in');
			resultsDisplay.style.opacity = '0';
			resultsDisplay.style.transform = 'translateY(10px)';
		}
		finally {
			if (typeof validateMatrixFormat === 'function') {
				validateMatrixFormat(); // Re-evaluate button state
			}
			else {
				solveButton.disabled = false;
				solveButton.classList.remove('disabled');
			}
		}
	}
});
