<template>
  <div class="p-3" style="margin-left: 20px">
    <div class="d-flex justify-content-between align-items-center">
      <h1>
        全部遊戲<img
          src="/public/img/loading-block-white.gif"
          style="margin: 20px; width: 30px; height: 30px; display: none"
          id="GameIndexLoading"
        />
      </h1>
      <div class="d-flex">
        <div class="d-flex align-items-center border border-white rounded-pill p-1 mx-1">
          <input id="checkbox" type="checkbox" v-model="btnPrice" />
          <label class="switch" for="checkbox0" @click="btnOnlinePriceClick">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" class="slider">
              <path
                d="M288 32c0-17.7-14.3-32-32-32s-32 14.3-32 32V256c0 17.7 14.3 32 32 32s32-14.3 32-32V32zM143.5 120.6c13.6-11.3 15.4-31.5 4.1-45.1s-31.5-15.4-45.1-4.1C49.7 115.4 16 181.8 16 256c0 132.5 107.5 240 240 240s240-107.5 240-240c0-74.2-33.8-140.6-86.6-184.6c-13.6-11.3-33.8-9.4-45.1 4.1s-9.4 33.8 4.1 45.1c38.9 32.3 63.5 81 63.5 135.4c0 97.2-78.8 176-176 176s-176-78.8-176-176c0-54.4 24.7-103.1 63.5-135.4z"
              ></path>
            </svg>
          </label>
          <span class="mx-2" @click="btnOnlinePriceClick" style="cursor: pointer"
            >{{ btnPriceFont }}更新當前價格</span
          >
        </div>
        <div class="d-flex align-items-center border border-white rounded-pill p-1 mx-1">
          <input id="checkbox" type="checkbox" v-model="btnOnlineUsers" />
          <label class="switch" for="checkbox1" @click="btnOnlineUsersClick">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" class="slider">
              <path
                d="M288 32c0-17.7-14.3-32-32-32s-32 14.3-32 32V256c0 17.7 14.3 32 32 32s32-14.3 32-32V32zM143.5 120.6c13.6-11.3 15.4-31.5 4.1-45.1s-31.5-15.4-45.1-4.1C49.7 115.4 16 181.8 16 256c0 132.5 107.5 240 240 240s240-107.5 240-240c0-74.2-33.8-140.6-86.6-184.6c-13.6-11.3-33.8-9.4-45.1 4.1s-9.4 33.8 4.1 45.1c38.9 32.3 63.5 81 63.5 135.4c0 97.2-78.8 176-176 176s-176-78.8-176-176c0-54.4 24.7-103.1 63.5-135.4z"
              ></path>
            </svg>
          </label>
          <span class="mx-2" @click="btnOnlineUsersClick" style="cursor: pointer"
            >{{ btnOnlineUsersFont }}更新在線人數</span
          >
        </div>
        <div class="d-flex align-items-center border border-white rounded-pill p-1 mx-1">
          <input id="checkbox" type="checkbox" v-model="btnNumberOfComments" />
          <label class="switch" for="checkbox2" @click="btnNumberOfCommentsClick">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" class="slider">
              <path
                d="M288 32c0-17.7-14.3-32-32-32s-32 14.3-32 32V256c0 17.7 14.3 32 32 32s32-14.3 32-32V32zM143.5 120.6c13.6-11.3 15.4-31.5 4.1-45.1s-31.5-15.4-45.1-4.1C49.7 115.4 16 181.8 16 256c0 132.5 107.5 240 240 240s240-107.5 240-240c0-74.2-33.8-140.6-86.6-184.6c-13.6-11.3-33.8-9.4-45.1 4.1s-9.4 33.8 4.1 45.1c38.9 32.3 63.5 81 63.5 135.4c0 97.2-78.8 176-176 176s-176-78.8-176-176c0-54.4 24.7-103.1 63.5-135.4z"
              ></path>
            </svg>
          </label>
          <span class="mx-2" @click="btnNumberOfCommentsClick" style="cursor: pointer"
            >{{ btnNumberOfCommentsFont }}更新當前評論</span
          >
        </div>
      </div>
    </div>
    <table id="GameManageTable" class="display" style="width: 100% !important">
      <thead>
        <tr>
          <th></th>
          <th>GameId</th>
          <th>SteamId</th>
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
  <!--Form Start-->
  <Form @submit="onSubmit">
    <CModal
      alignment="center"
      scrollable
      :visible="visibleVerticallyCenteredScrollableDemo"
      @close="
        () => {
          visibleVerticallyCenteredScrollableDemo = false
        }
      "
      aria-labelledby="VerticallyCenteredExample2"
    >
      <CModalTitle id="VerticallyCenteredExample2" class="mt-4 text-center fs-2 fw-bold">{{
        Title
      }}</CModalTitle>
      <hr />

      <CModalBody>
        <div class="formBox">
          <label class="text-center" for="AppId">AppId</label>
          <Field
            id="AppId"
            name="AppId"
            class="form-control text-center"
            type="number"
            rules="required|numeric"
            v-model="AppId"
          />
          <ErrorMessage class="text-danger" name="AppId" />
        </div>
        <div class="formBox">
          <label class="text-center" for="Name">遊戲名稱</label>
          <Field
            name="Name"
            class="form-control text-center"
            type="text"
            rules="required"
            v-model="Name"
          />
          <ErrorMessage class="text-danger" name="Name" />
        </div>
        <div class="formBox">
          <label class="text-center" for="OriginalPrice">原始價格</label>
          <Field
            name="OriginalPrice"
            class="form-control text-center"
            type="number"
            rules="required"
            v-model="OriginalPrice"
          />
          <ErrorMessage class="text-danger" name="OriginalPrice" />
        </div>
        <div class="formBox">
          <label class="text-center" for="AgeRating">遊戲分級</label>
          <Field
            name="AgeRating"
            class="form-control text-center"
            type="text"
            rules="required"
            v-model="AgeRating"
            aria-placeholder="18+"
          />
          <ErrorMessage class="text-danger" name="AgeRating" />
        </div>
        <div class="formBox">
          <label class="text-center" for="ReleaseDate">上市日期</label>
          <Field
            name="ReleaseDate"
            class="form-control text-center"
            type="date"
            rules=""
            v-model="ReleaseDate"
          />
          <ErrorMessage class="text-danger" name="ReleaseDate" />
        </div>
        <div class="formBox">
          <label class="text-center" for="Publisher">開發商</label>
          <Field
            name="Publisher"
            class="form-control text-center"
            type="text"
            rules=""
            v-model="Publisher"
          />
          <ErrorMessage class="text-danger" name="Publisher" />
        </div>
        <div class="formBox">
          <label class="text-center" for="Description">遊戲介紹</label>
          <Field
            name="Description"
            class="form-control text-center"
            type="text"
            rules=""
            v-model="Description"
          />
          <ErrorMessage class="text-danger" name="Description" />
        </div>
        <div class="formBox">
          <label class="text-center" for="ImagePath">遊戲圖片</label>
          <img v-bind:src="imagesrc" id="imgPreview" title="上無內容" style="width: 250px" /><br />
          <Field
            name="ImagePath"
            class="form-control text-center"
            type="text"
            rules="url"
            @change="ImageChange"
            v-model="ImagePath"
          />
          <ErrorMessage class="text-danger" name="ImagePath" />
        </div>
        <div class="formBox">
          <label class="text-center" for="VideoPath">遊戲影片</label>
          <video v-bind:src="videosrc" id="videoPreview" width="250" controls autoplay muted></video
          ><br />
          <Field
            name="VideoPath"
            class="form-control text-center"
            type="text"
            rules="url"
            @change="VideoChange"
            v-model="VideoPath"
          />
          <ErrorMessage class="text-danger" name="VideoPath" />
        </div>
      </CModalBody>
      <CModalFooter>
        <CButton
          color="secondary"
          @click="
            () => {
              visibleVerticallyCenteredScrollableDemo = false
            }
          "
        >
          關閉
        </CButton>
        <CButton type="submit" color="primary">儲存</CButton>
        <!-- <CButton  @click="submitDataToDb">儲存</CButton> -->
      </CModalFooter>
    </CModal>
  </Form>
  <!--Form End-->
  <CToaster class="p-3" placement="top-end" style="height: 10%">
    <CToast
      v-for="(toast, index) in toasts"
      visible
      :key="index"
      class="text-white align-items-center custom-toast"
      :autohide="false"
    >
      <CToastHeader closeButton>
        <span class="me-auto fw-bold">更新全部價格進度: {{ progressNum }}%</span>
        <!-- <small>7 min ago</small> -->
      </CToastHeader>
      <CToastBody>
        <CProgress color="info" variant="striped" animated :value="toast.Progress"></CProgress>
      </CToastBody>
    </CToast>
  </CToaster>
