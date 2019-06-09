using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;


namespace TiefSee {

    public partial class MainWindow {



        private void event_視窗() {

            event_視窗右上角的控制按鈕();

            //讓右下角可以拖曳
            border_右下角.PreviewMouseLeftButtonDown += (sender, e) => {
                if (this.WindowState != WindowState.Maximized)
                    if (fun_判斷滑鼠是否在右下角())
                        c_視窗改變大小.ResizeWindow(C_視窗拖曳改變大小.ResizeDirection.BottomRight);
            };



            //全熒幕時，取消右下角的拖曳
            this.StateChanged += (sender, e) => {

                if (bool_全螢幕 && this.WindowState != WindowState.Maximized) {
                    func_全螢幕(false);
                }

                if (this.WindowState == WindowState.Maximized) {
                    border_右下角.Visibility = Visibility.Collapsed;
                } else {
                    border_右下角.Visibility = Visibility.Visible;
                }


                //全螢幕時，大量瀏覽模式 webbrowser 的 Margin 拿掉
                if (u_大量瀏覽模式 != null) {
                    if (this.WindowState == WindowState.Maximized) {
                        u_大量瀏覽模式.WindowsFormsHost_01.Margin = new Thickness(0);
                    } else {
                        u_大量瀏覽模式.WindowsFormsHost_01.Margin = new Thickness(3, 0, 3, 3);
                    }
                }

            };





            //雙擊全螢幕
            this.MouseDoubleClick += (sender, e) => {

                if (e.RightButton == MouseButtonState.Pressed) {
                    return;
                }

                Object obj = e.OriginalSource;



                if (obj == scrollViewer_工具列 || obj == border_工具列_外框 || obj == dockPanel_標題 || obj == lab_title ||
                    obj == grid_img || obj == img || obj == img_voide ||
                    obj == img_gif || obj == scrollviewer_1 ||
                    obj == bor_外框_圖片size || obj == bor_外框_圖片類型 || obj == bor_外框_圖片修改時間 ||
                   (u_大量瀏覽模式 != null && obj == u_大量瀏覽模式.dockPanel_功能列)) {

                    if (this.WindowState != WindowState.Maximized)
                        this.WindowState = WindowState.Maximized;
                    else
                        this.WindowState = WindowState.Normal;
                }



                /*
                if (e.OriginalSource is Border)//按鈕
                    return;

                if (e.OriginalSource is IServiceProvider && e.OriginalSource != lab_title)//文字方塊
                    return;

                if (e.OriginalSource == stackPanel_exif_box) {
                    return;
                }

                if (this.WindowState != WindowState.Maximized)
                    this.WindowState = WindowState.Maximized;
                else
                    this.WindowState = WindowState.Normal;*/
            };
            stackPanel_exif_box.MouseDoubleClick += (sender, e) => {
                if (this.WindowState != WindowState.Maximized)
                    this.WindowState = WindowState.Maximized;
                else
                    this.WindowState = WindowState.Normal;
            };



        }



        #region 視窗右上角的控制按鈕
        private void event_視窗右上角的控制按鈕() {

            this.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, this.OnCloseWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, this.OnMaximizeWindow, this.ResizeWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, this.OnMinimizeWindow, this.OnCanMinimizeWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, this.OnRestoreWindow, this.ResizeWindow));

            //全螢幕或視窗化時，控制右上角的按鈕顯示或隱藏視窗化與全螢幕華
            this.StateChanged += (sender, e) => {
                if (this.WindowState == WindowState.Maximized) {
                    Restore.Visibility = Visibility.Visible;
                    Maximize.Visibility = Visibility.Collapsed;
                } else {
                    Restore.Visibility = Visibility.Collapsed;
                    Maximize.Visibility = Visibility.Visible;
                }

                //全螢幕的狀態下，隱藏邊框
                if (this.WindowState == WindowState.Maximized) {
                    border_視窗外框.Visibility = Visibility.Collapsed;
                } else {
                    border_視窗外框.Visibility = Visibility.Visible;
                }
            };


            //避免一開始是全螢幕的情況下，無法使用右上角的按鈕來視窗化
            if (this.WindowState == WindowState.Maximized) {
                Restore.Visibility = Visibility.Visible;
                Maximize.Visibility = Visibility.Collapsed;
            }

        }

        private void ResizeWindow(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = this.ResizeMode == ResizeMode.CanResize || this.ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private void OnCanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = this.ResizeMode != ResizeMode.NoResize;
        }

        private void OnCloseWindow(object target, ExecutedRoutedEventArgs e) {
            SystemCommands.CloseWindow(this);
        }

        private void OnMaximizeWindow(object target, ExecutedRoutedEventArgs e) {
            SystemCommands.MaximizeWindow(this);
        }

        private void OnMinimizeWindow(object target, ExecutedRoutedEventArgs e) {
            SystemCommands.MinimizeWindow(this);
        }

        private void OnRestoreWindow(object target, ExecutedRoutedEventArgs e) {
            SystemCommands.RestoreWindow(this);
        }

        #endregion







    }






    public class C_視窗拖曳改變大小 {


        private Window M;

        public C_視窗拖曳改變大小(Window m) {
            this.M = m;
        }


        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
            //Debug.WriteLine("WndProc messages: " + msg.ToString());

            if (msg == WM_SYSCOMMAND) {
                //Debug.WriteLine("WndProc messages: " + msg.ToString());
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// 初始化，在『InitializeComponent();』後面呼叫
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MainWindow_SourceInitialized(object sender, System.EventArgs e) {
            hwndSource = PresentationSource.FromVisual((Visual)sender) as HwndSource;
            hwndSource.AddHook(new HwndSourceHook(WndProc));
        }








        private const int WM_SYSCOMMAND = 0x112;
        private HwndSource hwndSource;
        IntPtr retInt = IntPtr.Zero;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        public enum ResizeDirection {
            Left = 1,
            Right = 2,
            Top = 3,
            TopLeft = 4,
            TopRight = 5,
            Bottom = 6,
            BottomLeft = 7,
            BottomRight = 8,
            Move = 9
        }

        public void ResizeWindow(ResizeDirection direction) {
            SendMessage(hwndSource.Handle, WM_SYSCOMMAND, (IntPtr)(61440 + direction), IntPtr.Zero);
        }


        public void ResetCursor(object sender, MouseEventArgs e) {
            if (Mouse.LeftButton != MouseButtonState.Pressed) {
                M.Cursor = Cursors.Arrow;
            }
        }

    }




}
