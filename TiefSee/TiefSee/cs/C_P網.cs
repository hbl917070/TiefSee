using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml;
using static TiefSee.MainWindow;

namespace TiefSee.cs {


    public class C_P網 {



        MainWindow M;

        BitmapSource[] ar_img = new BitmapSource[0];//圖片集合
        List<int> ar_img_delay = new List<int>();//圖片的延遲
        List<String> ar_path;//用於輸出GIF
        private String s_輸入GIF路徑 = "";
        private int x = 0;
        private bool bool_暫停 = false;
        private bool bool_開始動作 = true;




        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        public C_P網(MainWindow m) {
            this.M = m;

            var thr = new Thread(fun_P網動圖);
            thr.Start();


            M.but_動圖_上一張.Click += (sender, e) => {
                fun_上一張圖片();
            };

            M.but_動圖_下一張.Click += (sender, e) => {
                fun_下一張圖片();
            };

            M.but_動圖_播放.Click += (sender, e) => {
                fun_開始動作(!bool_開始動作);
            };


            M.check_循環.Unchecked += (sender, e) => {
                fun_循環(false);
            };
            M.check_循環.Checked += (sender, e) => {
                fun_循環(true);
            };
        }


        /// <summary>
        /// 
        /// </summary>
        public void fun_上一張圖片() {
            fun_開始動作(false);
            x--;
            if (x < 0) {
                x = ar_img.Length - 1;
            }
            M.img.Source = ar_img[x];
            fun_顯示目前圖片張數();
        }