</template>

<script setup>
// 使用 Pinia，利用 store 去訪問狀態
import { useIdentityStore } from '@/stores/identity.js'
const authStore = useIdentityStore()

import { CToaster, CToast } from '@coreui/vue'
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
import { toast } from 'vue3-toastify'
import 'vue3-toastify/dist/index.css'

import Swal from 'sweetalert2'
// 從環境變數取得 API BASE URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

//後端執行次數
// var priceProgress = false
var progressNum = ref(0)

// 初始化 DataTables
let dataTable = null
const visibleVerticallyCenteredScrollableDemo = ref(false)

//吐司初始化
var toasts = ref([])

//BootStrap呼喚alert畫面
const handleClick = () => {
  visibleVerticallyCenteredScrollableDemo.value = true
}

//Form驗證製作
import { defineRule, Form, Field, ErrorMessage, configure } from 'vee-validate'
import { required, between, confirmed, numeric, url } from '@vee-validate/rules'
import { localize } from '@vee-validate/i18n'
import zh_TW from '@/components/backend/Game/zh_TW.json'
import dataTableLanguage from '@/components/backend/hardware/dataTableLanguage.js'

defineRule('required', required)
defineRule('between', between)
defineRule('confirmed', confirmed)
defineRule('numeric', numeric)
defineRule('url', url)

