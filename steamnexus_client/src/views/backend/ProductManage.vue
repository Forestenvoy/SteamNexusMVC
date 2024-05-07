<template>
  <div class="row">
    <div class="col-12 col-md-6 order-md-1 d-flex justify-content-center justify-content-md-start">
      <h2 id="SystemName" style="margin-top: 8px">產品管理系統</h2>
    </div>
    <div
      class="col-12 col-md-6 order-md-2 d-flex justify-content-center justify-content-md-end"
      id="SystemMenu"
    >
      <select
        class="form-select"
        style="width: 250px; height: 40px; margin-bottom: 8px"
        id="HardSelect"
      >
        <option value="" disabled selected hidden>---- 請選擇硬體 ----</option>
      </select>
    </div>
  </div>
  <section class="section">
    <table id="HardwareManageTable" class="display" style="width: 100%">
      <thead id="HardwareThead">
        <tr>
          <th data-priority="1">ID</th>
          <th>產品名稱</th>
          <th>規格</th>
          <th>種類</th>
          <th>價格</th>
          <th>瓦數</th>
          <th>推薦</th>
          <th data-priority="2"></th>
        </tr>
      </thead>
    </table>
  </section>
</template>

<script setup>
import $ from 'jquery'
// Datatables
import DataTable from 'datatables.net-dt'
import 'datatables.net-fixedheader-dt'
import 'datatables.net-rowgroup-dt'
import 'datatables.net-buttons-dt'
import 'datatables.net-responsive-dt'

import { onMounted } from 'vue'

// 從環境變數取得 API BASE URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

console.log('API URL:', apiUrl)

// 宣告硬體選單陣列
// const HardwareSelect = ref([])

// function getHardwareSelect() {

// };

