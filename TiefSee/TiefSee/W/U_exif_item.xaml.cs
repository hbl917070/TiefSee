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
    /// U_exif_item.xaml 的互動邏輯
    /// </summary>
    public partial class U_exif_item : UserControl {
        public U_exif_item() {
            InitializeComponent();
        }


        /// <summary>
        /// 設定滑鼠指到上面時顯示的說明
        /// </summary>
        /// <param name="t"></param>
        public void fun_setToolTip(String t) {
            text_type.ToolTip = t;
        }


        /// <summary>
        /// 
        /// </summary>
        public void fun_不顯示分割線() {
            border.BorderBrush = new SolidColorBrush(Color.FromArgb(0,0,0,0));
        }


        /// <summary>
        /// 
        /// </summary>
        public void fun_淺色主題() {



            text_type.Foreground = new SolidColorBrush(Color.FromArgb(255,0,40,150));
            text_value.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));

            border.BorderBrush = new SolidColorBrush(Color.FromArgb(40, 0, 0, 0));

        }


    }
}
