using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCOI_lab_1
{
    class BezierCurve
    {
        static int Fuctorial(int n) // Функция вычисления факториала
        {
            int res = 1;
            for (int i = 1; i <= n; ++i)
                res *= i;
            return res;
        }
        static float polinom(int i, int n, float t)// Функция вычисления полинома Бернштейна
        {
            return Fuctorial(n) / (Fuctorial(i) * Fuctorial(n - i)) * (float)Math.Pow(t, i) * (float)Math.Pow(1 - t, n - i);
        }
        public static List<double> BuildSpline(double[] x, double[] y, int n)// Функция рисования кривой
        {
            int j = 0;
            float step = 0.0005f;// Возьмем шаг 0.01 для большей точности

            List<double> result = new List<double>(new double[256]);//Конечный массив точек кривой
            for (float t = 0; t < 1; t += step)
            {
                double ytmp = 0;
                double xtmp = 0;
                for (int i = 0; i < n; ++i)
                { // проходим по каждой точке
                    float b = polinom(i, n - 1, t); // вычисляем наш полином Бернштейна
                    ytmp += y[i] * b;
                    xtmp += x[i] * b;
                }
                result[(int)Math.Round(xtmp)] = ytmp;
                j++;
            }
            return result;
        }
    }
}
