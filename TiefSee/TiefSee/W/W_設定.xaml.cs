using TiefSee.cs;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using static TiefSee.MainWindow;

namespace TiefSee.W {
    /// <summary>
    /// W_設定.xaml 的互動邏輯
    /// </summary>
    public partial class W_設定 : Window {


        MainWindow M;

        public W_設定(MainWindow m) {





            this.M = m;
            InitializeComponent();

            this.Height = 550;
            this.Closing += W_設定_Closing;

            String ppp01 = Path.Combine(M.fun_執行檔路徑(), "data", "imgs", "setting_big_switch_01.png");
            if (File.Exists(ppp01)) {
                //img_大換頁按鈕_不啟用.Source = M.c_影像.func_get_BitmapImage_JPG(ppp01);
            }
            String ppp02 = Path.Combine(M.fun_執行檔路徑(), "data", "imgs", "setting_big_switch_02.png");
            if (File.Exists(ppp02)) {
                img_大換頁按鈕_下方.Source = M.c_影像.func_get_BitmapImage_JPG(ppp02);
            }
            String ppp03 = Path.Combine(M.fun_執行檔路徑(), "data", "imgs", "setting_big_switch_03.png");
            if (File.Exists(ppp03)) {
                //img_大換頁按鈕_兩側.Source = M.c_影像.func_get_BitmapImage_JPG(ppp03);
            }

            //-------------------------------

            //切換滑鼠滾輪的操作模式


            switch (M._e_滾輪用途) {
                case e_滾輪用途.縮放圖片:
                    radio_滾輪_縮放圖片.IsChecked = true;
                    break;

                case e_滾輪用途.上下移動:
                    radio_滾輪_上下移動圖片.IsChecked = true;
                    break;

                case e_滾輪用途.上下移動_到底時換頁:

                    break;

                case e_滾輪用途.換頁:
                    radio_滾輪_切換圖片.IsChecked = true;
                    break;

                case e_滾輪用途.換頁_大於視窗時上下移動:

                    break;

                case e_滾輪用途.換頁_大於視窗時上下移動_到底部時換頁:

                    break;

                default:
                    break;
            }

            radio_滾輪_縮放圖片.Checked += (sender, e) => {
                M._e_滾輪用途 = e_滾輪用途.縮放圖片;
            };
            radio_滾輪_切換圖片.Checked += (sender, e) => {
                M._e_滾輪用途 = e_滾輪用途.換頁;
            };
            radio_滾輪_上下移動圖片.Checked += (sender, e) => {
                M._e_滾輪用途 = e_滾輪用途.上下移動;
            };


            //-------------------------------


            //取得 windows啟動資料夾 的路徑
            String s_啟動資料夾 = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            String s_捷徑儲存路徑 = Path.Combine(s_啟動資料夾, "TeifSee.lnk");

            but_開啟_啟動資料夾.Click += (sender, e) => {
                //開啟資料夾
                System.Diagnostics.Process prc = new System.Diagnostics.Process();
                prc.StartInfo.FileName = s_啟動資料夾;
                prc.Start();
            };


            //從開啟資料夾判斷，程式是否已經設為自動啟用
            switch_開機自動啟動.IsChecked = File.Exists(s_捷徑儲存路徑);


            switch_開機自動啟動.Checked = (U_switch u, bool b2) => {
                func_新增或刪除開機自動啟動捷徑(b2);
            };

            //-------------------------------



            switch_快速啟動.Checked += (sender, value) => {

                if (value) {
                    st_快速啟動子選項.Opacity = 1;
                    st_快速啟動子選項.IsEnabled = true;
                } else {
                    st_快速啟動子選項.Opacity = 0.3f;
                    st_快速啟動子選項.IsEnabled = false;
                }
                MainWindow._bool_快速啟動 = value;

                //如果沒有啟用「快速啟動」，就刪除捷徑
                if (MainWindow._bool_快速啟動 == false) {
                    func_新增或刪除開機自動啟動捷徑(false);
                } else {
                    func_新增或刪除開機自動啟動捷徑(switch_開機自動啟動.IsChecked);
                }
            };

            switch_快速預覽_空白鍵.Checked += (sender, value) => {
                MainWindow._bool_快速預覽_空白鍵 = value;
            };
            switch_快速預覽_滑鼠滾輪.Checked += (sender, value) => {
                MainWindow._bool_快速預覽_滑鼠滾輪 = value;
            };

            //初始化
            switch_快速啟動.IsChecked = MainWindow._bool_快速啟動;
            switch_快速預覽_空白鍵.IsChecked = MainWindow._bool_快速預覽_空白鍵;
            switch_快速預覽_滑鼠滾輪.IsChecked = MainWindow._bool_快速預覽_滑鼠滾輪;


            //-------------------------------


            but_進階設定.Click += (sender, e) => {

                String s_進階設定的路徑 = Path.Combine(M.fun_執行檔路徑(), "data", "config.xml");

                try {
                    string file = C_按鈕選單_其他程式開啟.fun_作業系統的槽() + @":\Windows\explorer.exe";
                    string argument = @"/select, " + "\"" + s_進階設定的路徑 + "\"";
                    System.Diagnostics.Process.Start(file, argument);
                } catch {
                    MessageBox.Show("error");
                }

            };

            but_檢查更新.Click += (sender, e) => {
                M.c_set.fun_開啟網址("https://hbl917070.github.io/aeropic/index.html#/explain_soft/" + M.s_程式版本, false);
            };

            but_官網.Click += (sender, e) => {
                M.c_set.fun_開啟網址("https://hbl917070.github.io/aeropic/index.html#/", false);
            };

            but_下載Pixiv動圖.Click += (sender, e) => {
                M.c_set.fun_開啟網址("https://hbl917070.github.io/aeropic/index.html#/explain_pixiv", false);
            };

            but_討論區.Click += (sender, e) => {
                M.c_set.fun_開啟網址("https://forum.gamer.com.tw/C.php?page=1&bsn=60076&snA=4095280", false);
            };

            but_作者小屋.Click += (sender, e) => {
                M.c_set.fun_開啟網址("https://home.gamer.com.tw/homeindex.php?owner=hbl917070", false);
            };



            //---------------

            ///設定動畫
            tabControl_1.SelectionChanged += (sender, e) => {
                try {
                    TabControl tabc = (TabControl)e.OriginalSource;
                    var tabItem = (TabItem)tabc.Items[tabc.SelectedIndex];
                    MainWindow.fun_動畫((FrameworkElement)(tabItem.Content), 10, 0, "Y", () => { });
                } catch { }
            };


            M.c_set.fun_讀取setting();

            //讓界面跟設定同步
            check_aero.IsChecked = M.c_set.bool_aero;
            check_換頁按鈕_下.IsChecked = M.c_set.bool_換頁按鈕_下;
            check_工具列_換頁按鈕.IsChecked = M.c_set.bool_工具列_換頁按鈕;
            check_工具列_複製.IsChecked = M.c_set.bool_工具列_複製;
            check_工具列_縮放至視窗大小.IsChecked = M.c_set.bool_工具列_縮放至視窗大小;
            check_工具列_檢視縮放比例.IsChecked = M.c_set.bool_工具列_檢視縮放比例;
            check_工具列_旋轉.IsChecked = M.c_set.bool_工具列_旋轉;
            check_工具列_搜圖.IsChecked = M.c_set.bool_工具列_搜圖;
            check_工具列_大量瀏覽模式.IsChecked = M.c_set.bool_工具列_大量瀏覽模式;
            check_工具列_外部程式開啟.IsChecked = M.c_set.bool_工具列_外部程式開啟;
            check_工具列_放大縮小.IsChecked = M.c_set.bool_工具列_放大縮小;
            check_工具列_換資料夾.IsChecked = M.c_set.bool_工具列_換資料夾;
            check_工具列_排序.IsChecked = M.c_set.bool_工具列_排序;
            check_工具列_快速拖曳.IsChecked = M.c_set.bool_工具列_快速拖曳;
            check_工具列_刪除圖片.IsChecked = M.c_set.bool_工具列_刪除圖片;

            if (M.int_高品質成像模式 == 1) {
                radio_高品質成像_1.IsChecked = true;
            } else if (M.int_高品質成像模式 == 2) {
                radio_高品質成像_2.IsChecked = true;
            } else if (M.int_高品質成像模式 == 3) {
                radio_高品質成像_3.IsChecked = true;
            } else if (M.int_高品質成像模式 == 4) {
                radio_高品質成像_4.IsChecked = true;
            }

            //打勾時 立即套用設定
            CheckBox[] ar_but = { check_工具列_換頁按鈕, check_換頁按鈕_下, check_工具列_複製,
                check_工具列_縮放至視窗大小, check_工具列_檢視縮放比例, check_工具列_旋轉,
                check_工具列_搜圖, check_工具列_大量瀏覽模式, check_工具列_外部程式開啟,check_工具列_放大縮小,
               check_工具列_換資料夾,check_工具列_排序,check_工具列_快速拖曳,check_工具列_刪除圖片};

            foreach (CheckBox item in ar_but) {
                item.Checked += Check_套用設定;
                item.Unchecked += Check_套用設定;
            }




            but_清除cookies.Click += But_清除cookies_Click;


            text_作者資訊.Text =
                        "程式版本：" + M.s_程式版本 + "\n" +
                        "作者　　：LIAO, WEN-HONG" + "\n" +
                        "巴哈姆特：hbl917070（深海異音）" + "\n" +
                        "Ｅmail　：hbl917070@gmail.com";


            this.KeyUp += (sender, e) => {
                if (e.Key == Key.Escape) {
                    this.Close();
                }

            };



            but_設定附檔名關聯.Click += (serdne, e) => {

                String s_path = M.fun_執行檔路徑() + "\\data\\FilenameExtension.exe";

                if (System.IO.File.Exists(s_path)) {
                    try {

                        System.Diagnostics.Process.Start(s_path);

                    } catch (System.ComponentModel.Win32Exception) {
                        MessageBox.Show("作業已被取消");
                    } catch (Exception e2) {
                        MessageBox.Show(e2.ToString());
                    }
                } else {
                    MessageBox.Show("找不到檔案\n" + s_path);
                }

            };

            but_開啟系統設定.Click += (serdne, e) => {

                String s_path = "ms-settings:defaultapps";

                if (C_window_AERO.IsWindows10()) {
                    try {

                        System.Diagnostics.Process.Start(s_path);

                    } catch (Exception e2) {
                        MessageBox.Show(e2.ToString());
                    }
                } else {
                    MessageBox.Show("Windows10 才需要進行此步驟");

                }
            };

            //----------------------------------------

            but_複製顏色.Click += (seder, e) => {
                colorBox_背景顏色.SelectedColor = colorBox_標題列顏色.SelectedColor;
                M.c_set.s_color_背景顏色 = M.c_set.s_color_標題列顏色;
                M.c_set.fun_套用setting設定();
            };

            but_套用預設顏色_深色.Click += (seder, e) => {
                colorBox_背景顏色.SelectedColor = Color.FromArgb(235,35,45,50);
                M.c_set.s_color_背景顏色 = M.c_set.fun_colorToString(colorBox_背景顏色.SelectedColor);
                colorBox_標題列顏色.SelectedColor = Color.FromArgb(235, 35, 45, 50);
                M.c_set.s_color_標題列顏色 = M.c_set.fun_colorToString(colorBox_標題列顏色.SelectedColor);
                colorBox_外框顏色.SelectedColor = Color.FromArgb(120, 150, 150, 150);
                M.c_set.s_color_外框顏色 = M.c_set.fun_colorToString(colorBox_外框顏色.SelectedColor);
                M.c_set.fun_套用setting設定();
            };
            but_套用預設顏色_淺色.Click += (seder, e) => {
                colorBox_背景顏色.SelectedColor = Color.FromArgb(160, 120, 120, 120);
                M.c_set.s_color_背景顏色 = M.c_set.fun_colorToString(colorBox_背景顏色.SelectedColor);
                colorBox_標題列顏色.SelectedColor = Color.FromArgb(160, 120, 120, 120);
                M.c_set.s_color_標題列顏色 = M.c_set.fun_colorToString(colorBox_標題列顏色.SelectedColor);
                colorBox_外框顏色.SelectedColor = Color.FromArgb(110, 110, 110, 120);
                M.c_set.s_color_外框顏色 = M.c_set.fun_colorToString(colorBox_外框顏色.SelectedColor);
                M.c_set.fun_套用setting設定();
            };
            but_套用預設顏色_藍色.Click += (seder, e) => {
                colorBox_背景顏色.SelectedColor = Color.FromArgb(60, 255, 255, 255);
                M.c_set.s_color_背景顏色 = M.c_set.fun_colorToString(colorBox_背景顏色.SelectedColor);
                colorBox_標題列顏色.SelectedColor = Color.FromArgb(255, 0, 100, 180);
                M.c_set.s_color_標題列顏色 = M.c_set.fun_colorToString(colorBox_標題列顏色.SelectedColor);
                colorBox_外框顏色.SelectedColor = Color.FromArgb(110, 110, 110, 120);
                M.c_set.s_color_外框顏色 = M.c_set.fun_colorToString(colorBox_外框顏色.SelectedColor);
                M.c_set.fun_套用setting設定();
            };
            but_套用預設顏色_綠色.Click += (seder, e) => {
                colorBox_背景顏色.SelectedColor = Color.FromArgb(140, 40, 40, 50);
                M.c_set.s_color_背景顏色 = M.c_set.fun_colorToString(colorBox_背景顏色.SelectedColor);
                colorBox_標題列顏色.SelectedColor = Color.FromArgb(255, 0, 99, 58);
                M.c_set.s_color_標題列顏色 = M.c_set.fun_colorToString(colorBox_標題列顏色.SelectedColor);
                colorBox_外框顏色.SelectedColor = Color.FromArgb(120, 160, 160, 160);
                M.c_set.s_color_外框顏色 = M.c_set.fun_colorToString(colorBox_外框顏色.SelectedColor);
                M.c_set.fun_套用setting設定();
            };
            but_套用預設顏色_紅色.Click += (seder, e) => {
                colorBox_背景顏色.SelectedColor = Color.FromArgb(90, 204, 204, 204);
                M.c_set.s_color_背景顏色 = M.c_set.fun_colorToString(colorBox_背景顏色.SelectedColor);
                colorBox_標題列顏色.SelectedColor = Color.FromArgb(255, 160, 50, 40);
                M.c_set.s_color_標題列顏色 = M.c_set.fun_colorToString(colorBox_標題列顏色.SelectedColor);
                colorBox_外框顏色.SelectedColor = Color.FromArgb(120, 86, 86, 86);
                M.c_set.s_color_外框顏色 = M.c_set.fun_colorToString(colorBox_外框顏色.SelectedColor);
                M.c_set.fun_套用setting設定();
            };

            colorBox_背景顏色.MouseUp += (sender2, e2) => {
                colorBox_背景顏色.IsDropDownOpen = true;
            };

            colorBox_標題列顏色.MouseUp += (sender2, e2) => {
                colorBox_標題列顏色.IsDropDownOpen = true;
            };

            colorBox_外框顏色.MouseUp += (sender2, e2) => {
                colorBox_外框顏色.IsDropDownOpen = true;
            };


            //-----------------------

            //GIF 渲染類型

            if (M.e_GIF_Type == MainWindow.E_GIF_type.GDI) {
                radio_GIF_DGI.IsChecked = true;
            } else if (M.e_GIF_Type == MainWindow.E_GIF_type.WPF) {
                radio_GIF_wpf.IsChecked = true;
            }

            radio_GIF_DGI.Checked += (sender2, e2) => {
                M.e_GIF_Type = MainWindow.E_GIF_type.GDI;
            };
            radio_GIF_wpf.Checked += (sender2, e2) => {
                M.e_GIF_Type = MainWindow.E_GIF_type.WPF;
            };



            //-----------------------


            //初始設定
            System.Windows.Media.Color cc = M.c_set.fun_getColor(M.c_set.s_color_背景顏色);
            colorBox_背景顏色.SelectedColor = cc;//初始化設定的顏色   

            colorBox_背景顏色.IsMouseCaptureWithinChanged += (sender2, e2) => {
                if (e2.OldValue.Equals(true)) {
                    cc = colorBox_背景顏色.SelectedColor;
                    M.c_set.s_color_背景顏色 = M.c_set.fun_colorToString(cc);
                    M.c_set.fun_套用setting設定();
                }
            };


            System.Windows.Media.Color cc2 = M.c_set.fun_getColor(M.c_set.s_color_標題列顏色);
            colorBox_標題列顏色.SelectedColor = cc2;//初始化設定的顏色   

            colorBox_標題列顏色.IsMouseCaptureWithinChanged += (sender2, e2) => {
                if (e2.OldValue.Equals(true)) {
                    cc2 = colorBox_標題列顏色.SelectedColor;
                    M.c_set.s_color_標題列顏色 = M.c_set.fun_colorToString(cc2);
                    M.c_set.fun_套用setting設定();
                }
            };


            System.Windows.Media.Color cc3 = M.c_set.fun_getColor(M.c_set.s_color_外框顏色);
            colorBox_外框顏色.SelectedColor = cc3;//初始化設定的顏色   

            colorBox_外框顏色.IsMouseCaptureWithinChanged += (sender2, e2) => {
                if (e2.OldValue.Equals(true)) {
                    cc3 = colorBox_外框顏色.SelectedColor;
                    M.c_set.s_color_外框顏色 = M.c_set.fun_colorToString(cc3);
                    M.c_set.fun_套用setting設定();
                }
            };

            radio_高品質成像_1.Checked += Radio_高品質成像_1_Checked;
            radio_高品質成像_2.Checked += Radio_高品質成像_1_Checked;
            radio_高品質成像_3.Checked += Radio_高品質成像_1_Checked;
            radio_高品質成像_4.Checked += Radio_高品質成像_1_Checked;

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="b2"></param>
        public static void func_新增或刪除開機自動啟動捷徑(bool b2) {

            //取得 windows啟動資料夾 的路徑
            String s_啟動資料夾 = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            String s_捷徑儲存路徑 = Path.Combine(s_啟動資料夾, "TeifSee.lnk");

            if (b2) {//啟用「開機自動啟動程式」

                String s_啟動參數 = @"none";

                String s_工作資料夾 = Directory.GetParent(System.AppDomain.CurrentDomain.BaseDirectory).ToString();
                s_工作資料夾 = Directory.GetParent(s_工作資料夾).ToString();
                String s_exe路徑 = Path.Combine(s_工作資料夾, "TiefSee.exe");
                String s_圖示檔案 = s_exe路徑;

                if (File.Exists(s_exe路徑) == false) {
                    MessageBox.Show("找不到\n" + s_exe路徑);
                    return;
                }

                //產生捷徑
                ShellLink slLinkObject = new ShellLink();
                slLinkObject.WorkPath = s_工作資料夾;
                slLinkObject.IconLocation = s_圖示檔案 + ",0";   // 0 為圖示檔的 Index
                slLinkObject.ExecuteFile = s_exe路徑;
                slLinkObject.ExecuteArguments = s_啟動參數;
                slLinkObject.Save(s_捷徑儲存路徑);

                slLinkObject.Dispose();

            } else {//刪除捷徑

                if (File.Exists(s_捷徑儲存路徑)) {
                    try {
                        File.Delete(s_捷徑儲存路徑);
                    } catch { }
                }
            }

        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Radio_高品質成像_1_Checked(object sender, RoutedEventArgs e) {

            if (radio_高品質成像_1.IsChecked.Value == true) {
                M.int_高品質成像模式 = 1;
                ImageMagick.OpenCL.IsEnabled = true;
            }
            if (radio_高品質成像_2.IsChecked.Value == true) {
                M.int_高品質成像模式 = 2;
                ImageMagick.OpenCL.IsEnabled = true;
            }
            if (radio_高品質成像_3.IsChecked.Value == true) {
                M.int_高品質成像模式 = 3;
                ImageMagick.OpenCL.IsEnabled = true;
            }
            if (radio_高品質成像_4.IsChecked.Value == true) {
                M.int_高品質成像模式 = 4;
                //硬體加速
                //https://stackoverflow.com/questions/42050790/enable-opencl-with-magick-net
                ImageMagick.OpenCL.IsEnabled = false;

            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void But_清除cookies_Click(object sender, RoutedEventArgs e) {

            ResetCookie(M.img_web);

            if (M.u_大量瀏覽模式 != null)

                ResetCookie(M.u_大量瀏覽模式.web01);

            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);

            MessageBox.Show("清除完畢");

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Check_套用設定(object sender, RoutedEventArgs e) {

            M.c_set.bool_aero = check_aero.IsChecked;
            M.c_set.bool_工具列_換頁按鈕 = check_工具列_換頁按鈕.IsChecked.Value;
            M.c_set.bool_換頁按鈕_下 = check_換頁按鈕_下.IsChecked.Value;
            M.c_set.bool_工具列_外部程式開啟 = check_工具列_外部程式開啟.IsChecked.Value;
            M.c_set.bool_工具列_大量瀏覽模式 = check_工具列_大量瀏覽模式.IsChecked.Value;
            M.c_set.bool_工具列_搜圖 = check_工具列_搜圖.IsChecked.Value;
            M.c_set.bool_工具列_旋轉 = check_工具列_旋轉.IsChecked.Value;
            M.c_set.bool_工具列_放大縮小 = check_工具列_放大縮小.IsChecked.Value;
            M.c_set.bool_工具列_檢視縮放比例 = check_工具列_檢視縮放比例.IsChecked.Value;
            M.c_set.bool_工具列_縮放至視窗大小 = check_工具列_縮放至視窗大小.IsChecked.Value;
            M.c_set.bool_工具列_複製 = check_工具列_複製.IsChecked.Value;

            M.c_set.bool_工具列_換資料夾 = check_工具列_換資料夾.IsChecked.Value;
            M.c_set.bool_工具列_排序 = check_工具列_排序.IsChecked.Value;
            M.c_set.bool_工具列_快速拖曳 = check_工具列_快速拖曳.IsChecked.Value;
            M.c_set.bool_工具列_刪除圖片 = check_工具列_刪除圖片.IsChecked.Value;


            M.c_set.fun_儲存setting();
            M.c_set.fun_讀取setting();
            M.c_set.fun_套用setting設定();

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void W_設定_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            Check_套用設定(null, null);
        }








        //清空cookie
        private const int INTERNET_OPTION_END_BROWSER_SESSION = 42;
        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);
        private void ResetCookie(System.Windows.Forms.WebBrowser c_web) {

            try {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "cmd.exe";
                // 關閉Shell的使用
                p.StartInfo.UseShellExecute = false;
                // 重定向標準輸入
                p.StartInfo.RedirectStandardInput = true;
                // 重定向標準輸出
                p.StartInfo.RedirectStandardOutput = true;
                //重定向錯誤輸出
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.StandardInput.WriteLine("RunDll32.exe InetCpl.cpl,ClearMyTracksByProcess 8");
                p.StandardInput.WriteLine("exit");
            } catch (Exception xx) {

                MessageBox.Show(xx + "");
            }

            try {
                c_web.Document.Cookie.Remove(0, c_web.Document.Cookie.Length - 1);
            } catch { }

            string[] theCookies = System.IO.Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Cookies));
            foreach (string currentFile in theCookies) {
                try {
                    System.IO.File.Delete(currentFile);
                } catch { }
            }

        }




    }




}
