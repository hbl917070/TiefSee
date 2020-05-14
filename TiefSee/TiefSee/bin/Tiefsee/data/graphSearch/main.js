/**
 * 作者：hbl917070（深海異音）
 * 最後更新：2020/04/11
 * 
 */

//▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
// 這裡是使用者設定


//輸入你的sauceNAO「api key」。在「https://saucenao.com/user.php?page=search-api」裡面可以找到
var saucenao_api_key = '4faa605f1524c4f2ea929b26c15c77edfed3a333';


//▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

var net = window.external;

var gsType = net.Config_gsType();//搜圖類型。ascii2d , google , saucenao , yandex , bing, iqdb
var fileName = net.Config_fileName();//要搜圖的圖片路徑
var isThis = (net.isThis() == 'true');//true=於空白網頁載入


//----------------------------------

init();//初始化載入CSS

if (net.File_Exists(fileName) == 'false') {

	if (fileName == '') {
		print('沒有輸入圖片。');
		print('命令列指令「"搜圖種類" "圖片路徑"」');
		print('範例："google" "C:/a.jpg"');
		print('搜圖類型有6種 => ascii2d, google, saucenao, yandex, bing, iqdb');
	} else {
		print('找不到檔案 => \n' + fileName);
	}


} else {

	if (gsType == 'ascii2d') {
		func_ascii2d();
	} else if (gsType == 'google') {
		func_google();
	} else if (gsType == 'saucenao') {
		func_saucenao();
	} else if (gsType == 'yandex') {
		func_yandex();
	} else if (gsType == 'bing') {
		func_bing();
	} else if (gsType == 'iqdb') {
		func_iqdb();
	} else {
		print('沒有輸入正確的搜圖類型');
	}


}



/**
 * 
 */
function func_ascii2d() {

	print('搜圖類型 => Ascii2d (二次元画像詳細検索)' + '\n' +
		'圖片路徑 => ' + fileName);


	if (isThis) {
		net.Web_Load('https://ascii2d.net/');
		print('開始載入 => https://ascii2d.net/search/file');
		return;
	}

	print('開始載入 => https://ascii2d.net/search/file');
	print('載入完成 => https://ascii2d.net/search/file');

	var base64 = "data:image/jpeg;base64," + net.File_Load_base64(fileName);
	var file_file = dataURItoBlob(base64);

	//封裝要傳送的參數跟物件
	var fd = new FormData();
	fd.append('file', file_file);

	var authenticity_token = '';
	try {
		authenticity_token = document.getElementsByName('authenticity_token')[0].value;
		print('authenticity_token => ' + authenticity_token);
	} catch (e) {
		print('error => 取得「authenticity_token」失敗');
	}

	fd.append('authenticity_token', authenticity_token);
	fd.append('utf8', '✓');

	print('XMLHttpRequest post => https://ascii2d.net/search/file');

	var bool_run = false;//避免重複執行
	var request = new XMLHttpRequest();
	request.open('POST', 'https://ascii2d.net/search/file');
	request.onreadystatechange = function (response) {

		var html = document.createElement('div');
		html.innerHTML = response.target.response;

		try {
			if (html.getElementsByClassName('detail-link').length > 0) {
				if (bool_run) {
					return;
				}

				//從html裡面取得url
				var s_url = html.getElementsByClassName('detail-link')[0].getElementsByTagName('a')[0].href;
				bool_run = true;
				print('返回搜圖結果 => ' + s_url);
				net.OpenUrl(s_url);//用瀏覽器開啟網址
				net.Windows_close();//關閉視窗				
			}
		} catch (e) {

			print('HTML解析失敗 => \n' + e.toString());
		}

	};
	request.send(fd);

}


/**
 * 
 */
function func_google() {


	print('搜圖類型 => Google' + '\n' +
		'圖片路徑 => ' + fileName);

	print('HttpClient post => https://www.google.com.tw/searchbyimage/upload');


	setTimeout(function () {


		net.HttpClient_init();
		net.HttpClient_formDataAdd('btnG', '以圖搜尋');
		net.HttpClient_formDataAdd('image_content', '');
		net.HttpClient_formDataAdd('filename', '123.jpg');
		//net.HttpClient_formDataAdd('hl', 'zh-TW');
		net.HttpClient_formDataAdd('hl', navigator.language);
		net.HttpClient_formFileAdd('encoded_image', fileName);
		//net.HttpClient_formFileAdd('file', fileName);
		var bool = net.HttpClient_submit('post', 'https://www.google.com.tw/searchbyimage/upload');

		if (bool == 'true') {

			//var rhtml = net.HttpClient_getHTML();
			var rurl = net.HttpClient_getUrl();//轉址後的網址

			print('返回搜圖結果 => ' + rurl);

			net.OpenUrl(rurl);//用瀏覽器開啟網址
			net.Windows_close();//關閉視窗	

		} else {
			print('HttpClient error => \n' + bool);

		}
	}, 50);


}


