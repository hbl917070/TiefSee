using TiefSee.cs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TiefSee.W {
    public partial class F_web : Form {

        MainWindow M;

        public F_web(MainWindow m) {

            this.M = m;

            InitializeComponent();


            //  webBrowser1.Visible=false;

            webBrowser1 = new obj_webbrowser();

            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.Margin = new System.Windows.Forms.Padding(0);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScriptErrorsSuppressed = true;
            //this.webBrowser1.Size = new System.Drawing.Size(112, 91);
            this.webBrowser1.TabIndex = 0;
            // this.webBrowser1.WebBrowserShortcutsEnabled = false;

       
            this.Controls.Add(webBrowser1);


        }






        public class obj_webbrowser : WebBrowser {

            public obj_webbrowser() {
            }

        }



    }




}
