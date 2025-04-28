// pch.cpp: 對應到先行編譯標頭的來源檔案

#include "pch.h"
#include <string>
#include <cmath>

extern "C" {
	//===========================================================================
	//
	//===========================================================================
	__declspec(dllexport) void negative(int *f, int w, int h, int *g)
	{
		for (int i = 0; i < w * h; i++)
				g[i] = 255 - f[i];

	}
	__declspec(dllexport) void contrast_AND_Bright(int* f, int w, int h, int* g, float a, int b) {
		for (int i = 0; i < w * h; i++) {
			g[i] = (int)((f[i] - 128) * a + 128 + b);
			g[i] = (g[i] > 255) ? 255 : (g[i] < 0) ? 0 : g[i];
		}
	}

    __declspec(dllexport) void bitplane(int* f, int w, int h, int* g, int n) {  
		
		for (int i = 0; i < w * h; i++)
		{
			g[i] = (f[i] / (1 << n)) % 2;
			g[i] *= 255;
		}
    }

    __declspec(dllexport) void histogram_equalization(int* f, int w, int h, int* g) {
        int histogram[256] = { 0 };
        int cdf[256] = { 0 }; // 累積分佈函數 (CDF)
        int lut[256] = { 0 }; // 轉換表 (Look-up Table) 映射 計算後的灰階值
        int total_pixels = w * h;

        // 計算直方圖
        for (int i = 0; i < total_pixels; i++) {
            histogram[f[i]]++;
        }

        // 計算累積分佈函數 (CDF)
        cdf[0] = histogram[0];
        for (int i = 1; i < 256; i++) {
            cdf[i] = cdf[i - 1] + histogram[i];
        }

        // 構建轉換表 (LUT)
        int min_cdf = 0;
        for (int i = 0; i < 256; i++) {
            if (cdf[i] > 0) {
                min_cdf = cdf[i];
                break;
            }
        }
        for (int i = 0; i < 256; i++) {
            lut[i] = (int)((float)(cdf[i] - min_cdf) / (total_pixels - min_cdf) * 255.0);
            lut[i] = (lut[i] < 0) ? 0 : (lut[i] > 255) ? 255 : lut[i];
        }

        // 進行直方圖均衡化
        for (int i = 0; i < total_pixels; i++) {
            g[i] = lut[f[i]];
        }
    }

    __declspec(dllexport) void histograms(int* f, int w, int h, int* g) {
        float hist[256] = { 0 };
        for (int i = 0; i < 256; i++)
        {
            for (int j = 0; j < w * h; j++)
            {
                if (f[j] == i) {
                    hist[i]++;
                    g[i] = hist[i];
                }
            }
        }
    }

    __declspec(dllexport) void flip_horizontal(int* f, int w, int h, int* g) {
        for (size_t i = 0; i < h; i++)
        {
            for (size_t j = 0; j < w; j++) {
                g[i * w + j] = f[i * w + (h - j - 1)];
            }
        }
    }

    __declspec(dllexport) void flip_transpose_right(int* f, int w, int h, int* g) {
        for (size_t i = 0; i < h; i++)
        {
            for (size_t j = 0; j < w; j++) {
                g[j * h + (w - i - 1)] = f[i * w + j];
            }
        }
    }

    __declspec(dllexport) void flip_transpose_left(int* f, int w, int h, int* g) {
        for (size_t i = 0; i < h; i++)
        {
            for (size_t j = 0; j < w; j++) {
                g[(h - j - 1) * w + i] = f[i * w + j];
            }
        }
    }

    __declspec(dllexport) void mosaic(int* f, int w, int h, int* g) {
        // 複製輸入影像到輸出影像，確保未變更部分保持原樣
        for (int i = 0; i < w * h; i++) {
            g[i] = f[i];
        }

        // 進行 4x4 區塊馬賽克處理，只處理 [100:200, 100:200] 範圍
        for (int i = 100; i < 200; i += 4) {
            for (int j = 100; j < 200; j += 4) {
                // 取得區塊左上角的值
                int refValue = g[i * w + j];

                // 用該值填滿 4x4 區塊
                for (int di = 0; di < 4; di++) {
                    for (int dj = 0; dj < 4; dj++) {
                        int y = i + di;
                        int x = j + dj;
                        if (y < h && x < w) {  // 確保不超出影像範圍
                            g[y * w + x] = refValue;
                        }
                    }
                }
            }
        }
    }

	__declspec(dllexport) void Filter(int* f, int w, int h, int* g, double* kernel){  
        int x, y, i, j;
        int kernel_size = 5;
        int offset = kernel_size / 2;  
		int dx[25];
        int dy[25];

        for (i = 0; i < kernel_size; i++) {
            for (j = 0; j < kernel_size; j++) {
                dx[i * kernel_size + j] = j - offset;  
                dy[i * kernel_size + j] = i - offset;  
            }
        }
        for (y = 0; y < h; y++)
        {
            for (x = 0; x < w; x++)
            {
                double sum = 0.0;
                for (i = 0; i < 25; i++)
                {
                    int xx = x + dx[i];
                    int yy = y + dy[i];

                    unsigned char pixel = 0; // 預設取不到的地方是 0

                    if (xx >= 0 && xx < w && yy >= 0 && yy < h)
                    {
                        pixel = f[yy * w + xx];
                    }

                    sum += pixel * kernel[i];
                }

                if (sum < 0.0) sum = 0.0;
                if (sum > 255.0) sum = 255.0;

                g[y * w + x] = (unsigned char)(sum);
            }
        }

    }
}