/**
 * 
 */
function func_saucenao() {


	if (saucenao_api_key != '') {
		saucenao_api_key = '?api_key=' + saucenao_api_key;
	}

	print('搜圖類型 => SauceNAO' + '\n' +
		'圖片路徑 => ' + fileName);

	print('HttpClient post => https://saucenao.com/search.php');

	setTimeout(function () {

		net.HttpClient_init();
		//net.HttpClient_formDataAdd('', '');
		net.HttpClient_formFileAdd('file', fileName);
		var bool = net.HttpClient_submit('post', 'https://saucenao.com/search.php' + saucenao_api_key);

		if (bool == 'true') {

			var rhtml = net.HttpClient_getHTML();
			//var rurl = net.HttpClient_getUrl();//轉址後的網址

			var obj_div = document.createElement('div');
			obj_div.innerHTML = rhtml;
			var rr = obj_div.querySelector('#yourimage img');
			if (rr == undefined) {
				print('HTML解析失敗 => 找不到「#yourimage img」')
				return;
			}
			rr = rr.src.replace('about:userdata/', 'https://saucenao.com/userdata/');

			var rurl = 'http://saucenao.com/search.php?db=999&url=' + rr;
			print('返回搜圖結果 => ' + rurl);

			net.OpenUrl(rurl);//用瀏覽器開啟網址
			net.Windows_close();//關閉視窗	

		} else {
			print('HttpClient error => \n' + bool);
		}

	}, 50);

}


/**
 * 
 */
function func_bing() {

	print('搜圖類型 => bing' + '\n' +
		'圖片路徑 => ' + fileName);

	print('HttpClient post => https://www.bing.com/images/search?view=detailv2&iss=sbiupload&FORM=SBIIDP&sbisrc=ImgDropper&idpbck=1');

	var base64 = net.File_Load_base64(fileName);

	setTimeout(function () {

		net.HttpClient_init();
		net.HttpClient_formDataAdd('imageBin', base64);
		net.HttpClient_formDataAdd('imgurl', '');
		net.HttpClient_formDataAdd('cbir', '');
		//net.HttpClient_formFileAdd('file', fileName);
		var bool = net.HttpClient_submit('post', 'https://www.bing.com/images/search?view=detailv2&iss=sbiupload&FORM=SBIIDP&sbisrc=ImgDropper&idpbck=1');

		if (bool == 'true') {
			//var rhtml = net.HttpClient_getHTML();
			var rurl = net.HttpClient_getUrl();//轉址後的網址

			print('返回搜圖結果 => ' + rurl);
			//document.body.innerHTML = rhtml;
			net.OpenUrl(rurl);//用瀏覽器開啟網址
			net.Windows_close();//關閉視窗		

		} else {
			print('HttpClient error => \n' + bool);
		}
	}, 50);


}


/**
 * 
 */
