using TiefSee.W;
using ExifLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TiefSee {

    public class C_Exif {

        public int int_Orientation = 0;//圖片旋轉值

        private Func<string, string, data_exif>[] ar_fun;
        private MainWindow M;
        private List<data_exif> ar_exif = new List<data_exif>();
        private bool bool_排除預設值 = true;
        int int_界面寬度 = 250;

        private String GPSLatitudeRef = "";//判斷南半球或北半球
        private String GPSLongitudeRef = "";//判斷東西半球


        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        public C_Exif(MainWindow m) {

            this.M = m;

            //要解析的exit項目
            ar_fun = new Func<string, string, data_exif>[] {
                        f_Make,
                        f_Model,
                        f_LensModel,
                        f_Orientation,
                        f_PhotographicSensitivity,
                        //f_ResolutionUnit,//不重要
                        f_Software,
                        f_ExposureTime,
                        f_FNumber,
                        f_ExposureProgram,
                        //f_ExifVersion,//不重要
                        f_BrightnessValue,
                        f_ExposureBiasValue,
                        f_MaxApertureValue,
                        f_MeteringMode,
                        f_Flash,
                        f_FocalLength,
                        f_ExposureMode,
                        f_WhiteBalance,
                        f_SceneCaptureType,
                        f_ImageUniqueID,
                        f_ShutterSpeedValue,
                        f_ApertureValue,
                        f_SensingMethod,
                        f_CustomRendered,
                        f_FocalLengthIn35mmFilm,
                        f_DateTimeOriginal,
                        f_LightSource,
                        f_YCbCrPositioning,
                        f_DigitalZoomRatio,
                        f_SubjectDistanceRange,

                        f_GPSLatitudeRef,
                        f_GPSLatitude,
                        f_GPSLongitudeRef,
                        f_GPSLongitude,
                        f_GPSAltitude,

                };




            M.but_exif_googleMap.Click += (sendemr, e) => {
                try {
                    String url = "http://maps.google.com.tw/?q=" + s_緯度 + "," + s_經度;
                    M.c_set.fun_開啟網址(url, false);
                } catch {
                }
            };

            M.but_exif_複製.Click += (sendemr, e) => {



                StringBuilder sb = new StringBuilder();
                try {
                    for (int i = 0; i < ar_exif.Count; i++) {
                        var exif = ar_exif[i];
                        var u = new U_exif_item();
                        sb.Append(exif.type_ch + "(" + exif.type_en + ") : " + exif.value_ch + "\n");
                    }

                    System.Windows.Forms.Clipboard.SetDataObject(sb.ToString(), false, 5, 200);
                } catch { }

            };


        }





        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool fun_is_exif_type() {
            String ss = M.s_img_type_附檔名;
            if (ss == "JPG" || ss == "JPEG" || ss == "TIF" || ss == "TIFF") {
                return true;
            }
            return false;
        }



        /// <summary>
        /// 
        /// </summary>
        public void fun_顯示或隱藏視窗() {


            fun_清除界面();
            M.stackPanel_exif_box.ScrollToTop();


            //判斷是否顯示右上角的exif按鈕
            /*if (fun_is_exif_type()) {
                M.but_exif.Visibility = System.Windows.Visibility.Visible;
            } else {
                M.but_exif.Visibility = System.Windows.Visibility.Collapsed;
            }*/


            //判斷是否顯示exif視窗
            if (fun_is_exif_type() && M.bool_顯示exif視窗) {
                M.stackPanel_exif_box.Visibility = System.Windows.Visibility.Visible;
                fun_產生界面();
            } else {
                M.stackPanel_exif_box.Visibility = System.Windows.Visibility.Collapsed;
            }



        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagValue"></param>
        /// <returns></returns>
        private static string RenderTag(object tagValue) {
            // Arrays don't render well without assistance.
            var array = tagValue as Array;
            if (array != null) {
                // Hex rendering for really big byte arrays (ugly otherwise)
                if (array.Length > 20 && array.GetType().GetElementType() == typeof(byte))
                    return "0x" + string.Join("", array.Cast<byte>().Select(x => x.ToString("X2")).ToArray());

                return string.Join(", ", array.Cast<object>().Select(x => x.ToString()).ToArray());
            }

            return tagValue.ToString();
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public void fun_取得_exif(MemoryStream path) {

            //DateTime time_start = DateTime.Now;//計時開始 取得目前時間


            //String path = @"C:\Users\hbl91\Desktop\特殊圖片\drive-download-20170813T100101Z-001\20130222_111555.jpg";
            /*using (FileStream logFileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                using (MagickImage image = new MagickImage(logFileStream)) {

                    ExifProfile profile = image.GetExifProfile();

                    String sum = "";
                    foreach (ExifValue item in profile.Values) {
                        sum += item.Tag + "\t\t" + item.Value + "\n";
                    }
                    t2.Text = sum;
                }
            }*/



            ExifReader reader = new ExifReader(path);
            // Extract the tag data using the ExifTags enumeration
            String[] props = Enum.GetValues(typeof(ExifTags)).Cast<ushort>().Select(tagID => {
                object val;
                if (reader.GetTagValue(tagID, out val)) {
                    // Special case - some doubles are encoded as TIFF rationals. These
                    // items can be retrieved as 2 element arrays of {numerator, denominator}
                    if (val is double) {
                        int[] rational;
                        if (reader.GetTagValue(tagID, out rational))
                            val = string.Format("{0} ({1}/{2})", val, rational[0], rational[1]);
                    }

                    String type = Enum.GetName(typeof(ExifTags), tagID);
                    return string.Format("{0}：{1}", type, RenderTag(val));
                }

                return null;

            }).Where(x => x != null).ToArray();

            //string model;
            //reader.GetTagValue(ExifTags.Model, out model);

            //t2.Text = string.Join("\r\n", props);



            //StringBuilder sb = new StringBuilder();
            List<String> ar_避免重複 = new List<string>();
            List<String> ar_未定義 = new List<string>();


            fun_初始化exif資訊();

            for (int i = 0; i < props.Length; i++) {

                //避免重複  
                if (ar_避免重複.Contains(props[i])) {
                    continue;
                } else {
                    ar_避免重複.Add(props[i]);
                }

                bool book_未定義 = true;

                for (int j = 0; j < ar_fun.Length; j++) {
                    String[] props_s = props[i].Split('：');
                    var exif = ar_fun[j](props_s[0].Trim(), props_s[1].Trim());
                    if (exif != null) {
                        ar_exif.Add(exif);
                        book_未定義 = false;
                        break;
                    }
                }//for

                if (book_未定義)
                    ar_未定義.Add(props[i]);

            }//for


            /*
            StringBuilder sb = new StringBuilder();
            sb.Append("\n\n\n\n\n\n\n\n" + "\n\n--------------\n\n" + "\n\n\n\n\n\n\n\n");

            foreach (var item in ar_未定義) {
                sb.Append(item + "\n");
            }


            System.Console.Write(sb.ToString());
            */



            /*ExifFile file = ExifFile.Read(path);
            // Read metadata
             foreach (ExifProperty item in file.Properties.Values) {
                 // Do something with meta data
             }*/


            /*
            DateTime time_end = DateTime.Now;//計時結束 取得目前時間            
            string result2 = ((TimeSpan)(time_end - time_start)).TotalMilliseconds.ToString();//後面的時間減前面的時間後 轉型成TimeSpan即可印出時間差
            t1.Text = result2 + " 毫秒";
            */


        }



        /// <summary>
        /// 
        /// </summary>
        private void fun_清除界面() {
            int len = M.stackPanel_exif.Children.Count;
            for (int i = 0; i < len; i++) {
                M.stackPanel_exif.Children.Remove(M.stackPanel_exif.Children[0]);
            }

        }




        /// <summary>
        /// 
        /// </summary>
        private void fun_產生界面() {

            //判斷是否需要顯示『Google Map』的按鈕
            if (s_經度 != "" && s_緯度 != "") {
                M.but_exif_googleMap.Visibility = Visibility.Visible;
            } else {
                M.but_exif_googleMap.Visibility = Visibility.Collapsed;
            }


            //如果項目是空的，就不要顯示
            if (ar_exif.Count == 0) {
                M.stackPanel_exif_box.Width = 0;
                return;
            }
            M.stackPanel_exif_box.Width = int_界面寬度;

            //判斷是否使用淺色主題
            bool bool_淺色主題 = false;
            var col = M.c_set.fun_getColor(M.c_set.s_color_背景顏色);
            int iii = 0;
            if (col.R > 180) {
                iii++;
            }
            if (col.G > 180) {
                iii++;
            }
            if (col.B > 180) {
                iii++;
            }
            if (iii >= 2) {
                bool_淺色主題 = true;
            }


            for (int i = 0; i < ar_exif.Count; i++) {
                var exif = ar_exif[i];
                var u = new U_exif_item();
                u.text_type.Text = exif.type_ch;
                u.text_value.Text = exif.value_ch;
                u.fun_setToolTip(exif.type_ch + "\n(" + exif.type_en + ")");

                if (bool_淺色主題) {
                    u.fun_淺色主題();
                    M.icon_map.Foreground =new SolidColorBrush( Color.FromRgb(0,0,0));
                    M.icon_copy.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                } else {
                    M.icon_map.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    M.icon_copy.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                }
                M.stackPanel_exif.Children.Add(u);

                if (i == ar_exif.Count - 1) {
                    u.fun_不顯示分割線();
                }

            }


            //在下面產生一個空白高度
            Border bor = new Border();
            bor.Height = 110;
            M.stackPanel_exif.Children.Add(bor);

        }



        /// <summary>
        /// 
        /// </summary>
        public void fun_初始化exif資訊() {
            ar_exif = new List<data_exif>();
            int_Orientation = 0;
            GPSLatitudeRef = "";
            GPSLongitudeRef = "";
            s_經度 = "";
            s_緯度 = "";
        }








        #region 解析




        /// <summary>
        /// 相機廠商
        /// </summary>
        private data_exif f_Make(String type, String value) {
            if (type.ToUpper() == "Make".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "Make";
                exif.type_ch = "相機廠商";
                exif.value_en = value;
                return exif;
            } else {
                return null;
            }
        }



        /// <summary>
        /// 相機型號
        /// </summary>
        private data_exif f_Model(String type, String value) {
            if (type.ToUpper() == "Model".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "Model";
                exif.type_ch = "相機型號";
                exif.value_en = value;
                return exif;
            } else {
                return null;
            }
        }



        /// <summary>
        /// 鏡頭模型
        /// </summary>
        private data_exif f_LensModel(String type, String value) {
            if (type.ToUpper() == "LensModel".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "LensModel";
                exif.type_ch = "鏡頭模型";
                exif.value_en = value;
                return exif;
            } else {
                return null;
            }
        }





        /// <summary>
        /// 旋轉資訊
        /// </summary>
        private data_exif f_Orientation(String type, String value) {
            //http://janochen.blogspot.tw/2008/09/exif-orientation.html

            if (type.ToUpper() == "Orientation".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "Orientation";
                exif.type_ch = "方向";

                int_Orientation = 0;
           
                try {
                    int_Orientation = Int32.Parse(value);
                } catch {               
                    return null;
                }
                if (int_Orientation > 8 || int_Orientation <= 0) {
                    int_Orientation = 0;
                    return null;
                }


                if (value == "1") {//正常
                    exif.value_en = "0°";

                } else if (value == "2") {//水平
                    exif.value_en = "Horizontal";

                } else if (value == "3") {//180
                    exif.value_en = "180°";

                } else if (value == "4") {//垂直鏡像
                    exif.value_en = "Vertical";

                } else if (value == "5") {//垂直鏡像 + 90
                    exif.value_en = "Vertical + 90°";

                } else if (value == "6") {//270
                    exif.value_en = "270°";

                } else if (value == "7") {//水平鏡像+90
                    exif.value_en = "Horizontal + 90°";

                } else if (value == "8") {//90
                    exif.value_en = "90°";

                }
                return exif;
            } else {
                return null;
            }
        }


        /// <summary>
        /// 感光度
        /// </summary>
        private data_exif f_PhotographicSensitivity(String type, String value) {
            if (type.ToUpper() == "PhotographicSensitivity".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "PhotographicSensitivity";
                exif.type_ch = "感光度";

                try {
                    exif.value_en = float.Parse(value).ToString("0.0").Replace(".0", "");
                } catch (Exception) {
                    exif.value_en = value;
                }


                return exif;
            } else {
                return null;
            }
        }






        /// <summary>
        /// 解析度單位
        /// </summary>
        private data_exif f_ResolutionUnit(String type, String value) {
            if (type.ToUpper() == "ResolutionUnit".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "ResolutionUnit";
                exif.type_ch = "解析度單位";

                if (value == "1") {//
                    exif.value_en = "null";

                } else if (value == "2") {//
                    exif.value_en = "inch";
                    exif.value_ch = "inch (英吋)";
                } else if (value == "3") {//
                    exif.value_en = "cm";
                    exif.value_ch = "cm (公分)";
                } else {//
                    exif.value_en = "inch";
                    exif.value_ch = "inch (英吋)";
                }

                if (bool_排除預設值)
                    if (value == "2" || value == "") {
                        return null;
                    }

                return exif;
            } else {
                return null;
            }
        }


        /// <summary>
        /// 軟體
        /// </summary>
        private data_exif f_Software(String type, String value) {
            if (type.ToUpper() == "Software".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "Software";
                exif.type_ch = "軟體";
                exif.value_en = value;
                return exif;
            } else {
                return null;
            }
        }


        /// <summary>
        /// 拍攝時間
        /// </summary>
        private data_exif f_DateTime(String type, String value) {
            if (type.ToUpper() == "DateTime".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "DateTime";
                exif.type_ch = "拍攝時間";
                exif.value_en = value;
                return exif;
            } else {
                return null;
            }
        }



        /// <summary>
        /// 曝光時間
        /// </summary>
        private data_exif f_ExposureTime(String type, String value) {
            if (type.ToUpper() == "ExposureTime".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "ExposureTime";
                exif.type_ch = "曝光時間";

                try {
                    float ff = float.Parse(value.Split(' ')[0]);
                    exif.value_en = ff.ToString("0.000000") + "s " + value.Split(' ')[1];
                } catch {
                    exif.value_en = value;
                }

                return exif;
            } else {
                return null;
            }
        }


        /// <summary>
        /// 有效F值
        /// </summary>
        private data_exif f_FNumber(String type, String value) {
            if (type.ToUpper() == "FNumber".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "FNumber";
                exif.type_ch = "有效F值";
                exif.value_en = "f/" + value;
                return exif;
            } else {
                return null;
            }
        }


        /// <summary>
        /// 曝光軟體
        /// </summary>
        private data_exif f_ExposureProgram(String type, String value) {
            if (type.ToUpper() == "ExposureProgram".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "ExposureProgram";
                exif.type_ch = "曝光軟體";

                String tt = value;

                if (tt == "0") {
                    exif.value_en = "Not defined";
                    exif.value_ch = "沒有定義 (Not defined)";

                } else if (tt == "1") {
                    exif.value_en = "Manual";
                    exif.value_ch = "手動曝光 (Manual)";

                } else if (tt == "2") {
                    exif.value_en = "Normal program";
                    exif.value_ch = "正常 (Normal program)";

                } else if (tt == "3") {
                    exif.value_en = "Aperture priority";
                    exif.value_ch = "光圈優先 (Aperture priority)";

                } else if (tt == "4") {
                    exif.value_en = "Shutter priority";
                    exif.value_ch = "快門優先 (Shutter priority)";

                } else if (tt == "5") {
                    exif.value_en = "Creative program (biased toward depth of field)";
                    exif.value_ch = "創意程序 (慢速程序) (Creative program )";

                } else if (tt == "6") {
                    exif.value_en = "Action program (biased toward fast shutter speed)";
                    exif.value_ch = "動作程序 (高速程序) (Action program)";

                } else if (tt == "7") {
                    exif.value_en = "Portrait mode (for closeup photos with the background out of focus)";
                    exif.value_ch = "肖像模式 (Portrait mode)";

                } else if (tt == "8") {
                    exif.value_en = "Landscape mode (for landscape photos with the background in focus)";
                    exif.value_ch = "風景模式 (Landscape mode)";

                } else {
                    exif.value_en = value;
                }

                return exif;
            } else {
                return null;
            }
        }





        /// <summary>
        /// exif版本
        /// </summary>
        private data_exif f_ExifVersion(String type, String value) {
            if (type.ToUpper() == "ExifVersion".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "ExifVersion";
                exif.type_ch = "Exif 版本";
                exif.value_en = asciiToString(value);
                return exif;
            } else {
                return null;
            }
        }


        /// <summary>
        /// 亮度
        /// </summary>
        private data_exif f_BrightnessValue(String type, String value) {
            if (type.ToUpper() == "BrightnessValue".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "BrightnessValue";
                exif.type_ch = "亮度";

                try {
                    float ff = float.Parse(value.Split(' ')[0]);
                    exif.value_en = ff.ToString("0.00") + " " + value.Split(' ')[1];
                } catch {
                    exif.value_en = value;
                }

                return exif;
            } else {
                return null;
            }
        }



        /// <summary>
        /// 曝光補償
        /// </summary>
        private data_exif f_ExposureBiasValue(String type, String value) {
            if (type.ToUpper() == "ExposureBiasValue".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "ExposureBiasValue";
                exif.type_ch = "曝光補償";

                try {
                    float ff = float.Parse(value.Split(' ')[0]);
                    exif.value_en = ff.ToString("0.00").Replace(".00", "") + "EV " + value.Split(' ')[1];
                } catch {
                    exif.value_en = value;
                }

                return exif;
            } else {
                return null;
            }
        }




        /// <summary>
        /// 最大光圈值
        /// </summary>
        private data_exif f_MaxApertureValue(String type, String value) {
            if (type.ToUpper() == "MaxApertureValue".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "MaxApertureValue";
                exif.type_ch = "最大光圈值";

                try {
                    float ff = float.Parse(value.Split(' ')[0]);
                    exif.value_en = ff.ToString("0.00").Replace(".00", "") + " " + value.Split(' ')[1];
                } catch {
                    exif.value_en = value;
                }

                return exif;
            } else {
                return null;
            }
        }


        /// <summary>
        /// 曝光方式
        /// </summary>
        private data_exif f_MeteringMode(String type, String value) {
            if (type.ToUpper() == "MeteringMode".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "MeteringMode";
                exif.type_ch = "測光模式";

                String tt = value;

                if (tt == "0") {
                    exif.value_en = "Unknown";
                    exif.value_ch = "未知";

                } else if (tt == "1") {
                    exif.value_en = "Average";
                    exif.value_ch = "平均測光";

                } else if (tt == "2") {
                    exif.value_en = "CenterWeightedAverage";
                    exif.value_ch = "中央重點測光 (CenterWeightedAverage)";

                } else if (tt == "3") {
                    exif.value_en = "Spot";
                    exif.value_ch = "點測光 (Spot)";

                } else if (tt == "4") {
                    exif.value_en = "MultiSpot";
                    exif.value_ch = "多點測光 (MultiSpot)";

                } else if (tt == "5") {
                    exif.value_en = "Pattern";
                    exif.value_ch = "多區域測光 (Pattern)";

                } else if (tt == "6") {
                    exif.value_en = "Partial";
                    exif.value_ch = "部分測光 (Partial)";

                } else if (tt == "255") {
                    exif.value_en = "other";
                    exif.value_ch = "其他 (other)";

                } else {
                    exif.value_en = value;
                    exif.value_ch = value;

                }



                return exif;
            } else {
                return null;
            }
        }


        /// <summary>
        /// 閃光燈
        /// </summary>
        private data_exif f_Flash(String type, String value) {
            if (type.ToUpper() == "Flash".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "Flash";
                exif.type_ch = "閃光燈";



                if (value.Length > 0) {
                    switch (Int32.Parse(value)) {
                        case 0:
                            exif.value_en = "Flash did not fire.";
                            exif.value_ch = "未啟用";
                            break;
                        case 1:
                            exif.value_en = "Flash fired.";
                            exif.value_ch = "啟用";
                            break;
                        case 5:
                            exif.value_en = "Strobe return light not detected.";
                            //exif.value_ch = "未檢測閃光燈返回光";
                            break;
                        case 7:
                            exif.value_en = "Strobe return light detected.";
                            //exif.value_ch = "檢測到閃光燈返迴光";
                            break;
                        case 9:
                            exif.value_en = "Flash fired, compulsory flash mode.";
                            exif.value_ch = "啟用 / 強制閃光模式";
                            break;
                        case 13:
                            exif.value_en = "Flash fired, compulsory flash mode, return light not detected.";
                            exif.value_ch = "啟用 / 強制閃光模式 / return light not detected";
                            break;
                        case 15:
                            exif.value_en = "Flash fired, compulsory flash mode, return light detected.";
                            exif.value_ch = "啟用 / 強制閃光模式 / return light detected";
                            break;
                        case 16:
                            exif.value_en = "Flash did not fire, compulsory flash mode.";
                            exif.value_ch = "未啟動 / 強制閃光模式";
                            break;
                        case 24:
                            exif.value_en = "Flash did not fire, auto mode.";
                            exif.value_ch = "未啟動 / 自動模式";
                            break;
                        case 25:
                            exif.value_en = "Flash fired, auto mode.";
                            exif.value_ch = "啟動 / 自動模式";
                            break;
                        case 29:
                            exif.value_en = "Flash fired, auto mode, return light not detected.";
                            exif.value_ch = "啟用 / 自動模式 / return light not detected";
                            break;
                        case 31:
                            exif.value_en = "Flash fired, auto mode, return light detected.";
                            exif.value_ch = "啟用 / 自動模式 / return light detected";
                            break;
                        case 32:
                            exif.value_en = "No flash function.";
                            exif.value_ch = "無閃光燈功能";
                            break;
                        case 65:
                            exif.value_en = "Flash fired, red-eye reduction mode.";
                            exif.value_ch = "未啟動 / 防紅眼模式";
                            break;
                        case 69:
                            exif.value_en = "Flash fired, red-eye reduction mode, return light not detected.";
                            exif.value_ch = "啟動 / 防紅眼模式 / return light not detected";
                            break;
                        case 71:
                            exif.value_en = "Flash fired, red-eye reduction mode, return light detected.";
                            exif.value_ch = "啟動 / 防紅眼模式 /  return light detected";
                            break;
                        case 73:
                            exif.value_en = "Flash fired, compulsory flash mode, red-eye reduction mode.";
                            exif.value_ch = "啟動 / 強制閃光模式 / 防紅眼模式";
                            break;
                        case 77:
                            exif.value_en = "Flash fired, compulsory flash mode, red-eye reduction mode, return light not detected.";
                            exif.value_ch = "啟動 / 強制閃光模式 / 防紅眼模式 / return light not detected";
                            break;
                        case 79:
                            exif.value_en = "Flash fired, compulsory flash mode, red-eye reduction mode, return light detected.";
                            exif.value_ch = "啟動 / 強制閃光模式 / 防紅眼模式 / return light detected";
                            break;
                        case 89:
                            exif.value_en = "Flash fired, auto mode, red-eye reduction mode.";
                            exif.value_ch = "啟動 / 自動模式 / 防紅眼模式";
                            break;
                        case 93:
                            exif.value_en = "Flash fired, auto mode, return light not detected, red-eye reduction mode.";
                            exif.value_ch = "啟動 / 自動模式 / return light not detected / 防紅眼模式";
                            break;
                        case 95:
                            exif.value_en = "Flash fired, auto mode, return light detected, red-eye reduction mode.";
                            exif.value_ch = "啟動 / 自動模式 / return light detected / 防紅眼模式";
                            break;
                        default:
                            exif.value_en = "Not defined, reserved.";
                            exif.value_ch = "未定義";
                            break;
                    }
                }




                return exif;
            } else {
                return null;
            }
        }


        /// <summary>
        /// 焦距
        /// </summary>
        private data_exif f_FocalLength(String type, String value) {
            if (type.ToUpper() == "FocalLength".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "FocalLength";
                exif.type_ch = "焦距";

                try {
                    float ff = float.Parse(value.Split(' ')[0]);
                    exif.value_en = ff.ToString("0.00").Replace(".00", "") + "mm " + value.Split(' ')[1];
                } catch {
                    exif.value_en = value;
                }

                return exif;
            } else {
                return null;
            }
        }


        /// <summary>
        /// 曝光模式
        /// </summary>
        private data_exif f_ExposureMode(String type, String value) {
            if (type.ToUpper() == "ExposureMode".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "ExposureMode";
                exif.type_ch = "曝光模式";

                String tt = value;

                if (tt == "0") {
                    exif.value_en = "Auto exposure";
                    exif.value_ch = "自動 (Auto exposure)";

                } else if (tt == "1") {
                    exif.value_en = "Manual exposure";
                    exif.value_ch = "手動 (Manual exposure)";

                } else if (tt == "2") {
                    exif.value_en = "Auto bracket";
                    //exif.value_ch = "(Auto bracket)";

                } else {
                    exif.value_en = value;
                    exif.value_ch = value;

                }



                return exif;
            } else {
                return null;
            }
        }



        /// <summary>
        /// 白平衡
        /// </summary>
        private data_exif f_WhiteBalance(String type, String value) {
            if (type.ToUpper() == "WhiteBalance".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "WhiteBalance";
                exif.type_ch = "白平衡";

                String tt = value;

                if (tt == "0") {
                    exif.value_en = "Auto white balance";
                    exif.value_ch = "自動 (Auto white balance)";

                } else if (tt == "1") {
                    exif.value_en = "Manual white balance";
                    exif.value_ch = "手動 (Manual white balance)";

                } else {
                    exif.value_en = value;
                    exif.value_ch = value;

                }

                return exif;
            } else {
                return null;
            }
        }


        /// <summary>
        /// 場景拍攝類型
        /// </summary>
        private data_exif f_SceneCaptureType(String type, String value) {
            if (type.ToUpper() == "SceneCaptureType".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "SceneCaptureType";
                exif.type_ch = "場景拍攝類型";

                String tt = value;

                if (tt == "0") {
                    exif.value_en = "Standard";
                    exif.value_ch = "標準 (Standard)";

                } else if (tt == "1") {
                    exif.value_en = "Landscape";
                    exif.value_ch = "景觀 (Landscape)";

                } else if (tt == "2") {
                    exif.value_en = "Portrait";
                    exif.value_ch = "肖像 (Portrait)";

                } else if (tt == "3") {
                    exif.value_en = "Night scene";
                    exif.value_ch = "夜景 (Night scene)";

                } else {
                    exif.value_en = value;
                    exif.value_ch = value;

                }

                return exif;
            } else {
                return null;
            }
        }





        /// <summary>
        /// 影像ID
        /// </summary>
        private data_exif f_ImageUniqueID(String type, String value) {
            if (type.ToUpper() == "ImageUniqueID".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "ImageUniqueID";
                exif.type_ch = "影像ID";
                exif.value_en = value;

                return exif;
            } else {
                return null;
            }
        }


        /// <summary>
        /// 快門速度
        /// </summary>
        private data_exif f_ShutterSpeedValue(String type, String value) {
            if (type.ToUpper() == "ShutterSpeedValue".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "ShutterSpeedValue";
                exif.type_ch = "快門速度";

                try {
                    float ff = float.Parse(value.Split(' ')[0]);
                    exif.value_en = ff.ToString("0.00").Replace(".00", "") + " " + value.Split(' ')[1];
                } catch {
                    exif.value_en = value;
                }



                return exif;
            } else {
                return null;
            }
        }


        /// <summary>
        /// 光圈
        /// </summary>
        private data_exif f_ApertureValue(String type, String value) {
            if (type.ToUpper() == "ApertureValue".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "ApertureValue";
                exif.type_ch = "光圈";

                try {
                    float ff = float.Parse(value.Split(' ')[0]);
                    exif.value_en = ff.ToString("0.00").Replace(".00", "") + " " + value.Split(' ')[1];
                } catch {
                    exif.value_en = value;
                }

                return exif;
            } else {
                return null;
            }
        }


        /// <summary>
        /// 連續彩色線性傳感器
        /// </summary>
        private data_exif f_SensingMethod(String type, String value) {
            if (type.ToUpper() == "SensingMethod".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "SensingMethod";
                exif.type_ch = "連續彩色線性傳感器";
                exif.value_en = value;

                try {

                    int data = Int32.Parse(value);
                    switch (data) {
                        case 1:
                            exif.value_en = "Not defined.";
                            exif.value_ch = "沒有定義";
                            break;
                        case 2:
                            exif.value_en = "One-chip color area sensor";
                            break;
                        case 3:
                            exif.value_en = "Two-chip color area sensor";
                            break;
                        case 4:
                            exif.value_en = "Three-chip color area sensor";
                            break;
                        case 5:
                            exif.value_en = "Color sequential area sensor";
                            break;
                        case 7:
                            exif.value_en = "Trilinear sensor";
                            break;
                        case 8:
                            exif.value_en = "Color sequential linear sensor";
                            break;
                        default:
                            exif.value_en = "Reserved";
                            break;
                    }
                } catch { }


                return exif;
            } else {
                return null;
            }
        }



        /// <summary>
        /// 自定義渲染
        /// </summary>
        private data_exif f_CustomRendered(String type, String value) {
            if (type.ToUpper() == "CustomRendered".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "CustomRendered";
                exif.type_ch = "自定義渲染";
             

                try {

                    int data = Int32.Parse(value);
                    switch (data) {
                        case 1:
                            exif.value_en = "Normal process";
                            exif.value_ch = "正常";
                            break;
                        case 2:
                            exif.value_en = "Custom process";
                            exif.value_ch = "自訂";
                            break;

                        default:
                            exif.value_en = "Reserved";
                            break;
                    }
                } catch { }


                return exif;
            } else {
                return null;
            }
        }



        /// <summary>
        /// 拍攝時間 
        /// </summary>
        private data_exif f_DateTimeOriginal(String type, String value) {
            if (type.ToUpper() == "DateTimeOriginal".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "DateTimeOriginal";
                exif.type_ch = "拍攝時間";
                exif.value_en = value;

                return exif;
            } else {
                return null;
            }
        }


        /// <summary>
        /// FocalLengthIn35mmFilm
        /// </summary>
        private data_exif f_FocalLengthIn35mmFilm(String type, String value) {
            if (type.ToUpper() == "FocalLengthIn35mmFilm".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "FocalLengthIn35mmFilm";
                exif.type_ch = "35mm等效焦距";
                exif.value_en = value;

                return exif;
            } else {
                return null;
            }
        }






        /// <summary>
        /// 光源
        /// </summary>
        private data_exif f_LightSource(String type, String value) {
            if (type.ToUpper() == "LightSource".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "LightSource";
                exif.type_ch = "光源";
            

                try {

                    int data = Int32.Parse(value);
                    switch (data) {
                        case 1:
                            exif.value_en = "Daylight";
                            exif.value_ch = "陽光";
                            break;
                        case 2:
                            exif.value_en = "Fluorescent";
                            exif.value_ch = "日光燈";
                            break;
                        case 3:
                            exif.value_en = "Tungsten (incandescent light)";
                            exif.value_ch = "鎢（白熾燈）";
                            break;
                        case 4:
                            exif.value_en = "Flash";
                            exif.value_ch = "閃光燈";
                            break;
                        case 9:
                            exif.value_en = "Fine weather";
                            exif.value_ch = "晴天";
                            break;
                        case 10:
                            exif.value_en = "Cloudy weather";
                            exif.value_ch = "多雲天氣";
                            break;
                        case 11:
                            exif.value_en = "Shade";
                            exif.value_ch = "陰 (Shade)";
                            break;
                        case 12:
                            exif.value_en = "Daylight fluorescent (D 5700 - 7100K)";
                            break;
                        case 13:
                            exif.value_en = "Day white fluorescent (N 4600 - 5400K)";
                            break;
                        case 14:
                            exif.value_en = "Cool white fluorescent (W 3900 - 4500K)";
                            break;
                        case 15:
                            exif.value_en = "White fluorescent (WW 3200 - 3700K)";
                            break;
                        case 17:
                            exif.value_en = "Standard light A";
                            break;
                        case 18:
                            exif.value_en = "Standard light B";
                            break;
                        case 19:
                            exif.value_en = "Standard light C";
                            break;
                        case 20:
                            exif.value_en = "D55";
                            break;
                        case 21:
                            exif.value_en = "D65";
                            break;
                        case 22:
                            exif.value_en = "D75.";
                            break;
                        case 23:
                            exif.value_en = "D50";
                            break;
                        case 24:
                            exif.value_en = "ISO studio tungsten";
                            break;
                        case 255:
                            exif.value_en = "other light source";
                            exif.value_ch = "其他光源";
                            break;
                        default:
                            exif.value_en = "Reserved";
                            break;
                    }
                } catch { }


                return exif;
            } else {
                return null;
            }
        }



        /// <summary>
        /// 數位變焦比率 
        /// </summary>
        private data_exif f_DigitalZoomRatio(String type, String value) {
            if (type.ToUpper() == "DigitalZoomRatio".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "DigitalZoomRatio";
                exif.type_ch = "數位變焦比率";

                try {
                    float ff = float.Parse(value.Split(' ')[0]);
                    exif.value_en = ff.ToString("0.00").Replace(".00", "") + " " + value.Split(' ')[1];
                } catch {
                    exif.value_en = value;
                }

                return exif;
            } else {
                return null;
            }
        }


        /// <summary>
        /// SubjectDistanceRange 
        /// </summary>
        private data_exif f_SubjectDistanceRange(String type, String value) {
            if (type.ToUpper() == "SubjectDistanceRange".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "SubjectDistanceRange";
                exif.type_ch = "SubjectDistanceRange";

                try {

                    int data = Int32.Parse(value);
                    switch (data) {
                        case 0:
                            exif.value_en = "Unknown";
                            exif.value_ch = "未知";
                            break;
                        case 1:
                            exif.value_en = "Macro";
                            exif.value_ch = "微距 (Macro)";
                            break;
                        case 2:
                            exif.value_en = "Close view";
                            exif.value_ch = "近景 (Close view)";
                            break;
                        case 3:
                            exif.value_en = "Distant view";
                            exif.value_ch = "遠景 (Distant view)";
                            break;
                        default:
                            exif.value_en = "Reserved";
                            break;
                    }
                } catch { }

                return exif;
            } else {
                return null;
            }
        }



        /// <summary>
        /// YCbCrPositioning 
        /// </summary>
        private data_exif f_YCbCrPositioning(String type, String value) {
            if (type.ToUpper() == "YCbCrPositioning".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "YCbCrPositioning";
                exif.type_ch = "YCbCrPositioning";

                try {

                    int data = Int32.Parse(value);
                    switch (data) {
              
                        case 1:
                            exif.value_en = "centered";
                            exif.value_ch = "centered";
                            break;
                        case 2:
                            exif.value_en = "co-sited";
                            exif.value_ch = "co-sited";
                            break;
  
                        default:
                            exif.value_en = "Reserved";
                            break;
                    }
                } catch { }

                return exif;
            } else {
                return null;
            }
        }


        /// <summary>
        /// GPSLatitudeRef(判斷南北半球)(不顯示)
        /// </summary>
        private data_exif f_GPSLatitudeRef(String type, String value) {
            if (type.ToUpper() == "GPSLatitudeRef".ToUpper()) {
                GPSLatitudeRef = value;
                return null;
            } else {
                return null;
            }
        }


        /// <summary>
        /// GPSLongitudeRef(判斷東西半球)(不顯示)
        /// </summary>
        private data_exif f_GPSLongitudeRef(String type, String value) {
            if (type.ToUpper() == "GPSLongitudeRef".ToUpper()) {
                GPSLongitudeRef = value;
                return null;
            } else {
                return null;
            }
        }




        /// <summary>
        /// GPS 高度 
        /// </summary>
        private data_exif f_GPSAltitude(String type, String value) {
            if (type.ToUpper() == "GPSAltitude".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "GPSAltitude";
                exif.type_ch = "GPS 高度";

                try {
                    float ff = float.Parse(value.Split(' ')[0]);
                    exif.value_en = ff.ToString("0.0000").Replace(".0000", "") + " " + value.Split(' ')[1];
                } catch {
                    exif.value_en = value;
                }

                return exif;
            } else {
                return null;
            }
        }


        String s_緯度 = "";
        String s_經度 = "";

        /// <summary>
        /// GPS 緯度
        /// </summary>
        private data_exif f_GPSLatitude(String type, String value) {
            if (type.ToUpper() == "GPSLatitude".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "GPSLatitude";
                exif.type_ch = "GPS 緯度";

                try {

                    value = value.Replace(" ", "");
                    String[] ars = value.Split(',');
                    double d1 = double.Parse(ars[0]);
                    double d2 = double.Parse(ars[1]);
                    double d3 = double.Parse(ars[2]);

                    if (GPSLatitudeRef.ToUpper() == "S") {
                        d1 *= -1;
                    }

                    s_緯度 = d1 + " " + d2 + " " + d3;
                    exif.value_en = d1 + ", " + d2 + ", " + d3;
                } catch {
                    exif.value_en = value;
                }

                return exif;
            } else {
                return null;
            }
        }


        /// <summary>
        /// GPS 經度
        /// </summary>
        private data_exif f_GPSLongitude(String type, String value) {
            if (type.ToUpper() == "GPSLongitude".ToUpper()) {
                data_exif exif = new data_exif();
                exif.type_en = "GPSLongitude";
                exif.type_ch = "GPS 經度";

                try {

                    value = value.Replace(" ", "");
                    String[] ars = value.Split(',');
                    double d1 = double.Parse(ars[0]);
                    double d2 = double.Parse(ars[1]);
                    double d3 = double.Parse(ars[2]);

                    if (GPSLongitudeRef.ToUpper() == "W") {
                        d1 *= -1;
                    }

                    s_經度 = d1 + " " + d2 + " " + d3;
                    exif.value_en = d1 + ", " + d2 + ", " + d3;
                } catch {
                    exif.value_en = value;
                }

                return exif;
            } else {
                return null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private String asciiToString(String value) {
            for (int i = 48; i <= 57; i++) {
                value = value.Replace(i + "", (i - 48) + "");
            }
            value = value.Replace(" ", "").Replace(",", "");
            return value;
        }







        #endregion







    }


    public class data_exif {

        public String type_en;
        public String type_ch;
        public String value_en;

        private String _value_ch;
        public String value_ch {//如果沒有中文版的，就回傳英文
            get {
                if (_value_ch == null) {
                    return value_en;
                } else {
                    return _value_ch;
                }
            }
            set {
                _value_ch = value;
            }
        }

    }


}
