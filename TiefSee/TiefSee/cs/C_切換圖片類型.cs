using TiefSee.W;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace TiefSee {



    public partial class MainWindow {



        /// <summary>
        /// 
        /// </summary>
        public void fun_切換_隱藏web() {

            if (w_web == null) {//如果還沒初始化就不執行
                return;
            }


            //img_web.Visible = false;
            //img_web_box.Visibility = Visibility.Collapsed;
            //w_web.Visibility = Visibility.Collapsed;



            w_web.Visible = false;
            img_web.Document.InvokeScript("fun_open_imgbox", new Object[] { "", "300", "300" });

        }

        /// <summary>
        /// 
        /// </summary>
        public void fun_切換_隱藏gif() {
            WpfAnimatedGif.ImageBehavior.SetAutoStart(img_gif, false);
            img_gif.Visibility = Visibility.Collapsed;
            WpfAnimatedGif.ImageBehavior.SetAnimatedSource(img_gif, bitmapImage_none);
        }


        /// <summary>
        /// 
        /// </summary>
        public void fun_切換_隱藏voide() {
            img_voide.Visibility = Visibility.Collapsed;
            img_voide.Source = new Uri(fun_執行檔路徑() + "\\data\\imgs\\none.jpg");
            img_voide.LoadedBehavior = MediaState.Stop;
        }


        /// <summary>
        /// 
        /// </summary>
        public void fun_切換_隱藏jpg() {
            img.Visibility = Visibility.Collapsed;
            img.Source = bitmapImage_none;
        }











        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public void fun_顯示錯誤圖片(String path) {

            c_旋轉.fun_初始化旋轉(0);

            //載入圖片
            BitmapSource bitimg = null;

            try {

                //取得圖片在Windows系統的縮圖
                var icon = c_影像.func_作業系統圖示(path);

                bitimg = c_影像.BitmapToBitmapSource(icon);
                //顯示寬高
                int_img_w = bitimg.PixelWidth;
                int_img_h = bitimg.PixelHeight;
                text_imgSize.Text = int_img_w + " \n" + int_img_h;

            } catch {

                bitimg = c_影像.func_get_BitmapImage_JPG(fun_執行檔路徑() + "\\data\\imgs\\error.png");
                //顯示寬高
                text_imgSize.Text = " ??? \n ??? ";
                int_img_w = bitimg.PixelWidth;
                int_img_h = bitimg.PixelHeight;
            }


            //設定顯示圖片需要的參數
            img.Visibility = Visibility.Visible;
            img.Source = bitimg;

            fun_切換_隱藏web();
            fun_切換_隱藏gif();
            fun_切換_隱藏voide();
            //fun_切換_隱藏jpg();

            c_P網.fun_暫停();

            fun_圖片全滿(true);

        }






        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public void fun_顯示圖片(String path) {

            //強制視窗取得焦點
            if (bool_空白鍵預覽的視窗 == false) {
                this.Activate();
            }

            image_局部高清 = null;
            bool_啟動局部高清 = false;
            func_隱藏局部高清();

            fun_顯示圖片_核心(path);



            //----------------------------------
            //圖片載入完後


            //判斷什麼按鈕要鎖定，什麼按鈕要啟用
            try {
                if (text_imgType.Text.ToUpper().Contains("GIF")) {
                    u_解析動圖.func_類型_一般();
                } else {
                    u_解析動圖.func_類型_鎖定();
                }

                if (text_imgType.Text.ToUpper().Contains("PIXIV")) {
                    u_轉存GIF.func_類型_一般();
                } else {
                    u_轉存GIF.func_類型_鎖定();
                }
            } catch (System.NullReferenceException) { }




            //如果是data資料夾裡面的圖片，就隱藏工具列的圖片資訊
            if (path.IndexOf(fun_執行檔路徑() + "\\data\\imgs") == 0) {
                b_垂直線_旋轉與縮放.Opacity = 0;
                text_imgSize.Opacity = 0;
                border_垂直線_2.Opacity = 0;
                text_imgType.Opacity = 0;
                border_垂直線_3.Opacity = 0;
                text_imgTime.Opacity = 0;
            } else {
                b_垂直線_旋轉與縮放.Opacity = 1;
                text_imgSize.Opacity = 1;
                border_垂直線_2.Opacity = 1;
                text_imgType.Opacity = 1;
                border_垂直線_3.Opacity = 1;
                text_imgTime.Opacity = 1;
            }



            if (fun_取得記憶體用量() > 350)
                fun_清理記憶體();
        }






        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        private void fun_顯示圖片_核心(String path) {

            lab_title.ToolTip = path;




            //取消文字框的焦點
            if (stackPlanel_動圖工具.Visibility == Visibility.Visible) {
                fun_主視窗取得焦點();
            }

            int int_svg_w = 0;
            int int_svg_h = 0;

            c_旋轉.fun_初始化旋轉(0);

            //DateTime time_start = DateTime.Now;//計時開始 取得目前時間



            //避免某些檔案已經被刪除
            if (File.Exists(path) == false && Directory.Exists(path) == false) {

                ar_path = new List<string>();
                ar_path = fun_取得圖片名單(path);

                //避免超出陣列
                if (int_目前圖片位置 >= ar_path.Count) {
                    int_目前圖片位置 = ar_path.Count - 1;
                }

                //如果資料夾是空的
                if (ar_path.Count == 0) {
                    fun_載入圖片或資料夾(fun_執行檔路徑() + "\\data\\imgs\\start.png");
                    return;
                }

                fun_顯示圖片(ar_path[int_目前圖片位置]);
                return;
            }

            if (ar_path == null) {
                fun_載入圖片或資料夾(fun_執行檔路徑() + "\\data\\imgs\\start.png");
                return;
            }

            //設定title
            String name = path.Substring(path.LastIndexOf("\\") + 1);
            lab_title.Text = "『" + (int_目前圖片位置 + 1) + "/" + ar_path.Count + "』 " + name;
            Title = lab_title.Text;

            t_選單_資料夾名稱.Text = Directory.GetParent(path).Name;

            //判斷類型與檔案大小
            s_img_type_顯示類型 = c_影像.fun_取得附檔名(path);
            s_img_type_附檔名 = s_img_type_顯示類型;

            String file_size = "???";




            if (s_img_type_附檔名 == "GIF") {

                /*if (path.Substring(path.Length - 3).ToUpper().Equals("GIF") == false) {

                    //因為IE無法解析附檔名不是GIF的GIF，所以如果附檔名不是GIF，就不要用IE

                } else */
                
                if (e_GIF_Type == E_GIF_type.WPF && c_影像.fun_判斷檔案大小_MB(path) < 50) {

                    //用WPF渲染，且檔案小於50M

                    int int_幀數 = new GifBitmapDecoder(new Uri(path), BitmapCreateOptions.None, BitmapCacheOption.Default).Frames.Count;
                    if (int_幀數 >= 600) {//如果幀數太多，使用wpf渲染會無法顯示
                        if (w_web == null) {
                            c_web.fun_web初始化(path);
                            return;
                        }
                        s_img_type_顯示類型 = "WEB";
                    }

                } else {

                    //使用webbrowser來顯示

                    if (w_web == null) {
                        c_web.fun_web初始化(path);
                        return;
                    }
                    s_img_type_顯示類型 = "WEB";
                }

            }//gif



            try {
                file_size = c_影像.fun_判斷檔案大小(path);
            } catch { }

            if (s_img_type_附檔名 == "PNG") {
                if (c_apng.fun_載入並判斷是否APNG(path)) {
                    s_img_type_顯示類型 = "APNG";
                    s_img_type_附檔名 = "APNG";
                }
            }





            //如果是EXE
            if (s_img_type_附檔名 == "EXE" || s_img_type_附檔名 == "LNK") {

                //類型與時間
                text_imgType.Text = "EXE" + "\n" + file_size;
                text_imgTime.Text = File.GetLastWriteTime(path).ToString("yyyy/MM/dd\nHH:mm:ss");


                c_P網.fun_播放_EXE(path);

                //顯示jpg物件
                img.Visibility = Visibility.Visible;

                fun_切換_隱藏web();
                fun_切換_隱藏gif();
                fun_切換_隱藏voide();
                //fun_切換_隱藏jpg();
                fun_顯示背景顏色();

                return;
            }




            //如果是ICO
            if (s_img_type_附檔名 == "ICO") {

                //類型與時間
                text_imgType.Text = "ICO" + "\n" + file_size;
                text_imgTime.Text = File.GetLastWriteTime(path).ToString("yyyy/MM/dd\nHH:mm:ss");


                c_P網.fun_播放_ICON(path);

                //顯示jpg物件
                img.Visibility = Visibility.Visible;

                fun_切換_隱藏web();
                fun_切換_隱藏gif();
                fun_切換_隱藏voide();
                //fun_切換_隱藏jpg();
                fun_顯示背景顏色();

                return;
            }


            //如果是APNG
            if (s_img_type_顯示類型 == "APNG") {

                //類型與時間
                text_imgType.Text = "APNG" + "\n" + file_size;
                text_imgTime.Text = File.GetLastWriteTime(path).ToString("yyyy/MM/dd\nHH:mm:ss");


                c_P網.fun_播放_APNG(path);


                s_img_type_顯示類型 = "JPG";

                //顯示jpg物件
                img.Visibility = Visibility.Visible;


                fun_切換_隱藏web();
                fun_切換_隱藏gif();
                fun_切換_隱藏voide();
                //fun_切換_隱藏jpg();

                fun_顯示背景顏色();

                return;
            }


            //如果是資料夾，就當做是 P網 連續圖來播放
            if (Directory.Exists(path)) {

                //類型與時間
                text_imgType.Text = "pixiv動圖" + "\n" + file_size;
                text_imgTime.Text = File.GetLastWriteTime(path).ToString("yyyy/MM/dd\nHH:mm:ss");

                c_P網.fun_播放(path);


                //顯示寬高
                String s_連續圖的第一張 = "";
                var arp = new DirectoryInfo(path).GetFiles();
                foreach (var item in arp) {
                    String ne= Path.GetExtension(item.FullName).ToLower();
                    if (ne == ".jpg" || ne == ".png" || ne == ".gif" || ne == ".jpeg" || ne == ".jfif" || ne == ".bpm") {
                        s_連續圖的第一張 = item.FullName;
                        break;
                    }
                }
                if (s_連續圖的第一張 == "") {
                    fun_顯示錯誤圖片(arp[0].FullName);
                   return;
                } 

                var bitimg2 = c_影像.func_get_BitmapImage_JPG(s_連續圖的第一張);
                var img_width2 = (int)bitimg2.PixelWidth;
                var img_height2 = (int)bitimg2.PixelHeight;
                fun_設定顯示圖片size(img_width2, img_height2);//顯示寬高

                s_img_type_顯示類型 = "JPG";

                //顯示jpg物件
                img.Visibility = Visibility.Visible;

                fun_切換_隱藏web();
                fun_切換_隱藏gif();
                fun_切換_隱藏voide();
                //fun_切換_隱藏jpg();


                fun_圖片全滿(true);
                fun_顯示背景顏色();

                return;
            }







            ///如果是ZIP的話，就當做 P網 連續圖來播放
            if (s_img_type_附檔名 == "ZIP") {


                //類型與時間
                text_imgType.Text = "pixiv動圖" + "\n" + file_size;
                text_imgTime.Text = File.GetLastWriteTime(path).ToString("yyyy/MM/dd\nHH:mm:ss");

                //判斷是否為可以解析的P網動態圖
                bool bool_無法解析zip = false;
                try {
                    using (var x = ZipFile.OpenRead(path)) {
                        foreach (var item2 in x.Entries) {
                            if (item2.FullName.Equals("animation.json")) {
                                bool_無法解析zip = true;
                                break;
                            }
                        }//foreach
                    }//suing
                } catch { }

                if (bool_無法解析zip == false) {//無法解析就離開

                    text_imgType.Text = "zip" + "\n" + file_size;
                    fun_顯示錯誤圖片(path);
                    return;

                } else {//可以解析就播放

                    c_P網.fun_播放_zip(path);

                }

                s_img_type_顯示類型 = "JPG";

                //顯示jpg物件
                img.Visibility = Visibility.Visible;



                fun_切換_隱藏web();
                fun_切換_隱藏gif();
                fun_切換_隱藏voide();
                //fun_切換_隱藏jpg();

                fun_顯示背景顏色();
                return;
            }


            //-------------------------

            c_P網.fun_暫停();

            text_imgType.Text = s_img_type_附檔名 + "\n" + file_size;
            text_imgTime.Text = File.GetLastWriteTime(path).ToString("yyyy/MM/dd\nHH:mm:ss");



            try {

                //載入圖片
                BitmapSource bitimg = null;


                //顯示寬高
                int img_width = 500;
                int img_height = 500;


                if (s_img_type_附檔名 == "SVG") {

                    if (w_web == null) {
                        c_web.fun_web初始化(path);
                        return;
                    }

                    s_img_type_顯示類型 = "WEB";
                    text_imgSize.Text = " ??? \n ??? ";

                    //判斷SVG的長寬
                    int[] ar_wh = func_get_svg_size(path);
                    int_svg_w = ar_wh[0];
                    int_svg_h = ar_wh[1];
                    fun_設定顯示圖片size(int_svg_w, int_svg_h);


                } else if (s_img_type_附檔名 == "AVI" || s_img_type_附檔名 == "MP4" || s_img_type_附檔名 == "WEBM" ||
                    s_img_type_附檔名 == "MKV" || s_img_type_附檔名 == "MTS" || s_img_type_附檔名 == "MPG") {

                    s_img_type_顯示類型 = "MOVIE";
                    text_imgSize.Text = " ??? \n ??? ";

                } else {


                    bitimg = c_影像.func_get_BitmapImage_更新界面(path, ref img_width, ref img_height);

                    //顯示寬高
                    //img_width = (int)bitimg.PixelWidth;
                    //img_height = (int)bitimg.PixelHeight;
                    fun_設定顯示圖片size(img_width, img_height); //顯示寬高
                }


                //如果圖片太大，就改用web瀏覽
                /*if (s_img_type_顯示類型 == "PNG" || s_img_type_顯示類型 == "JPG" || s_img_type_顯示類型 == "BMP") {
                    if (img_width > 7000 || img_height > 7000) {
                        s_img_type_顯示類型 = "WEB";
                    }
                }*/


                /*if (file_size.Contains("MB")) {
                    if (double.Parse(file_size.Replace("MB", "")) > 3) {
                        s_img_type = "MOVIE";
                    }
                }*/



                if (s_img_type_顯示類型 == "WEB") {

                    //隱藏exif視窗
                    c_影像.c_exif.fun_初始化exif資訊();
                    c_影像.c_exif.fun_顯示或隱藏視窗();

                    if (w_web == null) {
                        c_web.fun_web初始化(path);
                        return;
                    }


                    w_web.Left = (int)((b.PointToScreen(new Point(0, 0)).X + 5));
                    w_web.Top = (int)((b.PointToScreen(new Point(0, 0)).Y));
                    w_web.Width = (int)((b2.ActualWidth - 10) * d_解析度比例_x);
                    w_web.Height = (int)((b.ActualHeight - 5) * d_解析度比例_y);


                    if (img_web.Visible == false) {
                        w_web.Visible = true;
                    }

                    fun_主視窗取得焦點();
                    new Thread(() => {//延遲載入，避免顯示不出東西
                        Thread.Sleep(10);
                        C_adapter.fun_UI執行緒(() => {

                            //如果只是一般圖片，就直接傳入SZIE，在第一時將圖片定位
                            if (int_svg_w == 0 && int_svg_h == 0) {
                                int_svg_w = img_width;
                                int_svg_h = img_height;
                            }

                            fun_主視窗取得焦點();
                            String s_url_path = "http://localhost:" + c_localhost.port + "/img_path/" + Uri.EscapeDataString(path) + "*" + DateTime.Now.ToString("yyyyMMddHHmmssffff") ;
                            img_web.Document.InvokeScript("fun_open_imgbox", new Object[] { s_url_path, int_svg_w.ToString(), int_svg_h.ToString() });
                            //img_web.Document.Focus();

                            Log.print(s_url_path);
                        });
                    }).Start();

                    //fun_切換_隱藏web();
                    fun_切換_隱藏gif();
                    fun_切換_隱藏voide();
                    fun_切換_隱藏jpg();

                    //text_imgSize.Focus();

                } else if (s_img_type_顯示類型 == "GIF") {

                    //隱藏exif視窗
                    c_影像.c_exif.fun_初始化exif資訊();
                    c_影像.c_exif.fun_顯示或隱藏視窗();

                    WpfAnimatedGif.ImageBehavior.SetAutoStart(img_gif, true);
                    img_gif.Visibility = Visibility.Visible;
                    WpfAnimatedGif.ImageBehavior.SetAnimatedSource(img_gif, bitimg);

                    fun_切換_隱藏web();
                    //fun_切換_隱藏gif();
                    fun_切換_隱藏voide();
                    fun_切換_隱藏jpg();

                } else if (s_img_type_顯示類型 == "MOVIE") {


                    //隱藏exif視窗
                    c_影像.c_exif.fun_初始化exif資訊();
                    c_影像.c_exif.fun_顯示或隱藏視窗();


                    fun_切換_隱藏web();
                    fun_切換_隱藏gif();
                    //fun_切換_隱藏voide();
                    fun_切換_隱藏jpg();

                    img_voide.Visibility = Visibility.Visible;
                    String path_v = func_取得影片檔案路徑(path);
                    img_voide.Source = new Uri(path_v);



                    img_voide.LoadedBehavior = MediaState.Play;
                    //MessageBox.Show(s_url_path);


                } else {//一般圖片


                    img.Visibility = Visibility.Visible;

                    //顯示圖片
                    /*if (img_width > 9999 && (s_img_type_顯示類型 == "PNG")) {//縮小圖片後在載入
                        img.Source = c_影像.fun_get_BitmapImage_max(path, "w", 9999);
                    } else if (img_height > 9999 && s_img_type_顯示類型 == "PNG") {
                        img.Source = c_影像.fun_get_BitmapImage_max(path, "h", 9999);
                    } else {
                        img.Source = bitimg;//直接顯示
                    }*/

                    //如果圖片太大，就把圖片縮小到視窗範圍的size
                    if (s_img_type_附檔名 == "PNG" || s_img_type_附檔名 == "JPG" || s_img_type_附檔名 == "TIF" || s_img_type_附檔名 == "BMP") {
                        if (img_width > scrollviewer_1.ViewportWidth || img_height > scrollviewer_1.ViewportHeight) {//縮小圖片後在載入
                            //2500 
                            var w03 = scrollviewer_1.ViewportWidth;
                            var h03 = scrollviewer_1.ViewportHeight;

                            try {
                                if (img_width / w03 > img_height / h03) {
                                    img.Source = c_影像.fun_get_BitmapImage_max(path, "w", (int)w03);
                                } else {
                                    img.Source = c_影像.fun_get_BitmapImage_max(path, "h", (int)h03);
                                }
                            } catch {

                                img.Source = bitimg;//直接顯示
                            }


                        } else {
                            img.Source = bitimg;//直接顯示
                        }

                    } else {
                        img.Source = bitimg;//直接顯示
                    }

                    fun_切換_隱藏web();
                    fun_切換_隱藏gif();
                    fun_切換_隱藏voide();
                    //fun_切換_隱藏jpg();
                    bool_啟動局部高清 = true;
                }


            } catch (Exception e) {

                //MessageBox.Show("fdf\n"+e.ToString());

                fun_顯示錯誤圖片(path);

            }


            fun_圖片全滿(true);




            fun_顯示背景顏色();

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private String func_取得影片檔案路徑(String path) {



            //不是 webm 的話，就直接回傳檔案的路徑
            if (Path.GetExtension(path).ToUpper() != ".WEBM") {
                return path;
            }

            //如果檔案大於500M，怎直接回傳檔案，避免複製過大的影片
            if (c_影像.fun_判斷檔案大小_MB(path) > 500) {
                return path;
            }

            /*
            String s_url_path = "http://localhost:" + c_localhost.port + "/img_path/" + Uri.EscapeDataString(path);
            img_voide.Source = new Uri(s_url_path);
            System.Console.WriteLine("\n\n\n"+s_url_path +"\n\n\n");*/


            //
            //
            //
            //如果是 webm 的話，附檔名必須是 mkv 才能正常播放，所以把檔案複製到暫存路徑
            String s_path_webm = Path.Combine(func_取得暫存路徑(), "webm");
            if (Directory.Exists(s_path_webm) == false) {//避免資料夾不存在
                Directory.CreateDirectory(s_path_webm);
            }
            String s_name = c_影像.func_雜湊檔案(path);
            s_path_webm = Path.Combine(s_path_webm, s_name + ".mkv");


            if (File.Exists(s_path_webm) == false) {//避免檔案已經存在
                File.Copy(path, s_path_webm);
            }



            return s_path_webm;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public int[] func_get_svg_size(String path) {


            Func<String, int> func_取得_int = (String s24) => {
                int i023 = 0;
                s24 = s24.Trim().ToUpper();
                s24 = s24.Replace("C", "").Replace("M", "").Replace("P", "")
                        .Replace("X", "").Replace("I", "").Replace("N", "")
                        .Replace("E", "").Replace("P", "").Replace("T", "")
                        .Replace("V", "").Replace("W", "").Replace("H", "")
                        .Replace("A", "").Replace("%", "");

                i023 = (int)float.Parse(s24);

                return i023;
            };



            int int_svg_w = 50;
            int int_svg_h = 50;

            using (StreamReader sr = new StreamReader(path, Encoding.UTF8)) {

                string input = sr.ReadToEnd();

                //<svg viewBox="0 0 50 50"
                try {
                    string pattern = "viewbox[ ]*=[ ]*([\"][^a-z]*[\"]|['][^a-z]*['])";
                    String s01 = Regex.Matches(input, pattern, RegexOptions.IgnoreCase)[0].Value;

                    s01 = s01.Substring(s01.IndexOf('=') + 1);
                    s01 = s01.Replace("\"", "").Replace("'", "");
                    s01 = s01.Trim();
                    if (s01.Contains(",")) {
                        s01 = s01.Replace(" ", "");
                    } else {
                        s01 = s01.Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ");
                        s01 = s01.Replace(" ", ",");
                    }

                    String[] s02 = s01.Split(',');
                    int_svg_w = func_取得_int(s02[2]);
                    int_svg_h = func_取得_int(s02[3]);

                    return new int[] { int_svg_w, int_svg_h };

                } catch {

                }


                //<svg width="1500" height="727"
                try {

                    string pattern = "<svg.*>";
                    String s01 = Regex.Matches(input, pattern, RegexOptions.IgnoreCase)[0].Value;

                    if (s01.Length > 10) {
                        //取得width
                        pattern = "width[ ]*=[ ]*([\"][^a-z]*[\"]|['][^a-z]*['])";
                        s01 = Regex.Matches(input, pattern, RegexOptions.IgnoreCase)[0].Value;
                        s01 = s01.Substring(s01.IndexOf('=') + 1);
                        s01 = s01.Replace("\"", "").Replace("'", "");
                        int_svg_w = func_取得_int(s01);

                        //取得height
                        pattern = "height[ ]*=[ ]*([\"][^a-z]*[\"]|['][^a-z]*['])";
                        s01 = Regex.Matches(input, pattern, RegexOptions.IgnoreCase)[0].Value;
                        s01 = s01.Substring(s01.IndexOf('=') + 1);
                        s01 = s01.Replace("\"", "").Replace("'", "");
                        int_svg_h = func_取得_int(s01);
                    }
                } catch { }
            }

            return new int[] { int_svg_w, int_svg_h };

        }






    }//class




}
