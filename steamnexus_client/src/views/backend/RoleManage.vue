<template>
  <div class="row">
    <div class="col-12 col-md-6 order-md-1 d-flex justify-content-center justify-content-md-start">
      <h2 id="SystemName" style="margin-top: 8px">權限管理系統</h2>
    </div>
    <div
      class="col-12 col-md-6 order-md-2 d-flex justify-content-center justify-content-md-end"
      id="SystemMenu"
    ></div>
  </div>

  <button type="button" class="btn btn-danger mb-3" @click="openCreateRoleModal">新增權限</button>

  <section class="section">
    <table id="RolesManageTable" class="display" style="width: 100%">
      <thead id="HardwareThead">
        <tr>
          <th data-priority="1">編號</th>
          <th>名稱</th>
          <th>刪除</th>
        </tr>
      </thead>
    </table>
  </section>

  <!-- 新增權限的浮動式窗 -->
  <CModal
    alignment="center"
    :visible="createRoleModal"
    @close="
      () => {
        createRoleModal = false
      }
    "
    aria-labelledby="CreateRoleModal"
  >
    <CModalHeader>
      <CModalTitle id="CreateRoleModal">新增權限</CModalTitle>
    </CModalHeader>
    <CModalBody>
      <form autocomplete="off">
        <div class="form-group">
          <label class="form-label">權限名稱：</label>
          <input class="form-control" id="RoleName" v-model="newRoleName" v-on:input="checkRole" />
          <div id="roleFeedback" class="invalid-feedback"></div>
        </div>
      </form>
    </CModalBody>
    <CModalFooter>
      <CButton
        color="secondary"
        @click="
          () => {
            createRoleModal = false
          }
        "
      >
        取消
      </CButton>
      <CButton color="primary" v-on:click="createRole" :disabled="!isFormValid">新增</CButton>
    </CModalFooter>
  </CModal>
</template>

<script setup>
// 核心模組 import
import $ from 'jquery'
// import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap/dist/js/bootstrap.bundle.js'
import DataTable from 'datatables.net-dt'
import 'datatables.net-fixedheader-dt'
import 'datatables.net-buttons-dt'
import 'datatables.net-responsive-dt'

import '@/assets/css/dataTables.datatables.min.css'
import '@/assets/css/fixedHeader.dataTables.min.css'
import '@/assets/css/buttons.dataTables.min.css'
import '@/assets/css/responsive.dataTables.min.css'

import { ref, onMounted } from 'vue'
import { CModal, CModalHeader, CModalTitle, CModalBody, CModalFooter, CButton } from '@coreui/vue'
import axios from 'axios'
import dataTableLanguage from '@/components/backend/hardware/dataTableLanguage.js' //i18n
import { onBeforeRouteLeave } from 'vue-router' //datatables off

// 使用 Pinia，利用 store 去訪問狀態
import { useIdentityStore } from '@/stores/identity.js'
const store = useIdentityStore()

// 特殊吐司
import { toast } from 'vue3-toastify'
import 'vue3-toastify/dist/index.css'

// 從環境變數取得 API BASE URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

var datatable = null

// 角色名稱
const newRoleName = ref('')
const rolesExists = ref(false) //確認名稱是否重複
const isFormValid = ref(false) //確認表單是否通過驗證，button disabled

let createRoleModal = ref(false)

// 清空表單
const clearForm = () => {
  newRoleName.value = '' // 重置權限名稱
  isFormValid.value = false // 重置表單驗證狀態
  $('#RoleName').removeClass('is-valid is-invalid') // 重置表單樣式
  $('#roleFeedback').hide() // 隱藏反饋訊息
}

const openCreateRoleModal = () => {
  clearForm() // 清空表單
  createRoleModal.value = true // 打開模態框
}

