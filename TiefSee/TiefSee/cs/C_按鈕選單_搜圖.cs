using TiefSee.W;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TiefSee {




    public class C_按鈕選單_搜圖 {


        MainWindow M;

        public U_menu u_menu_搜圖 ;



        public C_按鈕選單_搜圖(MainWindow m) {

            this.M = m;


            u_menu_搜圖 = new U_menu(m);



            M.but_搜圖.Click += (sender, e) => {
                u_menu_搜圖.func_open(M.but_搜圖);
            };

            u_menu_搜圖.func_add_menu("Google 搜圖", new BitmapImage(new Uri("/imgs/搜圖_google.png", UriKind.Relative)), () => {
                fun_執行搜圖("google");
            });
            u_menu_搜圖.func_add_menu("sauceNAO 搜圖", new BitmapImage(new Uri("/imgs/搜圖_sauceNAO.bmp", UriKind.Relative)), () => {
                fun_執行搜圖("saucenao");
            });
            u_menu_搜圖.func_add_menu("IQDB 搜圖", new BitmapImage(new Uri("/imgs/搜圖_IQDB.png", UriKind.Relative)), () => {
                fun_執行搜圖("iqdb");
            });
            u_menu_搜圖.func_add_menu("Ascii2d 搜圖", new BitmapImage(new Uri("/imgs/搜圖_ascii2d.bmp", UriKind.Relative)), () => {
                fun_執行搜圖("ascii2d");
            });




        }








        /// <summary>
        /// 
        /// </summary>
        /// <param name="s_type"></param>
        private void fun_執行搜圖(String s_type) {


            if (s_type == "iqdb") {

                fun_寫入搜圖input(s_type, M.ar_path[M.int_目前圖片位置]);

                return;
            }


            try {


                var img_width = M.int_img_w;
                var img_height = M.int_img_h;

                bool bool_需要縮小 = img_width > 1000 || img_height > 1000;
                bool_需要縮小 = false;


                BitmapImage bitmapImage = null;
                BitmapEncoder encoder = null;//可能是jpg或png或gif

                if (M.stackPlanel_動圖工具.Visibility == Visibility.Visible) {//動圖

                    encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(M.c_P網.fun_取得第一張圖片()));


                } /*else if (M.s_img_type_附檔名 == "SVG") {//SVG

                    MessageBox.Show("SVG圖片 無法線上搜圖");
                    return;

                }*/ else if (M.s_img_type_顯示類型 == "MOVIE") {//MMOVIE

                    //影片的話，就透過截圖的方式來處理

                    //如果影片大於視窗，就先縮小後再截圖
                    if (M.scrollviewer_1.ScrollableHeight > 0 || M.scrollviewer_1.ScrollableWidth > 0) {

                        M.fun_圖片全滿();

                        new Thread(() => {//50毫秒後重新執行這個function
                            Thread.Sleep(50);
                            C_adapter.fun_UI執行緒(() => {
                                fun_執行搜圖(s_type);
                            });
                        }).Start();

                        return;

                    } else {

                        encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(M.SaveTo(M.img_voide)));

                    }


                } else {//靜態圖


                    String s_當前目錄 = M.ar_path[M.int_目前圖片位置];


                    /*if ( M.s_img_type_附檔名 == "PSD") {//PSD 使用內建的解碼器

                        encoder = new PngBitmapEncoder();

                        //var im = M.fun_get_BitmapImage_s(s_當前目錄);

                        BitmapSource bitimg = null;
                        SimplePsd.CPSD psd = new SimplePsd.CPSD();
                        psd.Load(s_當前目錄);
                        System.Drawing.Bitmap fromHbitmap = System.Drawing.Image.FromHbitmap(psd.GetHBitmap());
                        bitimg = M.c_apng.BitmapToBitmapSource(fromHbitmap);
                        fromHbitmap.Dispose();
                        

                        encoder.Frames.Add(BitmapFrame.Create(bitimg));

                    } else*/

                    String ss = M.s_img_type_附檔名.ToUpper();

                    //一般格式
                    if (ss == "JPG" || ss == "JPEG" || ss == "BMP" || ss == "PNG" || ss == "GIF" || ss == "TIF") {


                        using (BinaryReader reader = new BinaryReader(File.Open(s_當前目錄, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))) {
                            FileInfo fi = new FileInfo(s_當前目錄);
                            byte[] bytes = reader.ReadBytes((int)fi.Length);
                            reader.Close();
                            bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.StreamSource = new MemoryStream(bytes);

                            if (bool_需要縮小)
                                if (img_width > img_height) {
                                    bitmapImage.DecodePixelWidth = 1000;
                                } else {
                                    bitmapImage.DecodePixelHeight = 1000;
                                }

                            bitmapImage.EndInit();
                            bitmapImage.CacheOption = BitmapCacheOption.None;
                        }





                        //依照檔案類型來初始化
                        if (M.s_img_type_附檔名 == "PNG") {
                            encoder = new PngBitmapEncoder();
                        } else if (M.s_img_type_附檔名 == "GIF") {
                            encoder = new GifBitmapEncoder();
                        } else {
                            encoder = new JpegBitmapEncoder();
                        }

                        encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

                    } else {

                        encoder = new JpegBitmapEncoder();
                        try {

                            using (var im = M.c_影像.c_Magick.getImg(s_當前目錄, M.s_img_type_附檔名)) {
                                encoder.Frames.Add(BitmapFrame.Create(im.ToBitmapSource()));
                            }
                              

                        } catch  {
                            encoder.Frames.Add(BitmapFrame.Create(M.c_影像.BitmapToBitmapSource(M.c_影像.func_作業系統圖示(s_當前目錄))));


                        }
                       
                    }







                }






                string base64String = "";

                using (MemoryStream memoryStream = new MemoryStream()) {

                    encoder.Save(memoryStream);
                    byte[] temp = memoryStream.ToArray();

                    //Clipboard.SetDataObject(temp);

                    base64String = "data:image/jpeg;base64," + Convert.ToBase64String(temp);

                    fun_寫入搜圖input(s_type, base64String);

                }


                //清理圖片
                if (encoder.Frames.Count > 0 && encoder.Frames[0] != null)
                    encoder.Frames[0].Freeze();

                if (bitmapImage != null)
                    bitmapImage.Freeze();


                /*
                using (FileStream st = new FileStream("11.jpg", FileMode.Create)) {
                    byte[] temp = memoryStream.ToArray();
                    st.Write(temp, 0, temp.Length);
                }*/

            } catch (Exception e3) {

                MessageBox.Show(e3.ToString());

            }

        }





        private void fun_寫入搜圖input(String s_type, String img_s) {

            //儲存成txt
            using (FileStream fs = new FileStream(M.fun_執行檔路徑() + @"/data/graphSearch/input.txt", FileMode.Create)) {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8)) {
                    sw.WriteLine("type:" + s_type);
                    sw.Write("img:");
                    sw.Write(img_s);
                }
            }


            //執行()                   
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.FileName = M.fun_執行檔路徑() + "/data/graphSearch/graphSearch.exe";//執行檔路徑
            psi.UseShellExecute = false;
            psi.WorkingDirectory = M.fun_執行檔路徑() + "/data/graphSearch";//設定執行檔坐在的資料夾
            psi.CreateNoWindow = false;
            System.Diagnostics.Process.Start(psi);

        }







    }



}
