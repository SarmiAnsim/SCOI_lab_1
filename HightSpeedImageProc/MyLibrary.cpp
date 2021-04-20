#include "pch.h"
#include <math.h>
#include <vector>
#include <queue>
#include <list>
typedef unsigned char byte;
# define M_PI           3.14159265358979323846  /* pi */

// DLL internal state variables:
static unsigned long long previous_;  // Previous value, if any
static unsigned long long current_;   // Current sequence value
static unsigned index_;               // Current seq. position

#ifndef N
#define N(l,P) ((double)(l)/(P))
#endif // !N

#ifndef Brig
#define Brig(Arr,counter) (0.0721 * Arr[counter] + 0.7154 * Arr[counter + 1] + 0.2125 * Arr[counter + 2])
#endif // !Brig


class Color
{
public:
    byte A = 0, R = 0, G = 0, B = 0;
    Color() {}
    Color(byte a, byte r, byte g, byte b) : A(a), R(r), G(g), B(b) {}

    static Color FromArgb(byte a, byte r, byte g, byte b)
    {
        return Color(a, r, g, b);
    }
    static Color FromRgba(byte r, byte g, byte b, byte a)
    {
        return Color(a, r, g, b);
    }
    static Color FromGbar(byte g, byte b, byte a, byte r)
    {
        return Color(a, r, g, b);
    }
    static Color FromBarg(byte b, byte a, byte r, byte g)
    {
        return Color(a, r, g, b);
    }
    static byte Clamp(const int value)
    {
        if (value > 255)
            return 255;
        else if (value < 0)
            return 0;
        else
            return value;
    }
    Color operator+(Color col)
    {
        return Color(
            Clamp(A + col.A),
            Clamp((int)(R * ((float)A / 255)) + (int)(col.R * ((float)col.A / 255))),
            Clamp((int)(G * ((float)A / 255)) + (int)(col.G * ((float)col.A / 255))),
            Clamp((int)(B * ((float)A / 255)) + (int)(col.B * ((float)col.A / 255))));
    }
    Color operator-(Color col)
    {
        return Color(
            Clamp(A + col.A),
            Clamp((int)(R * ((float)A / 255)) - (int)(col.R * ((float)col.A / 255))),
            Clamp((int)(G * ((float)A / 255)) - (int)(col.G * ((float)col.A / 255))),
            Clamp((int)(B * ((float)A / 255)) - (int)(col.B * ((float)col.A / 255))));
    }
    Color operator*(Color col)
    {
        return Color(
            Clamp(A + col.A),
            Clamp((int)((R * ((float)A / 255)) + (R * ((float)A / 255)) * (int)(col.R * ((float)col.A / 255)) / 255)),
            Clamp((int)((G * ((float)A / 255)) + (G * ((float)A / 255)) * (int)(col.G * ((float)col.A / 255)) / 255)),
            Clamp((int)((B * ((float)A / 255)) + (B * ((float)A / 255)) * (int)(col.B * ((float)col.A / 255)) / 255)));
    }
    Color operator&(Color col)
    {
        return Color(
            Clamp(A + col.A),
            Clamp((int)((R * ((float)A / 255)) + (int)(col.R * ((float)col.A / 255)) / 2)),
            Clamp((int)((G * ((float)A / 255)) + (int)(col.G * ((float)col.A / 255)) / 2)),
            Clamp((int)((B * ((float)A / 255)) + (int)(col.B * ((float)col.A / 255)) / 2)));
    }
    Color operator<(Color col)
    {
        return Color(
            Clamp(A + col.A),
            Clamp(min((int)(R * ((float)A / 255)), (int)(col.R * ((float)col.A / 255)))),
            Clamp(min((int)(G * ((float)A / 255)), (int)(col.G * ((float)col.A / 255)))),
            Clamp(min((int)(B * ((float)A / 255)), (int)(col.B * ((float)col.A / 255)))));
    }
    Color operator>(Color col)
    {
        return Color(
            Clamp(A + col.A),
            Clamp(max((int)(R * ((float)A / 255)), (int)(col.R * ((float)col.A / 255)))),
            Clamp(max((int)(G * ((float)A / 255)), (int)(col.G * ((float)col.A / 255)))),
            Clamp(max((int)(B * ((float)A / 255)), (int)(col.B * ((float)col.A / 255)))));
    }
};


