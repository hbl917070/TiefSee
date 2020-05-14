using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TiefSee.cs;
using static TiefSee.MainWindow;
using static TiefSee.C_視窗拖曳改變大小;

namespace TiefSee.W {
    /// <summary>
    /// U_大量瀏覽模式.xaml 的互動邏輯
    /// </summary>
    public partial class U_大量瀏覽模式 : UserControl {


        public MainWindow M;

        public String s_當前目錄 = "";

        String s_js_array;
        String[] ar_img_path;

        public bool bool_已經載入 = false;

        //String s_url = @"D:\GitHub\aeropic\img_view\imgs.html";//
        String s_url = "https://hbl917070.github.io/aeropic/img_view/imgs.html";



        /// <summary>
        /// 判斷是否連線
        /// 用法『 bool = InternetGetConnectedState(ref flags, 0);』
        /// </summary>
        /// <param name="lpdwFlags"></param>
        /// <param name="dwReserved"></param>
        /// <returns></returns>
        [DllImport("wininet")]
        public static extern bool InternetGetConnectedState(
           ref uint lpdwFlags,
           uint dwReserved
        );



        public U_大量瀏覽模式(MainWindow m) {
            InitializeComponent();

            this.M = m;
         
          

            //啟動時判斷是否隱藏工具列
            this.Loaded += (serder, e) => {
                M.func_顯示或隱藏工具列("no");
            };


            //---------------------------------------


          
            //---------------------------------------

            //初始化時判斷是否需要 webbrowser 最大化
            if (M.WindowState == WindowState.Maximized) {
                WindowsFormsHost_01.Margin = new Thickness(0, 0, 0, 0);
            }


            //初始網頁
            web01.Navigated += (sender, e) => {

               
                web01.Document.Focus();

                //讓web也能拖曳視窗
                web01.Document.MouseDown += ((sender2, e2) => {
                    web01.Document.Focus();

                    if (e2.MouseButtonsPressed == System.Windows.Forms.MouseButtons.Left)
                        try {
                            if (M.fun_判斷滑鼠是否在右下角()) {//讓右下角可以拖曳改變視窗大小
                                if (M.WindowState != WindowState.Maximized)
                                    M.c_視窗改變大小.ResizeWindow(ResizeDirection.BottomRight);
                            }
                        } catch { }

                });

                event_快速鍵();

            };//Navigated 網頁初始化




            //判斷是否連線
            if (true) {

                uint flags = 0x0;
                var bool_是否有網路 = true;
                try {
                    bool_是否有網路 = InternetGetConnectedState(ref flags, 0);
                } catch { }

                if (bool_是否有網路 == false) {
                    s_url = M.fun_執行檔路徑() + "/data/img_view/imgs.html";
                }
            }

            web01.Navigate(s_url);

            web01.ObjectForScripting = new C_web呼叫javaScript(this);//讓網頁允許存取C#
            M.fun_Zoom(web01, 100);//網頁比例100%



            var tim = new System.Windows.Forms.Timer();
            tim.Interval = 1000;
            tim.Tick += (sender, e) => {

                if (bool_已經載入 == false) {
                 
                    s_url = M.fun_執行檔路徑() + "/data/img_view/imgs.html";
                    web01.Navigate(s_url);
                }
                tim.Stop();
            };
            tim.Start();
            //---------------------------------------



            web01.Navigating += (sender2, e2) => {
                if (bool_已經載入) {
                    M.bool_自定圖片名單 = false;
                    M.fun_載入圖片或資料夾(e2.Url.ToString());
                    e2.Cancel = true;
                }
            };

            but_結束大量瀏覽.Click += (sender, e) => {
                fun_關閉大量瀏覽();
            };

            //讓視窗可以拖曳
            dockPanel_功能列.MouseLeftButtonDown += ((object sender, MouseButtonEventArgs e) => {
                try {
                    M.c_視窗改變大小.ResizeWindow(ResizeDirection.Move);//拖曳視窗
                } catch { }
            });

            but_排序.Click += (sender, e) => {
                M.c_排序.func_開啟選單_物件下方(but_排序);
                M.func_下拉選單背景(true, but_排序);
            };
            but_上一個資料夾.Click += (sender, e) => {
                M.func_開啟下一資料夾(0);
            };
            but_下一個資料夾.Click += (sender, e) => {
                M.func_開啟下一資料夾(1);
            };
            but_外部瀏覽器開啟.Click += (sernder, e) => {
                func_外部瀏覽器開啟();
            };

        }


