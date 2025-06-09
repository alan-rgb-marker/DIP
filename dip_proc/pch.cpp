// pch.cpp: 對應到先行編譯標頭的來源檔案

#include "pch.h"
#include <string>
#include <cmath>
#include <numbers>
#include <vector>
#include <algorithm>
#include <cstdlib>

extern "C" {
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
        /*
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
                    M[x] = (int)std::round(no[x][0] * j + no[x][1] * i + no[x][2]);
                }

                //g[M[1] + hs / 2 * nw + (M[0] + ks / 2)] = f[i * w + j];
                if (M[0] >= 0 && M[0] < nw && M[1] >= 0 && M[1] < nh)
                    g[M[1] * nw + M[0]] = f[i * w + j];

            }
        }

		int dx[4] = { -1, 1, 0, 0 };
		int dy[4] = { 0, 0, -1, 1 };

        for (int i = 0; i < nh; i++)
        {
            for (int j = 0; j < nw; j++)
            {
                if (g[i * nw + j]!=0) continue;
                bool b = false;
                for (size_t z = 0; z < 4; z++)
                {
                    int nx = j + dx[z]; // 計算相鄰像素的 x 座標
                    int ny = i + dy[z]; // 計算相鄰像素的 y 座標
                    if (nx < 0 || nx >= nw || ny < 0 || ny >= nh) continue; // 邊界檢查
                    if (g[ny * nw + nx] > 0)
                    {
                        b = true;
                        break;
                    }
                }      
                if (b == true) {
                    g[i * nw + j] = (g[i * nw + j + 1] + g[i * nw + j - 1])/2;
                }
            }
        }
        */
        if (theta % 360 == 0)
        {
            for(int i = 0; i < h * w; i++)
				g[i] = f[i]; // 如果角度是 0，直接複製原圖
        }
        else if (theta % 270 == 0) {
            for (size_t i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++) {
                    g[(255 - j) * w + i] = f[i * w + j];
                }
            }
        }
        else {
            double theta_rad = theta * std::numbers::pi / 180.0;
            int hs = w / 2;
            int ks = h / 2;
            int new_hs = nw / 2;
            int new_ks = nh / 2;

            // 初始化輸出畫布為黑色（灰階255）
            std::fill(g, g + nw * nh, 255);

            // 反向旋轉矩陣
            double cos_theta = std::cos(-theta_rad);
            double sin_theta = std::sin(-theta_rad);

            for (int y_new = 0; y_new < nh; ++y_new) {
                for (int x_new = 0; x_new < nw; ++x_new) {
                    // 轉換到中心座標系
                    double x_centered = x_new - new_hs;
                    double y_centered = y_new - new_ks;



                    // 反向旋轉對應原圖座標
                    double x_src = cos_theta * x_centered - sin_theta * y_centered + hs;
                    double y_src = sin_theta * x_centered + cos_theta * y_centered + ks;

                    // 檢查是否在原圖範圍內
                    if (x_src >= 0 && x_src < w - 1 && y_src >= 0 && y_src < h - 1) {
                        // 進行雙線性插值
                        int x0 = (int)std::floor(x_src);
                        int y0 = (int)std::floor(y_src);
                        double dx = x_src - x0;
                        double dy = y_src - y0;

                        int p00 = f[y0 * w + x0];
                        int p10 = f[y0 * w + (x0 + 1)];
                        int p01 = f[(y0 + 1) * w + x0];
                        int p11 = f[(y0 + 1) * w + (x0 + 1)];

                        double interpolated =
                            (1 - dx) * (1 - dy) * p00 +
                            dx * (1 - dy) * p10 +
                            (1 - dx) * dy * p01 +
                            dx * dy * p11;

                        g[y_new * nw + x_new] = (int)std::round(interpolated);
                    }
                }
            }
        }

    }


    //胡椒鹽濾波
    __declspec(dllexport)void salt_and_pepper(int* f, int w, int h, int* g) {
        std::vector<int> tmp_g(h * w);
        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                if (j + 2 > w - 1) {
                    tmp_g[i * w + j] = f[i * w + j];  //邊界處不做處理
                }
                else {
                    int tmp[3];
                    tmp[0] = f[i * w + j];  //取三個值
                    tmp[1] = f[i * w + j + 1];  //取三個值
                    tmp[2] = f[i * w + j + 2];  //取三個值
                    std::sort(tmp, tmp + 3); //取三個值排序
                    tmp_g[i * w + j] = tmp[1]; //取中間值
                }
            }
        }

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                if (j + 2 > h - 1) {
                    g[j * w + i] = tmp_g[j * w + i];  //邊界處不做處理
                }
                else {
                    int tmp[3];
                    tmp[0] = tmp_g[j * w + i];  //取三個值
                    tmp[1] = tmp_g[j * w + i + 1];  //取三個值
                    tmp[2] = tmp_g[j * w + i + 2];  //取三個值
                    std::sort(tmp, tmp + 3); //取三個值排序
                    g[j * w + i] = tmp[1]; //取中間值
                }
            }

        }
	}

    extern "C" __declspec(dllexport)int Otsu(int* f, int w, int h, int* g, int histSize) {
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
        MessageBoxA(NULL, "Otsu called!", "Debug", MB_OK);
        return 123; // 返回最佳閥值
    }
    //放大
    __declspec(dllexport) void scale_up_bilinear(int* f, int w, int h, int* g, int new_w, int new_h, double scale_factor) {
        if (scale_factor <= 1.0) return; 

        new_w = static_cast<int>(w * scale_factor);
        new_h = static_cast<int>(h * scale_factor);

        double scale_x = static_cast<double>(w) / new_w;
        double scale_y = static_cast<double>(h) / new_h;

        for (int y = 0; y < new_h; y++) {
            for (int x = 0; x < new_w; x++) {
                double src_x = x * scale_x;
                double src_y = y * scale_y;

                int x0 = static_cast<int>(src_x);
                int y0 = static_cast<int>(src_y);
                int x1 = min(x0 + 1, w - 1);
                int y1 = min(y0 + 1, h - 1);

                double dx = src_x - x0;
                double dy = src_y - y0;

                int p00 = f[y0 * w + x0];
                int p10 = f[y0 * w + x1];
                int p01 = f[y1 * w + x0];
                int p11 = f[y1 * w + x1];

                double top = p00 * (1 - dx) + p10 * dx;
                double bottom = p01 * (1 - dx) + p11 * dx;
                double value = top * (1 - dy) + bottom * dy;

                g[y * (new_w) + x] = static_cast<int>(value + 0.5);
            }
        }
    }
    //縮小
    __declspec(dllexport) void scale_down_bilinear(int* f, int w, int h, int* g, int new_w, int new_h, double shink_factor) {
        double scale_x = (double)w / new_w;
        double scale_y = (double)h / new_h;

        for (int y = 0; y < new_h; y++) {
            for (int x = 0; x < new_w; x++) {
                double src_x = x * scale_x;
                double src_y = y * scale_y;

                int x0 = (int)src_x;
                int y0 = (int)src_y;
                int x1 = min(x0 + 1, w - 1);
                int y1 = min(y0 + 1, h - 1);

                double dx = src_x - x0;
                double dy = src_y - y0;

                int p00 = f[y0 * w + x0];
                int p10 = f[y0 * w + x1];
                int p01 = f[y1 * w + x0];
                int p11 = f[y1 * w + x1];

                double top = p00 * (1 - dx) + p10 * dx;
                double bottom = p01 * (1 - dx) + p11 * dx;
                double value = top * (1 - dy) + bottom * dy;
                g[y * new_w + x] = (int)(value + 0.5);
            }
        }
    }
    __declspec(dllexport) int connected_component(int* f, int w, int h) {

		std::vector<int> labels(w * h, -1); // 初始化標記陣列，-1表示未標記
        
		int label = 0; // 標記計數器
		int nx, ny; // 相鄰像素的座標
        int dx[4] = { -1, 1, 0, 0 };
        int dy[4] = { 0, 0, -1, 1 };
        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++) {
                if (f[i * w + j] == 0) {
                    continue;  // 如果不是 255，就直接跳過
                }
				bool b = false; // 用於檢查是否有相鄰的前景像素
                int b_value = -1;
                for (size_t z = 0; z < 4; z++)
                {
					nx = j + dx[z]; // 計算相鄰像素的 x 座標
					ny = i + dy[z]; // 計算相鄰像素的 y 座標
					if (nx < 0 || nx >= w || ny < 0 || ny >= h) continue; // 邊界檢查

                    if (labels[ny * w + nx] > 0)
                    {
                        b = true;
                        b_value = labels[ny * w + nx];
                        break;
                    }
                    
                }
                if (f[i * w + j] > 0 && labels[i * w + j] == -1) {
                    if (b == true)
                    {
						labels[i * w + j] = b_value; // 標記當前像素為相鄰的前景像素值
                    }
                    else
                    {
                        label++;
                        labels[i * w + j] = label;
                    }
                }
            }
        }
		return label; // 返回標記數量
    }
 

