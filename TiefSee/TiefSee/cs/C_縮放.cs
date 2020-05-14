using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TiefSee {
    public partial class MainWindow {




        private double m_x;
        private double m_y;
        private double m_top;
        private double m_left;
        private double xxx;
        private double yyy;
        public double int_size = 500;//縮放的比例
        private double double_imgMaxSize = 8100;



        private void event_縮放() {
            scrollviewer_1.PreviewMouseWheel += Scrollviewer_1_PreviewMouseWheel;
            ((System.Windows.Controls.Grid)but_bottom_上一頁.Parent).PreviewMouseWheel += Scrollviewer_1_PreviewMouseWheel;
        }



        /// <summary>
        /// 縮放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Scrollviewer_1_PreviewMouseWheel(object sender, MouseWheelEventArgs e) {


            //避免開啟exif視窗後，滑鼠在exif視窗上也對圖片進行縮放
            if (fun_取得滑鼠().X > (b.PointToScreen(new Point(0, 0)).X) + b.ActualWidth * d_解析度比例_x) {
                return;
            }


            func_滾輪控制(e.Delta);

            e.Handled = true;

            /*img_set_size.Width = img.ActualHeight;
            img_set_size.Height = img.ActualWidth;
            var dd_w = (img.ActualHeight) * -1;
            var dd_h = (img.ActualWidth) * -1;
            img.Margin = new Thickness(dd_w, dd_h, dd_w, dd_h);*/

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="nub"></param>
        public void func_滾輪控制(int nub) {


            //如果按著ctrl，則一律使用「縮放圖片」
            if (Keyboard.IsKeyDown(Key.LeftCtrl)) {


                if (nub > 0) {
                    fun_放大圖片(false);
                } else {
                    fun_縮小圖片(false);
                }
                return;
            }


            //縮放圖片
            if (_e_滾輪用途 == e_滾輪用途.縮放圖片) {
                if (nub > 0) {
                    fun_放大圖片(false);
                } else {
                    fun_縮小圖片(false);
                }
                return;
            }

            //切換上下一張圖片
            if (_e_滾輪用途 == e_滾輪用途.換頁) {
                if (nub > 0) {
                    fun_上一張();
                } else {
                    fun_下一張();
                }
                return;
            }


            //切換上下一張圖片
            if (_e_滾輪用途 == e_滾輪用途.上下移動) {
                if (nub > 0) {
                    func_圖片移動_上();
                } else {
                    func_圖片移動_下();
                }
                return;
            }

        }


        /// <summary>
        /// 
        /// </summary>
        private void fun_縮小圖片(bool bool_從中央) {

            if (s_img_type_顯示類型 == "WEB") {


                if (w_web == null) {//如果還沒初始化就不執行 
                    return;
                }

                if (bool_從中央) {
                    img_web.Document.InvokeScript("fun_imgSizeSubtrat", new Object[] { });
                } else {

                    var mm = fun_取得滑鼠();

                    int int_X = mm.X - w_web.Left;
                    int int_Y = mm.Y - w_web.Top;

                    img_web.Document.InvokeScript("eval", new object[] { "fun_imgSizeSubtrat({ clientX: " + int_X + ", clientY: " + int_Y + " })" });

                }
                return;
            }

            func_隱藏局部高清();

            try {

                int_size *= 0.9090909090909;

                if (int_size <= 5) {
                    int_size /= 0.9090909090909;
                    return;
                }

                fun_修改圖片size(int_size);


                //從中央作為縮放起點
                if (bool_從中央) {

                    xxx = b.PointToScreen(new Point(0, 0)).X + (b.ActualWidth / 2) - b.PointToScreen(new Point(0, 0)).X + scrollviewer_1.HorizontalOffset;
                    yyy = b.PointToScreen(new Point(0, 0)).Y + (b.ActualHeight / 2) - b.PointToScreen(new Point(0, 0)).Y + scrollviewer_1.VerticalOffset;

                } else {//從滑鼠位置開始縮放

                    //計算游標目前在圖片的坐標
                    xxx = fun_取得滑鼠().X - b.PointToScreen(new Point(0, 0)).X + scrollviewer_1.HorizontalOffset;
                    yyy = fun_取得滑鼠().Y - b.PointToScreen(new Point(0, 0)).Y + scrollviewer_1.VerticalOffset;

                }

                //計算圖片改變大小後的差距
                var xx2 = grid_img.ActualWidth - grid_img.ActualWidth / 0.9090909090909;
                var yy2 = grid_img.ActualHeight - grid_img.ActualHeight / 0.9090909090909;

                //儲存目前的捲軸位置
                var top2 = scrollviewer_1.VerticalOffset;
                var left2 = scrollviewer_1.HorizontalOffset;

                scrollviewer_1.ScrollToVerticalOffset(top2 + ((yyy / grid_img.ActualHeight) * yy2) * 0.9090909090909);
                scrollviewer_1.ScrollToHorizontalOffset(left2 + ((xxx / grid_img.ActualWidth) * xx2) * 0.9090909090909);


            } catch { }
        }




        /// <summary>
        /// 
        /// </summary>
        private void fun_放大圖片(bool bool_從中央) {


            if (s_img_type_顯示類型 == "WEB") {


                if (w_web == null) {//如果還沒初始化就不執行 
                    return;
                }

                if (bool_從中央) {
                    img_web.Document.InvokeScript("fun_imgSizeAdd", new Object[] { });
                } else {

                    var mm = fun_取得滑鼠();

                    int int_X = mm.X - w_web.Left;
                    int int_Y = mm.Y - w_web.Top;

                    img_web.Document.InvokeScript("eval", new object[] { "fun_imgSizeAdd({ clientX: " + int_X + ", clientY: " + int_Y + " })" });

                }
                return;
            }



            func_隱藏局部高清();

            try {

                int_size *= 1.1;

                if (int_size >= double_imgMaxSize) {
                    int_size /= 1.1;
                    return;
                }

                fun_修改圖片size(int_size);

                //從中央作為縮放起點
                if (bool_從中央) {

                    xxx = b.PointToScreen(new Point(0, 0)).X + (b.ActualWidth / 2) - b.PointToScreen(new Point(0, 0)).X + scrollviewer_1.HorizontalOffset;
                    yyy = b.PointToScreen(new Point(0, 0)).Y + (b.ActualHeight / 2) - b.PointToScreen(new Point(0, 0)).Y + scrollviewer_1.VerticalOffset;

                } else {//從滑鼠位置開始縮放

                    //計算游標目前在圖片的坐標
                    xxx = fun_取得滑鼠().X - b.PointToScreen(new Point(0, 0)).X + scrollviewer_1.HorizontalOffset;
                    yyy = fun_取得滑鼠().Y - b.PointToScreen(new Point(0, 0)).Y + scrollviewer_1.VerticalOffset;

                }


                //計算圖片改變大小後的差距
                var xx2 = grid_img.ActualWidth - grid_img.ActualWidth / 1.1;
                var yy2 = grid_img.ActualHeight - grid_img.ActualHeight / 1.1;

                //儲存目前的捲軸位置
                var top2 = scrollviewer_1.VerticalOffset;
                var left2 = scrollviewer_1.HorizontalOffset;

                scrollviewer_1.ScrollToVerticalOffset(top2 + ((yyy / grid_img.ActualHeight) * yy2) * 1.1);
                scrollviewer_1.ScrollToHorizontalOffset(left2 + ((xxx / grid_img.ActualWidth) * xx2) * 1.1);




            } catch { }

        }





    }
}
