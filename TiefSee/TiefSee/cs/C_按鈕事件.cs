using TiefSee.W;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Threading;
using System.Windows.Controls;

namespace TiefSee {




    public partial class MainWindow {


        /// <summary>
        /// 
        /// </summary>
        private void event_註冊所有按鈕事件() {
            event_右鍵選單();
            event_按鈕_main選單_擴充();
            event_按鈕_main選單();
            event_按鈕_工具列();
            event_按鈕_選單_大量();
            event_下拉選單按鈕背景();
        }




        /// <summary>
        /// 
        /// </summary>
        private void event_按鈕_選單_大量() {

            but_menu_大量_上一資料夾.Click += (sender, e) => {
                func_開啟下一資料夾(0);
            };
            but_menu_大量_下一資料夾.Click += (sender, e) => {
                func_開啟下一資料夾(1);
            };

            but_menu_大量_排序.Click += (sender, e) => {
                popup_選單_大量.IsOpen = false;
                c_排序.func_開啟選單_滑鼠下方();
            };
            but_menu_大量_結束大量瀏覽.Click += (sender, e) => {
                popup_選單_大量.IsOpen = false;
                u_大量瀏覽模式.fun_關閉大量瀏覽();
            };
            but_menu_大量_瀏覽器開啟.Click += (sender, e) => {
                popup_選單_大量.IsOpen = false;
                u_大量瀏覽模式.func_外部瀏覽器開啟();
            };

            but_menu_大量_開啟資料夾.Click += (sender, e) => {
                popup_選單_大量.IsOpen = false;
                fun_用檔案總管開啟目前圖片();
            };

            but_menu_大量_全螢幕.Click += (sender, e) => {
                popup_選單_大量.IsOpen = false;
                func_全螢幕(true);
            };

            but_menu_大量_設定.Click += (sender, e) => {
                popup_選單_大量.IsOpen = false;
                func_開啟_設定();
            };

            but_menu_大量_工具列.Click += (sender, e) => {
                //popup_選單_大量.IsOpen = false;
                func_顯示或隱藏工具列("auto");
            };

        }



        /// <summary>
        /// 
        /// </summary>
        private void event_按鈕_main選單() {


            but_選單.Click += (sermder, e) => {

                Popup pop = popup_選單;
                System.Windows.Controls.Grid pop_容器 = popup_選單_容器;

                if (u_大量瀏覽模式 != null) {
                    pop = popup_選單_大量;
                    pop_容器 = popup_選單_容器_大量;
                }

                fun_動畫(pop_容器, -10, 0, "Y", () => { });

                pop.StaysOpen = false;
                pop.PlacementTarget = but_選單;
                pop.Placement = System.Windows.Controls.Primitives.PlacementMode.Center;
                pop.IsOpen = true;
                pop.VerticalOffset = (pop_容器.ActualHeight / 2) + (but_選單.ActualHeight / 2) - 15;

                // MessageBox.Show(text_imgType.Text);


            };








            //main選單 按鈕
            but_menu_main_上一張.Click += (sender, e) => {
                fun_上一張();
            };
            but_menu_main_下一張.Click += (sender, e) => {
                fun_下一張();
            };
            but_menu_main_上一資料夾.Click += (sender, e) => {
                func_開啟下一資料夾(0);
            };
            but_menu_main_下一資料夾.Click += (sender, e) => {
                func_開啟下一資料夾(1);
            };


            but_menu_main_全滿.Click += (sender, e) => {
                fun_圖片全滿();
            };
            but_menu_main_放大.Click += (sender, e) => {
                func_放大圖片();
            };
            but_menu_main_縮小.Click += (sender, e) => {
                func_圖片縮小();
            };
            but_menu_main_原始大小.Click += (sender, e) => {
                func_檢視原始大小();
            };
            but_menu_main_旋轉.Click += (sender, e) => {
                popup_選單.IsOpen = false;
                c_旋轉.u_menu_旋轉.func_open_滑鼠下方();
            };
            but_menu_main_排序.Click += (sender, e) => {
                popup_選單.IsOpen = false;
                c_排序.func_開啟選單_滑鼠下方();
            };




        }



