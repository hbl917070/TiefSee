using System;
using System.Collections.Generic;
using System.Linq;
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

namespace TiefSee.W {
    /// <summary>
    /// U_menu.xaml 的互動邏輯
    /// </summary>
    public partial class U_menu : UserControl {


        MainWindow M;


        /*
        public U_menu() {
            InitializeComponent();
            this.popup_選單.StaysOpen = false;
        }*/


        public U_menu(MainWindow m) {

            this.M = m;

            InitializeComponent();


            this.popup_選單.StaysOpen = false;

        }

     
        /// <summary>
        /// 圖示 使用 style 的 icon 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="icon_name"></param>
        /// <param name="ac"></param>
        /// <returns></returns>
        public U_menu_item func_add_menu_icon(String t, String icon_name, Action ac) {

            var umi = func_add_menu(t, null, ac);
            umi.func_setIcon(icon_name);
        
            return umi;
        }


        /// <summary>
        /// 圖示 使用 圖片路徑
        /// </summary>
        /// <param name="t"></param>
        /// <param name="img_path"></param>
        /// <param name="ac"></param>
        /// <returns></returns>
        public U_menu_item func_add_menu_imgPath(String t, String img_path, Action ac) {

            BitmapSource img = null;
            if (System.IO.File.Exists(img_path))
                try {
                    img = M.c_影像.func_get_BitmapImage_JPG(img_path);
                } catch { }

            var umi = func_add_menu(t, img, ac);

            return umi;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="img"></param>
        /// <param name="ac"></param>
        /// <returns></returns>
        public U_menu_item func_add_menu(String t, BitmapSource img, Action ac) {


            var item = new U_menu_item();
            item.t01.Text = t;
            if (img != null)
                item.img01.Source = img;

            item.but01.Click += (semder, e) => {
                popup_選單.IsOpen = false;
                ac();
            };


            st_容器.Children.Add(item);

            return item;
        }



        /// <summary>
        /// 
        /// </summary>
        public void func_add_水平線() {
            var item = new Border();
            //item.Background = new SolidColorBrush(Color.FromArgb(100, 75, 200, 255));
            item.Background = (SolidColorBrush)M.FindResource("sol_淺藍40"); ;//透明度30
            item.Margin = new Thickness(20, 10, 20, 10);

            item.Height = 1;
            st_容器.Children.Add(item);
        }



        /// <summary>
        /// 於物件下方開啟
        /// </summary>
        /// <param name="obj_but"></param>
        public void func_open(FrameworkElement obj_but) {
            MainWindow.fun_動畫(popup_選單_容器, -10, 0, "Y", () => { });
            popup_選單.PlacementTarget = obj_but;
            popup_選單.Placement = System.Windows.Controls.Primitives.PlacementMode.Center;
            popup_選單.IsOpen = true;

            popup_選單.HorizontalOffset = 0;
            popup_選單.VerticalOffset = (popup_選單_容器.ActualHeight / M.d_解析度比例_y / 2) + (obj_but.ActualHeight / 2) - 15;

        }




        /// <summary>
        /// 於滑鼠下方開啟
        /// </summary>
        public void func_open_滑鼠下方() {
            MainWindow.fun_動畫(popup_選單_容器, -10, 0, "Y", () => { });
            popup_選單.Placement = System.Windows.Controls.Primitives.PlacementMode.Absolute;
            popup_選單.IsOpen = true;

            var pos = System.Windows.Forms.Cursor.Position;

            popup_選單.HorizontalOffset = pos.X / M.d_解析度比例_x - (popup_選單_容器.ActualWidth / M.d_解析度比例_x / 2);
            popup_選單.VerticalOffset = pos.Y / M.d_解析度比例_y - 35;

        }


        /// <summary>
        /// 於滑鼠旁邊
        /// </summary>
        public void func_open_滑鼠旁邊() {

            MainWindow.fun_動畫(popup_選單_容器, -10, 0, "Y", () => { });
            popup_選單.Placement = System.Windows.Controls.Primitives.PlacementMode.Absolute;
            popup_選單.IsOpen = true;

            var pos = System.Windows.Forms.Cursor.Position;

            popup_選單.HorizontalOffset = pos.X / M.d_解析度比例_x;
            popup_選單.VerticalOffset = pos.Y / M.d_解析度比例_y - 15;

        }









    }
}