        /// <summary>
        /// 
        /// </summary>
        public void func_重新載入圖片() {

            //避免網頁還沒載入完成
            try {
                fun_取得圖片名單();
                web01.Document.InvokeScript("eval", new Object[] { s_js_array });
                web01.Document.InvokeScript("fun_產生圖片");
                M.bool_web_背景刷新 = true;
            } catch { }
        }



        /// <summary>
        /// 
        /// </summary>
        private void event_快速鍵() {

            // 在網頁按下按鍵時
            web01.PreviewKeyDown += (sender2, e2) => {

                var k = e2.KeyCode;
          
                if (k == System.Windows.Forms.Keys.Escape || k == System.Windows.Forms.Keys.Back) {
             
                    func_按下esc();

                } else if (k == System.Windows.Forms.Keys.Oemcomma) {

                   // M.func_開啟下一資料夾(0);
                    
                } else if (k == System.Windows.Forms.Keys.OemPeriod) {

                   // M.func_開啟下一資料夾(1);

                }

            };//PreviewKeyDown

        }


        /// <summary>
        /// 
        /// </summary>
        public void func_按下esc() {

            //如果圖片窗口處於開啟，或input text有焦點，就不執行返回
            String s_允許關閉 = (String)web01.Document.InvokeScript("fun_aeropic按下返回鍵");

            if (s_允許關閉 == "t") {

                //退出全螢幕模式
                if (M.bool_全螢幕) {            
                    new Thread(() => {
                        Thread.Sleep(10);
                        C_adapter.fun_UI執行緒(() => {
                            M.func_全螢幕(false);
                        });
                    }).Start();
                    return;
                }

                //延遲10毫秒在關閉大量瀏覽模式，直接關閉的話，會整個程式被關掉
                new Thread(() => {
                    Thread.Sleep(10);
                    C_adapter.fun_UI執行緒(() => {                  
                        fun_關閉大量瀏覽();
                    });
                }).Start();
            }

        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void func_外部瀏覽器開啟() {

            if (ar_img_path == null)
                return;

            //用時間產生雜湊
            var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            String s111 = DateTime.Now.ToString("HHmmss") + DateTime.Now.Millisecond.ToString();
            string resultSha1 = Convert.ToBase64String(sha1.ComputeHash(Encoding.Default.GetBytes(s111)))
                .Replace("\\", "").Replace("/", "").Replace("+", "").Replace("=", "").ToUpper();

            M.c_localhost.dic_圖片路徑.Add(resultSha1, ar_img_path);

            String open_url = s_url + "?" + M.c_localhost.port + "," + resultSha1 + "," + ar_img_path.Length;
            M.c_set.fun_開啟網址(open_url, false);

        }












        /// <summary>
        /// 
        /// </summary>
        public void fun_關閉大量瀏覽() {

            try {
                web01.Navigate("about:blank");
            } catch { }

            web01.Dispose();

            M.Grid_總容器.Children.Remove(this);
            M.DockPanel_一般圖片.Visibility = Visibility.Visible;

            M.u_大量瀏覽模式 = null;

            M.fun_主視窗取得焦點();
            M.fun_顯示圖片(M.ar_path[M.int_目前圖片位置]);


            M.fun_清理記憶體();

            //M.func_隱藏工具列_套用樣式(M.bool_隱藏工具列);

        }







        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public String fun_取得圖片名單() {


            List<String> ar_允許的附檔名 = new List<string>();
            ar_允許的附檔名.Add("JPG");
            ar_允許的附檔名.Add("JPEG");
            ar_允許的附檔名.Add("JFIF");
            ar_允許的附檔名.Add("PNG");
            ar_允許的附檔名.Add("GIF");
            ar_允許的附檔名.Add("BMP");
            ar_允許的附檔名.Add("SVG");
            ar_允許的附檔名.Add("ICO");
            ar_允許的附檔名.Add("TIF");
            ar_允許的附檔名.Add("TIFF");
            ar_允許的附檔名.Add("PSD");
            ar_允許的附檔名.Add("PSB");
            ar_允許的附檔名.Add("WEBP");
            ar_允許的附檔名.Add("SVG");
            ar_允許的附檔名.Add("MPO");
            ar_允許的附檔名.Add("DDS");

            //向量
            ar_允許的附檔名.Add("WMF");
            ar_允許的附檔名.Add("EMF");

            //其他
            ar_允許的附檔名.Add("PBM");
            ar_允許的附檔名.Add("PGM");
            ar_允許的附檔名.Add("PCX");
            ar_允許的附檔名.Add("TGA");
            ar_允許的附檔名.Add("PPM");
            ar_允許的附檔名.Add("PSB");

            //相機
            ar_允許的附檔名.Add("CRW");
            ar_允許的附檔名.Add("DNG");

            ar_允許的附檔名.Add("HEIC");


            //RAW
            foreach (var item in M.c_影像.ar_RAW) {
                ar_允許的附檔名.Add(item);
            }


            if (M.bool_自定圖片名單) {
                ar_允許的附檔名.Add("EXE");
                ar_允許的附檔名.Add("LNK");
                ar_允許的附檔名.Add("PDF");
                ar_允許的附檔名.Add("AI");
            }


            //篩選出所有能夠使用的類型
            List<String> ar = new List<String>();
            String[] ar_sort = new String[0];



            Action<String, bool> ac_add = (String item, bool bool_避免重複) => {


                String s_自動判斷副檔名 = item;

                int i23 = item.LastIndexOf("\\");
                if (i23 > 5) {
                    String s24 = item.Substring(i23);
                    if (s24.Contains(".") == false) {
                        s_自動判斷副檔名 = item + "." + M.c_影像.fun_取得附檔名(item);
                    }
                }


                //從附檔名判斷
                if (s_自動判斷副檔名.Length > 6) {
                    String x = s_自動判斷副檔名.Substring(s_自動判斷副檔名.Length - 7).ToUpper();
                    foreach (var item2 in ar_允許的附檔名) {
                        if (x.Substring(7 - item2.Length) == item2) {
                            String ss = item;

                            if (bool_避免重複) {
                                if (ar.Contains(ss) == false)
                                    ar.Add(ss);
                            } else {
                                ar.Add(ss);
                            }

                            break;
                        }
                    }
                }//if
            };




            Action<String[]> ac_修改為網址格式 = (String[] list) => {

                for (int i = 0; i < list.Length; i++) {
                    String ss = "http://localhost:" + M.c_localhost.port + "/img_path/" + Uri.EscapeDataString(list[i]);
                    list[i] = ss;
                }
            };





            if (M.bool_自定圖片名單 == false) {//不是自定定名單，就從目前資料夾抓取





                String path = this.s_當前目錄;
                String docPath = path.Substring(0, path.LastIndexOf("\\"));

                //避免根目錄出現問題
                if (docPath.Substring(docPath.Length - 1) == ":") {
                    docPath = docPath + "\\";
                }

                //篩選特定副檔名
                foreach (var item in Directory.GetFiles(docPath, "*.*")) {
                    ac_add(item, false);
                }


                //如果拖曳進來的是特殊圖片，也嘗試解析
                ar_允許的附檔名.Add("EXE");
                ar_允許的附檔名.Add("LNK");
                ar_允許的附檔名.Add("PDF");
                ar_允許的附檔名.Add("AI");
                ac_add(this.s_當前目錄, true);


                //排序
                ar_sort = M.c_排序.func_檔案排序(ar);

                //修改為網址格式
                ac_修改為網址格式(ar_sort);


            } else {//使用自定名單

                foreach (var item in M.ar_自定圖片名單) {
                    ac_add(item, false);

                    /*if (ar.Contains(item) == false)
                        ar.Add(item);*/

                }

                //排序
                ar_sort = M.c_排序.func_檔案排序(ar);

                //修改為網址格式
                ac_修改為網址格式(ar_sort);

            }


            /*
            //如果拖曳進來的是特殊圖片，也嘗試解析
            ar_允許的附檔名.Add("EXE");
            ar_允許的附檔名.Add("LNK");
            ar_允許的附檔名.Add("PDF");
            ar_允許的附檔名.Add("AI");
            ac_add(this.s_當前目錄, true);*/




            //回存、存到全域變數
            ar_img_path = new string[ar_sort.Length];
            var ar_path = new List<string>();
            for (int i = 0; i < ar_sort.Length; i++) {
                ar_path.Add(ar_sort[i]);
                ar_img_path[i] = ar_sort[i];
            }


            s_js_array = fun_array轉js(ar_sort);


            return s_js_array;

        }



        /// <summary>
        /// 在瀏覽器顯示圖片（傳入網址
        /// </summary>
        private String fun_array轉js(String[] urls) {

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < urls.Length; i++) {

                //轉換成網址格式（JavaScript you can use the encodeURI()
                /*String s_url = urls[i].Replace("%", "%25")//這個優先限轉換才不會出錯
                                .Replace(" ", "%20")
                                .Replace("!", "%21")
                                .Replace("\"", "%22")
                                .Replace("#", "%23")
                                .Replace("$", "%24")
                                .Replace("&", "%26")
                                .Replace("'", "%27")
                                .Replace("(", "%28")
                                .Replace(")", "%29")
                                .Replace("*", "%2A")
                                .Replace("+", "%2B")
                                .Replace(",", "%2C")
                                .Replace("-", "%2D")
                                .Replace(".", "%2E");*/

                String s_url = urls[i];
                sb.Append("'").Append(s_url.Replace("'", "\\\'").Replace("\\", "/")).Append("'");
                if (i != urls.Length - 1)
                    sb.Append(",");
            }

            return "var urls = new Array(" + sb.ToString() + ");";

        }




    }






