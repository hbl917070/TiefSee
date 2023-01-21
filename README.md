> **Warning**  
> 此為 TiefSee 3 的專案，不再進行更新與維護
> <br><br>
> 新版的 Tiefsee4 移至<br>
> https://github.com/hbl917070/Tiefsee4

---

## 簡介

- 開源綠色免安裝，真正專注於看圖的軟體
- 簡約高雅的界面，擁有全界面的透明毛玻璃
- 不因功能強大就在界面上擺一堆使用者看不懂的按鈕
- 使用大量瀏覽模式，讓看漫畫輕鬆無比
- 將視窗固定在最上層，協助完成複雜工作
- 在資料夾或桌面長按空白鍵，快速預覽圖片
- 支援多種特殊格式：apng、svg、psd、raw、web、pixiv動圖、exe 等...

###### 官方網站：https://hbl917070.github.io/aeropic/index.html#/
###### 討論區：https://forum.gamer.com.tw/C.php?bsn=60076&snA=4095280

<br><br><br>

## TiefSee
- 用途：Windows的圖片檢視器
- 專案類型：C# 的 WPF ( WPF 、 WindowForms 、 HTML 混合使用)
- 補充：必須使用 visual studio 2019 才能開啟專案

<img src="https://hbl917070.github.io/aeropic/imgs/explain_soft/ui-1.svg" width="500px" alt="TiefSee 工具列介紹"/>
<img src="https://hbl917070.github.io/aeropic/imgs/home_demo/1.gif" width="400px" alt="TiefSee實際運作" />

<br><br>

##  TiefSee、合成GIF
- 用途：將「pixiv動圖」轉存成「gif」
- 使用方式：在 ```input.xml``` 裡面寫入輸入與輸出的相關設定值後，直接開啟程式即可

<img src="https://hbl917070.github.io/aeropic/imgs/other/3.jpg" width="400px" alt="將「pixiv動圖」轉存成「gif」"/>

<br><br><br>

## TiefSee、快速啟動
- 用途：TiefSee的啟動器
- 運作原理：向TiefSee 發出 http請求，TiefSee 會以「新建一個視窗開啟圖片」，取代原本的「開一個全新的執行檔」
- 使用方式：在命令列傳入圖片的路徑當做參數，或是直接啟動

<br><br><br>

##  TiefSee、關聯附檔名
- 用途：讓TiefSee變成作業系統預設開啟圖片的程式
- 專案類型：C# 的 WPF
- 運作原理：修改 登入檔 (Registry)
- 使用方式：直接開啟程式即可使用


<img src="https://hbl917070.github.io/aeropic/imgs/explain_edit_default/10.jpg" width="400px" alt="TiefSee 關聯附檔名" />

<br><br><br>

##  TiefSee、搜圖
- 用途：TiefSee 的搜圖
- 專案類型：C# 的 WindowForms
- 運作原理：使用webbrowser向目標網站注入javascript
- 使用方式：在```input.txt```寫入搜圖的類型與圖片的base64，開啟程式後將會進行搜圖，並透過瀏覽器開啟搜圖結果


<img src="https://hbl917070.github.io/aeropic/imgs/home_demo/5.gif" width="400px" alt="TiefSee 搜圖" />

<br><br><br>

## 上述專案使用了下列的程式碼
- [jQuery](https://github.com/jquery/jquery "jQuery")：JavaScript函式庫
- [WpfAnimatedGif](https://github.com/XamlAnimatedGif/WpfAnimatedGif "WpfAnimatedGif")：WPF顯示GIF的套件
- [WPF Color Picker](http://https://www.codeproject.com/Articles/229442/WPF-Color-Picker-VS2010-Style "WPF Color Picker")：WPF的顏色選擇器套件
- [FluentWPF](https://github.com/sourcechord/FluentWPF "FluentWPF")：運行於WPF的Fluent Design
- [APNG.NET](https://github.com/xupefei/APNG.NET "APNG.NET")：C#解析並顯示APNG的套件
- [Explorer Shell Context Menu]( https://www.codeproject.com/Articles/22012/Explorer-Shell-Context-Menu "Explorer Shell Context Menu")：顯示檔案原生右鍵選單
- [ExifLib](https://www.codeproject.com/Articles/36342/ExifLib-A-Fast-Exif-Data-Extractor-for-NET-2-0 "ExifLib")：讀取exif的套件
- [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json "Newtonsoft.Json")：讀取json的套件
- [Magick.NET](https://github.com/dlemstra/Magick.NET "Magick.NET")：ImageMagick 的 net 封裝
- [dcraw](http://www.cybercom.net/~dcoffin/dcraw/ "dcraw")：RAW圖片的解析引擎
- [WinQuickLook](https://github.com/shibayan/WinQuickLook "WinQuickLook")：在檔案總管長按鍵盤空白鍵後，預覽該檔案
- [QuickLook](https://github.com/QL-Win/QuickLook "QuickLook")：將macOS“快速查看”功能帶到Windows
