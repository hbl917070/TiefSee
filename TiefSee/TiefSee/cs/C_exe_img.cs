using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsudaKageyu;

namespace TiefSee.cs {
    public class C_exe_img {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static List<Bitmap> fun_exe_img(String fileName) {




            List<Bitmap> ar_Bitmap_2 = new List<Bitmap>();



            Icon icon = null;
            Icon[] splitIcons = null;
            IconExtractor extractor = null;

            try {
                extractor = new IconExtractor(fileName);
            } catch {
                return null;
            }

            //同一個執行檔，會有很多套不同的icon
            for (int k = 0; k < extractor.Count; k++) {

                List<Bitmap> ar_Bitmap = new List<Bitmap>();//該套icon裡面的所有size

                try {
                    icon = extractor.GetIcon(k);
                    splitIcons = IconUtil.Split(icon);
                } catch {
                    continue;
                }


                // Update icons.
                foreach (var i in splitIcons) {
                    try {
                        //var size = i.Size;
                        //var bits = IconUtil.GetBitCount(i);
                        ar_Bitmap.Add(IconUtil.ToBitmap(i));
                        i.Dispose();
                    } catch {
                        continue;
                    }
                }

                //排序(由大到小
                for (int i = 0; i < ar_Bitmap.Count; i++)
                    for (int j = i; j < ar_Bitmap.Count; j++)
                        if (ar_Bitmap[i].Width < ar_Bitmap[j].Width) {
                            Bitmap bb = ar_Bitmap[i];
                            ar_Bitmap[i] = ar_Bitmap[j];
                            ar_Bitmap[j] = bb;
                        }

                //放入要回傳的陣列
                foreach (var item in ar_Bitmap) {
                    if (item != null)
                        if (item.Width > 5)
                            ar_Bitmap_2.Add(item);
                }

            }//for



            return ar_Bitmap_2;
        }


    }
}
