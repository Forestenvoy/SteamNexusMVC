<template>
  <div class="row">
    <div class="col-12 col-md-6 order-md-1 d-flex justify-content-center justify-content-md-start">
      <h2 style="margin-top: 8px">產品管理系統</h2>
    </div>
    <div
      class="col-12 col-md-6 order-md-2 d-flex justify-content-center justify-content-md-end align-items-center"
    >
      <select
        class="form-select mb-0"
        style="width: 250px; height: 40px; margin-bottom: 8px"
        @change="selectHardware()"
        v-model="selectedItem"
      >
        <option value="0" disabled selected hidden>---- 請選擇硬體 ----</option>
        <option v-for="item in HardwareSelect" :key="item.id" :value="item.id">
          {{ item.name }}
        </option>
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
  <!-- 吐司容器 -->
  <div
    class="position-fixed top-0 start-50 translate-middle-x p-3"
    style="z-index: 9999"
    id="ToastContainer"
  ></div>
</template>

<script setup>
// 核心模組 import
import $ from 'jquery'
import DataTable from 'datatables.net-dt'
import 'datatables.net-fixedheader-dt'
import 'datatables.net-rowgroup-dt'
import 'datatables.net-buttons-dt'
import 'datatables.net-responsive-dt'

import '@/assets/css/dataTables.datatables.min.css'
import '@/assets/css/fixedHeader.dataTables.min.css'
import '@/assets/css/rowGroup.dataTables.min.css'
import '@/assets/css/buttons.dataTables.min.css'
import '@/assets/css/responsive.dataTables.min.css'

import { ref, onMounted } from 'vue'
import { onBeforeRouteLeave } from 'vue-router'

import { dataTableConfig } from '@/components/backend/hardware/dataTableConfig.js'
import {
  getHardwareSelect,
  ToastInitialization,
  ToastProgressDisappear,
  HardwareSelect
} from '@/components/backend/hardware/productManageMethods.js'

// 使用 Pinia
import { useScraperStore } from '@/stores/scraper.js'
// 利用 store 去訪問狀態 ✨
const store = useScraperStore()

// 身份驗證
import { useIdentityStore } from '@/stores/identity.js'
const authStore = useIdentityStore()

// 從環境變數取得 API BASE URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

// 宣告 硬體選擇 初始值
const selectedItem = ref('0')

// 初始化 DataTables
let dataTable = null

// 重新整理 判斷
let isRefresh = false

// DataTable Config 加入按鈕
const myDataTablesConfig = {
  ...dataTableConfig,
  // 按鈕建立
  layout: {
    topMiddle: {
      buttons: [
        {
          text: '重新整理',
          // 按鈕點擊事件
          action: function () {
            isRefresh = true
            // 重新整理
            getDataTableData()
          }
        },
        {
          text: '單一零件更新',
          // 按鈕點擊事件
          action: function () {
            // 單一零件更新
            UpdateOneHardware()
          }
        },
        {
          text: '全零件更新',
          // 按鈕點擊事件
          action: function () {
            // 全零件更新
            UpdateAllHardware()
          }
        }
      ]
    }
  }
}

function getDataTableData() {
  // 如果沒選擇硬體 => 中斷事件
  if (selectedItem.value == '0') {
    return
  }
  let ToastContainer = null
  let toastId = 0
  let toast = null

  // 如果是 -重新整理-
  if (isRefresh) {
    // 取得吐司容器元素
    ToastContainer = document.querySelector('#ToastContainer')
    // 流水號生成
    toastId = Math.floor(Math.random() * 1000)
    // 吐司元素生成
    toast = ToastInitialization(toastId)
  }
  const hardwareId = selectedItem.value
  // 發送非同步GET請求
  fetch(`${apiUrl}/api/HardwareManage/GetProductData?Type=${hardwareId}`, {
    method: 'GET',
    headers: {
      Authorization: `Bearer ${authStore.getToken}`
    }
  })
    .then((result) => {
      // 此時 result 是一個請求結果的物件
      if (isRefresh) {
        // 發送吐司
        ToastContainer.insertAdjacentHTML(`afterbegin`, toast)
        // 顯示吐司
        $(`#${toastId}_Toast`).show()
        // 隱藏 載入狀態
        $(`#${toastId}_Toast_Status`).hide()
      }
      // 注意傳回值型態，字串用 text()，JSON 用 json()
      if (result.ok) {
        return result.json()
      }
    })
    .then((data) => {
      // 此時 data 為上一個 then 回傳的資料
      // 清空表格
      dataTable.clear().draw()
      // 添加新的資料
      dataTable.rows.add(data).draw()
      if (isRefresh) {
        // 顯示 吐司成功狀態
        $(`#${toastId}_ToastIcon_success`).addClass('animate__zoomIn').removeClass('d-none')
        $(`#${toastId}_ToastText`).text('重新整理成功')
        // 顯示進度條
        $(`#${toastId}_ToastProgress`).show()
        // 進度條消失
        ToastProgressDisappear(toastId)
      }
      isRefresh = false
    })
    .catch((error) => {
      alert(error)
      if (isRefresh) {
        // 顯示 吐司失敗狀態
        $(`#${toastId}_ToastIcon_fail`).addClass('animate__zoomIn').removeClass('d-none')
        $(`#${toastId}_ToastText`).text('重新整理失敗，請稍後再試')
        // 更改顏色
        $(`#${toastId}_ToastProgress_rect`).css('fill', '#FF0000')
        // 顯示進度條
        $(`#${toastId}_ToastProgress`).show()
        // 進度條消失
        ToastProgressDisappear(toastId)
      }
      isRefresh = false
    })
}

