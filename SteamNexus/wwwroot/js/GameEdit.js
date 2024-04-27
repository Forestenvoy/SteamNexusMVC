function ImageChange(event) {
    if (event.target.value != "") {
        var img = new Image();
        img.onload = function () {
            // 當圖片載入成功時，將其設置為預覽
            $("#imgPreview").attr("src", event.target.value);
        };
        img.onerror = function () {
            // 如果網址有效但沒有圖片，顯示預設圖片
            $("#imgPreview").attr("src", "/img/noImage.png");
        };
        img.src = event.target.value; // 設置圖片網址來檢查它是否有效
    }
    else {
        $("#imgPreview").attr("src", "/img/noImage.png");
    }
    
}

function VideoChange(event) {
    if (event.target.value != "") {
        $("#videoPreview").attr("src", event.target.value);
    } else {
        $("#videoPreview").attr("src", "/img/noImage.png");
    }
    
}