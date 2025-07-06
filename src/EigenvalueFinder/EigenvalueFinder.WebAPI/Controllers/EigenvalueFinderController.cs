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
                Matrix matrix = new Matrix(request.Matrix);
                List<Models.Eigenpair> webApiEigenpairs = new List<Models.Eigenpair>();

                try
                {
                         // eigenpairs = QRSolver.FindEigenpairs(matrix)
                         // DELETE: Testing placeholder
                         Complex e1 = new Complex(1, 2);
                         Complex e2 = new Complex(3, 4);
                         Complex e3 = new Complex(5, 6);

                         List<Complex> v1 = new List<Complex> { e1, e2, e3 };
                         List<Complex> v2 = new List<Complex> { e1, e2, e3 };
                         List<Complex> v3 = new List<Complex> { e1, e2, e3 };

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