// dataTable 資料載入
function selectHardware() {
  getDataTableData()
}

// 單一零件更新
function UpdateOneHardware() {
  // 取得硬體 ID
  const hardwareId = selectedItem.value

  // 如果沒選擇硬體 => 中斷事件
  if (hardwareId == '0') {
    return
  }

  store.setType('One')
  store.setHardwareId(hardwareId)
  store.setState(true)
}

// 所有零件更新
function UpdateAllHardware() {
  store.setType('All')
  store.setState(true)
}

onMounted(() => {
  // 取得硬體選單 by AJAX
  getHardwareSelect()
  // 初始化 DataTables
  dataTable = new DataTable('#HardwareManageTable', myDataTablesConfig)
  // 事件監聽器
  // 資料編輯 ~ 當任何具有 Edit-btn 類別元素被點擊時觸發
  $(document).on('click', '.Edit-btn', function () {
    // 被隱藏的按鈕不能觸發
    if ($(this).css('display') !== 'none') {
      // 取得 ID
      let productId = Number($(this).attr('class').slice(0, 5))
      // 取資料 ~ 只取看的見的元素
      let wattVal = Number($(`.${productId}_watt:visible`).val())
      let recVal = Number($(`.${productId}_recommend:visible`).val())
      // 確認按鈕
      let enterEle = `<button class="${productId}_enter btn Enter-btn d-flex justify-content-center align-items-center" style="width:30px;height:30px;"><i class="fa-solid fa-check"></i></button>`
      // 取消按鈕
      let cancelEle = `<button class="${productId}_reset btn Cancel-btn d-flex justify-content-center align-items-center" style="width:30px;height:30px;" data-origin="${wattVal},${recVal}"><i class="fa-solid fa-xmark"></i></button>`
      $(`#${productId}_div`).html(`${enterEle}&nbsp;&nbsp;&nbsp;${cancelEle}`)
      // 編輯模式 ~ 開啟 td 編輯功能
      $(`.${productId}_watt`).prop('disabled', false)
      $(`.${productId}_watt`).removeClass('defaultCellType')
      $(`.${productId}_recommend`).prop('disabled', false)
      $(`.${productId}_recommend`).removeClass('defaultCellType')
    }
  })

  // 資料變更確認 ~ 當任何具有 Enter-btn 類別元素被點擊時觸發
  $(document).on('click', '.Enter-btn', function () {
    // 被隱藏的按鈕不能觸發
    if ($(this).css('display') !== 'none') {
      // 取得 ID
      let productId = Number($(this).attr('class').slice(0, 5))

      // 取資料 ~ 只取看的見的元素
      let wattVal = Number($(`.${productId}_watt:visible`).val())
      let recVal = Number($(`.${productId}_recommend:visible`).val())

      // 取得吐司容器元素
      let ToastContainer = document.querySelector('#ToastContainer')
      // 資料包裝
      var data = {
        ProductId: productId,
        Wattage: wattVal,
        Recommend: recVal
      }
      // 流水號生成
      let toastId = Math.floor(Math.random() * 1000)
      // 吐司元素生成
      let toast = ToastInitialization(toastId)
      // 發送吐司
      ToastContainer.insertAdjacentHTML(`afterbegin`, toast)
      // 狀態值
      let PromiseStatus = false
      $(`#${toastId}_Toast`).show()
      // 發送非同步POST請求 ==> 資料庫資料變更
      fetch(`${apiUrl}/api/HardwareManage/ProductDataUpdate`, {
        method: 'POST',
        body: JSON.stringify(data),
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${authStore.getToken}`
        }
      })
        .then((result) => {
          // 隱藏吐司 loading
          $(`#${toastId}_Toast_Status`).addClass('animate__zoomOut')
          $(`#${toastId}_Toast_Status`).hide()
          // 如過 HTTP 響應的狀態馬碼在 200 到 299 的範圍內 ==> .ok 會回傳 true
          if (result.ok) {
            // 此時 result 是一個請求結果的物件 => 注意傳回值型態，字串用 text()，JSON 用 json()
            PromiseStatus = true
            return result.text()
          }
          return result.text()
        })
        .then((data) => {
          // 此時 data 為上一個 then 回傳的資料
          if (PromiseStatus) {
            // 顯示 吐司成功狀態
            $(`#${toastId}_ToastIcon_success`).addClass('animate__zoomIn').removeClass('d-none')
            $(`#${toastId}_ToastText`).text(data)
            // 顯示進度條
            $(`#${toastId}_ToastProgress`).show()
            // 進度條消失
            ToastProgressDisappear(toastId)
            // 恢復預設模式 ~ 關閉 td 編輯功能
            $(`.${productId}_watt`).attr('disabled', true)
            $(`.${productId}_watt`).addClass('defaultCellType')
            $(`.${productId}_recommend`).attr('disabled', true)
            $(`.${productId}_recommend`).addClass('defaultCellType')
            // 更換成編輯按鈕
            let editEle = `<button class="${productId}_edit btn Edit-btn d-flex justify-content-center align-items-center" style="width:30px;height:30px;" data-bs-toggle="popover" data-bs-content="nothing"><i class="fa-solid fa-pen-to-square"></i></button>`
            $(`#${productId}_div`).html(editEle)
          } else {
            // 顯示 吐司失敗狀態
            $(`#${toastId}_ToastIcon_fail`).addClass('animate__zoomIn').removeClass('d-none')
            $(`#${toastId}_ToastText`).text(data)
            // 進度條消失
            $(`#${toastId}_ToastProgress`).show()
            // 更改顏色
            $(`#${toastId}_ToastProgress_rect`).css('fill', '#FF0000')
            // 進度條消失
            ToastProgressDisappear(toastId)
          }
        })
        .catch((error) => {
          // 顯示 吐司失敗狀態
          $(`#${toastId}_ToastIcon_fail`).addClass('animate__zoomIn').removeClass('d-none')
          $(`#${toastId}_ToastText`).text(error.message)
        })
    }
  })

  // 資料變更取消 ~ 當任何具有 Cancel-btn 類別元素被點擊時觸發
  $(document).on('click', '.Cancel-btn', function () {
    // 被隱藏的按鈕不能觸發
    if ($(this).css('display') !== 'none') {
      // 取得 ID
      let productId = Number($(this).attr('class').slice(0, 5))
      // 取得原始資料
      let origin = $(this).attr('data-origin').split(',')
      let wattVal = Number(origin[0])
      let recVal = Number(origin[1])
      // 恢復預設模式 ~ 關閉 td 編輯功能
      $(`.${productId}_watt`).attr('disabled', true)
      $(`.${productId}_watt`).val(wattVal)
      $(`.${productId}_watt`).addClass('defaultCellType')
      $(`.${productId}_recommend`).attr('disabled', true)
      $(`.${productId}_recommend`).val(recVal)
      $(`.${productId}_recommend`).addClass('defaultCellType')
      // 更換成編輯按鈕
      let editEle = `<button class="${productId}_edit btn Edit-btn d-flex justify-content-center align-items-center" style="width:30px;height:30px;" data-bs-toggle="popover" data-bs-content="nothing"><i class="fa-solid fa-pen-to-square"></i></button>`
      $(`#${productId}_div`).html(editEle)
    }
  })
})

