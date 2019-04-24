using TiefSee.cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Resources;

namespace TiefSee {



    public class C_右下角圖示 {


        private System.Windows.Forms.NotifyIcon nIcon = new System.Windows.Forms.NotifyIcon();
        private MainWindow MM;

        /// <summary>
        /// 
        /// </summary>
        public C_右下角圖示() {

            MM = new MainWindow("notify_icon", "");



            var uri = new Uri("imgs/5.ico", UriKind.RelativeOrAbsolute);
            StreamResourceInfo sri = Application.GetResourceStream(uri);
            nIcon.Icon = new System.Drawing.Icon(sri.Stream);

            nIcon.Text = "TiefSee 快速啟動";
            nIcon.Visible = true;









            var cm = new System.Windows.Forms.ContextMenu();

            cm.MenuItems.Add("New", new EventHandler((sender2, e2) => {

                new MainWindow("open_img", MM.fun_執行檔路徑() + "\\data\\imgs\\start.png").Show();

            }));

            cm.MenuItems.Add("結束「快速啟動」", new EventHandler((sender2, e2) => {

                nIcon.Visible = false;
                MM.MainWindow_Closing(null, null);
                MM.Close();

            }));

            nIcon.ContextMenu = cm;


            nIcon.DoubleClick += (sender, e) => {
                new MainWindow("open_img", MM.fun_執行檔路徑() + "\\data\\imgs\\start.png").Show();

            };

            /*nIcon.BalloonTipTitle = "ttt";
            nIcon.BalloonTipText = "xxx";
            nIcon.ShowBalloonTip(1);*/


            //nIcon.ShowBalloonTip(3000, "Hi", "This is a BallonTip from Windows Notification", System.Windows.Forms.ToolTipIcon.Warning);

            //new Form1().Show();
        }




        public void func_顯示() {
            nIcon.Visible = true;
        }

        public void func_隱藏() {
            nIcon.Visible = false;
        }



    }



}
