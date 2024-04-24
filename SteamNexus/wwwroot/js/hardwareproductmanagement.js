// 創建一個新的 script 元素
var newScript = document.createElement('script');

// 設置新 script 的 src 屬性
newScript.src = '新的 JavaScript 文件的 URL';

// 定義當新 script 元素加載完成後執行的函式
newScript.onload = function () {
    // 當新 JavaScript 文件加載完成後，執行 document.ready 函式
    $(document).ready(function () {
        // 這裡放置需要在 document.ready 時執行的代碼
    });
};

// 將新 script 元素插入到文檔中
document.head.appendChild(newScript);