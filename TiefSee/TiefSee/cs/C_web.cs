using TiefSee.cs;
using TiefSee.W;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using static TiefSee.C_視窗拖曳改變大小;
using static TiefSee.MainWindow;

namespace TiefSee {



    public class C_web {

        MainWindow M;

        public C_web(MainWindow m) {
            this.M = m;
        }





        String path = "";


        [DllImport("user32.dll")]
        private static extern int SetWindowLong(HandleRef hWnd, int nIndex, int dwNewLong);

        /// <summary>
        /// 設定Windows.Forms.Form 為 WPF 的子視窗
        /// </summary>
        /// <param name="form"></param>
        /// <param name="owner"></param>
        public static void SetOwner(System.Windows.Forms.Form form, System.Windows.Window owner) {
            WindowInteropHelper helper = new WindowInteropHelper(owner);
            SetWindowLong(new HandleRef(form, form.Handle), -8, helper.Handle.ToInt32());
        }




        /// <summary>
        /// 
        /// </summary>
        public void fun_web初始化(String s_path) {



            if (M.Topmost == true) {

                M.Topmost = false;

                //初始化web
                M.w_web = new F_web(M);
                M.img_web = M.w_web.webBrowser1;
                SetOwner(M.w_web, M);

                M.Topmost = true;

            } else {

                //初始化web
                M.w_web = new F_web(M);

                M.img_web = M.w_web.webBrowser1;
                SetOwner(M.w_web, M);

            }





            bool bool_移動中 = false;
            bool bool_刷新背景 = true;

            //改變視窗size時隱藏web，避免拖曳速度很慢
            M.SizeChanged += (sender, e) => {
                if (M.s_img_type_顯示類型 == "WEB") {
                    //img_web_box.Visibility = Visibility.Collapsed;
                    //img_web.Visible = false;
                    //w_web.Visibility = Visibility.Collapsed;
                    //w_web.Visible = false;

                    bool_移動中 = true;
                }
            };



            //改變視窗size時，不要立即變動web size
            new Thread(() => {


                int n_修改視窗size = 0;
            



                while (M.bool_程式運行中) {

                    n_修改視窗size += 1;
                    
                    Thread.Sleep(30);

                    if (M.s_img_type_顯示類型 == "WEB") {


                        //讓web視窗跟著主視窗移動
                        C_adapter.fun_UI執行緒(() => {
                            if (M.bool_程式運行中) {


                                if (M.w_web.Left != (int)((M.b.PointToScreen(new Point(0, 0)).X + 5))) {

                                    M.w_web.Left = (int)((M.b.PointToScreen(new Point(0, 0)).X + 5));
                                    M.w_web.Top = (int)(M.b.PointToScreen(new Point(0, 0)).Y);                   

                                }


                            }
                        });


                        //每90毫秒修改一次視窗size
                        if (n_修改視窗size >= 3 ) {
                            n_修改視窗size = 0;


                            //避免快速瀏覽的視窗已經隱藏了，但是web視窗還沒隱藏
                            if (M.Visibility != Visibility.Visible) {
                                C_adapter.fun_UI執行緒(() => {
                                    M.fun_載入圖片或資料夾(M.fun_執行檔路徑() + "/data/imgs/space.png");
                                });
                            }

                            C_adapter.fun_UI執行緒(() => {

                                int bw = (int)((M.b2.ActualWidth - 10) * M.d_解析度比例_x);
                                int bh = (int)((M.b.ActualHeight - 5) * M.d_解析度比例_y);

                                if (bw <= 10) bw = 10;
                                if (bh <= 10) bh = 10;

                                if (M.w_web.Width != bw || M.w_web.Height != bh) {

                                    M.w_web.Width = bw;
                                    M.w_web.Height = bh;

                                } else {
                                    if (bool_移動中) {
                                        //如果大小跟視窗一樣，才顯示回來
                                        bool_移動中 = false;
                                        //w_web.Visible = true;
                                        M.img_web.Document.InvokeScript("eval", new Object[] { "setTimeout(function () { fun_imgSizeChange(); fun_beyond();}, 50);" });//重新調整圖片位置                                                                                                                                                               
                                        M.fun_主視窗取得焦點();

                                    }
                                }
                            });

                        }

                      


                     

                    }

                }//while
            }).Start();




            //初始網頁
            this.path = s_path;
            M.img_web.Navigated += Img_web_Navigated;

            M.img_web.Navigate(M.fun_執行檔路徑() + "/data/img.html");//載入     
            M.img_web.ObjectForScripting = new C_web呼叫javaScript(M);//讓網頁允許存取C#



            M.img_web.Navigating += (sender2, e2) => {
                M.fun_Zoom(M.img_web, 100);//網頁比例100%

                M.fun_載入圖片或資料夾(e2.Url.ToString());
                e2.Cancel = true;


            };


        }





