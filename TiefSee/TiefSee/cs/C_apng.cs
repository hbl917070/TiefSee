using APNGLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using static APNGLib.Frame;



namespace TiefSee.cs {


    public class C_apng {

        //public IList<Bitmap> Images { get; private set; }
        //public IList<System.Windows.Media.Imaging.BitmapSource> Images { get; private set; }


        private APNG APNGFile;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool fun_載入並判斷是否APNG(String path) {

            try {
                //載入
                APNGFile = new APNGLib.APNG();
                using (Stream s = File.OpenRead(path)) {
                    APNGFile.Load(s);
                }
                return APNGFile.IsAnimated;//判斷是否為APNG動畫
            } catch { }

            return false;
        }



        /// <summary>
        /// 
        /// </summary>
        public List<data_apng> fun_取得解析後的APNG() {

            List<data_apng> ar_apng = new List<data_apng>();

            if (APNGFile.IsAnimated) {

                Bitmap current = new Bitmap((int)APNGFile.Width, (int)APNGFile.Height);
                Bitmap previous = null;

                ImageRender.RenderNextFrame(current, System.Drawing.Point.Empty, APNGFile.ToBitmap(0), Frame.BlendOperation.SOURCE);
                fun_add(new Bitmap(current), ar_apng, APNGFile.GetFrame(0).Milliseconds);

                for (int i = 1; i < APNGFile.FrameCount; i++) {
                    APNGLib.Frame oldFrame = APNGFile.GetFrame(i - 1);
                    Bitmap prev = previous == null ? null : new Bitmap(previous);
                    if (oldFrame.DisposeOp != APNGLib.Frame.DisposeOperation.PREVIOUS) {
                        previous = (current);
                    }
                    ImageRender.DisposeBuffer(current, new Rectangle((int)oldFrame.XOffset, (int)oldFrame.YOffset, (int)oldFrame.Width, (int)oldFrame.Height), oldFrame.DisposeOp, prev);
                    APNGLib.Frame currFrame = APNGFile.GetFrame(i);
                    ImageRender.RenderNextFrame(current, new System.Drawing.Point((int)currFrame.XOffset, (int)currFrame.YOffset), APNGFile.ToBitmap(i), currFrame.BlendOp);
                    fun_add(new Bitmap(current), ar_apng, APNGFile.GetFrame(i).Milliseconds);

                   
                    if (prev != null) {
                        prev.Dispose();
                        prev = null;
                    }
                }

                if (current != null) {
                    current.Dispose();
                    current = null;
                }
                if (previous != null) {
                    previous.Dispose();
                    previous = null;
                }


            } else {//如果不是動畫

                fun_add(APNGFile.ToBitmap(), ar_apng, 60);

            }

            return ar_apng;

        }




        /*
        private void BitmapToBitmapImage(System.Drawing.Bitmap bitmap) {

            BitmapImage bitmapImage = new BitmapImage();
            try {
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream()) {
                    bitmap.Save(ms, bitmap.RawFormat);
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = ms;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                }

                Images.Add(bitmapImage);
            } catch (Exception) {
                //xxx++;
                //MessageBox.Show(xxx+"");
            }


            //return bitmapImage;
        }*/








        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);


        /// <summary>
        /// 轉成WPF圖片，並新增
        /// </summary>
        /// <param name="bitmap"></param>
        private void fun_add(System.Drawing.Bitmap bitmap, List<data_apng> ar_apng, int int_延遲) {
            IntPtr ptr = bitmap.GetHbitmap();
            BitmapSource result =
                System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    ptr, IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            //release resource
            DeleteObject(ptr);


            ar_apng.Add(new data_apng
            {
                int_延遲 = int_延遲,
                img = result
            });


            bitmap.Dispose();
            bitmap = null;

            //return result;
        }









    }


    public class data_apng {

        public int int_延遲 = 0;
        public String name = "";
        public BitmapSource img;
        public uint width = 1;
        public uint height = 1;
        public uint top = 0;
        public uint left = 0;
        //public DisposeOps ChunkType;
    }


}
