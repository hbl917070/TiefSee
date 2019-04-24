using TiefSee.cs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace TiefSee {
    public class C_影像 {


        MainWindow M;
        public C_Magick c_Magick;
        public C_Exif c_exif;


        public List<String> ar_RAW = new List<string>();
        List<String> ar_特殊格式 = new List<string>();

        public C_影像(MainWindow m) {
            this.M = m;
            c_Magick = new C_Magick(m);
            c_exif = new C_Exif(M);


            ar_RAW.Add("RAF");
            //ar_RAW.Add("CRW");
            ar_RAW.Add("CR2");
            ar_RAW.Add("MRW");
            ar_RAW.Add("NEF");
            ar_RAW.Add("X3F");
            ar_RAW.Add("PEF");
            //ar_RAW.Add("DNG");
            ar_RAW.Add("ORF");
            ar_RAW.Add("RW2");
            ar_RAW.Add("ARW");
            ar_RAW.Add("ERF");
            ar_RAW.Add("SR2");
            ar_RAW.Add("SRW");


            //ar_RAW.Add("MPO"); = jpg

            //----------


            //一般圖片
            ar_特殊格式.Add("PPM");
            ar_特殊格式.Add("TGA");//
            ar_特殊格式.Add("PCX");//
            ar_特殊格式.Add("PGM");//
            ar_特殊格式.Add("PBM");//
            ar_特殊格式.Add("PSB");//photoshop的其中一種檔案
            ar_特殊格式.Add("PSD");//photoshop的其中一種檔案

            //新圖片格式
            ar_特殊格式.Add("WEBP");
            ar_特殊格式.Add("JPF");//

            //相機
            ar_特殊格式.Add("MPO");//
            ar_特殊格式.Add("CR2");
            ar_特殊格式.Add("DNG");//

            //向量
            ar_特殊格式.Add("EMF");//
            ar_特殊格式.Add("WMF");//

            //360
            ar_特殊格式.Add("HDR");//
        }





        /// <summary>
        /// 判斷附檔名
        /// </summary>
        /// <param name="fileName">檔案或資料夾路徑</param>
        /// <returns>大寫附檔名，如果沒有則回傳 "" </returns>
        public String fun_取得附檔名(string fileName) {

            if (Directory.Exists(fileName)) {//如果是資料夾，就不會有附檔名
                return "";
            }

            String s_用路徑判斷附檔名 = "";//無法判斷時，直接用附檔名判斷

            try {
                int stert = fileName.LastIndexOf('.');
                if (stert > 0) {
                    String s = fileName.Substring(stert + 1);
                    s_用路徑判斷附檔名 = s.ToUpper();
                } else {
                    s_用路徑判斷附檔名 = "?";
                }
            } catch { }

            try {

                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read)) {
                    using (System.IO.BinaryReader br = new System.IO.BinaryReader(fs)) {


                        string fileType = string.Empty;
                        try {
                            byte data = br.ReadByte();
                            fileType += data.ToString();
                            data = br.ReadByte();
                            fileType += data.ToString();

                            if (fileType == "255216") {
                                return "JPG";
                            }
                            if (fileType == "7173") {
                                return "GIF";
                            }
                            if (fileType == "13780") {
                                return "PNG";
                            }
                            if (fileType == "6787") {
                                return "SWF";
                            }
                            if (fileType == "6677") {
                                return "BMP";
                            }
                            if (fileType == "5666") {
                                return "PSD";
                            }
                            if (fileType == "8297") {
                                return "RAR";
                            }
                            if (fileType == "8075") {
                                if (s_用路徑判斷附檔名 == "DOCX")
                                    return "DOCX";
                                else if (s_用路徑判斷附檔名 == "PPTX")
                                    return "PPTX";
                                else if (s_用路徑判斷附檔名 == "APK")
                                    return "APK";
                                else
                                    return "ZIP";
                                //return "ZIP";//docx pptx apk
                            }
                            if (fileType == "55122") {
                                return "7Z";
                            }
                            if (fileType == "3780") {
                                if (s_用路徑判斷附檔名 == "AI")
                                    return "AI";
                                else
                                    return "PDF";
                            }
                            if (fileType == "8273") {
                                if (s_用路徑判斷附檔名 == "AVI")
                                    return "AVI";
                                else if (s_用路徑判斷附檔名 == "WAV")
                                    return "WAV";
                                else
                                    return "WEBP";
                            }
                            if (fileType == "4838") {
                                return "WMV";
                            }
                            if (fileType == "2669") {
                                return "MKV";
                            }
                            if (fileType == "7076") {
                                return "FLV";
                            }
                            if (fileType == "1") {
                                return "TTF";
                            }

                            //無法判斷時，直接用附檔名判斷
                            return s_用路徑判斷附檔名;

                            /*
                            String extension = "？";
                            try {
                                extension = ((FileExtension)Enum.Parse(typeof(FileExtension), fileType)).ToString();
                            } catch {
                                extension = FileExtension.VALIDFILE.ToString();
                            }

                            System.Console.WriteLine(fileName + "\n" + fileType + "\n\n\n");

                            //無法判斷時，直接用附檔名判斷
                            if (extension == fileType || extension == "0") {
                                return s_用路徑判斷附檔名;
                            }

                            if (extension == "AVI" && s_用路徑判斷附檔名 == "WEBP") {
                                return "WebP";
                            }
                            return extension;*/

                        } catch (Exception ex) {

                            return s_用路徑判斷附檔名;

                        } finally {
                            if (fs != null) {
                                fs.Close();
                                br.Close();
                            }
                        }

                    }//using
                }//using

            } catch {

                return s_用路徑判斷附檔名;
            }



        }

        /*
        public enum FileExtension {
            JPG = 255216,
            GIF = 7173,
            PNG = 13780,
            SWF = 6787,
            BMP = 6677,
            PSD = 5666,

            RAR = 8297,
            ZIP = 8075,//docx pptx apk...
            _7Z = 55122,

            VALIDFILE = 9999999,

            PDF = 3780,//ai
            AVI = 8273,//wav
            WMV = 4838,
            //MP4 = 0,
            MKV = 2669,
            FLV = 7076,

            PPTX = 208207,
            //EXE = 7790,
            TTF = 1,
        }*/


        /// <summary>
        /// 
        /// </summary>
        public double fun_判斷檔案大小_MB(String path) {

            double len = 0;
            if (Directory.Exists(path)) {//資料夾
                DirectoryInfo dInfo = new DirectoryInfo(path);
                FileInfo[] fInfoArr = dInfo.GetFiles();
                foreach (var item in fInfoArr) {
                    len += item.Length;
                }
            } else {//檔案
                len = new FileInfo(path).Length;
            }

            return (len / (1024 * 1024));
         

    
        }

        /// <summary>
        /// 
        /// </summary>
        public String fun_判斷檔案大小(String path) {

            double len = 0;
            if (Directory.Exists(path)) {//資料夾
                DirectoryInfo dInfo = new DirectoryInfo(path);
                FileInfo[] fInfoArr = dInfo.GetFiles();
                foreach (var item in fInfoArr) {
                    len += item.Length;
                }
            } else {//檔案
                len = new FileInfo(path).Length;
            }

            if (len / 1024 < 1) {
                return len.ToString("0.0") + " B";
            } else if (len / (1024 * 1024) < 1) {
                return (len / (1024)).ToString("0.0") + " KB";
            } else if (len / (1024 * 1024 * 1024) < 1) {
                return (len / (1024 * 1024)).ToString("0.0") + " MB";
            } else if (len / (1024 * 1024 * 1024) / 1024 < 1) {
                return (len / (1024f * 1024f * 1024f)).ToString("0.00") + " GB";
            }

            return len.ToString("0.0");
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool func_判斷_RAW(String s_附檔名) {
            bool bool_執行 = false;

            foreach (var item in ar_RAW) {
                if (item == s_附檔名) {
                    bool_執行 = true;
                    break;
                }
            }
            return bool_執行;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="s_附檔名"></param>
        /// <returns></returns>
        public BitmapSource func_dcraw(string inputFile, String s_附檔名) {



            if (func_判斷_RAW(s_附檔名) == false) {
                return null;
            }


            string dcRawExe = M.fun_執行檔路徑() + "/data/dcraw-9.27-ms-32-bit.exe";
            var startInfo = new ProcessStartInfo(dcRawExe) {
                Arguments = "-c -e \"" + inputFile + "\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };


            try {

                using (var process = Process.Start(startInfo)) {

                    Stream st = process.StandardOutput.BaseStream;
                    var memoryStream = new MemoryStream();
                    st.CopyTo(memoryStream);
                    memoryStream.Position = 0;
                    var encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(memoryStream));

                    return encoder.Frames[0];
                }

            } catch { }

            return null;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public System.Drawing.Bitmap func_作業系統圖示(String path) {

            //取得圖片在Windows系統的縮圖
            var icon = WindowsThumbnailProvider.GetThumbnail(
                            path,
                            (int)(256 * M.d_解析度比例_x),
                            (int)(256 * M.d_解析度比例_y),
                            ThumbnailOptions.ScaleUp
                        );

            return icon;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="im"></param>
        /// <returns></returns>
        private BitmapSource func_mi_回傳前壓縮圖片(ImageMagick.MagickImage im) {

            if (im.Width > 2500 || im.Height > 2500) {//縮小圖片後在載入
                using (ImageMagick.IMagickImage ii = im.Clone()) {

                    //計算縮放比例
                    double d_縮放倍率 = 1;

                    var w03 = M.b2.ActualWidth;
                    var h03 = M.b2.ActualHeight;

                    if (ii.Width / w03 > ii.Height / h03) {
                        d_縮放倍率 = M.b2.ActualWidth / ii.Width;
                    } else {
                        d_縮放倍率 = M.b2.ActualHeight / ii.Height;
                    }

                    //縮放
                    var mg = new ImageMagick.MagickGeometry();
                    mg.Height = (int)(ii.Height * d_縮放倍率);
                    mg.Width = (int)(ii.Width * d_縮放倍率);
                    ii.Resize(mg);

                    return ii.ToBitmapSource();
                }
            } else {
                return im.ToBitmapSource();
            }
        }


        /// <summary>
        /// 更新界面的exif
        /// </summary>
        public BitmapSource func_get_BitmapImage_更新界面(String path, ref int img_width, ref int img_height) {

            var ss = M.s_img_type_附檔名.ToUpper();


            //如果是 RWA 圖片
            /*var img_raw = func_dcraw(path, ss);

            if (img_raw != null) {
                c_exif.fun_初始化exif資訊();
                c_exif.fun_顯示或隱藏視窗();
                img_width = img_raw.PixelWidth;
                img_height = img_raw.PixelHeight;
                //M.fun_設定顯示圖片size(img_raw.PixelWidth, img_raw.PixelHeight); //設定UI界面顯示的寬高
                return img_raw;
            }*/



            //如果是特殊圖片，就用『Magick.NET』來解析
            if (ar_特殊格式.Contains(ss) || func_判斷_RAW(ss)) {

                c_exif.fun_初始化exif資訊();
                c_exif.fun_顯示或隱藏視窗();

                var im = c_Magick.getImg(path, ss);

                if (M.image_局部高清 != null) {
                    M.image_局部高清.Dispose();


                }
                M.image_局部高清 = null;
                M.image_局部高清 = im;
                System.Console.WriteLine("特殊格式  局部高清 " + M.image_局部高清.Width);


                img_width = im.Width;
                img_height = im.Height;
                //M.fun_設定顯示圖片size(im.Width, im.Height); //設定UI界面顯示的寬高
                return func_mi_回傳前壓縮圖片(im);
            }


            //一般的jpg、png、bmp，就用內建函數，速度最快
            try {

                using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))) {
                    FileInfo fi = new FileInfo(path);
                    byte[] bytes = reader.ReadBytes((int)fi.Length);

                    MemoryStream ms = new MemoryStream(bytes);
                    BitmapDecoder bd = BitmapDecoder.Create(ms, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);


                    //讀取圖片時就讀取exif，才不會浪費載入的成本
                    if (c_exif.fun_is_exif_type() && M.stackPlanel_動圖工具.Visibility == Visibility.Collapsed) {

                        try {
                            using (MemoryStream ms2 = new MemoryStream(bytes)) {
                                c_exif.fun_取得_exif(ms2);
                            }
                            M.c_旋轉.fun_初始化旋轉(c_exif.int_Orientation);
                        } catch {
                            c_exif.fun_初始化exif資訊();
                        }

                    } else {
                        c_exif.fun_初始化exif資訊();
                    }
                    c_exif.fun_顯示或隱藏視窗();


                    reader.Close();
                    reader.Dispose();

                    img_width = bd.Frames[0].PixelWidth;
                    img_height = bd.Frames[0].PixelHeight;
                    //M.fun_設定顯示圖片size(bd.Frames[0].PixelWidth, bd.Frames[0].PixelHeight); //設定UI界面顯示的寬高

                    return bd.Frames[0];
                }

            } catch {


                c_exif.fun_初始化exif資訊();
                c_exif.fun_顯示或隱藏視窗();

                //return c_Magick.getImg(path, ss);
                var im = c_Magick.getImg(path, ss);
                M.image_局部高清 = im;
                //M.fun_設定顯示圖片size(im.Width, im.Height); //設定UI界面顯示的寬高
                img_width = im.Width;
                img_height = im.Height;
                return func_mi_回傳前壓縮圖片(im);





            }


        }



        /// <summary>
        /// 一般 (JPG、PNG、BMP、GIF)
        /// </summary>
        public BitmapSource func_get_BitmapImage_JPG(String path) {

            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))) {
                FileInfo fi = new FileInfo(path);
                byte[] bytes = reader.ReadBytes((int)fi.Length);

                MemoryStream ms = new MemoryStream(bytes);
                BitmapDecoder bd = BitmapDecoder.Create(ms, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);

                reader.Close();
                reader.Dispose();

                return bd.Frames[0];
            }

            /*
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.UriSource = new Uri(path);
            bitmapImage.EndInit();
            return bitmapImage;*/
        }


        /// <summary>
        /// 回傳高度或寬度最大為9999的圖片
        /// </summary>
        public BitmapImage fun_get_BitmapImage_max(String path, String max_type, int int_max) {


            //MessageBox.Show(path);

            //MessageBox.Show(new Uri(path,false).ToString());

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            //bitmapImage.UriSource = new Uri(path);

            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))) {
                FileInfo fi = new FileInfo(path);
                byte[] bytes = reader.ReadBytes((int)fi.Length);
                MemoryStream ms = new MemoryStream(bytes);
                bitmapImage.StreamSource = ms;
                reader.Close();
                reader.Dispose();
            }

            if (max_type == "h") {
                bitmapImage.DecodePixelHeight = int_max;
            } else {
                bitmapImage.DecodePixelWidth = int_max;
            }

            bitmapImage.EndInit();

            return bitmapImage;




            //var bitimg = new BitmapImage(new Uri(path));
            /*
            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open))) {
                FileInfo fi = new FileInfo(path);
                byte[] bytes = reader.ReadBytes((int)fi.Length);
                reader.Close();
                bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream(bytes);
                bitmapImage.EndInit();
                bitmapImage.CacheOption = BitmapCacheOption.None;
            }*/
            //img.Source = bitimg;
            /*using (FileStream FStream = new FileStream(path, FileMode.Open)) {
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = FStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                FStream.Close();
            }*/


            return bitmapImage;
        }










        //----------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPath"></param>
        /// <returns></returns>
        public String func_雜湊檔案(String pPath) {
            var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            long int_檔案大小 = new FileInfo(pPath).Length;
            String s_資料夾名稱 = Convert.ToBase64String(sha1.ComputeHash(Encoding.Default.GetBytes(int_檔案大小 + "")));
            s_資料夾名稱 = s_資料夾名稱.ToLower().Replace("\\", "").Replace("/", "").Replace("+", "").Replace("=", "");
            return s_資料夾名稱;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPath"></param>
        /// <returns></returns>
        public String func_分解GIF(String pPath) {


            String s_資料夾名稱 = func_雜湊檔案(pPath);

            String pSavedPath = M.func_取得暫存路徑() + "GIF\\" + s_資料夾名稱;


            if (Directory.Exists(pSavedPath)) {
                /* try {
                     foreach (var item in Directory.GetFiles(pSavedPath, "*.*")) {
                         File.Delete(item);
                     }
                 } catch { }*/
            } else {

                Directory.CreateDirectory(pSavedPath);


                var gif = System.Drawing.Image.FromFile(pPath);
                var fd = new System.Drawing.Imaging.FrameDimension(gif.FrameDimensionsList[0]);

                //獲取幀數(gif圖片可能包含多幀，其它格式圖片一般僅一幀)
                int count = gif.GetFrameCount(fd);
                //List<string> gifList = new List<string>();
                //以Jpeg格式保存各幀

                for (int i = 0; i < count; i++) {
                    gif.SelectActiveFrame(fd, i);
                    gif.Save(pSavedPath + "\\" + (i + 1) + ".png", System.Drawing.Imaging.ImageFormat.Png);
                    //gifList.Add(pSavedPath + "\\frame_" + i + ".png");
                }
            }

            return pSavedPath;

        }




        //---------------------------------------------





        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public BitmapSource BitmapToBitmapSource(System.Drawing.Bitmap bitmap) {
            IntPtr ptr = bitmap.GetHbitmap();
            BitmapSource result =
                System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    ptr, IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            //release resource
            DeleteObject(ptr);

            bitmap.Dispose();
            bitmap = null;

            return result;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmapsource"></param>
        /// <returns></returns>
        public System.Drawing.Bitmap BitmapFromSource(BitmapSource bitmapsource) {
            System.Drawing.Bitmap bitmap;
            using (MemoryStream outStream = new MemoryStream()) {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new System.Drawing.Bitmap(outStream);
            }
            return bitmap;
        }













    }


}