        /// <summary>
        /// 網頁載入完成後，才載入GIF
        /// </summary>
        private void Img_web_Navigated(object sender123, System.Windows.Forms.WebBrowserNavigatedEventArgs e123) {


            C_滑鼠偵測_滾動.MouseWheel += C_滑鼠偵測_滾動_MouseWheel;
            //M.w_web.MouseWheel += C_滑鼠偵測_滾動_MouseWheel;

            /*
            M.w_web.MouseWheel += (sernder, e) => {
                MessageBox.Show("" + e.Delta);
            };*/

            //web的選單
            M.img_web.Document.ContextMenuShowing += (object sender, System.Windows.Forms.HtmlElementEventArgs e) => {


                M.func_開啟右鍵選單();
            };



            //雙擊web 全螢幕 用的全域變數
            DateTime time_雙擊web全螢幕 = DateTime.Now;//
            System.Drawing.Point point_雙擊web全螢幕 = M.fun_取得滑鼠();


            //讓web也能拖曳視窗
            M.img_web.Document.MouseDown += ((sender2, e2) => {


                M.img_web.Document.Focus();

                if (e2.MouseButtonsPressed == System.Windows.Forms.MouseButtons.Left) {



                    //雙擊web 全螢幕
                    if (((TimeSpan)(DateTime.Now - time_雙擊web全螢幕)).TotalMilliseconds < 400) {//連點低於400毫秒
                        if (M.grid_換頁按鈕_下.Visibility == Visibility.Visible &&
                                M.fun_取得滑鼠().Y > M.PointToScreen(new Point(0, 0)).Y + M.Height - 100 &&
                                M.fun_取得滑鼠().Y < M.PointToScreen(new Point(0, 0)).Y + M.Height - 20 &&
                                M.fun_取得滑鼠().X > M.PointToScreen(new Point(0, 0)).X + 20 &&
                                M.fun_取得滑鼠().X < M.PointToScreen(new Point(0, 0)).X + M.ActualWidth - 20) {
                            return;
                        }

                        if (point_雙擊web全螢幕 == M.fun_取得滑鼠())//同一個點
                            if (M.WindowState != WindowState.Maximized) {
                                M.WindowState = WindowState.Maximized;
                            } else {
                                M.WindowState = WindowState.Normal;
                            }
                        time_雙擊web全螢幕 = DateTime.Now.AddHours(-5);//避免連點3下又跑回來
                    } else {
                        time_雙擊web全螢幕 = DateTime.Now;
                    }
                    point_雙擊web全螢幕 = M.fun_取得滑鼠();


                    try {
                        if (M.fun_判斷滑鼠是否在右下角()) {//讓右下角可以拖曳改變視窗大小
                            if (M.WindowState != WindowState.Maximized)
                                M.c_視窗改變大小.ResizeWindow(ResizeDirection.BottomRight);
                        } else {
                            //如果網頁沒有捲軸，就拖曳視窗
                            String x = (String)M.img_web.Document.InvokeScript("fun_bool_movie", new Object[] { });//用js判斷目前是否有捲軸
                            if (x.ToString().Equals("true"))
                                if (M.WindowState != WindowState.Maximized) {//不是全螢幕
                                    M.c_視窗改變大小.ResizeWindow(ResizeDirection.Move);//拖曳視窗
                                }
                        }
                    } catch { }

                }

            });



            //延遲載入，避免GIF無法顯示
            new Thread(() => {
                Thread.Sleep(1);
                C_adapter.fun_UI執行緒(() => {

                    M.c_set.fun_套用setting設定();
                    M.fun_載入圖片或資料夾(path);

                });
            }).Start();

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void C_滑鼠偵測_滾動_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e) {


            //如果視窗沒有焦點，就不執行
            if (M.IsActive == false) {
                return;
            }

            var mm = M.fun_取得滑鼠();

            int int_X = mm.X - M.w_web.Left;
            int int_Y = mm.Y - M.w_web.Top;

            if (M.s_img_type_顯示類型 == "WEB") {
                if (mm.X > M.w_web.Left && mm.X < M.w_web.Left + M.w_web.Width &&
                   mm.Y > M.w_web.Top && mm.Y < M.w_web.Top + M.w_web.Height) {


                    M.func_滾輪控制(e.Delta);

                    /*if (e.Delta > 0) {
                        M.img_web.Document.InvokeScript("eval", new object[] { "fun_imgSizeAdd({ clientX: " + int_X + ", clientY: " + int_Y + " })" });
                    } else {
                        M.img_web.Document.InvokeScript("eval", new object[] { "fun_imgSizeSubtrat({ clientX: " + int_X + ", clientY: " + int_Y + " })" });
                    }*/
                }

            }




        }





    }





    [ComVisible(true)]
    public class C_web呼叫javaScript {

        //   window.external.fun_setSizeText

        private MainWindow M;

        public C_web呼叫javaScript(MainWindow m) {
            this.M = m;
        }

        public void net_next_page() {
            M.fun_下一張();
        }

        public void net_previous_page() {
            M.fun_上一張();
        }

        /// <summary>
        /// 如果是GIF，就用javascript計算完圖片長寬後，在會傳到C#裡面
        /// </summary>
        /// <param name="s_w"></param>
        /// <param name="s_h"></param>
        public void net_setSizeText(String s_w, String s_h) {

            M.fun_設定顯示圖片size(Int32.Parse(s_w), Int32.Parse(s_h)); //顯示寬高

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        public void net_修改顯示圖片比例(String s) {
            M.text_圖片比例.Text = s;
        }


        /// <summary>
        /// 
        /// </summary>
        public void net_主視窗取得焦點() {
            M.fun_主視窗取得焦點();
        }

        /// <summary>
        /// 開啟『拖曳檔案』的視窗
        /// </summary>
        public void net_drag_window() {
            M.fun_開啟拖曳檔案的視窗();
        }




        public int net_滾輪操作() {
            return (int)M._e_滾輪用途;
        }

        public Boolean net_keyIsLeftCtr() {
            return System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftCtrl);
        }


    }





}
