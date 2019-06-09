using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TiefSee.cs {

    
    public class C_書籤 {
        /*
        MainWindow M;


        List<data_書籤_資料夾> ar_書籤_資料夾 = new List<data_書籤_資料夾>();
        W_bookmarks w_bookmarks = null;


        public class data_書籤_項目 {
            public String path;
            public String name;
            public String type;
        }


        public class data_書籤_資料夾 {
            public List<data_書籤_項目> ar_書籤_項目 = new List<data_書籤_項目>();
            public String name;
        }



        public C_書籤(MainWindow m) {
            this.M = m;







        }





        /// <summary>
        /// 
        /// </summary>
        private void fun_初始化() {



            if (w_bookmarks != null) {
                return;
            }


            w_bookmarks = new W_bookmarks();
            w_bookmarks.Owner = M;
            //w_bookmarks.Show();



       






        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="but_書籤"></param>
        public void fun_顯示視窗(Button but_書籤) {


            fun_初始化();

            w_bookmarks.Width = 600;
            w_bookmarks.Height = 500;

            double d_x = but_書籤.PointToScreen(new Point(0, 0)).X - w_bookmarks.Width + but_書籤.ActualWidth;
            double d_y = but_書籤.PointToScreen(new Point(0, 0)).Y + but_書籤.ActualHeight;

            w_bookmarks.Left = d_x;
            w_bookmarks.Top = d_y;

            w_bookmarks.Visibility = Visibility.Visible;
         //   fun_更新界面();
        }


        /// <summary>
        /// 
        /// </summary>
        public void fun_關閉視窗() {
            w_bookmarks.Visibility = Visibility.Collapsed;
        }



        /// <summary>
        /// 
        /// </summary>
        public void fun_新增書籤() {


            if (ar_書籤_資料夾.Count == 0) {
                var data_資料夾 = new data_書籤_資料夾();
                data_資料夾.name = "Default";
                ar_書籤_資料夾.Add(data_資料夾);
            }


            if (M.bool_自定圖片名單 == false) {



                String s_當前目錄 = M.u_大量瀏覽模式.s_當前目錄;
                s_當前目錄 = s_當前目錄.Replace("\\", "/");
                s_當前目錄 = s_當前目錄.Substring(0, s_當前目錄.LastIndexOf("/"));
                if (s_當前目錄.IndexOf("/") < 0)//避免在C槽底下出現BUG
                    s_當前目錄 += "/";

                String s_預設名稱 = s_當前目錄;
                if (s_當前目錄.LastIndexOf("/") > -1)
                    s_預設名稱 = s_當前目錄.Substring(s_當前目錄.LastIndexOf("/") + 1);


                var data_item = new data_書籤_項目();
                data_item.path = s_當前目錄;
                data_item.type = "directory";
                data_item.name = s_預設名稱;
                ar_書籤_資料夾[0].ar_書籤_項目.Add(data_item);


                String path_儲存路徑 = fun_取得圖片雜湊路徑(s_當前目錄);
                String path_原圖 = null;


                List<String> ar_允許的附檔名 = new List<string>();
                ar_允許的附檔名.Add("JPG");
                ar_允許的附檔名.Add("JPEG");
                ar_允許的附檔名.Add("PNG");
                ar_允許的附檔名.Add("GIF");
                ar_允許的附檔名.Add("BMP");
                ar_允許的附檔名.Add("TIF");
                ar_允許的附檔名.Add("TIFF");


                foreach (var item in Directory.GetFiles(s_當前目錄, "*.*")) {
                    //從附檔名判斷
                    if (item.Length > 6) {
                        String x = item.Substring(item.Length - 7).ToUpper();
                        foreach (var item2 in ar_允許的附檔名) {
                            if (x.Substring(7 - item2.Length) == item2) {
                                path_原圖 = item;
                                break;
                            }
                        }
                        if (path_原圖 != null)
                            break;
                    }//if
                }


                fun_產生縮圖(path_原圖, path_儲存路徑);



            }


        }




        private String fun_取得圖片雜湊路徑(String img_path) {

            String path_儲存路徑 = M.fun_執行檔路徑() + "\\bookmarks\\image\\";

            //避免資料夾不存在
            if (Directory.Exists(path_儲存路徑) == false) {
                Directory.CreateDirectory(path_儲存路徑);
            }

            //產生雜湊圖片檔名
            var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            string resultSha1 = Convert.ToBase64String(sha1.ComputeHash(Encoding.Default.GetBytes(img_path))).Replace("\\", "").Replace("/", "");
            path_儲存路徑 += resultSha1 + ".jpg";

            return path_儲存路徑;
        }


        public void fun_儲存() {

        }



        public void fun_讀取() {


        }



 






        /// <summary>
        /// 
        /// </summary>
        private void fun_產生縮圖(String path, String path_儲存路徑) {



            //取得顯示範圍
            int box_w = 130;
            int box_h = 180;


            //取得原圖size      
            double img_原_w = 10;
            double img_原_h = 10;
            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))) {
                FileInfo fi = new FileInfo(path);
                byte[] bytes = reader.ReadBytes((int)fi.Length);
                MemoryStream ms = new MemoryStream(bytes);
                BitmapDecoder bd = BitmapDecoder.Create(ms, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
                reader.Close();
                reader.Dispose();

                BitmapSource bs = bd.Frames[0];
                img_原_w = bs.PixelWidth;
                img_原_h = bs.PixelHeight;
                bs.Freeze();//釋放
            }




            //縮小圖片
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.UriSource = new Uri(path);
            if (img_原_w / box_w > img_原_h / box_h) {
                bitmapImage.DecodePixelHeight = box_h;
            } else {
                bitmapImage.DecodePixelWidth = box_w;
            }
            bitmapImage.EndInit();


            ///-----


            //計算裁剪位置
            Int32Rect rect = new Int32Rect();
            rect.Width = box_w;
            rect.Height = box_h;
            if (img_原_w / box_w > img_原_h / box_h) {
                rect.X = (bitmapImage.PixelWidth - box_w) / 2;
                rect.Y = 0;
            } else {
                rect.X = 0;
                rect.Y = (bitmapImage.PixelHeight - box_h) / 2;
            }

            //裁剪
            var bsend = CutImage(bitmapImage, rect);

            //儲存成jpg
            var encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bsend));
            using (MemoryStream memoryStream = new MemoryStream()) {
                encoder.Save(memoryStream);
                byte[] temp = memoryStream.ToArray();
                FileStream BFile = new FileStream(path_儲存路徑, FileMode.Create); //開啟"D:\test.txt"          
                BFile.Write(temp, 0, temp.GetUpperBound(0) + 1);
                BFile.Flush();
                BFile.Close();
            }


        }



        /// <summary>
        /// 裁圖
        /// </summary>
        /// <param name="bitmapSource">圖源</param>
        /// <param name="cut">切割區域</param>
        /// <returns></returns>
        public BitmapSource CutImage(BitmapSource bitmapSource, Int32Rect cut) {
            //計算Stride
            var stride = bitmapSource.Format.BitsPerPixel * cut.Width / 8;
            //聲明字節數組
            byte[] data = new byte[cut.Height * stride];
            //調用CopyPixels
            bitmapSource.CopyPixels(cut, data, stride, 0);

            return BitmapSource.Create(cut.Width, cut.Height, 0, 0, PixelFormats.Bgr32, null, data, stride);
        }

    */

    }

}
