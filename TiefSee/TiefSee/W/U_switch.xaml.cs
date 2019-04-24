using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TiefSee.W {
    /// <summary>
    /// U_switch.xaml 的互動邏輯
    /// </summary>
    public partial class U_switch : UserControl {
        public U_switch() {
            InitializeComponent();

            border01.MouseDown += (sender, e) => {
                IsChecked = !IsChecked;
            };

        }

        /// <summary>
        /// 值 改變後觸發的事件
        /// </summary>
        public Action<U_switch, bool> Checked = new Action<U_switch, bool>((U_switch u, bool b2) => { });

        private Boolean b;
        /// <summary>
        /// 設定或取得目前是否勾選
        /// </summary>
        public bool IsChecked {
            get {


                return b;
            }
            set {
                b = value;

                if (b) {
                    func_移動動畫(ellipse01, 0, 19, "X");
                    func_縮放動畫(bac, 0, 1, "");
                } else {
                    func_移動動畫(ellipse01, 19, 0, "X");
                    func_縮放動畫(bac, 1,0 , "");
                }

                Checked(this, b);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="s_移動方式"></param>
        public void func_縮放動畫(FrameworkElement f, double from, double to, String s_移動方式) {

            if (f == null)
                return;

            s_移動方式 = s_移動方式.ToUpper();

            
            //位移
            Storyboard storyboard2 = new Storyboard();
            DoubleAnimation growAnimation2 = new DoubleAnimation();
            growAnimation2.Duration = (Duration)TimeSpan.FromSeconds(0.15f);

            growAnimation2.Completed += (sender, e) => {//完成時執行

            };

            f.RenderTransform = new ScaleTransform {  ScaleX = to  , ScaleY =  to};

            growAnimation2.From = from;
            growAnimation2.To = to;
            Storyboard.SetTargetProperty(growAnimation2, new PropertyPath("RenderTransform.ScaleX"));
           Storyboard.SetTarget(growAnimation2, f);
            storyboard2.Children.Add(growAnimation2);
            storyboard2.Begin();

            Storyboard.SetTargetProperty(growAnimation2, new PropertyPath("RenderTransform.ScaleY"));
            Storyboard.SetTarget(growAnimation2, f);

            storyboard2.Children.Add(growAnimation2);
            storyboard2.Begin();
            
            //----------------------
            
            Storyboard storyboard3 = new Storyboard();
            DoubleAnimation growAnimation3 = new DoubleAnimation();
            growAnimation3.Duration = (Duration)TimeSpan.FromSeconds(0.3f);

            growAnimation3.Completed += (sender, e) => {//完成時執行
                //f.Visibility = Visibility.Collapsed;
            };

            //f.RenderTransform = new TranslateTransform();

            growAnimation3.From = from;
            growAnimation3.To = to;

            Storyboard.SetTargetProperty(growAnimation3, new PropertyPath("Opacity"));
            Storyboard.SetTarget(growAnimation3, f);

            storyboard3.Children.Add(growAnimation3);
            storyboard3.Begin();

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="s_移動方式"></param>
        public void func_移動動畫(FrameworkElement f, double from, double to, String s_移動方式) {

            if (f == null)
                return;

            s_移動方式 = s_移動方式.ToUpper();


            //位移
            Storyboard storyboard2 = new Storyboard();
            DoubleAnimation growAnimation2 = new DoubleAnimation();
            growAnimation2.Duration = (Duration)TimeSpan.FromSeconds(0.15f);

            growAnimation2.Completed += (sender, e) => {//完成時執行

            };

            f.RenderTransform = new TranslateTransform();

            growAnimation2.From = from;
            growAnimation2.To = to;

            Storyboard.SetTargetProperty(growAnimation2, new PropertyPath("(FrameworkElement.RenderTransform).(TranslateTransform." + s_移動方式 + ")"));
            Storyboard.SetTarget(growAnimation2, f);

            storyboard2.Children.Add(growAnimation2);
            storyboard2.Begin();

            //----------------------
            /*
            Storyboard storyboard3 = new Storyboard();
            DoubleAnimation growAnimation3 = new DoubleAnimation();
            growAnimation3.Duration = (Duration)TimeSpan.FromSeconds(0.15f);

            growAnimation3.Completed += (sender, e) => {//完成時執行
                //f.Visibility = Visibility.Collapsed;
            };

            //f.RenderTransform = new TranslateTransform();

            growAnimation3.From = 0.5;
            growAnimation3.To = 1;

            Storyboard.SetTargetProperty(growAnimation3, new PropertyPath("Opacity"));
            Storyboard.SetTarget(growAnimation3, f);

            storyboard3.Children.Add(growAnimation3);
            storyboard3.Begin();*/

        }


    }
}
