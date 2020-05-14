using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TiefSee.W;
using System.Windows.Media;
using ColorPicker;
using System.Windows;
using System.Windows.Controls;
using static TiefSee.MainWindow;

namespace TiefSee.cs {
    public class C_setting {


        MainWindow M;
        public String path_position = "";//記錄程式坐標
        public String path_XML_setting = "";//記錄設定值
        private String path_config = "../config.xml";//進階設定值（例如瀏覽器使用順序
        public String s_color_背景顏色 = "180,0,0,0";
        public String s_color_標題列顏色 = "180,0,0,0";
        public String s_color_外框顏色 = "180,0,0,0";



        /// <summary>
        /// 儲存時，將color轉成String
        /// </summary>
        public String fun_colorToString(Color co) {
            return co.A + "," + co.R + "," + co.G + "," + co.B;
        }


        /// <summary>
        /// 讀取時，設定顏色
        /// </summary>
        public Color fun_getColor(String color_顏色) {

            Color c = new Color();

            try {
                Byte[] x = new Byte[4];
                String[] s = color_顏色.Split(',');
                for (int i = 0; i < 4; i++) {
                    x[i] = Byte.Parse(s[i]);
                }

                c.A = x[0];
                c.R = x[1];
                c.G = x[2];
                c.B = x[3];


            } catch { }

            return c;
        }



        /// <summary>
        /// 從String轉成color
        /// </summary>
        public Byte[] fun_getColorByte(String s_col) {

            Byte[] x = new Byte[4];
            try {
                x = new Byte[4];
                String[] s = s_col.Split(',');
                for (int i = 0; i < 4; i++) {
                    x[i] = Byte.Parse(s[i]);
                }
            } catch (Exception) {
                //color_顏色 = "180,0,0,0";
                return new Byte[] { 0, 0, 0, 0 };
            }
            return x;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        public C_setting(MainWindow m) {
            this.M = m;
            path_position = M.fun_執行檔路徑() + "/" + path_position;

            path_position = M.fun_執行檔路徑() + "/data/position.txt";
            path_XML_setting = M.fun_執行檔路徑() + "/data/setting.xml";

            //判斷config.xml檔案是否存在
            path_config = M.fun_執行檔路徑() + "/data/config.xml";
            if (File.Exists(path_config) == false) {
                System.Windows.Forms.MessageBox.Show("找不到 data/config.xml");

            }
            fun_config();
        }













        #region position(程式坐標)



        /// <summary>
        /// 
        /// </summary>
        public void fun_儲存_position(Boolean bool_偏移) {

            int int_偏移量 = 30;

            if (bool_偏移 == false) {
                int_偏移量 = 0;
            }

            using (FileStream fs = new FileStream(path_position, FileMode.Create)) {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8)) {
                    sw.WriteLine("width: " + M.Width);
                    sw.WriteLine("height: " + M.Height);
                    sw.WriteLine("left: " + (M.Left + int_偏移量));
                    sw.WriteLine("top: " + (M.Top + int_偏移量));
                    sw.WriteLine("max: " + (M.WindowState == System.Windows.WindowState.Maximized));
                }
            }


        }