// 檢查權限名稱與英文大小寫
const checkRole = async () => {
  const roleValue = RoleName.value
  const englishRegex = /^[A-Za-z]+$/

  if (!roleValue) {
    $('#roleFeedback').hide()
    isFormValid.value = false
    return
  }

  // 檢查是否為英文字符
  if (!englishRegex.test(roleValue)) {
    const feedbackElement = $('#roleFeedback')
    feedbackElement.text('請輸入英文').removeClass('valid-feedback').addClass('invalid-feedback')
    $('#RoleName').removeClass('is-valid').addClass('is-invalid')
    feedbackElement.show()
    isFormValid.value = false
    return
  }

  store.getToken
  console.log('JWT DatatablesToken:', store.getToken) // 輸出 JWT Token

  try {
    const response = await axios.get(`${apiUrl}/api/MemberManagement/CheckRolesExists`, {
      params: { rolename: roleValue },
      headers: {
        Authorization: `Bearer ${store.getToken}` // 使用獲取的 JWT Token
      }
    })
    rolesExists.value = !response.data
    const feedbackElement = $('#roleFeedback')
    if (response.data) {
      feedbackElement
        .text('此 名稱 可以使用')
        .removeClass('invalid-feedback')
        .addClass('valid-feedback')
      $('#RoleName').removeClass('is-invalid').addClass('is-valid')
      isFormValid.value = true
    } else {
      feedbackElement
        .text('該 名稱 已被使用，請更換 名稱')
        .removeClass('valid-feedback')
        .addClass('invalid-feedback')
      $('#RoleName').removeClass('is-valid').addClass('is-invalid')
      isFormValid.value = false
    }
    feedbackElement.show()
  } catch (error) {
    $('#roleFeedback').text('無法檢查名稱').addClass('invalid-feedback').show()
    isFormValid.value = false
  }
}

// 檢查權限名稱是否存在
const checkRoleExists = async (roleName) => {
  store.getToken
  console.log('JWT DatatablesToken:', store.getToken) // 輸出 JWT Token

  try {
    const response = await axios.get(`${apiUrl}/api/MemberManagement/CheckRolesExists`, {
      params: { rolename: roleName },
      headers: {
        Authorization: `Bearer ${store.getToken}` // 使用獲取的 JWT Token
      }
    })
    return response.data
  } catch (error) {
    console.error('檢查權限名稱失敗：', error)
    return false
  }
}

// 新增角色
const createRole = async () => {
  console.log('新增權限表單提交')
  if (newRoleName.value.trim() === '') {
    // alert('請輸入權限名稱')
    toast.error('請輸入權限名稱', {
        theme: 'colored',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.BOTTOM_CENTER
    })
    // return
  }

  const roleExists = await checkRoleExists(newRoleName.value)
  console.log('權限名稱檢查結果:', roleExists)
  if (roleExists === false) {
    // alert('角色名稱已存在')
    toast.error('權限名稱已存在', {
        theme: 'colored',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.BOTTOM_CENTER
    })
    return
  }

  // 創建 FormData 對象
  const formData = new FormData()
  formData.append('RoleName', newRoleName.value)

  store.getToken
  console.log('JWT DatatablesToken:', store.getToken) // 輸出 JWT Token

  axios
    .post(`${apiUrl}/api/MemberManagement/CreateRole`, formData, {
      headers: {
        Authorization: `Bearer ${store.getToken}`, // 使用獲取的 JWT Token
        'Content-Type': 'multipart/form-data'
      }
    })
    .then((response) => {
      if (response.data && response.data.message) {
        //
        toast.success(response.data.message, {
          theme: 'colored',
          autoClose: 1000,
          transition: toast.TRANSITIONS.ZOOM,
          position: toast.POSITION.BOTTOM_CENTER
        })
        //
        newRoleName.value = ''
        createRoleModal.value = false
        datatable.ajax.reload()
        isFormValid.value = false // 重置表單狀態
      } else {
        // alert('新增角色失敗，請重試')
        toast.error('新增權限失敗，請重試', {
          theme: 'colored',
          autoClose: 1000,
          transition: toast.TRANSITIONS.ZOOM,
           position: toast.POSITION.BOTTOM_CENTER
        })
      }
    })
    .catch((err) => {
      console.error('新增角色失敗，錯誤詳情：', err)
      if (err.response) {
        toast.error('請確認是否輸入英文大小寫', {
          theme: 'colored',
          autoClose: 1000,
           transition: toast.TRANSITIONS.ZOOM,
           position: toast.POSITION.BOTTOM_CENTER
        })
      } else {
        toast.error('新增權限失敗，請重新測試', {
          theme: 'colored',
          autoClose: 1000,
          transition: toast.TRANSITIONS.ZOOM,
          position: toast.POSITION.BOTTOM_CENTER
        })
      }
    })
}