        /// <summary>
        /// 
        /// </summary>
        private void event_按鈕_main選單_擴充() {

            var u_1 = new U_menu_main("icon_外部開啟", "其他開啟");
            u_1.set_title("用其他程式開啟圖片");
            u_1.but.Click += (senrer, e) => {
                popup_選單.IsOpen = false;
                c_按鈕選單_其他程式開啟.u_menu_用外部程式開啟.func_open_滑鼠下方();
            };
            grid_選單_main.Children.Add(u_1);


            var u_2 = new U_menu_main("icon_複製", "複製");
            u_2.but.Click += (senrer, e) => {
                popup_選單.IsOpen = false;
                c_按鈕選單_複製.func_隱藏不必要的選項();
                c_按鈕選單_複製.u_menu_複製.func_open_滑鼠下方();
            };
            grid_選單_main.Children.Add(u_2);



            var u_3 = new U_menu_main("icon_搜圖", "搜圖");
            u_3.set_title("在網路上搜尋此圖片");
            u_3.but.Click += (senrer, e) => {
                popup_選單.IsOpen = false;
                c_按鈕選單_搜圖.u_menu_搜圖.func_open_滑鼠下方();
            };
            grid_選單_main.Children.Add(u_3);


            var u_4 = new U_menu_main("icon_大量瀏覽模式", "大量瀏覽模式");
            u_4.set_title("進入『大量瀏覽模式』");
            u_4.but.Click += (senrer, e) => {
                popup_選單.IsOpen = false;
                fun_新建大量閱讀模式();
            };
            grid_選單_main.Children.Add(u_4);


            u_顯示exif = new U_menu_main("icon_exif訊息", "EXIF資訊");
            u_顯示exif.set_title("顯示或隱藏『EXIF資訊』");
            u_顯示exif.but.Click += (senrer, e) => {
                //popup_選單.IsOpen = false;

                func_顯示或隱藏exif視窗("auto");

            };
            grid_選單_main.Children.Add(u_顯示exif);


            u_工具列 = new U_menu_main("icon_工具列", "工具列");
            u_工具列.set_title("顯示或隱藏「工具列」");
            u_工具列.but.Click += (senrer, e) => {
                func_顯示或隱藏工具列("auto");

            };
            grid_選單_main.Children.Add(u_工具列);




            var u_7 = new U_menu_main("icon_垃圾桶", "刪除");
            u_7.but.Click += (senrer, e) => {
                popup_選單.IsOpen = false;
                fun_刪除檔案();
            };
            grid_選單_main.Children.Add(u_7);



            u_解析動圖 = new U_menu_main("icon_解析動圖", "解析GIF");
            u_解析動圖.set_title("解析「GIF動圖」的每一幀");
            u_解析動圖.but.Click += (senrer, e) => {

                popup_選單.IsOpen = false;
                c_set.fun_儲存_position(true);//儲存目前的視窗位置
                String path = ar_path[int_目前圖片位置];
                String s_path_output = c_影像.func_分解GIF(path);//分解GIF
                var mw = new MainWindow("open_gif", s_path_output);//新開視窗

                if (this.Topmost) {//判斷視窗是否有需要置頂
                    func_鎖定視窗(mw, "true");
                }
                mw.Show();

            };
            grid_選單_main.Children.Add(u_解析動圖);


            u_轉存GIF = new U_menu_main("icon_儲存", "轉存GIF");
            u_轉存GIF.set_title("將「pixiv動圖」轉存成「GIF動圖」");
            u_轉存GIF.but.Click += (senrer, e) => {
                popup_選單.IsOpen = false;
                c_P網.fun_輸出成GIF();
            };
            grid_選單_main.Children.Add(u_轉存GIF);


            var u_6 = new U_menu_main("icon_設定", "設定");
            u_6.but.Click += (senrer, e) => {
                popup_選單.IsOpen = false;
                func_開啟_設定();
            };
            grid_選單_main.Children.Add(u_6);


            var u_10 = new U_menu_main("icon_說明", "說明");
            u_10.but.Click += (senrer, e) => {
                popup_選單.IsOpen = false;
                func_開啟_說明();
            };
            grid_選單_main.Children.Add(u_10);



            var u_11 = new U_menu_main("icon_全螢幕", "全螢幕");
            u_11.set_title("進入全螢幕模式  (F11)");
            u_11.but.Click += (senrer, e) => {
                popup_選單.IsOpen = false;
                func_全螢幕(true);
            };
            grid_選單_main.Children.Add(u_11);


        }



