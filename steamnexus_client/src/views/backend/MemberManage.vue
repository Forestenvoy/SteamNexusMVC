<template>
  <div class="row">
    <div class="col-12 col-md-6 order-md-1 d-flex justify-content-center justify-content-md-start">
      <h2 id="SystemName" style="margin-top: 8px">會員管理系統</h2>
    </div>
    <div
      class="col-12 col-md-6 order-md-2 d-flex justify-content-center justify-content-md-end"
      id="SystemMenu"
    ></div>
  </div>

  <button type="button" class="btn btn-danger mb-3" @click="openCreateMemberModal">
    新增使用者
  </button>

  <section class="section">
    <table id="MemberManageTable" class="display" style="width: 100%">
      <thead id="HardwareThead">
        <tr>
          <th data-priority="1">姓名</th>
          <th>信箱</th>
          <th>性別</th>
          <th>電話</th>
          <th>生日</th>
          <th>個人照</th>
          <th>權限</th>
          <th>操作</th>
        </tr>
      </thead>
    </table>
  </section>

  <!-- 新增使用者浮動視窗 -->
  <CModal
    alignment="center"
    :visible="createUserModal"
    @close="
      () => {
        createUserModal = false
      }
    "
    aria-labelledby="createUserModal"
  >
    <CModalHeader>
      <CModalTitle id="createUserModal">新增使用者</CModalTitle>
    </CModalHeader>
    <CModalBody>
      <CRow class="mb-3">
        <form @submit.prevent="submitForm" autocomplete="off">
          <!-- 第一部分：信箱、密碼、確認密碼 -->
          <div v-if="currentPage === 1">
            <div class="input-group input-group-lg mb-3">
              <span class="input-group-text" id="inputGroup-sizing-lg" style="color: white"
                >Email：</span
              >
              <input
                type="email"
                class="form-control"
                aria-label="Sizing example input"
                aria-describedby="inputGroup-sizing-lg"
                id="email"
                placeholder="請輸入您的電子信箱"
                required
                maxlength="100"
                v-model="email"
                @input="checkEmail"
              />
              <div id="emailFeedback" class="invalid-feedback"></div>
            </div>
            <div class="input-group input-group-lg mb-3">
              <span class="input-group-text" id="inputGroup-sizing-lg" style="color: white"
                >密碼：</span
              >
              <input
                type="password"
                class="form-control"
                aria-label="Sizing example input"
                aria-describedby="inputGroup-sizing-lg"
                id="Password"
                placeholder="請輸入您的密碼"
                required
                maxlength="20"
                v-model="password"
                @input="validatePasswords"
              />
            </div>
            <div class="input-group input-group-lg mb-3">
              <span class="input-group-text" id="inputGroup-sizing-lg" style="color: white"
                >確認密碼：</span
              >
              <input
                type="password"
                class="form-control"
                aria-label="Sizing example input"
                aria-describedby="inputGroup-sizing-lg"
                id="ConfirmPassword"
                placeholder="請再次輸入您的密碼"
                required
                maxlength="20"
                v-model="confirmPassword"
                @input="validatePasswords"
              />
              <div id="passwordMismatchFeedback" class="invalid-feedback">密碼與確認密碼不一致</div>
            </div>

            <!-- 第一部分按鈕 -->
            <div class="d-flex justify-content-end">
              <CButton color="secondary" @click="closeCreateUserModal">取消</CButton>
              <CButton
                color="primary"
                @click="goToNextPage"
                class="ms-2"
                :disabled="!isFirstPartValid"
                >下一頁</CButton
              >
            </div>
          </div>

          <!-- 第二部分：姓名、電話、性別、生日、大頭貼 -->
          <div v-if="currentPage === 2">
            <div class="input-group input-group-lg mb-3">
              <span class="input-group-text" id="inputGroup-sizing-lg" style="color: white"
                >姓名：</span
              >
              <input
                type="text"
                class="form-control"
                aria-label="Sizing example input"
                aria-describedby="inputGroup-sizing-lg"
                id="name"
                placeholder="請輸入您的姓名"
                required
                maxlength="50"
                v-model="name"
                @input="validatename"
              />
              <div id="nameFeedback" class="invalid-feedback">此為必填欄位</div>
            </div>
            <div class="input-group input-group-lg mb-3">
              <span class="input-group-text" id="inputGroup-sizing-lg" style="color: white"
                >電話：</span
              >
              <input
                type="text"
                class="form-control"
                aria-label="Sizing example input"
                aria-describedby="inputGroup-sizing-lg"
                id="phone"
                pattern="^09\d{8}$"
                maxlength="10"
                placeholder="手機號碼必須以09開頭且是10位數字"
                v-model="phone"
              />
            </div>
            <div class="input-group input-group-lg mb-3">
              <span class="input-group-text" id="inputGroup-sizing-lg" style="color: white"
                >性別：</span
              >
              <div class="d-flex align-items-center justify-content-start ps-4">
                <div class="form-check form-check-inline">
                  <input
                    class="form-check-input text-center"
                    type="radio"
                    name="gender2"
                    id="male2"
                    value="true"
                    checked
                    v-model="gender"
                  />
                  <label class="form-check-label" for="male">男</label>
                </div>
                <div class="form-check form-check-inline">
                  <input
                    class="form-check-input text-center"
                    type="radio"
                    name="gender2"
                    id="female2"
                    value="false"
                    v-model="gender"
                  />
                  <label class="form-check-label" for="female">女</label>
                </div>
              </div>
            </div>
            <div class="input-group input-group-lg mb-3">
              <span class="input-group-text" id="inputGroup-sizing-lg" style="color: white"
                >生日：</span
              >
              <input
                type="date"
                class="form-control"
                aria-label="Sizing example input"
                aria-describedby="inputGroup-sizing-lg"
                id="birthday"
                v-model="birthday"
              />
            </div>
            <div class="clearfix">
              <span class="input-group-text" id="inputGroup-sizing-lg" style="color: white"
                >大頭照：</span
              >
              <input
                type="file"
                class="form-control"
                aria-label="Sizing example input"
                aria-describedby="inputGroup-sizing-lg"
                id="photo"
                @change="uploadPhoto"
              />
              <CImage
                align="center"
                :src="photoPreview"
                class="img-thumbnail"
                aria-label="Sizing example input"
                aria-describedby="inputGroup-sizing-lg"
                id="display_photo"
                alt="預覽圖片"
                width="200"
                height="200"
              />
            </div>

            <!-- 第二部分按鈕 -->
            <div class="d-flex justify-content-end">
              <CButton color="secondary" @click="goToPreviousPage">上一頁</CButton>
              <CButton color="secondary" @click="closeCreateUserModal" class="ms-2">取消</CButton>
              <CButton color="primary" @click="submitForm" class="ms-2">新增</CButton>
            </div>
          </div>
        </form>
      </CRow>
    </CModalBody>
    <CModalFooter>
      <!-- <CButton 
        color="secondary"
        @click="
          () => {
            createUserModal = false
          }
        "
      >
        關閉
      </CButton>
      <CButton color="primary" v-on:click="submitForm">新增</CButton> -->
    </CModalFooter>
  </CModal>

  <!-- 新增編輯浮動視窗 -->
  <CModal
    alignment="center"
    :visible="EditUserModal"
    @close="
      () => {
        EditUserModal = false
      }
    "
    aria-labelledby="EditUserModal"
  >
    <CModalHeader>
      <CModalTitle id="EditUserModal">編輯使用者</CModalTitle>
    </CModalHeader>
    <CModalBody>
      <CRow class="mb-3">
        <form v-on:submit.prevent="submitForm_Edit" autocomplete="off">
          <div class="input-group input-group-lg mb-3">
            <span class="input-group-text" id="inputGroup-sizing-lg" style="color: white"
              >編號：</span
            >
            <input
              type="text"
              class="form-control"
              aria-label="Sizing example input"
              aria-describedby="inputGroup-sizing-lg"
              id="userid"
              placeholder="請輸入您的姓名"
              readonly
              maxlength="50"
              v-model="userid"
            />
          </div>

          <div class="input-group input-group-lg mb-3">
            <span class="input-group-text" id="inputGroup-sizing-lg" style="color: white"
              >Email：</span
            >
            <input
              type="email"
              class="form-control"
              aria-label="Sizing example input"
              aria-describedby="inputGroup-sizing-lg"
              id="email"
              placeholder="請輸入您的電子信箱"
              readonly
              maxlength="100"
              v-model="email"
              v-on:input="checkEmail"
            />
            <!-- <div v-if="emailExists" class="invalid-feedback">電子郵件已存在</div>   V-on:blur="checkEmail"-->
            <div id="emailFeedback" class="invalid-feedback"></div>
          </div>
          <div class="input-group input-group-lg mb-3">
            <span class="input-group-text" id="inputGroup-sizing-lg" style="color: white"
              >姓名：</span
            >
            <input
              type="text"
              class="form-control"
              aria-label="Sizing example input"
              aria-describedby="inputGroup-sizing-lg"
              id="name"
              placeholder="請輸入您的姓名"
              required
              maxlength="50"
              v-model="name"
            />
          </div>
          <div class="input-group input-group-lg mb-3">
            <span class="input-group-text" id="inputGroup-sizing-lg" style="color: white"
              >生日：</span
            >
            <input
              type="date"
              class="form-control"
              aria-label="Sizing example input"
              aria-describedby="inputGroup-sizing-lg"
              id="birthday"
              v-model="birthday"
            />
          </div>
          <div class="input-group input-group-lg mb-3">
            <span class="input-group-text" id="inputGroup-sizing-lg" style="color: white"
              >電話：</span
            >
            <input
              type="text"
              class="form-control"
              aria-label="Sizing example input"
              aria-describedby="inputGroup-sizing-lg"
              id="phone"
              pattern="^09\d{8}$"
              maxlength="10"
              placeholder="手機號碼必須以09開頭且是10位數字"
              v-model="phone"
            />
          </div>
          <div class="input-group input-group-lg mb-3">
            <span class="input-group-text" id="inputGroup-sizing-lg" style="color: white"
              >性別：</span
            >
            <div class="d-flex align-items-center justify-content-start ps-4">
              <div class="form-check form-check-inline">
                <input
                  class="form-check-input text-center"
                  type="radio"
                  name="gender"
                  id="male"
                  v-model="gender"
                  :value="true"
                />
                <label class="form-check-label" for="male">男</label>
              </div>
              <div class="form-check form-check-inline">
                <input
                  class="form-check-input text-center"
                  type="radio"
                  name="gender"
                  id="female"
                  v-model="gender"
                  :value="false"
                />
                <label class="form-check-label" for="female">女</label>
              </div>
            </div>
          </div>
          <div class="input-group input-group-lg mb-3">
            <span class="input-group-text" id="inputGroup-sizing-lg" style="color: white"
              >權限：</span
            >
            <input
              type="text"
              class="form-control"
              aria-label="Sizing example input"
              aria-describedby="inputGroup-sizing-lg"
              id="roleName"
              maxlength="50"
              v-model="newRoleName"
            />
          </div>
          <div class="clearfix">
            <span class="input-group-text" id="inputGroup-sizing-lg" style="color: white"
              >大頭照：</span
            >
            <input
              type="file"
              class="form-control"
              aria-label="Sizing example input"
              aria-describedby="inputGroup-sizing-lg"
              id="photo"
              v-on:change="uploadPhoto"
            />
            <CImage
              align="center"
              :src="photoPreview"
              class="img-thumbnail"
              aria-label="Sizing example input"
              aria-describedby="inputGroup-sizing-lg"
              id="display_photo"
              alt="預覽圖片"
              width="200"
              height="200"
            />
          </div>
        </form>
      </CRow>
    </CModalBody>
    <CModalFooter>
      <CButton
        color="secondary"
        @click="
          () => {
            EditUserModal = false
          }
        "
      >
        關閉
      </CButton>
      <CButton color="primary" v-on:click="submitEditForm">修改</CButton>
    </CModalFooter>
  </CModal>
