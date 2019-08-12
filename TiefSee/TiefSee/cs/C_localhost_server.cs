using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TiefSee.cs {
    /// <summary>
    /// 在本機建立一個localhost的伺服器
    /// </summary>
    public class C_localhost_server {


        public int port = 8145;
        public Dictionary<String, String[]> dic_圖片路徑 = new Dictionary<string, String[]>();
        private HttpListener _httpListener = new HttpListener();
        private bool bool_執行中 = true;


        MainWindow M;


        String s_用檔案記錄port = "";

        /// <summary>
        /// 
        /// </summary>
        public C_localhost_server(MainWindow m) {


            this.M = m;


            port = fun_取得可用的post();

            String d_根目錄 = System.AppDomain.CurrentDomain.BaseDirectory;


            if (Directory.Exists(d_根目錄 + "data\\port") == false) {
                Directory.CreateDirectory(d_根目錄 + "data\\port");
            }

            s_用檔案記錄port = d_根目錄 + "data\\port\\" + port;

            using (FileStream fs = new FileStream(s_用檔案記錄port, FileMode.Create)) {
            }




            Log.print("Starting server...");
            _httpListener.Prefixes.Add("http://localhost:" + port + "/"); // add prefix "http://localhost:5000/"


            //避免啟動程式失敗
            try {

                _httpListener.Start(); // start server (Run application as Administrator!)
                Log.print("Server started.   " + port);
                Thread _responseThread = new Thread(ResponseThread);
                _responseThread.Start(); // start the response thread


            } catch (Exception) {

            }

        }




        /// <summary>
        /// 
        /// </summary>
        void ResponseThread() {
            while (bool_執行中) {

                HttpListenerContext context = null;

                try {
                    context = _httpListener.GetContext(); // get a context
                } catch (System.Net.HttpListenerException) {
                    //程式結束後立即停止伺服器
                    return;
                }


                HttpListenerRequest request = context.Request; //取得輸入的網址


                // Now, you'll find the request URL in context.Request.Url
                //byte[] _responseArray = Encoding.UTF8.GetBytes("<html><head><title>Localhost server -- port 5000</title></head>" + 
                //"<body>Welcome to the <strong>Localhost server</strong> -- <em>port 5000!<h1>" + request.Url.ToString() + "</h1></em></body></html>"); // get the bytes to response


                String s_localhost = "http://localhost:" + port + "/";
                //s_localhost = "/";

                String path = request.RawUrl.ToString();
                byte[] _responseArray = new byte[0];

                String img_path = "";


                var ac_回傳錯誤 = new Action(() => {

                    if (M == null) {
                        return;
                    }

                    try {

                        //直接回傳圖示
                        if (File.Exists(img_path)) {

                            using (System.Drawing.Bitmap bit = M.c_影像.func_作業系統圖示(img_path)) {
                                MemoryStream ms = new MemoryStream();
                                bit.Save(ms, ImageFormat.Png);
                                ms.Position = 0;
                                _responseArray = new byte[ms.Length];
                                ms.Read(_responseArray, 0, _responseArray.Length);
                                ms.Close();
                            }

                        } else {//找不到檔案



                            //回傳錯誤圖片            
                            using (FileStream fs = new FileStream(M.fun_執行檔路徑() + "/data/imgs/error.png", FileMode.Open, FileAccess.Read)) {
                                _responseArray = new byte[fs.Length];
                                fs.Read(_responseArray, 0, _responseArray.Length);
                                fs.Close();
                            }

                        }

                    } catch {



                        //回傳錯誤圖片               
                        using (FileStream fs = new FileStream(M.fun_執行檔路徑() + "/data/imgs/error.png", FileMode.Open, FileAccess.Read)) {
                            _responseArray = new byte[fs.Length];
                            fs.Read(_responseArray, 0, _responseArray.Length);
                            fs.Close();
                        }

                    }







                });

                ///
                ///
                ///
                var ac_回傳圖片 = new Action(() => {

                    if (M == null) {
                        return;
                    }

                    if (File.Exists(img_path)) {

                        String s_附檔名 = "";

                        //s_附檔名 = M.c_影像.fun_取得附檔名(img_path);

                        if (img_path.IndexOf('.') > 0)
                            s_附檔名 = img_path.Substring(img_path.LastIndexOf('.') + 1).ToUpper();


                        //如果是 RWA 圖片
                        if (M.c_影像.func_判斷_RAW(s_附檔名)) {

                            var img_raw = M.c_影像.func_dcraw(img_path, s_附檔名);

                            if (img_raw != null) {
                                //設定Headers
                                var header = new System.Collections.Specialized.NameValueCollection();
                                header.Add("Content-Type", "image/png");
                                context.Response.Headers.Add(header);


                                //回傳檔案                    
                                //BitmapSource bs_img = M.fun_get_BitmapImage_s(img_path, false);          
                                var encoder = new PngBitmapEncoder();
                                encoder.Frames.Add(BitmapFrame.Create(img_raw));

                                using (MemoryStream memoryStream = new MemoryStream()) {
                                    encoder.Save(memoryStream);
                                    _responseArray = memoryStream.ToArray();
                                }
                            } else {

                                ac_回傳錯誤();

                            }

                        } else
                        if (s_附檔名 == "SVG") {  //SVG

                            //設定Headers
                            var header = new System.Collections.Specialized.NameValueCollection();
                            header.Add("Access-Control-Allow-Origin", "*");
                            header.Add("Access-Control-Allow-Methods", "GET,POST");
                            header.Add("Access-Control-Allow-Headers", "Content-Type, Accept, X-Requested-With");
                            header.Add("Access-Control-Max-Age", "1728000");
                            header.Add("Content-Type", "image/svg+xml");
                            header.Add("Vary", "Accept-Encoding");
                            context.Response.Headers.Add(header);

                            //回傳檔案
                            /*using (FileStream fs = new FileStream(img_path, FileMode.Open, FileAccess.Read)) {
                                _responseArray = new byte[fs.Length];
                                fs.Read(_responseArray, 0, _responseArray.Length);
                                fs.Close();
                            }*/

                            String svg = func_get_svg(img_path);
                            _responseArray = Encoding.UTF8.GetBytes(svg);

                        } else
                            if (s_附檔名 == "JPG" || s_附檔名 == "JPEG" || s_附檔名 == "PNG" || s_附檔名 == "GIF" || s_附檔名 == "BMP") {//一般圖片(jpg、png、gif)

                            //回傳檔案
                            using (FileStream fs = new FileStream(img_path, FileMode.Open, FileAccess.Read)) {
                                _responseArray = new byte[fs.Length];
                                fs.Read(_responseArray, 0, _responseArray.Length);
                                fs.Close();
                            }

                        } else
                            if (s_附檔名 == "WEBM") {//webm 影片


                            //設定Headers
                            var header = new System.Collections.Specialized.NameValueCollection();
                            //header.Add("Content-Type", "video/x-matroska");
                            header.Add("Content-Type", "video/webm");
                            //header.Add("Content-Disposition", "form-data");
                            //header.Add("name", "fieldName");
                            header.Add("filename", "aaa.mkv");
                            context.Response.Headers.Add(header);

                            //回傳檔案
                            using (FileStream fs = new FileStream(img_path, FileMode.Open, FileAccess.Read)) {
                                _responseArray = new byte[fs.Length];
                                fs.Read(_responseArray, 0, _responseArray.Length);
                                fs.Close();
                            }

                        } else
                            if (s_附檔名 == "EXE" || s_附檔名 == "LNK") {

                            BitmapSource bs_img = M.c_P網.fun_取得exe圖示(img_path, s_附檔名);



                            var encoder = new PngBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(bs_img));

                            using (MemoryStream memoryStream = new MemoryStream()) {
                                encoder.Save(memoryStream);
                                memoryStream.Position = 0;
                                _responseArray = memoryStream.ToArray();
                            }

                        } else
                            if (s_附檔名 == "ICO") {

                            BitmapSource bs_img = M.c_P網.fun_取得ICO圖示(img_path);
                            var encoder = new PngBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(bs_img));

                            using (MemoryStream memoryStream = new MemoryStream()) {
                                encoder.Save(memoryStream);
                                _responseArray = memoryStream.ToArray();
                            }

                        } else
                        /*if (s_附檔名 == "PSD" || s_附檔名 == "PSB" || s_附檔名 == "WEBP" || s_附檔名 == "TIF" || s_附檔名 == "TIFF" ||
                            s_附檔名 == "PDF" || s_附檔名 == "AI" || s_附檔名 == "JP2" || s_附檔名 == "PBM" || s_附檔名 == "PGM" || s_附檔名 == "PCX" ||
                            s_附檔名 == "TGA" || s_附檔名 == "PPM" || s_附檔名 == "PSB" || s_附檔名 == "DDS") */{

                            //設定Headers
                            var header = new System.Collections.Specialized.NameValueCollection();
                            header.Add("Content-Type", "image/png");
                            context.Response.Headers.Add(header);


                            //回傳檔案                    
                            //BitmapSource bs_img = M.fun_get_BitmapImage_s(img_path, false);

                            using (var im = M.c_影像.c_Magick.getImg(img_path, s_附檔名)) {
                                BitmapSource bs_img = im.ToBitmapSource();
                                var encoder = new PngBitmapEncoder();
                                encoder.Frames.Add(BitmapFrame.Create(bs_img));

                                using (MemoryStream memoryStream = new MemoryStream()) {
                                    encoder.Save(memoryStream);
                                    _responseArray = memoryStream.ToArray();
                                }
                            }

                        }


                    } else {//如果檔案不存在

                        ac_回傳錯誤();
                    }


                });





                try {


                    if (path.IndexOf("/new_window/") > -1) {//快速啟動 - 

                        path = path.Substring("/new_window/".Length);
                        img_path = Uri.UnescapeDataString(path);//


                        C_adapter.fun_UI執行緒(() => {
                            MainWindow.func_新視窗開啟圖片(img_path);
                        });


                    } else if (path.IndexOf("/get_memory") > -1) {//取得目前記憶體用量

                        //回傳檔案
                        String sss = MainWindow.fun_取得記憶體用量() + "";
                        _responseArray = System.Text.Encoding.ASCII.GetBytes(sss);



                    } else if (path.IndexOf("/img_path/") > -1) {//大量瀏覽模式-直接在程式內部開啟圖片

                        path = path.Substring("/img_path/".Length);

                        if (path.IndexOf("*") > 5) {//無視「*」後面的文字
                            path = path.Substring(0, path.IndexOf("*"));
                        }

                        img_path = Uri.UnescapeDataString(path);


                        if (img_path.IndexOf(":") < 0)
                            img_path = "\\\\" + img_path;//避免某些不是從硬碟取得圖片的特殊路徑，例如：『\\vmware-host\Shared Folders\D\圖片』

                        ac_回傳圖片();



                    } else if (path.IndexOf("/open_img") > -1) {//外部瀏覽器、新開圖片視窗

                        //path = path.Replace(s_localhost, "");
                        path = path.Substring(1);

                        String[] ars = path.Split('/');
                        String s1 = ars[0];
                        String s2 = ars[1];
                        if (dic_圖片路徑.ContainsKey(s1)) {
                            img_path = dic_圖片路徑[s1][Int32.Parse(s2)];
                        }

                        img_path = img_path.Replace(s_localhost + "img_path/", "");
                        img_path = Uri.UnescapeDataString(img_path);



                        if (File.Exists(img_path)) {
                            C_adapter.fun_UI執行緒(() => {
                                M.func_新視窗開啟圖片_大量瀏覽模式(img_path);
                            });
                        }

                    } else {//外部瀏覽器、顯示圖片

                        //path = path.Replace(s_localhost, "");
                        path = path.Substring(1);

                        //根據網址來取得相對應的圖片路徑
                        String[] ars = path.Split('/');
                        String s1 = ars[0];
                        String s2 = ars[1];
                        if (dic_圖片路徑.ContainsKey(s1)) {
                            img_path = dic_圖片路徑[s1][Int32.Parse(s2)];
                        }
                        img_path = img_path.Replace(s_localhost + "img_path/", "");
                        img_path = Uri.UnescapeDataString(img_path);

                        ac_回傳圖片();
                    }



                } catch (Exception e2) {

                    ac_回傳錯誤();


                }




                try {
                    context.Response.OutputStream.Write(_responseArray, 0, _responseArray.Length); // write bytes to the output stream
                    context.Response.KeepAlive = false; // set the KeepAlive bool to false
                    context.Response.Close(); // close the connection
                } catch (System.Net.HttpListenerException) { }




            }


            func_刪除記錄用post();


        }



        /// <summary>
        /// 
        /// </summary>
        private void func_刪除記錄用post() {


            if (File.Exists(s_用檔案記錄port)) {
                try {
                    File.Delete(s_用檔案記錄port);
                } catch { }

            }

        }




        /// <summary>
        /// 關閉程式前呼叫
        /// </summary>
        public void fun_end() {
            bool_執行中 = false;
            _httpListener.Abort();
            func_刪除記錄用post();
        }


        /// <summary>
        /// 判斷port是否有被佔用
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        private bool PortInUse(int port) {
            bool inUse = false;

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();

            foreach (IPEndPoint endPoint in ipEndPoints) { // www.jbxue.com
                if (endPoint.Port == port) {
                    inUse = true;
                    break;
                }
            }

            return inUse;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int fun_取得可用的post() {

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();

            for (int i = 55111; i < 65535; i++) {
                bool inUse = false;
                foreach (IPEndPoint endPoint in ipEndPoints) { // www.jbxue.com
                    if (endPoint.Port == i) {
                        inUse = true;
                        break;
                    }
                }
                if (inUse == false) {
                    return i;
                }
            }

            return 48763;
        }





        String base64ToString(String base64) {
            byte[] bytes2 = Convert.FromBase64String(base64);
            string str = Encoding.UTF8.GetString(bytes2);
            return str;
        }

        String stringToBase64(String s) {
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            string base64 = Convert.ToBase64String(bytes);
            return base64;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public String func_get_svg(String path) {


            Func<String, int> func_取得_int = (String s24) => {
                int i023 = 0;
                s24 = s24.Trim().ToUpper();
                s24 = s24.Replace("C", "").Replace("M", "").Replace("P", "")
                        .Replace("X", "").Replace("I", "").Replace("N", "")
                        .Replace("E", "").Replace("P", "").Replace("T", "")
                        .Replace("V", "").Replace("W", "").Replace("H", "")
                        .Replace("A", "").Replace("%", "");

                i023 = (int)float.Parse(s24);

                return i023;
            };



            int int_svg_w = 50;
            int int_svg_h = 50;

            String svg = "";
            using (StreamReader sr = new StreamReader(path, Encoding.UTF8)) {

                svg = sr.ReadToEnd();
            }



            bool bool_wh = true;
            bool bool_viewbox = true;

            //<svg width="1500" height="727"
            try {

                string pattern = "<svg.*>";
                String s01 = Regex.Matches(svg, pattern, RegexOptions.IgnoreCase)[0].Value;

                if (s01.Length > 10) {
                    //取得width
                    pattern = "width[ ]*=[ ]*([\"][^a-z]*[\"]|['][^a-z]*['])";
                    s01 = Regex.Matches(svg, pattern, RegexOptions.IgnoreCase)[0].Value;
                    s01 = s01.Substring(s01.IndexOf('=') + 1);
                    s01 = s01.Replace("\"", "").Replace("'", "");
                    int_svg_w = func_取得_int(s01);

                    //取得height
                    pattern = "height[ ]*=[ ]*([\"][^a-z]*[\"]|['][^a-z]*['])";
                    s01 = Regex.Matches(svg, pattern, RegexOptions.IgnoreCase)[0].Value;
                    s01 = s01.Substring(s01.IndexOf('=') + 1);
                    s01 = s01.Replace("\"", "").Replace("'", "");
                    int_svg_h = func_取得_int(s01);
                }
            } catch {

                bool_wh = false;

            }

            //<svg viewBox="0 0 50 50"
            try {
                string pattern = "viewbox[ ]*=[ ]*([\"][^a-z]*[\"]|['][^a-z]*['])";
                String s01 = Regex.Matches(svg, pattern, RegexOptions.IgnoreCase)[0].Value;

                s01 = s01.Substring(s01.IndexOf('=') + 1);
                s01 = s01.Replace("\"", "").Replace("'", "");
                s01 = s01.Trim();
                if (s01.Contains(",")) {
                    s01 = s01.Replace(" ", "");
                } else {
                    s01 = s01.Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ");
                    s01 = s01.Replace(" ", ",");
                }

                String[] s02 = s01.Split(',');
                int_svg_w = func_取得_int(s02[2]);
                int_svg_h = func_取得_int(s02[3]);



            } catch {
                bool_viewbox = false;
            }




            if (bool_wh == true && bool_viewbox == false) {

                int svg_i = svg.IndexOf("<svg", StringComparison.CurrentCultureIgnoreCase);
                String svg1 = svg.Substring(0, svg_i);
                String svg2 = svg.Substring(svg_i + 4);
                svg = svg1 + $"<svg viewBox=\"0 0 {int_svg_w} {int_svg_h}\" " + svg2;

            }


            return svg;

        }



    }

}
