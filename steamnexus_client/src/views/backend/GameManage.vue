<template>
  <div>
      <h1>全部遊戲<img src="/public/img/loading-block-white.gif" style="margin:20px;width:30px;height:30px;display:none" id="GameIndexLoading" /></h1>
      
      <table id="GameManageTable" class="display" style="width:100%">
          <thead>
              <tr>
                  <th></th>
                  <th>代碼</th>
                  <th>名稱</th>
                  <th>原始價格</th>
                  <th>現在價格</th>
                  <th>遊戲分級</th>
                  <th>評價</th>
                  <th>評價數量</th>
                  <th>發行商</th>
              </tr>
          </thead>
      </table>
  </div>
</template>

<script setup >
import $ from 'jquery'
import DataTable from 'datatables.net-dt'
import 'datatables.net-fixedheader-dt'
import 'datatables.net-rowgroup-dt'
import 'datatables.net-buttons-dt'
import 'datatables.net-responsive-dt'

import { ref, onMounted } from 'vue'
import { onBeforeRouteLeave } from 'vue-router'

// 從環境變數取得 API BASE URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

// 初始化 DataTables
let dataTable = null

//拿取datatable的資料
function getdatatableData() {
    // 發送非同步GET請求
    fetch(`${apiUrl}/api/GamesManagement/IndexJson`, {
        method: "GET"
    }).then(result => {
        // 此時 result 是一個請求結果的物件
        // 注意傳回值型態，字串用 text()，JSON 用 json()
        if (result.ok) {
            return result.json();
        }
    }).then(data => {
        // 添加新的資料
        if (data && data.length > 0) {
            dataTable.rows.add(data).draw();
        }
        $("#GameIndexLoading").hide();
        
    }).catch(error => {
        console.log(error);
    });
};