var Title = ref('')
var imagesrc = ref('http://localhost:5173/public/img/noImage.png')
var videosrc = ref('#')
var gameGameId = ref('')
var AppId = ref('')
var Name = ref('')
var OriginalPrice = ref('')
var AgeRating = ref('')
var ReleaseDate = ref('')
var Publisher = ref('')
var Description = ref('')
var ImagePath = ref('')
var VideoPath = ref('')

var btnPrice = ref(false)
var btnOnlineUsers = ref(false)
var btnNumberOfComments = ref(false)

var btnPriceFont = ref('開啟')
var btnOnlineUsersFont = ref('開啟')
var btnNumberOfCommentsFont = ref('開啟')

// this.toasts[0].visible = false

//更新人數
function isTimeOpen() {
  fetch(`${apiUrl}/api/GamesManagement/isTimeOpen`, {
    method: 'GET'
  })
    .then((result) => {
      // 此時 result 是一個請求結果的物件
      // 注意傳回值型態，字串用 text()，JSON 用 json()
      if (result.ok) {
        return result.json()
      }
    })
    .then((data) => {
      btnPrice.value = data[0].toLowerCase()
      btnOnlineUsers.value = data[1].toLowerCase()
      btnNumberOfComments.value = data[2].toLowerCase()
    })
    .catch((error) => {
      console.log(error)
    })
}

//更新價格
function btnOnlinePriceClick() {
  fetch(`${apiUrl}/api/GamesManagement/GetGamePriceDataToDB`, {
    method: 'GET',
    headers: {
      Authorization: `Bearer ${authStore.getToken}` // 確保 token 正確傳遞
    }
  })
    .then((result) => {
      // 此時 result 是一個請求結果的物件
      // 注意傳回值型態，字串用 text()，JSON 用 json()
      if (result.ok) {
        return result.text()
      }
    })
    .then((data) => {
      if (data == 'True') {
        btnPrice.value = true
        btnPriceFont.value = '關閉'
      } else {
        btnPrice.value = false
        btnPriceFont.value = '開啟'
      }
    })
    .catch((error) => {
      console.log(error)
    })
  // btnPrice.value = true
}
//更新人數
function btnOnlineUsersClick() {
  toasts.value.push({
    // title: `更新全部價格進度:${progressNum.value}%`,
    content: 'Toast Content',
    visible: true,
    Progress: progressNum
  })
  const source = new EventSource(`${apiUrl}/api/GamesManagement/GamePriceProgress`)
  source.onmessage = (event) => {
    console.log('Received event with id:', event.lastEventId)
    progressNum.value = event.data
    toasts.value = [...toasts.value]
    // toasts.value.forEach((toast) => {
    //   toast.progress = progressNum.value
    // })
  }

  fetch(`${apiUrl}/api/GamesManagement/GetOnlineUsersDataToDB`, {
    method: 'GET',
    headers: {
      Authorization: `Bearer ${authStore.getToken}` // 確保 token 正確傳遞
    }
  })
    .then((result) => {
      // 此時 result 是一個請求結果的物件
      // 注意傳回值型態，字串用 text()，JSON 用 json()
      if (result.ok) {
        return result.text()
      }
    })
    .then((data) => {
      if (data == 'True') {
        btnOnlineUsers.value = true
        btnOnlineUsersFont.value = '關閉'
      } else {
        btnOnlineUsers.value = false
        btnOnlineUsersFont.value = '開啟'
      }
    })
    .catch((error) => {
      console.log(error)
    })
}
//更新評論
function btnNumberOfCommentsClick() {
  fetch(`${apiUrl}/api/GamesManagement/GetNumberOfCommentsDataToDB`, {
    method: 'GET',
    headers: {
      Authorization: `Bearer ${authStore.getToken}` // 確保 token 正確傳遞
    }
  })
    .then((result) => {
      // 此時 result 是一個請求結果的物件
      // 注意傳回值型態，字串用 text()，JSON 用 json()
      if (result.ok) {
        return result.text()
      }
    })
    .then((data) => {
      if (data == 'True') {
        btnNumberOfComments.value = true
        btnNumberOfCommentsFont.value = '關閉'
      } else {
        btnNumberOfComments.value = false
        btnNumberOfCommentsFont.value = '開啟'
      }
    })
    .catch((error) => {
      console.log(error)
    })
}

