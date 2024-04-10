function previewImage(string, img) {
    if (string) {
        if (string.indexOf('jpg')!==-1) {
            img.attr("src", string);
        }
        else if (string.indexOf('png') !== -1) {
            img.attr("src", string);
        }
        else if (string.indexOf('jpeg') !== -1) {
            img.attr("src", string);
        }
        else {
            alert("這不是圖片檔");
        }
        
    }
}
function previewVideo(string, Video) {
    if (string) {
        if (string.indexOf('movie') !== -1) {
            Video.attr("src", string);
        }
        else {
            alert("這不是影片檔");
        }

    }
}