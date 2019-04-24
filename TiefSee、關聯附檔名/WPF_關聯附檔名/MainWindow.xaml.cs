using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace WPF_關聯附檔名 {
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window {




        String s_name = "TiefSee";

        String s_path = @"D:\軟體\AeroPic\AeroPic.exe";


        /// <summary>
        /// 
        /// </summary>
        public MainWindow() {
            InitializeComponent();

            try {
                s_path = System.AppDomain.CurrentDomain.BaseDirectory;
                s_path = s_path.Substring(0, s_path.LastIndexOf('\\'));
                s_path = s_path.Substring(0, s_path.LastIndexOf('\\'));
                s_path = System.IO.Path.GetFullPath(s_path + "\\..\\TiefSee.exe");

                if (File.Exists(s_path) == false) {
                    MessageBox.Show("找不到檔案\n" + s_path);
                }
            } catch (Exception) {

                MessageBox.Show("路徑錯誤");
            }





            t_副檔名.Text = @"JPG
JPEG
PNG
GIF
BMP
TIF
TIFF
DDS
PGM
PPM
PNM
ICO
SVG
WEBP";

        }








        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e) {

            if (File.Exists(s_path) == false) {
                MessageBox.Show("關聯失敗，找不到檔案\n" + s_path);
                return;
            }



            List<String> ar_附檔名 = new List<string>();

            String[] ar = t_副檔名.Text.Split('\n');

            for (int i = 0; i < ar.Length; i++) {

                //避免特殊符號
                String item = ar[i].Trim().ToLower()
                             .Replace("\t", "").Replace("~", "").Replace("!", "").Replace("@", "")
                             .Replace("#", "").Replace("$", "").Replace("%", "").Replace("^", "")
                             .Replace("&", "").Replace("*", "").Replace("(", "").Replace(")", "")
                             .Replace("_", "").Replace("-", "").Replace("=", "").Replace("[", "")
                             .Replace("]", "").Replace("|", "").Replace("\\", "").Replace("/", "")
                             .Replace(";", "").Replace("'", "").Replace("\"", "").Replace("<", "")
                             .Replace(">", "").Replace("?", "").Replace("`", "").Replace(":", "")
                             .Replace(",", "").Replace(".", "").Replace(" ", "").Replace("+", "");

                //如果副檔名不是exe或空白
                if (item != "exe" && item != "bat" && item != "") {
                    ar_附檔名.Add(item);
                }

            }

            try {

                //設定哪一些附檔名要預設使用aeropic開啟
                foreach (var item in ar_附檔名) {
                    Registry.ClassesRoot.CreateSubKey("." + item).SetValue("", s_name, RegistryValueKind.String); //步驟1,2
                }

                //設定程式的路徑
                Registry.ClassesRoot.CreateSubKey(s_name + "\\shell\\open\\command").SetValue("", "\"" + s_path + "\"" + " \"%1\"", RegistryValueKind.ExpandString); //步驟3,4,5


                //註冊aeropic允許關聯的附檔名
                var SupportedTypes = Registry.ClassesRoot.CreateSubKey(s_name + "\\SupportedTypes");
                SupportedTypes.SetValue("", "", RegistryValueKind.String);

                foreach (var item in ar_附檔名) {
                    SupportedTypes.SetValue("." + item, "", RegistryValueKind.String);
                }

                /*
                SupportedTypes.SetValue(".jpeg", "", RegistryValueKind.String);
                SupportedTypes.SetValue(".png", "", RegistryValueKind.String);
                SupportedTypes.SetValue(".gif", "", RegistryValueKind.String);
                SupportedTypes.SetValue(".bmp", "", RegistryValueKind.String);
                SupportedTypes.SetValue(".svg", "", RegistryValueKind.String);
                SupportedTypes.SetValue(".psd", "", RegistryValueKind.String);
                SupportedTypes.SetValue(".tif", "", RegistryValueKind.String);
                SupportedTypes.SetValue(".ico", "", RegistryValueKind.String);
                SupportedTypes.SetValue(".pixiv", "", RegistryValueKind.String);*/
                SupportedTypes.Close();


                MessageBox.Show("TiefSee 已經與圖片進行關聯\n(" + s_path + ")");

            } catch (System.UnauthorizedAccessException) {

                MessageBox.Show("權限不足，請『右鍵』→『以系統管理員身份執行』此程式");

            } catch (Exception ee) {

                MessageBox.Show(ee.ToString());

            }



        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e) {


            try {

                //舊的名字
                try {
                    Registry.ClassesRoot.DeleteSubKeyTree("aeropic");
                } catch { }


                Registry.ClassesRoot.DeleteSubKeyTree(s_name);
                MessageBox.Show("已經解除 TiefSee 與圖片的關聯");

            } catch (System.ArgumentException) {

                MessageBox.Show("完全沒有關聯");

            } catch (System.UnauthorizedAccessException) {

                MessageBox.Show("權限不足，請『右鍵』→『以系統管理員身份執行』此程式");

            } catch (Exception ee) {

                MessageBox.Show(ee.ToString());

            }


        }








    }
}