// 刪除角色
const deleteRole = (roleId) => {
  store.getToken
  console.log('JWT DatatablesToken:', store.getToken) // 輸出 JWT Token
  axios
    .post(
      `${apiUrl}/api/MemberManagement/DeleteRole?id=${roleId}`,
      {}, // 這裡可以留空或傳遞需要的數據
      {
        headers: {
          Authorization: `Bearer ${store.getToken}`,
          'Content-Type': 'application/json'
        }
      }
    )
    .then((response) => {
      // alert(response.data.message)
      toast.success(response.data.message, {
        theme: 'colored',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.BOTTOM_CENTER
      })
      // 重新加載表格數據
      datatable.ajax.reload()
    })
    .catch(() => {
      //alert('資料已有關聯紀錄，刪除失敗')
      toast.error('資料已有關聯紀錄，刪除失敗', {
        theme: 'colored',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.BOTTOM_CENTER
      })
    })
}

onMounted(() => {
  // 初始化 Datatables
  store.getToken
  console.log('JWT DatatablesToken:', store.getToken) // 輸出 JWT Token
  datatable = new DataTable('#RolesManageTable', {
    ajax: {
      type: 'GET',
      url: `${apiUrl}/api/MemberManagement/RolesData`,
      dataSrc: function (json) {
        return json
      },
      headers: {
        Authorization: `Bearer ${store.getToken}`,
        'Content-Type': 'application/json'
      }
    },
    // column 定義
    columns: [
      { data: 'roleId', width: '10%', className: 'text-center' },
      { data: 'roleName', width: '10%', className: 'text-center' },
      {
        data: null,
        orderable: false,
        width: '10%',
        className: 'text-center',
        render: function (data, type, row) {
          // 取得 roleId

          let RoleName = row.roleName
          // 刪除按鈕
          let resetEle = `<button class="btn btn-danger del-btn" data-roleid="${row.roleId}" data-rolename="${RoleName}" data-bs-toggle="popover" data-bs-content="nothing"><i class="bi bi-trash3-fill"></i></button>`

          if (type === 'display') {
            return `${resetEle}`
          }
          return data
        },
        responsivePriority: 1
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
    language: dataTableLanguage,
    // 預設排序 ~ 根據第一個欄位 升冪排序
    order: [[1, 'asc']],
    // 自動寬度 關閉
    autoWidth: true
  })

  // 綁定刪除按鈕的點擊事件
  $('#RolesManageTable').on('click', '.del-btn', function () {
    const roleId = $(this).data('roleid')
    if (confirm('確定要刪除此權限嗎？')) {
      deleteRole(roleId)
    }
  })
})

// 路由離開時觸發
onBeforeRouteLeave(() => {
  // 銷毀 DataTable
  datatable.clear().draw()
  datatable.destroy()
  datatable = null
  // 事件監聽器移除
  $('#RolesManageTable').off('click', '.del-btn')
  // $(document).off('click', '.Cancel-btn')
  // $(document).off('click', '.Edit-btn')
})
</script>

<style></style>
