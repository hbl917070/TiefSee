using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TiefSee.W {
    /// <summary>
    /// W_拖曳開啟.xaml 的互動邏輯
    /// </summary>
    public partial class W_拖曳開啟 : Window {




        MainWindow M;

        public W_拖曳開啟(MainWindow m) {
            this.M = m;

            InitializeComponent();

            //滑鼠進入時
            this.MouseUp += (sefsmde, ee) => {
                //避免某些特殊情況，『拖曳視窗』沒有正常被關閉，滑鼠再次進入視窗時關閉
                this.Visibility = Visibility.Collapsed;//關閉
            };



            event_將資料夾用圖片檢視器開啟();



            var tim = new System.Windows.Forms.Timer();
            tim.Interval = 500;



            tim.Tick += (sender, e) => {
                if (this.Visibility == Visibility.Visible) {

                    try {
                        var mouse = M.fun_取得滑鼠();

                        var double_Left = (this.PointToScreen(new Point(0, 0)).X) / M.d_解析度比例_x;
                        var double_Top = (this.PointToScreen(new Point(0, 0)).Y) / M.d_解析度比例_y;
                        var double_w = this.ActualWidth;
                        var double_h = this.ActualHeight;


                        if (mouse.X / M.d_解析度比例_x < double_Left) {
                            this.Visibility = Visibility.Collapsed;//關閉
                        } else if (mouse.Y / M.d_解析度比例_y < double_Top) {
                            this.Visibility = Visibility.Collapsed;//關閉
                        } else if (mouse.X / M.d_解析度比例_x > double_Left + double_w) {
                            this.Visibility = Visibility.Collapsed;//關閉
                        } else if (mouse.Y / M.d_解析度比例_y > double_Top + double_h) {
                            this.Visibility = Visibility.Collapsed;//關閉
                        }
                    } catch { }


                }
            };
            tim.Start();
        }




        /// <summary>
        /// 
        /// </summary>
        private void event_將資料夾用圖片檢視器開啟() {



            //允許檔案拖曳
            this.AllowDrop = true;


            this.AddHandler(Window.DragOverEvent, new DragEventHandler((object sender, DragEventArgs e) => {

                //this.Visibility = Visibility.Visible;

                if (e.Data.GetDataPresent(DataFormats.FileDrop) || e.Data.GetDataPresent(DataFormats.Html)) {
                    e.Effects = DragDropEffects.All;
                } else {
                    e.Effects = DragDropEffects.None;
                }
                e.Handled = false;

            }), true);

            //結束拖曳檔案時
            this.AddHandler(Window.DragLeaveEvent, new DragEventHandler((object sender, DragEventArgs e) => {
                this.Visibility = Visibility.Collapsed;
            }), true);


            this.AddHandler(Window.DropEvent, new DragEventHandler((object sender, DragEventArgs e) => {

                if (e.Data.GetDataPresent(DataFormats.FileDrop)) {//檔案
                    string[] docPath = ((string[])e.Data.GetData(DataFormats.FileDrop));

                    if (docPath.Length > 1) {
                        M.bool_自定圖片名單 = true;
                        M.fun_自定圖片名單(docPath);
                    } else {
                        M.bool_自定圖片名單 = false;
                    }

                    M.fun_載入圖片或資料夾(docPath[0]);

                }

                this.Visibility = Visibility.Collapsed;//執行完畢後關閉視窗

            }), true);

        }


    }
}
