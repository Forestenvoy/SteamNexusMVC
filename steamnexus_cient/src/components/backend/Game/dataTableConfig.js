export const dataTableConfig = {
    columns: [
        {
            "data": "imagePath",
            "width": "5%",
            "className": "text-center",
            "render": function (data, type, row) {
                return '<img src="' + data + '" alt="圖片" style="width:150px">';
            }, responsivePriority: 1
        },
        { "data": "appId", "width": "5%"},
        { "data": "name", responsivePriority: 1, "width": "5%" },
        { "data": "originalPrice", "width": "2%" },
        { "data": "currentPrice", responsivePriority: 2, "width": "2%" },
        { "data": "ageRating", "width": "5%" },
        { "data": "comment", "width": "5%" },
        { "data": "commentNum", "width": "5%" },
        { "data": "publisher", "width": "5%" },
        {
            "data": null,
            "orderable": false,
            "width": "5%",
            "className": "text-center",
            // 按鈕 自定義
            render: function (data, type, row, meta) {
                // 取得 productId
                let name = row.name;
                let GameId = row.gameId;
                // 編輯按鈕
                let editEle = '<button data-GameId="' + GameId + '"  data-name="' + name + '" id="edit_button" data-bs-toggle="popover" data-bs-content="nothing"><i class="fa-solid fa-pen-to-square"></i></button>';
                // 刪除按鈕
                let deleteEle = '<button data-GameId="' + GameId + '"  data-name="' + name + '" id="delete_button" data-bs-toggle="popover" data-bs-content="nothing"><i class="fa-solid fa-trash"></i></button>';
                if (type === 'display') {
                    return `${editEle}${deleteEle}`;
                }
                return data;
            }, responsivePriority: 1
        }
    ],
    // 標頭固定
    fixedHeader: true,
    // 響應式設計
    responsive: true,
    language: {url: '//cdn.datatables.net/plug-ins/2.0.3/i18n/zh-HANT.json'},
    // 自動寬度 關閉
    autoWidth: true,
    // 資料載入中 gif
    processing: true
}