</template>

<script setup>
// 核心模組 import
import $ from 'jquery'
import DataTable from 'datatables.net-dt'
import 'datatables.net-fixedheader-dt'
import 'datatables.net-buttons-dt'
import 'datatables.net-responsive-dt'

import '@/assets/css/dataTables.datatables.min.css'
import '@/assets/css/fixedHeader.dataTables.min.css'
import '@/assets/css/buttons.dataTables.min.css'
import '@/assets/css/responsive.dataTables.min.css'

import { ref, onMounted, computed } from 'vue'
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

let createUserModal = ref(false) // 建立使用者模態框
let EditUserModal = ref(false) // 修改使用者模態框

// 使用者名稱
const userid = ref(0)
const email = ref('')
const emailExists = ref(false) //確認電子信箱是否重複
const password = ref('')
const confirmPassword = ref('')
const name = ref('')
const birthday = ref('')
const phone = ref('')
const gender = ref(true) // 默認為男性
const photo = ref(null) // 存儲上傳的照片
const photoPreview = ref(null) // 存儲照片預覽
const newRoleName = ref('') //權限名稱

// 預設圖片的URL
const defaultPhotoUrl = `${apiUrl}/images/headshots/default.jpg`

const currentPage = ref(1)

//切換註冊頁面
const goToNextPage = () => {
  if (isFirstPartValid.value) {
    currentPage.value = 2
  }
}

