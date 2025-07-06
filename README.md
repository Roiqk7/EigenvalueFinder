# EigenvalueFinder

Program for finding eigenvalues and eigenvectors using the [QR algorithm](https://en.wikipedia.org/wiki/QR_algorithm).

**TODO: gif of the web usage**

## Installation

Clone the repository:

```
git clone https://github.com/Roiqk7/EigenvalueFinder.git
```
## Dependencies

**TODO:**

## Usage

**TODO:**

## Feedback

Feel free to report any issues or suggest features in the issues section.

## What the program CAN'T do aka TODOs

* Find eigenvector for a given eigenvalue
  * The program simply returns the collumns of the accumulated `Q` matrix and assigns it to the `EigenvalueFinder.Core.QRUtils.Eigenpair.eigenvector` variable without knowing if the eigenvalue and eigenvector form an eigenpair.
  * This could be solved by using the [inverse iteration](https://en.wikipedia.org/wiki/Inverse_iteration).

## Gallery

**TODO:**
