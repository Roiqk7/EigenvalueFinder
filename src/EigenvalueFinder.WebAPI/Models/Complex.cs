namespace EigenvalueFinder.WebAPI.Models;

public class Complex
{
        public double Real { get; set; }
        public double Imaginary { get; set; }

        public Complex(double real, double imaginary)
        {
                Real = real;
                Imaginary = imaginary;
        }
}
