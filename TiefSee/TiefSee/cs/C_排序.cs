using TiefSee.W;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace TiefSee {
    public class C_排序 {



        MainWindow M;
        U_menu_item[] ar_u_item_檔名;
        U_menu_item[] ar_u_item_資料夾;

        enum_檔案排序方式 e_檔案排序方式;
        enum_資料夾排序方式 e_資料夾排序方式;


        public C_排序(MainWindow m) {
            this.M = m;


            func_讀取排序初始化();


            M.popup_選單_排序.StaysOpen = false;


            M.u_menu_排序_檔案_檔名_順.t01.Text = "檔名";
            M.u_menu_排序_檔案_檔名_反.t01.Text = "檔名 (反)";
            M.u_menu_排序_檔案_日期_順.t01.Text = "修改日期";
            M.u_menu_排序_檔案_日期_反.t01.Text = "修改日期 (反)";

            M.u_menu_排序_資料夾_檔名_順.t01.Text = "檔名";
            M.u_menu_排序_資料夾_檔名_反.t01.Text = "檔名 (反)";
            M.u_menu_排序_資料夾_日期_順.t01.Text = "修改日期";
            M.u_menu_排序_資料夾_日期_反.t01.Text = "修改日期 (反)";

            ar_u_item_檔名 = new U_menu_item[]{
                M.u_menu_排序_檔案_檔名_順,
                M.u_menu_排序_檔案_檔名_反,
                M.u_menu_排序_檔案_日期_順,
                M.u_menu_排序_檔案_日期_反
            };

            ar_u_item_資料夾 = new U_menu_item[]{
                M.u_menu_排序_資料夾_檔名_順,
                M.u_menu_排序_資料夾_檔名_反,
                M.u_menu_排序_資料夾_日期_順,
                M.u_menu_排序_資料夾_日期_反
            };


            //檔案
            for (int i = 0; i < ar_u_item_檔名.Length; i++) {

                var item = ar_u_item_檔名[i];
                item.but01.Click += (sender, e) => {

                    //修改排序模式
                    func_設定檔案排序模式(item);

                    //重新取得名單
                    String path = M.ar_path[M.int_目前圖片位置];
                    M.ar_path = new List<string>();
                    M.ar_path = M.fun_取得圖片名單(path);

                    //修改目前位置
                    M.int_目前圖片位置 = 0;
                    for (int k = 0; k < M.ar_path.Count; k++) {
                        if (M.ar_path[k] == path) {
                            M.int_目前圖片位置 = k;
                            break;
                        }
                    }

                    //記錄到order.txt
                    func_新增儲存排序_file(M.func_取得目前資料夾路徑());
                };
            }
            func_設定檔案排序模式(M.u_menu_排序_檔案_檔名_順);




            //資料夾
            for (int i = 0; i < ar_u_item_資料夾.Length; i++) {

                var item = ar_u_item_資料夾[i];
                item.but01.Click += (sender, e) => {

                    //修改排序模式
                    func_設定資料夾排序模式(item);

                    //記錄到order.txt
                    func_新增儲存排序_dir(M.func_取得目前資料夾路徑());
                };
            }
            func_設定資料夾排序模式(M.u_menu_排序_資料夾_檔名_順);







        }



        #region 記錄排序



        public Dictionary<String, enum_檔案排序方式> dic_order_file = new Dictionary<String, enum_檔案排序方式>();
        public Dictionary<String, enum_資料夾排序方式> dic_order_dir = new Dictionary<String, enum_資料夾排序方式>();

        private String s_path_排序資料;
        private Boolean bool_需要儲存排序 = false;





        /// <summary>
        /// 初始化class是呼叫，每個新視窗只會執行一次
        /// </summary>
        public void func_讀取排序初始化() {
            s_path_排序資料 = M.fun_執行檔路徑() + "\\data\\order.txt";
            func_讀取排序();
        }



        /// <summary>
        /// 從檔案讀取排序順序
        /// </summary>
        public void func_讀取排序() {

            //避免檔案不存在
            if (File.Exists(s_path_排序資料) == false) {
                using (FileStream fs = new FileStream(s_path_排序資料, FileMode.Create)) {
                }
            }

            using (StreamReader sr = new StreamReader(s_path_排序資料, Encoding.UTF8)) {

                String line;
                while ((line = sr.ReadLine()) != null) {

                    if (line.Length < 5)
                        continue;

                    String[] ar_65 = line.Split('\t');

                    if (ar_65.Length < 2)
                        continue;


                    try {
                        String s1 = ar_65[0].Trim();//類型
                        String s2 = ar_65[1].Trim();//排序模式
                        String s3 = ar_65[2].Trim();//路徑

                        if (s1 == "file") {

                            var enum_file = (enum_檔案排序方式)Int32.Parse(s2);
                            if (dic_order_file.ContainsKey(s3)) {
                                dic_order_file[s3] = enum_file;
                            } else {
                                dic_order_file.Add(s3, enum_file);
                            }

                        } else if (s1 == "dir") {

                            var enum_dir = (enum_資料夾排序方式)Int32.Parse(s2);
                            if (dic_order_dir.ContainsKey(s3)) {
                                dic_order_dir[s3] = enum_dir;
                            } else {
                                dic_order_dir.Add(s3, enum_dir);
                            }

                        }

                    } catch { }






                }
            }

        }


        /// <summary>
        /// 載入資料夾後呼叫
        /// </summary>
        /// <param name="path"></param>
        public void func_判斷是否已設定過排序(String path) {

            //避免錯誤的路徑
            if (File.Exists(path) || Directory.Exists(path)) {         

            } else {
                //預設值
                func_設定檔案排序模式(M.u_menu_排序_檔案_檔名_順);
                func_設定資料夾排序模式(M.u_menu_排序_資料夾_檔名_順);
                return;
            }
         
            path = path.ToLower();

         
            if (dic_order_file.ContainsKey(path)) {

                //使用陣列來轉換對應的物件
                var dic_檔案排序方式 = new Dictionary<enum_檔案排序方式, U_menu_item>();
                dic_檔案排序方式.Add(enum_檔案排序方式.檔名_順, M.u_menu_排序_檔案_檔名_順);
                dic_檔案排序方式.Add(enum_檔案排序方式.檔名_反, M.u_menu_排序_檔案_檔名_反);
                dic_檔案排序方式.Add(enum_檔案排序方式.修改日期_順, M.u_menu_排序_檔案_日期_順);
                dic_檔案排序方式.Add(enum_檔案排序方式.修改日期_反, M.u_menu_排序_檔案_日期_反);

                //設定排序方式，並且同步更新UI
                func_設定檔案排序模式(dic_檔案排序方式[dic_order_file[path]]);

            } else {   
                //預設值
                func_設定檔案排序模式(M.u_menu_排序_檔案_檔名_順);
            }



           

            try {

                //避免在根目錄的時候出錯
                if (path == Path.GetPathRoot(path).ToLower()) {
                    //預設值
                    func_設定資料夾排序模式(M.u_menu_排序_資料夾_檔名_順);
                    return;
                }

                String path_dir = Path.GetDirectoryName(path).ToLower();//父路徑，用於資料夾排序

                if (path_dir != null && path_dir != "") {
                    if (dic_order_dir.ContainsKey(path_dir)) {

                        //使用陣列來轉換對應的物件
                        var dic_資料夾排序方式 = new Dictionary<enum_資料夾排序方式, U_menu_item>();
                        dic_資料夾排序方式.Add(enum_資料夾排序方式.檔名_順, M.u_menu_排序_資料夾_檔名_順);
                        dic_資料夾排序方式.Add(enum_資料夾排序方式.檔名_反, M.u_menu_排序_資料夾_檔名_反);
                        dic_資料夾排序方式.Add(enum_資料夾排序方式.修改日期_順, M.u_menu_排序_資料夾_日期_順);
                        dic_資料夾排序方式.Add(enum_資料夾排序方式.修改日期_反, M.u_menu_排序_資料夾_日期_反);

                        //設定排序方式，並且同步更新UI
                        func_設定資料夾排序模式(dic_資料夾排序方式[dic_order_dir[path_dir]]);

                    } else {
                        //預設值
                        func_設定資料夾排序模式(M.u_menu_排序_資料夾_檔名_順);
                    }
                }

            } catch {
                //預設值
                func_設定資料夾排序模式(M.u_menu_排序_資料夾_檔名_順);
            }
            

        }



        /// <summary>
        /// 修改完排序模式後呼叫
        /// </summary>
        /// <param name="path"></param>
        public void func_新增儲存排序_file(String path) {

            //避免錯誤的路徑
            if (File.Exists(path) || Directory.Exists(path)) {

            } else {
                return;
            }

            //自定路徑就不儲存排序方式
            if (M.bool_自定圖片名單 == true) {
                return;
            }

            bool_需要儲存排序 = true;//如果都沒修改過排序，關閉程式時就不處理排序的儲存

            path = path.ToLower();

            if (dic_order_file.ContainsKey(path)) {
                dic_order_file[path] = e_檔案排序方式;
            } else {
                dic_order_file.Add(path, e_檔案排序方式);
            }

            //避免檔案不存在
            if (File.Exists(s_path_排序資料) == false) {
                using (FileStream fs = new FileStream(s_path_排序資料, FileMode.Create)) {
                }
            }

            using (StreamWriter sw = new StreamWriter(s_path_排序資料, true)) {
                String s = "file" + "\t" + ((int)e_檔案排序方式) + "\t" + path;
                sw.WriteLine(s);
            }


        }

        /// <summary>
        /// 修改完排序模式後呼叫
        /// </summary>
        /// <param name="path"></param>
        public void func_新增儲存排序_dir(String path) {

            //避免錯誤的路徑
            if (File.Exists(path) || Directory.Exists(path)) {

            } else {
                return;
            }

            //自定路徑就不儲存排序方式
            if (M.bool_自定圖片名單 == true) {
                return;
            }


            bool_需要儲存排序 = true;//如果都沒修改過排序，關閉程式時就不處理排序的儲存

            String path_dir = Path.GetDirectoryName(path.ToLower());//父路徑，用於資料夾排序

            if (path_dir == null || path_dir == "")
                return;

            if (dic_order_dir.ContainsKey(path_dir)) {
                dic_order_dir[path_dir] = e_資料夾排序方式;
            } else {
                dic_order_dir.Add(path_dir, e_資料夾排序方式);
            }

            //避免檔案不存在
            if (File.Exists(s_path_排序資料) == false) {
                using (FileStream fs = new FileStream(s_path_排序資料, FileMode.Create)) {
                }
            }

            using (StreamWriter sw = new StreamWriter(s_path_排序資料, true)) {
                String s2 = "dir" + "\t" + ((int)e_資料夾排序方式) + "\t" + path_dir;
                sw.WriteLine(s2);
            }


        }


        /// <summary>
        /// 程式結束時呼叫
        /// </summary>
        public void func_儲存與優化排序() {

            //如果完全沒有修改過排序，就不儲存
            if (bool_需要儲存排序 == false)
                return;

            //讀取
            func_讀取排序();

            //避免檔案不存在
            if (File.Exists(s_path_排序資料) == false) {
                using (FileStream fs = new FileStream(s_path_排序資料, FileMode.Create)) {
                }
            }

            using (FileStream fs = new FileStream(s_path_排序資料, FileMode.Create)) {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8)) {

                    foreach (String key in dic_order_file.Keys) {
                        String s = "file" + "\t" + ((int)dic_order_file[key]) + "\t" + key;
                        sw.WriteLine(s);
                    }

                    foreach (String key in dic_order_dir.Keys) {
                        String s = "dir" + "\t" + ((int)dic_order_dir[key]) + "\t" + key;
                        sw.WriteLine(s);
                    }

                }
            }


        }



        #endregion






        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void func_設定檔案排序模式(U_menu_item item) {


            for (int j = 0; j < ar_u_item_檔名.Length; j++) {
                var item2 = ar_u_item_檔名[j];
                item2.img01.Source = new BitmapImage();
            }

            if (item == M.u_menu_排序_檔案_檔名_順) {
                e_檔案排序方式 = enum_檔案排序方式.檔名_順;

            } else if (item == M.u_menu_排序_檔案_檔名_反) {
                e_檔案排序方式 = enum_檔案排序方式.檔名_反;

            } else if (item == M.u_menu_排序_檔案_日期_順) {
                e_檔案排序方式 = enum_檔案排序方式.修改日期_順;

            } else if (item == M.u_menu_排序_檔案_日期_反) {
                e_檔案排序方式 = enum_檔案排序方式.修改日期_反;

            }

            item.img01.Source = new BitmapImage(new Uri("/imgs/yes.png", UriKind.Relative));

            if (M.u_大量瀏覽模式 != null) {
                M.u_大量瀏覽模式.func_重新載入圖片();
            }

            
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void func_設定資料夾排序模式(U_menu_item item) {


            for (int j = 0; j < ar_u_item_資料夾.Length; j++) {
                var item2 = ar_u_item_資料夾[j];
                item2.img01.Source = new BitmapImage();
            }

            if (item == M.u_menu_排序_資料夾_檔名_順) {
                e_資料夾排序方式 = enum_資料夾排序方式.檔名_順;

            } else if (item == M.u_menu_排序_資料夾_檔名_反) {
                e_資料夾排序方式 = enum_資料夾排序方式.檔名_反;

            } else if (item == M.u_menu_排序_資料夾_日期_順) {
                e_資料夾排序方式 = enum_資料夾排序方式.修改日期_順;

            } else if (item == M.u_menu_排序_資料夾_日期_反) {
                e_資料夾排序方式 = enum_資料夾排序方式.修改日期_反;

            }

            item.img01.Source = new BitmapImage(new Uri("/imgs/yes.png", UriKind.Relative));

        }


        /// <summary>
        /// 
        /// </summary>
        public void func_開啟選單_滑鼠下方() {

            var popup_選單 = M.popup_選單_排序;
            var popup_選單_容器 = M.popup_選單_容器_排序;
            var but = M.but_menu_main_排序;

            MainWindow.fun_動畫(popup_選單_容器, -10, 0, "Y", () => { });
            popup_選單.Placement = System.Windows.Controls.Primitives.PlacementMode.Absolute;
            popup_選單.IsOpen = true;

            var pos = System.Windows.Forms.Cursor.Position;

            popup_選單.HorizontalOffset = pos.X / M.d_解析度比例_x - (popup_選單_容器.ActualWidth / 2);
            popup_選單.VerticalOffset = pos.Y / M.d_解析度比例_y - 35;

        }


        /// <summary>
        /// 
        /// </summary>
        public void func_開啟選單_物件下方(FrameworkElement obj_but) {

            var popup_選單 = M.popup_選單_排序;
            var popup_選單_容器 = M.popup_選單_容器_排序;
            var but = M.but_menu_main_排序;

            MainWindow.fun_動畫(popup_選單_容器, -10, 0, "Y", () => { });
            popup_選單.PlacementTarget = obj_but;
            popup_選單.Placement = System.Windows.Controls.Primitives.PlacementMode.Center;
            popup_選單.IsOpen = true;

            var pos = System.Windows.Forms.Cursor.Position;

            popup_選單.HorizontalOffset = 0;
            popup_選單.VerticalOffset = (popup_選單_容器.ActualHeight / M.d_解析度比例_y / 2) + (obj_but.ActualHeight / 2) - 15;

        }


        public enum enum_檔案排序方式 {

            檔名_順 = 1,
            檔名_反 = 2,
            修改日期_順 = 11,
            修改日期_反 = 12

        }

        public enum enum_資料夾排序方式 {

            檔名_順 = 1,
            檔名_反 = 2,
            修改日期_順 = 11,
            修改日期_反 = 12

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ar"></param>
        /// <returns></returns>
        public String[] func_檔案排序(List<String> ar) {

            var ar2 = ar.ToArray();

            if (e_檔案排序方式 == enum_檔案排序方式.檔名_順) {
                Array.Sort(ar2, new Sort_自然排序_正());

            } else if (e_檔案排序方式 == enum_檔案排序方式.檔名_反) {
                Array.Sort(ar2, new Sort_自然排序_反());

            } else if (e_檔案排序方式 == enum_檔案排序方式.修改日期_順) {
                ar2 = func_儲存時間(ar2, true);

            } else if (e_檔案排序方式 == enum_檔案排序方式.修改日期_反) {
                ar2 = func_儲存時間(ar2, false);

            }

            return ar2;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="ar"></param>
        /// <returns></returns>
        public String[] func_資料夾排序(String[] ar2) {

            //var ar2 = ar.ToArray();

            if (e_資料夾排序方式 == enum_資料夾排序方式.檔名_順) {
                Array.Sort(ar2, new Sort_自然排序_正());

            } else if (e_資料夾排序方式 == enum_資料夾排序方式.檔名_反) {
                Array.Sort(ar2, new Sort_自然排序_反());

            } else if (e_資料夾排序方式 == enum_資料夾排序方式.修改日期_順) {
                ar2 = func_儲存時間(ar2, true);

            } else if (e_資料夾排序方式 == enum_資料夾排序方式.修改日期_反) {
                ar2 = func_儲存時間(ar2, false);

            }

            return ar2;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="ar"></param>
        /// <returns></returns>
        public String[] func_儲存時間(String[] ar, Boolean bool_遞增) {


            var list_檔案 = new List<c_檔案資料>();
            for (int i = 0; i < ar.Length; i++) {
                list_檔案.Add(new c_檔案資料 {
                    s_path = ar[i],
                    date_儲存時間 = File.GetLastWriteTime(ar[i])
                });
            }

            fun_排序_時間(list_檔案, bool_遞增);

            var ar2 = new String[list_檔案.Count];
            for (int i = 0; i < list_檔案.Count; i++) {
                ar2[i] = list_檔案[i].s_path;
            }

            return ar2;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="list_檔案"></param>
        /// <param name="bool_遞增"></param>
        private void fun_排序_時間(List<c_檔案資料> list_檔案, Boolean bool_遞增) {
            for (int i = 0; i < list_檔案.Count; i++)
                if (bool_遞增 == true) {
                    for (int j = i; j < list_檔案.Count; j++)
                        if (list_檔案[i].date_儲存時間 < list_檔案[j].date_儲存時間) {
                            var d = list_檔案[i];
                            list_檔案[i] = list_檔案[j];
                            list_檔案[j] = d;
                        }
                } else {
                    for (int j = i; j < list_檔案.Count; j++)
                        if (list_檔案[i].date_儲存時間 > list_檔案[j].date_儲存時間) {
                            var d = list_檔案[i];
                            list_檔案[i] = list_檔案[j];
                            list_檔案[j] = d;
                        }
                }
        }



    }


    class c_檔案資料 {
        public DateTime date_儲存時間;
        public String s_path = "";


        public c_檔案資料() {

        }


    }



    /// <summary>
    /// 檔案排序（自然排序）
    /// </summary>
    public class Sort_自然排序_正 : IComparer<string> {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        static extern int StrCmpLogicalW(String x, String y);
        public int Compare(string x, string y) {
            return StrCmpLogicalW(x, y);
        }
    }

    /// <summary>
    /// 檔案排序（自然排序）
    /// </summary>
    public class Sort_自然排序_反 : IComparer<string> {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        static extern int StrCmpLogicalW(String x, String y);
        public int Compare(string x, string y) {
            return StrCmpLogicalW(y, x);
        }
    }




}



