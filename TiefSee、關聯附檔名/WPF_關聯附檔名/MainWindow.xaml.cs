using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace WPF_關聯副檔名 {
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window {


        String s_appName = "TiefSee";
        String s_appPath = @"D:\軟體\AeroPic\AeroPic.exe";


        /// <summary>
        /// 
        /// </summary>
        public MainWindow() {
            InitializeComponent();

            try {

                s_appPath = System.AppDomain.CurrentDomain.BaseDirectory;
                s_appPath = Regex.Replace(s_appPath, @"([^\\]+\\+){2}\z", $"{s_appName}.exe"); //最後兩層資料夾的路徑名稱，以此置換

                if (File.Exists(s_appPath) == false) {
                    MessageBox.Show($"找不到 {s_appName} 的路徑：\n({s_appPath})。", String.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
                }

            } catch  {

                MessageBox.Show("路徑錯誤");

            }


            t_副檔名.Text = @"JPG/JPEG/PNG/GIF/BMP/TIF/TIFF/DDS/PGM/PPM/PNM/ICO/SVG/WEBP".Replace("/", "\n");

        }




        /// <summary>
        /// 設定 附檔名關聯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_AssociateFilenameExtension(object sender, RoutedEventArgs e) {

            if (File.Exists(s_appPath) == false) {
                MessageBox.Show($"關聯失敗，找不到 {s_appName} 的路徑：\n({s_appPath})。", String.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string[] ar_副檔名 = t_副檔名.Text.Split('\n');

            StringBuilder s_invalidFilenameExtensionsInformation = new StringBuilder();
            try {
                for (int i = 0; i < ar_副檔名.Length; ++i) {
                    ar_副檔名[i] = ar_副檔名[i].Trim(); //其實，副檔名可以前綴空白字元 ' '，這裡卻移除了空白字元的前綴
                    if (IsInvalidFilenameExtension(ar_副檔名[i])) {
                        s_invalidFilenameExtensionsInformation.AppendLine($"第 {i + 1} 行：「{ar_副檔名[i]}」");
                    } else if (s_invalidFilenameExtensionsInformation.Length == 0) {
                        // 確定沒有含有無效的副檔名，下段陳述式才有執行意義
                        ar_副檔名[i] = ar_副檔名[i].ToLower();
                        if (ar_副檔名[i][0] != '.')
                            ar_副檔名[i] = $".{ar_副檔名[i]}";
                    }
                }
            } catch (ArgumentOutOfRangeException) {
                // ArgumentOutOfRangeException: Enlarging the value of this instance would exceed MaxCapacity.
                // 就算發生，則一定是包含無效的副檔名，結果便是關聯失敗
            }

            // 無效副檔名的資訊，其字串的長度表示了，是否含有無效副檔名
            if (s_invalidFilenameExtensionsInformation.Length != 0) {
                MessageBox.Show($"關聯失敗，無效的副檔名：\n{s_invalidFilenameExtensionsInformation.ToString()}", String.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try {

                /* https://docs.microsoft.com/windows/desktop/shell/fa-intro
                 * https://docs.microsoft.com/windows/desktop/SysInfo/hkey-classes-root-key
                 * The HKEY_CLASSES_ROOT key provides a view of the registry that merges the information from these two sources.
                 * To change the settings for the interactive user, store the changes under HKEY_CURRENT_USER\Software\Classes rather than HKEY_CLASSES_ROOT.
                 */

                // 登錄程式的路徑 ex.:「"D:\Program Files\TiefSee\TiefSee.exe" "%1"」
                using (var reg_appPath = Registry.CurrentUser.CreateSubKey($@"Software\Classes\{s_appName}\shell\open\command")) {
                    reg_appPath.SetValue(String.Empty, $"\"{s_appPath}\" \"%1\"", RegistryValueKind.String);
                }

                // 登錄 TiefSee 支援的檔案類型
                // Doing so enables the application to be listed in the cascade menu of the Open with dialog box.
                using (var reg_appSupportedTypes = Registry.CurrentUser.CreateSubKey($@"Software\Classes\{s_appName}\SupportedTypes")) {
                    reg_appSupportedTypes.SetValue(String.Empty, String.Empty, RegistryValueKind.String);
                    foreach (var item in ar_副檔名)
                        reg_appSupportedTypes.SetValue($"{item}", String.Empty, RegistryValueKind.String);
                }

                // 登錄哪一些副檔名預設使用 TiefSee 開啟
                foreach (var item in ar_副檔名) {
                    using (var reg_filenameExtension = Registry.CurrentUser.CreateSubKey($@"Software\Classes\.{item}")) {
                        reg_filenameExtension.SetValue(String.Empty, s_appName, RegistryValueKind.String);
                    }
                }

                /**
                SupportedTypes.SetValue(".jpeg", "", RegistryValueKind.String);
                SupportedTypes.SetValue(".png", "", RegistryValueKind.String);
                SupportedTypes.SetValue(".gif", "", RegistryValueKind.String);
                SupportedTypes.SetValue(".bmp", "", RegistryValueKind.String);
                SupportedTypes.SetValue(".svg", "", RegistryValueKind.String);
                SupportedTypes.SetValue(".psd", "", RegistryValueKind.String);
                SupportedTypes.SetValue(".tif", "", RegistryValueKind.String);
                SupportedTypes.SetValue(".ico", "", RegistryValueKind.String);
                SupportedTypes.SetValue(".pixiv", "", RegistryValueKind.String);
                SupportedTypes.Close();*/

                MessageBox.Show($"{s_appName} 已經與圖片關聯，{s_appName} 的路徑：\n({s_appPath})", String.Empty, MessageBoxButton.OK, MessageBoxImage.Asterisk);

            } catch (System.UnauthorizedAccessException) {

                MessageBox.Show("權限不足，請『右鍵』→『以系統管理員身份執行』此程式", String.Empty, MessageBoxButton.OK, MessageBoxImage.Error);

            } catch (Exception ee) {

                MessageBox.Show(ee.ToString());

            }

            /**
            RegistryKey reg_appPath = null;
            RegistryKey reg_appSupportedTypes = null;
            try {
                reg_appPath = Registry.CurrentUser.CreateSubKey($@"Software\Classes\{s_appName}\shell\open\command");
                reg_appPath.SetValue(String.Empty, $"\"{s_appPath}\" \"%1\"", RegistryValueKind.String);
                reg_appSupportedTypes = Registry.CurrentUser.CreateSubKey($@"Software\Classes\{s_appName}\SupportedTypes");
                reg_appSupportedTypes.SetValue(String.Empty, String.Empty, RegistryValueKind.String);
                foreach (var item in ar_副檔名)
                    reg_appSupportedTypes.SetValue($".{item}", String.Empty, RegistryValueKind.String);
                foreach (var item in ar_副檔名) {
                    using (var reg_filenameExtension = Registry.CurrentUser.CreateSubKey($@"Software\Classes\.{item}")) {
                        reg_filenameExtension.SetValue(String.Empty, s_appName, RegistryValueKind.String);
                    }
                }
                MessageBox.Show($"{s_appName} 已經與圖片關聯，{s_appName} 的路徑：\n({s_appPath})");
            } catch (System.UnauthorizedAccessException) {
                MessageBox.Show("權限不足，請『右鍵』→『以系統管理員身份執行』此程式", String.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ee) {
                MessageBox.Show(ee.ToString());
            } finally {
                
                if (reg_appPath != null)
                    reg_appPath.Close();
                if (reg_appSupportedTypes != null)
                    reg_appSupportedTypes.Close();
            }*/

        }




        /// <summary>
        /// 解除 附檔名關聯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_DisassociateFilenameExtansion(object sender, RoutedEventArgs e) {


            try {

                //舊的名字
                try {
                    Registry.ClassesRoot.DeleteSubKeyTree("aeropic");
                } catch { }

                Registry.ClassesRoot.DeleteSubKeyTree(s_appName);
                MessageBox.Show("已經解除 TiefSee 與圖片的關聯");

            } catch (System.ArgumentException) {

                MessageBox.Show("已經沒有 TiefSee 的關聯");

            } catch (System.UnauthorizedAccessException) {

                MessageBox.Show("權限不足，請『右鍵』→『以系統管理員身份執行』此程式");

            } catch (Exception ee) {

                MessageBox.Show(ee.ToString());

            }

        }




        /// <summary>
        /// 檢查副檔名是否無效（副檔名的內容可以含有前綴 '.'，也可以不含有，無視大小寫）
        /// </summary>
        /// <param name="s_filenameExtendsion"></param>
        /// <returns>「無效」則傳回 ture</returns>
        private static bool IsInvalidFilenameExtension(string s_filenameExtendsion) {

            // Windows 下檔名的無效字元: '\0', '\u0001'... '\u001f', '\a', '\b', '\t', '\n', '\v', '\f', '\r', '"', '*', '/', ':', '<', '>', '?', '\\', '|'
            var ar_invalidFileNameChars = System.IO.Path.GetInvalidFileNameChars();

            // 不允許檔案類型的副檔名，包含空的副檔名
            Regex pattern_invalidFilenameExtension = new Regex(@"\A\.?(|exe|bat)\z", RegexOptions.IgnoreCase);

            return s_filenameExtendsion == null
                || s_filenameExtendsion.LastIndexOf('.') > 0 //若含有 '.'，則僅限前綴
                || s_filenameExtendsion.IndexOfAny(ar_invalidFileNameChars) > -1
                || pattern_invalidFilenameExtension.IsMatch(s_filenameExtendsion);

        }




    }
}