const goToPreviousPage = () => {
  currentPage.value = 1
}

// 清空表單
const openCreateMemberModal = () => {
  clearForm()
  photoPreview.value = defaultPhotoUrl // 設置圖片預覽為預設圖片
  createUserModal.value = true // 打開模態框
  currentPage.value = 1
}

//關閉表單
const closeCreateUserModal = () => {
  createUserModal.value = false
}

//確認電子信箱是否重複
const checkEmail = async () => {
  const emailValue = email.value
  if (emailValue) {
    store.getToken
    console.log('JWT DatatablesToken:', store.getToken) // 輸出 JWT Token
    try {
      const response = await axios.get(`${apiUrl}/api/MemberManagement/CheckEmailExists`, {
        params: { email: email.value },
        headers: {
          Authorization: `Bearer ${store.getToken}`, // 使用獲取的 JWT Token
          'Content-Type': 'application/json'
        }
      })
      emailExists.value = !response.data
      const feedbackElement = $('#emailFeedback')
      if (response.data) {
        feedbackElement
          .text('此 Email 可以使用')
          .removeClass('invalid-feedback')
          .addClass('valid-feedback')
        $('#email').removeClass('is-invalid').addClass('is-valid')
      } else {
        feedbackElement
          .text('該 Email 已被使用,請更換Email')
          .removeClass('valid-feedback')
          .addClass('invalid-feedback')
        $('#email').removeClass('is-valid').addClass('is-invalid')
      }
      feedbackElement.show()
    } catch (error) {
      $('#emailFeedback').text('無法檢查 Email').addClass('invalid-feedback').show()
    }
  } else {
    $('#emailFeedback').hide()
  }
}

