using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace TiefSee {


    public partial class MainWindow {


        public MagickImage image_局部高清 = null;
        //public IMagickImage image_局部高清_50 = null;
        public bool bool_啟動局部高清 = true;
        //public DateTime s_局部高清_處理方式 = DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        public void func_局部高清_初始化() {

            String s_path = "";//圖片路徑
            double d_倍率 = 1;//縮放倍率
            double d_倍率_old = 1;//超過500毫秒沒有進行縮放才重新算圖
            double d_x = 1;//圖片位置
            double d_y = 1;
            double d_window_w = 1;//視窗寬度
            double d_window_h = 1;
            int int_旋轉角度 = 0;
            bool bool_剪裁 = false;

            DateTime time_避免捲動中算圖 = DateTime.Now;//

           

            //-------------

            new Thread(() => {

                while (bool_程式運行中) {

                    //-------------

                    Thread.Sleep(100);

                    if (u_大量瀏覽模式 != null) {
                        s_path = "";
                        continue;
                    }

                    if (bool_啟動局部高清 == false) {
                        s_path = "";
                        continue;
                    }

                    //在程式預設開啟的資料夾不啟用
                    if (int_目前圖片位置 < ar_path.Count)
                        if (ar_path[int_目前圖片位置].IndexOf(fun_執行檔路徑() + "\\data\\imgs") == 0) {
                            s_path = "";
                            continue;
                        }

                    /*
                    if (this.Visibility != Visibility.Visible) {
                        //s_path = "";
                        Thread.Sleep(100);
                        continue;
                    }*/


                    /*
                    if (s_img_type_附檔名 == "PNG" || s_img_type_附檔名 == "JPG" ||
                        s_img_type_附檔名 == "TIF" || s_img_type_附檔名 == "BMP" ||
                        image_局部高清 != null) {

                    } else {
                        continue;
                    }*/



                    bool bool_重新繪製 = false;
                    bool bool_新圖片 = false;
                    bool bool_不剪裁且不重新繪製 = false;


                    C_adapter.fun_UI執行緒(() => {

                        try {



                            if (d_倍率 == img.ActualWidth && bool_剪裁 == false) {
                                //System.Console.WriteLine("不剪裁且不重新繪製: " + ar_path[int_目前圖片位置]);
                                bool_不剪裁且不重新繪製 = true;
                            }

                            if (s_path != ar_path[int_目前圖片位置]) {

                                //System.Console.WriteLine("新圖片: " + ar_path[int_目前圖片位置]);

                                s_path = ar_path[int_目前圖片位置];
                                bool_新圖片 = true;
                                bool_不剪裁且不重新繪製 = false;
                                bool_重新繪製 = true;

                                //讓新圖片一定能立即運算
                                d_倍率 = 0.123456;
                                d_倍率_old = img.ActualWidth;
                            }

                            if (d_倍率_old == img.ActualWidth) {
                                //bool_重新繪製 = false;

                                if (d_倍率 != img.ActualWidth ||
                                  d_x != scrollviewer_1.ContentHorizontalOffset || d_y != scrollviewer_1.ContentVerticalOffset ||
                                  d_window_w != this.ActualWidth || d_window_h != this.ActualHeight ||
                                  int_旋轉角度 != c_旋轉.int_旋轉) {

                                    d_倍率 = img.ActualWidth;

                                    d_x = scrollviewer_1.ContentHorizontalOffset;
                                    d_y = scrollviewer_1.ContentVerticalOffset;
                                    d_window_w = this.ActualWidth;
                                    d_window_h = this.ActualHeight;
                                    int_旋轉角度 = c_旋轉.int_旋轉;

                                    bool_重新繪製 = true;
                                }

                            }

                            if (((TimeSpan)(DateTime.Now - time_避免捲動中算圖)).TotalMilliseconds < 800) {//連點低於400毫秒

                            } else {
                                d_倍率_old = img.ActualWidth;
                                time_避免捲動中算圖 = DateTime.Now;
                            }

                        } catch {

                       

                            s_path = "";
                            bool_重新繪製 = false;
                            return;
                        }

                    });


                    if (bool_不剪裁且不重新繪製) {
                        continue;
                    }
                    if (bool_重新繪製 == false) {
                        continue;
                    }



                    if (bool_新圖片) {


                        try {

                            if (File.Exists(ar_path[int_目前圖片位置])) {

                                if (image_局部高清 == null) {
                             
                               
                                    var im5 = new ImageMagick.MagickImage((ar_path[int_目前圖片位置]));
                                    //
                                    
                                    //如果讀取完圖片後，使用者已經切換到其他圖片，就不要寫入到「image_局部高清」，直接解析新圖片
                                    if (int_目前圖片位置 < 0 || int_目前圖片位置 >= ar_path.Count || s_path != ar_path[int_目前圖片位置]) {
                                        image_局部高清 = null;
                                        s_path = null;
                                        im5 = null;
                                        continue;
                                    } else {
                                        image_局部高清 = im5;
                                    }

                                    //System.Console.WriteLine("一般  " + s_path);

                                } else {

                                    //image_局部高清 = new ImageMagick.MagickImage((ar_path[int_目前圖片位置]));
                                    //System.Console.WriteLine("特殊 " + s_path);

                                }


                                //https://www.smashingmagazine.com/2015/06/efficient-image-resizing-with-imagemagick/
                                //不使用 漸進式渲染
                                image_局部高清.Interlace = Interlace.NoInterlace;


                                //剝離所有配置文件和註釋的圖像。
                                image_局部高清.Strip();

                                //雙線性插值
                                //image_局部高清.FilterType = FilterType.Hann;

                            } else {
                                s_path = "";
                                continue;
                            }
                        } catch {
                            s_path = "";
                            System.Console.WriteLine("*******局部高清 失敗");
                            continue;
                        }

                    }

                    //避免已經切換到其他圖片
                    if (int_目前圖片位置 < 0 || int_目前圖片位置 >= ar_path.Count) {
                        continue;
                    }
                    if (d_倍率 != img.ActualWidth || s_path != ar_path[int_目前圖片位置]) {
                        continue;
                    }
                    if (bool_啟動局部高清 == false) {
                        s_path = "";
                        continue;
                    }

                    double w03 = 1;
                    double h03 = 1;
                    double l03 = 1;
                    double t03 = 1;
                    double d_縮放倍率 = 1;

                    C_adapter.fun_UI執行緒(() => {

                        w03 = scrollviewer_1.ViewportWidth;
                        h03 = scrollviewer_1.ViewportHeight;
                        l03 = scrollviewer_1.ContentHorizontalOffset;
                        t03 = scrollviewer_1.ContentVerticalOffset;

                        if (c_旋轉.bool_垂直鏡像) {
                            t03 = scrollviewer_1.ScrollableHeight - t03;
                        }
                        if (c_旋轉.bool_水平鏡像) {
                            l03 = scrollviewer_1.ScrollableWidth - l03;
                        }

                        if (c_旋轉.int_旋轉 == 0) {
                            if (w03 > grid_img.ActualWidth) {
                                w03 = grid_img.ActualWidth;
                            }
                            if (h03 > grid_img.ActualHeight) {
                                h03 = grid_img.ActualHeight;
                            }
                        }

                        if (c_旋轉.int_旋轉 == 180) {
                            if (w03 > grid_img.ActualWidth) {
                                w03 = grid_img.ActualWidth;
                            }
                            if (h03 > grid_img.ActualHeight) {
                                h03 = grid_img.ActualHeight;
                            }
                            l03 = scrollviewer_1.ScrollableWidth - l03;
                            t03 = scrollviewer_1.ScrollableHeight - t03;
                        }

                        if (c_旋轉.int_旋轉 == 90) {

                            if (w03 > grid_img.ActualWidth) {
                                h03 = grid_img.ActualHeight;
                            } else {
                                h03 = scrollviewer_1.ViewportWidth;
                            }

                            if (h03 > grid_img.ActualHeight) {
                                w03 = grid_img.ActualWidth;
                            } else {
                                w03 = scrollviewer_1.ViewportHeight;
                            }

                            var zzz2 = scrollviewer_1.ScrollableWidth - l03;
                            l03 = t03;
                            t03 = zzz2;
                        }

                        if (c_旋轉.int_旋轉 == 270) {

                            if (w03 > grid_img.ActualWidth) {
                                h03 = grid_img.ActualHeight;
                            } else {
                                h03 = scrollviewer_1.ViewportWidth;
                            }

                            if (h03 > grid_img.ActualHeight) {
                                w03 = grid_img.ActualWidth;
                            } else {
                                w03 = scrollviewer_1.ViewportHeight;
                            }

                            var zzz2 = l03;
                            l03 = scrollviewer_1.ScrollableHeight - t03;
                            t03 = zzz2;
                        }

                    });


                    w03 += 50;
                    h03 += 50;

                    //避免圖片還沒完全載入
                    Thread.Sleep(30);


                    //複製物件
                    ImageMagick.IMagickImage ii = null;


                    try {

                        /*if (image_局部高清_50 != null && img.ActualWidth < int_img_w / 2) {
                            ii = image_局部高清_50.Clone();
                        } else {*/
                        if (image_局部高清 == null)
                            continue;
                        ii = image_局部高清.Clone();
                        //}


                        //計算縮放比例
                        if (img.ActualWidth > img.ActualHeight) {
                            d_縮放倍率 = img.ActualWidth / ii.Width;
                        } else {
                            d_縮放倍率 = img.ActualHeight / ii.Height;
                        }


                        bool_剪裁 = false;
                        if (d_縮放倍率 * int_img_w > 5000 || d_縮放倍率 * int_img_h > 5000) {

                            bool_剪裁 = true;

                            //剪裁
                            ii.Crop(new MagickGeometry(
                              (int)(l03 / d_縮放倍率),
                              (int)(t03 / d_縮放倍率),
                              (int)(w03 / d_縮放倍率),
                              (int)(h03 / d_縮放倍率))
                            );
                        }


                        //縮放
                        var mg = new ImageMagick.MagickGeometry();
                        mg.Height = (int)(ii.Height * d_縮放倍率);
                        mg.Width = (int)(ii.Width * d_縮放倍率);

                        DateTime time_start = DateTime.Now;//計時開始 取得目前時間

                       
                        if (int_高品質成像模式 == 1 || int_高品質成像模式 == 4) {
                            ii.Resize(mg);//縮放圖片-快
                            if (d_縮放倍率 < 1) {
                                ii.UnsharpMask(0.8, 0.8);//銳化-快速          
                            }
                            //System.Console.WriteLine($"111111111");
                        }

                        if (int_高品質成像模式 == 2) {
                            ii.Resize(mg);//縮放圖片-快
                            if (d_縮放倍率 < 1) {
                                ii.RemoveWriteMask();//沒有獨立顯卡的電腦，必須用這個語法來延遲，避免圖片顯示不出來
                                ii.UnsharpMask(0.8, 0.8);//銳化-快速 
                            }
                            //System.Console.WriteLine($"2222222");
                        }

                        if (int_高品質成像模式 == 3|| int_高品質成像模式 == 3) {
                            ii.Scale(mg);//縮放圖片-慢
                            if (d_縮放倍率 < 1) {
                                ii.Sharpen();//銳化-慢
                            }
                            //System.Console.WriteLine($"3333333");
                        }

                        
                        DateTime time_end = DateTime.Now;//計時結束 取得目前時間            
                        string result2 = ((TimeSpan)(time_end - time_start)).TotalMilliseconds.ToString();//後面的時間減前面的時間後 轉型成TimeSpan即可印出時間差
                        System.Console.WriteLine("+++++++++++++++++++++++++++++++++++" + result2 + " 毫秒");
                        

                        //ii.Sample(ii.Width,ii.Height);//品質差，速度極快
                        //ii.Extent(mg);//意義不明
                        //ii.Thumbnail(mg.Width, mg.Height);//某些情況下會很慢

                        //縮小圖片後進行銳化
                        /*if (d_縮放倍率 < 1) {     
                            //ii.RemoveWriteMask();
                            //ii.UnsharpMask(0.8, 0.8);
                            //ii.UnsharpMask(0.25, 0.25,8,0.065);
                        }*/


                        //System.Console.WriteLine($"mg {mg.Width}   ii {ii.Width}");

                        //避免已經切換到其他圖片
                        if (int_目前圖片位置 < 0 || int_目前圖片位置 >= ar_path.Count) {
                            continue;
                        }
                        if (d_倍率 != img.ActualWidth || s_path != ar_path[int_目前圖片位置]) {
                            continue;
                        }
                        if (bool_啟動局部高清 == false) {
                            s_path = "";
                            continue;
                        }


                        C_adapter.fun_UI執行緒(() => {

                            if (bool_剪裁) {

                                img_局部高清.Margin = new Thickness(
                                     ((int)(l03 / d_縮放倍率)) * d_縮放倍率,
                                     ((int)(t03 / d_縮放倍率)) * d_縮放倍率,
                                     -((int)(l03 / d_縮放倍率)) * d_縮放倍率 - 5000,
                                     -((int)(t03 / d_縮放倍率)) * d_縮放倍率 - 5000
                                 );
                                img_局部高清.Width = mg.Width;
                                img_局部高清.Height = mg.Height;
                                img_局部高清.Source = ii.ToBitmapSource();

                            } else {

                                img.Source = ii.ToBitmapSource();
                            }

                        });

                    } catch (Exception e) {

                        C_adapter.fun_UI執行緒(() => {

                            MessageBox.Show("局部高清 錯誤 \n" + e.ToString());

                        });

                    }


                    Thread.Sleep(1);



                    C_adapter.fun_UI執行緒(() => {

                        //避免已經切換到其他圖片
                        if (int_目前圖片位置 < 0 || int_目前圖片位置 >= ar_path.Count) {
                            return;
                        }
                        if (d_倍率 != img.ActualWidth || s_path != ar_path[int_目前圖片位置]) {
                            return;
                        }
                        if (bool_啟動局部高清 == false) {
                            s_path = "";
                            return;
                        }

                        if (bool_剪裁) {
                            img_局部高清.Visibility = Visibility.Visible;
                            if (ii.IsOpaque == false) {
                                img.Visibility = Visibility.Hidden;
                                //System.Console.WriteLine($"透明");
                            }
                            //System.Console.WriteLine($"剪裁");
                        } else {
                            //System.Console.WriteLine($"原圖");
                            img_局部高清.Visibility = Visibility.Collapsed;
                            img.Visibility = Visibility.Visible;
                        }



                    });


                    ii.Dispose();

                }

            }).Start();


        }





        /// <summary>
        /// 
        /// </summary>
        public void func_隱藏局部高清_移動時() {

            /*
            if (s_img_type_附檔名 == "PNG") {
                img.Visibility = Visibility.Visible;
                img_局部高清.Visibility = Visibility.Collapsed;
            }*/
        }

        /// <summary>
        /// 
        /// </summary>
        public void func_隱藏局部高清() {
            /*
            var s_img_type_附檔名 = M.s_img_type_附檔名;
            var img = M.img;
            var img_局部高清 = M.img_局部高清;*/

            //---------



            img_局部高清.Visibility = Visibility.Collapsed;

            //if (s_img_type_附檔名 == "PNG") {
            if (s_img_type_顯示類型 == "WEB" || s_img_type_顯示類型 == "MOVIE" ||
                s_img_type_顯示類型 == "GIF") {

            } else {
                img.Visibility = Visibility.Visible;
            }



            //}
        }








    }
}