onMounted(() => {
  // 取得硬體選單 by AJAX

  // 初始化 Datatables
  new DataTable('#HardwareManageTable', {
    // column 定義
    columns: [
      { data: 'productId', width: '5%', className: 'text-center' },
      { data: 'productName', width: '34%' },
      { data: 'specification', width: '29%' },
      { data: 'componentClassificationName', visible: false },
      { data: 'price', width: '8%', className: 'text-center' },
      {
        data: 'wattage',
        width: '8%',
        className: 'text-center',
        // 瓦數 ~ 文字輸入方塊
        render: function (data, type, row) {
          // 此處 type 為 'sort' 表示 DataTables 正在進行排序
          if (type === 'sort') {
            // 返回元素的值作為排序依據
            return data
          }
          // 取得 productId
          let productId = row.productId
          // input 欄位 ~ 可編輯
          let inputEle = `<input type="text" class="${productId}_watt defaultcellType" value="${data}" style="width: 50px; text-align: center;"  disabled>`
          return inputEle
        },
        // 將此列的數據類型設置為數字 ~ 排序才會正常運作
        type: 'num'
      },
      {
        data: 'recommend',
        width: '8%',
        className: 'text-center',
        // 推薦 ~ 下拉式選單
        render: function (data, type, row) {
          // 此處 type 為 'sort' 表示 DataTables 正在進行排序
          if (type === 'sort') {
            // 返回元素的值作為排序依據
            return data
          }
          // 取得 productId
          let productId = row.productId
          const isRec1 = data == 0 ? 'selected' : ''
          const isRec2 = data == 1 ? 'selected' : ''
          const isRec3 = data == 2 ? 'selected' : ''
          const isRec4 = data == 3 ? 'selected' : ''
          const ele1 = `<option value = "0" ${isRec1}>無</option>`
          const ele2 = `<option value = "1" ${isRec2}>基本</option>`
          const ele3 = `<option value = "2" ${isRec3}>優良</option>`
          const ele4 = `<option value = "3" ${isRec4}>完美</option>`
          // select 欄位 ~ 可編輯
          let selectEle = `<select class="${productId}_recommend defaultcellType" disabled>${ele1}${ele2}${ele3}${ele4}</select>`
          return selectEle
        },
        // 將此列的數據類型設置為數字 ~ 排序才會正常運作
        type: 'num'
      },
      {
        data: null,
        orderable: false,
        width: '8%',
        // 按鈕 自定義
        render: function (data, type, row) {
          // 取得 productId
          let productId = row.productId
          // 編輯按鈕
          let editEle = `<button class="${productId}_edit btn Edit-btn d-flex justify-content-center align-items-center" style="width:30px;height:30px;" data-slefColumn-isShow="false"><i class="fa-solid fa-pen-to-square"></i></button>`
          let div = `<div class="d-flex justify-content-between" id="${productId}_div">${editEle}</div>`
          if (type === 'display') {
            return `${div}`
          }
          return data
        },
        type: 'html'
      }
    ],
    // 標頭固定
    fixedHeader: {
      // 固定 header
      header: true,
      // 使用導覽欄的高度作為偏移量
      headerOffset: $('#pageNav').outerHeight()
    },
    // 響應式設計
    responsive: true,
    // 語言設定
    language: {
      url: '//cdn.datatables.net/plug-ins/2.0.6/i18n/zh-HANT.json'
    },
    // 預設排序 ~ 根據第一個欄位 升冪排序
    order: [[1, 'asc']],
    // 行分組
    rowGroup: {
      dataSrc: 'componentClassificationName'
    },
    // 資料載入中 gif
    processing: true,
    // 自動寬度 關閉
    autoWidth: true,
    // 資料行樣式
    createdRow: function (row, data) {
      if (data.recommend === 1) {
        row.classList.add('green-row')
      } else if (data.recommend === 2) {
        row.classList.add('blue-row')
      } else if (data.recommend === 3) {
        row.classList.add('red-row')
      }
      // 加上 ID
      $(row).attr('id', data.productId + '_row')
      // 為特定行加入 class ==> 不會影響 th
      $(row).find('td:eq(6)').addClass('d-flex')
      $(row).find('td:eq(6)').addClass('justify-content-center')
    },
    // 按鈕建立
    layout: {
      topMiddle: {
        buttons: [
          {
            text: '重新整理',
            // 按鈕點擊事件
            action: function () {
              // isrefresh = true
              // 重新整理
              // getdatatableData()
            }
          },
          {
            text: '單一零件更新',
            // 按鈕點擊事件
            action: function () {
              // 單一零件更新
              // UpdateOneHardware()
            }
          },
          {
            text: '全零件更新',
            // 按鈕點擊事件
            action: function () {
              // 全零件更新
              // UpdateAllHardware()
            }
          }
        ]
      }
    }
  })
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

<style>
/* datatables 頂部布局調整 */
#HardwareManageTable_wrapper .dt-end .dt-search {
  text-align: center !important;
}

#HardwareManageTable_wrapper .dt-middle {
  text-align: end !important;
}

/* 編輯按鈕 */
html[data-bs-theme='dark'] #HardwareManageTable_wrapper .Edit-btn {
  background-color: #007fff;
}

html[data-bs-theme='dark'] #HardwareManageTable_wrapper .Edit-btn svg {
  color: white;
}

html[data-bs-theme='dark'] #HardwareManageTable_wrapper .Edit-btn:hover {
  background-color: white;
}

html[data-bs-theme='dark'] #HardwareManageTable_wrapper .Edit-btn:hover svg {
  color: #007fff;
}

html[data-bs-theme='light'] #HardwareManageTable_wrapper .Edit-btn {
  background-color: white;
}

html[data-bs-theme='light'] #HardwareManageTable_wrapper .Edit-btn svg {
  color: #007fff;
}

html[data-bs-theme='light'] #HardwareManageTable_wrapper .Edit-btn:hover {
  background-color: #007fff;
}

html[data-bs-theme='light'] #HardwareManageTable_wrapper .Edit-btn:hover svg {
  color: white;
}

/* 確認按鈕 */
html[data-bs-theme='dark'] #HardwareManageTable_wrapper .Enter-btn {
  background-color: #00ff00;
}