// 路由離開時觸發
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
/* datatables 頂部布局調整 */
#HardwareManageTable_wrapper .dt-end .dt-search {
  text-align: center !important;
}

#HardwareManageTable_wrapper .dt-middle {
  /* text-align: end !important; */
  display: flex;
  justify-content: flex-end;
}

#HardwareManageTable_wrapper .dt-button {
  margin-bottom: 0;
}

/* 編輯按鈕 */
html[data-coreui-theme='dark'] #HardwareManageTable_wrapper .Edit-btn {
  background-color: #007fff;
}

html[data-coreui-theme='dark'] #HardwareManageTable_wrapper .Edit-btn svg {
  color: white;
}

html[data-coreui-theme='dark'] #HardwareManageTable_wrapper .Edit-btn:hover {
  background-color: white;
}

html[data-coreui-theme='dark'] #HardwareManageTable_wrapper .Edit-btn:hover svg {
  color: #007fff;
}

html[data-coreui-theme='light'] #HardwareManageTable_wrapper .Edit-btn {
  background-color: white;
}

html[data-coreui-theme='light'] #HardwareManageTable_wrapper .Edit-btn svg {
  color: #007fff;
}

html[data-coreui-theme='light'] #HardwareManageTable_wrapper .Edit-btn:hover {
  background-color: #007fff;
}