double* ExpectationAndVariance(double* X, int count)
{
    double EAV[2] = { 0, 0 };
    for (int i = 0; i < count; ++i)
        EAV[0] += X[i];
    EAV[0] /= count;

    for (int i = 0; i < count; ++i)
        EAV[1] += (X[i] - EAV[0]) * (X[i] - EAV[0]);
    EAV[1] /= (count - 1);

    return EAV;
}

std::vector<std::vector<std::vector<long double>>> IntegralImage(byte* bgrAValues, int* size)
{
    std::vector<std::vector<long double>> II1 = {};
    std::vector<std::vector<long double>> II2 = {};

    for (int i = 0; i < size[0]; ++i)
    {
        std::vector<long double> line1 = {};
        std::vector<long double> line2 = {};
        for (int j = 0; j < size[1]; ++j)
        {
            long double rez1 = (double)Brig(bgrAValues, 4 * (i * size[1] + j)) / 255.;
            long double rez2 = pow(rez1, 2);
            if (j > 0)
            {
                rez1 += line1[j - 1];
                rez2 += line2[j - 1];
            }
            line1.push_back(rez1);
            line2.push_back(rez2);
        }
        for (int j = 0; j < size[1]; ++j)
        {
            if (i > 0)
            {
                line1[j] += II1[i - 1][j];
                line2[j] += II2[i - 1][j];
            }
        }
        II1.push_back(line1);
        II2.push_back(line2);
    }
    std::vector<std::vector<std::vector<long double>>> result = {};
    result.push_back(II1);
    result.push_back(II2);

    return result;
}

byte partition(byte arr[], int l, int r)
{
    int x = arr[r], i = l;
    for (int j = l; j <= r - 1; j++) {
        if (arr[j] <= x) {
            std::swap(arr[i], arr[j]);
            i++;
        }
    }
    std::swap(arr[i], arr[r]);
    return i;
}
byte kthSmallest(byte arr[], int l, int r, int k)
{
    if (k > 0 && k <= r - l + 1) {

        int index = partition(arr, l, r);

        if (index - l == k - 1)
            return arr[index];

        if (index - l > k - 1)
            return kthSmallest(arr, l, index - 1, k);

        return kthSmallest(arr, index + 1, r,
            k - index + l - 1);
    }

    return INT_MAX;
}

