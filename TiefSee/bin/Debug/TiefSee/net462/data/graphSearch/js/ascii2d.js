
//https://ascii2d.net/

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


var file_file = dataURItoBlob('{{base64}}');//這裡會在C#裡面被取代成正確的網址

//封裝要傳送的參數跟物件
var fd = new FormData();
fd.append('file', file_file);
try {
	fd.append('authenticity_token', document.getElementsByName('authenticity_token')[0].value);
	fd.append('utf8', '✓');
} catch (e) { }


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

			window.external.fun_open_url(s_url);//用瀏覽器開啟網址
			window.external.fun_close();//關閉視窗

			bool_run = true;
		}
	} catch (e) { }

};
request.send(fd);