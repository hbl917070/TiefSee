using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TiefSee_快速啟動 {
    class Program {
        static void Main(string[] args) {


            /*
            Console.OutputEncoding = UTF8Encoding.UTF8;
            String uri = (@"C:\Users\WEN\Desktop\[COM3D対応]_ヴァイオレット・エヴァーガーデン%E3%80%80髪型\eagle-large.jpg");
            String u2 = Uri.EscapeDataString(uri);
            String u3 = Uri.UnescapeDataString(u2);
            System.Console.WriteLine(uri);
            System.Console.WriteLine(u3);
            System.Console.WriteLine(u2);


            byte[] bytes = Encoding.UTF8.GetBytes(uri);
            string base64 = Convert.ToBase64String(bytes);
            Console.WriteLine(base64);

        
            byte[] bytes2 = Convert.FromBase64String(base64);
            string str = Encoding.UTF8.GetString(bytes2);
            Console.WriteLine(str);

            string x = Console.ReadLine();
            */




            var c1 = new C1();
            var bool_無延遲啟動 = c1.func_無延遲開啟(args);

            //如果沒有辦法無延遲啟動，就直接開啟
            if (bool_無延遲啟動 == false) {
                c1.func_直接開啟(args);
            }



            //string x = Console.ReadLine();


        }
    }





    class C1 {

        /*
        String base64ToString(String base64) {
            byte[] bytes2 = Convert.FromBase64String(base64);
            string str = Encoding.UTF8.GetString(bytes2);
            return str;
        }

        String stringToBase64(String s) {
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            string base64 = Convert.ToBase64String(bytes);
            return base64;
        }*/


        /// <summary>
        /// 取得執行目標路徑
        /// </summary>
        /// <returns></returns>
        private String func_取得執行目標路徑() {

            bool b_64 = Environment.Is64BitOperatingSystem;//判斷電腦是否為64位元

            String s_path = "";
            if (b_64) {

                s_path = System.AppDomain.CurrentDomain.BaseDirectory + "\\app\\TiefSee_64.exe";
                if (File.Exists(s_path)) {
                    return s_path;
                }

                s_path = System.AppDomain.CurrentDomain.BaseDirectory + "\\app\\AeroPic_64.exe";
                if (File.Exists(s_path)) {
                    return s_path;
                }

            } else {

                s_path = System.AppDomain.CurrentDomain.BaseDirectory + "\\app\\TiefSee_32.exe";
                if (File.Exists(s_path)) {
                    return s_path;
                }


                s_path = System.AppDomain.CurrentDomain.BaseDirectory + "\\app\\AeroPic_32.exe";
                if (File.Exists(s_path)) {
                    return s_path;
                }

            }


            s_path = System.AppDomain.CurrentDomain.BaseDirectory + "\\app\\TiefSee.exe";
            if (File.Exists(s_path)) {
                return s_path;
            }


            s_path = System.AppDomain.CurrentDomain.BaseDirectory + "\\app\\AeroPic.exe";
            if (File.Exists(s_path)) {
                return s_path;
            }


            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public void func_直接開啟(string[] args) {

            String s_path = func_取得執行目標路徑();

            if (s_path == null) {
                return;
            }

            String sum = "";
            foreach (String item in args) {
                sum += "\"" + item + "\"" + " ";
            }




            using (System.Diagnostics.Process Info = new System.Diagnostics.Process()) {
                Info.StartInfo.FileName = s_path;  //要啟動的應用程式          
                Info.StartInfo.Arguments = sum; //該應用程式的指令

                Info.StartInfo.WorkingDirectory = System.AppDomain.CurrentDomain.BaseDirectory;//設定預設鎖定的目錄，沒有設定這個的話，開啟圖片後會不會能刪除資料夾

                Info.Start();
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool func_無延遲開啟(string[] main_s) {


            //如果檔案大於50M，就正常開啟，不要用無延遲開啟
            if (main_s.Length > 0) {
                if (File.Exists(main_s[0])) {

                    var len = new FileInfo(main_s[0]).Length;
                    len = (len / (1024 * 1024));

                    if (len > 50)
                        return false;
                }
            }





            //新開視窗的話，立即啟動

            //取得目前是否有其他開啟的aeropic
            // var ar_Process = Process.GetProcessesByName(System.Diagnostics.Process.GetCurrentProcess().ProcessName);
            //DateTime time_start = DateTime.Now;//計時開始 取得目前時間
            //uri = "http://localhost:55111/3ZEAKQ39UCTVD23FXQHFUB0NVVU/1/open_img/1";


            String s023 = System.AppDomain.CurrentDomain.BaseDirectory + "\\app\\data\\port";
            //s023 = @"D:\軟體\AeroPic\app\data\port";

            //System.Console.WriteLine(s023);



            if (Directory.Exists(s023)) {

                var ar2 = Directory.GetFiles(s023, "*");
                String[] ar = null;

                //直接使用第一個跟最後一個port
                if (ar2.Length >= 1) {
                    ar = new string[] { ar2[0], ar2[ar2.Length - 1] };
                } else if (ar2.Length == 0) {
                    return false;
                }

                for (int j = 0; j < ar.Length; j++) {//判斷目前已經開啟的視窗


                    try {

                        //取得記憶體用量
                        String s = "";
                        String uri = "http://localhost:" + Path.GetFileName(ar[j]) + "/get_memory";
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                        request.Timeout = 300;
                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                            using (Stream stream = response.GetResponseStream()) {
                                using (StreamReader reader = new StreamReader(stream)) {
                                    s = reader.ReadToEnd();
                                }
                            }
                        }

                        //記憶體用量低於350M，就無延遲開啟視窗
                        if (float.Parse(s) < 400 && float.Parse(s) > 1) {

                            var sb = new System.Text.StringBuilder();
                            for (int i = 0; i < main_s.Length; i++) {
                                if (i != main_s.Length - 1) {
                                    sb.Append(main_s[i] + "\n");
                                } else {
                                    sb.Append(main_s[i]);
                                }
                            }

                            uri = "http://localhost:" + Path.GetFileName(ar[j]) + "/new_window/" +
                                    Uri.EscapeDataString(sb.ToString());

                            HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(uri);
                            using (HttpWebResponse response = (HttpWebResponse)request2.GetResponse()) {
                            }



                            return true;
                        } else {

                        }

                    } catch { }
                }//foreacj
            }//if 



            return false;
        }







    }

}