function func_yandex() {

	print('搜圖類型 => yandex' + '\n' +
		'圖片路徑 => ' + fileName);

	if (isThis) {
		net.Web_Load('https://yandex.com/images');
		print('開始載入 => https://yandex.com/images');
		return;
	}

	print('開始載入 => https://yandex.com/images');
	print('載入完成 => https://yandex.com/images');

	try {

		var base64 = 'data:image/jpeg;base64,' + net.File_Load_base64(fileName);
		var file_file = dataURItoBlob(base64);//這裡會在C#裡面被取代成正確的網址

		var serpid = Ya.SerpContext.serpid;
		print('取得 serpid => ' + serpid);

		//封裝要傳送的參數跟物件
		var fd = new FormData();
		fd.append('upfile', file_file);

		var posturl = 'https://yandex.com/images/search?serpid=' + serpid +
			'&serpListType=horizontal&uinfo=sw-1920-sh-1080-ww-1536-wh-746-pd-1.25-wp-16x9_2560x1440&rpt=imageview&format=json&request=%7B%22blocks%22%3A%5B%7B%22block%22%3A%22b-page_type_search-by-image__link%22%7D%5D%7D';

		print('XMLHttpRequest post => ' + posturl);

		//&serpListType=horizontal
		//&serpListType=vertical
		var bool_run = false;//避免重複執行
		var request = new XMLHttpRequest();
		request.open('POST', posturl);
		request.onreadystatechange = function (response) {
			if (bool_run) {
				return;
			}
			var j = response.target.response;

			try {


				if (j != '') {
					j = JSON.parse(j);
					var rurl = 'https://yandex.com/images/search?rpt=imageview&' + j.blocks[0].params.url;
					bool_run = true;

					print('返回搜圖結果 => ' + rurl);

					net.OpenUrl(rurl);//用瀏覽器開啟網址
					net.Windows_close();//關閉視窗	
				}

			} catch (e2) {
				print('json解析失敗 => \n' + j);
			}

			//net.print(response)			

		};
		request.send(fd);

	} catch (e) {
		print('XMLHttpRequest error => \n' + e)
	}




	//https://yandex.com/images/search?rpt=imageview&cbir_id=2127935%2FGo7yPP7NcG4dqu0KDUCRPQ
	//cbir_id = 2047231 % 2FoUoJvjFeQQrm5p27zOnwag & uinfo=sw - 1920 - sh - 1080 - ww - 1536 - wh - 746 - pd - 1.25 - wp - 16x9_2560x1440 & rpt=imageview

	/*try {

		var serpid = Ya.SerpContext.serpid;
		net.print('****serpid  ' + serpid);

		net.HttpClient_init();
		net.HttpClient_setCookies();
		//net.HttpClient_formDataAdd('serpid', Ya.SerpContext.serpid);
		net.HttpClient_formFileAdd('upfile', fileName);
		net.HttpClient_submit('post', 'https://yandex.com/images/search?serpid=' + serpid +
			'&uinfo=sw-1920-sh-1080-ww-1536-wh-746-pd-1.25-wp-16x9_2560x1440&rpt=imageview&format=json&request=%7B%22blocks%22%3A%5B%7B%22block%22%3A%22b-page_type_search-by-image__link%22%7D%5D%7D');

		var rhtml = net.HttpClient_getHTML();
		//var rurl = net.HttpClient_getUrl();//轉址後的網址


		net.print('****rhtml  ' + rhtml);
		document.body.innerHTML = '<div>' + rhtml + '</div>';

		//net.OpenUrl(rurl);//用瀏覽器開啟網址
		net.Windows_close();//關閉視窗	
	} catch (e) {
		alert(e)
	}*/


}






/**
 * 
 */
function func_iqdb() {

	print('搜圖類型 => iqdb (Multi-service image search)' + '\n' +
		'圖片路徑 => ' + fileName);

	print('HttpClient post => https://www.iqdb.org/');

	setTimeout(function () {

		net.HttpClient_init();
		//net.HttpClient_formDataAdd('', '');
		net.HttpClient_formFileAdd('file', fileName);
		var bool = net.HttpClient_submit('post', 'https://www.iqdb.org/');

		if (bool == 'true') {

			var rhtml = net.HttpClient_getHTML();
			//var rurl = net.HttpClient_getUrl();//轉址後的網址

			//<script type="text/javascript">try{document.getElementById("yetmore").href = "/?thu=736b5b57&org=hello1.jpg&more=1";}catch(e){document.write(" (not supported by your browser)");}</script>
			var inof = rhtml.indexOf('/?thu=');

			if (inof == -1) {
				print('HTML解析失敗 => 找不到「/?thu=」')
				return;

			} else {
				var s2 = rhtml.substr(inof);
				s2 = s2.substr(0, s2.indexOf('"'));

				var rurl = 'http://iqdb.org' + s2;
				print('返回搜圖結果 => ' + rurl);

				net.OpenUrl(rurl);//用瀏覽器開啟網址
				net.Windows_close();//關閉視窗	
			}


		} else {
			print('HttpClient error => \n' + bool);
		}

	}, 50);

}







/**
 * 
 * @param {*} s 
 */
function print(s) {

	var body2 = document.getElementById('body2');
	let list = document.createElement('list');
	list.innerHTML = s.replace(/\n/ig, '<br>');
	body2.appendChild(list);
	net.print(s);
}


/**
 * 
 */
