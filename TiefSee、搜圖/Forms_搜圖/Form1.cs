using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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




        String path_config = "../config.xml";
        public List<String> ar_預設瀏覽器 = new List<string>();


        public bool bool_webLoad = false;

        public string isThis = "true";
        public string Config_fileName = @"";
        public string Config_gsType = "";

        //public string Config_gsType = "ascii2d";
        //public string Config_gsType = "google";
        //public string Config_gsType = "saucenao";
        //public string Config_gsType = "bing";


        /// <summary>
        /// 
        /// </summary>
        public Form1(String[] args) {



            //判斷config.xml檔案是否存在
            /*path_config = System.Windows.Forms.Application.StartupPath + "/../config.xml";
            if (File.Exists(path_config) == false) {
                path_config = System.Windows.Forms.Application.StartupPath + "/config.xml";
                if (File.Exists(path_config) == false) {
                    MessageBox.Show("找不到『config.xml』\n此程式必須透過 AeroPic 來調用");
                    return;
                }
            }

            //讀取預設瀏覽器的順序
            fun_config();
            fun_讀取input();*/

            fun_升級web核心();

            InitializeComponent();

            webBrowser1.ObjectForScripting = new C_web呼叫javaScript(this);//讓網頁允許存取C#
            webBrowser1.ScriptErrorsSuppressed = true;


            //fun_ttt4();


            if (args.Length == 1) {
                Config_gsType = "google";
                Config_fileName = args[0];
            }
            if (args.Length >= 2) {
                Config_gsType = args[0];
                Config_fileName = args[1];
            }




            webBrowser1.DocumentCompleted += (sender, e) => {

                if (bool_webLoad) {
                    return;
                }

                new Thread(() => {

                    for (int i = 0; i < 300; i++) {
                        if (bool_webLoad) {
                            return;
                        }
                        Thread.Sleep(100);
                        bool bool_break = false;
                        this.Invoke(new MethodInvoker(() => {//委託UI行緒
                            var wrs = webBrowser1.ReadyState;
                            if (wrs == WebBrowserReadyState.Loaded || wrs == WebBrowserReadyState.Interactive || wrs == WebBrowserReadyState.Complete) {
                                bool_webLoad = true;
                                bool_break = true;
                                try {

                                    String jspath = Path.Combine(
                                        System.AppDomain.CurrentDomain.BaseDirectory, 
                                        "main.js"
                                    );
                                    using (StreamReader sr = new StreamReader(jspath, Encoding.UTF8)) {     //讀取【ascii2d】的.js
                                        String js_ascii2d = sr.ReadToEnd();
                                        webBrowser1.Document.InvokeScript("eval", new Object[] { js_ascii2d });

                                    }
                                    System.Console.WriteLine("JS注入-OK");
                                } catch (Exception ee) {
                                    MessageBox.Show("JS注入-失敗\n" + ee, "Error");
                                    Close();
                                }

                            }
                        }));
                        if (bool_break)
                            break;
                    }
                }).Start();


            };

            //webBrowser1.Navigate("https://ascii2d.net/");

            webBrowser1.DocumentText = @"<!DOCTYPE html>
                                        <html lang='en'>
                                        <head>
                                            <meta charset='UTF-8'>     
                                            <meta http-equiv='X-UA-Compatible' content='IE=edge,chrome=1' />
                                        </head>
                                        <body></body>
                                        </html>";




            return;




        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public void func_loadWeb(String url) {
            bool_webLoad = false;
            isThis = "false";
            webBrowser1.Navigate(url);
        }





        private void fun_ttt4() {


            String postfile = @"C:\Users\hbl91\Desktop\ch0.png";


            // 呼叫介面上傳檔案
            using (var client = new HttpClient()) {
                using (var multipartFormDataContent = new MultipartFormDataContent()) {

                    client.DefaultRequestHeaders.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36 Edg/80.0.361.69");


                    var values = new[] {
                        new KeyValuePair<string, string>("imgurl", ""),
                        new KeyValuePair<string, string>("cbir", "sbi"),
                        new KeyValuePair<string, string>("imageBin", ImageToBase64(postfile)),

                         //other values
                    };

                    foreach (var keyValuePair in values) {
                        multipartFormDataContent.Add(new StringContent(keyValuePair.Value),
                            String.Format("\"{0}\"", keyValuePair.Key));
                    }

                    /*multipartFormDataContent.Add(new ByteArrayContent(System.IO.File.ReadAllBytes(postfile)),
                        "\"encoded_image\"",
                        "\"test.jpg\"");*/

                    var requestUri = "https://www.bing.com/images/search?view=detailv2&iss=sbiupload&FORM=SBIIDP&sbisrc=ImgDropper&idpbck=1";
                    HttpResponseMessage response = client.PostAsync(requestUri, multipartFormDataContent).Result;
                    var html = response.Content.ReadAsStringAsync().Result;
                    System.Console.WriteLine(response.RequestMessage.RequestUri);

                    FileStream fs = new FileStream(@"aa.html", FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                    sw.WriteLine(html);

                    sw.Flush();//清空緩衝區
                    sw.Close();//關閉流
                    sw = null;
                    fs.Close();
                }
            }

        }



        private void fun_ttt3() {


            String postfile = @"C:\Users\hbl91\Desktop\2fa30a96dc86518b88a557909f2208ff.png";


            // 呼叫介面上傳檔案
            using (var client = new HttpClient()) {
                using (var multipartFormDataContent = new MultipartFormDataContent()) {

                    client.DefaultRequestHeaders.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36 Edg/80.0.361.69");


                    var values = new[] {
                        new KeyValuePair<string, string>("hl", "zh-TW"),
                        new KeyValuePair<string, string>("filename", "123.jpg"),
                        new KeyValuePair<string, string>("image_content", ""),
                        new KeyValuePair<string, string>("btnG", "以圖搜尋"),
                         //other values
                    };

                    foreach (var keyValuePair in values) {
                        multipartFormDataContent.Add(new StringContent(keyValuePair.Value),
                            String.Format("\"{0}\"", keyValuePair.Key));
                    }

                    multipartFormDataContent.Add(new ByteArrayContent(System.IO.File.ReadAllBytes(postfile)),
                        "\"encoded_image\"",
                        "\"test.jpg\"");

                    var requestUri = "https://www.google.com.tw/searchbyimage/upload";
                    HttpResponseMessage response = client.PostAsync(requestUri, multipartFormDataContent).Result;
                    var html = response.Content.ReadAsStringAsync().Result;
                    System.Console.WriteLine(response.RequestMessage.RequestUri);

                    FileStream fs = new FileStream(@"aa.html", FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                    sw.WriteLine(html);

                    sw.Flush();//清空緩衝區
                    sw.Close();//關閉流
                    sw = null;
                    fs.Close();
                }
            }

        }






        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /*private async void fun_MultiService(String path) {

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
        }*/







        /// <summary>
        /// 
        /// </summary>
        public void fun_google搜圖失敗_重新整理() {

            webBrowser1.Refresh();
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e) {




        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /*public void fun_open_saucenao(String s) {
            String url = "http://saucenao.com/search.php?db=999&url=" + Uri.EscapeDataString(s);

            fun_開啟網址(url, true);
            Close();
        }*/




        /// <summary>
        /// 
        /// </summary>
        /*private void fun_讀取input() {

            //讀取【圖片網址】與【搜圖類型】
            using (StreamReader sr = new StreamReader("input.txt", Encoding.UTF8)) {
                String line;
                while ((line = sr.ReadLine()) != null) {
                    if (line.Substring(0, 4) == "type") {
                        //type = line.Replace("type:", "");
                    } else if (line.Substring(0, 3) == "img") {
                        //s_img_url = line.Replace("img:", "");
                    }
                }//while
            }//using1

        }*/








        /// <summary>
        /// 將 Image 物件轉 Base64
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        /*public string ImageToBase64(System.Drawing.Image image) {

            MemoryStream ms = new MemoryStream();

            // 將圖片轉成 byte[]
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] imageBytes = ms.ToArray();

            // 將 byte[] 轉 base64
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;

        }*/



        /// <summary>
        /// 將 Image 物件轉 Base64
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public string ImageToBase64(String path) {
            Byte[] bytes = File.ReadAllBytes(path);
            String file = Convert.ToBase64String(bytes);
            return file;

        }











        /// <summary>
        /// 讀取系統的 config 進階設定檔
        /// </summary>
        /*public void fun_config() {

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

        }*/



        /// <summary>
        /// 
        /// </summary>
        /// <param name="s_view"></param>
        /// <param name="bool_轉碼"></param>
        /*public void fun_開啟網址(String s_view, bool bool_轉碼) {
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

        }*/



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



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e) {

            webBrowser1.Document.InvokeScript("eval", new Object[] { textBox1.Text });

        }


    }



    [ComVisible(true)]
    public class C_web呼叫javaScript {


        private Form1 M;

        public C_web呼叫javaScript(Form1 m) {
            this.M = m;
        }

        public String isThis() {
            return M.isThis;
        }
        public String Config_fileName() {
            return M.Config_fileName;
        }

        public String Config_gsType() {
            return M.Config_gsType;
        }

        public String Config_isMain() {
            return "true";
        }


        public String Web_Load(String url) {

            M.func_loadWeb(url);
            return "true";
        }

        public String web_inScript() {
            //webBrowser1.Document.InvokeScript("eval", new Object[] { js_google.Replace("{{base64}}", s_img_url) });
            return "";
        }

        public String File_save(String path, String txt) {

            using (FileStream fs = new FileStream(path, FileMode.Create)) {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8)) {
                    sw.WriteLine(txt);

                    sw.Flush();//清空緩衝區
                    sw.Close();//關閉流                  
                    fs.Close();
                }
            }
            return "true";
        }


        public String File_size(String path) {
            return "true";
        }

        public String File_w_h(String path) {
            return "true";
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String Windows_close() {
            try {
                M.Close();
                return "true";
            } catch { }

            return "false";
        }

        HttpClientHandler handler = new HttpClientHandler();
        HttpClient client = new HttpClient();
        MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
        String HttpClient_HTML = "";
        String HttpClient_Url = "";


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String HttpClient_init() {

            // 邊界符
            String boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
            String ContentType = "multipart/form-data; boundary=" + boundary;



            //handler = new HttpClientHandler();
            //client = new HttpClient(handler);
            //multipartFormDataContent = new MultipartFormDataContent();
            HttpClient_HTML = "";
            HttpClient_Url = "";

            client.DefaultRequestHeaders.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36 Edg/80.0.361.69");
            //client.DefaultRequestHeaders.Add("accept-encoding", "gzip, deflate, br");;
            //client.DefaultRequestHeaders.Add("accept-language", "zh-TW,zh;q=0.9,en;q=0.8,en-GBq=0.7,en-US;q=0.6,ja;q=0.5");
            //client.DefaultRequestHeaders.Add("cache-control", "max-age=0");
            //client.DefaultRequestHeaders.Add("content-length", "696060");
            //client.DefaultRequestHeaders.Add("content-type", ContentType);
            /*client.DefaultRequestHeaders.Add("dnt", "1");
            client.DefaultRequestHeaders.Add("origin", "https://ascii2d.net");
            client.DefaultRequestHeaders.Add("referer", "https://ascii2d.net/");
            client.DefaultRequestHeaders.Add("sec-fetch-dest", "document");
            client.DefaultRequestHeaders.Add("sec-fetch-mode", "navigate");
            client.DefaultRequestHeaders.Add("sec-fetch-site", "same-origin");
            client.DefaultRequestHeaders.Add("sec-fetch-user", "?1");
            client.DefaultRequestHeaders.Add("upgrade-insecure-requests", "1");*/
            //client.DefaultRequestHeaders.Add("host", "ascii2d.net");

            //client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            //client.DefaultRequestHeaders.Add("", "");
            //client.DefaultRequestHeaders.Add("cookie", "_session_id=0eea936d101877c069d398b493a9362d");
            return "true";

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public String HttpClient_useCookies(String b) {

            try {
                b = b.Trim().ToLower();
                bool b2 = (b == "true" || b == "1");
                handler.UseCookies = b2;
                return "true";
            } catch (Exception) {
                return "false";
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public String HttpClient_setCookies(String cookie) {

            try {
                handler.UseCookies = false;
                //multipartFormDataContent.Headers.Add("Cookie", cookie);
                client.DefaultRequestHeaders.Add("Cookie", cookie);
                //client.DefaultRequestHeaders.Add("Cookie", M.webBrowser1.Document.Cookie);

                return "true";
            } catch (Exception) {
                return "false";
            }

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public String HttpClient_formDataAdd(String key, String value) {

            try {
                multipartFormDataContent.Add(new StringContent(value), String.Format("\"{0}\"", key));
                return "true";
            } catch (Exception e) {
                return e.ToString();
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public String HttpClient_formFileAdd(String key, String path) {
            try {
                multipartFormDataContent.Add(
                    new ByteArrayContent(System.IO.File.ReadAllBytes(path)),
                    $"\"{key}\"",
                    $"\"{Path.GetFileName(path)}\""
                );
                //System.Console.WriteLine(path);
                return "true";
            } catch (Exception e) {
                return e.ToString();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public String HttpClient_submit(String type, String requestUri) {


            try {
                type = type.Trim().ToLower();

                HttpResponseMessage response = null;
                if (type == "get") {
                    response = client.GetAsync(requestUri).Result;
                } else {
                    response = client.PostAsync(requestUri, multipartFormDataContent).Result;
                }

                HttpClient_HTML = response.Content.ReadAsStringAsync().Result;
                HttpClient_Url = response.RequestMessage.RequestUri.ToString();

                return "true";

            } catch (Exception e) {

                return e.ToString();
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String HttpClient_getHTML() {
            return HttpClient_HTML;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String HttpClient_getUrl() {
            return HttpClient_Url;
        }



        #region Path


        /// <summary>
        /// 取得檔案所在的目錄
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public String Path_GetDirectoryName(String path) {
            try {
                return Path.GetDirectoryName(path);
            } catch {
                return "false";
            }
        }

        /// <summary>
        /// 取得檔名+副檔名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public String Path_GetFileName(String path) {
            try {
                return Path.GetFileName(path);
            } catch {
                return "false";
            }
        }


        /// <summary>
        /// 取得檔名(不含副檔名)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public String Path_GetFileNameWithoutExtension(String path) {
            try {

                return Path.GetFileNameWithoutExtension(path); ;
            } catch {
                return "false";
            }
        }


        /// <summary>
        /// 取得檔案副檔名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public String Path_GetExtension(String path) {
            try {
                return Path.GetExtension(path); ;
            } catch {
                return "false";
            }
        }



        /// <summary>
        /// 取得桌面路徑
        /// </summary>
        /// <returns></returns>
        public String Path_Desktop() {
            string filder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            return filder;
        }


        /// <summary>
        /// 取得執行檔路徑
        /// </summary>
        public String Path_StartupPath() {
            return System.Windows.Forms.Application.StartupPath;
        }

        #endregion Path



        #region File

        /// <summary>
        /// 讀取檔案
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public String File_Load(String path) {
            try {
                using (StreamReader sr = new StreamReader(path, Encoding.UTF8)) {
                    return sr.ReadToEnd();
                }
            } catch {
                return "false";
            }
        }


        /// <summary>
        /// 讀取檔案(base64)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public String File_Load_base64(String path) {
            try {

                Byte[] bytes = File.ReadAllBytes(path);
                String base64 = Convert.ToBase64String(bytes);
                return base64;

            } catch {
                return "false";
            }
        }


        /// <summary>
        /// 新建一個檔案
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public String File_Write(String path, String txt) {
            try {
                using (FileStream fs = new FileStream(path, FileMode.Create)) {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8)) {
                        sw.WriteLine(txt);
                    }
                }
                return "true";
            } catch {
                return "false";
            }
        }


        /// <summary>
        /// 建立新檔案，將指定的字串寫入檔案。 如果檔案已經存在，則會覆寫該檔案。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public String File_WriteAllText(String path, String txt) {
            try {
                File.WriteAllText(path, txt, Encoding.UTF8);
                return "true";
            } catch {
                return "false";
            }
        }


        /// <summary>
        /// 在檔案中加入幾行內容。 如果指定的檔案不存在，則這個方法會建立檔案，將指定的程式行寫入檔案
        /// </summary>
        /// <param name="path"></param>
        /// <param name="txt"></param>
        /// <returns></returns>
        public String File_AppendAllLines(String path, String txt) {
            try {
                string[] lines = { txt };
                File.AppendAllLines(path, lines, Encoding.UTF8);
                return "true";
            } catch {
                return "false";
            }
        }


        /// <summary>
        /// 新建一個檔案(base64)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public String File_Write_base64(String path, String b64Str) {
            try {

                Byte[] bytes = Convert.FromBase64String(b64Str);
                File.WriteAllBytes(path, bytes);

                return "true";
            } catch {
                return "false";
            }
        }



        /// <summary>
        /// 判斷檔案是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public String File_Exists(String path) {
            if (File.Exists(path)) {
                return "true";
            } else {
                return "false";
            }
        }


        /// <summary>
        /// 刪除檔案
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public String File_Delete(String path) {
            try {
                File.Delete(path);
                return "true";
            } catch {
                return "false";
            }
        }

        /// <summary>
        /// 取得檔案的存取日期
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type">yyyy/MM/dd HH:mm:ss</param>
        /// <returns></returns>
        public String File_GetLastAccessTime(String path, String type) {
            try {
                return File.GetLastAccessTime(path).ToString(type);
            } catch {
                return "false";
            }
        }


        /// <summary>
        /// 取得檔案的修改日期
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public String File_GetLastWriteTime(String path, String type) {
            try {
                return File.GetLastWriteTime(path).ToString(type);
            } catch {
                return "false";
            }
        }

        #endregion File




        #region Directory


        /// <summary>
        /// 新建資料夾
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public String Directory_CreateDirectory(String path) {
            try {
                Directory.CreateDirectory(path);
                return "true";
            } catch {
                return "false";
            }
        }


        /// <summary>
        /// 取得父親路徑
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public String Directory_GetParent(String path) {
            try {
                return Directory.GetParent(path).ToString();
            } catch {
                return "false";
            }
        }



        /// <summary>
        /// 判斷資料夾是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public String Directory_Exists(String path) {
            if (Directory.Exists(path)) {
                return "true";
            } else {
                return "false";
            }
        }


        /// <summary>
        /// 刪除資料夾
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public String Directory_Delete(String path) {
            try {
                Directory.Delete(path, true);
                return "true";
            } catch {
                return "false";
            }
        }



        /// <summary>
        /// 取得資料夾的存取日期
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type">yyyy/MM/dd HH:mm:ss</param>
        /// <returns></returns>
        public String Directory_GetLastAccessTime(String path, String type) {
            try {
                return Directory.GetLastAccessTime(path).ToString(type);
            } catch {
                return "false";
            }
        }


        /// <summary>
        /// 取得資料夾的修改日期
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public String Directory_GetLastWriteTime(String path, String type) {
            try {
                return Directory.GetLastWriteTime(path).ToString(type);
            } catch {
                return "false";
            }
        }

        /// <summary>
        /// 取得資料夾裡面所有資料夾的完整路徑
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"> *.png </param>
        /// <returns></returns>
        public String Directory_GetDirectories(String path, String name) {
            try {

                StringBuilder sb = new StringBuilder();
                var ar = Directory.GetDirectories(path, name);
                for (int i = 0; i < ar.Length; i++) {
                    sb.Append(ar[i]);
                    if (i != ar.Length - 1) {
                        sb.Append("\n");
                    }
                }
                return sb.ToString();

            } catch {
                return "false";
            }
        }


        /// <summary>
        /// 取得資料夾裡面所有檔案的完整路徑
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"> *.png </param>
        /// <returns></returns>
        public String Directory_GetFiles(String path, String name) {
            try {

                StringBuilder sb = new StringBuilder();
                var ar = Directory.GetFiles(path, name);
                for (int i = 0; i < ar.Length; i++) {
                    sb.Append(ar[i]);
                    if (i != ar.Length - 1) {
                        sb.Append("\n");
                    }
                }
                return sb.ToString();

            } catch {
                return "false";
            }
        }


        #endregion Directory




        /// <summary>
        /// 設定剪貼簿
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public String Clipboard_SetData(String text) {
            try {
                Clipboard.SetData(DataFormats.Text, text);
            } catch {
                return "false";
            }
            return "true";
        }



        /// <summary>
        /// 取得剪貼簿
        /// </summary>
        /// <returns></returns>
        public String Clipboard_GetData() {
            try {
                IDataObject data = Clipboard.GetDataObject();
                String s = "null";
                if (data.GetDataPresent(DataFormats.Text)) {
                    s = data.GetData(DataFormats.Text).ToString();
                }
                return s;

            } catch {
                return "false";
            }
        }



        /// <summary>
        /// 開啓檔案
        /// </summary>
        /// <returns></returns>
        public String Process_Start(String path) {
            try {
                Process.Start(path);
                return "true";
            } catch (Exception) {
                return "false";
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public String OpenUrl(String url) {
            return Process_Start(url);
        }


        public void print(String s) {
            System.Console.WriteLine(s);
        }

    }





}