    [ComVisible(true)]
    public class C_web呼叫javaScript {

        private U_大量瀏覽模式 U;

        public C_web呼叫javaScript(U_大量瀏覽模式 u) {
            this.U = u;
        }


        /// <summary>
        /// 當js載入完成後，由網頁的js來呼叫此函數，來初始化圖片
        /// </summary>
        /// <returns></returns>
        public String fun_getImgs() {
            U.bool_已經載入 = true;
            U.WindowsFormsHost_01.Visibility = Visibility.Visible;//預設是隱藏，載入完成才顯示
            U.M.c_set.fun_套用setting設定();
            return U.fun_取得圖片名單();
        }


        /// <summary>
        /// 開啟圖片
        /// </summary>
        /// <param name="path"></param>
        public void fun_open_file(String path) {
            //path = "\"" + path + "\"";
            try {

                String s_localhost = "http://localhost:" + U.M.c_localhost.port + "/";
                path = path.Replace(s_localhost + "img_path/", "");

                path = Uri.UnescapeDataString(path);

                //System.Diagnostics.Process.Start(path);


                U.M.func_新視窗開啟圖片_大量瀏覽模式(path);


            } catch (Exception e) {
                MessageBox.Show(e.ToString());
            }
        }




        /// <summary>
        /// 開啟『拖曳檔案』的視窗
        /// </summary>
        public void fun_drag_window() {
            U.M.fun_開啟拖曳檔案的視窗();
        }



    }


}
