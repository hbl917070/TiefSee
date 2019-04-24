using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiefSee {


    public partial class MainWindow {

        public String _s_pixiv工作路徑 = "";

        public static bool _bool_快速啟動 = false;
        public static bool _bool_快速預覽_空白鍵 = true;
        public static bool _bool_快速預覽_滑鼠滾輪 = true;

        public E_GIF_type e_GIF_Type = E_GIF_type.WPF;


        public e_目前工作模式 _e_目前工作模式 = e_目前工作模式.一般圖片;
        public e_視窗類型 _e_視窗類型 = e_視窗類型.一般視窗;
        public e_大型換頁按鈕 _e_大型換頁按鈕 = e_大型換頁按鈕.下方;
        public e_滾輪用途 _e_滾輪用途 = e_滾輪用途.換頁;

        public e_圖片縮放模式 _e_圖片縮放模式 = e_圖片縮放模式.magick_低像素圖;
        public e_預設瀏覽器 _e_預設瀏覽器 = e_預設瀏覽器.Chrome;
        public e_鎖定size _e_鎖定size = e_鎖定size.寬度;
        public e_鎖定size_單位 _e_鎖定size_單位 = e_鎖定size_單位.cm;





        /// <summary>
        /// GIF的渲染模式
        /// </summary>
        public enum E_GIF_type {
            GDI = 1, // webbrowser
            WPF = 2
        }


        public enum e_目前工作模式 {
            一般圖片 = 1,
            動圖 = 2,
            GIF = 3,
            影片 = 4,
            大量瀏覽模式 = 10,
            web_圖片 = 20,
        }


        public enum e_視窗類型 {
            快速啟動 = 0,
            一般視窗 = 1,
            透明視窗 = 10,
        }

        public enum e_大型換頁按鈕 {
            無 = 0,
            下方 = 1,
            兩側 = 2,
        }

        public enum e_滾輪用途 {
            縮放圖片 = 1,

            上下移動 = 11,
            上下移動_到底時換頁 = 12,

            換頁 = 21,
            換頁_大於視窗時上下移動 = 22,
            換頁_大於視窗時上下移動_到底部時換頁 = 23,
        }


      


        public enum e_圖片縮放模式 {
            原始WPF = 11,
            原始WPF_高品質 = 12,
            magick_高品質 = 21,
            magick_低像素圖 = 22,//圖片比例超過100%時，用
        }


        public enum e_預設瀏覽器 {
            電腦預設 = 0,
            IE = 1,
            Chrome = 2,
            Firefox = 3,
            Vivaldi = 4,
            Opera = 5,
        }



        public enum e_鎖定size {
            無 = 0,
            高度 = 1,
            寬度 = 2,

        }
        public enum e_鎖定size_單位 {
            px = 0,
            比例 = 1,
            cm = 2,

        }



    }


}
