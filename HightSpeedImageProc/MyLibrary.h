// MathLibrary.h - Contains declarations of math functions
#pragma once

#ifdef MYLIBRARY_EXPORTS
#define MATHLIBRARY_API __declspec(dllexport)
#else
#define MYLIBRARY_API __declspec(dllimport)
#endif

typedef unsigned char byte;

extern "C" MYLIBRARY_API void ChangeBytes(
    byte* basis_bgrAValues, byte* supplement_bgrAValues, int count, int action);

extern "C" MYLIBRARY_API void FuncChangeBytes(
    byte* AbgrValues, int count, int* func, bool flag, int* out_GD);

extern "C" MYLIBRARY_API void GetGraphData(
    byte* AbgrValues, int count, int* out_GD);

extern "C" MYLIBRARY_API void AverageBrightness(
    byte* bgrAValues, int count, int* out_t);

extern "C" MYLIBRARY_API void OtsuCriterion(
    int* L, int count, int* out_t);

extern "C" MYLIBRARY_API void GlobalBinarize(
    byte* bgrAValues, int count, int t);

extern "C" MYLIBRARY_API void LocalBinarize(
    byte * bgrAValues, int* size, int a, float k, int version);

extern "C" MYLIBRARY_API void GetGauss(
    long double* out_val, double sig, int a, int b);

extern "C" MYLIBRARY_API void LineFilter(
    byte * bgrAValues, int* size, double* M, int a, int b, long double* out_test);

extern "C" MYLIBRARY_API void MedianFilter(
    byte * bgrAValues, int* size, int a, int b, long double* out_test);

extern "C" MYLIBRARY_API void GetDFT(
    byte * bgrAValues, int* size, double* out_DFTone, double* out_DFTtwo, double* max);

extern "C" MYLIBRARY_API void ImageFromDFT(
    byte * bgrAValues, int* size, double* DFTone, double* DFTtwo);