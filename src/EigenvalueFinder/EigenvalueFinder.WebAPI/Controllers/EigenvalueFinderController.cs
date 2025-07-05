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
                List<Models.Eigenpair> webApiEigenpairs = new List<Models.Eigenpair>();

                try
                {
                         // eigenpairs = QRSolver.FindEigenpairs(matrix)
                         // DELETE: Testing placeholder
                         double e1 = 1;
                         double e2 = 2;
                         double e3 = 3;

                         List<double> v1 = new List<double> { 1, 0, 0 };
                         List<double> v2 = new List<double> { 0, 1, 0 };
                         List<double> v3 = new List<double> { 0, 0, 1 };

                         var eigenpair1 = new Eigenpair(e1, v1);
                         var eigenpair2 = new Eigenpair(e2, v2);
                         var eigenpair3 = new Eigenpair(e3, v3);

                         webApiEigenpairs.Add(eigenpair1);
                         webApiEigenpairs.Add(eigenpair2);
                         webApiEigenpairs.Add(eigenpair3);
                }
                catch (Exception ex)
                {
                        // TODO: Log the exception and handle it better
                        Console.WriteLine($"Error during eigenvalue calculation: {ex.Message}");
                        return StatusCode(500, "An error occurred during eigenvalue calculation.");
                }

                var response = new EigenvalueResponse(webApiEigenpairs);

                return Ok(response);
        }
}