html[data-bs-theme='dark'] #HardwareManageTable_wrapper .Enter-btn svg {
  color: white;
}

html[data-bs-theme='dark'] #HardwareManageTable_wrapper .Enter-btn:hover {
  background-color: white;
}

html[data-bs-theme='dark'] #HardwareManageTable_wrapper .Enter-btn:hover svg {
  color: #00ff00;
}

html[data-bs-theme='light'] #HardwareManageTable_wrapper .Enter-btn {
  background-color: white;
}

html[data-bs-theme='light'] #HardwareManageTable_wrapper .Enter-btn svg {
  color: #00ff00;
}

html[data-bs-theme='light'] #HardwareManageTable_wrapper .Enter-btn:hover {
  background-color: #00ff00;
}

html[data-bs-theme='light'] #HardwareManageTable_wrapper .Enter-btn:hover svg {
  color: white;
}

/* 還原按鈕 */
html[data-bs-theme='dark'] #HardwareManageTable_wrapper .Cancell-btn {
  background-color: #ff0000;
}

html[data-bs-theme='dark'] #HardwareManageTable_wrapper .Cancell-btn svg {
  color: white;
}

html[data-bs-theme='dark'] #HardwareManageTable_wrapper .Cancell-btn:hover {
  background-color: white;
}

html[data-bs-theme='dark'] #HardwareManageTable_wrapper .Cancell-btn:hover svg {
  color: #ff0000;
}

html[data-bs-theme='light'] #HardwareManageTable_wrapper .Cancell-btn {
  background-color: white;
}

html[data-bs-theme='light'] #HardwareManageTable_wrapper .Cancell-btn svg {
  color: #ff0000;
}

html[data-bs-theme='light'] #HardwareManageTable_wrapper .Cancell-btn:hover {
  background-color: #ff0000;
}

html[data-bs-theme='light'] #HardwareManageTable_wrapper .Cancell-btn:hover svg {
  color: white;
}

/* 推薦設定 文字顏色 */
#HardwareManageTable_wrapper .green-row {
  color: green !important;
}

#HardwareManageTable_wrapper .blue-row {
  color: blue !important;
}

#HardwareManageTable_wrapper .red-row {
  color: red !important;
}

/* 吐司預設樣式 */
html[data-bs-theme='dark'] .defaultToast {
  height: 47px;
  width: 250px;
  background-color: black !important;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.5);
}

html[data-bs-theme='light'] .defaultToast {
  height: 47px;
  width: 250px;
  background-color: white !important;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.5);
}

.ToastIcon-success {
  background-color: #00ff00;
  height: 1.2rem;
  width: 1.2rem;
  border-radius: 50%;
}

html[data-bs-theme='dark'] .ToastIcon-success svg {
  color: black;
  width: 1rem;
  height: 1rem;
}

html[data-bs-theme='light'] .ToastIcon-success svg {
  color: white;
  width: 1rem;
  height: 1rem;
}

.ToastIcon-fail {
  background-color: #ff0000;
  height: 1.2rem;
  width: 1.2rem;
  border-radius: 50%;
}

html[data-bs-theme='dark'] .ToastIcon-fail svg {
  color: black;
  width: 1rem;
  height: 1rem;
}

html[data-bs-theme='light'] .ToastIcon-fail svg {
  color: white;
  width: 1rem;
  height: 1rem;
}

/* 表格的佈局採用固定佈局算法 */
body {
  table-layout: fixed;
}

/* 欄位預設樣式 */
html[data-bs-theme='dark'] #HardwareManageTable_wrapper .defaultcellType {
  border: none; /* 取消邊框 */
  background-color: transparent; /* 背景透明 */
  appearance: none; /* 去除默認樣式 */
  color: #c2c2d9; /* 文字顏色 */
}

html[data-bs-theme='light'] #HardwareManageTable_wrapper .defaultcellType {
  border: none; /* 取消邊框 */
  background-color: transparent; /* 背景透明 */
  appearance: none; /* 去除默認樣式 */
  color: #607080; /* 文字顏色 */
}
</style>
