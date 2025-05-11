// pch.cpp: 對應到先行編譯標頭的來源檔案

#include "pch.h"
#include <string>
#include <cmath>
#include <numbers>
#include <vector>

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

	//__declspec(dllexport) void Filter(int* f, int w, int h, int* g, double* kernel){  
 //       int x, y, i, j;
 //       int kernel_size = 5;
 //       int offset = kernel_size / 2;  
	//	int dx[25];
 //       int dy[25];

 //       for (i = 0; i < kernel_size; i++) {
 //           for (j = 0; j < kernel_size; j++) {
 //               dx[i * kernel_size + j] = j - offset;  
 //               dy[i * kernel_size + j] = i - offset;  
 //           }
 //       }
 //       for (y = 0; y < h; y++)
 //       {
 //           for (x = 0; x < w; x++)
 //           {
 //               double sum = 0.0;
 //               for (i = 0; i < 25; i++)
 //               {
 //                   int xx = x + dx[i];
 //                   int yy = y + dy[i];

 //                   unsigned char pixel = 0; // 預設取不到的地方是 0

 //                   if (xx >= 0 && xx < w && yy >= 0 && yy < h)
 //                   {
 //                       pixel = f[yy * w + xx];
 //                   }

 //                   sum += pixel * kernel[i];
 //               }

 //               if (sum < 0.0) sum = 0.0;
 //               if (sum > 255.0) sum = 255.0;

 //               g[y * w + x] = (unsigned char)(sum);
 //           }
 //       }
 //   }
    __declspec(dllexport) void Filter(int* f, int w, int h, int* g, double* kernel, double sigma) {
        int kernel_size = 5;
        int offset = kernel_size / 2;
        int dx[25], dy[25];
        double gaussian_kernel[25]; // 若 sigma > 0 時使用這個
        // 建立 dx, dy 位移表
        for (int i = 0; i < kernel_size; i++) {
            for (int j = 0; j < kernel_size; j++) {
                dx[i * kernel_size + j] = j - offset;
                dy[i * kernel_size + j] = i - offset;
            }
        }
        // 如果 sigma > 0，產生高斯 kernel
        if (sigma > 0.0) {
            double sum = 0.0;
            for (int i = 0; i < kernel_size; i++) {
                for (int j = 0; j < kernel_size; j++) {
                    int x = j - offset;
                    int y = i - offset;
                    double value = std::exp(-(x * x + y * y) / (2 * sigma * sigma));
                    gaussian_kernel[i * kernel_size + j] = value;
                    sum += value;
                }
            }
            // 正規化
            for (int i = 0; i < 25; i++) {
                gaussian_kernel[i] /= sum;
            }
        }
        for (int y = 0; y < h; y++) {
            for (int x = 0; x < w; x++) {
                double sum = 0.0;
                for (int i = 0; i < 25; i++) {
                    int xx = x + dx[i];
                    int yy = y + dy[i];

                    unsigned char pixel = 0;
                    if (xx >= 0 && xx < w && yy >= 0 && yy < h) {
                        pixel = f[yy * w + xx];
                    }

                    double k = (sigma > 0.0) ? gaussian_kernel[i] : kernel[i];
                    sum += pixel * k;
                }
                if (sum < 0.0) sum = 0.0;
                if (sum > 255.0) sum = 255.0;
                g[y * w + x] = (unsigned char)(sum);
            }
        }
    }

    __declspec(dllexport) void rotations(int* f, int w, int h, int nw, int nh, int* g, int theta) {

        double theta_rad = theta * std::numbers::pi / 180;
        int hs = w / 2;
        int ks = h / 2;

        for (int i = 0; i < nw * nh; i++)
        {
            g[i] = 0;
        }
        double no[3][3] = {
            {cos(theta_rad), -sin(theta_rad), -hs * cos(theta_rad) + ks * sin(theta_rad) + nw / 2},
            {sin(theta_rad), cos(theta_rad), -hs * sin(theta_rad) - ks * cos(theta_rad) + nh / 2},
            {0, 0, 1},
        };

        int M[3] = { 0 };

        for (int i = 0; i < h; i++) {
            for (int j = 0; j < w; j++) {
                for (int x = 0; x < 3; x++) {
                    M[x] = no[x][0] * j + no[x][1] * i + no[x][2];
                }
                //g[M[1] + hs / 2 * nw + (M[0] + ks / 2)] = f[i * w + j];
                g[M[1] * nw + M[0]] = f[i * w + j];
            }
        }

    }
    __declspec(dllexport)void Otsu(int* f, int w, int h, int* g, int histSize) {
        int total = w * h;  //像素數
        int* hist = (int*)malloc(sizeof(int) * histSize);  //直方圖
        double* prob = (double*)malloc(sizeof(double) * histSize); //機率
        double* levels = (double*)malloc(sizeof(double) * histSize);  //灰階

        // 初始化
        for (int i = 0; i < histSize; ++i) {
            hist[i] = 0;
        }

        // 計算直方圖
        for (int i = 0; i < total; ++i) {
            int val = f[i];
            int bin = val * histSize / 256;
            if (bin >= histSize) bin = histSize - 1;
            hist[bin]++;
        }

        // 計算機率 + 建立灰階表
        for (int i = 0; i < histSize; ++i) {
            prob[i] = (double)hist[i] / total;
            levels[i] = i * (256.0 / histSize);
        }

        // Otsu找最佳Threshold
        //double between_var = w0 * w1 * (mu0 - mu1) * (mu0 - mu1); 核心公式
        // w0 背景權重 mu0背景均值
        // w1 前景權重 mu1 前景均值
        // 當前灰階級別機率為背景 ex 看prob[0] 那灰階級別 0 當背景 其他為前景
        //找出最大值 就是最佳閥值
        double max_between_var = 0.0;
        int best_thresh_bin = 0;

        for (int t = 1; t < histSize; ++t) {
            double w0 = 0.0, mu0 = 0.0;
            for (int i = 0; i < t; ++i) {
                w0 += prob[i];
                mu0 += levels[i] * prob[i];
            }
            if (w0 == 0.0) continue;
            mu0 /= w0;

            double w1 = 1.0 - w0;
            if (w1 == 0.0) continue;

            double mu1 = 0.0;
            for (int i = t; i < histSize; ++i) {
                mu1 += levels[i] * prob[i];
            }
            mu1 /= w1;

            double between_var = w0 * w1 * (mu0 - mu1) * (mu0 - mu1);
            if (between_var > max_between_var) {
                max_between_var = between_var;
                best_thresh_bin = t;
            }
        }

        // 計算實際灰階門檻
        int threshold = best_thresh_bin * 256 / histSize;
        if (threshold > 255) threshold = 255;

        // 二值化處理
        for (int i = 0; i < total; ++i) {
            int val = f[i];
            g[i] = (val >= threshold) ? 255 : 0;
        }

        // 釋放記憶體
        free(hist);
        free(prob);
        free(levels);
    }

    __declspec(dllexport) void connected_component(int* f, int w, int h, int* g) {
        // 初始化標記陣列
        int label = 0;
        int* labels = new int[w * h];
        for (int i = 0; i < w * h; i++) {
            labels[i] = -1; // -1 表示未標記
        }
        // 進行連通區域標記
        for (int y = 0; y < h; y++) {
            for (int x = 0; x < w; x++) {
                if (f[y * w + x] > 0 && labels[y * w + x] == -1) { // 如果是前景且未標記
                    label++;
                    std::vector<std::pair<int, int>> stack;
                    stack.push_back({ x, y });
                    while (!stack.empty()) {
                        auto [cx, cy] = stack.back();
                        stack.pop_back();
                        if (cx < 0 || cx >= w || cy < 0 || cy >= h) continue; // 邊界檢查
                        if (f[cy * w + cx] == 0 || labels[cy * w + cx] != -1) continue; // 背景或已標記
                        labels[cy * w + cx] = label;
                        // 將相鄰像素加入堆疊
                        stack.push_back({ cx + 1, cy });
                        stack.push_back({ cx - 1, cy });
                        stack.push_back({ cx, cy + 1 });
                        stack.push_back({ cx, cy - 1 });
                    }
                }
            }
        }
        // 將標記結果寫入輸出影像
        for (int i = 0; i < w * h; i++) {
            g[i] = labels[i];
        }
        delete[] labels;
    }

}