        public bool bool_全螢幕 = false;
        private WindowState ws_全螢幕前的狀態;
        private String s_全螢幕前的狀態_工具列 = "true";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        public void func_全螢幕(bool b) {

            if (b) {

                if (bool_全螢幕 == true) {
                    return;
                }

                ws_全螢幕前的狀態 = this.WindowState;//儲存 全螢幕前的狀態
                s_全螢幕前的狀態_工具列 = (scrollViewer_工具列.Visibility == Visibility.Visible) + "";

                this.WindowStyle = System.Windows.WindowStyle.None;//無邊框

                //在多螢幕的情況下最大化時，坐標與實際所在的螢幕不見得一樣，所以要手動設定
                var d_L = this.PointToScreen(new Point(0, 0)).X + 10;
                var d_T = this.PointToScreen(new Point(0, 0)).Y + 10;

                //先把程式視窗化，才不會無法最大化
                this.WindowState = System.Windows.WindowState.Normal;
                this.Left = d_L;
                this.Top = d_T;


                // new Thread(()=> {
                //Thread.Sleep(500);
                //  C_adapter.fun_UI執行緒(()=> {

                this.WindowState = System.Windows.WindowState.Maximized;
                func_顯示或隱藏工具列("false");
                dockPanel_標題.Visibility = Visibility.Collapsed;
                bac.Margin = new Thickness(0, 0, 0, 0);
                bac_標題列.Visibility = Visibility.Collapsed;
                this.Topmost = true;
                bool_全螢幕 = true;
                //    });
                //   }).Start();




            } else {

                bool_全螢幕 = false;
                this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
                this.WindowState = ws_全螢幕前的狀態;

                func_顯示或隱藏工具列(s_全螢幕前的狀態_工具列);
                dockPanel_標題.Visibility = Visibility.Visible;
                bac_標題列.Visibility = Visibility.Visible;
                func_鎖定視窗(this, "false");




            }

        }


        /// <summary>
        /// 
        /// </summary>
        private void event_按鈕_工具列() {


            //工具列按鈕            
            but_上一張.Click += (sender, e) => {
                fun_上一張();
            };
            but_下一張.Click += (sender, e) => {
                fun_下一張();
            };
            but_圖片全滿.Click += (sender, e) => {
                fun_圖片全滿();
            };
            but_圖片放大.Click += (sender, e) => {
                func_放大圖片();
            };
            but_圖片縮小.Click += (sender, e) => {
                func_圖片縮小();
            };

            but_排序.Click += (sender, e) => {
                c_排序.func_開啟選單_物件下方(but_排序);

            };
            but_上一資料夾.Click += (sender, e) => {
                func_開啟下一資料夾(0);
            };
            but_下一資料夾.Click += (sender, e) => {
                func_開啟下一資料夾(1);
            };
            but_刪除圖片.Click += (sender, e) => {
                fun_刪除檔案();
            };

            but_拖出檔案.PreviewMouseDown += (sender, e) => {
                if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed) {
                    string[] files = { ar_path[int_目前圖片位置] };
                    try {
                        var file = new DataObject(DataFormats.FileDrop, files);
                        DragDrop.DoDragDrop(but_拖出檔案, file, DragDropEffects.All);

                    } catch { }
                    e.Handled = true;
                }
            };

        }




