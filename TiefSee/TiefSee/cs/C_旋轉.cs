using TiefSee.W;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace TiefSee {
    public class C_旋轉 {






        MainWindow M;
        public int int_旋轉;
        public bool bool_水平鏡像;
        public bool bool_垂直鏡像;
        public U_menu u_menu_旋轉 ;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        public C_旋轉(MainWindow m) {


            this.M = m;
            u_menu_旋轉 = new U_menu(m);

            M.but_旋轉.Click += (sender, e) => {
                u_menu_旋轉.func_open(M.but_旋轉);
            };

            u_menu_旋轉.func_add_menu("順時針 90°", null, () => {
                func_旋轉_順時針_90();
            });
            u_menu_旋轉.func_add_menu("逆時針 90°", null, () => {
                func_旋轉_逆時針_90();
            });

            u_menu_旋轉.func_add_水平線();

            u_menu_旋轉.func_add_menu("水平鏡像", null, () => {
                func_旋轉_水平();
            });
            u_menu_旋轉.func_add_menu("垂直鏡像", null, () => {
                func_旋轉_垂直();
            });

            u_menu_旋轉.func_add_水平線();

            u_menu_旋轉.func_add_menu("初始化旋轉", null, () => {

                if (int_旋轉 == 270) {
                    int_旋轉 = -90;
                }

                if (int_旋轉 == 360) {
                    int_旋轉 = 0;
                }

                fun_旋轉動畫(
                    M.grid_img, int_旋轉, 0,
                    bool_水平鏡像, false,
                    bool_垂直鏡像, false
                );

                int_旋轉 = 0;
                bool_水平鏡像 = false;
                bool_垂直鏡像 = false;

                // fun_初始化旋轉(0);
            });


   


           

        }

        DateTime dt_避免連續旋轉 = DateTime.Now;
        int int_旋轉後延遲 = 400;

        /// <summary>
        /// 
        /// </summary>
        public void func_旋轉_順時針_90() {

            if (DateTime.Now < dt_避免連續旋轉) {
                return;
            }
            dt_避免連續旋轉 = DateTime.Now.AddMilliseconds(int_旋轉後延遲);


            int 角度_前 = int_旋轉;
            int_旋轉 += 90;

            fun_旋轉動畫(
                M.grid_img, 角度_前, int_旋轉,
                bool_水平鏡像, bool_水平鏡像,
                bool_垂直鏡像, bool_垂直鏡像
            );

            if (int_旋轉 >= 360) {
                int_旋轉 = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void func_旋轉_逆時針_90() {

            if (DateTime.Now < dt_避免連續旋轉) {
                return;
            }
            dt_避免連續旋轉 = DateTime.Now.AddMilliseconds(int_旋轉後延遲);

            int 角度_前 = int_旋轉;
            int_旋轉 -= 90;

            fun_旋轉動畫(
                M.grid_img, 角度_前, int_旋轉,
                bool_水平鏡像, bool_水平鏡像,
                bool_垂直鏡像, bool_垂直鏡像
            );

            if (int_旋轉 <= 0) {
                int_旋轉 = 360 + int_旋轉;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void func_旋轉_水平() {

            if (DateTime.Now < dt_避免連續旋轉) {
                return;
            }
            dt_避免連續旋轉 = DateTime.Now.AddMilliseconds(int_旋轉後延遲);

            bool b_鏡像_前 = bool_水平鏡像;

            bool_水平鏡像 = !bool_水平鏡像;

            fun_旋轉動畫(
                M.grid_img, int_旋轉, int_旋轉,
                b_鏡像_前, bool_水平鏡像,
                bool_垂直鏡像, bool_垂直鏡像
            );
        }



        /// <summary>
        /// 
        /// </summary>
        public void func_旋轉_垂直() {

            if (DateTime.Now < dt_避免連續旋轉) {
                return;
            }
            dt_避免連續旋轉 = DateTime.Now.AddMilliseconds(int_旋轉後延遲);

            bool b_鏡像_前 = bool_垂直鏡像;

            bool_垂直鏡像 = !bool_垂直鏡像;

            fun_旋轉動畫(
                M.grid_img, int_旋轉, int_旋轉,
                bool_水平鏡像, bool_水平鏡像,
                b_鏡像_前, bool_垂直鏡像
            );
        }


        /// <summary>
        /// 
        /// </summary>
        public void fun_初始化旋轉(int int_Orientation) {

            if (int_Orientation == 0) {
                int_旋轉 = 0;
                bool_水平鏡像 = false;
                bool_垂直鏡像 = false;
            } else
            if (int_Orientation == 1) {
                int_旋轉 = 0;
                bool_水平鏡像 = false;
                bool_垂直鏡像 = false;
            } else
            if (int_Orientation == 2) {
                int_旋轉 = 0;
                bool_水平鏡像 = true;
                bool_垂直鏡像 = false;
            } else
            if (int_Orientation == 3) {
                int_旋轉 = 180;
                bool_水平鏡像 = false;
                bool_垂直鏡像 = false;
            } else
            if (int_Orientation == 4) {
                int_旋轉 = 180;
                bool_水平鏡像 = true;
                bool_垂直鏡像 = false;
            } else
            if (int_Orientation == 5) {
                int_旋轉 = 90;
                bool_水平鏡像 = true;
                bool_垂直鏡像 = false;
            } else
            if (int_Orientation == 6) {
                int_旋轉 = 90;
                bool_水平鏡像 = false;
                bool_垂直鏡像 = false;
            } else
            if (int_Orientation == 7) {
                int_旋轉 = 270;
                bool_水平鏡像 = true;
                bool_垂直鏡像 = false;
            } else
            if (int_Orientation == 8) {
                int_旋轉 = 270;
                bool_水平鏡像 = false;
                bool_垂直鏡像 = false;
            }


            fun_直接旋轉(M.grid_img, int_旋轉, bool_水平鏡像, bool_垂直鏡像);

        }



        public void fun_直接旋轉(FrameworkElement f, double d_角度_前, bool bool_水平鏡像_前, bool bool_垂直鏡像_前) {


            if (f == null)
                return;



            RotateTransform transform = new RotateTransform()
            {
                CenterX = 0.5,
                CenterY = 0.5,
                Angle = d_角度_前,
            };

            ScaleTransform transform2 = new ScaleTransform()
            {
                CenterX = 0.5,
                CenterY = 0.5,
                ScaleX = (bool_水平鏡像_前) ? -1 : 1,
                ScaleY = (bool_垂直鏡像_前) ? -1 : 1,
            };


            TransformGroup tf = new TransformGroup();
            tf.Children.Add(transform);
            tf.Children.Add(transform2);


            f.LayoutTransform = tf;

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="s_移動方式"></param>
        private void fun_旋轉動畫(FrameworkElement f, double d_角度_前, double d_角度_後, bool bool_水平鏡像_前, bool bool_水平鏡像_後, bool bool_垂直鏡像_前, bool bool_垂直鏡像_後) {

            if (f == null)
                return;


            if (M.s_img_type_顯示類型 == "WEB") {

                String int_角度 = Int32.Parse(d_角度_後 + "") + "";
                String bool_水平 = (bool_水平鏡像_後) ? "-1" : "1";
                String bool_垂直 = (bool_垂直鏡像_後) ? "-1" : "1";

                M.img_web.Document.InvokeScript("fun_套用旋轉", new Object[] { d_角度_前+"", d_角度_後+"",
                 (bool_水平鏡像_前) ? "-1" : "1", (bool_水平鏡像_後) ? "-1" : "1",
                 (bool_垂直鏡像_前) ? "-1" : "1", (bool_垂直鏡像_後) ? "-1" : "1"});

                return;
            }


            double d_運行時間 = 0.3d;


            //初始化角度
            RotateTransform transform = new RotateTransform()
            {
                CenterX = 0.5,
                CenterY = 0.5,
                Angle = d_角度_前,
            };

            //初始化縮放
            ScaleTransform transform2 = new ScaleTransform()
            {
                CenterX = 0.5,
                CenterY = 0.5,
                ScaleX = (bool_水平鏡像_前) ? -1 : 1,
                ScaleY = (bool_垂直鏡像_前) ? -1 : 1,
            };


            //放入變形群組
            TransformGroup tf = new TransformGroup();
            tf.Children.Add(transform);
            tf.Children.Add(transform2);

            //套用
            f.LayoutTransform = tf;

            //設定動畫執行時間
            DoubleAnimation animation = new DoubleAnimation()
            {
                From = d_角度_前,
                To = d_角度_後,
                Duration = TimeSpan.FromSeconds(d_運行時間),
                //RepeatBehavior = RepeatBehavior.Forever
            };

            //設定動畫執行時間
            DoubleAnimation animation2 = new DoubleAnimation()
            {
                From = (bool_水平鏡像_前) ? -1 : 1,
                To = (bool_水平鏡像_後) ? -1 : 1,
                Duration = TimeSpan.FromSeconds(d_運行時間),
            };

            //設定動畫執行時間
            DoubleAnimation animation3 = new DoubleAnimation()
            {
                From = (bool_垂直鏡像_前) ? -1 : 1,
                To = (bool_垂直鏡像_後) ? -1 : 1,
                Duration = TimeSpan.FromSeconds(d_運行時間),
            };

            //執行動畫
            transform.BeginAnimation(RotateTransform.AngleProperty, animation);

            if (bool_水平鏡像_前 != bool_水平鏡像_後)
                transform2.BeginAnimation(ScaleTransform.ScaleXProperty, animation2);

            if (bool_垂直鏡像_前 != bool_垂直鏡像_後)
                transform2.BeginAnimation(ScaleTransform.ScaleYProperty, animation3);






        }






    }






}
