using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Forms_搜圖 {
    public partial class Form1 : Form {



        private String type = "google";
        private String s_img_url = "";
        private String js_google = "";
        private String js_saucenao = "";
        bool bool_第一次載入;
        String path_config = "../config.xml";
        public List<String> ar_預設瀏覽器 = new List<string>();



        /// <summary>
        /// 
        /// </summary>
        public Form1() {

            //判斷config.xml檔案是否存在
            path_config = System.Windows.Forms.Application.StartupPath + "/../config.xml";
            if (File.Exists(path_config) == false) {
                path_config = System.Windows.Forms.Application.StartupPath + "/config.xml";
                if (File.Exists(path_config) == false) {
                    MessageBox.Show("找不到『config.xml』\n此程式必須透過 AeroPic 來調用");
                    return;
                }
            }

            //讀取預設瀏覽器的順序
            fun_config();
            fun_讀取input();

            fun_升級web核心();

            InitializeComponent();


            //顯示動畫
            int int_tim = 0;
            System.Windows.Forms.Timer tim = new System.Windows.Forms.Timer();
            tim.Interval = 500;
            tim.Tick += (sender, e) => {
                int_tim++;
                if (int_tim > 3)
                    int_tim = 0;
                if (int_tim == 3) {
                    label_loading.Text = "Loading...";
                } else if (int_tim == 2) {
                    label_loading.Text = "Loading..";
                } else if (int_tim == 1) {
                    label_loading.Text = "Loading.";
                } else if (int_tim == 0) {
                    label_loading.Text = "Loading";
                }
            };
            tim.Start();


            webBrowser1.ObjectForScripting = new C_web呼叫javaScript(this);//讓網頁允許存取C#
            webBrowser1.ScriptErrorsSuppressed = true;

            bool_第一次載入 = true;


            if (type == "iqdb") {//如果是iqdb，就直接post到後台

                fun_MultiService(s_img_url);
                return;
            }

            if (type == "ascii2d") {//如果是 ascii2d ，用webbrowser處理

                webBrowser1.DocumentCompleted += (sender, e) => {

                    new Thread(() => {
                        for (int i = 0; i < 100; i++) {
                            Thread.Sleep(100);
                            bool bool_break = false;
                            this.Invoke(new MethodInvoker(() => {//委託UI行緒
                                if (webBrowser1.ReadyState == WebBrowserReadyState.Interactive || webBrowser1.ReadyState == WebBrowserReadyState.Complete) {
                                    try {
                                        System.Console.WriteLine("sdfsdffsdf++++");
                                        using (StreamReader sr = new StreamReader("js/ascii2d.js", Encoding.UTF8)) {     //讀取【ascii2d】的.js
                                            String js_ascii2d = sr.ReadToEnd();
                                            webBrowser1.Document.InvokeScript("eval", new Object[] { js_ascii2d.Replace("{{base64}}", s_img_url) });
                                        }

                                    } catch (Exception ee) {
                                        MessageBox.Show(ee + "", "Error");
                                        Close();
                                    }
                                    bool_break = true;
                                }
                            }));
                            if (bool_break)
                                break;
                        }
                    }).Start();


                };

                webBrowser1.Navigate("https://ascii2d.net/");

                return;
            }



            if (type == "saucenao" || type == "google") {//Google搜尋或sauceNAO，就是用webbrowser處理


                webBrowser1.DocumentCompleted += (sender, e) => {
                    if (bool_第一次載入) {

                        new Thread(() => {
                            for (int i = 0; i < 100; i++) {
                                Thread.Sleep(100);
                                bool bool_break = false;
                                this.Invoke(new MethodInvoker(() => {//委託UI行緒
                                    if (webBrowser1.ReadyState == WebBrowserReadyState.Interactive || webBrowser1.ReadyState == WebBrowserReadyState.Complete) {
                                        try {
                                            fun_執行搜圖js();
                                        } catch (Exception ee) {
                                            MessageBox.Show(ee + "", "Error");
                                            Close();
                                        }
                                        bool_break = true;
                                    }
                                }));
                                if (bool_break)
                                    break;
                            }
                        }).Start();
                        bool_第一次載入 = false;
                    }
                };

                webBrowser1.Navigate("http://www.google.com/searchbyimage");


                webBrowser1.Navigated += WebBrowser1_Navigated;
            }






        }









        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private async void fun_MultiService(String path) {

            //取得圖片byte[]
            byte[] b = File.ReadAllBytes(path);

            //post到伺服器並回傳html
            HttpClient httpClient = new HttpClient();
            MultipartFormDataContent form = new MultipartFormDataContent();

            form.Add(new ByteArrayContent(b, 0, b.Length), "file", path);

            HttpResponseMessage response = await httpClient.PostAsync("http://iqdb.org/", form);

            response.EnsureSuccessStatusCode();
            httpClient.Dispose();
            string html = response.Content.ReadAsStringAsync().Result;


            //用正則式是篩選出網址
            //<script type="text/javascript">try{document.getElementById("yetmore").href = "/?thu=736b5b57&org=hello1.jpg&more=1";}catch(e){document.write(" (not supported by your browser)");}</script>
            String s_條件 = "[/][?][t][h][u][=].*?([\"]|['])";
            Match match = Regex.Match(html, s_條件);
            String r = match.Groups[0].Value;
            r = r.Replace("\"", "");
            r = "http://iqdb.org" + r;

            fun_開啟網址(r, true);
            Close();
        }







        /// <summary>
        /// 
        /// </summary>
        public void fun_google搜圖失敗_重新整理() {
            bool_第一次載入 = true;
            webBrowser1.Refresh();
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e) {

            String s_url = e.Url.ToString();

            if (type == "google") {//google搜圖
                if (s_url.Contains("google.com.tw/search?") || s_url.Contains("google.com/search?")) {
                    fun_開啟網址(s_url, true);
                    Close();
                }

            } else if (type == "saucenao") {

                if (s_url.Contains("google.com.tw/search?") || s_url.Contains("google.com/search?")) {
                    webBrowser1.Document.InvokeScript("eval", new Object[] { js_saucenao });//透過js抓取圖片，之後js會呼叫【fun_open_saucenao】來啟動
                }

            }


        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        public void fun_open_saucenao(String s) {
            String url = "http://saucenao.com/search.php?db=999&url=" + Uri.EscapeDataString(s);

            fun_開啟網址(url, true);
            Close();
        }




        /// <summary>
        /// 
        /// </summary>
        private void fun_讀取input() {

            //讀取【圖片網址】與【搜圖類型】
            using (StreamReader sr = new StreamReader("input.txt", Encoding.UTF8)) {
                String line;
                while ((line = sr.ReadLine()) != null) {
                    if (line.Substring(0, 4) == "type") {
                        type = line.Replace("type:", "");
                    } else if (line.Substring(0, 3) == "img") {
                        s_img_url = line.Replace("img:", "");
                    }
                }//while
            }//using1

        }




        /// <summary>
        /// 
        /// </summary>
        private void fun_執行搜圖js() {

            //讀取【google搜圖】的.js
            using (StreamReader sr = new StreamReader("js/google.js", Encoding.UTF8)) {
                js_google = sr.ReadToEnd();
            }//using

            //讀取【sauceNAO搜圖】的.js
            using (StreamReader sr = new StreamReader("js/saucenao.js", Encoding.UTF8)) {
                js_saucenao = sr.ReadToEnd();
            }//using

            webBrowser1.Document.InvokeScript("eval", new Object[] { js_google.Replace("{{base64}}", s_img_url) });

        }






        /// <summary>
        /// 將 Image 物件轉 Base64
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public string ImageToBase64(System.Drawing.Image image) {

            MemoryStream ms = new MemoryStream();


            // 將圖片轉成 byte[]
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] imageBytes = ms.ToArray();


            // 將 byte[] 轉 base64
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;

        }











        /// <summary>
        /// 讀取系統的 config 進階設定檔
        /// </summary>
        public void fun_config() {

            try {


                //先讀取文字檔
                StreamReader sr = new StreamReader(path_config, Encoding.UTF8);
                String s_xml = sr.ReadToEnd();

                sr.Close();
                sr.Dispose();

                //因為網址裡面可能有【&】，所以要先取代成【&amp;】
                s_xml = s_xml.Replace("&amp;", "&");
                s_xml = s_xml.Replace("&lt;", "*+<+*");
                s_xml = s_xml.Replace("&gt;", "*+>+*");
                s_xml = s_xml.Replace("&", "&amp;");
                s_xml = s_xml.Replace("*+<+*", "&lt;");
                s_xml = s_xml.Replace("*+>+*", "&gt;");


                XmlDocument XmlDoc = new XmlDocument();
                XmlDoc.LoadXml(s_xml);//從字串解析xml
                XmlNodeList NodeLists = XmlDoc.SelectNodes("settings/item");

                foreach (XmlNode item in NodeLists) {

                    if (item.Attributes["name"].Value.Equals("預設瀏覽器")) {//
                        foreach (XmlNode item2 in item.ChildNodes) {
                            ar_預設瀏覽器.Add(item2.InnerText.Trim().Replace("\"", ""));
                        }
                    }

                }
            } catch { }



        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="s_view"></param>
        /// <param name="bool_轉碼"></param>
        public void fun_開啟網址(String s_view, bool bool_轉碼) {
            //https://www.w3schools.com/tags/ref_urlencode.asp

            if (bool_轉碼)
                s_view = s_view
                    //.Replace("%", "%25")
                    .Replace(" ", "%20")
                    .Replace("\"", "%22")
                    //.Replace("#", "%23")
                    .Replace("'", "%27")
                    //.Replace(",", "%2C")
                    //.Replace(";", "%3B")
                    //.Replace("`", "%60")
                    //.Replace("&", "%26")
                    //.Replace(":", "%3A")
                    //.Replace("?", "%3F")
                    //.Replace("-", "%2D")
                    //.Replace("$", "%24")
                    //.Replace("=", "%3D")            
                    ;//避免命令列參數被切割


            s_view = "\"" + s_view + "\"";

            foreach (var item in ar_預設瀏覽器) {
                try {
                    if (System.IO.File.Exists(item)) {//檔案存在
                        System.Diagnostics.Process.Start(item, s_view);
                        return;
                    }
                } catch { }
            }

            try {
                System.Diagnostics.Process.Start(s_view);
            } catch { }


        }



        /// <summary>
        /// 使用IE11核心
        /// </summary>
        // set WebBrowser features, more info: http://stackoverflow.com/a/18333982/1768303
        private void fun_升級web核心() {
            // don't change the registry if running in-proc inside Visual Studio
            if (LicenseManager.UsageMode != LicenseUsageMode.Runtime)
                return;

            //判斷IE版本的方法
            var GetBrowserEmulationMode = new Func<UInt32>(() => {

                int browserVersion = 0;
                using (var ieKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer",
                    RegistryKeyPermissionCheck.ReadSubTree,
                    System.Security.AccessControl.RegistryRights.QueryValues)) {
                    var version = ieKey.GetValue("svcVersion");
                    if (null == version) {
                        version = ieKey.GetValue("Version");
                        if (null == version)
                            throw new ApplicationException("Microsoft Internet Explorer is required!");
                    }
                    int.TryParse(version.ToString().Split('.')[0], out browserVersion);
                }

                if (browserVersion < 7) {
                    throw new ApplicationException("Unsupported version of Microsoft Internet Explorer!");
                }

                UInt32 mode = 11000; // Internet Explorer 11. Webpages containing standards-based !DOCTYPE directives are displayed in IE11 Standards mode. 

                switch (browserVersion) {
                    case 7:
                        mode = 7000; // Webpages containing standards-based !DOCTYPE directives are displayed in IE7 Standards mode. 
                        break;
                    case 8:
                        mode = 8000; // Webpages containing standards-based !DOCTYPE directives are displayed in IE8 mode. 
                        break;
                    case 9:
                        mode = 9000; // Internet Explorer 9. Webpages containing standards-based !DOCTYPE directives are displayed in IE9 mode.                    
                        break;
                    case 10:
                        mode = 10000; // Internet Explorer 10.
                        break;
                }

                return mode;
            });

            var appName = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            var featureControlRegKey = @"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main\FeatureControl\";

            //修改預設IE版本
            Registry.SetValue(featureControlRegKey + "FEATURE_BROWSER_EMULATION", appName, GetBrowserEmulationMode(), RegistryValueKind.DWord);

            //使用完整的IE瀏覽器功能
            //Registry.SetValue(featureControlRegKey + "FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", appName, 1, RegistryValueKind.DWord);
            //Registry.SetValue(featureControlRegKey + "FEATURE_AJAX_CONNECTIONEVENTS", appName, 1, RegistryValueKind.DWord);
            Registry.SetValue(featureControlRegKey + "FEATURE_GPU_RENDERING", appName, 1, RegistryValueKind.DWord);
            //Registry.SetValue(featureControlRegKey + "FEATURE_WEBOC_DOCUMENT_ZOOM", appName, 1, RegistryValueKind.DWord);
            //Registry.SetValue(featureControlRegKey + "FEATURE_NINPUT_LEGACYMODE", appName, 0, RegistryValueKind.DWord);

        }




    }



    [ComVisible(true)]
    public class C_web呼叫javaScript {


        private Form1 M;

        public C_web呼叫javaScript(Form1 m) {
            this.M = m;
        }

        public void fun_open_saucenao(String s) {
            M.fun_open_saucenao(s);
        }


        public void fun_google_error() {
            M.fun_google搜圖失敗_重新整理();
        }

        /// <summary>
        /// 開啟網址
        /// </summary>
        /// <param name="url"></param>
        public void fun_open_url(String url) {
            M.fun_開啟網址(url, true);
        }

        /// <summary>
        /// 關閉視窗
        /// </summary>
        /// <param name="url"></param>
        public void fun_close() {



                M.Close();
            

        }


    }





}