        /// <summary>
        /// 
        /// </summary>
        public void fun_讀取_position() {

            double width = 300;
            double height = 300;
            double left = 50;
            double top = 50;
            Boolean max = false;


            if (File.Exists(path_position))
                using (StreamReader sr = new StreamReader(path_position, Encoding.UTF8)) {
                    String line;
                    while ((line = sr.ReadLine()) != null) {
                        line = line.Trim().Replace(" ", "").Replace(":", "");

                        if (line.Contains("width")) {
                            width = double.Parse(line.Replace("width", ""));
                        } else if (line.Contains("height")) {
                            height = double.Parse(line.Replace("height", ""));
                        } else if (line.Contains("left")) {
                            left = double.Parse(line.Replace("left", ""));
                        } else if (line.Contains("top")) {
                            top = double.Parse(line.Replace("top", ""));
                        } else if (line.Contains("max")) {
                            max = line.Replace("max", "").ToUpper().Equals("TRUE");
                        }
                    }//while
                }//using

            if (max) {
                M.WindowState = System.Windows.WindowState.Maximized;
                M.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width * 0.6;
                M.Height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height * 0.6;
            } else {
                M.Width = width;
                M.Height = height;
            }

            var size = System.Windows.Forms.Cursor.Clip.Size;



            int int_最左邊 = 0;
            int int_最上面 = 0;
            int int_最右邊 = 0;
            int int_最下面 = 0;


            //計算多螢幕的邊界（有些多螢幕的坐標會有負值）
            foreach (System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens) {//列出所有螢幕資訊
                int xx = screen.Bounds.X;
                if (xx < int_最左邊)
                    int_最左邊 = xx;

                int yy = screen.Bounds.Y;
                if (yy < int_最上面)
                    int_最上面 = yy;

                int xx2 = screen.Bounds.X + (int)(screen.Bounds.Width / M.d_解析度比例_x);
                if (xx2 > int_最右邊)
                    int_最右邊 = xx2;

                int yy2 = screen.Bounds.Y + (int)(screen.Bounds.Height / M.d_解析度比例_y);
                if (yy2 > int_最下面)
                    int_最下面 = yy2;
            }




            if (left < int_最左邊) {
                M.Left = int_最左邊;
            } else if (M.Width + left > int_最右邊) {
                M.Left = int_最右邊 - M.Width;
            } else {
                M.Left = left;
            }


            if (top < int_最上面) {
                M.Top = int_最上面;
            } else if (M.Height + top > int_最下面) {
                M.Top = int_最下面 - M.Height;
            } else {
                M.Top = top;
            }


            //MessageBox.Show(M.d_解析度比例_y+"");

        }

        #endregion


        public bool bool_aero = true;
        public bool bool_換頁按鈕_下 = true;

        public bool bool_工具列_換頁按鈕 = false;
        public bool bool_工具列_換資料夾 = false;
        public bool bool_工具列_排序 = true;
        public bool bool_工具列_外部程式開啟 = false;
        public bool bool_工具列_大量瀏覽模式 = true;
        public bool bool_工具列_搜圖 = true;
        public bool bool_工具列_旋轉 = true;
        public bool bool_工具列_檢視縮放比例 = true;
        public bool bool_工具列_放大縮小 = false;
        public bool bool_工具列_縮放至視窗大小 = true;
        public bool bool_工具列_複製 = true;
        public bool bool_工具列_快速拖曳 = false;
        public bool bool_工具列_刪除圖片 = false;






        /// <summary>
        /// 
        /// </summary>
        /// <param name="ss"></param>
        /// <returns></returns>
        private bool get_bool(String ss) {
            ss = ss.Trim().ToUpper();
            if (ss == "TRUE" || ss == "1") {
                return true;
            }
            return false;
        }





        #region setting