        /// <summary>
        /// 
        /// </summary>
        private void event_右鍵選單() {

            this.ContextMenu = new System.Windows.Controls.ContextMenu();

            this.ContextMenuOpening += (senrder, e) => {
                e.Handled = true;
                func_開啟右鍵選單();
            };


            u_menu_右鍵_用檔案總管開啟.t01.Text = "開啟檔案位置";
            u_menu_右鍵_用檔案總管開啟.but01.Click += (senrder, e) => {
                popup_選單_右鍵.IsOpen = false;
                fun_用檔案總管開啟目前圖片();
            };

            u_menu_右鍵_原生右鍵.t01.Text = "檔案右鍵選單";
            u_menu_右鍵_原生右鍵.but01.Click += (senrder, e) => {
                popup_選單_右鍵.IsOpen = false;
                c_按鈕選單_其他程式開啟.fun_顯示原生右鍵選單(false);
            };

            u_menu_右鍵_複製影像.t01.Text = "複製影像";
            u_menu_右鍵_複製影像.but01.Click += (senrder, e) => {
                popup_選單_右鍵.IsOpen = false;
                c_按鈕選單_複製.fun_複製影像();
            };

            u_menu_右鍵_刪除圖片.t01.Text = "刪除圖片";
            u_menu_右鍵_刪除圖片.but01.Click += (senrder, e) => {
                popup_選單_右鍵.IsOpen = false;
                fun_刪除檔案();
            };

            u_menu_右鍵_設定.t01.Text = "設定";
            u_menu_右鍵_設定.but01.Click += (senrder, e) => {
                popup_選單_右鍵.IsOpen = false;
                func_開啟_設定();
            };

            u_menu_右鍵_說明.t01.Text = "說明 與 檢查更新";
            u_menu_右鍵_說明.but01.Click += (senrder, e) => {
                popup_選單_右鍵.IsOpen = false;
                func_開啟_說明();
            };

            u_menu_右鍵_關閉程式.t01.Text = "關閉程式";
            u_menu_右鍵_關閉程式.but01.Click += (senrder, e) => {
                popup_選單_右鍵.IsOpen = false;
                this.Close();
            };




            //main選單 按鈕
            but_menu_main_上一張1.Click += (sender, e) => {
                fun_上一張();
            };
            but_menu_main_下一張1.Click += (sender, e) => {
                fun_下一張();
            };
            but_menu_main_上一資料夾1.Click += (sender, e) => {
                func_開啟下一資料夾(0);
            };
            but_menu_main_下一資料夾1.Click += (sender, e) => {
                func_開啟下一資料夾(1);
            };


            but_menu_main_全滿1.Click += (sender, e) => {
                fun_圖片全滿();
            };
            but_menu_main_放大1.Click += (sender, e) => {
                func_放大圖片();
            };
            but_menu_main_縮小1.Click += (sender, e) => {
                func_圖片縮小();
            };
            but_menu_main_原始大小1.Click += (sender, e) => {
                func_檢視原始大小();
            };
            but_menu_main_旋轉1.Click += (sender, e) => {
                popup_選單_右鍵.IsOpen = false;
                c_旋轉.u_menu_旋轉.func_open_滑鼠下方();
            };
            but_menu_main_排序1.Click += (sender, e) => {
                popup_選單_右鍵.IsOpen = false;
                c_排序.func_開啟選單_滑鼠下方();
            };


            /*
            u_menu_主視窗右鍵.func_add_menu("說明 與 檢查更新", null, () => {
                func_開啟_說明();
            });

            u_menu_主視窗右鍵.func_add_menu("設定", null, () => {
                func_開啟_設定();
            });

            propertyMenu_輸出GIF = u_menu_主視窗右鍵.func_add_menu("輸出成GIF", null, () => {
                c_P網.fun_輸出成GIF();
            });

            u_menu_主視窗右鍵.func_add_水平線();

            meun_顯示隱藏工具列 = u_menu_主視窗右鍵.func_add_menu("隱藏工具列", null, () => {
                func_顯示或隱藏工具列("auto");
            });

            u_menu_主視窗右鍵.func_add_水平線();

            u_menu_主視窗右鍵.func_add_menu("關閉程式", null, () => {
                this.Close();
            });*/



        }




        /// <summary>
        /// 
        /// </summary>
        private void event_下拉選單按鈕背景() {

            Button[] ar_bur = { but_用外部程式開啟, but_複製, but_排序, but_旋轉, but_搜圖, but_選單 };

            foreach (var item in ar_bur) {
                item.Click += (sender, e) => {
                    func_下拉選單背景(true, (Button)sender);
                };
            }

            Popup[] ar_pop = {
                popup_選單,
                popup_選單_排序,
                popup_選單_大量 ,
                c_按鈕選單_其他程式開啟.u_menu_用外部程式開啟.popup_選單,
                c_旋轉.u_menu_旋轉.popup_選單,
                c_按鈕選單_複製.u_menu_複製.popup_選單,
                c_按鈕選單_搜圖.u_menu_搜圖.popup_選單
            };

            foreach (var item in ar_pop) {
                item.Closed += (sender, e) => {
                    func_下拉選單背景(false, null);
                };
            }


        }


        /// <summary>
        /// 下拉選單展開時，設定背景顏色
        /// </summary>
        /// <param name="show"></param>
        /// <param name="but"></param>
        public void func_下拉選單背景(bool show, Button but) {



            if (show) {
                re_下拉選單的背景.Visibility = Visibility.Visible;

                re_下拉選單的背景.Width = but.ActualWidth;
                re_下拉選單的背景.Height = but.ActualHeight+1;

                re_下拉選單的背景.Margin = new Thickness(
                    but.PointToScreen(new Point(0, 0)).X - grid_all.PointToScreen(new Point(0, 0)).X,
                    but.PointToScreen(new Point(0, 0)).Y - grid_all.PointToScreen(new Point(0, 0)).Y,
                0, 0);

            } else {

                re_下拉選單的背景.Visibility = Visibility.Collapsed;

            }

        }




    }










}
