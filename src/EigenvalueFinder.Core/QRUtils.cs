using System.Numerics;

namespace EigenvalueFinder.Core;

public static class QRUtils
{
        public struct QR
        {
                public EigenvalueFinder.Core.Matrix Q;
                public EigenvalueFinder.Core.Matrix R;

                public QR(Matrix Q, Matrix R)
                {
                        this.Q = Q;
                        this.R = R;
                }
        }

        public struct Eigenpair
        {
                public Complex eigenvalue;
                public EigenvalueFinder.Core.Matrix eigenvector;
        }
}