        /// <summary>
        /// 
        /// </summary>
        public void fun_讀取setting() {

            try {

                XmlDocument XmlDoc = new XmlDocument();
                XmlDoc.Load(path_XML_setting);
                XmlNodeList NodeLists = XmlDoc.SelectNodes("settings/item");

                foreach (XmlNode item in NodeLists) {

                    if (item.Attributes["name"].Value == "bool_aero") {//
                        bool_aero = get_bool(item.InnerText);
                    } else if (item.Attributes["name"].Value == "bool_換頁按鈕_下") {//
                        bool_換頁按鈕_下 = get_bool(item.InnerText);
                    } else if (item.Attributes["name"].Value == "bool_工具列_換頁按鈕") {//
                        bool_工具列_換頁按鈕 = get_bool(item.InnerText);
                    } else if (item.Attributes["name"].Value == "bool_工具列_外部程式開啟") {//
                        bool_工具列_外部程式開啟 = get_bool(item.InnerText);
                    } else if (item.Attributes["name"].Value == "bool_工具列_大量瀏覽模式") {//
                        bool_工具列_大量瀏覽模式 = get_bool(item.InnerText);
                    } else if (item.Attributes["name"].Value == "bool_工具列_搜圖") {//
                        bool_工具列_搜圖 = get_bool(item.InnerText);
                    } else if (item.Attributes["name"].Value == "bool_工具列_旋轉") {//
                        bool_工具列_旋轉 = get_bool(item.InnerText);
                    } else if (item.Attributes["name"].Value == "bool_工具列_放大縮小") {//
                        bool_工具列_放大縮小 = get_bool(item.InnerText);
                    } else if (item.Attributes["name"].Value == "bool_工具列_檢視縮放比例") {//
                        bool_工具列_檢視縮放比例 = get_bool(item.InnerText);
                    } else if (item.Attributes["name"].Value == "bool_工具列_縮放至視窗大小") {//
                        bool_工具列_縮放至視窗大小 = get_bool(item.InnerText);
                    } else if (item.Attributes["name"].Value == "bool_工具列_複製") {//
                        bool_工具列_複製 = get_bool(item.InnerText);

                    } else if (item.Attributes["name"].Value == "bool_工具列_換資料夾") {//
                        bool_工具列_換資料夾 = get_bool(item.InnerText);
                    } else if (item.Attributes["name"].Value == "bool_工具列_排序") {//
                        bool_工具列_排序 = get_bool(item.InnerText);
                    } else if (item.Attributes["name"].Value == "bool_工具列_快速拖曳") {//
                        bool_工具列_快速拖曳 = get_bool(item.InnerText);
                    } else if (item.Attributes["name"].Value == "bool_工具列_刪除圖片") {//
                        bool_工具列_刪除圖片 = get_bool(item.InnerText);


                    } else if (item.Attributes["name"].Value == "bool_顯示工具列") {//
                        M.bool_顯示工具列 = get_bool(item.InnerText);
                    } else if (item.Attributes["name"].Value == "bool_顯示exif視窗") {//
                        M.bool_顯示exif視窗 = get_bool(item.InnerText);
                    } else


                   if (item.Attributes["name"].Value == "color_背景顏色") {//
                        s_color_背景顏色 = (item.InnerText);
                    }

                    if (item.Attributes["name"].Value == "color_標題列顏色") {//
                        s_color_標題列顏色 = (item.InnerText);
                    }
                    if (item.Attributes["name"].Value == "color_外框顏色") {//
                        s_color_外框顏色 = (item.InnerText);
                    }


                    if (item.Attributes["name"].Value == "radio_高品質成像") {//
                        M.int_高品質成像模式 = Int32.Parse(item.InnerText);
                        if (M.int_高品質成像模式 == 4) {
                            ImageMagick.OpenCL.IsEnabled = false;//硬體加速
                        } else {
                            ImageMagick.OpenCL.IsEnabled = true;//硬體加速
                        }
                    }


                    if (item.Attributes["name"].Value == "e_gif_type") {//
                        try {
                            M.e_GIF_Type = (E_GIF_type)(Int32.Parse(item.InnerText));
                        } catch { }
                    }
         
                    if (item.Attributes["name"].Value == "_bool_快速預覽_滑鼠滾輪") {
                        MainWindow._bool_快速預覽_滑鼠滾輪 = get_bool(item.InnerText);
                    }
                    if (item.Attributes["name"].Value == "_bool_快速預覽_空白鍵") {
                        MainWindow._bool_快速預覽_空白鍵 = get_bool(item.InnerText);
                    }
                    if (item.Attributes["name"].Value == "_bool_快速啟動") {
                        MainWindow._bool_快速啟動 = get_bool(item.InnerText);
                    }


                    if (item.Attributes["name"].Value == "_e_滾輪用途") {//
                        try {
                            M._e_滾輪用途 = (e_滾輪用途)(Int32.Parse(item.InnerText));
                        } catch { }
                    }
                }


            } catch { }




        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="w_設定"></param>
        public void fun_儲存setting() {


            var fun_儲存 = new Action<XmlTextWriter, String, String>((XmlTextWriter XTW, String key, String value) => {
                XTW.WriteStartElement("item");
                XTW.WriteAttributeString("name", key);
                XTW.WriteString(value);
                XTW.WriteEndElement();
            });

            XmlTextWriter X = new XmlTextWriter(path_XML_setting, Encoding.UTF8);


            X.WriteStartDocument();//使用1.0版本
            X.Formatting = Formatting.Indented;//自動縮排
            X.Indentation = 2;//縮排距離

            X.WriteStartElement("settings");

            //-------------------


            /*
            fun_儲存(X, "bool_aero", w_設定.check_aero.IsChecked.Value.ToString());

            fun_儲存(X, "bool_工具列_換頁按鈕", w_設定.check_工具列_換頁按鈕.IsChecked.Value.ToString());
            fun_儲存(X, "bool_換頁按鈕_下", w_設定.check_換頁按鈕_下.IsChecked.Value.ToString());
            fun_儲存(X, "bool_工具列_外部程式開啟", w_設定.check_工具列_外部程式開啟.IsChecked.Value.ToString());
            fun_儲存(X, "bool_工具列_大量瀏覽模式", w_設定.check_工具列_大量瀏覽模式.IsChecked.Value.ToString());
            fun_儲存(X, "bool_工具列_搜圖", w_設定.check_工具列_搜圖.IsChecked.Value.ToString());
            fun_儲存(X, "bool_工具列_旋轉", w_設定.check_工具列_旋轉.IsChecked.Value.ToString());
            fun_儲存(X, "bool_工具列_放大縮小", w_設定.check_工具列_放大縮小.IsChecked.Value.ToString());
            fun_儲存(X, "bool_工具列_檢視縮放比例", w_設定.check_工具列_檢視縮放比例.IsChecked.Value.ToString());
            fun_儲存(X, "bool_工具列_縮放至視窗大小", w_設定.check_工具列_縮放至視窗大小.IsChecked.Value.ToString());
            fun_儲存(X, "bool_工具列_複製", w_設定.check_工具列_複製.IsChecked.Value.ToString());*/

            fun_儲存(X, "bool_aero", bool_aero.ToString());

            fun_儲存(X, "bool_工具列_換頁按鈕", bool_工具列_換頁按鈕.ToString());
            fun_儲存(X, "bool_換頁按鈕_下", bool_換頁按鈕_下.ToString());
            fun_儲存(X, "bool_工具列_外部程式開啟", bool_工具列_外部程式開啟.ToString());
            fun_儲存(X, "bool_工具列_大量瀏覽模式", bool_工具列_大量瀏覽模式.ToString());
            fun_儲存(X, "bool_工具列_搜圖", bool_工具列_搜圖.ToString());
            fun_儲存(X, "bool_工具列_旋轉", bool_工具列_旋轉.ToString());
            fun_儲存(X, "bool_工具列_放大縮小", bool_工具列_放大縮小.ToString());
            fun_儲存(X, "bool_工具列_檢視縮放比例", bool_工具列_檢視縮放比例.ToString());
            fun_儲存(X, "bool_工具列_縮放至視窗大小", bool_工具列_縮放至視窗大小.ToString());
            fun_儲存(X, "bool_工具列_複製", bool_工具列_複製.ToString());

            fun_儲存(X, "bool_工具列_換資料夾", bool_工具列_換資料夾.ToString());
            fun_儲存(X, "bool_工具列_排序", bool_工具列_排序.ToString());
            fun_儲存(X, "bool_工具列_快速拖曳", bool_工具列_快速拖曳.ToString());
            fun_儲存(X, "bool_工具列_刪除圖片", bool_工具列_刪除圖片.ToString());


            fun_儲存(X, "bool_顯示工具列", M.bool_顯示工具列.ToString());
            fun_儲存(X, "bool_顯示exif視窗", M.bool_顯示exif視窗.ToString());

            fun_儲存(X, "radio_高品質成像", M.int_高品質成像模式 + "");

            //fun_儲存(X, "bool_psd使用內建解碼器", w_設定.check_psd用內建解碼器.IsChecked.Value.ToString());

            fun_儲存(X, "color_背景顏色", s_color_背景顏色);
            fun_儲存(X, "color_標題列顏色", s_color_標題列顏色);
            fun_儲存(X, "color_外框顏色", s_color_外框顏色);

            try {
                fun_儲存(X, "e_gif_type", ((int)M.e_GIF_Type) + "");
            } catch { }


            fun_儲存(X, "_bool_快速預覽_滑鼠滾輪", MainWindow._bool_快速預覽_滑鼠滾輪.ToString());
            fun_儲存(X, "_bool_快速預覽_空白鍵", MainWindow._bool_快速預覽_空白鍵.ToString());
            fun_儲存(X, "_bool_快速啟動", MainWindow._bool_快速啟動.ToString());

            try {
                fun_儲存(X, "_e_滾輪用途", ((int)M._e_滾輪用途) + "");
            } catch { }

            //-------------------

            X.WriteEndElement();

            X.Flush();     //寫這行才會寫入檔案
            X.Close();
            X.Dispose();
        }




        /// <summary>
        /// 
        /// </summary>
        public void fun_套用setting設定() {



            Action<FrameworkElement, bool> ac_顯示或隱藏 = (FrameworkElement obj, bool bool_顯示) => {
                if (bool_顯示) {
                    obj.Visibility = Visibility.Visible;
                } else {
                    obj.Visibility = Visibility.Collapsed;
                }
            };



            if (M.w_web != null) {//如果還沒初始化就不執行
                if (bool_換頁按鈕_下) {
                    try {
                        M.img_web.Document.InvokeScript("fun_顯示或隱藏換頁按鈕", new Object[] { "t" });
                    } catch { }
                } else {
                    try {
                        M.img_web.Document.InvokeScript("fun_顯示或隱藏換頁按鈕", new Object[] { "f" });
                    } catch { }
                }
                var ccc = fun_getColorByte(s_color_背景顏色);
                try {
                    M.img_web.Document.InvokeScript("set_bac_color", new Object[] { $"rgb({ccc[1]},{ccc[2]},{ccc[3]})" });
                } catch { }
            }


            if (M.u_大量瀏覽模式 != null) {//如果還沒初始化就不執行
                var ccc = fun_getColorByte(s_color_背景顏色);
                try {
                    M.u_大量瀏覽模式.web01.Document.InvokeScript("set_bac_color", new Object[] { $"rgb({ccc[1]},{ccc[2]},{ccc[3]})" });
                } catch { }
            }


            ac_顯示或隱藏(M.grid_換頁按鈕_下, bool_換頁按鈕_下);
            ac_顯示或隱藏(M.but_上一張, bool_工具列_換頁按鈕);
            ac_顯示或隱藏(M.but_下一張, bool_工具列_換頁按鈕);
            ac_顯示或隱藏(M.but_圖片放大, bool_工具列_放大縮小);
            ac_顯示或隱藏(M.but_圖片縮小, bool_工具列_放大縮小);
            ac_顯示或隱藏(M.but_用外部程式開啟, bool_工具列_外部程式開啟);
            ac_顯示或隱藏(M.but_進入大量瀏覽, bool_工具列_大量瀏覽模式);
            ac_顯示或隱藏(M.but_搜圖, bool_工具列_搜圖);
            ac_顯示或隱藏(M.but_旋轉, bool_工具列_旋轉);
            ac_顯示或隱藏(M.but_檢視原始大小, bool_工具列_檢視縮放比例);
            ac_顯示或隱藏(M.but_圖片全滿, bool_工具列_縮放至視窗大小);

            ac_顯示或隱藏(M.but_複製, bool_工具列_複製);

            ac_顯示或隱藏(M.but_上一資料夾, bool_工具列_換資料夾);
            ac_顯示或隱藏(M.but_下一資料夾, bool_工具列_換資料夾);
            ac_顯示或隱藏(M.but_排序, bool_工具列_排序);
            ac_顯示或隱藏(M.but_拖出檔案, bool_工具列_快速拖曳);
            ac_顯示或隱藏(M.but_刪除圖片, bool_工具列_刪除圖片);




            //func_顯示或隱藏垂直線();


            var cc = fun_getColor(s_color_背景顏色);
            M.bac.Fill = new SolidColorBrush(cc);//背景顏色
            M.fun_顯示背景顏色();

            var cc2 = fun_getColor(s_color_標題列顏色);

           
                M.bac_標題列.Fill = new SolidColorBrush(cc2);
            

     

            var cc3 = fun_getColor(s_color_外框顏色);
            M.border_視窗外框.BorderBrush = new SolidColorBrush(cc3);


        }






        #endregion




        #region config

        public List<String> ar_預設瀏覽器 = new List<string>();
        public List<data_開啟程式> ar_外部開啟 = new List<data_開啟程式>();




        /// <summary>
        /// 
        /// </summary>
        /// <param name="s_view"></param>
        /// <param name="bool_轉碼"></param>
        public void fun_開啟網址(String s_view, bool bool_轉碼) {
            //https://www.w3schools.com/tags/ref_urlencode.asp



            if (bool_轉碼)
                s_view = s_view
                    //.Replace("%", "%25")
                    .Replace(" ", "%20")
                    .Replace("\"", "%22")
                    .Replace("#", "%23")
                    .Replace("'", "%27")
                    //.Replace(",", "%2C")
                    //.Replace(";", "%3B")
                    //.Replace("`", "%60")
                    //.Replace("&", "%26")
                    //.Replace(":", "%3A")
                    //.Replace("?", "%3F")
                    //.Replace("-", "%2D")
                    //.Replace("$", "%24")
                    //.Replace("=", "%3D")            
                    ;//避免命令列參數被切割

            //



            if (s_view.Substring(0, 4).ToUpper().Equals("HTTP") == false) {
                //MessageBox.Show(s_view.Substring(0, 4).ToUpper());
                s_view = "file:///" + s_view;
            }
            s_view = "\"" + s_view + "\"";


            foreach (var item in ar_預設瀏覽器) {
                try {
                    if (System.IO.File.Exists(item)) {//檔案存在
                        System.Diagnostics.Process.Start(item, s_view);
                        return;
                    }
                } catch { }
            }

            try {
                System.Diagnostics.Process.Start(s_view);
            } catch { }


        }




        /// <summary>
        /// 讀取系統的 config 進階設定檔
        /// </summary>
        public void fun_config() {

            // try {


            //先讀取文字檔
            StreamReader sr = new StreamReader(path_config, Encoding.UTF8);
            String s_xml = sr.ReadToEnd();

            sr.Close();
            sr.Dispose();

            //因為網址裡面可能有【&】，所以要先取代成【&amp;】
            s_xml = s_xml.Replace("&amp;", "&");
            s_xml = s_xml.Replace("&lt;", "*+<+*");
            s_xml = s_xml.Replace("&gt;", "*+>+*");
            s_xml = s_xml.Replace("&", "&amp;");
            s_xml = s_xml.Replace("*+<+*", "&lt;");
            s_xml = s_xml.Replace("*+>+*", "&gt;");


            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.LoadXml(s_xml);//從字串解析xml
            XmlNodeList NodeLists = XmlDoc.SelectNodes("settings/item");

            foreach (XmlNode item in NodeLists) {

                if (item.Attributes["name"].Value.Equals("預設瀏覽器")) {//

                    foreach (XmlNode item2 in item.ChildNodes) {
                        ar_預設瀏覽器.Add(item2.InnerText.Trim().Replace("\"", ""));
                    }

                } else if (item.Attributes["name"].Value.Equals("使用其他程式開啟")) {

                    foreach (XmlNode item2 in item.ChildNodes) {
                        if (item2.Name != "path")
                            continue;

                        var cls = new data_開啟程式();
                        cls.type = item2.Attributes["type"].Value.Trim();
                        cls.path = item2.InnerText.Trim().Replace("\"", "").Replace("/", "\\");
                        if (item2.Attributes["name"] != null) {
                            cls.name = item2.Attributes["name"].Value.Trim();
                        }
                        cls.lnk_name = cls.path;

                        ar_外部開啟.Add(cls);
                    }

                }

            }
            // } catch { }





        }

        #endregion



    }



    public class data_開啟程式 {

        public String path = "";
        public String name = "";
        public String type = "";
        public String lnk_name = "";
        public System.Drawing.Bitmap img = null;

        public data_開啟程式 get_copy() {
            return (data_開啟程式)this.MemberwiseClone();
        }
    }






}
