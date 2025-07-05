namespace EigenvalueFinder.WebAPI.Models;

public class Eigenpair
{
        public double Eigenvalue { get; set; }
        public List<double> Eigenvector { get; set; }

        public Eigenpair(double eigenvalue, List<double> eigenvector)
        {
                Eigenvalue = eigenvalue;
                Eigenvector = eigenvector;
        }
}
