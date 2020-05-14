

function imgView(option) {

    var box = option.box;

    console.log(box);

    /**
     * 公開 API
     */
    return {
        box: box
    }
load

   onChangeZoom //圖片縮放
    onMove

    放大
    縮小


getZoom //圖片縮放比例
getX
getY
getTop
getLeft
getWidth
getHeight

setSize
圖片原始大小


get水平
get垂直
get旋轉
int_圖片邊距
get移動速度
get拖曳速度



}

return;


function imgView() {


    /**
     * 
     * @param {*} id 
     */
    function getId(id) {
        return document.getElementById(id);
    }

    /**
     * 轉數字。"12.3px" => 12.3
     * @param {*} nn 
     */
    function fun_nub(nn) {
        return Number(nn.replace('px', ''));
    }


    var int_旋轉 = 0;

    /**
     * 
     * @param {*} int_旋轉_前 
     * @param {*} int_旋轉_後 
     * @param {*} bool_水平_前 
     * @param {*} bool_水平_後 
     * @param {*} bool_垂直_前 
     * @param {*} bool_垂直_後 
     */
    function fun_套用旋轉(int_旋轉_前, int_旋轉_後, bool_水平_前, bool_水平_後, bool_垂直_前, bool_垂直_後) {

        //
        getId("css_rotate").innerHTML =
            "#img_img{" +
            "transform: rotate(" + int_旋轉_前 + "deg)  scaleX(" + bool_水平_前 + ")  scaleY(" + bool_垂直_前 + ");" +
            "}";

        int_旋轉 = int_旋轉_後;

        //延遲執行，避免動畫失效
        setTimeout(function () {

            getId("css_rotate").innerHTML =
                "#img_img{" +
                "transition: transform 0.3s ,left 0.3s ,top 0.3s;" +
                "transform: rotate(" + int_旋轉_後 + "deg)  scaleX(" + bool_水平_後 + ")  scaleY(" + bool_垂直_後 + ");" +
                "}";

            //修正圖片位置，必須等到旋轉完成才執行
            setTimeout(function () {
                fun_beyond();
                setTimeout(function () {
                    getId("css_rotate").innerHTML =
                        "#img_img{" +
                        "transform: rotate(" + int_旋轉_後 + "deg)  scaleX(" + bool_水平_後 + ")  scaleY(" + bool_垂直_後 + ");" +
                        "}";
                }, 1);
            }, 301);

        }, 1);

    }



    /**
     * 取得圖片長寬
     * @param {*} img 
     */
    function fun_getImgSize(img) {
        var nWidth;
        var nHeight;

        if (img.naturalWidth) { // 現代瀏覽器
            nWidth = img.naturalWidth;
            nHeight = img.naturalHeight;
        } else { // IE6/7/8
            var image = new Image();
            image.src = img.src;
            nWidth = image.width;
            nHeight = image.height;
            image = null;
        }

        //避免svg出錯
        if (nWidth === 0 && nHeight === 0) {
            //nWidth = img.clientWidth;
            //nHeight = img.clientHeight;
        }

        return new Array(nWidth, nHeight);
    }




    //--------------拖曳------------------------------


    var xxx;
    var yyy;
    var double_keen = 1.5;//拖曳靈敏度
    var int_縮放起始值 = 99;
    var int_圖片邊距 = 40;


    var obj_img;//圖片物件
    var obj_img_content;//圖片物件的容器


    //拖曳時用的
    var m_x;
    var m_y;
    var m_top;
    var m_left;


    ///
    ///
    ///
    function main() {

        //fun_open_imgbox();

        //初始化物件
        obj_img_content = getId("img_content");
        obj_img = getId("img_img");


        ///
        ///按下時，註冊事件
        ///
        obj_img_content.addEventListener("mousedown", function (e) {

            m_x = e.screenX;
            m_y = e.screenY;

            /*var int_捲軸寬度x = obj_img_content.offsetWidth - obj_img_content.clientWidth;
            var int_捲軸寬度y = obj_img_content.offsetHeight - obj_img_content.clientHeight;
            if (e.pageX > obj_img_content.offsetWidth - int_捲軸寬度x) {
            return;
            }
            if (e.pageY > obj_img_content.offsetHeight - int_捲軸寬度y) {
            return;
            }*/

            e.preventDefault();//取消點擊事件

            document.onmousemove = mousemove;

            m_top = fun_nub(obj_img.style.top);
            m_left = fun_nub(obj_img.style.left);

        }, false);


        /**
         * 滑鼠移動時
         * @param {*} e 
         */
        function mousemove(e) {

            var int_top = m_top - (m_y - e.screenY) * double_keen;
            var int_left = m_left - (m_x - e.screenX) * double_keen;

            obj_img.style.top = int_top + "px";
            obj_img.style.left = int_left + "px";

            fun_beyond(e);//避免超出範圍
        }

        /**
         * 放開時，取消事件
         */
        document.onmouseup = function () {//
            document.onmousemove = null;
            net.net_主視窗取得焦點();//讓視窗取得焦點才能使用快速鍵
            down_type = "";//避免下面的換頁按鈕被誤觸
        };

    }



    /**
     * 處理圖片超過邊界的函數
     * @param {*} e 
     */
    function fun_beyond(e) {

        var o_l = (obj_img.getBoundingClientRect().left);
        var o_t = (obj_img.getBoundingClientRect().top);

        var img_offsetW = obj_img.getBoundingClientRect().width;
        var img_offsetH = obj_img.getBoundingClientRect().height;

        if (int_旋轉 % 180 == 0) {

            if (img_offsetW > obj_img_content.offsetWidth && o_l > int_圖片邊距) {//寬度大於容器時，限制往右移
                obj_img.style.left = int_圖片邊距 + "px";
                if (e != undefined) {//拖曳超過邊界時，讓滑鼠能夠立即返回
                    m_x = e.screenX;
                    m_left = int_圖片邊距;
                }
            }
            if (img_offsetW <= obj_img_content.offsetWidth) {//寬度小於容器時，圖片置中
                obj_img.style.left = (obj_img_content.offsetWidth - img_offsetW) / 2 + "px";
            }

            if (img_offsetW > obj_img_content.offsetWidth && o_l < obj_img_content.offsetWidth - img_offsetW - int_圖片邊距) {//寬度大於容器時，限制往左移
                obj_img.style.left = obj_img_content.offsetWidth - img_offsetW - int_圖片邊距 + "px";
                if (e != undefined) {
                    m_x = e.screenX;
                    m_left = fun_nub(obj_img.style.left);
                }
            }

            if (img_offsetH > obj_img_content.offsetHeight && o_t > int_圖片邊距) {
                obj_img.style.top = int_圖片邊距 + "px";
                if (e != undefined) {
                    m_y = e.screenY;
                    m_top = int_圖片邊距;
                }
            }
            if (img_offsetH <= obj_img_content.offsetHeight) {
                obj_img.style.top = (obj_img_content.offsetHeight - img_offsetH) / 2 + "px";
            }

            if (img_offsetH > obj_img_content.offsetHeight && o_t < obj_img_content.offsetHeight - img_offsetH - int_圖片邊距) {
                obj_img.style.top = obj_img_content.offsetHeight - img_offsetH - int_圖片邊距 + "px";
                if (e != undefined) {
                    m_y = e.screenY;
                    m_top = fun_nub(obj_img.style.top);
                }
            }


        } else {


            var mL = (obj_img.offsetHeight - obj_img.offsetWidth) / 2;


            if (img_offsetW > obj_img_content.offsetWidth && o_l > int_圖片邊距) {//寬度大於容器時，限制往右移
                obj_img.style.left = (int_圖片邊距 + mL) + "px";
                if (e != undefined) {//拖曳超過邊界時，讓滑鼠能夠立即返回
                    m_x = e.screenX;
                    m_left = int_圖片邊距 + mL;
                }
            }
            if (img_offsetW <= obj_img_content.offsetWidth) {//寬度小於容器時，圖片置中
                obj_img.style.left = (obj_img_content.offsetWidth - img_offsetW) / 2 + mL + "px";
            }
            if (img_offsetW > obj_img_content.offsetWidth && o_l < obj_img_content.offsetWidth - obj_img.offsetHeight - int_圖片邊距) {//寬度大於容器時，限制往左移
                obj_img.style.left = obj_img_content.offsetWidth - img_offsetW - int_圖片邊距 + mL + "px";
                if (e != undefined) {
                    m_x = e.screenX;
                    m_left = fun_nub(obj_img.style.left);
                }
            }


            if (img_offsetH > obj_img_content.offsetHeight && o_t > int_圖片邊距) {
                obj_img.style.top = (int_圖片邊距 - mL) + "px";
                if (e != undefined) {
                    m_y = e.screenY;
                    m_top = int_圖片邊距 - mL;
                }
            }
            if (img_offsetH <= obj_img_content.offsetHeight) {
                obj_img.style.top = (obj_img_content.offsetHeight - img_offsetH) / 2 - mL + "px";
            }
            if (img_offsetH > obj_img_content.offsetHeight && o_t < obj_img_content.offsetHeight - obj_img.offsetWidth - int_圖片邊距) {
                obj_img.style.top = obj_img_content.offsetHeight - img_offsetH - int_圖片邊距 - mL + "px";
                if (e != undefined) {
                    m_y = e.screenY;
                    m_top = fun_nub(obj_img.style.top);
                }
            }

        }

    }



    //----------放大--------------------------------------




    var int_size = 100;
    var img_size;


    // 註冊滾動事件
    if ('onmousewheel' in window) {
        window.onmousewheel = MouseWheel;
    } else if ('onmousewheel' in document) {
        document.onmousewheel = MouseWheel;
    } else if ('addEventListener' in window) {
        window.addEventListener("mousewheel", MouseWheel, true);
        window.addEventListener("DOMMouseScroll", MouseWheel, true);
    }

    /**
     * 滑鼠捲動時
     * @param {*} e 
     */
    function MouseWheel(e) {

        e.preventDefault();//禁止頁面滾動
        /*e = e || window.event;

        //縮放計算
        if (e.wheelDelta <= 0 || e.detail > 0) {
            fun_imgSizeSubtrat(e);
        } else {
            fun_imgSizeAdd(e);
        }

        fun_beyond();//計算圖片位置*/
    }



    /**
     * 縮小圖片(C#調用)
     * @param {*} e 
     */
    function fun_imgSizeSubtrat(e) {

        int_size *= 0.9090909090909;
        if (int_size <= 5) {
            int_size /= 0.9090909090909;
            return;
        }

        //如果不是透過滾輪來縮放，就從中央作為縮放起點
        if (e === undefined) {
            e = { clientX: 0, clientY: 0 };
            e.clientX = obj_img_content.offsetWidth / 2;
            e.clientY = obj_img_content.offsetHeight / 2;
        }

        //計算游標目前在圖片的坐標
        xxx = e.clientX - (getId("img_box")).offsetLeft - 0 - fun_nub(obj_img.style.left);
        yyy = e.clientY - (getId("img_box")).offsetTop - 0 - fun_nub(obj_img.style.top);

        //計算圖片改變大小後的差距
        var xx2 = obj_img.offsetWidth - obj_img.offsetWidth * 0.9090909090909;
        var yy2 = obj_img.offsetHeight - obj_img.offsetHeight * 0.9090909090909;

        //儲存目前的捲軸位置
        var top2 = fun_nub(obj_img.style.top);
        var left2 = fun_nub(obj_img.style.left);

        fun_imgSizeChange();//改變大小

        obj_img.style.top = top2 + ((yyy / obj_img.offsetHeight) * yy2) * 0.9090909090909 + "px";
        obj_img.style.left = left2 + ((xxx / obj_img.offsetWidth) * xx2) * 0.9090909090909 + "px";


        fun_beyond();//計算圖片位置

    }


    /**
     * 放大圖片(C#調用)
     * @param {*} e 
     */
    function fun_imgSizeAdd(e) {

        int_size *= 1.1;
        if (int_size >= 6000) {
            int_size /= 1.1;
            return;
        }

        fun_imgSizeChange();

        //如果不是透過滾輪來縮放，就從中央作為縮放起點
        if (e == undefined) {
            e = { clientX: 0, clientY: 0 };
            e.clientX = obj_img_content.offsetWidth / 2;
            e.clientY = obj_img_content.offsetHeight / 2;
        }

        xxx = e.clientX - (document.getElementById("img_box")).offsetLeft - 0 - fun_nub(obj_img.style.left);
        yyy = e.clientY - (document.getElementById("img_box")).offsetTop - 0 - fun_nub(obj_img.style.top);

        var xx2 = obj_img.offsetWidth - obj_img.offsetWidth / 1.1;
        var yy2 = obj_img.offsetHeight - obj_img.offsetHeight / 1.1;

        obj_img.style.top = (fun_nub(obj_img.style.top) - ((yyy / obj_img.offsetHeight) * yy2) * 1.1) + "px";
        obj_img.style.left = (fun_nub(obj_img.style.left) - ((xxx / obj_img.offsetWidth) * xx2) * 1.1) + "px";


        fun_beyond();//計算圖片位置
    }







    //--------------------圖片檢視視窗----------------------------




    var svg_size = null;//svg用的圖片size



    /**
     * 載入圖片(C#調用)
     * @param img_url
     * @param img_w
     * @param img_h
     */
    function fun_open_imgbox(img_url, img_w, img_h) {


        //載入圖片前，初始化旋轉
        if (img_url != "") {
            document.getElementById("css_rotate").innerHTML = "#img_img{" +
                "transform: rotate(0deg)  scaleX(1)  scaleY(1);" +
                "}";
            int_旋轉 = 0;
        }



        //obj_img.style.opacity = 0;//載入完成前，先隱藏圖片

        //如果傳入的是svg類型，就強制帶入size（因為js無法計算svg的size，所以從C#計算）
        if (img_w != 0) {
            img_size = new Array(img_w, img_h);

            if (obj_img_content.offsetWidth > img_w && obj_img_content.offsetHeight > img_h) {
                fun_檢視原始大小();
            } else {
                fun_100scale();
            }


        }

        document.getElementById("img_img").src = img_url;//載入圖片，之後的事情會在圖片的onload處理




        //img_size = new Array(img_w, img_h);//取得圖片寬高

        /*
        var src = "https://truth.bahamut.com.tw/s01/201702/8dab59654f4cf7ea8b8af4c78656a5f4.PNG";
        document.getElementById("img_img").src = src;
        */
    }


    var bool_width;//判斷縮放方式
    var st_zoomMode = "full";


    ///
    ///選擇縮放模式
    ///
    function fun_zoomMode(x) {
        st_zoomMode = x;
        //document.getElementById("bu_full").className = "";
        //document.getElementById("bu_full_h").className = "";
        //document.getElementById("bu_full_v").className = "";

        if (x == 'full') {
            document.getElementById("bu_full").className = "bu_sel";
        } else if (x == 'v') {
            document.getElementById("bu_full_v").className = "bu_sel";
        } else if (x == 'h') {
            document.getElementById("bu_full_h").className = "bu_sel";
        }

        fun_100scale();
    }



    ///
    ///圖片 初始 & 最大 化
    ///
    function fun_100scale() {
        if (img_size == null || img_size == undefined)
            return;
        int_size = 100;

        try {
            int_size = int_縮放起始值;
            if (int_size < 10) {
                int_size = 10;
            } else if (int_size > 500) {
                int_size = 500;
            } else if (isNaN(int_size)) {
                int_size = 100;
            }
            int_縮放起始值 = int_size + "";
        } catch (e) {
            int_size = 100;
            int_縮放起始值 = "100";
        }


        if (st_zoomMode == "full") {//圖片滿版，需要判斷


            if (int_旋轉 % 180 == 0) {

                if (img_size[0] && img_size[1]) {

                    if ((img_size[0] / obj_img_content.offsetWidth) > (img_size[1] / obj_img_content.offsetHeight)) {
                        bool_width = true;
                    } else {
                        bool_width = false;
                    }
                } else {
                    bool_width = false;
                }

            } else {

                if ((img_size[1] / obj_img_content.offsetWidth) > (img_size[0] / obj_img_content.offsetHeight)) {
                    bool_width = true;

                    int_size = (img_size[0] / obj_img_content.offsetWidth) / (img_size[1] / obj_img_content.offsetWidth) * 100;
                    // alert(img_size[1] + " " + obj_img_content.offsetWidth + "\n" + int_size);
                } else {

                    int_size = (img_size[1] / obj_img_content.offsetHeight) / (img_size[0] / obj_img_content.offsetHeight) * 100;

                    bool_width = false;
                }


            }









        } else if (st_zoomMode == "v") {
            bool_width = false;
        } else if (st_zoomMode == "h") {
            bool_width = true;
        }


        var ar_wh = fun_imgSizeChange();//修改圖片size並回傳

        //調整圖片位置
        obj_img.style.left = (obj_img_content.offsetWidth - ar_wh[0]) / 2 + "px";
        obj_img.style.top = (obj_img_content.offsetHeight - ar_wh[1]) / 2 + "px";


        /*
        setTimeout(function () {
        obj_img.style.left = (obj_img_content.offsetWidth - obj_img.offsetWidth) / 2 + "px";
        obj_img.style.top = (obj_img_content.offsetHeight - obj_img.offsetHeight) / 2 + "px";
        }, 10);*/

    }







    /**
     * 縮放
     */
    function fun_imgSizeChange() {

        var int_ww = 50;
        var int_hh = 50;

        if (bool_width) {
            //document.getElementById("img_img").style.width = int_size + "%";
            obj_img.style.width = (obj_img_content.offsetWidth * int_size / 100) + "px";
            obj_img.style.height = "auto";

            int_ww = (obj_img_content.offsetWidth * int_size / 100);
            int_hh = img_size[1] / img_size[0] * int_ww;

        } else {
            //document.getElementById("img_img").style.height = int_size + "%";
            obj_img.style.height = (obj_img_content.offsetHeight * int_size / 100) + "px";
            obj_img.style.width = "auto";

            int_hh = (obj_img_content.offsetHeight * int_size / 100);
            int_ww = img_size[0] / img_size[1] * int_hh;

        }

        document.onmousemove = null;//取消拖曳事件

        //呼叫C#，修改目前圖片的比例
        var s_顯示比例 = Math.ceil(obj_img.offsetWidth / img_size[0] * 100) + "%"
        net.net_修改顯示圖片比例(s_顯示比例);


        return new Array(int_ww, int_hh);

    }




    /**
     * 由C#呼叫，檢視圖片原始的大小
     */
    function fun_檢視原始大小() {

        bool_width = true;
        int_size = img_size[0] / obj_img_content.offsetWidth * 100;
        fun_imgSizeChange();
        fun_beyond();//計算圖片位置
    }





    /**
     * 回傳目前圖片是否大於視窗（用於判斷是否可以拖曳視窗）
     */
    function fun_bool_movie() {

        //如果在上下一頁的按鈕上，則禁止拖曳
        if (bool_div_bottom_onmouseenter) {
            return "false";
        }
***
        var obj_img = document.getElementById("img_img");
        var obj_img_w = document.getElementById("img_w");

        if (obj_img.getBoundingClientRect().height >= obj_img_w.offsetHeight || obj_img.getBoundingClientRect().width >= obj_img_w.offsetWidth) {
            return "false";
        } else {
            return "true";
        }
    }

    /**
     * 用C#控制圖片捲軸（上
     */
    function fun_scrollTop() {
        obj_img.style.top = fun_nub(obj_img.style.top) + 50 + "px";
        fun_beyond();//避免超出位置
    }

    /**
     * 用C#控制圖片捲軸（下
     */
    function fun_scrollBottom() {
        obj_img.style.top = fun_nub(obj_img.style.top) - 50 + "px";
        fun_beyond();//避免超出位置
    }



    ///
    ///按下按鍵
    ///
    /*document.onkeydown = function (e) {
    alert();
    var currKey = 0;
    e = e || event;
    currKey = e.keyCode || e.which || e.charCode;
    e.preventDefault();
    };*/

    //按下按鍵
    /*document.onkeydown = function (e) {
    
        var currKey = 0;
        e = e || event;
        currKey = e.keyCode || e.which || e.charCode;
    
        if (currKey == 38 || currKey == 104) {//上 8
            document.getElementById("img_content").scrollTop -= 50;
            e.preventDefault();
        } else if (currKey == 40 || currKey == 101) {//下 5
            document.getElementById("img_content").scrollTop += 50;
            e.preventDefault();
        } else if (currKey == 100 || currKey == 37) {//左 4
    
            fun_next();
    
            e.preventDefault();
        } else if (currKey == 102 || currKey == 39) {//右 6
    
    
            fun_previous();
    
            e.preventDefault();
        } else if (currKey == 107 || currKey == 16) {// 放大 + shift
            fun_imgSizeAdd();
            e.preventDefault();
        } else if (currKey == 109 || currKey == 17) {//縮小 - ctrl
            fun_imgSizeSubtrat();
            e.preventDefault();
        }
    
    
    };*/







}