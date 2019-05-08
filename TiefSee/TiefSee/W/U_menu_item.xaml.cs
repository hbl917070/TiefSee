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
    /// U_menu_item.xaml 的互動邏輯
    /// </summary>
    public partial class U_menu_item : UserControl {
        public U_menu_item() {
            InitializeComponent();
        }


        /// <summary>
        /// 使用 style 的icon 來替代圖片
        /// </summary>
        /// <param name="icon_name"></param>
        public void func_setIcon(String icon_name) {

           lab_icon.Style = (Style)FindResource(icon_name);
           lab_icon_border.Visibility = Visibility.Visible;
           img01.Visibility = Visibility.Collapsed;

        }

    }
}
