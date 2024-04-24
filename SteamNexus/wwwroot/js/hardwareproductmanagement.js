$(document).ready(function () {
    // 計算被響應式隱藏的 column
    var responsiveColumnsCount = 0;
    // 重新整理檢測
    var isrefresh = false;

    // 初始化 Datatables
    var datatable = new DataTable('#HardwareManageTable', {
        // column 定義
        columns: [
            { "data": "productId", "width": "5%", "className": "text-center" },
            { "data": "productName", "width": "34%" },
            { "data": "specification", "width": "29%" },
            { "data": "componentClassificationName", "visible": false },
            { "data": "price", "width": "8%", "className": "text-center" },
            {
                "data": "wattage",
                "width": "8%",
                "className": "text-center",
                // 瓦數 ~ 文字輸入方塊
                render: function (data, type, row, meta) {
                    // 此處 type 為 'sort' 表示 DataTables 正在進行排序
                    if (type === 'sort') {
                        // 返回元素的值作為排序依據
                        return data;
                    }
                    // 取得 productId
                    let productId = row.productId;
                    // input 欄位 ~ 可編輯
                    let inputEle = `<input type="text" class="${productId}_watt defaultcellType" value="${data}" style="width: 50px; text-align: center;"  disabled>`;
                    return inputEle;
                },
                // 將此列的數據類型設置為數字 ~ 排序才會正常運作
                "type": "num"
            },
            {
                "data": "recommend",
                "width": "8%",
                "className": "text-center",
                // 推薦 ~ 下拉式選單
                render: function (data, type, row, meta) {
                    // 此處 type 為 'sort' 表示 DataTables 正在進行排序
                    if (type === 'sort') {
                        // 返回元素的值作為排序依據
                        return data;
                    }
                    // 取得 productId
                    let productId = row.productId;
                    const isRec1 = data == 0 ? 'selected' : '';
                    const isRec2 = data == 1 ? 'selected' : '';
                    const isRec3 = data == 2 ? 'selected' : '';
                    const isRec4 = data == 3 ? 'selected' : '';
                    const ele1 = `<option value = "0" ${isRec1}>無</option>`;
                    const ele2 = `<option value = "1" ${isRec2}>基本</option>`;
                    const ele3 = `<option value = "2" ${isRec3}>優良</option>`;
                    const ele4 = `<option value = "3" ${isRec4}>完美</option>`;
                    // select 欄位 ~ 可編輯
                    let selectEle = `<select class="${productId}_recommend defaultcellType" disabled>${ele1}${ele2}${ele3}${ele4}</select>`;
                    return selectEle;
                },
                // 將此列的數據類型設置為數字 ~ 排序才會正常運作
                "type": "num"
            },
            {
                "data": null,
                "orderable": false,
                "width": "8%",
                // 按鈕 自定義
                render: function (data, type, row, meta) {
                    // 取得 productId
                    let productId = row.productId;
                    // 編輯按鈕
                    let editEle = `<button class="${productId}_edit btn Edit-btn d-flex justify-content-center align-items-center" style="width:30px;height:30px;" data-slefColumn-isShow="false"><i class="fa-solid fa-pen-to-square"></i></button>`;
                    let div = `<div class="d-flex justify-content-between" id="${productId}_div">${editEle}</div>`;
                    if (type === 'display') {
                        return `${div}`;
                    }
                    return data;
                },
                "type": "html"
            }
        ],
        // 標頭固定
        fixedHeader: {
            // 固定 header
            header: true,
            // 使用導覽欄的高度作為偏移量
            headerOffset: $('#pageNav').outerHeight(),
        },
        // 響應式設計
        responsive: true,
        // 語言設定
        language: {
            url: '//cdn.datatables.net/plug-ins/2.0.3/i18n/zh-HANT.json',
        },
        // 預設排序 ~ 根據第一個欄位 升冪排序
        order: [[1, 'asc']],
        // 行分組
        rowGroup: {
            dataSrc: 'componentClassificationName',
        },
        // 資料載入中 gif
        processing: true,
        // 自動寬度 關閉
        autoWidth: true,
        // 資料行樣式
        createdRow: function (row, data, dataIndex) {
            if (data.recommend === 1) {
                row.classList.add('green-row');
            } else if (data.recommend === 2) {
                row.classList.add('blue-row');
            } else if (data.recommend === 3) {
                row.classList.add('red-row');
            }
            // 加上 ID
            $(row).attr('id', data.productId + '_row');
            // 為特定行加入 class ==> 不會影響 th
            $(row).find('td:eq(6)').addClass('d-flex');
            $(row).find('td:eq(6)').addClass('justify-content-center');
        },
        // 按鈕建立
        layout: {
            topMiddle: {
                buttons: [
                    {
                        text: '重新整理',
                        // 按鈕點擊事件
                        action: function (e, dt, node, config) {
                            isrefresh = true;
                            // 重新整理
                            getdatatableData();
                        }
                    },
                    {
                        text: '單一零件更新',
                        // 按鈕點擊事件
                        action: function (e, dt, node, config) {
                            // 單一零件更新
                            // UpdateOneHardware();
                        }
                    },
                    {
                        text: '全零件更新',
                        // 按鈕點擊事件
                        action: function (e, dt, node, config) {
                            // 全零件更新
                            // UpdateAllHardware();
                        }
                    },

                ]
            },
        },
    });

    // 響應式cloumn重構事件
    datatable.on('responsive-resize', function (e, datatable, columns) {
        // 計算隱藏的行數
        responsiveColumnsCount = columns.reduce(function (a, b) {
            return b === false ? a + 1 : a;
        }, 0);
        if (responsiveColumnsCount === 0) {
            // 將 展開屬性設定 false
            $('.Edit-btn').attr('data-slefColumn-isShow', 'false');
        }
    });
    // 子行展開收縮事件觸發
    datatable.on('responsive-display.dt', function (e, datatable, row, showHide, update) {

    });



    // 資料編輯 ~ 當任何具有 Edit-btn 類別元素被點擊時觸發
    $(document).on('click', '.Edit-btn', function () {
        // 被隱藏的按鈕不能觸發
        if ($(this).css('display') !== 'none') {
            // 檢測有隱藏的行數
            // if (responsiveColumnsCount > 0) {
            //     // 檢查使用者有沒有展開
            //     if ($(this).attr('data-slefColumn-isShow') === 'false') {
            //         // 取得吐司容器元素
            //         let ToastContainer = document.querySelector('#ToastContainer');
            //         // 流水號生成
            //         let toastId = Math.floor(Math.random() * 1000);
            //         // 吐司元素生成
            //         let toast = ToastInitialization(toastId);
            //         // 發送吐司
            //         ToastContainer.insertAdjacentHTML(`afterbegin`, toast);
            //         $(`#${toastId}_Toast`).show();
            //         // 隱藏 載入狀態
            //         $(`#${toastId}_Toast_Status`).hide();
            //         // 顯示 吐司失敗狀態
            //         $(`#${toastId}_ToastIcon_fail`).addClass('animate__zoomIn').removeClass('d-none');
            //         $(`#${toastId}_ToastText`).text("請先展開該列");
            //         // 進度條消失
            //         $(`#${toastId}_ToastProgress`).show();
            //         // 更改顏色
            //         $(`#${toastId}_ToastProgress_rect`).css('fill', '#FF0000');
            //         // 進度條消失
            //         ToastProgressDisappear(toastId);
            //         return;
            //     }
            // }
            // 取得 ID
            let productId = Number($(this).attr('class').slice(0, 5));
            // 取資料 ~ 只取看的見的元素
            let wattVal = Number($(`.${productId}_watt:visible`).val());
            let recVal = Number($(`.${productId}_recommend:visible`).val());
            // 確認按鈕
            let enterEle = `<button class="${productId}_enter btn Enter-btn d-flex justify-content-center align-items-center" style="width:30px;height:30px;"><i class="fa-solid fa-check"></i></button>`;
            // 取消按鈕
            let cancellEle = `<button class="${productId}_reset btn Cancell-btn d-flex justify-content-center align-items-center" style="width:30px;height:30px;" data-origin="${wattVal},${recVal}"><i class="fa-solid fa-xmark"></i></button>`;
            $(`#${productId}_div`).html(`${enterEle}&nbsp;&nbsp;&nbsp;${cancellEle}`);
            // 編輯模式 ~ 開啟 td 編輯功能
            $(`.${productId}_watt`).prop('disabled', false);
            $(`.${productId}_watt`).removeClass("defaultcellType");
            $(`.${productId}_recommend`).prop('disabled', false);
            $(`.${productId}_recommend`).removeClass("defaultcellType");

        }
    });

    // 資料變更確認 ~ 當任何具有 Enter-btn 類別元素被點擊時觸發
    $(document).on('click', '.Enter-btn', function () {
        // 被隱藏的按鈕不能觸發
        if ($(this).css('display') !== 'none') {
            // 取得 ID
            let productId = Number($(this).attr('class').slice(0, 5));

            // 取資料 ~ 只取看的見的元素
            let wattVal = Number($(`.${productId}_watt:visible`).val());
            let recVal = Number($(`.${productId}_recommend:visible`).val());

            // 取得吐司容器元素
            let ToastContainer = document.querySelector('#ToastContainer');
            // 資料包裝
            var data = {
                ProductId: productId,
                Wattage: wattVal,
                Recommend: recVal
            };
            // 獲取防偽標籤值
            let csrfToken = document.querySelector('input[name="__Antiforgery__SteamNexus"]').value;
            // 流水號生成
            let toastId = Math.floor(Math.random() * 1000);
            // 吐司元素生成
            let toast = ToastInitialization(toastId);
            // 發送吐司
            ToastContainer.insertAdjacentHTML(`afterbegin`, toast);
            // 狀態值
            let PromiseStatus = false;
            $(`#${toastId}_Toast`).show();
            // 發送非同步POST請求 ==> 資料庫資料變更
            fetch(`/Administrator/HardwareManagement/ProductDataUpdate`, {
                method: "POST",
                body: JSON.stringify(data),
                headers: {
                    "Content-Type": "application/json",
                    "X-CSRF-TOKEN": csrfToken
                }
            }).then(result => {
                // 隱藏吐司 loading
                $(`#${toastId}_Toast_Status`).addClass('animate__zoomOut');
                $(`#${toastId}_Toast_Status`).hide();
                // 如過 HTTP 響應的狀態馬碼在 200 到 299 的範圍內 ==> .ok 會回傳 true
                if (result.ok) {
                    // 此時 result 是一個請求結果的物件 => 注意傳回值型態，字串用 text()，JSON 用 json()
                    PromiseStatus = true;
                    return result.text();
                }
                return result.text();
            }).then(data => {
                // 此時 data 為上一個 then 回傳的資料
                if (PromiseStatus) {
                    // 顯示 吐司成功狀態
                    $(`#${toastId}_ToastIcon_success`).addClass('animate__zoomIn').removeClass('d-none');
                    $(`#${toastId}_ToastText`).text(data);
                    // 顯示進度條
                    $(`#${toastId}_ToastProgress`).show();
                    // 進度條消失
                    ToastProgressDisappear(toastId);
                    // 恢復預設模式 ~ 關閉 td 編輯功能
                    $(`.${productId}_watt`).attr('disabled', true);
                    $(`.${productId}_watt`).addClass("defaultcellType");
                    $(`.${productId}_recommend`).attr('disabled', true);
                    $(`.${productId}_recommend`).addClass("defaultcellType");
                    // 更換成編輯按鈕
                    let editEle = `<button class="${productId}_edit btn Edit-btn d-flex justify-content-center align-items-center" style="width:30px;height:30px;" data-bs-toggle="popover" data-bs-content="nothing"><i class="fa-solid fa-pen-to-square"></i></button>`;
                    $(`#${productId}_div`).html(editEle);
                }
                else {
                    // 顯示 吐司失敗狀態
                    $(`#${toastId}_ToastIcon_fail`).addClass('animate__zoomIn').removeClass('d-none');
                    $(`#${toastId}_ToastText`).text(data);
                    // 進度條消失
                    $(`#${toastId}_ToastProgress`).show();
                    // 更改顏色
                    $(`#${toastId}_ToastProgress_rect`).css('fill', '#FF0000');
                    // 進度條消失
                    ToastProgressDisappear(toastId);
                }
            }).catch(error => {
                // 顯示 吐司失敗狀態
                $(`#${toastId}_ToastIcon_fail`).addClass('animate__zoomIn').removeClass('d-none');
                $(`#${toastId}_ToastText`).text(error.message);
            });
        }
    });

    // 資料變更取消 ~ 當任何具有 Cancell-btn 類別元素被點擊時觸發
    $(document).on('click', '.Cancell-btn', function () {
        // 被隱藏的按鈕不能觸發
        if ($(this).css('display') !== 'none') {
            // 取得 ID
            let productId = Number($(this).attr('class').slice(0, 5));
            // 取得原始資料
            let origin = $(this).attr('data-origin').split(',');
            let wattVal = Number(origin[0]);
            let recVal = Number(origin[1]);
            // 恢復預設模式 ~ 關閉 td 編輯功能
            $(`.${productId}_watt`).attr('disabled', true);
            $(`.${productId}_watt`).val(wattVal);
            $(`.${productId}_watt`).addClass("defaultcellType");
            $(`.${productId}_recommend`).attr('disabled', true);
            $(`.${productId}_recommend`).val(recVal);
            $(`.${productId}_recommend`).addClass("defaultcellType");
            // 更換成編輯按鈕
            let editEle = `<button class="${productId}_edit btn Edit-btn d-flex justify-content-center align-items-center" style="width:30px;height:30px;" data-bs-toggle="popover" data-bs-content="nothing"><i class="fa-solid fa-pen-to-square"></i></button>`;
            $(`#${productId}_div`).html(editEle);
        }
    });

    // Toast 彈出式通知 元素初始化
    function ToastInitialization(id) {
        // loading
        let Toast_Status = `<div class="spinner-border spinner-border-sm animate__animated me-2" role="status" id="${id}_Toast_Status"></div>`;
        // 成功 svg
        let ToastIcon_success = `<div class="ToastIcon-success d-flex justify-content-center align-items-center animate__animated me-2 d-none" id="${id}_ToastIcon_success"><i class="fa-solid fa-check"></i></div>`;
        // 失敗 svg
        let ToastIcon_fail = `<div class="ToastIcon-fail d-flex justify-content-center align-items-center animate__animated me-2 d-none" id="${id}_ToastIcon_fail"><i class="fa-solid fa-xmark"></i></div>`;
        // 吐司文字
        let ToastText = `<span style="margin-top: 2px" id="${id}_ToastText">請求傳送中</span>`;
        // 吐司進度條
        let ToastProgress = `<svg width="250px" height="2px" class="position-absolute bottom-0 start-0" style="display: none" id="${id}_ToastProgress"><rect width="250px" height="2px" style="fill: #00FF00" id="${id}_ToastProgress_rect"></rect></svg>`;
        // 吐司容器
        let ToastElement = `<div class="toast align-items-center mb-3 animate__animated animate__fadeInLeft defaultToast" role="alert" aria-live="assertive" aria-atomic="true" data-bs-autohide="false" id="${id}_Toast"><div class="d-flex align-items-center"><div class="toast-body d-flex align-items-center position-relative">${Toast_Status}${ToastIcon_success}${ToastIcon_fail}${ToastText}${ToastProgress}</div></div></div>`;
        // 回傳 吐司元素
        return ToastElement;
    };

    // 吐司進度條消失
    function ToastProgressDisappear(toastId) {
        // 初始寬度
        let initialWidth = 250;
        // 每次更新寬度 ~ 總共 2000 毫秒 變動 200 次
        let widthChange = 250 / 200;
        // 計時器啟動 ~ 10 毫秒執行一次
        var intervalId = setInterval(function () {
            initialWidth -= widthChange;
            // 應用更新後的寬度
            $(`#${toastId}_ToastProgress_rect`).attr('width', `${initialWidth}px`);
            // 寬度小於0停止計時器
            if (initialWidth <= 0) {
                clearInterval(intervalId);
                // 關閉吐司
                $(`#${toastId}_Toast`).addClass(`animate__fadeOutRight`);
                // 元素移除
                setTimeout(function () {
                    $(`#${toastId}_Toast`).remove();
                }, 1000);
            }
        }, 10);
    }

    // 下拉式選單 切換 硬體產品資料
    $("#HardSelect").change(function () {
        getdatatableData();
    });

    // 獲取下拉式選單的資料類型
    function getType() {
        var selectedOption = $("#HardSelect").val();
        switch (selectedOption) {
            case "CPU 中央處理器":
                return encodeURI("Type=10000");
                break;
            case "GPU 顯示卡":
                return encodeURI("Type=10001");
                break;
            case "RAM 記憶體":
                return encodeURI("Type=10002");
                break;
            case "MotherBoard 主機板":
                return encodeURI("Type=10003");
                break;
            case "SSD 固態硬碟":
                return encodeURI("Type=10004");
                break;
            case "HDD 傳統硬碟":
                return encodeURI("Type=10005");
                break;
            case "AirCooler 風冷散熱器":
                return encodeURI("Type=10006");
                break;
            case "LiquidCooler 水冷散熱器":
                return encodeURI("Type=10007");
                break;
            case "CASE 機殼":
                return encodeURI("Type=10008");
                break;
            case "PSU 電源供應器":
                return encodeURI("Type=10009");
                break;
            case "OS 作業系統":
                return encodeURI("Type=10010");
                break;
            default:
                return "NoSeclect";
        };
    };

    // AJAX 資料載入 datatable
    function getdatatableData() {
        let p = getType();
        console.log(p);
        // 如果沒選擇硬體 => 中斷事件
        if (p == "NoSeclect") {
            return;
        }
        // 取得吐司容器元素
        let ToastContainer = document.querySelector('#ToastContainer');
        // 流水號生成
        let toastId = Math.floor(Math.random() * 1000);
        // 吐司元素生成
        let toast = ToastInitialization(toastId);
        // 發送非同步GET請求
        fetch(`/Administrator/HardwareManagement/HardwareData?${p}`, {
            method: "GET"
        }).then(result => {
            // 此時 result 是一個請求結果的物件
            if (isrefresh) {
                // 發送吐司
                ToastContainer.insertAdjacentHTML(`afterbegin`, toast);
                // 顯示吐司
                $(`#${toastId}_Toast`).show();
                // 隱藏 載入狀態
                $(`#${toastId}_Toast_Status`).hide();
            }
            // 注意傳回值型態，字串用 text()，JSON 用 json()
            if (result.ok) {
                return result.json();
            }
        }).then(data => {
            // 此時 data 為上一個 then 回傳的資料
            // 清空表格
            datatable.clear().draw();
            // 添加新的資料
            datatable.rows.add(data).draw();
            if (isrefresh) {
                // 顯示 吐司成功狀態
                $(`#${toastId}_ToastIcon_success`).addClass('animate__zoomIn').removeClass('d-none');
                $(`#${toastId}_ToastText`).text("重新整理成功");
                // 顯示進度條
                $(`#${toastId}_ToastProgress`).show();
                // 進度條消失
                ToastProgressDisappear(toastId);
            }
            isrefresh = false;
        }).catch(error => {
            alert(error);
            if (isrefresh) {
                // 顯示 吐司失敗狀態
                $(`#${toastId}_ToastIcon_fail`).addClass('animate__zoomIn').removeClass('d-none');
                $(`#${toastId}_ToastText`).text("重新整理失敗，請稍後再試");
                // 更改顏色
                $(`#${toastId}_ToastProgress_rect`).css('fill', '#FF0000');
                // 顯示進度條
                $(`#${toastId}_ToastProgress`).show();
                // 進度條消失
                ToastProgressDisappear(toastId);
            }
            isrefresh = false;
        });
    };
});


