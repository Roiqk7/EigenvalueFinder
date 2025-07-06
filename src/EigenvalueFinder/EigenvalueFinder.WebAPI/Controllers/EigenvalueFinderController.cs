using Microsoft.AspNetCore.Mvc;
using EigenvalueFinder.Core;
using EigenvalueFinder.WebAPI.Models;

namespace EigenvalueFinder.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EigenvalueController : ControllerBase
{
        /// <summary>
        /// Calculates the eigenvalues and eigenvectors of a given matrix using the QR algorithm.
        /// </summary>
        /// <param name="request">MatrixRequest containing the matrix data.</param>
        /// <returns>Returns an EigenvalueResponse containing eigenpairs (eigenvalue and eigenvector).</returns>
        [HttpPost("calculate")]
        public IActionResult CalculateEigenvalues([FromBody] MatrixRequest request)
        {
                Matrix matrix = new Matrix(request.Matrix);
                List<Models.Eigenpair> webApiEigenpairs;

                try
                {
                        List<QRUtils.Eigenpair> eigenpairs = QRSolver.FindEigenpairs(matrix);

                        // Convert core eigenpairs to web API eigenpairs
                        webApiEigenpairs = ConvertToWebApiModel(eigenpairs);
                }
                catch (Exception ex)
                {
                        // TODO: Use proper logging mechanism (e.g., ILogger)
                        Console.WriteLine($"Error during eigenvalue calculation: {ex.Message}");
                        return StatusCode(500, "An error occurred during eigenvalue calculation.");
                }

                var response = new EigenvalueResponse(webApiEigenpairs);
                return Ok(response);
        }

        /// <summary>
        /// Converts a list of core library Eigenpairs to a list of Web API Eigenpairs.
        /// </summary>
        /// <param name="corePairs">List of core Eigenpairs to convert.</param>
        /// <returns>List of converted Web API Eigenpairs.</returns>
        private static List<EigenvalueFinder.WebAPI.Models.Eigenpair> ConvertToWebApiModel(List<EigenvalueFinder.Core.QRUtils.Eigenpair> corePairs)
        {
                return corePairs.Select(ConvertToWebApiModel).ToList();
        }

        /// <summary>
        /// Converts a single core Eigenpair to a Web API Eigenpair.
        /// </summary>
        /// <param name="corePair">The core Eigenpair to convert.</param>
        /// <returns>The converted Web API Eigenpair.</returns>
        private static EigenvalueFinder.WebAPI.Models.Eigenpair ConvertToWebApiModel(EigenvalueFinder.Core.QRUtils.Eigenpair corePair)
        {
                var eigenvalue = new EigenvalueFinder.WebAPI.Models.Complex(
                        corePair.eigenvalue.Real,
                        corePair.eigenvalue.Imaginary
                );

                var vector = new List<EigenvalueFinder.WebAPI.Models.Complex>();
                for (int i = 0; i < corePair.eigenvector.RowCount; i++)
                {
                        var c = corePair.eigenvector[i, 0];
                        vector.Add(new EigenvalueFinder.WebAPI.Models.Complex(c.Real, c.Imaginary));
                }

                return new EigenvalueFinder.WebAPI.Models.Eigenpair(eigenvalue, vector);
        }
}
