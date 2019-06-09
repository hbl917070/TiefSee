using Gif.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Forms_合成GIF {
    public partial class Form1 : Form {



        int int_width = 500;
        int int_height = 300;
        List<Data_img> ar_img = new List<Data_img>();
        bool bool_程式執行中 = true;

        List<StreamReader> ar_鎖定檔案 = new List<StreamReader>();//避免執行到一半檔案被刪除

        public Form1() {
            InitializeComponent();

            fun_讀取input_xml();

            label_size.Text = "w:" + int_width + "     h:" + int_height;
            textBox_w.Text = int_width + "";
            textBox_h.Text = int_height + "";

            textBox_w.TextChanged += (senderm, e) => {
                if (textBox_w.Focused)
                    try {
                        int ww = Int32.Parse(textBox_w.Text);
                        int hh = (int)(int_height * 1.0f / int_width * 1.0f * ww);
                        textBox_h.Text = hh + "";
                    } catch { }
            };

            textBox_h.TextChanged += (senderm, e) => {
                if (textBox_h.Focused)
                    try {
                        int hh = Int32.Parse(textBox_h.Text);
                        int ww = (int)(int_width * 1.0f / int_height * 1.0f * hh);
                        textBox_w.Text = ww + "";
                    } catch { }
            };


            this.FormClosing += (sender, e) => {
                bool_程式執行中 = false;
            };

        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e22"></param>
        private void button1_Click(object sender, EventArgs e22) {



            int width = 200; //寬度
            int heigt = 200;  //高度

            bool bool_修改size = checkBox_size.Checked;

            //如果需要修改size的話
            if (bool_修改size)
                try {
                    width = Int32.Parse(textBox_w.Text.Trim()); //寬度
                    heigt = Int32.Parse(textBox_h.Text.Trim()); ; //高度
                } catch {
                    MessageBox.Show("Image size error", "Error");
                    return;
                }
            if (width < 1 || heigt < 1) {
                MessageBox.Show("Image size error", "Error");
                return;
            }



            DateTime time_start = DateTime.Now;//計時開始 取得目前時間

            label_進度.Text = "0%";
            button_執行.Visible = false;//隱藏執行按鈕
            //this.Enabled = false;

            string outputFilePath = textBox_output_path.Text;

            //如果檔案已經存在，就先刪除。否則會有ＢＵＧ
            if (File.Exists(outputFilePath)) {
                File.Delete(outputFilePath);
            }

            new Thread(() => {

                AnimatedGifEncoder gif = new AnimatedGifEncoder();
                gif.Start(outputFilePath);
                gif.SetRepeat(0); //1表示只动一次，0：表示循环，n：表示循环n次

                for (int i = 0; i < ar_img.Count; i++) {

                    //程式被結束前，結束迴圈
                    if (bool_程式執行中 == false) {
                        gif.Finish();
                        return;
                    }

                    //必當檔案不存在
                    if (File.Exists(ar_img[i].path) == false) {
                        MessageBox.Show("找不到檔案:\n" + ar_img[i].path, "Erroe");
                        break;
                    }

                    //修改進度
                    this.Invoke(new MethodInvoker(() => {//委託UI行緒
                        if (bool_程式執行中)
                            label_進度.Text = ((100 * 1.0f / ar_img.Count) * (i + 1)).ToString("0.0") + "%";
                    }));

                    //設定延遲
                    gif.SetDelay(ar_img[i].delay);

                    if (bool_修改size) {//強制修改大小

                        using (var img = Image.FromFile(ar_img[i].path)) {
                            //ImageFormat format = img.RawFormat;

                            using (Bitmap imgoutput = new Bitmap(img, width, heigt)) { //輸出一個新圖片
                                gif.AddFrame(imgoutput);
                            }
                        }

                    } else {//預設大小

                        using (var img = Image.FromFile(ar_img[i].path)) {
                            gif.AddFrame(img);
                            img.Dispose();
                        }

                    }

                }//for

                gif.Finish();


                this.Invoke(new MethodInvoker(() => {//委託UI行緒

                    DateTime time_end = DateTime.Now;//計時結束 取得目前時間            
                    String result2 = ((TimeSpan)(time_end - time_start)).TotalMilliseconds.ToString("0.0");//後面的時間減前面的時間後 轉型成TimeSpan即可印出時間差

                    button_執行.Visible = true;
                    //this.Enabled = true;

                    MessageBox.Show("運行時間：" + result2 + " 毫秒\n" +
                        "檔案大小：" + fun_判斷檔案大小(outputFilePath), "輸出完成");

                }));




            }).Start();


            // textBox1.Text=( result2 + " 毫秒");



        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public String fun_判斷檔案大小(String path) {

            if (File.Exists(path) == false)
                return "0";

            double len = 0;
            if (Directory.Exists(path)) {//資料夾
                DirectoryInfo dInfo = new DirectoryInfo(path);
                FileInfo[] fInfoArr = dInfo.GetFiles();
                foreach (var item in fInfoArr) {
                    len += item.Length;
                }
            } else {//檔案
                len = new FileInfo(path).Length;
            }

            if (len / 1024 < 1) {
                return len.ToString("0.0") + " B";
            } else if (len / (1024 * 1024) < 1) {
                return (len / (1024)).ToString("0.0") + " KB";
            } else if (len / (1024 * 1024 * 1024) < 1) {
                return (len / (1024 * 1024)).ToString("0.0") + " MB";
            } else if (len / (1024 * 1024 * 1024) / 1024 < 1) {
                return (len / (1024f * 1024f * 1024f)).ToString("0.00") + " GB";
            }

            return len.ToString("0.0");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_open_Click(object sender, EventArgs e) {


            SaveFileDialog openFileDialog1 = new SaveFileDialog();
            openFileDialog1.Filter = "GIF Image|*.gif";
            //openFileDialog1.Title = "select output path";

            String name = textBox_output_path.Text;
            name = name.Substring(name.LastIndexOf("\\") + 1);
            openFileDialog1.FileName = name;

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                textBox_output_path.Text = openFileDialog1.FileName;
            }




        }



        /// <summary>
        /// 
        /// </summary>
        private void fun_讀取input_xml() {


            String path_input_xml = System.Windows.Forms.Application.StartupPath + "\\input.xml";

            if (File.Exists(path_input_xml) == false) {
                MessageBox.Show("Can not find\n\n" + path_input_xml, "Error");
                Close();
                return;
            }


            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(path_input_xml);

            try {
                int_width = Int32.Parse(XmlDoc.SelectNodes("settings/width")[0].InnerText.Trim());
            } catch {
                MessageBox.Show("Can not find 'width'", "Error");
            }

            try {
                int_height = Int32.Parse(XmlDoc.SelectNodes("settings/height")[0].InnerText.Trim());
            } catch {
                MessageBox.Show("Can not find 'height'", "Error");
            }

            try {
                textBox_input_path.Text = XmlDoc.SelectNodes("settings/input_path")[0].InnerText.Trim();
            } catch {
                MessageBox.Show("Can not find 'input_path'", "Error");
            }

            try {
                textBox_output_path.Text = XmlDoc.SelectNodes("settings/output_path")[0].InnerText.Trim();
            } catch {
                MessageBox.Show("Can not find 'output_path'", "Error");
            }



            /*
            string s_桌面 = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            String name = textBox_input_path.Text;
            if (name.Substring(name.Length - 1) == "\\") {
                name = name.Substring(0, name.Length - 2);
            }
            name = name.Substring(name.LastIndexOf("\\") + 1);
            textBox_output_path.Text = s_桌面 + "\\" + name + ".gif";
            */

            XmlNodeList NodeLists = XmlDoc.SelectNodes("settings/img/item");

            foreach (XmlNode item in NodeLists) {

                String s_path = item.Attributes["path"].Value.Trim();

                Data_img data_img = new Data_img()
                {
                    path = s_path,
                    delay = Int32.Parse(item.Attributes["delay"].Value.Trim())
                };
                System.Console.WriteLine("---------------------- " + data_img.delay + "");
                ar_img.Add(data_img);

                if (File.Exists(s_path)) {
                    ar_鎖定檔案.Add(new StreamReader(data_img.path));//鎖住檔案，避免合成到一半檔案被刪除
                } else {
                    MessageBox.Show("Can not find\n\n" + s_path, "Error");
                    Close();
                    return;
                }
                                                             //new FileStream(data_img.path, FileMode.Append);
            }

        }



        public class Data_img {
            public String path = "";
            public int delay = 16;
        }



    }
}