// 檢查電子信箱是否重複
// const checkEmail = async () => {
//   try {
//     const response = await axios.get(`${apiUrl}/api/MemberManagement/CheckEmailExists`, {
//       params: { email: email.value }
//     })
//     emailExists.value = !response.data
//   } catch (error) {
//     toast.error('檢查電子郵件失敗', {
//       theme: 'dark',
//       autoClose: 1000,
//       transition: toast.TRANSITIONS.ZOOM,
//       position: toast.POSITION.TOP_CENTER
//     })
//   }
// }

// 上傳照片
const uploadPhoto = (event) => {
  const file = event.target.files[0]
  if (file) {
    photo.value = file
    const reader = new FileReader()
    reader.onload = (e) => {
      photoPreview.value = e.target.result
    }
    reader.readAsDataURL(file)
  } else {
    photo.value = null
    photoPreview.value = null
    // photoPreview.value = defaultPhotoUrl // 重置為預設圖片
  }
}

// 檢查密碼是否一致
const validatePasswords = () => {
  const passwordValue = $('#Password').val()
  const confirmPasswordValue = $('#ConfirmPassword').val()
  if (passwordValue !== confirmPasswordValue) {
    $('#passwordMismatchFeedback').show()
    $('#ConfirmPassword').addClass('is-invalid')
  } else {
    $('#passwordMismatchFeedback').hide()
    $('#ConfirmPassword').removeClass('is-invalid').addClass('is-valid')
  }
}

// 驗證姓名是否有填入數值
const validatename = () => {
  const nameValue = $('#name').val()
  if (nameValue.trim() === '') {
    $('#nameFeedback').show()
    $('#name').addClass('is-invalid')
  } else {
    $('#nameFeedback').hide()
    $('#name').removeClass('is-invalid').addClass('is-valid')
  }
}

//驗證第一部份表單是否有誤
const isFirstPartValid = computed(() => {
  return (
    email.value &&
    password.value &&
    confirmPassword.value &&
    password.value === confirmPassword.value &&
    !emailExists.value
  )
})