extern "C"
{
    __declspec(dllexport) void __stdcall ChangeBytes(byte* basis_bgrAValues, byte* supplement_bgrAValues, int count, int action)
    {
        for (int counter = 0; counter < count; counter += 4)
        {
            Color first(
                basis_bgrAValues[counter + 3],
                basis_bgrAValues[counter + 2],
                basis_bgrAValues[counter + 1],
                basis_bgrAValues[counter]);
            Color second(
                supplement_bgrAValues[counter + 3],
                supplement_bgrAValues[counter + 2],
                supplement_bgrAValues[counter + 1],
                supplement_bgrAValues[counter]);
            Color result = first;

            switch (action)
            {
            case 0:
                break;
            case 1:
                result = first + second;
                break;
            case 2:
                result = first - second;
                break;
            case 3:
                result = first * second;
                break;
            case 4:
                result = first & second;
                break;
            case 5:
                result = first < second;
                break;
            case 6:
                result = first > second;
                break;
            case 7:
                result = second;
                break;
            }

            basis_bgrAValues[counter + 3] = result.A;
            basis_bgrAValues[counter + 2] = result.R;
            basis_bgrAValues[counter + 1] = result.G;
            basis_bgrAValues[counter] = result.B;
        }
    }

    __declspec(dllexport) void __stdcall FuncChangeBytes(byte* AbgrValues, int count, int* func, bool flag, int* out_GD)
    {
        for (int counter = 0; counter < count && !flag; counter += 4)
        {
            AbgrValues[counter] = (byte)func[AbgrValues[counter]];
            AbgrValues[counter + 1] = (byte)func[AbgrValues[counter + 1]];
            AbgrValues[counter + 2] = (byte)func[AbgrValues[counter + 2]];

            int c = (AbgrValues[counter] + AbgrValues[counter + 1] + AbgrValues[counter + 2]) / 3;
            out_GD[c] = out_GD[c] + 1;
        }
    }

    __declspec(dllexport) void __stdcall GetGraphData(byte* bgrAValues, int count, int* out_GD)
    {
        for (int counter = 0; counter < count; counter += 4)
        {
            int c = (bgrAValues[counter] + bgrAValues[counter + 1] + bgrAValues[counter + 2]) / 3;
            out_GD[c] = out_GD[c] + 1;
        }
    }

    __declspec(dllexport) void __stdcall AverageBrightness(byte* bgrAValues, int count, int* out_t)
    {
        int S = 0;
        for (int counter = 0; counter < count; counter += 4)
        {
            int c = 0.0721 * bgrAValues[counter] + 0.7154 * bgrAValues[counter + 1] + 0.2125 * bgrAValues[counter + 2];
            S += c;
        }
        out_t[0] = (int)((float)S / ((float)count / 4));
    }

    __declspec(dllexport) void __stdcall OtsuCriterion(int* L, int count, int* out_t)
    {
        double mt = 0, qb = 0;
        for (int i = 0; i < 256; ++i)
        {
            mt += i * N(L[i], count);
        }
        for (int l = 0; l < 256; ++l)
        {
            double w1 = 0, w2;
            for (int i = 0; i < l; ++i)
                w1 += N(L[i], count);
            w2 = 1 - w1;

            double m1 = 0, m2;
            for (int i = 0; i < l; ++i)
                m1 += i * N(L[i], count);
            m1 /= w1;
            m2 = (mt - m1 * w1) / w2;

            double nqb = w1 * w2 * (m1 - m2) * (m1 - m2);
            if (nqb > qb)
            {
                qb = nqb;
                out_t[0] = l;
            }
        }
    }

    __declspec(dllexport) void __stdcall GlobalBinarize(byte* bgrAValues, int count, int t)
    {
        for (int counter = 0; counter < count; counter += 4)
        {
            bgrAValues[counter] = (bgrAValues[counter] <= t ? 0 : 255);
            bgrAValues[counter + 1] = (bgrAValues[counter + 1] <= t ? 0 : 255);
            bgrAValues[counter + 2] = (bgrAValues[counter + 2] <= t ? 0 : 255);
        }
    }

    __declspec(dllexport) void __stdcall LocalBinarize(byte* bgrAValues, int* size, int a, float k, int version)
    {
        std::vector<std::vector<std::vector<long double>>> IIs = IntegralImage(bgrAValues, size);

        std::vector<std::vector<double>> t = {};
        std::vector<std::vector<double>> Qs = {};
        double min = -5;
        double max = NULL;
        for (int i = 0; i < size[0]; ++i)
        {
            std::vector<double> Qmp = {};
            std::vector<double> tmp = {};
            for (int j = 0; j < size[1]; ++j)
            {
                if (min > Brig(bgrAValues, 4 * (i * size[1] + j)))
                    min = Brig(bgrAValues, 4 * (i * size[1] + j));

                double EAV[2] = {0, 0};

                int it1, jt1, it2, jt2;
                int r = (a - 1) / 2;

                it1 = (i - r - 1 < 0) ? 0 : (i - r - 1);
                jt1 = (j - r - 1 < 0) ? 0 : (j - r - 1);
                it2 = (i + r > size[0] - 1) ? (size[0] - 1) : (i + r);
                jt2 = (j + r > size[1] - 1) ? (size[1] - 1) : (j + r);

                EAV[0] = (IIs[0][it2][jt2] - IIs[0][it1][jt2] - IIs[0][it2][jt1] + IIs[0][it1][jt1]) / ((it2 - it1) * (jt2 - jt1));
                EAV[1] = (IIs[1][it2][jt2] - IIs[1][it1][jt2] - IIs[1][it2][jt1] + IIs[1][it1][jt1]) / ((it2 - it1) * (jt2 - jt1)) - EAV[0] * EAV[0];

                double Q = sqrt(EAV[1]);
                Qmp.push_back(Q);
                if (Q > max || max == NULL)
                    max = Q;

                switch (version)
                {
                case 0:
                    tmp.push_back((EAV[0] + k * Q));
                    break;
                case 1:
                    tmp.push_back((EAV[0] * (1 + k * (Q / 128 - 1))));
                    break;
                case 2:
                    tmp.push_back(EAV[0]);
                    break;
                case 3:
                    tmp.push_back(EAV[0] * (1 - k));
                    break;
                default:
                    break;
                }
            }
            t.push_back(tmp);
            Qs.push_back(Qmp);
        }
        for (int i = 0; i < size[0]; ++i)
        {
            for (int j = 0; j < size[1]; ++j)
            {
                if (version == 2)
                    t[i][j] = (t[i][j] * 255 * (1 - k) + k * min + k * (Qs[i][j] / max) * (t[i][j] * 255 - min)) / 255.;

                bgrAValues[4 * (i * size[1] + j)] = (((double)bgrAValues[4 * (i * size[1] + j)] / 255.) <= t[i][j] ? 0 : 255);
                bgrAValues[4 * (i * size[1] + j) + 1] = (((double)bgrAValues[4 * (i * size[1] + j) + 1] / 255.) <= t[i][j] ? 0 : 255);
                bgrAValues[4 * (i * size[1] + j) + 2] = (((double)bgrAValues[4 * (i * size[1] + j) + 2] / 255.) <= t[i][j] ? 0 : 255);
            }
        }
    }

    __declspec(dllexport) void __stdcall GetGauss(long double* out_val, double sig, int a, int b)
    {
        for (int i = 0; i < a; ++i)
        {
            int it = i - (a - 1) / 2;
            for (int j = 0; j < b; ++j)
            {
                int jt = j - (b - 1) / 2;
                out_val[i * b + j] = (1.0 / (2 * M_PI * sig * sig)) * exp(-(it * it + jt * jt) / (2 * sig * sig));
            }
        }
    }

    __declspec(dllexport) void __stdcall LineFilter(byte* bgrAValues, int* size, double* M, int a, int b, long double* out_test)
    {
        std::queue<std::vector<byte>> S = {};
        for (int i = 0; i < size[0]; ++i)
        {
            for (int j = 0; j < size[1]; ++j)
            {
                if (i > (a - 1) / 2)
                {
                    bgrAValues[4 * ((i - (a - 1) / 2 - 1) * size[1] + j)] = S.front()[0];
                    bgrAValues[4 * ((i - (a - 1) / 2 - 1) * size[1] + j) + 1] = S.front()[1];
                    bgrAValues[4 * ((i - (a - 1) / 2 - 1) * size[1] + j) + 2] = S.front()[2];
                    S.pop();
                }
                double sum[3] = { 0, 0, 0 };
                for (int it = 0; it < a; ++it)
                {
                    for (int jt = 0; jt < b; ++jt)
                    {
                        int iter = i - (a - 1) / 2 + it;
                        iter = (iter < 0) ? (0 - iter) : iter;
                        iter = (iter > (size[0] - 1)) ? (2 * (size[0] - 1) - iter) : iter;

                        int jter = j - (b - 1) / 2 + jt;
                        jter = (jter < 0) ? (0 - jter) : jter;
                        jter = (jter > (size[1] - 1)) ? (2 * (size[1] - 1) - jter) : jter;

                        sum[0] += (double)bgrAValues[4 * (iter * size[1] + jter)] * M[it * b + jt];
                        sum[1] += (double)bgrAValues[4 * (iter * size[1] + jter) + 1] * M[it * b + jt];
                        sum[2] += (double)bgrAValues[4 * (iter * size[1] + jter) + 2] * M[it * b + jt];
                    }
                }

                std::vector<byte> tmpS = { Color::Clamp(round(sum[0])), Color::Clamp(round(sum[1])), Color::Clamp(round(sum[2])) };
                out_test[i * size[1] + j] = tmpS[0];
                S.push(tmpS);
                tmpS.clear();
                tmpS.~vector();
            }
            
        }
        for (int i = size[0] - (a - 1) / 2 - 1; i < size[0]; ++i)
        {
            for (int j = 0; j < size[1]; ++j)
            {
                bgrAValues[4 * (i * size[1] + j)] = S.front()[0];
                bgrAValues[4 * (i * size[1] + j) + 1] = S.front()[1];
                bgrAValues[4 * (i * size[1] + j) + 2] = S.front()[2];

                S.pop();
            }
        }
    }

    __declspec(dllexport) void __stdcall MedianFilter(byte* bgrAValues, int* size, int a, int b, long double* out_test)
    {
        std::queue<std::vector<byte>> S = {};
        for (int i = 0; i < size[0]; ++i)
        {
            for (int j = 0; j < size[1]; ++j)
            {
                if (i > (a - 1) / 2)
                {
                    bgrAValues[4 * ((i - (a - 1) / 2 - 1) * size[1] + j)] = S.front()[0];
                    bgrAValues[4 * ((i - (a - 1) / 2 - 1) * size[1] + j) + 1] = S.front()[1];
                    bgrAValues[4 * ((i - (a - 1) / 2 - 1) * size[1] + j) + 2] = S.front()[2];
                    S.pop();
                }

                std::vector<byte> data1 = {};
                std::vector<byte> data2 = {};
                std::vector<byte> data3 = {};

                for (int it = 0; it < a; ++it)
                {
                    for (int jt = 0; jt < b; ++jt)
                    {
                        int iter = i - (a - 1) / 2 + it;
                        iter = (iter < 0) ? (0 - iter) : iter;
                        iter = (iter > (size[0] - 1)) ? (2 * (size[0] - 1) - iter) : iter;

                        int jter = j - (b - 1) / 2 + jt;
                        jter = (jter < 0) ? (0 - jter) : jter;
                        jter = (jter > (size[1] - 1)) ? (2 * (size[1] - 1) - jter) : jter;

                        data1.push_back(bgrAValues[4 * (iter * size[1] + jter)]);
                        data2.push_back(bgrAValues[4 * (iter * size[1] + jter) + 1]);
                        data3.push_back(bgrAValues[4 * (iter * size[1] + jter) + 2]);
                    }
                }

                kthSmallest(data1.data(), 0, data1.size() - 1, (data1.size() - 1) / 2 + 1);
                S.push({ 
                    kthSmallest(data1.data(), 0, data1.size() - 1, (data1.size() - 1) / 2 + 1),
                    kthSmallest(data2.data(), 0, data2.size() - 1, (data2.size() - 1) / 2 + 1),
                    kthSmallest(data3.data(), 0, data3.size() - 1, (data3.size() - 1) / 2 + 1) });

                /*std::vector<const byte*> ptr1(data1.size());
                std::vector<const byte*> ptr2(data2.size());
                std::vector<const byte*> ptr3(data3.size());
                transform(data1.begin(), data1.end(), ptr1.begin(), [](const byte& d) {return &d; });
                transform(data2.begin(), data2.end(), ptr2.begin(), [](const byte& d) {return &d; });
                transform(data3.begin(), data3.end(), ptr3.begin(), [](const byte& d) {return &d; });
                auto mid1 = next(ptr1.begin(), data1.size() / 2);
                auto mid2 = next(ptr2.begin(), data2.size() / 2);
                auto mid3 = next(ptr3.begin(), data3.size() / 2);
                nth_element(ptr1.begin(), mid1, ptr1.end(), [](const byte* lhs, const byte* rhs) {return *lhs < *rhs; });
                nth_element(ptr2.begin(), mid2, ptr2.end(), [](const byte* lhs, const byte* rhs) {return *lhs < *rhs; });
                nth_element(ptr3.begin(), mid3, ptr3.end(), [](const byte* lhs, const byte* rhs) {return *lhs < *rhs; });
                ptrdiff_t pos1 = *mid1 - &data1[0];
                ptrdiff_t pos2 = *mid2 - &data2[0];
                ptrdiff_t pos3 = *mid3 - &data3[0];

                out_test[i * size[1] + j] = data1[pos1];
                S.push({ data1[pos1], data2[pos2], data3[pos3] });*/
                data1.clear();
                data1.~vector();
                data2.clear();
                data2.~vector();
                data3.clear();
                data3.~vector();
            }

        }
        for (int i = size[0] - (a - 1) / 2 - 1; i < size[0]; ++i)
        {
            for (int j = 0; j < size[1]; ++j)
            {
                bgrAValues[4 * (i * size[1] + j)] = S.front()[0];
                bgrAValues[4 * (i * size[1] + j) + 1] = S.front()[1];
                bgrAValues[4 * (i * size[1] + j) + 2] = S.front()[2];

                S.pop();
            }
        }
    }
}