using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCOI_lab_1
{
    class QuadraticSpline
    {
        SplineTuple[] splines; // Сплайн

        // Структура, описывающая сплайн на каждом сегменте сетки
        private struct SplineTuple
        {
            public double a, b, c, x;
        }

        // Построение сплайна
        // x - узлы сетки, должны быть упорядочены по возрастанию, кратные узлы запрещены
        // y - значения функции в узлах сетки
        // n - количество узлов сетки
        public void BuildSpline(double[] x, double[] y, int n)
        {
            if(n > 2)
            {
                splines = new SplineTuple[n - 2];
                for (int i = 0; i < n - 2; ++i)
                {
                    splines[i].x = x[i + 1];
                }

                for (int i = 1; i < n - 1; ++i)
                {
                    splines[i - 1].c = (y[i + 1] - y[i - 1]) / ((x[i + 1] - x[i - 1]) * (x[i + 1] - x[i])) - (y[i] - y[i - 1]) / ((x[i] - x[i - 1]) * (x[i + 1] - x[i]));
                    splines[i - 1].b = (y[i] - y[i - 1]) / (x[i] - x[i - 1]) - splines[i - 1].c * (x[i] + x[i - 1]);
                    splines[i - 1].a = y[i - 1] - splines[i - 1].b * x[i - 1] - splines[i - 1].c * Math.Pow(x[i - 1], 2);
                }
            }
            else
            {
                splines = new SplineTuple[1];
                splines[0].a = (x[1] * y[0] - x[0] * y[1]) / (x[1] - x[0]);
                splines[0].b = (y[1] - y[0]) / (x[1] - x[0]);
                splines[0].c = 0;
                splines[0].x = x[1];
            }
        }

        // Вычисление значения интерполированной функции в произвольной точке
        public double Interpolate(double x)
        {
            if (splines == null)
            {
                return double.NaN; // Если сплайны ещё не построены - возвращаем NaN
            }
            int n = splines.Length;
            SplineTuple s;

            if (x <= splines[0].x) // Если x меньше точки сетки x[0] - пользуемся первым эл-тов массива
            {
                s = splines[0];
            }
            else if (x >= splines[n - 1].x) // Если x больше точки сетки x[n - 1] - пользуемся последним эл-том массива
            {
                s = splines[n - 1];
            }
            else // Иначе x лежит между граничными точками сетки - производим бинарный поиск нужного эл-та массива
            {
                int i = 0;
                int j = n - 1;
                while (i + 1 < j)
                {
                    int k = i + (j - i) / 2;
                    if (x <= splines[k].x)
                    {
                        j = k;
                    }
                    else
                    {
                        i = k;
                    }
                }
                s = splines[i];
            }

            // Вычисляем значение сплайна в заданной точке по схеме Горнера (в принципе, "умный" компилятор применил бы схему Горнера сам, но ведь не все так умны, как кажутся)
            return s.a + s.b * x + s.c * Math.Pow(x, 2);
        }
    }
}
