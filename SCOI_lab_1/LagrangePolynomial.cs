using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCOI_lab_1
{
    class LagrangePolynomial
    {
        public double Interpolate(double[] x, double[] y, int n, double X)
        {
            double result = 0;
            for (int i = 0; i < n; ++i)
            {
                result += y[i] * P(x, n, i, X);
            }
            return result;
        }
        private double P(double[] x, int n, int ind, double X)
        {
            double result = 1;
            for(int i = 0; i < n; ++i)
            {
                if (i != ind)
                    result *= (X - x[i]) / (x[ind] - x[i]);
            }
            return result;
        }
    }
}
