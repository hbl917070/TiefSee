using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace TiefSee.cs {

    class C_window_AERO {





        /// <summary>
        /// 
        /// </summary>
        /// <param name="M"></param>
        /// <returns></returns>
        public bool func_啟用毛玻璃(Window M) {


            //win10 啟用毛玻璃特效
            if (IsWindows10()) {

                try {
                    func_win10_aero(M);
                    return true;
                } catch {
                    return false;
                }
             
            }//win10



            //win7 啟用毛玻璃特效
            if (IsWindows7()) {

                try {
                    func_win7_aero(M);
                    return true;
                } catch {
                    return false;
                }

            }//win7



            //win8
            return false;

        }





        /// <summary>
        /// 判斷是否為 win10
        /// </summary>
        public static bool IsWindows10() {
            try {
                var reg = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
                string productName = (string)reg.GetValue("ProductName");
                return productName.StartsWith("Windows 10");
            } catch {
                return false;
            }

        }




        /// <summary>
        /// 判斷是否為 win7
        /// </summary>
        public static bool IsWindows7() {
            try {
                String s156 = System.Environment.OSVersion.Version.ToString();//取得作業系統地版本
                bool bbb = s156.Length > 3 && s156.Substring(0, 3) == "6.1";//win7
                return bbb;
            } catch {
                return false;
            }
        }







        #region win10毛玻璃

        internal enum AccentState {
            ACCENT_DISABLED = 0,
            ACCENT_ENABLE_GRADIENT = 1,
            ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
            ACCENT_ENABLE_BLURBEHIND = 3,
            ACCENT_INVALID_STATE = 4,

        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct AccentPolicy {
            public AccentState AccentState;
            public int AccentFlags;
            public int GradientColor;
            public int AnimationId;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct WindowCompositionAttributeData {
            public WindowCompositionAttribute Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }

        internal enum WindowCompositionAttribute {
            // ...
            WCA_ACCENT_POLICY = 19
            // ...
        }

        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);



        /// <summary>
        /// 設定aero
        /// </summary>
        /// <param name="w"></param>
        public void func_win10_aero(Window w) {
            var windowHelper = new WindowInteropHelper(w);

            var accent = new AccentPolicy();
            accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;

            var accentStructSize = Marshal.SizeOf(accent);

            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData();
            data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
            data.SizeOfData = accentStructSize;
            data.Data = accentPtr;

            SetWindowCompositionAttribute(windowHelper.Handle, ref data);

            Marshal.FreeHGlobal(accentPtr);
        }

        #endregion










        #region win7毛玻璃

        [StructLayout(LayoutKind.Sequential)]
        public struct MARGINS {
            public int cxLeftWidth;      // width of left border that retains its size  
            public int cxRightWidth;     // width of right border that retains its size  
            public int cyTopHeight;      // height of top border that retains its size  
            public int cyBottomHeight;   // height of bottom border that retains its size  
        };

        [DllImport("DwmApi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(
            IntPtr hwnd,
            ref MARGINS pMarInset);


        /// <summary>
        /// 啟用win7 aero
        /// </summary>
        /// <param name="M"></param>
        private void func_win7_aero(Window M) {
           

                //取得最高的螢幕
                int h = 0;
                foreach (var screen in System.Windows.Forms.Screen.AllScreens) {//列出所有螢幕資訊
                    int xx = screen.Bounds.Y + screen.Bounds.Height;
                    if (xx > h)
                        h = xx;
                }
                h += 50;


                // Obtain the window handle for WPF application  
                IntPtr mainWindowPtr = new WindowInteropHelper(M).Handle;
                HwndSource mainWindowSrc = HwndSource.FromHwnd(mainWindowPtr);
                mainWindowSrc.CompositionTarget.BackgroundColor = System.Windows.Media.Color.FromArgb(0, 0, 0, 0);

                // Get System Dpi  
                System.Drawing.Graphics desktop = System.Drawing.Graphics.FromHwnd(mainWindowPtr);
                float DesktopDpiX = desktop.DpiX;
                float DesktopDpiY = desktop.DpiY;

                // Set Margins  
                MARGINS margins = new MARGINS();

                // Extend glass frame into client area  
                // Note that the default desktop Dpi is 96dpi. The  margins are  
                // adjusted for the system Dpi.  
                margins.cxLeftWidth = Convert.ToInt32(0 * (DesktopDpiX / 96));
                margins.cxRightWidth = Convert.ToInt32(0 * (DesktopDpiX / 96));
                margins.cyTopHeight = Convert.ToInt32(((int)h) * (DesktopDpiX / 96));
                margins.cyBottomHeight = Convert.ToInt32(0 * (DesktopDpiX / 96));

                int hr = DwmExtendFrameIntoClientArea(mainWindowSrc.Handle, ref margins);
                //  
                if (hr < 0) {
                    //DwmExtendFrameIntoClientArea Failed  
                }

                M.BorderThickness = new Thickness(10, 0, 10, h);

      
        }

        #endregion



    }
}