//照片更新事件
function ImageChange(event) {
  if (event.target.value != '') {
    var img = new Image()
    img.onload = function () {
      imagesrc.value = event.target.value
    }
    img.onerror = function () {
      // 如果網址有效但沒有圖片，顯示預設圖片
      $('#imgPreview').attr('src', '/img/noImage.png')
    }
    img.src = event.target.value // 設置圖片網址來檢查它是否有效
  } else {
    $('#imgPreview').attr('src', '/img/noImage.png')
  }
}

//影片更新事件
function VideoChange(event) {
  if (event.target.value != '') {
    videosrc.value = event.target.value
  }
}

//設定驗證功能之{field}名稱(紅色的字的名稱預設為英文)
configure({
  generateMessage: localize('zh_TW', {
    names: {
      Name: '遊戲名稱',
      OriginalPrice: '原始價格',
      AgeRating: '遊戲分級',
      ReleaseDate: '上市日期',
      Publisher: '開發商',
      Description: '遊戲介紹',
      ImagePath: '圖片連結',
      VideoPath: '影片連結'
    },
    messages: zh_TW.messages
  })
})

//編輯事件
$(document).on('click', 'button[id$="edit_button"]', function (event) {
  gameGameId.value = $(this).attr('data-GameId')
  event.preventDefault()
  event.stopPropagation()
  Title.value = '編輯遊戲'
  //優先載入js 在執行fetch
  fetch(`${apiUrl}/api/GamesManagement/GetEditJSON?id=${gameGameId.value}`, {
    method: 'GET'
  })
    .then((response) => {
      // 確保請求是否成功
      if (!response.ok) {
        throw new Error('Network response was not ok')
      }
      // 解析 html
      return response.json()
    })
    .then((val) => {
      AppId.value = val.appId
      Name.value = val.name
      OriginalPrice.value = val.originalPrice
      AgeRating.value = val.ageRating
      ReleaseDate.value = val.releaseDate
      Publisher.value = val.publisher
      Description.value = val.description
      ImagePath.value = val.imagePath
      VideoPath.value = val.videoPath
      imagesrc.value = val.imagePath
      videosrc.value = val.videoPath
      handleClick()
    })
    .catch((error) => {
      alert(error)
    })
    .finally(() => {
      // 异步操作完成后启用按钮
      $(this).prop('disabled', false)
    })
})

