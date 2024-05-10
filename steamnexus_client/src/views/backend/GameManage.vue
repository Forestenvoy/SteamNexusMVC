<template>
  <div style="margin-left: 20px;">
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

import { onBeforeRouteLeave } from 'vue-router'
 
import {dataTableConfig} from'@/components/backend/Game/dataTableConfig.js'
import {GetGamePriceDataToDB,GetOnlineUsersDataToDB,GetNumberOfCommentsDataToDB,GetMinDataToDB,GetRecDataToDB} from'@/components/backend/Game/topBtnFetch.js'
import Swal from 'sweetalert2'

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

//將前端內容傳至後端(新增.編輯)
async function showSwal(val,urlValue) {
    await Swal.fire({
        html: `${val}`,
        focusConfirm: false, //設定初始聚焦為第一個input元素 而不是"確認"按鈕
        confirmButtonText: "確認", //按鈕文字
        preConfirm: () => {   //在按下確認後所發生的事情
            event.preventDefault();
            var num = $("#PostEditPartialToDB").prop("name") //我把GameId藏在這裡
            var token = $('input[name="__Antiforgery__SteamNexus"]').val(); //擷取防偽標籤的值
            var action = ""
            if (urlValue.includes("PostCreatPartialToDB")) {
                action = "新增"
            }else{
                action = "編輯"
            }
            $("#systemLoading").show();
            console.log(urlValue)
            $.ajax({
                type: "POST",
                data: {
                    GameId: num,
                    AppId: $("#AppId").val(),
                    Name: $("#Name").val(),
                    OriginalPrice: $("#OriginalPrice").val(),
                    AgeRating: $("#AgeRating").val(),
                    ReleaseDate: $("#ReleaseDate").val(),
                    Publisher: $("#Publisher").val(),
                    Description: $("#Description").val(),
                    ImagePath: $("#ImagePath").val(),
                    VideoPath: $("#VideoPath").val(),
                    __Antiforgery__SteamNexus: token  //防偽標籤傳送點
                },
                url: urlValue
            }).done(data => {
                //若資料驗證有錯server會回傳editview 若正確他會回傳indexview 用if來辨識他是否為indexview
                if (data.includes("<h1>全部遊戲")) {
                    console.log("已完成");
                    $("#System").html(data);
                    if (action == "新增") {
                        Swal.fire({
                            title: "新增成功",
                            icon: "success",
                            confirmButtonText: "確認",
                        });
                    } else {
                        Swal.fire({
                            title: "編輯成功",
                            icon: "success",
                            confirmButtonText: "確認",
                        });
                    }
                    
                } else {
                    console.log("內容有錯");
                    showSwal(data, urlValue);
                }
            })
                .fail(data => {
                    console.log("失敗");
                    $("#systemLoading").hide();
                    $("#System").html(data);
                });
        }
    });
    // if (formValues) {
    //     Swal.fire(JSON.stringify(formValues));
    // }
}

onMounted(() => {
  getdatatableData();
  dataTable = new DataTable('#GameManageTable', {
    ...dataTableConfig,
    layout: {
        topMiddle: {
            buttons: [
                {
                    text: '新增遊戲',
                    // 按鈕點擊事件
                    action: function () {
                        showSwal('<h1>全部遊戲<img src="/public/img/loading-block-white.gif" style="margin:20px;width:30px;height:30px;display:none" id="GameIndexLoading" /></h1>',`${apiUrl}/api/GamesManagement/PostCreatPartialToDB`);
                    }
                },
                {
                    text: '更新全部價格',
                    // 按鈕點擊事件
                    action: function () {
                      GetGamePriceDataToDB();
                    }
                },
                {
                    text: '更新目前在線人數',
                    // 按鈕點擊事件
                    action: function () {
                      GetOnlineUsersDataToDB();
                        // fetch(`${apiUrl}/api/GamesManagement/GetOnlineUsersDataToDB`, {
                        //     method: "GET"
                        // }).then(result => {
                        //     // 此時 result 是一個請求結果的物件
                        //     // 注意傳回值型態，字串用 text()，JSON 用 json()
                        //     if (result.ok) {
                        //         return result.text();
                        //     }
                        // }).then(data => {
                        //     console.log(data);
                        // }).catch(error => {
                        //     console.log(error);
                        // });
                    }
                },
                {
                    text: '更新目前所有評論',
                    // 按鈕點擊事件
                    action: function () {
                      GetNumberOfCommentsDataToDB();
                        // fetch(`${apiUrl}/api/GamesManagement/GetNumberOfCommentsDataToDB`, {
                        //     method: "GET"
                        // }).then(result => {
                        //     // 此時 result 是一個請求結果的物件
                        //     // 注意傳回值型態，字串用 text()，JSON 用 json()
                        //     if (result.ok) {
                        //         return result.text();
                        //     }
                        // }).then(data => {
                        //     console.log(data);
                        // }).catch(error => {
                        //     console.log(error);
                        // });
                    }
                },
                {
                    text: '抓取最低配備',
                    // 按鈕點擊事件
                    action: function (e, dt, node, config) {
                      GetMinDataToDB();
                        // fetch(`${apiUrl}/api/GamesManagement/GetDataToDB?isMinimumRequirement=true`, {
                        //     method: "GET"
                        // }).then(result => {
                        //     // 此時 result 是一個請求結果的物件
                        //     // 注意傳回值型態，字串用 text()，JSON 用 json()
                        //     if (result.ok) {
                        //         return result.text();
                        //     }
                        // }).then(data => {
                        //     console.log(data);
                        // }).catch(error => {
                        //     console.log(error);
                        // });
                    }
                },
                {
                    text: '抓取最高配備',
                    // 按鈕點擊事件
                    action: function () {
                        GetRecDataToDB();
                        // fetch(`${apiUrl}/api/GamesManagement/GetDataToDB?isMinimumRequirement=false`, {
                        //     method: "GET"
                        // }).then(result => {
                        //     // 此時 result 是一個請求結果的物件
                        //     // 注意傳回值型態，字串用 text()，JSON 用 json()
                        //     if (result.ok) {
                        //         return result.text();
                        //     }
                        // }).then(data => {
                        //     console.log(data);
                        // }).catch(error => {
                        //     console.log(error);
                        // });
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
