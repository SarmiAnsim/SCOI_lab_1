using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCOI_lab_1
{
    class Newton_sPolynomial
    {
        SplineTuple[] splines;
        private struct SplineTuple
        {
            public double dy;
            public double[] x;
        }
        public void BuildSpline(double[] x, double[] y, int n)
        {
            splines = new SplineTuple[n];
            splines[0].dy = y[0];
            for (int i = 1; i < n; ++i)
            {
                splines[i].dy = 0;
                splines[i].x = x.Reverse().Skip(n-i).ToArray();
                for (int j = 0; j <= i; ++j)
                    splines[i].dy += y[j] / Pznam(x, i, j);
            }

        }
        public double Interpolate(double X)
        {
            double rez = splines[0].dy;
            for(int i = 1; i < splines.Length; ++i)
            {
                double Xs = 1;
                for (int j = 0; j < splines[i].x.Length; ++j)
                    Xs *= X - splines[i].x[j];
                rez += splines[i].dy * Xs;
            }
            return rez;
        }
        private double Pznam(double[] x, int n, int ind)
        {
            double result = 1;
            for (int i = 0; i <= n; ++i)
            {
                if (i != ind)
                    result *= (x[ind] - x[i]);
            }
            return result;
        }
    }
}