// 新增使用者
const submitForm = async () => {
  // 確認密碼是否一致
  // if (password.value !== confirmPassword.value) {
  //   toast.error('密碼和確認密碼不匹配', {
  //     theme: 'dark',
  //     autoClose: 1000,
  //     transition: toast.TRANSITIONS.ZOOM,
  //     position: toast.POSITION.TOP_CENTER
  //   })
  //   return
  // }

  // 確認電子信箱是否存在
  // try {
  //   const response = await axios.get(`${apiUrl}/api/MemberManagement/CheckEmailExists`, {
  //     params: { email: email.value }
  //   })
  //   emailExists.value = !response.data

  //   if (emailExists.value) {
  //     toast.error('電子郵件已存在', {
  //       theme: 'dark',
  //       autoClose: 1000,
  //       transition: toast.TRANSITIONS.ZOOM,
  //       position: toast.POSITION.TOP_CENTER
  //     })
  //     return
  //   }
  // } catch (error) {
  //   toast.error('檢查電子郵件失敗', {
  //     theme: 'dark',
  //     autoClose: 1000,
  //     transition: toast.TRANSITIONS.ZOOM,
  //     position: toast.POSITION.TOP_CENTER
  //   })
  //   return
  // }

  const formData = new FormData()
  formData.append('Name', name.value)
  formData.append('Password', password.value)
  formData.append('ConfirmPassword', confirmPassword.value)
  formData.append('Email', email.value)
  formData.append('Birthday', birthday.value)
  formData.append('Phone', phone.value)
  formData.append('Gender', gender.value)
  if (photo.value) {
    formData.append('Photo', photo.value)
  }

  store.getToken
  console.log('JWT DatatablesToken:', store.getToken) // 輸出 JWT Token

  axios
    .post(`${apiUrl}/api/MemberManagement/CreateMember`, formData, {
      headers: {
        Authorization: `Bearer ${store.getToken}`, // 使用獲取的 JWT Token
        'Content-Type': 'multipart/form-data'
      }
    })
    .then((response) => {
      if (response.data.success) {
        toast.success(response.data.message, {
          theme: 'colored',
           autoClose: 1000,
          transition: toast.TRANSITIONS.ZOOM,
          position: toast.POSITION.BOTTOM_CENTER
        })
        datatable.ajax.reload()
        createUserModal.value = false
        clearForm()
      } else {
        toast.error(response.data.message, {
          theme: 'colored',
           autoClose: 1000,
           transition: toast.TRANSITIONS.ZOOM,
           position: toast.POSITION.BOTTOM_CENTER
        })
      }
    })
    .catch((error) => {
      toast.error('新增使用者失敗：姓名未填寫', {
        theme: 'colored',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.BOTTOM_CENTER
      })
    })
}

// 重置表單
const clearForm = () => {
  email.value = ''
  password.value = ''
  confirmPassword.value = ''
  name.value = ''
  birthday.value = ''
  phone.value = ''
  gender.value = true
  photo.value = null
  // photoPreview.value = null
}

// 刪除使用者
const deleteUser = (userId) => {
  store.getToken
  console.log('JWT DatatablesToken:', store.getToken) // 輸出 JWT Token
  console.log('userId:', userId)
  axios
    // .delete(`${apiUrl}/api/MemberManagement/DeleteUser`, {
    .post(
      `${apiUrl}/api/MemberManagement/DeleteUser?id=${userId}`,
      {}, // 這裡可以留空或傳遞需要的數據
      {
        headers: {
          Authorization: `Bearer ${store.getToken}`, // 使用獲取的 JWT Token
          'Content-Type': 'application/json'
        }
      }
    )
    .then((response) => {
      //alert('使用者刪除成功')
      toast.success(response.data.message, {
        theme: 'colored',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.BOTTOM_CENTER
      })
      datatable.ajax.reload()
    })
    .catch((err) => {
      //alert('使用者刪除失敗')
      toast.error('資料已有關聯紀錄，刪除失敗', {
        theme: 'colored',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.BOTTOM_CENTER
      })
    })
}

// 清空表單
const openEditMemberModal = (user) => {
  clearForm()

  // 填充表單
  userid.value = user.userId
  email.value = user.email
  name.value = user.name
  birthday.value = user.birthday ? new Date(user.birthday).toISOString().substring(0, 10) : ''
  phone.value = user.phone
  gender.value = user.gender === '男'
  photoPreview.value = user.photo ? `${apiUrl}/images/headshots/${user.photo}` : null
  newRoleName.value = user.roleName // 新增角色名稱

  // 打開模態框
  EditUserModal.value = true
}

