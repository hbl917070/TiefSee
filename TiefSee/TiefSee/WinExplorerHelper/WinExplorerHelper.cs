using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;


namespace 取得檔案總管選取的檔案 {
    public static class WinExplorerHelper {




        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static object GetSizeFormat(long length) {
            if (length >= 1024 * 1024 * 1024) {
                return $"{length / (double)(1024 * 1024 * 1024):0.##} GB";
            }
            if (length >= 1024 * 1024) {
                return $"{length / (double)(1024 * 1024):0.##} MB";
            }
            if (length >= 1024) {
                return $"{length / (double)1024:0.##} KB";
            }

            return $"{length} B";
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetSelectedItem() {
            var foregroundHwnd = NativeMethods.GetForegroundWindow();

            if (foregroundHwnd == IntPtr.Zero) {
                return null;
            }

            var shellWindows = (IShellWindows)Activator.CreateInstance(CLSID.ShellWindowsType);

            string fileName = null;

            if (IsDesktopWindow(foregroundHwnd)) {
                var pvarLoc = new object();

                shellWindows.FindWindowSW(ref pvarLoc, ref pvarLoc, ShellWindowTypeConstants.SWC_DESKTOP, out var desktopHwnd, ShellWindowFindWindowOptions.SWFO_NEEDDISPATCH, out var webBrowserApp);

                if (!IsCaretActive(desktopHwnd)) {
                    fileName = GetSelectedItemCore(webBrowserApp);
                }
            } else {
                for (int i = 0; i < shellWindows.Count; i++) {

                    try {
                        var webBrowserApp = (IWebBrowserApp)shellWindows.Item(i);
                        var hwnd = webBrowserApp.get_HWND();
                        if (hwnd == foregroundHwnd && !IsCaretActive(hwnd)) {
                            fileName = GetSelectedItemCore(webBrowserApp);
                            break;
                        }
                    } catch  {   }
                    
                }
            }

            Marshal.FinalReleaseComObject(shellWindows);

            if (fileName == null || (!File.Exists(fileName) && !Directory.Exists(fileName))) {
                return null;
            }

            return fileName;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="webBrowserApp"></param>
        /// <returns></returns>
        private static string GetSelectedItemCore(IWebBrowserApp webBrowserApp) {

            try {
                var serviceProvider = webBrowserApp.QueryInterface<IServiceProvider>();
                var shellBrowser = serviceProvider.QueryService<IShellBrowser>(SID.STopLevelBrowser);
                var shellView = shellBrowser.QueryActiveShellView();
                var folderView = shellView.QueryInterface<IFolderView>();
                int focus = folderView.GetFocusedItem();
                var persistFolder2 = folderView.GetFolder<IPersistFolder2>();
                var shellFolder = persistFolder2.QueryInterface<IShellFolder>();
                var pidl = folderView.Item(focus);
                var fileName = shellFolder.GetDisplayNameOf(pidl, SHGDNF.FORPARSING);

                // Cleanup
                Marshal.FreeCoTaskMem(pidl);

                Marshal.FinalReleaseComObject(shellFolder);
                Marshal.FinalReleaseComObject(folderView);
                Marshal.FinalReleaseComObject(shellBrowser);
                Marshal.FinalReleaseComObject(serviceProvider);


                return fileName;

            } catch (System.Runtime.InteropServices.COMException) {

                return null;
            }

         
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        private static bool IsCaretActive(IntPtr hwnd) {
            var threadId = NativeMethods.GetWindowThreadProcessId(hwnd, IntPtr.Zero);

            var info = new GUITHREADINFO {
                cbSize = Marshal.SizeOf<GUITHREADINFO>()
            };

            NativeMethods.GetGUIThreadInfo(threadId, ref info);

            return info.flags != 0 || info.hwndCaret != IntPtr.Zero;
        }


        /// <summary>
        /// 判斷是否在桌面
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        private static bool IsDesktopWindow(IntPtr hwnd) {
            var classNameBuilder = new StringBuilder(Consts.MAX_PATH);

            NativeMethods.GetClassName(hwnd, classNameBuilder, Consts.MAX_PATH);

            var className = classNameBuilder.ToString();

            //桌面 Progman
            //桌面-更改壁紙後 WorkerW
            if (className != "Progman" && className != "WorkerW") {
                return false;
            }

            return NativeMethods.FindWindowEx(hwnd, IntPtr.Zero, "SHELLDLL_DefView", null) != IntPtr.Zero;
        }
    }
}
