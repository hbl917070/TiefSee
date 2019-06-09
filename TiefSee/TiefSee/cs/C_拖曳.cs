using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TiefSee.cs;
using static TiefSee.C_視窗拖曳改變大小;

namespace TiefSee {

    public partial class MainWindow {



        private Thread thread_拖曳圖片;//拖曳圖片用的執行緒

        private double double_keen = 1.5;//拖曳靈敏度
        private Boolean bool_允許拖曳瀏覽 = false;





        /// <summary>
        /// 
        /// </summary>
        private void event_拖曳圖片() {

            //按下
            b.MouseLeftButtonDown += B_MouseDown;
            b.MouseUp += (sender, e) => {
                bool_允許拖曳瀏覽 = false;
            };

            //拖曳
            thread_拖曳圖片 = new Thread(fun_拖曳圖片);
            thread_拖曳圖片.Start();


        }




        /// <summary>
        /// 放開滑鼠
        /// </summary>
        private void MouseHookMouseUp(object sender, System.Drawing.Point p) {
            bool_允許拖曳瀏覽 = false;

            

            MouseHook.OnMouseUp -= MouseHookMouseUp;//放開
        }


        /// <summary>
        /// 
        /// </summary>
        public bool fun_判斷滑鼠是否在右下角() {
            var x = fun_取得滑鼠().X;
            var y = fun_取得滑鼠().Y;
            var xx = this.PointToScreen(new Point(0, 0)).X + (this.ActualWidth*d_解析度比例_x) - 20;
            var yy = this.PointToScreen(new Point(0, 0)).Y + (this.ActualHeight*d_解析度比例_y) - 20;

            //MessageBox.Show($"X: {x}\nL: {this.PointToScreen(new Point(0, 0)).X }\nW: {this.ActualWidth }");

            if (x >= xx && x < xx + 20 && y >= yy && y < yy + 20) {

                return true;
            }
            return false;
        }



        /// <summary>
        /// 按下滑鼠
        /// </summary>
        private void B_MouseDown(object sender, MouseButtonEventArgs e) {

            //取消文字框的焦點
            if (stackPlanel_動圖工具.Visibility == Visibility.Visible) {
                fun_主視窗取得焦點();
            }



            //改變程式size
            if (fun_判斷滑鼠是否在右下角()) {


                // } else if (grid_img.ActualWidth <= b2.ActualWidth && grid_img.ActualHeight <= b.ActualHeight) { //在圖片小於視窗的情況下直接拖曳視窗
            } else if (scrollviewer_1.ExtentHeight < scrollviewer_1.ViewportHeight && scrollviewer_1.ExtentWidth < scrollviewer_1.ViewportWidth) { //在圖片小於視窗的情況下直接拖曳視窗


                try {
                    //this.DragMove();
                    if (this.WindowState != WindowState.Maximized)//不是全螢幕
                        c_視窗改變大小.ResizeWindow(ResizeDirection.Move);//拖曳視窗
                } catch { }

            } else {//拖曳瀏覽圖片

                MouseHook.OnMouseUp += MouseHookMouseUp;//放開

                bool_允許拖曳瀏覽 = true;

                //記錄點下時的位置
                m_x = fun_取得滑鼠().X;
                m_y = fun_取得滑鼠().Y;
                m_top = scrollviewer_1.VerticalOffset;
                m_left = scrollviewer_1.HorizontalOffset;
            }


        }





        /// <summary>
        /// 拖曳滑鼠
        /// </summary>
        private void fun_拖曳圖片() {

            while (bool_程式運行中) {

                Thread.Sleep(10);

                if (bool_允許拖曳瀏覽 == false) {
                    continue;
                }

                C_adapter.fun_UI執行緒(() => {

                    func_隱藏局部高清_移動時();

                    var int_top = m_top + (m_y - fun_取得滑鼠().Y) * double_keen;
                    var int_left = m_left + (m_x - fun_取得滑鼠().X) * double_keen;

                    //當拖曳超出捲軸最大之時，則重新抓取拖動前記錄的坐標，這樣就不用返回到原點才能往回拖曳
                    if (int_top < 0) {
                        m_y = fun_取得滑鼠().Y;
                        m_top = 0;
                    }
                    if (int_top >= scrollviewer_1.ScrollableHeight) {
                        m_y = fun_取得滑鼠().Y;
                        m_top = scrollviewer_1.ScrollableHeight;
                    }
                    if (int_left < 0) {
                        m_x = fun_取得滑鼠().X;
                        m_left = 0;
                    }
                    if (int_left >= scrollviewer_1.ScrollableWidth) {
                        m_x = fun_取得滑鼠().X;
                        m_left = scrollviewer_1.ScrollableWidth;
                    }

                    //修改捲軸位置
                    scrollviewer_1.ScrollToVerticalOffset(int_top);
                    scrollviewer_1.ScrollToHorizontalOffset(int_left);
                });

            }//while

        }







    }



}
