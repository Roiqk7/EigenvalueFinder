namespace EigenvalueFinder.WebAPI.Models;

public class Eigenpair
{
        public Complex Eigenvalue { get; set; }
        public List<Complex> Eigenvector { get; set; }

        public Eigenpair(Complex eigenvalue, List<Complex> eigenvector)
        {
                Eigenvalue = eigenvalue;
                Eigenvector = eigenvector;
        }
}
