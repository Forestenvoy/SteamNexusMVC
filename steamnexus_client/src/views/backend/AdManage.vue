<template>
  <div class="row">
    <div class="col-12 col-md-6 order-md-1 d-flex justify-content-center justify-content-md-start">
      <h2 id="SystemName" style="margin-top: 8px;">廣告管理系統</h2>
    </div>
    <div class="col-12 col-md-6 order-md-2 d-flex justify-content-center justify-content-md-end" id="SystemMenu">
    </div>
  </div>
  <section class="section">
    <table id="AdvertiseManageTable" class="display" style="width:100%">
      <thead>
        <tr>
          <th>ID</th>
          <th>標題</th>
          <th>Url</th>
          <th>圖片</th>
          <th>說明</th>
          <th>狀態</th>
        </tr>
      </thead>
    </table>
  </section>
</template>
<script setup>
// 核心模組 import
import $ from 'jquery'
import DataTable from 'datatables.net-dt'
import 'datatables.net-fixedheader-dt'
import 'datatables.net-buttons-dt'
import 'datatables.net-responsive-dt'

import { onMounted } from 'vue'
import { onBeforeRouteLeave } from 'vue-router'
var dataTable = null

// 從環境變數取得 API BASE URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

function fetchDatatable() {
  fetch(`${apiUrl}/api/Advertisement/AdvertiseData`, {
    method: 'GET',
  })
    .then(response => {
      if (!response.ok) {
        throw new Error('Network response was not ok');
      }
      return response.json();
    })
    .then(jsonData => {
      // 清空表格
      dataTable.clear().draw();
      // 添加新的資料
      dataTable.rows.add(jsonData).draw();
    })
    .catch(error => {
      console.error('Fetch error:', error);
    });
};
onMounted(() => {
  dataTable = new DataTable('#AdvertiseManageTable', {
    columns: [
      { "data": "advertisementId", "width": "5%" },
      { "data": "title", "width": "10%" },
      { "data": "url", "width": "25%" },
      {
        "data": "imagePath", "width": "15%",
        "className": "text-center",
        "render": function (data) {
          return `<img src="${data}" alt="Image" style="width:80%" />`;
        }, responsivePriority: 1
      },
      { "data": "discription", "width": "10%" },
      {
        "data": "isShow",
        "width": "20%",
        "className": "text-center",
        "render": function (data, type, row) {
          // data: 欄位的資料值
          // type: render 的呼叫類型，例如 'display', 'filter', 'sort', 'type'
          // row: 包含整列資料的物件
          let AdId = row.advertisementId;
          const Show = data ? 'checked' : '';
          const UnShow = data ? '' : 'checked';
          let showEle = `<input type="radio" class="radio-isShow" name = "${AdId}" value = "true" data-AdId="${AdId}" id = "${AdId}Show" ${Show}><label for= "${AdId}Show" style="margin-right: 10px">上架</label>`;
          let unShowEle = `<input type="radio" class="radio-isShow" name="${AdId}" value="false" data-AdId="${AdId}" id="${AdId}UnShow" ${UnShow}><label for="${AdId}UnShow">下架</label>`;
          return `${showEle}${unShowEle}`;
        }
      },
      {
        "data": null,
        "orderable": false,
        "width": "15%",
        "className": "text-center",
        // 按鈕 自定義
        "render": function (data, type, row) {
          // 取得 productId
          let title = row.title;
          let AdId = row.advertisementId;
          let url = row.url;
          let disc = row.discription;
          let image = row.imagePath;
          // 編輯按鈕
          let editEle = '<button class="btn btn-primary" data-AdId="' + AdId + '"  data-title="' + title + '"  data-url="' + url + '"  data-disc="' + disc + '"  data-image="' + image + '" id="edit_button"><i class="bi bi-pencil-square"></i></button>';
          // 刪除按鈕
          let deletEle = '<button class="btn btn-danger" data-AdId="' + AdId + '"  data-title="' + title + '"  data-url="' + url + '"  data-disc="' + disc + '"  data-image="' + image + '" id="delete_button"><i class="bi bi-trash3"></i></button>';
          if (type === 'display') {
            return `${editEle}${deletEle}`;
          }
          return data;
        }, responsivePriority: 1
      }
    ],
    fixedHeader: {
      // 固定 header
      header: true
    },
    // 響應式設計
    responsive: true,
    // 語言設定
    language: {
      url: '//cdn.datatables.net/plug-ins/2.0.3/i18n/zh-HANT.json',
    },
    // 預設排序 ~ 根據第一個欄位 升冪排序
    order: [[0, 'asc']],
    // 自動寬度 開啟
    autoWidth: true,
    layout: {
      topMiddile: {
        //Create按鈕建立
        buttons: [
          {
            text: '新增廣告',
            action: function () {
              // 彈出 Bootstrap modal 表單
              $('#addAdvertisementModal').modal('show');
            }
          }
        ]
      },
    },
  });

  fetchDatatable();

  // 監聽上下架按鈕的 change 事件
  $(document).on('change', '.radio-isShow', function () {
    // alert("change");
    const adId = $(this).data('adid');
    const isShow = $(this).val() === 'true';
    // // alert(`change ${adId} ${isShow}`);
    fetch(`${apiUrl}/api/Advertisement/UpdateIsShow?adId=${adId}&isShow=${isShow}`, {
      method: 'PUT',
    }).then(response => {
      if (response.ok) {
        return response.text();
      }
      else {
        return response.text();
      }
    }).then(result => {
      alert(result); // 處理成功回應的操作
    }).catch(error => {
      alert(error); // 處理錯誤
    });
  });

})

// 路由離開時觸發
onBeforeRouteLeave(() => {
  // 銷毀 DataTable
  dataTable.clear().draw()
  dataTable.destroy()
  dataTable = null
  // 事件監聽器移除
  // $(document).off('click', '.Enter-btn')
  // $(document).off('click', '.Cancel-btn')
  // $(document).off('click', '.Edit-btn')
})
</script>

<style>
/* DataTables */
@import 'datatables.net-dt/css/dataTables.datatables.min.css';
@import 'datatables.net-fixedheader-dt/css/fixedHeader.dataTables.min.css';
@import 'datatables.net-buttons-dt/css/buttons.dataTables.min.css';
@import 'datatables.net-responsive-dt/css/responsive.dataTables.min.css';
</style>
<style>
/***** datatables 自訂樣式 *****/

.dt-end .dt-search {
  text-align: center !important;
  /* 將搜索框的文字內容設置為居中對齊 */
}

.dt-middle {
  text-align: end !important;
}

body {
  table-layout: fixed;
  /* 將表格的列寬設置為固定值，以確保列寬不會根據內容而自動調整 */
}
</style>