html[data-coreui-theme='light'] #HardwareManageTable_wrapper .Edit-btn:hover svg {
  color: white;
}

/* 確認按鈕 */
html[data-coreui-theme='dark'] #HardwareManageTable_wrapper .Enter-btn {
  background-color: #00ff00;
}

html[data-coreui-theme='dark'] #HardwareManageTable_wrapper .Enter-btn svg {
  color: white;
}

html[data-coreui-theme='dark'] #HardwareManageTable_wrapper .Enter-btn:hover {
  background-color: white;
}

html[data-coreui-theme='dark'] #HardwareManageTable_wrapper .Enter-btn:hover svg {
  color: #00ff00;
}

html[data-coreui-theme='light'] #HardwareManageTable_wrapper .Enter-btn {
  background-color: white;
}

html[data-coreui-theme='light'] #HardwareManageTable_wrapper .Enter-btn svg {
  color: #00ff00;
}

html[data-coreui-theme='light'] #HardwareManageTable_wrapper .Enter-btn:hover {
  background-color: #00ff00;
}

html[data-coreui-theme='light'] #HardwareManageTable_wrapper .Enter-btn:hover svg {
  color: white;
}

/* 還原按鈕 */
html[data-coreui-theme='dark'] #HardwareManageTable_wrapper .Cancel-btn {
  background-color: #ff0000;
}

html[data-coreui-theme='dark'] #HardwareManageTable_wrapper .Cancel-btn svg {
  color: white;
}

html[data-coreui-theme='dark'] #HardwareManageTable_wrapper .Cancel-btn:hover {
  background-color: white;
}

html[data-coreui-theme='dark'] #HardwareManageTable_wrapper .Cancel-btn:hover svg {
  color: #ff0000;
}

html[data-coreui-theme='light'] #HardwareManageTable_wrapper .Cancel-btn {
  background-color: white;
}

html[data-coreui-theme='light'] #HardwareManageTable_wrapper .Cancel-btn svg {
  color: #ff0000;
}

html[data-coreui-theme='light'] #HardwareManageTable_wrapper .Cancel-btn:hover {
  background-color: #ff0000;
}

html[data-coreui-theme='light'] #HardwareManageTable_wrapper .Cancel-btn:hover svg {
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
html[data-coreui-theme='dark'] .defaultToast {
  height: 47px;
  width: 250px;
  background-color: black !important;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.5);
}

html[data-coreui-theme='light'] .defaultToast {
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

html[data-coreui-theme='dark'] .ToastIcon-success svg {
  color: black;
  width: 1rem;
  height: 1rem;
}

html[data-coreui-theme='light'] .ToastIcon-success svg {
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

html[data-coreui-theme='dark'] .ToastIcon-fail svg {
  color: black;
  width: 1rem;
  height: 1rem;
}

html[data-coreui-theme='light'] .ToastIcon-fail svg {
  color: white;
  width: 1rem;
  height: 1rem;
}

/* 表格的佈局採用固定佈局算法 */
body {
  table-layout: fixed;
}

/* 欄位預設樣式 */
html[data-coreui-theme='dark'] #HardwareManageTable_wrapper .defaultCellType {
  border: none; /* 取消邊框 */
  background-color: transparent; /* 背景透明 */
  appearance: none; /* 去除默認樣式 */
  color: #c2c2d9; /* 文字顏色 */
}

html[data-coreui-theme='light'] #HardwareManageTable_wrapper .defaultCellType {
  border: none; /* 取消邊框 */
  background-color: transparent; /* 背景透明 */
  appearance: none; /* 去除默認樣式 */
  color: #607080; /* 文字顏色 */
}
</style>
