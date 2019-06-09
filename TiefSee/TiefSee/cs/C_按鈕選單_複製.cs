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
using System.Windows.Media.Imaging;

namespace TiefSee {



    public class C_按鈕選單_複製 {



        private MainWindow M;

        public U_menu u_menu_複製;

        U_menu_item propertyMenu_複製_png;
        U_menu_item propertyMenu_複製_svg;
        U_menu_item propertyMenu_複製_影像;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        public C_按鈕選單_複製(MainWindow m) {

            this.M = m;
            u_menu_複製 = new U_menu(m);

            m.but_複製.Click += (sender, e) => {

                func_隱藏不必要的選項();
                u_menu_複製.func_open(M.but_複製);

            };


            event_複製的選單();


        }



        /// <summary>
        /// 
        /// </summary>
        public void func_隱藏不必要的選項() {

            //如果是png才顯示專門【複製透明底】的複製選項
            if (M.s_img_type_附檔名 == "PNG" || M.s_img_type_附檔名 == "EXE" || M.s_img_type_附檔名 == "LNK" ||
                M.s_img_type_附檔名 == "APNG" || M.s_img_type_附檔名 == "PSD" || M.s_img_type_附檔名 == "PDF" ||
                M.s_img_type_附檔名 == "AI" || M.s_img_type_附檔名 == "WEBP" || M.s_img_type_附檔名 == "GIF" ||
                 M.s_img_type_附檔名 == "ICO") {
                propertyMenu_複製_png.Visibility = Visibility.Visible;
            } else {
                propertyMenu_複製_png.Visibility = Visibility.Collapsed;
            }

            //如果是SVG，就隱藏【複製影像】的選項，並顯示【複製文字】
            if (M.s_img_type_附檔名 == "SVG") {
                propertyMenu_複製_svg.Visibility = Visibility.Visible;
                //propertyMenu_複製_影像.Visibility = Visibility.Collapsed;
            } else {
                propertyMenu_複製_svg.Visibility = Visibility.Collapsed;
                //propertyMenu_複製_影像.Visibility = Visibility.Visible;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        private void event_複製的選單() {



            u_menu_複製.func_add_menu("複製 檔案", null, () => {
                String s_path = M.ar_path[M.int_目前圖片位置];
                //檔案或資料夾存在才複製
                if (File.Exists(s_path) || Directory.Exists(s_path)) {
                    var f = new System.Collections.Specialized.StringCollection();
                    f.Add(s_path);
                    Clipboard.SetFileDropList(f);
                }
            });



            u_menu_複製.func_add_menu("複製 檔名", null, () => {
                String s_path = M.ar_path[M.int_目前圖片位置];
                s_path = s_path.Substring(s_path.LastIndexOf("\\") + 1);
                try {
                    System.Windows.Forms.Clipboard.SetDataObject(
                    s_path, //text to store in clipboard
                    false,       //do not keep after our app exits
                    5,           //retry 5 times
                    200);        //200ms delay between retries
                } catch { }
            });
            u_menu_複製.func_add_menu("複製 完整路徑", null, () => {
                String s_path = M.ar_path[M.int_目前圖片位置];
                try {
                    System.Windows.Forms.Clipboard.SetDataObject(
                    s_path, //text to store in clipboard
                    false,       //do not keep after our app exits
                    5,           //retry 5 times
                    200);        //200ms delay between retries
                } catch { }
            });


            u_menu_複製.func_add_水平線();

            propertyMenu_複製_影像 = u_menu_複製.func_add_menu("複製 影像", null, () => {

                try {
                    fun_複製影像();
                } catch (Exception e2) {
                    MessageBox.Show(e2.ToString());
                }
            });




            propertyMenu_複製_png = u_menu_複製.func_add_menu("複製 PNG (低相容)", null, () => {
                String s_當前目錄 = M.ar_path[M.int_目前圖片位置];
                if (File.Exists(s_當前目錄)) {
                    try {
                        System.Drawing.Bitmap bm_transparent = null;
                        MemoryStream ms = new MemoryStream();


                        if (M.stackPlanel_動圖工具.Visibility == Visibility.Visible) {//如果是動圖，就直接抓目前幀的圖片來處理

                            BitmapEncoder enc = new PngBitmapEncoder();
                            enc.Frames.Add(BitmapFrame.Create(M.c_P網.fun_取得目前圖片()));
                            enc.Save(ms);

                        } else {

                            String s_附檔名 = M.s_img_type_附檔名;

                            if (s_附檔名 == "PSD" || s_附檔名 == "AI" || s_附檔名 == "PDF" || s_附檔名 == "WEBP") {

                                using (var mi = M.c_影像.c_Magick.getImg(s_當前目錄, s_附檔名)) {
                                    bm_transparent =mi.ToBitmap() ;
                                }

                            } else if (s_附檔名 == "EXE" || s_附檔名 == "LNK") {

                                bm_transparent = M.c_影像.BitmapFromSource(M.c_P網.fun_取得exe圖示(s_當前目錄, s_附檔名));

                            } else {

                                bm_transparent = new System.Drawing.Bitmap(s_當前目錄);

                            }

                            bm_transparent.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                        }


                        System.Windows.Forms.Clipboard.Clear();//先清理剪貼簿


                        System.Windows.Forms.IDataObject data_object = new System.Windows.Forms.DataObject();
                        data_object.SetData("PNG", false, ms);
                        System.Windows.Forms.Clipboard.SetDataObject(data_object, false);
                    } catch (Exception e2) {
                        MessageBox.Show(e2.ToString());
                    }
                }
            });








            propertyMenu_複製_svg = propertyMenu_複製_影像 = u_menu_複製.func_add_menu("複製 SVG (文字)", null, () => {

                String s_當前目錄 = M.ar_path[M.int_目前圖片位置];
                try {

                    if (File.Exists(s_當前目錄)) {
                        using (StreamReader sr = new StreamReader(s_當前目錄, Encoding.UTF8)) {

                            System.Windows.Forms.Clipboard.SetDataObject(sr.ReadToEnd(), false, 5, 200);
                        }
                    }

                } catch (Exception e2) {
                    MessageBox.Show(e2.ToString());
                }
            });



           propertyMenu_複製_影像 = u_menu_複製.func_add_menu("複製 base64", null, () => {

               func_複製_base64();
            });


            

        }







        /// <summary>
        /// 
        /// </summary>
        public void func_複製_base64() {

            try {

                String s_path = M.ar_path[M.int_目前圖片位置];

                byte[] temp = File.ReadAllBytes(s_path);
                string base64String = "";

                if (M.s_img_type_附檔名 == "PNG") {
                    base64String = "data:image/png;base64," + Convert.ToBase64String(temp);

                } else if (M.s_img_type_附檔名 == "GIF") {
                    base64String = "data:image/gif;base64," + Convert.ToBase64String(temp);

                } else if (M.s_img_type_附檔名 == "SVG") {
                    base64String = "data:image/svg+xml;base64," + Convert.ToBase64String(temp);

                } else if (M.s_img_type_附檔名 == "BMP") {
                    base64String = "data:image/bmp;base64," + Convert.ToBase64String(temp);

                } else {
                    base64String = "data:image/jpeg;base64," + Convert.ToBase64String(temp);
                }



                System.Windows.Forms.Clipboard.SetDataObject(base64String, false, 5, 200);//存入剪貼簿
            
            } catch (Exception e) {
                MessageBox.Show(e.ToString());
            }
        }




        /// <summary>
        /// 
        /// </summary>
        public void fun_複製影像() {

            /*
            //印出剪貼簿裡面所有的格式
            IDataObject data = Clipboard.GetDataObject();
            foreach (var item in data.GetFormats()) {
                System.Console.WriteLine("----------------------------++++ " + item);
            }*/




            BitmapSource bs = null;
            BitmapEncoder encoder = null;//可能是jpg或png或gif


            if (M.stackPlanel_動圖工具.Visibility == Visibility.Visible) {//動圖

                bs = M.c_P網.fun_取得目前圖片();
                if (bs != null) {
                    Clipboard.Clear();
                    Clipboard.SetImage(bs);
                }
            } /*else if (M.s_img_type_附檔名 == "SVG") {//SVG

                MessageBox.Show("SVG圖片 無法複製影像");

            }*/ /*else if (M.s_img_type_附檔名 == "PSD") {//

                String s_當前目錄 = M.ar_path[M.int_目前圖片位置];
                if (File.Exists(s_當前目錄)) {


                    try {
                        encoder = new PngBitmapEncoder();
                        BitmapDecoder bd = null;

                        //用串流方式讀取圖片（避免圖片因為資源被暫用導致無法刪除
                        using (BinaryReader reader = new BinaryReader(File.Open(s_當前目錄, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))) {
                            FileInfo fi = new FileInfo(s_當前目錄);
                            byte[] bytes = reader.ReadBytes((int)fi.Length);

                            MemoryStream ms = new MemoryStream(bytes);
                            bd = BitmapDecoder.Create(ms, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
                            reader.Close();
                            reader.Dispose();
                        }

                        //把圖片轉成PNG類型
                        encoder.Frames.Add(bd.Frames[0]);

                        //把PNG儲存回 BitmapDecoder ，這樣才能正常複製PSD檔案
                        using (MemoryStream memoryStream = new MemoryStream()) {
                            encoder.Save(memoryStream);
                            bd = BitmapDecoder.Create(memoryStream, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
                            Clipboard.Clear();
                            Clipboard.SetImage(bd.Frames[0]);
                        }

                        //釋放資源
                        encoder.Frames[0].Freeze();
                        bd.Frames[0].Freeze();

                    } catch (Exception) {


                        //如果作業系統內沒有PSD的解碼器，就改用程式內建的
                        SimplePsd.CPSD psd = new SimplePsd.CPSD();
                        psd.Load(s_當前目錄);
                        System.Drawing.Bitmap fromHbitmap = System.Drawing.Image.FromHbitmap(psd.GetHBitmap());
                        System.Windows.Forms.Clipboard.SetImage(fromHbitmap);


                    }




                }//if 檔案存在


            } */else if (M.s_img_type_顯示類型 == "MOVIE") {//MMOVIE

                //影片無法直接複製印象，所以用截圖的方式

                //如果影片超過視窗範圍，就先縮小影片
                if (M.scrollviewer_1.ScrollableHeight > 0 || M.scrollviewer_1.ScrollableWidth > 0) {

                    //全滿
                    M.fun_圖片全滿();

                    //50毫秒後重新執行這個函數
                    new Thread(() => {
                        Thread.Sleep(50);
                        C_adapter.fun_UI執行緒(() => {
                            fun_複製影像();
                        });
                    }).Start();

                    return;

                } else {

                    //圖片處於全滿狀狀態，使用截圖來複製影像
                    encoder = new PngBitmapEncoder();
                    var img56 = M.SaveTo(M.img_voide);
                    if (img56 != null) {
                        encoder.Frames.Add(BitmapFrame.Create(img56));
                        bs = encoder.Frames[0];
                        Clipboard.Clear();
                        Clipboard.SetImage(bs);
                        bs.Freeze();
                    } else {
                        MessageBox.Show("無法在影片旋轉後複製影像");
                    }
                }


            } else {//靜態圖

                //使用 Forms 的剪貼簿物件，使用WPF的話，PNG會因為剪貼簿無法儲存透明資訊，導致嚴重破圖
                try {

                    String s_當前目錄 = M.ar_path[M.int_目前圖片位置];
                    String ss = M.s_img_type_附檔名.ToUpper();

                    //一般格式
                    if (ss == "JPG" || ss == "JPEG" || ss == "BMP" || ss == "PNG" || ss == "GIF" || ss == "TIF") {

                        if (File.Exists(s_當前目錄)) {
                            System.Drawing.Bitmap bm_transparent = new System.Drawing.Bitmap(s_當前目錄);
                            System.Windows.Forms.Clipboard.SetImage(bm_transparent);
                            bm_transparent.Dispose();
                        }

                    } else {//特殊格式


                        System.Drawing.Bitmap bm_transparent = null;
                        try {
                    
                            using (var mi = M.c_影像.c_Magick.getImg(s_當前目錄, M.s_img_type_附檔名)) {
                                bm_transparent = mi.ToBitmap();
                            }

                        } catch (Exception) {
                            bm_transparent = M.c_影像.func_作業系統圖示(s_當前目錄);
                        }
                      
                        System.Windows.Forms.Clipboard.SetImage(bm_transparent);
                        bm_transparent.Dispose();

                    }

                } catch { }





            }//if





        }//function




    }



}
