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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TiefSee.W {

    /// <summary>
    /// U_menu_main.xaml 的互動邏輯
    /// </summary>
    public partial class U_menu_main : UserControl {


        public U_menu_main() {
            InitializeComponent();
        }


        public U_menu_main(String s_圖示, String s_t) {

            InitializeComponent();
            lab_圖示.Style = (Style)FindResource(s_圖示);

            t_文字.Text = s_t;

        }



        /// <summary>
        /// 滑鼠移入時，顯示的說明文字
        /// </summary>
        /// <param name="v"></param>
        public void set_title(string v) {
            this.ToolTip = v;
        }


        /// <summary>
        /// 
        /// </summary>
        public void func_類型_高亮() {
            lab_圖示.Foreground = new SolidColorBrush { Color = Color.FromArgb(255, 75, 200, 255) };
            t_文字.Foreground = new SolidColorBrush { Color = Color.FromArgb(255, 75, 200, 255) };
            but.IsHitTestVisible = true;//允許點擊
        }

        /// <summary>
        /// 
        /// </summary>
        public void func_類型_一般() {
            lab_圖示.Foreground = new SolidColorBrush { Color = Color.FromArgb(255, 255, 255, 255) };
            t_文字.Foreground = new SolidColorBrush { Color = Color.FromArgb(255, 255, 255, 255) };
            but.IsHitTestVisible = true;//允許點擊
        }

        /// <summary>
        /// 
        /// </summary>
        public void func_類型_鎖定() {
            lab_圖示.Foreground = new SolidColorBrush { Color = Color.FromArgb(100, 255, 255, 255) };
            t_文字.Foreground = new SolidColorBrush { Color = Color.FromArgb(100, 255, 255, 255) };
            but.IsHitTestVisible = false;//禁止點擊
        }



    }
}