// 提交編輯表單
const submitEditForm = async () => {
  console.log('submitEditForm called')
  const formData = new FormData()
  formData.append('UserId', userid.value)
  formData.append('Email', email.value)
  formData.append('Name', name.value)
  formData.append('Birthday', birthday.value)
  formData.append('Phone', phone.value)
  formData.append('Gender', gender.value)
  formData.append('RoleName', roleName.value)
  if (photo.value) {
    formData.append('Photo', photo.value)
  }

  store.getToken
  console.log('JWT DatatablesToken:', store.getToken) // 輸出 JWT Token

  try {
    const response = await axios.post(`${apiUrl}/api/MemberManagement/EditUser`, formData, {
      headers: {
        Authorization: `Bearer ${store.getToken}`, // 使用獲取的 JWT Token
        'Content-Type': 'multipart/form-data'
      }
    })

    if (response.status === 204) {
      toast.success('使用者信息已成功更新', {
        theme: 'colored',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.BOTTOM_CENTER
      })
      datatable.ajax.reload()
      EditUserModal.value = false
      // 在這裡刷新數據表格，例如使用 emit 事件通知父組件
    } else {
      toast.error('更新使用者信息失敗', {
        theme: 'colored',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.BOTTOM_CENTER
      })
    }
  } catch (error) {
    toast.error('更新使用者信息失敗', {
        theme: 'colored',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.BOTTOM_CENTER
    })
  }
}

onMounted(() => {
  // 初始化 Datatables
  store.getToken
  console.log('JWT DatatablesToken:', store.getToken) // 輸出 JWT Token
  datatable = new DataTable('#MemberManageTable', {
    ajax: {
      type: 'GET',
      url: `${apiUrl}/api/MemberManagement/GetUsersWithRoles`,
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
      // { "data": "userId", "width": "10%", "className": "text-center" },
      { data: 'name', width: '10%', className: 'text-center' },
      { data: 'email', width: '10%', className: 'text-center' },
      { data: 'gender', width: '10%', className: 'text-center' },
      { data: 'phone', width: '10%', className: 'text-center' },
      {
        data: 'birthday',
        width: '10%',
        className: 'text-center',
        render: function (data, type, row) {
          if (type === 'display' && data) {
            let date = new Date(data)
            return `${date.getFullYear()}-${('0' + (date.getMonth() + 1)).slice(-2)}-${('0' + date.getDate()).slice(-2)}`
          }
          return data
        }
      },
      {
        data: 'photo',
        width: '10%',
        className: 'text-center',
        render: function (data, type, row) {
          return data
            ? `<img src="${apiUrl}/images/headshots/${data}" alt="Image" style="width: 100px; height: 100px;" />`
            : 'No Image'
        }
      },
      { data: 'roleName', width: '10%', className: 'text-center' },
      {
        data: null,
        orderable: false,
        width: '10%',
        className: 'text-center',
        render: function (data, type, row, meta) {
          // 取得 UserId
          let UserId = row.userId
          let Name = row.name
          let Email = row.email
          let Gender = row.gender
          let Phone = row.phone
          let Birthday = row.birthday
          let Photo = row.photo
          let RoleName = row.roleName
          // 編輯按鈕
          let enterEle = `<button class="btn btn-danger" data-userid="${UserId}" data-name="${Name}" data-email="${Email}"  data-gender="${Gender}" data-phone="${Phone}" data-birthday="${Birthday}" data-photo="${Photo}" data-rolename="${RoleName}" id="edit_btn" data-bs-toggle="popover" data-bs-content="nothing"><i class="bi bi-pencil-square"></i></button>`
          // 刪除按鈕
          let resetEle = `<button class="btn btn-danger" data-userid="${UserId}" data-name="${Name}" data-email="${Email}"  data-gender="${Gender}" data-phone="${Phone}" data-birthday="${Birthday}" data-photo="${Photo}" data-rolename="${RoleName}" id="delete_btn" data-bs-toggle="popover" data-bs-content="nothing"><i class="bi bi-trash3-fill"></i></button>`

          if (type === 'display') {
            return `${enterEle}&nbsp;${resetEle}`
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
  $('#MemberManageTable').on('click', '#delete_btn', function () {
    const userId = $(this).data('userid')
    if (confirm('確定要刪除此使用者嗎？')) {
      deleteUser(userId)
    }
  })

  // 確認密碼是否一致
  $('#Password, #ConfirmPassword').on('input', validatePasswords)

  // 綁定編輯按鈕的點擊事件
  $('#MemberManageTable').on('click', '#edit_btn', function () {
    const userId = $(this).data('userid')
    const user = datatable
      .rows()
      .data()
      .toArray()
      .find((user) => user.userId === userId)
    if (user) {
      console.log('user', user)
      openEditMemberModal(user)
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