//刪除事件
$(document).on('click', 'button[id$="delete_button"]', function (event) {
  //抓取delete_button自訂屬性的值
  const deleteName = $(this).attr('data-name')
  const deleteGameId = $(this).attr('data-GameId')
  console.log(deleteGameId)
  event.stopPropagation()
  //sweetalert設定
  const swalWithBootstrapButtons = Swal.mixin({
    customClass: {
      confirmButton: 'btn btn-success',
      cancelButton: 'btn btn-danger'
    },
    buttonsStyling: false
  })
  swalWithBootstrapButtons
    .fire({
      title: `你確定要刪除${deleteName}?`,
      text: '您將無法恢復此狀態！',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: '是的，我要刪除！',
      cancelButtonText: '不，我再考慮一下',
      reverseButtons: true
    })
    .then((result) => {
      if (result.isConfirmed) {
        $.ajax({
          type: 'GET',
          url: `${apiUrl}/api/GamesManagement/PostDeletePartialToDB?id=${deleteGameId}`,
          headers: {
            Authorization: `Bearer ${authStore.getToken}` // 確保 token 正確傳遞
          }
        })
          .done((data) => {
            console.log(data)
            swalWithBootstrapButtons.fire({
              title: `已刪除${deleteName}`,
              text: '此操作無法復原',
              icon: 'success'
            })
            getdatatableData()
          })
          .fail((data) => {
            $('#systemLoading').hide()
            $('#System').html(data)
          })
      } else if (
        /* Read more about handling dismissals below */
        result.dismiss === Swal.DismissReason.cancel
      ) {
        swalWithBootstrapButtons.fire({
          title: `未刪除${deleteName}`,
          // text: "遊戲未刪除 :)",
          icon: 'error'
        })
      }
    })
})

//資料發送事件
function onSubmit() {
  var action = Title.value == '新增遊戲' ? 'PostCreatPartialToDB' : 'PostEditPartialToDB'

  $.ajax({
    type: 'POST',
    contentType: 'application/json',
    data: JSON.stringify({
      gameId: gameGameId.value,
      appId: AppId.value,
      name: Name.value,
      originalPrice: OriginalPrice.value,
      ageRating: AgeRating.value,
      releaseDate: ReleaseDate.value == '' ? null : ReleaseDate.value,
      publisher: Publisher.value,
      description: Description.value,
      imagePath: ImagePath.value,
      videoPath: VideoPath.value
    }),
    url: `${apiUrl}/api/GamesManagement/${action}`,
    headers: {
      Authorization: `Bearer ${authStore.getToken}` // 確保 token 正確傳遞
    }
  })
    .done((data) => {
      console.log(data)
      visibleVerticallyCenteredScrollableDemo.value = false
      toast.success(Title.value == '新增遊戲' ? `已新增 ${Name.value}` : `已編輯 ${Name.value}`, {
        theme: 'dark',
        autoClose: 2000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.TOP_CENTER
      })
      getdatatableData()
    })
    .fail((data) => {
      $('#systemLoading').hide()
      $('#System').html(data)
    })
}

//DataTable開始
//拿取datatable的資料
function getdatatableData() {
  // 發送非同步GET請求
  fetch(`${apiUrl}/api/GamesManagement/IndexJson`, {
    method: 'GET',
    headers: {
      Authorization: `Bearer ${authStore.getToken}` // 確保 token 正確傳遞
    }
  })
    .then((result) => {
      // 此時 result 是一個請求結果的物件
      // 注意傳回值型態，字串用 text()，JSON 用 json()
      if (result.ok) {
        return result.json()
      }
    })
    .then((data) => {
      // 添加新的資料
      dataTable.clear().draw()
      dataTable.rows.add(data).draw()

      $('#GameIndexLoading').hide()
    })
    .catch((error) => {
      console.log(error)
    })
}

onMounted(() => {
  isTimeOpen()
  getdatatableData()
  dataTable = new DataTable('#GameManageTable', {
    // ...dataTableConfig,
    columns: [
      {
        data: 'imagePath',
        width: '5%',
        className: 'text-center',
        render: function (data) {
          return '<img src="' + data + '" alt="圖片" style="width:150px">'
        },
        responsivePriority: 1
      },
      { data: 'gameId', width: '1%' },
      { data: 'appId', width: '1%' },
      { data: 'name', responsivePriority: 1, width: '5%' },
      { data: 'originalPrice', width: '2%' },
      { data: 'currentPrice', responsivePriority: 2, width: '2%' },
      { data: 'ageRating', width: '5%' },
      { data: 'comment', width: '5%' },
      { data: 'commentNum', width: '5%' },
      { data: 'publisher', width: '5%' },
      {
        data: null,
        orderable: false,
        width: '5%',
        className: 'text-center',
        // 按鈕 自定義
        render: function (data, type, row) {
          // 取得 productId
          let name = row.name
          let GameId = row.gameId
          // 編輯按鈕
          let editEle =
            '<button data-GameId="' +
            GameId +
            '"  data-name="' +
            name +
            '" @click="edit_button" class="mx-1" id="edit_button" data-bs-toggle="popover" data-bs-content="nothing"><i class="fa-solid fa-pen-to-square"></i></button>'
          // 刪除按鈕
          let deleteEle =
            '<button data-GameId="' +
            GameId +
            '"  data-name="' +
            name +
            '" id="delete_button" class="mx-1" data-bs-toggle="popover" data-bs-content="nothing"><i class="fa-solid fa-trash"></i></button>'
          if (type === 'display') {
            return `${editEle}${deleteEle}`
          }
          return data
        },
        responsivePriority: 1
      }
    ],
    // 標頭固定
    fixedHeader: true,
    // 響應式設計
    responsive: true,
    language: dataTableLanguage,
    // 自動寬度 關閉
    autoWidth: true,
    // 資料載入中 gif
    processing: true,
    buttons: ['copy', 'excel', 'pdf']
  })
})

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
#checkbox {
  display: none;
}