// 在輸出影像 g 上畫線時的顏色（灰階值）。可自行調整。
// 0 表示黑，255 表示白。這裡我們用白色 (255) 來畫出偵測到的直線。

    __declspec(dllexport) int hough_line_transform(int* f, int w, int h, int* g) {
        const int max_r = (int)std::floor(std::sqrt(w * w + h * h)) + 1;
        //int labels[180][max_r * 2] = { 0 }; //第一欄是r 第二欄是角度
        std::vector<std::vector<int>> labels(180, std::vector<int>(max_r * 2, 0));
        int r;
        double theta;
        int ct = 0;
        for (size_t i = 0; i < h * w; i++)
        {
            g[i] = 0;
        }

        for (size_t i = 0; i < h; i++)
        {
            for (size_t j = 0; j < w; j++) { 
                //如果是黑色就跳下一個
                if (f[i * w + j] == 0)continue;
                
                //計算r和ʘ的值
                for (size_t b = 0; b < 180; b++) {
                    theta = b * std::numbers::pi / 180;
                    r = (int)std::floor(j * cos(theta) + i * sin(theta));
                    if (r >= 0)
                    {
                        labels[b][r]++;
                    }
                    else if (r < 0)
                    {
                        labels[b][-r + max_r]++;
                    }

                }
            }
        }

        //for (size_t i = 0; i < 180; i++)
        //{
        //    for (size_t j = 0; j < 2 * max_r; j++)
        //    {
        //        if (labels[i][j] > 41) {
        //            ct++;
        //            for (size_t y = 0; y < h; y++)
        //            {
        //                for (size_t x = 0; x < w; x++)
        //                {
        //                    //if (f[y * w + x] == 0)continue;
        //                    theta = i * std::numbers::pi / 180;
        //                    int tmp = (int)std::round(x * cos(theta) + y * sin(theta));
        //                    if (tmp < 0)
        //                    {
        //                        tmp =  -tmp + max_r;
        //                    }
        //                    if (j == tmp)
        //                    {
        //                        g[y * w + x] = 255;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        for (size_t i = 0; i < 180; i++) { // θ 從 0 到 179 度
            for (size_t j = 0; j < 2 * max_r; j++) { // r 的範圍
                if (labels[i][j] > 41) { // 投票數超過閾值
                    // 檢查是否為局部最大值
                    bool is_peak = true;
                    // 檢查 θ 和 r 方向 ±1 的鄰域
                    for (int di = -4; di <= 4 && is_peak; di++) {
                        for (int dj = -4; dj <= 4; dj++) {
                            if (di == 0 && dj == 0) continue; // 跳過自己
                            size_t ni = i + di; // 鄰居的 θ
                            size_t nj = j + dj; // 鄰居的 r
                            // 確保不超出範圍
                            if (ni >= 0 && ni < 180 && nj >= 0 && nj < 2 * max_r) {
                                if (labels[ni][nj] > labels[i][j]) {
                                    is_peak = false; // 發現更大的鄰居
                                    labels[i][j] = 0;
                                    break;
                                }
                            }
                        }
                    }
                    if (is_peak) { // 真的是局部最大值
                        // 在這裡畫線...
                        // 例如：用 (θ=i°, r=j) 計算並畫出直線
                        ct++;
                        for (size_t y = 0; y < h; y++)
                        {
                            for (size_t x = 0; x < w; x++)
                            {
                                //if (f[y * w + x] == 0)continue;
                                theta = i * std::numbers::pi / 180;
                                int tmp = (int)std::round(x * cos(theta) + y * sin(theta));
                                if (tmp < 0)
                                {
                                    tmp =  -tmp + max_r;
                                }
                                if (j == tmp)
                                {
                                    g[y * w + x] = 255;
                                }
                            }
                        }
                    }
                }
            }
        }

        return ct;
    }

    __declspec(dllexport) int hough_circle_transform(int* f, int w, int h, int* g) {
        // label[a][b][r]
        const int A = w, B = h, R = 100;
        std::vector<std::vector<std::vector<int>>> labels(A, std::vector<std::vector<int>>(B, std::vector<int>(R, 0)));
        int ct = 0;
        for (size_t i = 0; i < w * h; i++)
        {
            g[i] = 0;
        }

        for (size_t y = 0; y < h; y++)
        {
            for (size_t x = 0; x < w; x++)
            {        
                if (f[y * w + x] != 255) continue;
                for (size_t r = 20; r < R; r+=1)
                {
                    for (size_t theta = 0; theta < 360; theta+=1)
                    {
                        int a = x - (int)std::round(r * std::cos(theta * std::numbers::pi / 180)); // x
                        int b = y - (int)std::round(r * std::sin(theta * std::numbers::pi / 180)); // y
                        if (a < 0 || a >= w || b < 0 || b >= h) continue;                        
                        labels[a][b][r]++;                        
                    }
                }
            }
        }
        
        for (size_t i = 0; i < A; i++)
        {
            for (size_t j = 0; j < B; j++)
            {
                for (size_t k = 3; k < R; k++)
                {
                    if (labels[i][j][k] > 3 * k)
                    {
                        ct++;
                    }
                    else { continue; }
                    for (size_t theta = 0; theta < 360; theta++)
                    {                                        
                        int x = (int)std::round(k * std::cos(theta * std::numbers::pi / 180)) + i;
                        int y = (int)std::round(k * std::sin(theta * std::numbers::pi / 180)) + j;
                        if (x < 0 || x >= w || y < 0 || y >= h) continue;
                        g[y * w + x] = 255;                    
                    }
                }
            }
        }
   
        return ct;   
    }

}