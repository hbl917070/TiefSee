using TiefSee.W;
using IconHandler;
using ShellTestApp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TiefSee.cs {



    public class C_按鈕選單_其他程式開啟 {


        MainWindow M;
        List<String> ar_file = new List<string>();//開始選單裡面的所有檔案
        public U_menu u_menu_用外部程式開啟 ;


        public C_按鈕選單_其他程式開啟(MainWindow m) {

            this.M = m;
            u_menu_用外部程式開啟 = new U_menu(m);

            //
            //
            //



            //使用執行緒讀取名單，加快啟用程式的速度
            new Thread(() => {

                var ar = fun_處理名單();

                C_adapter.fun_UI執行緒(() => {

                    event_其他程式啟動_選單(ar);

                });

            }).Start();


        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String fun_作業系統的槽() {
            String sss = "C";
            try {
                sss = Environment.GetFolderPath(Environment.SpecialFolder.Windows).Substring(0, 1);
            } catch  {  }         
            return sss;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<data_開啟程式> fun_處理名單() {


            List<data_開啟程式> ar = new List<data_開啟程式>();//處理後的名單


            if (Directory.Exists(fun_作業系統的槽() + @":\ProgramData\Microsoft\Windows\Start Menu\Programs")) {
                fun_遍歷開始選單資料夾(fun_作業系統的槽() + @":\ProgramData\Microsoft\Windows\Start Menu\Programs");//windows的開始
            } else {

            }


            //路徑沒有重複才新增
            Action<data_開啟程式> ac_add2 = (data_開啟程式 data) => {
                if (data.path.ToLower().Contains("uninstall"))
                    return;

                //判斷是否可以新增
                bool bool_可以新增 = true;
                for (int i = 0; i < ar.Count; i++) {
                    if (data != null)
                        if (data.path.ToUpper() == ar[i].path.ToUpper()) {
                            bool_可以新增 = false;
                            break;
                        }
                }
                if (bool_可以新增 && data != null)
                    ar.Add(data);
            };






            //跟開始選單裡面的程式 join
            foreach (data_開啟程式 item in M.c_set.ar_外部開啟) {

                data_開啟程式 cc = null;

                if (item.type == "start_menu") {//開始選單

                    try {
                        foreach (var item2 in ar_file) {

                            String path_name = item2;//使用檔名判斷，而不是使用完整路徑
                            if (path_name.Contains("\\")) {
                                path_name = path_name.Substring(path_name.LastIndexOf('\\'));
                            }

                            if (path_name.ToUpper().Contains(item.path.ToUpper()))
                                if (item2.Length > 5 && item2.Substring(item2.Length - 3).ToUpper() == "LNK") {//超連結才新增
                                    cc = new data_開啟程式();
                                    cc.type = "absolute";
                                    try {
                                        cc.path = ResolveShortcut(item2);
                                    } catch { }
                                    cc.lnk_name = item2;
                                    cc.name = item.name;

                                    ac_add2(cc);
                                }
                        }
                    } catch { }

                } else if (item.type == "absolute") {//絕對路徑
                    cc = item;
                    ac_add2(cc);
                }

            }


            //處理名稱。如果沒有設定name，就使用檔案名稱
            foreach (data_開啟程式 item in ar) {
                try {
                    String s_name = item.lnk_name;
                    if (item.name == "") {

                        int int_start = s_name.LastIndexOf("\\");//最後一個\
                        if (int_start > -1)
                            s_name = s_name.Substring(int_start + 1);

                        int int_end = s_name.LastIndexOf(".");//去掉附檔名
                        if (int_end > -1)
                            s_name = s_name.Substring(0, int_end);

                        item.name = s_name;
                    }
                } catch { }
            }


            //移除檔案不存在的路徑
            List<data_開啟程式> ar2 = new List<data_開啟程式>();
            foreach (var item in ar) {
                try {
                    if (File.Exists(item.path)) {

                        string Dialog = item.path;
                        System.Drawing.Icon[] GetIcon = IconHandler.IconHandler.IconsFromFile(Dialog, IconSize.Large);
                        if (GetIcon.Length > 0)
                            item.img = GetIcon[0].ToBitmap();

                        ar2.Add(item);
                    }
                } catch { }
            }

            return ar2;


        }




        /// <summary>
        /// true=在程式左上角顯示、false=根據滑鼠位置顯示
        /// </summary>
        public void fun_顯示原生右鍵選單(bool bb) {
            try {
                ShellContextMenu ctxMnu = new ShellContextMenu();
                FileInfo[] arrFI = new FileInfo[1];
                arrFI[0] = new FileInfo(M.ar_path[M.int_目前圖片位置]);

                if (bb) {
                    ctxMnu.ShowContextMenu(arrFI, new System.Drawing.Point(
                        (int)M.PointToScreen(new Point(0, 0)).X + 10,
                        (int)M.PointToScreen(new Point(0, 0)).Y + 10)
                    );
                } else {
                    ctxMnu.ShowContextMenu(arrFI, M.fun_取得滑鼠());
                }
            } catch {
                MessageBox.Show("error");
            }

        }



        /// <summary>
        /// 
        /// </summary>
        private void event_其他程式啟動_選單(List<data_開啟程式> ar) {

          

        

            M.but_用外部程式開啟.Click += (sender, e) => {
                u_menu_用外部程式開啟.func_open(M.but_用外部程式開啟);
            };




            u_menu_用外部程式開啟.func_add_menu("檔案右鍵選單", null, () => {
                fun_顯示原生右鍵選單(false);
            });

            u_menu_用外部程式開啟.func_add_menu("開啟檔案位置", null, () => {
                M.fun_用檔案總管開啟目前圖片();
            });

            u_menu_用外部程式開啟.func_add_menu("列印", null, () => {
                try {
                    var pr = new System.Diagnostics.Process();
                    pr.StartInfo.FileName = M.ar_path[M.int_目前圖片位置];//文件全稱-包括文件後綴
                    pr.StartInfo.CreateNoWindow = true;
                    pr.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    pr.StartInfo.Verb = "Print";
                    pr.Start();
                } catch (Exception e2) {
                    MessageBox.Show("找不到對應開啟的程式：\n" + e2.ToString(), "列印失敗");
                }
            });

            u_menu_用外部程式開啟.func_add_menu("設成桌布", null, () => {
                if (File.Exists(M.ar_path[M.int_目前圖片位置])) { //判別檔案是否存在於對應的路徑
                    try {
                        SystemParametersInfo(20, 1, M.ar_path[M.int_目前圖片位置], 0x1 | 0x2);  //存在成立，修改桌布　　(uActuin 20 參數為修改wallpaper
                    } catch (Exception e2) {
                        MessageBox.Show("設定桌布失敗：\n" + e2.ToString(), "失敗");
                    }
                }
            });

            u_menu_用外部程式開啟.func_add_menu("選擇其他程式", null, () => {
                if (File.Exists(M.ar_path[M.int_目前圖片位置])) { //判別檔案是否存在於對應的路徑
                    try {
                        String _path = M.ar_path[M.int_目前圖片位置];
                        Process.Start(new ProcessStartInfo("rundll32.exe") {
                            Arguments = $"shell32.dll,OpenAs_RunDLL {_path}",
                            WorkingDirectory = Path.GetDirectoryName(_path)
                        });
                    } catch (Exception e2) {
                        MessageBox.Show( e2.ToString(), "Error");
                    }
                }
            });



            u_menu_用外部程式開啟.func_add_水平線();

            if (C_window_AERO.IsWindows10()) {

                // 3D小畫家
                String s_Paint3D = M.fun_執行檔路徑() + "/data/imgs/icon-Paint3D.png";
                u_menu_用外部程式開啟.func_add_menu_imgPath("3D小畫家", s_Paint3D, () => {
                    if (File.Exists(M.ar_path[M.int_目前圖片位置])) { //判別檔案是否存在於對應的路徑
                        try {
                            System.Diagnostics.Process.Start("mspaint", '"' + M.ar_path[M.int_目前圖片位置] + '"' + " /ForceBootstrapPaint3D");
                        } catch (Exception e2) {
                            MessageBox.Show(e2.ToString(), "失敗");
                        }
                    }
                });

                // win10相片 APP
                String s_photos = M.fun_執行檔路徑() + "/data/imgs/icon-photos.png";
                u_menu_用外部程式開啟.func_add_menu_imgPath("相片 APP", s_photos, () => {
                    if (File.Exists(M.ar_path[M.int_目前圖片位置])) { //判別檔案是否存在於對應的路徑
                        try {
                            String url_path = Uri.EscapeDataString(M.ar_path[M.int_目前圖片位置]);
                            System.Diagnostics.Process.Start("ms-photos:viewer?fileName=" + url_path);
                        } catch (Exception e2) {
                            MessageBox.Show(e2.ToString(), "失敗");
                        }
                    }
                });
            }



            //使用者自定的名單
            for (int i = 0; i < ar.Count; i++) {

                int xx = i;
                BitmapSource img = null;
                try {
                    img = M.c_影像.BitmapToBitmapSource(ar[i].img);
                } catch { }

                u_menu_用外部程式開啟.func_add_menu(ar[i].name, img, () => {
                    try {
                        System.Diagnostics.Process.Start(ar[xx].path, "\"" + M.ar_path[M.int_目前圖片位置] + "\"");
                    } catch (Exception e2) {
                        MessageBox.Show(e2.ToString());
                    }
                });         

            }//for


        }



        /// <summary>
        /// 修改桌布用的函數
        /// </summary>
        /// <param name="uAction"></param>
        /// <param name="uParam"></param>
        /// <param name="lpvParam"></param>
        /// <param name="uWinlni"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int uWinlni);







        /// <summary>
        /// 
        /// </summary>
        /// <param name="s_資料夾"></param>
        private void fun_遍歷開始選單資料夾(String s_資料夾) {

            var ar_所有資料夾 = Directory.EnumerateFileSystemEntries(s_資料夾);

            foreach (var item in ar_所有資料夾) {
                if (File.Exists(item)) {
                    ar_file.Add(item);
                } else if (Directory.Exists(item)) {
                    fun_遍歷開始選單資料夾(item);
                }
            }

        }





        #region 從lnk捷徑取得真實路徑 （Signitures imported from http://pinvoke.net

        [DllImport("shfolder.dll", CharSet = CharSet.Auto)]
        internal static extern int SHGetFolderPath(IntPtr hwndOwner, int nFolder, IntPtr hToken, int dwFlags, StringBuilder lpszPath);

        [Flags()]
        enum SLGP_FLAGS {
            /// <summary>Retrieves the standard short (8.3 format) file name</summary>
            SLGP_SHORTPATH = 0x1,
            /// <summary>Retrieves the Universal Naming Convention (UNC) path name of the file</summary>
            SLGP_UNCPRIORITY = 0x2,
            /// <summary>Retrieves the raw path name. A raw path is something that might not exist and may include environment variables that need to be expanded</summary>
            SLGP_RAWPATH = 0x4
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        struct WIN32_FIND_DATAW {
            public uint dwFileAttributes;
            public long ftCreationTime;
            public long ftLastAccessTime;
            public long ftLastWriteTime;
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
            public uint dwReserved0;
            public uint dwReserved1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string cFileName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            public string cAlternateFileName;
        }

        [Flags()]
        enum SLR_FLAGS {
            /// <summary>
            /// Do not display a dialog box if the link cannot be resolved. When SLR_NO_UI is set,
            /// the high-order word of fFlags can be set to a time-out value that specifies the
            /// maximum amount of time to be spent resolving the link. The function returns if the
            /// link cannot be resolved within the time-out duration. If the high-order word is set
            /// to zero, the time-out duration will be set to the default value of 3,000 milliseconds
            /// (3 seconds). To specify a value, set the high word of fFlags to the desired time-out
            /// duration, in milliseconds.
            /// </summary>
            SLR_NO_UI = 0x1,
            /// <summary>Obsolete and no longer used</summary>
            SLR_ANY_MATCH = 0x2,
            /// <summary>If the link object has changed, update its path and list of identifiers.
            /// If SLR_UPDATE is set, you do not need to call IPersistFile::IsDirty to determine
            /// whether or not the link object has changed.</summary>
            SLR_UPDATE = 0x4,
            /// <summary>Do not update the link information</summary>
            SLR_NOUPDATE = 0x8,
            /// <summary>Do not execute the search heuristics</summary>
            SLR_NOSEARCH = 0x10,
            /// <summary>Do not use distributed link tracking</summary>
            SLR_NOTRACK = 0x20,
            /// <summary>Disable distributed link tracking. By default, distributed link tracking tracks
            /// removable media across multiple devices based on the volume name. It also uses the
            /// Universal Naming Convention (UNC) path to track remote file systems whose drive letter
            /// has changed. Setting SLR_NOLINKINFO disables both types of tracking.</summary>
            SLR_NOLINKINFO = 0x40,
            /// <summary>Call the Microsoft Windows Installer</summary>
            SLR_INVOKE_MSI = 0x80
        }


        /// <summary>The IShellLink interface allows Shell links to be created, modified, and resolved</summary>
        [ComImport(), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("000214F9-0000-0000-C000-000000000046")]
        interface IShellLinkW {
            /// <summary>Retrieves the path and file name of a Shell link object</summary>
            void GetPath([Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, out WIN32_FIND_DATAW pfd, SLGP_FLAGS fFlags);
            /// <summary>Retrieves the list of item identifiers for a Shell link object</summary>
            void GetIDList(out IntPtr ppidl);
            /// <summary>Sets the pointer to an item identifier list (PIDL) for a Shell link object.</summary>
            void SetIDList(IntPtr pidl);
            /// <summary>Retrieves the description string for a Shell link object</summary>
            void GetDescription([Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);
            /// <summary>Sets the description for a Shell link object. The description can be any application-defined string</summary>
            void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
            /// <summary>Retrieves the name of the working directory for a Shell link object</summary>
            void GetWorkingDirectory([Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
            /// <summary>Sets the name of the working directory for a Shell link object</summary>
            void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
            /// <summary>Retrieves the command-line arguments associated with a Shell link object</summary>
            void GetArguments([Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
            /// <summary>Sets the command-line arguments for a Shell link object</summary>
            void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
            /// <summary>Retrieves the hot key for a Shell link object</summary>
            void GetHotkey(out short pwHotkey);
            /// <summary>Sets a hot key for a Shell link object</summary>
            void SetHotkey(short wHotkey);
            /// <summary>Retrieves the show command for a Shell link object</summary>
            void GetShowCmd(out int piShowCmd);
            /// <summary>Sets the show command for a Shell link object. The show command sets the initial show state of the window.</summary>
            void SetShowCmd(int iShowCmd);
            /// <summary>Retrieves the location (path and index) of the icon for a Shell link object</summary>
            void GetIconLocation([Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath,
                int cchIconPath, out int piIcon);
            /// <summary>Sets the location (path and index) of the icon for a Shell link object</summary>
            void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
            /// <summary>Sets the relative path to the Shell link object</summary>
            void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);
            /// <summary>Attempts to find the target of a Shell link, even if it has been moved or renamed</summary>
            void Resolve(IntPtr hwnd, SLR_FLAGS fFlags);
            /// <summary>Sets the path and file name of a Shell link object</summary>
            void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);

        }

        [ComImport, Guid("0000010c-0000-0000-c000-000000000046"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IPersist {
            [PreserveSig]
            void GetClassID(out Guid pClassID);
        }


        [ComImport, Guid("0000010b-0000-0000-C000-000000000046"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IPersistFile : IPersist {
            new void GetClassID(out Guid pClassID);
            [PreserveSig]
            int IsDirty();

            [PreserveSig]
            void Load([In, MarshalAs(UnmanagedType.LPWStr)]
    string pszFileName, uint dwMode);

            [PreserveSig]
            void Save([In, MarshalAs(UnmanagedType.LPWStr)] string pszFileName,
                [In, MarshalAs(UnmanagedType.Bool)] bool fRemember);

            [PreserveSig]
            void SaveCompleted([In, MarshalAs(UnmanagedType.LPWStr)] string pszFileName);

            [PreserveSig]
            void GetCurFile([In, MarshalAs(UnmanagedType.LPWStr)] string ppszFileName);
        }

        const uint STGM_READ = 0;
        const int MAX_PATH = 260;

        // CLSID_ShellLink from ShlGuid.h 
        [
            ComImport(),
            Guid("00021401-0000-0000-C000-000000000046")
        ]
        public class ShellLink {
        }

        #endregion



        /// <summary>
        /// 從lnk捷徑抓取真實檔案位置
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string ResolveShortcut(string filename) {
            ShellLink link = new ShellLink();
            ((IPersistFile)link).Load(filename, STGM_READ);
            // TODO: if I can get hold of the hwnd call resolve first. This handles moved and renamed files.  
            // ((IShellLinkW)link).Resolve(hwnd, 0) 
            StringBuilder sb = new StringBuilder(MAX_PATH);
            WIN32_FIND_DATAW data = new WIN32_FIND_DATAW();
            ((IShellLinkW)link).GetPath(sb, sb.Capacity, out data, 0);
            return fun_判斷32或64(sb.ToString());
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static String fun_判斷32或64(String path) {
            String s_64 = null;
            String s_32 = null;

            if (path.Length > 25 && path.Substring(0, 23) == fun_作業系統的槽() + @":\Program Files (x86)\") {
                String path_end = path.Substring(23);
                s_64 = fun_作業系統的槽() + @":\Program Files\" + path_end;
                s_32 = fun_作業系統的槽() + @":\Program Files (x86)\" + path_end;
            } else if (path.Length > 20 && path.Substring(0, 17) == fun_作業系統的槽() + @":\Program Files\") {
                String path_end = path.Substring(17);
                s_64 = fun_作業系統的槽() + @":\Program Files\" + path_end;
                s_32 = fun_作業系統的槽() + @":\Program Files (x86)\" + path_end;
            }

            if (File.Exists(s_64)) {
                return s_64;
            } else if (File.Exists(s_32)) {
                return s_32;
            } else if (File.Exists(path)) {
                return path;
            } else {
                return null;
            }

        }



    }


}