.switch {
  position: relative;
  width: 30px;
  height: 30px;
  background-color: rgb(99, 99, 99);
  border-radius: 50%;
  z-index: 1;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  border: 2px solid rgb(126, 126, 126);
  box-shadow: 0px 0px 3px rgb(2, 2, 2) inset;
}
.switch svg {
  width: 1em;
}
.switch svg path {
  fill: rgb(48, 48, 48);
}
#checkbox:checked + .switch {
  box-shadow:
    0px 0px 1px green inset,
    0px 0px 2px rgb(151, 243, 255) inset,
    0px 0px 10px green inset,
    0px 0px 40px rgb(151, 243, 255),
    0px 0px 100px green,
    0px 0px 5px green;
  border: 2px solid rgb(255, 255, 255);
  background-color: rgb(100, 245, 150);
}
#checkbox:checked + .switch svg {
  filter: drop-shadow(0px 0px 5px green);
}
#checkbox:checked + .switch svg path {
  fill: rgb(255, 255, 255);
}

.mx-1 {
  border: 0px solid white;
  background-color: grey;
  color: black;
  border-radius: 5px;
}

.mx-1:hover {
  background-color: rgb(145, 145, 145);
}

.dt-button {
  background-color: #313131 !important;
}
/* @import 'bootstrap/dist/css/bootstrap.min.css'; */
/* datatables 頂部布局調整 */
#GameManageTable_wrapper .dt-end .dt-search {
  text-align: center !important;
}

#GameManageTable_wrapper .dt-middle {
  /* text-align: end !important; */
  display: flex;
  justify-content: flex-end;
}

#GameManageTable_wrapper .dt-button {
  margin-bottom: 0;
}

.custom-toast {
  width: 600px;
}

.formBox {
  margin-bottom: 20px;
}

/* span,
button {
  display: block;
  margin: 10px 0;
}

label {
  display: block;
  margin-bottom: 5px;
} */
</style>
<!-- AppId=""
Name=""
OriginalPrice=""
AgeRating=""
ReleaseDate=""
Publisher=""
Description=""
ImagePath=""
VideoPath="" -->
<!-- import gameCreateView from "../backend/Game/gameCreateView.vue"
function submitDataToDb(){
$.ajax({
    type: "POST",
    contentType: "application/json",
    data: JSON.stringify({
        appId: AppId,
        name: Name,
        originalPrice:OriginalPrice,
        ageRating: AgeRating,
        releaseDate: ReleaseDate,
        publisher: Publisher,
        description: Description,
        imagePath: ImagePath,
        videoPath: VideoPath
    }),
    url: `${apiUrl}/api/GamesManagement/PostCreatPartialToDB`
}).done(data => {
        $("#systemLoading").hide();
        $("#System").html(data);
    })
    .fail(data => {
        $("#systemLoading").hide();
        $("#System").html(data);
    });
    
} 
function submitDataToDb(){
    const createViewContent = gameCreateViewRef.value
$.ajax({
    type: "POST",
    contentType: "application/json",
    data: JSON.stringify({
        appId: createViewContent.AppId,
        name: createViewContent.Name,
        originalPrice:createViewContent.OriginalPrice,
        ageRating: createViewContent.AgeRating,
        releaseDate: createViewContent.ReleaseDate,
        publisher: createViewContent.Publisher,
        description: createViewContent.Description,
        imagePath: createViewContent.ImagePath,
        videoPath: createViewContent.VideoPath
    }),
    url: `${apiUrl}/api/GamesManagement/PostCreatPartialToDB`
}).done(data => {
        $("#systemLoading").hide();
        $("#System").html(data);
    })
    .fail(data => {
        $("#systemLoading").hide();
        $("#System").html(data);
    });
}-->