onMounted(() => {
  getdatatableData();
  dataTable = new DataTable('#GameManageTable', {
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
    processing: true,
    //Create按鈕建立
    layout: {
        topMiddle: {
            buttons: [
                {
                    text: '新增遊戲',
                    // 按鈕點擊事件
                    action: function (e, dt, node, config) {
                        
                        event.preventDefault();
                        event.stopPropagation();

                        $.getScript("/js/GameEdit.js")
                            .done(function (script, textStatus) {
                                console.log("加载成功");
                                fetch(`@Url.Action("GetCreatPartialView", "GamesManagement", new { area = "Administrator" })`, {
                                    method: "GET"
                                }).then(response => {
                                    // 確保請求是否成功
                                    if (!response.ok) {
                                        throw new Error('Network response was not ok');
                                    }
                                    // 解析 html
                                    return response.text();
                                }).then(val => {
                                    if (val) {
                                        showSwal(val, `@Url.Action("PostCreatPartialToDB", "GamesManagement", new { area = "Administrator" })`);
                                    }

                                }).catch(error => {
                                    alert(error);
                                }).finally(() => {
                                    // 异步操作完成后启用按钮
                                    $(this).prop('disabled', false);
                                });
                            })
                            //圖片更新抓不到
                            .fail(function (jqxhr, settings, exception) {
                                console.error("加载失败");
                                fetch(`@Url.Action("GetCreatPartialView", "GamesManagement", new { area = "Administrator" })`, {
                                    method: "GET"
                                }).then(response => {
                                    // 確保請求是否成功
                                    if (!response.ok) {
                                        throw new Error('Network response was not ok');
                                    }
                                    // 解析 html
                                    return response.text();
                                }).then(val => {
                                    if (val) {
                                        showSwal(val, `@Url.Action("PostCreatPartialToDB", "GamesManagement", new { area = "Administrator" })`);
                                    }

                                }).catch(error => {
                                    alert(error);
                                }).finally(() => {
                                    // 异步操作完成后启用按钮
                                    $(this).prop('disabled', false);
                                });
                            });
                    }
                },
                {
                    text: '更新全部價格',
                    // 按鈕點擊事件
                    action: function (e, dt, node, config) {
                        event.preventDefault();
                        event.stopPropagation();
                        fetch(`@Url.Action("GetGamePriceDataToDB", "GamesManagement", new { area = "Administrator" })`, {
                            method: "GET"
                        }).then(result => {
                            // 此時 result 是一個請求結果的物件
                            // 注意傳回值型態，字串用 text()，JSON 用 json()
                            if (result.ok) {
                                return result.text();
                            }
                        }).then(data => {
                            console.log(data);
                        }).catch(error => {
                            console.log(error);
                        });
                    }
                },
                {
                    text: '更新目前在線人數',
                    // 按鈕點擊事件
                    action: function (e, dt, node, config) {
                        event.preventDefault();
                        event.stopPropagation();
                        fetch(`@Url.Action("GetOnlineUsersDataToDB", "GamesManagement", new { area = "Administrator" })`, {
                            method: "GET"
                        }).then(result => {
                            // 此時 result 是一個請求結果的物件
                            // 注意傳回值型態，字串用 text()，JSON 用 json()
                            if (result.ok) {
                                return result.text();
                            }
                        }).then(data => {
                            console.log(data);
                        }).catch(error => {
                            console.log(error);
                        });
                    }
                },
                {
                    text: '更新目前所有評論',
                    // 按鈕點擊事件
                    action: function (e, dt, node, config) {
                        event.preventDefault();
                        event.stopPropagation();
                        fetch(`@Url.Action("GetNumberOfCommentsDataToDB", "GamesManagement", new { area = "Administrator" })`, {
                            method: "GET"
                        }).then(result => {
                            // 此時 result 是一個請求結果的物件
                            // 注意傳回值型態，字串用 text()，JSON 用 json()
                            if (result.ok) {
                                return result.text();
                            }
                        }).then(data => {
                            console.log(data);
                        }).catch(error => {
                            console.log(error);
                        });
                    }
                },
                {
                    text: '抓取最低配備',
                    // 按鈕點擊事件
                    action: function (e, dt, node, config) {
                        event.preventDefault();
                        event.stopPropagation();
                        fetch(`@Url.Action("GetDataToDB", "GamesManagement", new { area = "Administrator" })?isMinimumRequirement=true`, {
                            method: "GET"
                        }).then(result => {
                            // 此時 result 是一個請求結果的物件
                            // 注意傳回值型態，字串用 text()，JSON 用 json()
                            if (result.ok) {
                                return result.text();
                            }
                        }).then(data => {
                            console.log(data);
                        }).catch(error => {
                            console.log(error);
                        });
                    }
                },
                {
                    text: '抓取最高配備',
                    // 按鈕點擊事件
                    action: function (e, dt, node, config) {
                        event.preventDefault();
                        event.stopPropagation();
                        fetch(`@Url.Action("GetDataToDB", "GamesManagement", new { area = "Administrator" })?isMinimumRequirement=false`, {
                            method: "GET"
                        }).then(result => {
                            // 此時 result 是一個請求結果的物件
                            // 注意傳回值型態，字串用 text()，JSON 用 json()
                            if (result.ok) {
                                return result.text();
                            }
                        }).then(data => {
                            console.log(data);
                        }).catch(error => {
                            console.log(error);
                        });
                    }
                }

            ]
        },
    },
});
});

onBeforeRouteLeave(() => {
  // 銷毀 DataTable
  dataTable.clear().draw()
  dataTable.destroy()
  dataTable = null
  // 事件監聽器移除
  $(document).off('click', '.Enter-btn')
  $(document).off('click', '.Cancel-btn')
  $(document).off('click', '.Edit-btn')
})
</script>

<style>
/* DataTables */
@import 'datatables.net-dt/css/dataTables.datatables.min.css';
@import 'datatables.net-fixedheader-dt/css/fixedHeader.dataTables.min.css';
@import 'datatables.net-rowgroup-dt/css/rowGroup.dataTables.min.css';
@import 'datatables.net-buttons-dt/css/buttons.dataTables.min.css';
@import 'datatables.net-responsive-dt/css/responsive.dataTables.min.css';
</style>