        /// <summary>
        /// 
        /// </summary>
        public void fun_下一張圖片() {
            fun_開始動作(false);
            x++;
            if (x >= ar_img.Length) {
                x = 0;
            }
            M.img.Source = ar_img[x];
            fun_顯示目前圖片張數();
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="bool_啟動"></param>
        private void fun_開始動作(bool bool_啟動) {

            this.bool_開始動作 = bool_啟動;

            if (bool_開始動作) {
                M.icon_播放.Visibility = Visibility.Collapsed;
                M.icon_暫停.Visibility = Visibility.Visible;
            } else {
                M.icon_播放.Visibility = Visibility.Visible;
                M.icon_暫停.Visibility = Visibility.Collapsed;
            }
        }




        /// <summary>
        /// 
        /// </summary>
        private void fun_顯示目前圖片張數() {
            M.text_動圖總數量.Text = (x + 1) + " / " + ar_img.Length;
            if (M.s_img_type_顯示類型 == "ICO" || M.s_img_type_顯示類型 == "EXE" || M.s_img_type_顯示類型 == "LNK") {
                M.fun_設定顯示圖片size(ar_img[x].PixelWidth, ar_img[x].PixelHeight);//顯示寬高
                M.text_圖片比例.Text = Math.Ceiling(((M.img.Width / M.int_img_w * 1.0f) * 100)) + "%";
            }
        }



        /// <summary>
        /// 
        /// </summary>
        private int fun_取得延遲(int int_目前) {

            int int_de = 60;
            bool bool_固定 = false;

            C_adapter.fun_UI執行緒(() => {
                bool_固定 = M.check_固定.IsChecked.Value;
            });

            if (bool_固定) {//固定轉換的延遲

                C_adapter.fun_UI執行緒(() => {
                    try {
                        int_de = Int32.Parse(M.text_延遲.Text);
                    } catch { }
                });

                if (int_de < 10) {//最低切換延遲
                    int_de = 10;
                }
                if (int_de > 5000) {//最高切換延遲
                    int_de = 5000;
                }

                return int_de;

            } else {//直接回傳從Json的延遲

                //取得上一針的延遲
                int xxx = int_目前 - 1;
                if (xxx >= ar_img_delay.Count) {
                    xxx = ar_img_delay.Count - 1;
                }
                if (xxx < 0) {
                    xxx = ar_img_delay.Count - 1;
                }
                if (xxx == -1) {
                    xxx = 0;
                }

                if (ar_img_delay.Count > 0)
                    return ar_img_delay[xxx];
                else
                    return 60;
            }

        }






        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public void fun_播放_zip(String path) {


            //如果是ZIP，就先把他解壓縮到暫存資料夾，然後才播放

            var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            string resultSha1 = Convert.ToBase64String(sha1.ComputeHash(Encoding.Default.GetBytes(path))).Replace("\\", "").Replace("/", "");

            String s_暫存路徑 = M.fun_執行檔路徑() + "\\data\\Temporary\\" + resultSha1 + "\\";//用檔名雜湊當做暫存資料夾的名稱


            if (Directory.Exists(s_暫存路徑) == false) {//資料夾不存在就新建
                Directory.CreateDirectory(s_暫存路徑);
            }





           

            //List<d_zip_item> ar_zip_item = UnZip(path, s_暫存路徑, null);

            //創建zip
            /*using (ZipOutputStream s = new ZipOutputStream(File.Create(@"C:\Users\WEN\Downloads\PxDownloader\つんでれ伯爵(14414)\xx.zip"))) {
                s.SetLevel(1);  //設置壓縮等級，等級越高壓縮效果越明顯，但佔用CPU也會更多

                foreach (String item in Directory.GetFiles(s_暫存路徑, "*.*")) {

                    using (FileStream fs = File.OpenRead(item)) {
                        byte[] buffer = new byte[4 * 1024];  //緩衝區，每次操作大小
                        ZipEntry entry = new ZipEntry(Path.GetFileName(Path.GetFileName(item)));     //創建壓縮包內的文件
                        entry.DateTime = DateTime.Now;  //文件創建時間
                        s.PutNextEntry(entry);          //將文件寫入壓縮包

                        int sourceBytes;
                        do {

                            sourceBytes = fs.Read(buffer, 0, buffer.Length);    //讀取文件內容(1次讀4M，寫4M)
                            s.Write(buffer, 0, sourceBytes);                    //將文件內容寫入壓縮相應的文件
                        } while (sourceBytes > 0);
                    }
                }

                s.CloseEntry();
            }*/



            //old
            using (var zip = ZipFile.OpenRead(path)) {

                foreach (ZipArchiveEntry item in zip.Entries) {
                    String x = item.FullName.Substring(item.FullName.Length - 4).ToUpper();
                    var st = item.Open();
                    if (x == ".JPG" || x == ".PNG" || x == ".GIF" || x == ".BMP" || x == "JPEG" || x == "JSON") {

                        String s_file_path = s_暫存路徑 + item.FullName;

                        if (File.Exists(s_file_path) == false)//檔案如果不存在，才進行解壓縮
                            item.ExtractToFile(s_file_path);

                    }
                }

            }
            fun_播放(s_暫存路徑);
          
            





            /*ar_img_delay = new List<int>();
            x = 0;
            //int_延遲 = 16;
            bool_暫停 = false;
            M.stackPlanel_動圖工具.Visibility = Visibility.Visible;
            fun_開始動作(true);
            

            ar_img = new BitmapSource[ar_zip_item.Count - 1];

            //MessageBox.Show("" + ar_img.Length);

            for (int i = 0; i < ar_img.Length; i++) {

                System.Console.WriteLine(ar_zip_item[i].path);

                String x = Path.GetExtension(ar_zip_item[i].name).ToUpper();

                if (x == ".JPG" || x == ".PNG" || x == ".GIF" || x == ".BMP" || x == "JPEG") {

                    ar_zip_item[i].st.Position = 0;

                    System.Console.WriteLine(ar_zip_item[i].st.Length);

                    using (var stream = ar_zip_item[i].st) {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = stream;
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        bitmap.Freeze();
                        ar_img[i] = bitmap;
                    }
                }

                int int_延遲 = 60;
                if (int_延遲 < 1)
                    int_延遲 = 60;
                if (int_延遲 < 10)
                    int_延遲 = 10;
                ar_img_delay.Add(int_延遲);
            }
            ar_zip_item = null;*/



            //顯示寬高
            var bitimg2 = ar_img[0];
            var img_width2 = (int)bitimg2.PixelWidth;
            var img_height2 = (int)bitimg2.PixelHeight;
            M.fun_設定顯示圖片size(img_width2, img_height2);
            M.fun_圖片全滿(true);


        }





        /*class d_zip_item {
            public MemoryStream st;
            public String path;
            public String name;

        }*/

        /// <summary>   
        /// 解壓功能   
        /// </summary>   
        /// <param name="fileToUnZip">待解壓的文件</param>   
        /// <param name="zipedFolder">指定解壓目標目錄</param>   
        /// <param name="password">密碼</param>   
        /// <returns>解壓結果</returns>   
        /*private List<d_zip_item> UnZip(string fileToUnZip, string zipedFolder, string password) {

            List<d_zip_item> ar_item = new List<d_zip_item>();

            bool result = true;
            FileStream fs = null;
            ZipInputStream zipStream = null;
            ZipEntry ent = null;
            string fileName;

            if (!File.Exists(fileToUnZip))
                return ar_item;

            if (!Directory.Exists(zipedFolder))
                Directory.CreateDirectory(zipedFolder);

            try {
                zipStream = new ZipInputStream(File.OpenRead(fileToUnZip.Trim()));
                if (!string.IsNullOrEmpty(password)) zipStream.Password = password;
                while ((ent = zipStream.GetNextEntry()) != null) {
                    if (!string.IsNullOrEmpty(ent.Name)) {
                        fileName = Path.Combine(zipedFolder, ent.Name);
                        fileName = fileName.Replace('/', '\\');

                        if (fileName.EndsWith("\\")) {
                            Directory.CreateDirectory(fileName);
                            continue;
                        }

                        //using (fs = File.Create(fileName)) {
                        //    int size = 2048;
                        //    byte[] data = new byte[size];
                        //    while (true) {
                        //        size = zipStream.Read(data, 0, data.Length);
                        //        if (size > 0) {
                        //            fs.Write(data, 0, size);
                        //        } else
                        //            break;
                        //    }
                        //}

                        MemoryStream str = new MemoryStream();
                        int size = 2048;
                        byte[] data = new byte[size];
                        while (true) {
                            size = zipStream.Read(data, 0, data.Length);
                            if (size > 0) {
                                str.Write(data, 0, size);
                            } else
                                break;
                        }

                        ar_item.Add(new d_zip_item {
                            name = Path.GetFileName(fileName),
                            path = fileName,
                            st = str
                        });


                    }
                }
            } catch {
                result = false;
            } finally {
                if (fs != null) {
                    fs.Close();
                    fs.Dispose();
                }
                if (zipStream != null) {
                    zipStream.Close();
                    zipStream.Dispose();
                }
                if (ent != null) {
                    ent = null;
                }
                //GC.Collect();
                //GC.Collect(1);
            }
            return ar_item;
        }*/


        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public void fun_播放(String s_path) {


            M.fun_清理記憶體();

            s_輸入GIF路徑 = s_path;//用於輸出GIF

            fun_解析json(s_path + "/animation.json");

            x = 0;
            //int_延遲 = 16;
            bool_暫停 = false;
            M.stackPlanel_動圖工具.Visibility = Visibility.Visible;
            fun_開始動作(true);

            FileInfo[] Files = new DirectoryInfo(s_path).GetFiles();//取得所有檔案

            ar_path = new List<string>();

            int L = Files.ToString().Length;
            for (int i = 0; i < Files.Length; i++) {
                String path_img = Files[i].FullName;
                if (path_img.Substring(path_img.Length - 4).ToUpper() == ".JPG") {
                    ar_path.Add(path_img);
                }
            }

            //排序
            var ar2 = ar_path.ToArray();
            Array.Sort(ar2, new Sort_自然排序_正());

            ar_img = new BitmapFrame[ar2.Length];
            for (int i = 0; i < ar2.Length; i++) {
                ar_img[i] = M.c_影像.func_get_BitmapImage_JPG(ar2[i]);
            }

            //立刻播放第一張圖片
            if (ar_img.Length >= 1)
                M.img.Source = ar_img[0];

            //隱藏exif視窗
            M.c_影像.c_exif.fun_初始化exif資訊();
            M.c_影像.c_exif.fun_顯示或隱藏視窗();

        }


        /// <summary>
        /// 
        /// </summary>
        public void fun_暫停() {

            bool_暫停 = true;
            ar_img = new BitmapFrame[0];//清空影像
            ar_img_delay = new List<int>();
            M.stackPlanel_動圖工具.Visibility = Visibility.Collapsed;//隱藏工具列
        }



        bool bool_循環 = false;
        int bool_循環播放方向 = 1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        private void fun_循環(bool b) {
            bool_循環 = b;
            if (b) {

            }
        }



        /// <summary>
        /// thread
        /// </summary>
        private void fun_P網動圖() {


            while (M.bool_程式運行中) {

                if (bool_暫停 == false && bool_開始動作)
                    C_adapter.fun_UI執行緒(() => {


                        if (ar_img.Length == 0) {

                        } else

                        if (ar_img.Length == 1) {
                            M.img.Source = ar_img[0];
                        } else


                        if (bool_循環) {

                            if (bool_暫停 == false && ar_img.Length > 0) {
                                if (x >= ar_img.Length) {
                                    x = ar_img.Length - 2;
                                    bool_循環播放方向 = -1;
                                }
                                if (x < 0) {
                                    x = 1;
                                    bool_循環播放方向 = 1;
                                }

                                M.img.Source = ar_img[x];
                                fun_顯示目前圖片張數();
                                x += bool_循環播放方向;
                            }


                        } else {

                            if (bool_暫停 == false && ar_img.Length > 0) {
                                if (x >= ar_img.Length) {
                                    x = 0;
                                }
                                if (x < 0) {
                                    x = 0;
                                }
                                M.img.Source = ar_img[x];
                                fun_顯示目前圖片張數();
                                x++;
                            }

                        }


                    });



                Thread.Sleep(fun_取得延遲(x));

            }//while

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        private void fun_解析json(String path) {

            ar_img_delay = new List<int>();

            try {

                //讀取json內容
                String s = "";
                using (StreamReader sr = new StreamReader(path, Encoding.UTF8)) {
                    s = sr.ReadToEnd();
                }

                var json = JObject.Parse(s);
                JToken frames = null;

                try {
                    frames = json["ugokuIllustFullscreenData"]["frames"];
                } catch (Exception) {

                    frames = json["ugokuIllustData"]["frames"];
                }




                foreach (JToken item in frames) {
                    ar_img_delay.Add(Int32.Parse(item["delay"] + ""));
                }

            } catch (Exception) {

                //如果無法解析，就一律當做60

            }


        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public void fun_播放_APNG(string path) {

            M.fun_清理記憶體();

            ar_img_delay = new List<int>();

            List<data_apng> ar_apng = M.c_apng.fun_取得解析後的APNG();

            x = 0;
            //int_延遲 = 16;
            bool_暫停 = false;
            M.stackPlanel_動圖工具.Visibility = Visibility.Visible;
            fun_開始動作(true);


            ar_img = new BitmapSource[ar_apng.Count];
            for (int i = 0; i < ar_apng.Count; i++) {
                ar_img[i] = ar_apng[i].img;
                int int_延遲 = ar_apng[i].int_延遲;
                if (int_延遲 < 1)
                    int_延遲 = 60;
                if (int_延遲 < 10)
                    int_延遲 = 10;
                ar_img_delay.Add(int_延遲);
            }
            ar_apng = null;


            //顯示寬高
            var bitimg2 = ar_img[0];
            var img_width2 = (int)bitimg2.PixelWidth;
            var img_height2 = (int)bitimg2.PixelHeight;
            M.fun_設定顯示圖片size(img_width2, img_height2);

            M.fun_圖片全滿(true);

            //隱藏exif視窗
            M.c_影像.c_exif.fun_初始化exif資訊();
            M.c_影像.c_exif.fun_顯示或隱藏視窗();
        }



        /// <summary>
        /// 判斷圖片是否為完全透明
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public bool IsTransparentPalette(System.Drawing.Bitmap bmp) {
            for (int i = 0; i < bmp.Width; i++) {
                for (int j = 0; j < bmp.Height; j++) {
                    System.Drawing.Color cl = bmp.GetPixel(i, j);
                    if (cl.A != 0) {
                        return false;
                    }
                }
            }
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public BitmapSource fun_取得ICO圖示(string path) {

            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))) {
                FileInfo fi = new FileInfo(path);
                byte[] bytes = reader.ReadBytes((int)fi.Length);

                MemoryStream ms = new MemoryStream(bytes);
                BitmapDecoder bd = BitmapDecoder.Create(ms, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
                reader.Close();
                reader.Dispose();

                if (bd.Frames.Count >= 1)
                    return bd.Frames[0];

            }

            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public BitmapSource fun_取得exe圖示(string path, String s_附檔名) {

            //如果是捷徑，就先抓取真實路徑
            if (s_附檔名 == "LNK") {
                path = cs.C_按鈕選單_其他程式開啟.ResolveShortcut(path);
                if (path == null) {//如果抓不到圖片，就直接顯示錯誤圖片
                    return null;
                }
            }

            //取得EXE的圖示
            var ar_exe = C_exe_img.fun_exe_img(path);
            if (ar_exe != null)
                foreach (var item in ar_exe) {
                    return M.c_影像.BitmapToBitmapSource(item);
                }

            return null;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public void fun_播放_EXE(string path) {

            ar_img_delay = new List<int>();

            x = 0;
            //int_延遲 = 16;
            bool_暫停 = false;
            M.stackPlanel_動圖工具.Visibility = Visibility.Visible;
            fun_開始動作(false);

            String path_真實路徑 = path;

            //如果是捷徑，就先抓取真實路徑
            if (M.s_img_type_附檔名 == "LNK") {
                path_真實路徑 = cs.C_按鈕選單_其他程式開啟.ResolveShortcut(path);
                if (path_真實路徑 == null) {//如果抓不到圖片，就直接顯示錯誤圖片

                    M.fun_顯示錯誤圖片(path);
                    return;
                }
            }


            List<BitmapSource> ar_BitmapSource = new List<BitmapSource>();


            //取得EXE的圖示
            var ar_exe = C_exe_img.fun_exe_img(path_真實路徑);
            if (ar_exe != null)
                foreach (var item in ar_exe) {
                    ar_BitmapSource.Add(M.c_影像.BitmapToBitmapSource(item));
                }

            //取得EXE的圖示      
            /*foreach (var item in IconHandler.IconHandler.IconsFromFile(path, IconHandler.IconSize.Large)) {
                var b = item.ToBitmap();
                if (IsTransparentPalette(b)==false)
                    ar_BitmapSource.Add(M.c_apng.BitmapToBitmapSource(b));
            }
            foreach (var item in IconHandler.IconHandler.IconsFromFile(path, IconHandler.IconSize.Small)) {
                var b = item.ToBitmap();
                if (IsTransparentPalette(b) == false)
                    ar_BitmapSource.Add(M.c_apng.BitmapToBitmapSource(b));
            }*/

            //存到全域變數
            ar_img = new BitmapSource[ar_BitmapSource.Count];
            for (int i = 0; i < ar_BitmapSource.Count; i++) {
                ar_img[i] = ar_BitmapSource[i];
                ar_img_delay.Add(500);
            }


            if (ar_img.Length > 0) {//至少有一張圖片才顯示

                M.img.Source = ar_img[x];//顯示圖片         
                M.fun_設定顯示圖片size(ar_img[0].PixelWidth, ar_img[0].PixelHeight); //顯示寬高
                fun_顯示目前圖片張數();//顯示總張數
                M.fun_圖片全滿(true);

            } else {

                M.fun_顯示錯誤圖片(path);

            }

            //隱藏exif視窗
            M.c_影像.c_exif.fun_初始化exif資訊();
            M.c_影像.c_exif.fun_顯示或隱藏視窗();

        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public void fun_播放_ICON(string path) {

            ar_img_delay = new List<int>();

            x = 0;
            //int_延遲 = 16;
            bool_暫停 = false;
            M.stackPlanel_動圖工具.Visibility = Visibility.Visible;
            fun_開始動作(false);

            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))) {
                FileInfo fi = new FileInfo(path);
                byte[] bytes = reader.ReadBytes((int)fi.Length);

                MemoryStream ms = new MemoryStream(bytes);
                BitmapDecoder bd = BitmapDecoder.Create(ms, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
                reader.Close();
                reader.Dispose();

                ar_img = new BitmapSource[bd.Frames.Count];
                for (int i = 0; i < bd.Frames.Count; i++) {
                    ar_img[i] = bd.Frames[i];
                    ar_img_delay.Add(500);
                }

            }

            M.img.Source = ar_img[x];//顯示圖片         
            M.fun_設定顯示圖片size(ar_img[0].PixelWidth, ar_img[0].PixelHeight); //顯示寬高
            fun_顯示目前圖片張數();//顯示總張數

            M.fun_圖片全滿(true);

            //隱藏exif視窗
            M.c_影像.c_exif.fun_初始化exif資訊();
            M.c_影像.c_exif.fun_顯示或隱藏視窗();

        }




        /// <summary>
        /// 用於『搜圖』
        /// </summary>
        /// <returns></returns>
        public BitmapSource fun_取得第一張圖片() {
            return ar_img[0];
        }


        /// <summary>
        /// 用於『複製』
        /// </summary>
        /// <returns></returns>
        public BitmapSource fun_取得目前圖片() {

            //避免超出陣列範圍
            int int_x = x;
            if (int_x < 0) {
                int_x = 0;
            }
            if (int_x >= ar_img.Length) {
                int_x = ar_img.Length - 1;
            }

            return ar_img[int_x];
        }









        /// <summary>
        /// 
        /// </summary>
        public void fun_輸出成GIF() {

            XmlTextWriter X = new XmlTextWriter(M.fun_執行檔路徑() + "/data/output_gif/input.xml", Encoding.UTF8);

            X.WriteStartDocument();//使用1.0版本
            X.Formatting = Formatting.Indented;//自動縮排
            X.Indentation = 2;//縮排距離

            X.WriteStartElement("settings");
            //----------------------------------------

            //輸入路徑
            X.WriteStartElement("input_path");
            X.WriteString(s_輸入GIF路徑);
            X.WriteEndElement();

            //輸出路徑：桌面+檔名+.gif
            string s_桌面 = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            String output_name = M.ar_path[M.int_目前圖片位置];
            output_name = output_name.Substring(output_name.LastIndexOf('\\') + 1);
            if (output_name.Length > 4 && output_name.Substring(output_name.Length - 4, 4).ToUpper() == ".ZIP") {
                output_name = output_name.Substring(0, output_name.Length - 4);
            }
            if (output_name.Length > 6 && output_name.Substring(output_name.Length - 6, 6).ToUpper() == ".PIXIV") {
                output_name = output_name.Substring(0, output_name.Length - 6);
            }
            output_name = s_桌面 + "\\" + output_name + ".gif";
            X.WriteStartElement("output_path");
            X.WriteString(output_name);
            X.WriteEndElement();

            //圖片寬度
            X.WriteStartElement("width");
            X.WriteString(M.int_img_w + "");
            X.WriteEndElement();

            //圖片高度
            X.WriteStartElement("height");
            X.WriteString(M.int_img_h + "");
            X.WriteEndElement();


            //圖片集合
            X.WriteStartElement("img");
            for (int i = 0; i < ar_img.Length; i++) {
                X.WriteStartElement("item");
                X.WriteAttributeString("delay", fun_取得延遲(i + 1) + "");
                X.WriteAttributeString("path", ar_path[i]);
                X.WriteString("");
                X.WriteEndElement();
            }
            if (M.check_循環.IsChecked.Value) {
                for (int i = ar_img.Length - 2; i >= 1; i--) {
                    X.WriteStartElement("item");
                    X.WriteAttributeString("delay", fun_取得延遲(i + 1) + "");
                    X.WriteAttributeString("path", ar_path[i]);
                    X.WriteString("");
                    X.WriteEndElement();
                }
            }
            X.WriteEndElement();



            //----------------------------------------
            X.WriteEndElement();

            X.Flush();     //寫這行才會寫入檔案
            X.Close();
            X.Dispose();


            //-------------------------

            //執行()                   
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.FileName = M.fun_執行檔路徑() + "/data/output_gif/output_gif.exe";//執行檔路徑
            psi.UseShellExecute = false;
            psi.WorkingDirectory = M.fun_執行檔路徑() + "/data/output_gif";//設定執行檔所在的資料夾
            psi.CreateNoWindow = false;
            System.Diagnostics.Process.Start(psi);


        }




    }
}