function init() {

	//document.body.style.display = 'none';

	let body2 = document.createElement('body2');
	body2.setAttribute('id', 'body2');
	document.body.appendChild(body2);

	let obj_style = document.createElement('style');
	obj_style.innerHTML = heredoc(function () {/*
		body {
			overflow: hidden !important;
        }
        body * {
			display: none !important;
			overflow: hidden ;
        }

        body2,
        body2 * {
            display: block !important;
        }

        body2 {
            position: fixed !important;
            top: 0 !important;
            left: 0 !important;
            right: 0 !important;
            bottom: 0 !important;
            background-color: rgb(255, 255, 255) !important;
            font-family: "微軟正黑體", Microsoft JhengHei, "黑體-繁", "蘋果儷中黑", sans-serif !important;
            display: block !important;
            overflow-y: auto !important;
            overflow-x: hidden !important;
        }

        body2 list {
            font-size: 14px !important;

            color: #000 !important;
            font-weight: 0 !important;
            line-height: 16px !important;

            display: block !important;
            padding: 3px !important;
            padding-top: 6px !important;
            margin: 5px !important;
            border-top: 1px solid rgb(219, 219, 219) !important;
            margin-right: 10px !important;
            margin-left: 10px !important;
            word-wrap: break-word !important;
            word-break: break-all !important;
        }

        body2 list:first-child {
            margin-top: 3px !important;
            border-top: 0px solid rgba(0, 0, 0, 0) !important;
        }

        body2 list:last-child {
            margin-bottom: 80px !important;

        }
 */});


	document.body.appendChild(obj_style);
}



//-------------------------------------------------------------------
//-------------------------------------------------------------------
//-------------------------------------------------------------------



function heredoc(fn) {
	return fn.toString().split('\n').slice(1, -1).join('\n') + '\n'
}


/**
 * 
 * @param {*} base64Data 
 */
function dataURItoBlob(base64Data) {
	var byteString;
	if (base64Data.split(',')[0].indexOf('base64') >= 0)
		byteString = atob(base64Data.split(',')[1]);
	else
		byteString = unescape(base64Data.split(',')[1]);
	var mimeString = base64Data.split(',')[0].split(':')[1].split(';')[0];
	var ia = new Uint8Array(byteString.length);
	for (var i = 0; i < byteString.length; i++) {
		ia[i] = byteString.charCodeAt(i);
	}

	return new Blob([ia], { type: mimeString });
}







