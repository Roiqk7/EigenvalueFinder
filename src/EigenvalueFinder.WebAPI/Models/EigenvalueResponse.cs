using EigenvalueFinder.Core;

namespace EigenvalueFinder.WebAPI.Models;

public class EigenvalueResponse
{
        public List<Eigenpair> Eigenpairs { get; set; }

        public EigenvalueResponse(List<Eigenpair> eigenpairs)
        {
                Eigenpairs = eigenpairs;
        }
}
