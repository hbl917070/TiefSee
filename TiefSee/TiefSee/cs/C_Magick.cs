using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TiefSee.cs {
    public class C_Magick {



        MainWindow M;

        public C_Magick(MainWindow m) {


            this.M = m;


        }







        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MagickImage getImg(String inputPath, String type) {

            MagickImage mmm = null;

            if (M.c_影像.func_判斷_RAW(type)) {

                string dcRawExe = M.fun_執行檔路徑() + "/data/dcraw-9.27-ms-32-bit.exe";
                var startInfo = new System.Diagnostics.ProcessStartInfo(dcRawExe) {
                    Arguments = "-c -e \"" + inputPath + "\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                try {
                    using (var process = System.Diagnostics.Process.Start(startInfo)) {
                        Stream st = process.StandardOutput.BaseStream;
                        var memoryStream = new MemoryStream();
                        st.CopyTo(memoryStream);
                        memoryStream.Position = 0;

                        if (type == "X3F") {
                            mmm = new MagickImage(new System.Drawing.Bitmap(memoryStream));
                        } else {
                            mmm = new MagickImage((memoryStream));
                        }

                    }
                } catch (Exception e) {

                    Log.print("MagickImage錯誤" + e.ToString());
                }


            } else




            if (type == "CR2") {

                //raw。必須從stream讀取圖片，不然會有BUG
                using (FileStream logFileStream = new FileStream(inputPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                    mmm = new MagickImage(logFileStream);
                }

            } else if (type == "PDF" || type == "SVG" || type == "AI" || type == "WMF" || type == "EMF") {

                //設定輸出的解析度，用於向量圖片(svg、ai、pdf
                MagickReadSettings settings = new MagickReadSettings();
                settings.Density = new Density(300, 300);
                mmm = new MagickImage(inputPath, settings);

            } else {

                //一般       
                mmm = new MagickImage(inputPath);

            }


            //矯正顏色
            // Adding the second profile will transform the colorspace from CMYK to RGB
            ColorProfile cp = ColorProfile.SRGB;
            Log.print(mmm.ColorSpace + "  " + mmm.ColorType);
            if (mmm.ColorSpace == ColorSpace.YCbCr) {
                Log.print("停止使用SRGB");
            } else {
                //mmm.AddProfile(cp);
                mmm.SetProfile(cp);
            }
            /*if (im5.GetColorProfile() != null ) {
                if (im5.GetColorProfile().Model != null) {
                    Log.print("***" + im5.GetColorProfile().Model + "***");
                    im5.AddProfile(cp);
                } else if (im5.ColorSpace == ColorSpace.CMYK) {
                    Log.print("類型：CMYK");

                    im5.AddProfile(cp);
                } else {
                    Log.print("無 GetColorProfile().Model");
                }
            } else {
                Log.print("無2 GetColorProfile()");
                im5.AddProfile(cp);
            }*/




            return mmm;
        }







        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /*public System.Drawing.Bitmap getImgBitmap(String inputPath, String type) {


            //設定輸出的解析度，用於向量圖片(svg、ai、pdf
            MagickReadSettings settings = new MagickReadSettings();
            settings.Density = new Density(300, 300);


            bool bool_raw = false;

            foreach (var item in ar_RAW格式) {
                if (type == item) {
                    bool_raw = true;
                    break;
                }
            }

            if (bool_raw) { //raw。必須從stream讀取圖片，不然會有BUG              
                using (FileStream logFileStream = new FileStream(inputPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                    using (var image = new MagickImage(logFileStream)) {
                        return image.ToBitmap();
                    }
                }
            } else if (type == "PDF" || type == "SVG" || type == "AI" || type == "WMF" || type == "EMF") {
                //向量
                using (var image = new MagickImage(inputPath, settings)) {
                    return image.ToBitmap();
                }
            } else {  //一般     
                using (var image = new MagickImage(inputPath)) {
                    return image.ToBitmap();
                }
            }
        }*/





    }
}