function old() {

	var file_base64 = 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAC4AAAA7CAIAAACLwC9IAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAItSURBVGhD7ZYhtsIwEEUjkUjkXwJLQLIEJBKJxCE5rADJEpBIJJJlwA6Q/Hc6jzInmTYtyecjch3TkNwmM5m6x9dQVCyKikVRsSgqFkXFoqhYpKocj8fJZLLf7/k7gVSV0WjknBsMBvydQKoKPAT+TqCoWHyfCgrhp6JvLVAkowokZMZetXC9XuVfgKEEOMVms+GUnSeVjZS/jMdjRhN4LSyTAiwAM0abkRtFOBwOjCbwUsHRcOKKy+XCBw1wnHPT6ZShNF4q+oxAewqfTqd6GEPJ+JmBzZA1QEsKz+dzGbNarRhKxkjSxWIhywBzY9brNR87dz6fGU3GUAF13oQbs9vt5BHIlSWCraLzBnvAaEVdOPC43++M5sBWAcvlUpYEdQqjaBlyLq8HaFTBSnhvLlud1Ha71QXPcflomxE2daV4ILU5KB/xl/PuG5Dlbg3ptM+3240WFe2339vEVbAHw+GQFk+QNGjLHJGJuIpuexrYdOma3Ymo4EuAKz9vXl3k4QWYQkRFbwlD1ZHpqs6VOhEVrhZUr94bkGV7GlX0Rxpg9El45Xj94Q1sFXjoqpnNZnwQ4PUHNEs+6I+tolMEHi3txusPApzeKC5bRWclQ8009YfoJ6mHvVJ92XfvNaZQr+KKv3Rf9CepJppJ+VWA/iT1aCm0P1ERwpYuNJ3aH6powkILhT6kAsK89u7oz6kI3qkxWvFpFUGEvJvif1RMiopFUbEoKhZfo/J4/AI6dYvIZ2UWJQAAAABJRU5ErkJggg==';
	var file_file = dataURItoBlob(file_base64);//這裡會在C#裡面被取代成正確的網址


	/**
	 * 
	 */
	function old_yandex() {


		//封裝要傳送的參數跟物件
		var fd = new FormData();
		fd.append('upfile', file_file);


		var postUrl = 'https://yandex.com/images/search?serpid=' + JSON.parse(document.body.dataset.bem)['i-global']['serpid'] + '&serpListType=horizontal&rpt=imageview&format=json&request=%7B%22blocks%22%3A%5B%7B%22block%22%3A%22b-page_type_search-by-image__link%22%7D%5D%7D'
		var html = '';
		var bool_run = false;//避免重複執行
		var request = new XMLHttpRequest();
		request.open('POST', postUrl);
		request.onreadystatechange = function (response) {

			html = document.createElement('div');
			html.innerHTML = response.target.response;

			var obJ_img = html.querySelector('#yourimage img');//圖片物件

			try {
				if (obJ_img != undefined) {
					if (bool_run) {
						return;
					}

					//從html裡面取得url
					var web_href = 'https://saucenao.com/search.php?url=' + obJ_img.src;

					//console.log(web_href)
					window.external.fun_open_url(web_href);//用瀏覽器開啟網址
					window.external.fun_close();//關閉視窗

					bool_run = true;
				}
			} catch (e) { }

		};
		request.send(fd);

	}


	/**
	 * 
	 */
	function old_google() {

		//封裝要傳送的參數跟物件
		var fd = new FormData();
		fd.append('encoded_image', file_file);
		fd.append('btnG', '以圖搜尋');
		fd.append('image_content', '');
		fd.append('filename', '123.jpg');
		fd.append('hl', navigator.language);
		//fd.append('hl', 'zh-TW');


		var postUrl = 'https://www.google.com.tw/searchbyimage/upload'
		var html = '';
		var bool_run = false;//避免重複執行
		var request = new XMLHttpRequest();
		request.open('POST', postUrl);
		request.onreadystatechange = function (response) {

			//console.log(request.getAllResponseHeaders())

			html = document.createElement('div');
			html.innerHTML = response.target.response;

			var obJ_img = html.getElementsByTagName('noscript');

			//try {
			if (obJ_img != undefined) {
				if (bool_run) {
					return;
				}

				//從html裡面取得url
				var web_href = '';
				for (var i = 0; i < obJ_img.length; i++) {
					var ust = obJ_img[i].innerHTML.indexOf('/search?tbs=');
					if (ust > -1) {
						var su = obJ_img[i].innerHTML.substr(ust)
						su = su.substr(0, su.indexOf('"'));
						su = 'https://www.google.com.tw' + su + '&safeui=off';
						web_href = su;
					}
				}

				console.log(web_href)
				if (web_href != '') {
					bool_run = true;
					request.abort();
					net.fun_open_url(web_href);//用瀏覽器開啟網址
					net.fun_close();//關閉視窗
				} else {
					//console.log(obJ_img[i])

				}

			}
			//} catch (e) { }

		};
		request.send(fd);

	}


	/**
	 * 
	 */
	function old_saucenao() {


		//封裝要傳送的參數跟物件
		var fd = new FormData();
		fd.append('file', file_file);
		fd.append('submit', 'get sauce');


		var bool_run = false;//避免重複執行
		var request = new XMLHttpRequest();
		request.open('POST', 'https://saucenao.com/search.php');
		request.onreadystatechange = function (response) {

			var html = document.createElement('div');
			html.innerHTML = response.target.response;

			var obJ_img = html.querySelector('#yourimage img');//圖片物件

			try {
				if (obJ_img != undefined) {
					if (bool_run) {
						return;
					}

					//從html裡面取得url
					var web_href = 'https://saucenao.com/search.php?url=' + obJ_img.src;

					//console.log(web_href)
					window.external.fun_open_url(web_href);//用瀏覽器開啟網址
					window.external.fun_close();//關閉視窗

					bool_run = true;
				}
			} catch (e) { }

		};
		request.send(fd);
	}

	/**
	 * 
	 */
	function old_bing() {


		//封裝要傳送的參數跟物件
		var fd = new FormData();
		fd.append('imgurl', '');
		fd.append('cbir', 'sbi');
		fd.append('imageBin', file_base64);

		var bool_run = false;//避免重複執行

		var r = '';

		var request = new XMLHttpRequest();
		request.open('POST', 'https://www.bing.com/images/search?view=detailv2&iss=sbiupload&FORM=SBIIDP&sbisrc=ImgDropper&idpbck=1&sbifsz=46+x+59+%c2%b7+0.81+kB+%c2%b7+png&sbifnm=%E6%9C%AA%E5%91%BD%E5%90%8D.png&thw=46&thh=59&ptime=13&dlen=1104&expw=46&exph=59');
		request.onreadystatechange = function (response) {

			var html = document.createElement('div');
			html.innerHTML = response.target.response;

			r = response;


		};
		request.send(fd);

	}
}


