
fun_input();

function fun_input() {


	var qbui = document.getElementById('qbui');

	if (qbui == undefined) {//如果物件不存在，就100毫秒後重新執行
		
		var qbi = document.getElementById('qbi');
		
		if(qbi == undefined){
			
		    window.external.fun_google_error();//發生錯誤時，呼叫C#重新載入網頁
			
		}else{		
			qbi.click();//物件存在才點擊
		}
		
		
		window.setTimeout(function () {
			fun_input();
		}, 300);
		return;

	} else {

		qbui.value = '{{base64}}';//這裡會在C#裡面被取代成正確的網址

		window.setTimeout(function () {

			//document.getElementById('qbf').submit();
			document.getElementById('qbbtc').getElementsByTagName('input')[0].click();//送出

		}, 100);
	   
	}

}