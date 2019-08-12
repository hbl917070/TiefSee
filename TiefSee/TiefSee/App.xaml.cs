using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;

namespace TiefSee {
    /// <summary>
    /// App.xaml 的互動邏輯
    /// </summary>
    public partial class App : Application {


        public static String[] main_s = { };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Application_Startup(object sender, StartupEventArgs e) {

         

            main_s = new string[] { };
           
            if (e.Args.Length > 0) {


                main_s = e.Args;
          
            }

        
            //--------------------


            //關閉所有視窗
            var b1 = func_全部關閉(e);
            if (b1)
                return;


            //無延遲開啟
            /*var b2 = func_無延遲開啟(e);
            if (b2)
                return;*/

            //--------------------


            //取得目前是否有其他開啟的 TiefSee
            Process[] ar_Process = Process.GetProcessesByName(System.Diagnostics.Process.GetCurrentProcess().ProcessName);

            //System.Console.WriteLine("**"+ System.Diagnostics.Process.GetCurrentProcess().ProcessName);
            //System.Console.WriteLine("**" + ar_Process.Length);

            String s023 = Path.Combine(System.Windows.Forms.Application.StartupPath, "data", "port");
            if (ar_Process.Length <= 1) {
                //如果上一次是因為錯誤導致程式閃退，就清除port資料夾

                if (Directory.Exists(s023)) {
                    Directory.Delete(s023, true);
                }
            }



            //正常啟動


            //建立 MagickAnyCPU 的暫存目錄
            /*String s_Magick_TempDir = System.AppDomain.CurrentDomain.BaseDirectory + "data/Magick_TempDir";
            if (Directory.Exists(s_Magick_TempDir) == false) {
                Directory.CreateDirectory(s_Magick_TempDir);
            }
            ImageMagick.MagickAnyCPU.CacheDirectory = s_Magick_TempDir;*/


            //如果啟動參數是「none」，就不開啟視窗，直接開啟「快速啟動」
            if (App.main_s.Length == 1 &&
                App.main_s[0].ToLower().Trim() == "none" && (Directory.Exists(s023) == false)) {

                try {
                    TiefSee.MainWindow.fun_升級web核心();
                } catch { }
                C_adapter.Initialize();

                new C_右下角圖示();

            } else {
                new MainWindow().Show();
            }




        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool func_全部關閉(StartupEventArgs e) {

            if (e.Args.Count() >= 1) {
              
                if (e.Args[0] == "**close_all") {



                    //清除port資料夾
                    String s023 = System.AppDomain.CurrentDomain.BaseDirectory + "data\\port";
                    if (Directory.Exists(s023)) {
                        Directory.Delete(s023, true);
                    }

                    Process[] proc = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
                    for (int i = proc.Length - 1; i >= 0; i--) {
                        try {
                            if (proc[i].Id == Process.GetCurrentProcess().Id)
                                continue;
                            proc[i].Kill(); //關閉執行中的程式
                        } catch { }
                    }

                    try {
                        Shutdown();
                    } catch { }

                    //main_s = new string[] { "close" };

                    return true;

                }

            }
            return false;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool func_無延遲開啟(StartupEventArgs e) {



            //如果檔案大於50M，就正常開啟，不要用無延遲開啟
            if (e.Args.Length > 0) {
                if (File.Exists(e.Args[0])) {

                    var len = new FileInfo(e.Args[0]).Length;
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


            String s023 = System.Windows.Forms.Application.StartupPath + "\\data\\port";
            if (Directory.Exists(s023)) {
                foreach (String item in Directory.GetFiles(s023, "*")) {//判斷目前已經開啟的視窗
                    try {

                        //取得記憶體用量
                        String s = "";
                        String uri = "http://localhost:" + Path.GetFileName(item) + "/get_memory";
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                        request.Timeout = 30;
                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                            using (Stream stream = response.GetResponseStream()) {
                                using (StreamReader reader = new StreamReader(stream)) {
                                    s = reader.ReadToEnd();
                                }
                            }
                        }

                        //記憶體用量低於350M，就無延遲開啟視窗
                        if (float.Parse(s) < 350 && float.Parse(s) > 1) {

                            var sb = new System.Text.StringBuilder();
                            for (int i = 0; i < main_s.Length; i++) {
                                if (i != main_s.Length - 1) {
                                    sb.Append(main_s[i] + "\n");
                                } else {
                                    sb.Append(main_s[i]);
                                }
                            }

                            uri = "http://localhost:" + Path.GetFileName(item) + "/new_window/" +
                                    Uri.EscapeDataString(sb.ToString());
                            HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(uri);
                            using (HttpWebResponse response = (HttpWebResponse)request2.GetResponse()) {
                            }


                            try {
                                Shutdown();
                            } catch { }

                            //main_s = new string[] { "close" };

                            return true;
                        }

                    } catch { }
                }//foreach
            }//if 



            return false;
        }










        //http://www.wpftutorial.net/Jumplists.html
        /// <summary>
        /// 工作列的右鍵選單
        /// </summary>
        /// <param name="e"></param>










    }
}
