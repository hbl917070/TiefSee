//
//作者：hbl917070（深海異音）
//小屋：https://home.gamer.com.tw/homeindex.php?owner=hbl917070
//
//說明：讀取google搜圖後的縮圖來進行【saucenao搜圖】
//
//最後修改：2017/07/24
//

ret_img();

function ret_img() {

    var obj_u6 = document.querySelector("#topstuff img");

    if (obj_u6 == undefined) {//如果物件不存在，就100毫秒後再重新執行
        setTimeout(function () {
            ret_img();
        }, 100);
        return;
    }

    window.external.fun_open_saucenao(obj_u6.src);//調用C#開啟【saucenao】的函數

}


