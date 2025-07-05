using Microsoft.AspNetCore.Mvc;
using EigenvalueFinder.Core;
using EigenvalueFinder.WebAPI.Models;

namespace EigenvalueFinder.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EigenvalueController : ControllerBase
{
        [HttpPost("calculate")]
        public IActionResult CalculateEigenvalues([FromBody] MatrixRequest request)
        {
                // Input validation: Check if the request or matrix is null/empty.
                // Assuming matrix is a valid square matrix (N x N) and contains valid numbers
                // as frontend (JavaScript) handles this validation.
                if (request == null || request.Matrix == null)
                {
                        return BadRequest("Matrix data is required.");
                }

                Matrix matrix = new Matrix(request.Matrix);
                List<QRUtils.Eigenpair> eigenpairs = new List<QRUtils.Eigenpair>();

                try
                {
                         // eigenpairs = QRSolver.FindEigenpairs(matrix)
                }
                catch (Exception ex)
                {
                        // TODO: Log the exception and handle it better
                        Console.WriteLine($"Error during eigenvalue calculation: {ex.Message}");
                        return StatusCode(500, "An error occurred during eigenvalue calculation.");
                }

                // --- TODO: Prepare the API response with the calculated eigenpairs ---
                var response = new EigenvalueResponse
                {
                        // TODO: Convert the .Core eigenvalues to .WebAPI eigenvalues
                };

                return Ok(response);
        }
}
