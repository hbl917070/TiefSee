using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

using System.Xml;
using TiefSee.cs;
using TiefSee.W;
using System.Windows.Media.Animation;

using System.Windows.Shell;
using System.Windows.Controls.Primitives;
using 取得檔案總管選取的檔案;

namespace TiefSee {
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window {

        public String s_程式版本 = "3.0.1";

        bool bool_直接開啟大量瀏覽模式 = false;
        public int int_高品質成像模式 = 1;

        //圖片陣列
        public List<String> ar_path = new List<string>();
        public int int_目前圖片位置 = 0;
        public bool bool_自定圖片名單 = false;
        public List<string> ar_自定圖片名單 = new List<string>();




        //private C_web_圖片 c_web_圖片;
        //public float float_記憶體 = 0;
        public String s_img_type_顯示類型 = "";
        public String s_img_type_附檔名 = "";

        private BitmapSource bitmapImage_none;
        public bool bool_程式運行中 = true;//程式結束時關閉執行緒

        public C_setting c_set;
        public C_P網 c_P網;
        public C_apng c_apng;
        public W_設定 w_設定;
        public U_大量瀏覽模式 u_大量瀏覽模式;
        public F_web w_web;
        public C_web c_web;


        public C_按鈕選單_其他程式開啟 c_按鈕選單_其他程式開啟;
        public C_按鈕選單_複製 c_按鈕選單_複製;
        public C_按鈕選單_搜圖 c_按鈕選單_搜圖;
        public C_旋轉 c_旋轉;
        public C_視窗拖曳改變大小 c_視窗改變大小;
        //public C_書籤 c_書籤;
        public C_localhost_server c_localhost;
        public C_影像 c_影像;
        public C_排序 c_排序;
        private C_右下角圖示 c_右下角圖示;


        public System.Windows.Forms.WebBrowser img_web;
        public U_menu_item propertyMenu_輸出GIF;

        public int int_img_w = 50;//圖片size
        public int int_img_h = 50;
        public List<String> ar_附檔名_關聯 = new List<string>();



        public double d_解析度比例_x = 1;
        public double d_解析度比例_y = 1;

        public U_menu u_menu_主視窗右鍵;
        private U_menu_main u_工具列;
        private U_menu_main u_顯示exif;
        U_menu_main u_轉存GIF;
        U_menu_main u_解析動圖;



        public bool bool_顯示工具列 = true;
        public bool bool_顯示exif視窗 = false;


        public static int int_視窗執行數量 = 0;









        /// <summary>
        /// 
        /// </summary>
        public MainWindow() {



            try {
                TiefSee.MainWindow.fun_升級web核心();
            } catch { }

            C_adapter.Initialize();




            init();


            if (_bool_快速啟動) {
                String s023 = Path.Combine(System.Windows.Forms.Application.StartupPath, "data", "port");
                if (Directory.Exists(s023)) {
                    if (Directory.GetFiles(s023, "*").Length == 1) {
                        c_右下角圖示 = new C_右下角圖示();
                    }
                }
            }



        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="path"></param>
        public MainWindow(String type, String path) {

            if (type.ToLower() == "open_gif") {
                App.main_s = new String[] { path };
                bool_直接開啟大量瀏覽模式 = true;
                init();
            }

            if (type.ToLower() == "open_img") {
                App.main_s = new String[] { path };
                bool_直接開啟大量瀏覽模式 = false;

                init();
            }



            if (type.ToLower() == "notify_icon") {


                //讀取是否使用「快速預覽」的設定
                var c_set = new C_setting(this);
                c_set.fun_讀取setting();//讀取設定


                c_localhost = new C_localhost_server(null);
                int_視窗執行數量 += 1;
                //InitializeComponent();
                this.Width = 1;
                this.Height = 1;
                Show();
                //grid_all.Children.RemoveRange(0, grid_all.Children.Count - 1);             
                this.Visibility = Visibility.Collapsed;

                //GlobalHook.HookManager.KeyUp += HookManager_KeyUp;
                //GlobalHook.HookManager.KeyDown += HookManager_KeyDown;

                //避免載入圖片的時候放開空白鍵，導致無法辨識空白鍵已經釋放
                var tim = new System.Windows.Forms.Timer();
                tim.Interval = 50;
                tim.Tick += (sender2, e2) => {

                    Boolean bool_按下滑鼠中間 = false;
                    Boolean bool_按下空白鍵 = false;

                    if (_bool_快速啟動) {
                        if (_bool_快速預覽_滑鼠滾輪) {
                            bool_按下滑鼠中間 = System.Windows.Forms.Control.MouseButtons == System.Windows.Forms.MouseButtons.Middle;
                        }
                        if (_bool_快速預覽_空白鍵) {
                            bool_按下空白鍵 = Keyboard.IsKeyDown(Key.Space);
                        }
                    }


                    if (bool_按下空白鍵 || bool_按下滑鼠中間) {

                        HookManager_KeyDown();

                    } else {
                        bool_按下空白 = false;
                        if (w_空白鍵開啟的視窗 != null && w_空白鍵開啟的視窗.Visibility == Visibility.Visible) {
                            if (w_空白鍵開啟的視窗 != null && bool_快速瀏覽_開啟前200毫秒 == false) {

                                w_空白鍵開啟的視窗.fun_載入圖片或資料夾(fun_執行檔路徑() + "/data/imgs/space.png");
                                w_空白鍵開啟的視窗.WindowState = WindowState.Normal;
                                w_空白鍵開啟的視窗.Visibility = Visibility.Collapsed;
                                bool_啟動局部高清 = false;

                                /*w_空白鍵開啟的視窗.fun_切換_隱藏gif();                         
                                w_空白鍵開啟的視窗.fun_切換_隱藏voide();
                                w_空白鍵開啟的視窗.fun_切換_隱藏web();
                                w_空白鍵開啟的視窗.fun_切換_隱藏jpg();*/

                            }
                        }

                    }
                };
                tim.Start();

                this.Closing += (sender, e) => {
                    System.Console.WriteLine("關閉「快速啟動」");
                    //GlobalHook.HookManager.KeyUp -= HookManager_KeyUp;
                    //GlobalHook.HookManager.KeyDown -= HookManager_KeyDown;
                    tim.Stop();
                    if (w_空白鍵開啟的視窗 != null) {
                        w_空白鍵開啟的視窗.Close();
                    }
                };

            }

        }



        bool bool_按下空白 = false;
        MainWindow w_空白鍵開啟的視窗;//用來判斷目前這個視窗是否為空空白鍵預覽視窗
        bool bool_空白鍵預覽的視窗 = false;
        bool bool_快速瀏覽_開啟前200毫秒 = false;

        private void HookManager_KeyDown() {


            if (bool_按下空白) {
                return;
            }

            //this.Dispatcher.BeginInvoke(new Action(delegate () {//wpf委託UI行緒

            bool_按下空白 = true;

            String selectedItem2 = "";
            try {
                selectedItem2 = WinExplorerHelper.GetSelectedItem();
            } catch { }


            if (selectedItem2 != null && selectedItem2 != "") {

                //C_adapter.fun_UI執行緒(() => {

                if (w_空白鍵開啟的視窗 == null) {
                    try {
                        w_空白鍵開啟的視窗 = new MainWindow("open_img", selectedItem2);
                        w_空白鍵開啟的視窗.bool_空白鍵預覽的視窗 = true;
                    } catch { }
                    w_空白鍵開啟的視窗.Closing += (semder2, e2) => {
                        w_空白鍵開啟的視窗 = null;
                    };

                    bool_快速瀏覽_開啟前200毫秒 = false;

                    w_空白鍵開啟的視窗.Show();
                    w_空白鍵開啟的視窗.WindowState = WindowState.Normal;


                    new Thread(() => {
                        Thread.Sleep(500);
                        C_adapter.fun_UI執行緒(() => {
                            w_空白鍵開啟的視窗.fun_圖片全滿(true);
                        });
                    }).Start();

                } else {

                    bool_快速瀏覽_開啟前200毫秒 = true;
                    new Thread(() => {
                        Thread.Sleep(100);
                        C_adapter.fun_UI執行緒(() => {
                            w_空白鍵開啟的視窗.fun_載入圖片或資料夾(selectedItem2);
                            bool_快速瀏覽_開啟前200毫秒 = false;
                        });
                    }).Start();

                }

                //根據滑鼠所在的螢幕，來讓視窗顯示在正中央
                var mm3 = fun_取得滑鼠();
                foreach (System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens) {//列出所有螢幕資訊
                    if (mm3.X > screen.Bounds.X && mm3.X < screen.Bounds.X + screen.Bounds.Width &&
                        mm3.Y > screen.Bounds.Y && mm3.Y < screen.Bounds.Y + screen.Bounds.Height) {

                        w_空白鍵開啟的視窗.Width = screen.WorkingArea.Width / w_空白鍵開啟的視窗.d_解析度比例_x * 0.8;
                        w_空白鍵開啟的視窗.Height = screen.WorkingArea.Height / w_空白鍵開啟的視窗.d_解析度比例_y * 0.8;
                        w_空白鍵開啟的視窗.Left = screen.WorkingArea.X + (screen.Bounds.Width * 0.1 / w_空白鍵開啟的視窗.d_解析度比例_x);
                        w_空白鍵開啟的視窗.Top = screen.WorkingArea.Y + (screen.Bounds.Height * 0.1 / w_空白鍵開啟的視窗.d_解析度比例_y);

                        break;
                    }

                }

                //w_空白鍵開啟的視窗.func_鎖定視窗(w_空白鍵開啟的視窗, "auto");
                w_空白鍵開啟的視窗.func_鎖定視窗(w_空白鍵開啟的視窗, "auto");
                w_空白鍵開啟的視窗.func_鎖定視窗(w_空白鍵開啟的視窗, "true");

                w_空白鍵開啟的視窗.Visibility = Visibility.Visible;
                //w_空白鍵開啟的視窗.Show();

                //});
            }

            //}));


        }





        /// <summary>
        /// 
        /// </summary>
        public void init() {


            int_視窗執行數量 += 1;


            if (c_localhost == null)
                c_localhost = new C_localhost_server(this);


            InitializeComponent();



            c_set = new C_setting(this);
            c_set.fun_讀取_position();
            c_set.fun_儲存_position(true);
            c_set.fun_讀取setting();//讀取設定
            c_set.fun_套用setting設定(); //套用設定

            c_視窗改變大小 = new C_視窗拖曳改變大小(this);
            this.SourceInitialized += new System.EventHandler(c_視窗改變大小.MainWindow_SourceInitialized);//右下角拖曳


            c_P網 = new C_P網(this);
            c_apng = new C_apng();
            c_web = new C_web(this);
            c_排序 = new C_排序(this);

            c_旋轉 = new C_旋轉(this);
            c_按鈕選單_複製 = new C_按鈕選單_複製(this);
            c_按鈕選單_搜圖 = new C_按鈕選單_搜圖(this);
            c_按鈕選單_其他程式開啟 = new C_按鈕選單_其他程式開啟(this);
            //c_書籤 = new C_書籤(this);
            c_影像 = new C_影像(this);

            u_menu_主視窗右鍵 = new U_menu(this);


            bitmapImage_none = c_影像.func_get_BitmapImage_JPG(fun_執行檔路徑() + "\\data\\imgs\\space.png");

            //event_處理外框顏色();

            this.Closing += MainWindow_Closing;
            this.Loaded += MainWindow_Loaded;



        }




        /// <summary>
        /// 
        /// </summary>
        public String func_取得暫存路徑() {
            String s_暫存路徑 = System.IO.Path.GetTempPath();
            return s_暫存路徑 + "TiefSee\\";

        }




        #region 視窗取得焦點
        //https://stackoverflow.com/questions/257587/bring-a-window-to-the-front-in-wpf


        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 SWP_SHOWWINDOW = 0x0040;



        /// <summary>
        /// Activate a window from anywhere by attaching to the foreground window
        /// </summary>
        public static void GlobalActivate(Window w) {
            //Get the process ID for this window's thread
            var interopHelper = new WindowInteropHelper(w);
            var thisWindowThreadId = GetWindowThreadProcessId(interopHelper.Handle, IntPtr.Zero);

            //Get the process ID for the foreground window's thread
            var currentForegroundWindow = GetForegroundWindow();
            var currentForegroundWindowThreadId = GetWindowThreadProcessId(currentForegroundWindow, IntPtr.Zero);

            //Attach this window's thread to the current window's thread
            AttachThreadInput(currentForegroundWindowThreadId, thisWindowThreadId, true);


            //Set the window position
            //SetWindowPos(interopHelper.Handle, new IntPtr(0), 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_SHOWWINDOW);

            //Detach this window's thread from the current window's thread
            /*AttachThreadInput(currentForegroundWindowThreadId, thisWindowThreadId, false);

            //Show and activate the window
            if (w.WindowState == WindowState.Minimized) w.WindowState = WindowState.Normal;
            w.Show();
            w.Activate();*/

        }



        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [DllImport("user32.dll")]
        private static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        #endregion



        /// <summary>
        /// 
        /// </summary>
        private void MainWindow_Loaded(object sender22, RoutedEventArgs e22) {

            //取得解析度偏差
            PresentationSource source = PresentationSource.FromVisual(this);
            d_解析度比例_x = source.CompositionTarget.TransformToDevice.M11;
            d_解析度比例_y = source.CompositionTarget.TransformToDevice.M22;

            //避免修改過文字大小的螢幕，開啟程式後視窗定位錯誤
            if (d_解析度比例_x != 1 || d_解析度比例_y != 1) {
                c_set.fun_讀取_position();
            }


            //開啟視窗後，讓視窗取得焦點
            GlobalActivate(this);
            this.Activate();

            //一開始先隱藏
            Grid_總容器.Visibility = Visibility.Collapsed;



            //一般格式
            ar_附檔名_關聯.Add("JPG");
            ar_附檔名_關聯.Add("JPEG");
            ar_附檔名_關聯.Add("JPGE");
            ar_附檔名_關聯.Add("JFIF");
            ar_附檔名_關聯.Add("PNG");
            ar_附檔名_關聯.Add("GIF");
            ar_附檔名_關聯.Add("BMP");
            ar_附檔名_關聯.Add("ICO");
            ar_附檔名_關聯.Add("TIF");
            ar_附檔名_關聯.Add("TIFF");
            ar_附檔名_關聯.Add("SVG");
            ar_附檔名_關聯.Add("MPO");

            //(Windows內建的解碼器即可閱讀)
            ar_附檔名_關聯.Add("CRW");//
            ar_附檔名_關聯.Add("NEF");//

            //一般(Magick.NET)
            ar_附檔名_關聯.Add("PPM");
            ar_附檔名_關聯.Add("TGA");//
            ar_附檔名_關聯.Add("PCX");//
            ar_附檔名_關聯.Add("PGM");//
            ar_附檔名_關聯.Add("PBM");//
            ar_附檔名_關聯.Add("PSB");//photoshop的其中一種檔案
            ar_附檔名_關聯.Add("PSD");//photoshop的其中一種檔案

            //新圖片格式
            ar_附檔名_關聯.Add("WEBP");
            ar_附檔名_關聯.Add("JPF");//
            ar_附檔名_關聯.Add("HEIC");//

            //向量
            ar_附檔名_關聯.Add("EMF");//
            ar_附檔名_關聯.Add("WMF");//

            //素材用圖片
            ar_附檔名_關聯.Add("DDS");//

            //相機
            ar_附檔名_關聯.Add("CRW");
            ar_附檔名_關聯.Add("DNG");

            //相機
            /*ar_附檔名_關聯.Add("MPO");
            ar_附檔名_關聯.Add("CR2");
            ar_附檔名_關聯.Add("DNG");
            ar_附檔名_關聯.Add("ARW");*/

            //RAW
            foreach (var item in c_影像.ar_RAW) {
                ar_附檔名_關聯.Add(item);
            }


            if (c_set.bool_aero) {
                var c_毛玻璃_win10 = new C_window_AERO();
                c_毛玻璃_win10.func_啟用毛玻璃(this);
            }


            // 延遲250毫秒後才載入圖片，避免造成啟動是看起來lag
            new Thread(() => {

                Thread.Sleep(250);

                C_adapter.fun_UI執行緒(() => {

                    Grid_總容器.Visibility = Visibility.Visible;

                    //載入初始圖片
                    if (App.main_s.Length == 0) {
                        String s_初始圖片 = fun_執行檔路徑() + "\\data\\imgs\\start.png";
                        fun_載入圖片或資料夾(s_初始圖片);
                    } else if (App.main_s.Length == 1) {
                        fun_載入圖片或資料夾(App.main_s[0]);
                    } else if (App.main_s.Length > 1) {
                        bool_自定圖片名單 = true;
                        fun_自定圖片名單(App.main_s);
                        fun_載入圖片或資料夾(App.main_s[0]);
                    }


                    func_局部高清_初始化();

                    if (bool_直接開啟大量瀏覽模式) {
                        fun_新建大量閱讀模式();
                    }

                });
            }).Start();



            //讓視窗可以拖曳
            /*scrollViewer_工具列.MouseLeftButtonDown += ((object sender, MouseButtonEventArgs e) => {
                try {
                    //取消文字框的焦點
                    if (stackPlanel_動圖工具.Visibility == Visibility.Visible) {
                        fun_主視窗取得焦點();
                    }
                    c_視窗改變大小.ResizeWindow(C_視窗拖曳改變大小.ResizeDirection.Move);//拖曳視窗
                } catch { }
            });*/
            dockPanel_標題.MouseLeftButtonDown += ((object sender, MouseButtonEventArgs e) => {
                try {
                    //取消文字框的焦點
                    if (stackPlanel_動圖工具.Visibility == Visibility.Visible) {
                        fun_主視窗取得焦點();
                    }
                    c_視窗改變大小.ResizeWindow(C_視窗拖曳改變大小.ResizeDirection.Move);//拖曳視窗
                } catch { }
            });
            stackPanel_exif_box.MouseLeftButtonDown += ((object sender, MouseButtonEventArgs e) => {
                if (this.WindowState != WindowState.Maximized)//不是全螢幕
                    try {
                        //取消文字框的焦點
                        if (stackPlanel_動圖工具.Visibility == Visibility.Visible) {
                            fun_主視窗取得焦點();
                        }
                        c_視窗改變大小.ResizeWindow(C_視窗拖曳改變大小.ResizeDirection.Move);//拖曳視窗
                    } catch { }
            });
            border_工具列_外框.MouseLeftButtonDown += ((object sender, MouseButtonEventArgs e) => {
                //if (this.WindowState != WindowState.Maximized)//不是全螢幕
                try {
                    //取消文字框的焦點
                    if (stackPlanel_動圖工具.Visibility == Visibility.Visible) {
                        fun_主視窗取得焦點();
                    }
                    c_視窗改變大小.ResizeWindow(C_視窗拖曳改變大小.ResizeDirection.Move);//拖曳視窗
                } catch { }
            });

            //讓工具列可以水平捲動
            ((UIElement)scrollViewer_工具列.Content).MouseWheel += new MouseWheelEventHandler((object sender, MouseWheelEventArgs e) => {
                int x = e.Delta;
                if (x > 0)
                    scrollViewer_工具列.LineLeft();
                else
                    scrollViewer_工具列.LineRight();
            });

            event_視窗();
            event_拖曳圖片();
            event_將資料夾用圖片檢視器開啟();

            event_縮放();
            event_註冊所有按鈕事件();


            this.PreviewKeyDown += MainWindow_KeyUp;

            //func_顯示或隱藏工具列("ture");//初始化

            func_顯示或隱藏工具列(bool_顯示工具列 + "");
            func_顯示或隱藏exif視窗(bool_顯示exif視窗 + "");


            //初始化影片播放器
            img_voide.LoadedBehavior = MediaState.Stop;
            img_voide.MediaEnded += (sender, e) => {
                String path_v = func_取得影片檔案路徑(ar_path[int_目前圖片位置]);
                img_voide.Source = new Uri(path_v);//重新播放
                img_voide.LoadedBehavior = MediaState.Play;
            };
            img_voide.MediaOpened += (sender, e) => {
                if (s_img_type_顯示類型 == "MOVIE" && img_voide.NaturalVideoWidth != 0) {
                    fun_設定顯示圖片size(img_voide.NaturalVideoWidth, img_voide.NaturalVideoHeight); //顯示寬高
                    fun_圖片全滿();
                }
            };

            but_bottom_上一頁.Click += (sender, e) => {
                fun_上一張();
            };
            but_bottom_下一頁.Click += (sender, e) => {
                fun_下一張();
            };
            Button[] ar_底下換頁按鈕 = { but_bottom_上一頁, but_bottom_下一頁 };
            for (int i = 0; i < ar_底下換頁按鈕.Length; i++) {
                ar_底下換頁按鈕[i].Opacity = 0;
                ar_底下換頁按鈕[i].MouseEnter += (sender, e) => {
                    var but = (Button)sender;
                    but.Opacity = 1;
                };
                ar_底下換頁按鈕[i].MouseLeave += (sender, e) => {
                    var but = (Button)sender;
                    but.Opacity = 0;
                };
            }


            but_進入大量瀏覽.Click += (sender, e) => {
                fun_新建大量閱讀模式();
            };

            //鎖定視窗在最上層
            but_鎖.Click += (sender, e) => {
                func_鎖定視窗(this, "auto");
            };


            but_檢視原始大小.Click += (sender, e) => {
                func_檢視原始大小();
            };

            //滑鼠側邊按鍵可以切換上下一張圖片
            this.MouseDown += (sersdf, e) => {
                if (u_大量瀏覽模式 == null)
                    if (e.XButton1 == MouseButtonState.Pressed) {
                        fun_上一張();
                    } else if (e.XButton2 == MouseButtonState.Pressed) {
                        fun_下一張();
                    }
            };

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="s_移動方式"></param>
        public static void fun_動畫(FrameworkElement f, double from, double to, String s_移動方式, Action ac) {

            if (f == null)
                return;

            s_移動方式 = s_移動方式.ToUpper();


            //位移
            Storyboard storyboard2 = new Storyboard();
            DoubleAnimation growAnimation2 = new DoubleAnimation();
            growAnimation2.Duration = (Duration)TimeSpan.FromSeconds(0.15f);

            growAnimation2.Completed += (sender, e) => {//完成時執行
                ac();
                //f.Visibility = Visibility.Collapsed;
            };

            f.RenderTransform = new TranslateTransform();

            growAnimation2.From = from;
            growAnimation2.To = to;

            Storyboard.SetTargetProperty(growAnimation2, new PropertyPath("(FrameworkElement.RenderTransform).(TranslateTransform." + s_移動方式 + ")"));
            Storyboard.SetTarget(growAnimation2, f);

            storyboard2.Children.Add(growAnimation2);
            storyboard2.Begin();

            //----------------------

            Storyboard storyboard3 = new Storyboard();
            DoubleAnimation growAnimation3 = new DoubleAnimation();
            growAnimation3.Duration = (Duration)TimeSpan.FromSeconds(0.15f);

            growAnimation3.Completed += (sender, e) => {//完成時執行

                //f.Visibility = Visibility.Collapsed;
            };

            //f.RenderTransform = new TranslateTransform();

            growAnimation3.From = 0.5;
            growAnimation3.To = 1;

            Storyboard.SetTargetProperty(growAnimation3, new PropertyPath("Opacity"));
            Storyboard.SetTarget(growAnimation3, f);

            storyboard3.Children.Add(growAnimation3);
            storyboard3.Begin();

        }



        /// <summary>
        /// 
        /// </summary>
        public void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {

            if (w_設定 != null) {
                w_設定.Close();
            }

            if (c_排序 != null)
                c_排序.func_儲存與優化排序();

            //如果沒有啟用「快速啟動」，就刪除捷徑
            if (_bool_快速啟動 == false) {
                W_設定.func_新增或刪除開機自動啟動捷徑(false);
            }

            int_視窗執行數量 -= 1;
            bool_程式運行中 = false;

            if (c_set != null) {
                c_set.fun_儲存setting();
                c_set.fun_儲存_position(false);
            }

            if (c_localhost != null)
                c_localhost.fun_end();

            // 1 表示只剩下快速啟動，所以清理記憶體
            //if (int_視窗執行數量 == 1) {
            fun_清理記憶體();
            //}

            //取得目前是否有其他開啟的aeropic
            var ar_Process = Process.GetProcessesByName(System.Diagnostics.Process.GetCurrentProcess().ProcessName);

            //完全沒有aeropic
            if (ar_Process.Length <= 1 && int_視窗執行數量 <= 0) {//沒有的話才刪除暫存資料夾
                try {
                    String s_暫存路徑 = fun_執行檔路徑() + "\\data\\Temporary";
                    if (Directory.Exists(s_暫存路徑))
                        System.IO.Directory.Delete(s_暫存路徑, true);
                } catch { }


                try {
                    String s_暫存路徑 = func_取得暫存路徑();
                    if (Directory.Exists(s_暫存路徑))
                        System.IO.Directory.Delete(s_暫存路徑, true);
                } catch { }

                try {
                    String s_暫存路徑 = fun_執行檔路徑() + "\\data\\port";
                    if (Directory.Exists(s_暫存路徑))
                        System.IO.Directory.Delete(s_暫存路徑, true);
                } catch { }


                String s_搜圖暫存 = fun_執行檔路徑() + "\\data\\graphSearch\\input.txt";
                if (File.Exists(s_搜圖暫存)) {
                    try {
                        System.IO.File.Delete(s_搜圖暫存);
                    } catch { }
                }

                //pixiv動圖轉GIF
                String s_輸出GIF暫存 = fun_執行檔路徑() + "\\data\\output_gif\\input.xml";
                if (File.Exists(s_輸出GIF暫存)) {
                    try {
                        System.IO.File.Delete(s_輸出GIF暫存);
                    } catch { }
                }
            }


            if (w_web != null) {
                C_滑鼠偵測_滾動.MouseWheel -= c_web.C_滑鼠偵測_滾動_MouseWheel;
            }

        }







        /// <summary>
        /// 自動判斷目前背景是否要實色，並設定
        /// </summary>
        public void fun_顯示背景顏色() {
            bool b = false;
            if (s_img_type_顯示類型 == "WEB" || u_大量瀏覽模式 != null) {
                b = true;
            }
            var c = c_set.fun_getColor(c_set.s_color_背景顏色);
            if (b) {
                bac.Fill = new SolidColorBrush(Color.FromArgb(255, c.R, c.G, c.B));
            } else {
                bac.Fill = new SolidColorBrush(c);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public void fun_設定顯示圖片size(int w, int h) {
            text_imgSize.Text = w + "\n" + h;//顯示圖片size
            int_img_w = w;//讓其他地方能夠取得圖片size
            int_img_h = h;
        }










        /// <summary>
        /// 
        /// </summary>
        /*private void event_處理外框顏色() {

            var tt = new System.Windows.Forms.Timer();
            tt.Interval = 300;
            tt.Tick += (sdfsdf, e) => {

                bool web_的焦點 = false;
                if (w_web != null) {
                    web_的焦點 = w_web.webBrowser1.Focused;
                }

                if (this.WindowState == WindowState.Maximized) {//全熒幕就不要顯示外框

                    border_視窗外框.BorderThickness = new Thickness(0);

                } else if (this.WindowState == WindowState.Normal) {//視窗化就顯示外框

                    border_視窗外框.BorderThickness = new Thickness(1.3);

                    if (this.IsActive || web_的焦點) {//取得焦點
                        border_視窗外框.BorderBrush = new SolidColorBrush { Color = Color.FromRgb(0, 178, 255) };
                    } else {
                        border_視窗外框.BorderBrush = new SolidColorBrush { Color = Color.FromRgb(255, 255, 255) };
                    }
                }
            };
            
            if (IsWindows7() == false) {//win7變邊框會有會有白邊，所以乾脆不要啟用 取得焦點就變顏色 的功能
                //tt.Start();
            }
        }*/



        /// <summary>
        /// 
        /// </summary>
        private void fun_新建大量閱讀模式() {

            String s_當前目錄 = ar_path[int_目前圖片位置];
            u_大量瀏覽模式 = new U_大量瀏覽模式(this);
            Grid_總容器.Children.Add(u_大量瀏覽模式);
            DockPanel_一般圖片.Visibility = Visibility.Collapsed;
            fun_重啟大量閱讀模式();

        }



        /// <summary>
        /// 
        /// </summary>
        private void fun_重啟大量閱讀模式() {

            String s_當前目錄 = ar_path[int_目前圖片位置];
            u_大量瀏覽模式.s_當前目錄 = ar_path[int_目前圖片位置];

            //u_大量瀏覽模式 = new U_大量瀏覽模式(this, s_當前目錄);

            s_img_type_顯示類型 = "*";

            //Grid_總容器.Children.Add(u_大量瀏覽模式);
            //DockPanel_一般圖片.Visibility = Visibility.Collapsed;

            fun_切換_隱藏web();
            fun_切換_隱藏gif();
            fun_切換_隱藏voide();
            fun_切換_隱藏jpg();


            s_當前目錄 = s_當前目錄.Replace("\\", "/");
            s_當前目錄 = s_當前目錄.Substring(0, s_當前目錄.LastIndexOf("/"));
            lab_title.Text = " " + s_當前目錄;

            if (s_當前目錄.LastIndexOf("/") > -1)
                s_當前目錄 = s_當前目錄.Substring(s_當前目錄.LastIndexOf("/"));

            s_當前目錄 = s_當前目錄.Replace("/", "");
            Title = s_當前目錄;
            t_選單_大量_資料夾名稱.Text = s_當前目錄;

        }


        /// <summary>
        /// 
        /// </summary>
        private void MainWindow_KeyUp(object sender, KeyEventArgs e) {

            var k = e.Key;
            var k2 = e.ImeProcessedKey;//中文輸入法的情況下，必須用這個來取得。用於英文按鍵


            if (k == Key.F11) {
                func_全螢幕(!bool_全螢幕);
                return;
            }


            //大量閱讀模式的快速鍵在【U_大量瀏覽模式.cs】裡面
            if (u_大量瀏覽模式 != null) {

                if (k == Key.Escape || k == Key.Back) {

                    u_大量瀏覽模式.func_按下esc();

                } else if (k == Key.OemComma || k2 == Key.OemComma) {

                    func_開啟下一資料夾(0);
                    e.Handled = true;

                } else if (k == Key.OemPeriod || k2 == Key.OemPeriod) {

                    func_開啟下一資料夾(1);
                    e.Handled = true;

                }


                return;
            }


            //退出全螢幕模式
            if (bool_全螢幕) {
                if (k == Key.Escape) {
                    func_全螢幕(false);
                    return;
                }
            }


            //MessageBox.Show(k2.ToString());


            if (k == Key.Right) {

                fun_下一張();
                e.Handled = true;

            } else if (k == Key.Left) {

                fun_上一張();
                e.Handled = true;

            } else if (k == Key.Escape) {

                Close();

            } else if (k == Key.F5) {

                fun_顯示圖片(ar_path[int_目前圖片位置]);
                e.Handled = true;

            } else if (k == Key.Add || k == Key.OemPlus || k == Key.RightShift) {

                fun_放大圖片(true);
                e.Handled = true;

            } else if (k == Key.Subtract || k == Key.OemMinus || k == Key.RightCtrl) {

                fun_縮小圖片(true);
                e.Handled = true;

            } else if (k == Key.O || k2 == Key.O) {

                fun_用檔案總管開啟目前圖片();
                e.Handled = true;

            } else if (k == Key.Delete) {

                fun_刪除檔案();

            } else if (k == Key.Up) {

                func_圖片移動_上();

            } else if (k == Key.Down) {

                func_圖片移動_下();

            } else if (k == Key.F || k2 == Key.F) {

                fun_圖片全滿();
                e.Handled = true;

            } else if (k == Key.OemOpenBrackets || k2 == Key.OemOpenBrackets) {
                if (stackPlanel_動圖工具.Visibility == Visibility.Visible) {
                    c_P網.fun_上一張圖片();
                    e.Handled = true;
                }
            } else if (k == Key.Oem6 || k2 == Key.Oem6) {
                if (stackPlanel_動圖工具.Visibility == Visibility.Visible) {
                    c_P網.fun_下一張圖片();
                    e.Handled = true;
                }

            } else if (k == Key.B || k2 == Key.B) {

                fun_新建大量閱讀模式();
                e.Handled = true;

            } else if (k == Key.M || k2 == Key.M) {

                c_按鈕選單_其他程式開啟.fun_顯示原生右鍵選單(true);
                e.Handled = true;

            } else if ((k == Key.C || k2 == Key.C) && Keyboard.IsKeyDown(Key.LeftCtrl)) {

                c_按鈕選單_複製.fun_複製影像();
                e.Handled = true;

            } else if (k == Key.R || k2 == Key.R) {

                c_旋轉.func_旋轉_順時針_90();
                e.Handled = true;

            } else if (k == Key.H || k2 == Key.H) {

                c_旋轉.func_旋轉_水平();
                e.Handled = true;

            } else if (k == Key.V || k2 == Key.V) {

                c_旋轉.func_旋轉_垂直();
                e.Handled = true;

            } else if (k == Key.OemComma || k2 == Key.OemComma) {

                func_開啟下一資料夾(0);
                e.Handled = true;

            } else if (k == Key.OemPeriod || k2 == Key.OemPeriod) {

                func_開啟下一資料夾(1);
                e.Handled = true;

            }

        }


        /// <summary>
        /// 回傳目前記憶體使用量（MB
        /// </summary>
        public static float fun_取得記憶體用量() {
            Process proc = Process.GetCurrentProcess();
            var xx = proc.WorkingSet64;
            return xx / 1024 / 1024;

            /*     
            Process thisProc = Process.GetCurrentProcess();  
            PerformanceCounter PC = new PerformanceCounter();
            float fff = 0;
            try {
                PC.CategoryName = "Process";
                PC.CounterName = "Working Set - Private";
                PC.InstanceName = thisProc.ProcessName;
                fff = PC.NextValue() / 1024 / 1024;
            } catch { }
            return fff;*/
        }




        /// <summary>
        /// 
        /// </summary>
        public void fun_用檔案總管開啟目前圖片() {
            try {
                string file = C_按鈕選單_其他程式開啟.fun_作業系統的槽() + @":\Windows\explorer.exe";
                string argument = @"/select, " + "\"" + ar_path[int_目前圖片位置] + "\"";
                System.Diagnostics.Process.Start(file, argument);

            } catch (Exception e) {
                MessageBox.Show(e.ToString(), "error");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void func_圖片移動_上() {
            if (s_img_type_顯示類型 == "WEB") {
                if (w_web == null) {//如果還沒初始化就不執行
                    return;
                }
                img_web.Document.InvokeScript("fun_scrollTop", new Object[] { });
            } else {
                scrollviewer_1.ScrollToVerticalOffset(scrollviewer_1.VerticalOffset - 50);
                func_隱藏局部高清_移動時();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void func_圖片移動_下() {
            if (s_img_type_顯示類型 == "WEB") {
                if (w_web == null) {//如果還沒初始化就不執行
                    return;
                }
                img_web.Document.InvokeScript("fun_scrollBottom", new Object[] { });
            } else {
                scrollviewer_1.ScrollToVerticalOffset(scrollviewer_1.VerticalOffset + 50);
                func_隱藏局部高清_移動時();
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public void fun_上一張() {
            if (ar_path.Count == 0)
                return;
            //s_局部高清_處理方式 = DateTime.Now.AddSeconds(-99);
            int_目前圖片位置--;
            if (int_目前圖片位置 < 0) {
                int_目前圖片位置 = ar_path.Count - 1;
            }
            fun_顯示圖片(ar_path[int_目前圖片位置]);
        }


        /// <summary>
        /// 
        /// </summary>
        public void fun_下一張() {
            if (ar_path.Count == 0)
                return;
            //s_局部高清_處理方式 = DateTime.Now.AddSeconds(-99);
            int_目前圖片位置++;
            if (int_目前圖片位置 > ar_path.Count - 1) {
                int_目前圖片位置 = 0;
            }
            fun_顯示圖片(ar_path[int_目前圖片位置]);
        }



        /// <summary>
        /// 
        /// </summary>
        public void func_放大圖片() {
            if (s_img_type_顯示類型 == "WEB") {
                if (w_web == null) {//如果還沒初始化就不執行
                    return;
                }
                img_web.Document.InvokeScript("fun_imgSizeAdd", new Object[] { });
            } else {
                fun_放大圖片(true);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void func_圖片縮小() {
            if (s_img_type_顯示類型 == "WEB") {
                if (w_web == null) {//如果還沒初始化就不執行
                    return;
                }
                img_web.Document.InvokeScript("fun_imgSizeSubtrat", new Object[] { });
            } else {
                fun_縮小圖片(true);
            }
        }




        /// <summary>
        /// 
        /// </summary>
        public void func_檢視原始大小() {
            if (w_web != null && s_img_type_顯示類型 == "WEB") {
                img_web.Document.InvokeScript("fun_檢視原始大小");
            } else {
                int_size = int_img_w;
                fun_修改圖片size(int_img_w);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public void fun_圖片全滿(bool bool_初始載入 = false) {


            if (w_web != null && s_img_type_顯示類型 == "WEB") {



                if (bool_初始載入 == true && b2.ActualWidth > int_img_w && b2.ActualHeight > int_img_h) {

                    //System.Console.WriteLine(b2.ActualWidth + "  " + int_img_w);
                    img_web.Document.InvokeScript("fun_檢視原始大小");


                } else {
                    img_web.Document.InvokeScript("fun_100scale", new Object[] { });
                }


                return;
            }

            double img_width = int_img_w;
            double img_height = int_img_h;





            double thisWidth = b2.ActualWidth;


            if (stackPanel_exif_box.Visibility == Visibility.Visible) {
                thisWidth -= stackPanel_exif_box.Width;
            }
            if (thisWidth <= 0) {
                thisWidth = 1;
            }

            //設定圖片寬度
            double double_比例 = img_width / thisWidth;
            double w = 1;

            //判斷圖是是否被旋轉
            if (c_旋轉.int_旋轉 == 90 || c_旋轉.int_旋轉 == 270) {
                img_width = int_img_h;
                img_height = int_img_w;
            }

            //判斷要用寬度還是高度來全滿
            if (img_height / double_比例 > b.ActualHeight) {
                w = (b.ActualHeight - 10) / (img_height / double_比例) * thisWidth;
            } else {
                w = thisWidth - 10;
            }

            //載入圖片時，避免很小的圖片也被放大至全滿
            if (bool_初始載入 == true) {
                if ((w / int_img_w * 1.0f) > 1) {
                    func_檢視原始大小();
                    return;
                }
            }

            //修改UI界面顯示的縮放比例
            fun_修改圖片size(w);


            //設定圖片最大值
            double_imgMaxSize = img_width * 24;
            if (double_imgMaxSize < thisWidth) {
                double_imgMaxSize = thisWidth;
            }


            /*
            //設定圖片寬度
            double double_比例 = img_width / b.ActualWidth;
            double w = 1;

            if (img_height / double_比例 > b.ActualHeight) {
                w = (b.ActualHeight - 10) / (img_height / double_比例) * b.ActualWidth;
            } else {
                w = b.ActualWidth - 10;
            }
            fun_修改圖片size(w);


            //設定圖片最大值
            double_imgMaxSize = img_width * 24;
            if (double_imgMaxSize < b.ActualWidth) {
                double_imgMaxSize = b.ActualWidth;
            }
            */
            //記錄圖片縮放用的size
            if (s_img_type_顯示類型 == "WEB") {

            } else if (s_img_type_顯示類型 == "GIF") {
                int_size = img_gif.Width;
            } else if (s_img_type_顯示類型 == "MOVIE") {
                int_size = img_voide.Width;
            } else {
                int_size = img.Width;
            }

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="int_下一資料夾"></param>
        public void func_開啟下一資料夾(int int_下一資料夾) {

            String[] ar = null;
            String path = ar_path[int_目前圖片位置];
            String docPath = Directory.GetParent(path).FullName;

            try {
                String path_父目錄 = Directory.GetParent(docPath).FullName;
                if (path_父目錄 == null) {
                    return;
                }

                ar = Directory.GetDirectories(path_父目錄);
                ar = c_排序.func_資料夾排序(ar);
            } catch {
                return;
            }

            //取得目前位置
            int xx = 0;
            for (int i = 0; i < ar.Length; i++) {
                if (docPath == ar[i]) {
                    xx = i;
                    break;
                }
            }

            for (int i = 0; i < ar.Length; i++) {

                if (int_下一資料夾 == 1) {//下一資料夾
                    xx += 1;
                } else {//上一資料夾
                    xx -= 1;
                }

                //避免超出界限
                if (xx >= ar.Length) {
                    xx = 0;
                }
                if (xx < 0) {
                    xx = ar.Length - 1;
                }

                //判斷資料夾裡面是否裡面有圖片
                String ppp = ar[xx];
                bool bool_有效資料夾 = false;

                String[] ar_ff = new string[0];
                try {
                    ar_ff = Directory.GetFiles(ppp);
                } catch { }

                foreach (String item in ar_ff) {

                    if (item.Length > 6) {
                        String x = item.Substring(item.Length - 7).ToUpper();

                        foreach (var item2 in ar_附檔名_關聯) {
                            if (x.Substring(7 - item2.Length) == item2) {
                                bool_有效資料夾 = true;
                                break;
                            }
                        }
                        if (bool_有效資料夾)
                            break;
                    }//if
                }//for

                if (bool_有效資料夾) {
                    int_目前圖片位置 = 0;
                    fun_載入圖片或資料夾(ar[xx]);
                    return;
                }

            }//for


        }






        /// <summary>
        /// 
        /// </summary>
        public void func_開啟_說明() {
            c_set.fun_開啟網址("https://hbl917070.github.io/aeropic/index.html#/explain_soft/" + s_程式版本, false);
        }


        /// <summary>
        /// 
        /// </summary>
        public void func_開啟_設定() {
            if (w_設定 != null)
                w_設定.Close();
            w_設定 = new W_設定(this);
            w_設定.Owner = this;
            w_設定.Show();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public void func_鎖定視窗(MainWindow m, String type) {

            bool bool_目前顯示 = true;
            if (type == null || type.ToLower() == "auto" || type == "") {//自動
                bool_目前顯示 = !m.Topmost;
            }
            if (type.ToLower() == "false") {//手動強制
                bool_目前顯示 = false;
            }

            if (type.ToLower() == "no") {
                bool_目前顯示 = m.Topmost;
            }


            if (bool_目前顯示 == false) {
                m.but_鎖.Background = new SolidColorBrush { Color = Color.FromScRgb(0, 0, 0, 0) };
                m.Topmost = false;
            } else {
                m.but_鎖.Background = new SolidColorBrush { Color = Color.FromScRgb(0.2f, 1, 1, 1) };
                m.Topmost = true;
            }
        }



        /// <summary>
        /// type = auto(自動變化) 或 false 或 true 或 no(無變化)
        /// </summary>
        /// <param name="type"></param>
        public void func_顯示或隱藏exif視窗(String type) {

            bool bool_目前顯示 = true;
            if (type == null || type.ToLower() == "auto" || type == "") {//自動
                bool_目前顯示 = !bool_顯示exif視窗;
            }
            if (type.ToLower() == "false") {//手動強制
                bool_目前顯示 = false;
            }
            if (type.ToLower() == "no") {
                bool_目前顯示 = bool_顯示exif視窗;
            }

            //顯示或隱藏eixf視窗
            if (bool_目前顯示 == false) {
                u_顯示exif.func_類型_一般();
                bool_顯示exif視窗 = false;
                fun_動畫(stackPanel_exif_box, 0, 30, "X", () => { });
            } else {
                u_顯示exif.func_類型_高亮();
                bool_顯示exif視窗 = true;
                fun_動畫(stackPanel_exif_box, 30, 0, "X", () => { });
            }
            c_影像.c_exif.fun_顯示或隱藏視窗();

        }


        /// <summary>
        /// type = auto 或 false 或 true 或 no(無變化)
        /// </summary>
        public void func_顯示或隱藏工具列(String type) {

            bool bool_目前顯示 = true;
            if (type == null || type.ToLower() == "auto" || type == "") {//自動
                bool_目前顯示 = scrollViewer_工具列.Visibility == Visibility.Collapsed;
            }
            if (type.ToLower() == "false") {//手動強制
                bool_目前顯示 = false;
            }
            if (type.ToLower() == "no") {
                bool_目前顯示 = bool_顯示工具列;
            }



            if (bool_目前顯示) {
                scrollViewer_工具列.Visibility = Visibility.Visible;
                //meun_顯示隱藏工具列.t01.Text = "隱藏工具列";
                bool_顯示工具列 = true;
                func_隱藏工具列_套用樣式(false);
                u_工具列.func_類型_高亮();

                if (u_大量瀏覽模式 != null) {
                    u_大量瀏覽模式.dockPanel_功能列.Visibility = Visibility.Visible;
                }

                if (type.ToLower() == "auto") {
                    fun_動畫(scrollViewer_工具列, -15, 0, "Y", () => { });
                }

            } else {
                scrollViewer_工具列.Visibility = Visibility.Collapsed;
                //meun_顯示隱藏工具列.t01.Text = "顯示工具列";
                bool_顯示工具列 = false;
                func_隱藏工具列_套用樣式(true);
                u_工具列.func_類型_一般();

                if (u_大量瀏覽模式 != null) {
                    u_大量瀏覽模式.dockPanel_功能列.Visibility = Visibility.Collapsed;
                }
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bool_3"></param>
        private void func_隱藏工具列_套用樣式(bool bool_3) {

            if (bool_全螢幕) {
                bac_標題列.Visibility = Visibility.Collapsed;
                bac.Margin = new Thickness(0, 0, 0, 0);
                return;
            }

            if (bool_3 == false) {
                bac_標題列.Height = 68;
                bac.Margin = new Thickness(0, 68, 0, 0);
            } else {
                bac_標題列.Height = 33;
                bac.Margin = new Thickness(0, 33, 0, 0);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public void func_開啟右鍵選單() {

            //顯示或隱藏輸出成GIF
            /*if (text_imgType.Text.Contains("pixiv")) {
                propertyMenu_輸出GIF.Visibility = Visibility.Visible;

            } else {
                propertyMenu_輸出GIF.Visibility = Visibility.Collapsed;
            }*/

            popup_選單.IsOpen = false;


            var p_選單 = popup_選單_右鍵;
            var p_選單_容器 = popup_選單_右鍵_容器;

            MainWindow.fun_動畫(p_選單_容器, -10, 0, "Y", () => { });

            p_選單.StaysOpen = false;
            p_選單.Placement = System.Windows.Controls.Primitives.PlacementMode.Absolute;
            p_選單.IsOpen = true;

            var pos = System.Windows.Forms.Cursor.Position;

            p_選單.HorizontalOffset = pos.X / d_解析度比例_x;
            p_選單.VerticalOffset = pos.Y / d_解析度比例_y - 120;



            //u_menu_主視窗右鍵.func_open_滑鼠旁邊();
        }






        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">資料夾或檔案</param>
        public List<String> fun_取得圖片名單(String path) {

            //避免檔案不存在
            /*if (File.Exists(path) == false && Directory.Exists(path) == false && path.Contains("***") == false) {
                return new List<String>();
            }*/


            //如果是自定名單，就直接回傳事先已經選取的項目
            if (bool_自定圖片名單) {

                //移除不存在的項目
                for (int i = ar_自定圖片名單.Count - 1; i >= 0; i--) {
                    if (File.Exists(ar_自定圖片名單[i]) == false && Directory.Exists(ar_自定圖片名單[i]) == false) {
                        ar_自定圖片名單.RemoveAt(i);
                    }
                }


                return ar_自定圖片名單;
            }



            String docPath = path.Substring(0, path.LastIndexOf("\\"));

            //避免根目錄出現問題
            if (docPath.Substring(docPath.Length - 1) == ":") {
                docPath = docPath + "\\";
            }

            if (Directory.Exists(docPath) == false) {
                return new List<String>();
            }



            //篩選出所有能夠使用的類型
            List<String> ar = new List<String>();
            foreach (String item in Directory.GetFiles(docPath)) {

                if (item.Length > 6) {
                    String x = item.Substring(item.Length - 7).ToUpper();

                    foreach (var item2 in ar_附檔名_關聯) {
                        if (x.Substring(7 - item2.Length) == item2) {
                            ar.Add(item);
                            break;
                        }
                    }

                }//if

            }//for

            //讀取 P網 動圖 資料夾
            foreach (var item in Directory.GetDirectories(docPath)) {
                if (File.Exists(item + "\\animation.json")) {
                    ar.Add(item);
                }
            }

            //讀取 P網 動圖 ZIP
            List<String> ar_zip = new List<string>();
            foreach (var item in Directory.GetFiles(docPath, "*.zip")) {
                if (new FileInfo(item).Length / 1024 / 1024 < 300)//小於300M的壓縮檔才判斷
                    ar_zip.Add(item);
            }
            foreach (var item in Directory.GetFiles(docPath, "*.pixiv")) {
                ar_zip.Add(item);
            }
            foreach (var item in ar_zip) {
                try {
                    using (var x = ZipFile.OpenRead(item)) {
                        foreach (var item2 in x.Entries) {
                            /*if (item2.FullName.Length >= 5) {
                                String sn = item2.FullName.Substring(item2.FullName.Length - 4).ToUpper();
                                if (sn != ".JPG" && sn != ".PNG" && sn != ".GIF" && sn != ".BMP" && sn != "JSON") {
                                    break;
                                }
                            }*/
                            if (item2.FullName == "animation.json") {
                                ar.Add(item);
                                break;
                            }
                        }//foreach
                    }//suing
                } catch { }
            }//foreach


            //排序
            var ar2 = c_排序.func_檔案排序(ar);

            //回存
            var ar3 = new List<string>();
            foreach (var item in ar2) {

                ar3.Add(item);
            }

            return ar3;

        }




        /// <summary>
        /// 
        /// </summary>
        private void fun_刪除檔案() {

            String path = ar_path[int_目前圖片位置];

            if (path.Contains(fun_執行檔路徑() + "\\data")) {
                MessageBox.Show("請不要刪除data資料夾裡面的檔案");
                return;
            }

            if (File.Exists(path) == false) {
                return;
            }

            try {
                //把檔案移到垃圾桶，並顯示詢問視窗
                Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(path,
                    Microsoft.VisualBasic.FileIO.UIOption.AllDialogs,
                    Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);

                ar_path = new List<string>();
                ar_path = fun_取得圖片名單(path);


                if (int_目前圖片位置 >= ar_path.Count) {//避免超出陣列
                    int_目前圖片位置 = ar_path.Count - 1;
                }

                if (ar_path.Count > 0) {
                    fun_顯示圖片(ar_path[int_目前圖片位置]);
                } else {
                    fun_載入圖片或資料夾(fun_執行檔路徑() + "\\data\\imgs\\start.png");
                }

                this.fun_主視窗取得焦點();

            } catch (System.OperationCanceledException e) {//使用者取消

            } catch (Exception e) {
                MessageBox.Show(e.ToString());
            }

        }




        /// <summary>
        /// 
        /// </summary>
        public void fun_載入圖片或資料夾(String path) {

            if (System.IO.Directory.Exists(path)) {//資料夾

                //讀取 P網 動圖 資料夾                    
                //if (new DirectoryInfo(path).GetFiles("animation.json").Length > 0) {
                if (File.Exists(path + "\\animation.json")) {//有【animation.json】才視為P網動圖

                    fun_初始載入圖片(path);
                    return;
                }
                fun_初始載入圖片(path + "\\*3*4*5");//隨便給一個名稱

            } else if (System.IO.File.Exists(path)) {//圖片

                fun_初始載入圖片(path);

            } else {//如果不是圖片也不是資料夾

                fun_初始載入圖片(fun_執行檔路徑() + "\\data\\imgs\\start.png");
            }

        }



        #region 拖曳檔案的視窗

        public W_拖曳開啟 w_拖曳開啟;


        /// <summary>
        /// 
        /// </summary>
        public void fun_開啟拖曳檔案的視窗() {

            w_拖曳開啟.Width = this.ActualWidth + 2;
            w_拖曳開啟.Height = this.ActualHeight + 2;
            w_拖曳開啟.Left = (this.PointToScreen(new Point(0, 0)).X - 1) / d_解析度比例_x;
            w_拖曳開啟.Top = (this.PointToScreen(new Point(0, 0)).Y - 1) / d_解析度比例_y;
            w_拖曳開啟.Visibility = Visibility.Visible;

            w_拖曳開啟.Focus();
        }



        /// <summary>
        /// 
        /// </summary>
        private void event_將資料夾用圖片檢視器開啟() {

            w_拖曳開啟 = new W_拖曳開啟(this);
            w_拖曳開啟.Visibility = Visibility.Collapsed;
            w_拖曳開啟.Owner = this;
            //w_拖曳開啟.Topmost = true;

            //允許檔案拖曳
            this.AllowDrop = true;


            this.AddHandler(Window.DragOverEvent, new DragEventHandler((object sender, DragEventArgs e) => {


                /*if (e.Data.GetDataPresent(DataFormats.FileDrop) || e.Data.GetDataPresent(DataFormats.Html)) {
                    e.Effects = DragDropEffects.All;
                } else {        
                    e.Effects = DragDropEffects.None;
                }

                e.Handled = false;

                 */
                if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                    fun_開啟拖曳檔案的視窗();
                } else {
                    e.Effects = DragDropEffects.None;
                }

            }), true);

            /*
            this.AddHandler(Window.DropEvent, new DragEventHandler((object sender, DragEventArgs e) => {
                if (e.Data.GetDataPresent(DataFormats.FileDrop)) {//檔案
                    string[] docPath = ((string[])e.Data.GetData(DataFormats.FileDrop));
                    if (docPath.Length > 1) {
                        bool_自定圖片名單 = true;
                        fun_自定圖片名單(docPath);
                    } else {
                        bool_自定圖片名單 = false;
                    }
                    fun_載入圖片或資料夾(docPath[0]);
                }
                w_拖曳開啟.Visibility = Visibility.Collapsed;
            }), true);*/

        }

        #endregion



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String func_取得目前資料夾路徑() {

            if (bool_自定圖片名單) {
                return "";
            }

            if (ar_path.Count == 0) {
                return "";
            }

            return System.IO.Path.GetDirectoryName(ar_path[0]);

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ar_file"></param>
        public void fun_自定圖片名單(String[] ar_file) {

            ar_自定圖片名單 = new List<string>();

            //篩選出所有能夠使用的類型
            List<String> ar = new List<String>();
            List<String> ar_zip = new List<String>();


            List<string> ar_排除 = new List<string>();//排除常見的附檔名
            ar_排除.Add("TXT");
            ar_排除.Add("INI");
            ar_排除.Add("CSS");
            ar_排除.Add("JS");
            ar_排除.Add("HTML");
            ar_排除.Add("RAR");
            ar_排除.Add("DOCX");
            ar_排除.Add("DOC");
            ar_排除.Add("PPT");
            ar_排除.Add("PPTX");
            ar_排除.Add("DLL");
            ar_排除.Add("XML");
            ar_排除.Add("TTF");
            ar_排除.Add("BAT");
            ar_排除.Add("SYS");
            ar_排除.Add("CONFIG");
            ar_排除.Add("LOG");
            ar_排除.Add("REG");
            ar_排除.Add("H");
            ar_排除.Add("C");
            ar_排除.Add("CS");
            ar_排除.Add("INF");
            ar_排除.Add("CAT");
            ar_排除.Add("DB");
            ar_排除.Add("DAT");
            ar_排除.Add("PHP");
            ar_排除.Add("LIB");
            ar_排除.Add("7Z");
            ar_排除.Add("SQL");
            //ar_排除.Add("AI");
            ar_排除.Add("HTM");
            ar_排除.Add("MP3");
            //ar_排除.Add("PDF");
            ar_排除.Add("MIDI");
            ar_排除.Add("M4A");
            ar_排除.Add("FLAC");
            ar_排除.Add("DMP");
            ar_排除.Add("JSON");
            ar_排除.Add("ZIP");
            ar_排除.Add("ISO");
            ar_排除.Add("XLS");
            ar_排除.Add("XLSX");
            ar_排除.Add("PY");
            ar_排除.Add("FBX");
            ar_排除.Add("X3D");
            ar_排除.Add("WAV");
            ar_排除.Add("APK");
            ar_排除.Add("LESS");
            ar_排除.Add("R");
            ar_排除.Add("CPP");
            ar_排除.Add("GO");
            ar_排除.Add("JAVA");
            ar_排除.Add("CLASS");
            ar_排除.Add("JAR");
            ar_排除.Add("BIN");
            ar_排除.Add("CAT");
            ar_排除.Add("RTF");
            ar_排除.Add("DATA");
            ar_排除.Add("REG");
            ar_排除.Add("ICC");
            ar_排除.Add("CHM");
            ar_排除.Add("MSI");
            ar_排除.Add("AAC");
            ar_排除.Add("VQF");
            ar_排除.Add("APE");
            ar_排除.Add("OGG");
            ar_排除.Add("ASF");


            //處理選取的『檔案』
            foreach (var item in ar_file) {

                bool bool_add = true;

                if (File.Exists(item)) {

                    //排除
                    if (item.Length > 6) {
                        String x = item.Substring(item.Length - 7).ToUpper();
                        foreach (var item2 in ar_排除) {
                            if (x.Substring(7 - item2.Length) == item2) {
                                bool_add = false;
                                break;
                            }
                        }
                    }//if

                    if (bool_add)
                        ar.Add(item);

                    /*
                    //從附檔名判斷
                    if (item.Length > 6) {
                        String x = item.Substring(item.Length - 7).ToUpper();
                        foreach (var item2 in ar_附檔名_關聯) {
                            if (x.Substring(7 - item2.Length) == item2) {
                                ar.Add(item);
                                break;
                            }
                        }
                    }//if*/

                    //讀取 P網 動圖 ZIP
                    if (item.Substring(item.Length - 4).ToUpper() == ".ZIP") {
                        ar_zip.Add(item);
                    }
                    if (item.Substring(item.Length - 6).ToUpper() == ".PIXIV") {
                        ar_zip.Add(item);
                    }

                }
            }

            //處理Pixiv動圖
            foreach (var item in ar_zip) {
                try {
                    using (var x = ZipFile.OpenRead(item)) {
                        foreach (var item2 in x.Entries) {
                            if (item2.FullName.Equals("animation.json")) {
                                ar.Add(item);
                                break;
                            }
                        }//foreach
                    }//suing
                } catch { }
            }//foreach

            //排序
            //var ar2 = ar.ToArray();
            //Array.Sort(ar2, new Sort_自然排序_正());

            var ar2 = c_排序.func_檔案排序(ar);

            //回存
            foreach (var item in ar2) {
                ar_自定圖片名單.Add(item);
            }

            bool_自定圖片名單 = false;//避免取得資料夾裡面的資料時出錯，所以暫時切回false

            //處理選取的『資料夾』
            foreach (var item in ar_file) {
                if (Directory.Exists(item)) {
                    foreach (var item2 in fun_取得圖片名單(item + "\\***阿")) {
                        ar_自定圖片名單.Add(item2);//取得該資料夾裡面的所有圖片，並加入到lise裡面
                    }
                }
            }
            bool_自定圖片名單 = true;

        }



        /// <summary>
        /// 修改UI界面上面顯示的縮放比例
        /// </summary>
        /// <param name="w">輸入圖片寬度</param>
        private void fun_修改圖片size(double w) {

            if (s_img_type_顯示類型 == "WEB") {
                return;
            }

            if (w < 1) {//避免寬度小於0而出現錯誤
                w = 1;
            }

            if (s_img_type_顯示類型 == "GIF") {
                img_gif.Width = w;
            } else if (s_img_type_顯示類型 == "MOVIE") {
                img_voide.Width = w;
            } else {
                img.Width = w;
            }

            text_圖片比例.Text = Math.Ceiling(((w / int_img_w * 1.0f) * 100)) + "%";
            func_隱藏局部高清();


        }




        /// <summary>
        /// 讀取圖片名單，並且顯示圖片
        /// </summary>
        /// <param name="path">資料夾或檔案</param>
        private void fun_初始載入圖片(String path) {

            ar_path = new List<string>();


            ar_path = fun_取得圖片名單(path);

            c_排序.func_判斷是否已設定過排序(func_取得目前資料夾路徑());

            //再次套用排序，避免第一次載入圖片，排序沒有生效
            ar_path = c_排序.func_檔案排序(ar_path).ToList<String>();




            int_目前圖片位置 = -1;

            for (int i = 0; i < ar_path.Count; i++) {
                if (ar_path[i] == path) {
                    int_目前圖片位置 = i;
                    break;
                }
            }

            //如果名單內找不到，就是預設沒有關聯的附檔名，把該圖片放在第一張
            if (int_目前圖片位置 == -1) {
                int_目前圖片位置 = 0;
                if (File.Exists(path)) {
                    ar_path.Insert(0, path);
                }
            }

            if (u_大量瀏覽模式 != null) {

                fun_重啟大量閱讀模式();

                //
                new Thread(() => {
                    Thread.Sleep(20);
                    C_adapter.fun_UI執行緒(() => {
                        //u_大量瀏覽模式.web01.Document.InvokeScript("eval", new object[] { "window.location.reload()" });
                        try {
                            u_大量瀏覽模式.web01.Refresh();
                        } catch { }

                    });

                }).Start();


                //u_大量瀏覽模式.func_重新載入圖片();

            } else {
                fun_顯示圖片(path);
            }

            /*if (this.Visibility == Visibility.Visible && DockPanel_一般圖片.Visibility != Visibility) {
                if (u_大量瀏覽模式 == null) {
                    //u_大量瀏覽模式.fun_關閉大量瀏覽();

                    fun_新建大量閱讀模式();
                } else {
                    fun_重啟大量閱讀模式();
                    u_大量瀏覽模式.web01.Refresh();
                    //u_大量瀏覽模式.func_重新載入圖片();

                }
            } else {
                fun_顯示圖片(path);
            }*/
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public void func_新視窗開啟圖片_大量瀏覽模式(String path) {

            c_set.fun_儲存_position(true);//儲存目前的視窗位置
            var mw = new MainWindow("open_img", path);//新開視窗
            if (Topmost) {//判斷視窗是否有需要置頂
                func_鎖定視窗(mw, "true");
            }
            mw.Show();

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public static void func_新視窗開啟圖片(String path) {

            App.main_s = path.Split('\n');
            if (App.main_s.Length == 1 && App.main_s[0] == "")
                App.main_s = new string[] { };


            var mw = new MainWindow();//新開視窗    
            mw.Show();

        }



        /// <summary>
        /// 
        /// </summary>
        public System.Drawing.Point fun_取得滑鼠() {
            return System.Windows.Forms.Cursor.Position;
        }


        /// <summary>
        /// 
        /// </summary>
        public void fun_主視窗取得焦點() {
            obj_焦點.Focus();
        }

        /// <summary>
        /// 
        /// </summary>
        public String fun_執行檔路徑() {
            String s = System.AppDomain.CurrentDomain.BaseDirectory;
            if (s.Substring(s.Length - 1) == "\\") {
                return s.Substring(0, s.Length - 1);
            }
            return s;
        }


        /// <summary>
        /// 取得視窗局部截圖
        /// </summary>
        public BitmapSource SaveTo(System.Windows.FrameworkElement v) {
            /// get bound of the visual
            Rect b = VisualTreeHelper.GetDescendantBounds(v);

            /// new a RenderTargetBitmap with actual size of c
            RenderTargetBitmap r = new RenderTargetBitmap(
                (int)this.ActualWidth, (int)this.ActualHeight,
                96, 96, PixelFormats.Pbgra32);

            /// render visual
            r.Render(this);

            int int_x = (int)(v.PointToScreen(new Point(0, 0)).X - this.PointToScreen(new Point(0, 0)).X);
            int int_y = (int)(v.PointToScreen(new Point(0, 0)).Y - this.PointToScreen(new Point(0, 0)).Y);

            CroppedBitmap chainedBitMap = null;
            try {
                chainedBitMap = new CroppedBitmap(r, new Int32Rect(int_x, int_y, (int)b.Width, (int)b.Height));
            } catch { }

            return chainedBitMap;

            /// new a JpegBitmapEncoder and add r into it 
            JpegBitmapEncoder e = new JpegBitmapEncoder();
            e.Frames.Add(BitmapFrame.Create(chainedBitMap));

            /// new a FileStream to write the image file
            FileStream s = new FileStream("1.jpg",
                FileMode.OpenOrCreate, FileAccess.Write);
            e.Save(s);
            s.Close();
        }



        /// <summary>
        /// 清理記憶體
        /// </summary>
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool SetProcessWorkingSetSize(IntPtr proc, int min, int max);
        public void fun_清理記憶體() {
            try {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
                    SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                }
            } catch { }
        }






        /// <summary>
        /// 縮放webbrowser，正常100、最小10
        /// </summary>
        public void fun_Zoom(System.Windows.Forms.WebBrowser web, int factor) {
            try {
                SHDocVw.IWebBrowser2 axIWebBrowser2 = (SHDocVw.IWebBrowser2)web.ActiveXInstance;
                object pvaIn = factor;

                axIWebBrowser2.ExecWB(SHDocVw.OLECMDID.OLECMDID_OPTICAL_ZOOM,
                   SHDocVw.OLECMDEXECOPT.OLECMDEXECOPT_DONTPROMPTUSER,
                   ref pvaIn, IntPtr.Zero);
            } catch {
                //MessageBox.Show("");
            }
        }







        /// <summary>
        /// 使用IE11核心
        /// </summary>
        // set WebBrowser features, more info: http://stackoverflow.com/a/18333982/1768303
        public static void fun_升級web核心() {
            // don't change the registry if running in-proc inside Visual Studio
            if (LicenseManager.UsageMode != LicenseUsageMode.Runtime)
                return;

            //判斷IE版本的方法
            var GetBrowserEmulationMode = new Func<UInt32>(() => {

                int browserVersion = 0;
                using (var ieKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer",
                    RegistryKeyPermissionCheck.ReadSubTree,
                    System.Security.AccessControl.RegistryRights.QueryValues)) {
                    var version = ieKey.GetValue("svcVersion");
                    if (null == version) {
                        version = ieKey.GetValue("Version");
                        if (null == version)
                            throw new ApplicationException("Microsoft Internet Explorer is required!");
                    }
                    int.TryParse(version.ToString().Split('.')[0], out browserVersion);
                }

                if (browserVersion < 7) {
                    throw new ApplicationException("Unsupported version of Microsoft Internet Explorer!");
                }

                UInt32 mode = 11000; // Internet Explorer 11. Webpages containing standards-based !DOCTYPE directives are displayed in IE11 Standards mode. 

                switch (browserVersion) {
                    case 7:
                        mode = 7000; // Webpages containing standards-based !DOCTYPE directives are displayed in IE7 Standards mode. 
                        break;
                    case 8:
                        mode = 8000; // Webpages containing standards-based !DOCTYPE directives are displayed in IE8 mode. 
                        break;
                    case 9:
                        mode = 9000; // Internet Explorer 9. Webpages containing standards-based !DOCTYPE directives are displayed in IE9 mode.                    
                        break;
                    case 10:
                        mode = 10000; // Internet Explorer 10.
                        break;
                }

                return mode;
            });

            var appName = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            var featureControlRegKey = @"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main\FeatureControl\";

            //修改預設IE版本
            Registry.SetValue(featureControlRegKey + "FEATURE_BROWSER_EMULATION", appName, GetBrowserEmulationMode(), RegistryValueKind.DWord);

            //使用完整的IE瀏覽器功能

            Registry.SetValue(featureControlRegKey + "FEATURE_AJAX_CONNECTIONEVENTS", appName, 1, RegistryValueKind.DWord);
            Registry.SetValue(featureControlRegKey + "FEATURE_GPU_RENDERING", appName, 1, RegistryValueKind.DWord);
            Registry.SetValue(featureControlRegKey + "FEATURE_WEBOC_DOCUMENT_ZOOM", appName, 0, RegistryValueKind.DWord);
            //  Registry.SetValue(featureControlRegKey + "FEATURE_NINPUT_LEGACYMODE", appName, 0, RegistryValueKind.DWord);//預設=1，1=用舊版輸入模型，0=啓用win8的平滑捲動模式（會導致視窗無法拖拖拽）

            Registry.SetValue(featureControlRegKey + "FEATURE_SPELLCHECKING", appName, 0, RegistryValueKind.DWord);//關閉拼字檢查

        }






    }








}
