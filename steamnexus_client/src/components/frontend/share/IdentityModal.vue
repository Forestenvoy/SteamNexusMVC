<template>
  <div class="LRModal" ref="LRModal" v-if="store.getShowLogin">
    <span class="icon-close" @click="closeModal">
      <i class="fa fa-times" aria-hidden="true"></i>
    </span>

    <div class="form-box login" ref="loginForm">
      <h2>會員登入</h2>
      <form @submit.prevent="submitLogin">
        <!-- 信箱 -->
        <div class="input-box">
          <span class="icon"><i class="fas fa-envelope"></i></span>
          <input
            type="text"
            class="mx-2"
            required
            v-model="loginEmail"
            autocomplete="off"
            :tabindex="loginTabIndex"
          />
          <label>Email</label>
        </div>
        <!-- 密碼 -->
        <div class="input-box">
          <span class="icon-eye" @click="togglePasswordVisibility('loginPassword')">
            <i :class="loginPasswordType === 'password' ? 'fas fa-eye' : 'fas fa-eye-slash'"></i>
          </span>
          <span class="icon"><i class="fas fa-lock"></i></span>
          <input
            :type="loginPasswordType"
            class="mx-2"
            required
            v-model="loginPassword"
            autocomplete="off"
            :tabindex="loginTabIndex"
          />
          <label>Password</label>
        </div>

        <!-- 記住我 -->
        <div class="remember-forgot">
          <label
            ><input type="checkbox" v-model="rememberMe" :tabindex="loginTabIndex" /> 記住我
          </label>
          <!-- 忘記密碼 -->
          <a href="#" :tabindex="loginTabIndex">忘記密碼?</a>
        </div>
        <!-- 登入按鈕 -->
        <button type="submit" class="btn" :tabindex="loginTabIndex">登入</button>
        <!-- 註冊按鈕 -->
        <div class="login-register">
          <p>
            沒有帳戶?
            <a href="#" @click.prevent="showRegister" :tabindex="loginTabIndex">會員註冊</a>
          </p>
        </div>
      </form>
    </div>

    <div class="form-box register" ref="registerForm">
      <h2>會員註冊</h2>
      <form @submit.prevent="submitRegister">
        <!-- 名字 -->
        <div class="input-box">
          <span class="icon"><i class="fa fa-user" aria-hidden="true"></i></span>
          <input
            type="text"
            class="mx-2"
            id="name"
            required
            v-model="name"
            autocomplete="off"
            :tabindex="registerTabIndex"
            @input="validatename"
          />
          <label>Name</label>
          <div id="nameFeedback" class="invalid-feedback">此為必填欄位</div>
        </div>
        <!-- 信箱 -->
        <div class="input-box mt-5">
          <span class="icon"><i class="fas fa-envelope"></i></span>
          <input
            type="email"
            class="mx-2"
            id="email"
            required
            v-model="email"
            autocomplete="off"
            :tabindex="registerTabIndex"
            @input="checkEmail"
          />
          <label>Email</label>
          <div id="emailFeedback" class="invalid-feedback mb-3"></div>
        </div>
        <!-- 密碼 -->
        <div class="input-box mt-5">
          <span class="icon-eye" @click="togglePasswordVisibility('password')">
            <i :class="passwordType === 'password' ? 'fas fa-eye' : 'fas fa-eye-slash'"></i>
          </span>
          <span class="icon"><i class="fas fa-lock"></i></span>
          <input
            :type="passwordType"
            class="mx-2"
            id="Password"
            required
            v-model="password"
            autocomplete="off"
            :tabindex="registerTabIndex"
            @input="validatePasswords"
          />
          <label>Password</label>
        </div>
        <!-- 確認密碼 -->
        <div class="input-box">
          <span class="icon-eye" @click="togglePasswordVisibility('confirmPassword')">
            <i :class="confirmPasswordType === 'password' ? 'fas fa-eye' : 'fas fa-eye-slash'"></i>
          </span>
          <span class="icon"><i class="fas fa-lock"></i></span>
          <input
            :type="confirmPasswordType"
            class="mx-2"
            id="ConfirmPassword"
            required
            v-model="confirmPassword"
            autocomplete="off"
            :tabindex="registerTabIndex"
            @input="validatePasswords"
          />
          <label>Confirm Password</label>
          <div id="passwordMismatchFeedback" class="invalid-feedback">密碼與確認密碼不一致</div>
        </div>
        <!-- 註冊前確認規定同意書 -->
        <div class="remember-forgot mt-3">
          <label
            ><input type="checkbox" required :tabindex="registerTabIndex" v-model="agree" />
            我同意條款與條件
          </label>
        </div>
        <!-- 註冊按鈕 -->
        <button
          type="submit"
          class="btn"
          :tabindex="registerTabIndex"
          :disabled="!isFirstPartValid"
        >
          Register
        </button>
        <!-- 登入按鈕 -->
        <div class="login-register">
          <p>
            已經有帳戶了嗎?
            <a href="#" @click.prevent="showLogin" :tabindex="registerTabIndex"> 會員登入 </a>
          </p>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import $ from 'jquery'
// 使用 Pinia，利用 store 去訪問狀態
import { useIdentityStore } from '@/stores/identity.js'
const store = useIdentityStore()

import { ref, onMounted, computed } from 'vue'
import axios from 'axios'

// 特殊吐司
import { toast } from 'vue3-toastify'
import 'vue3-toastify/dist/index.css'

// 從環境變數取得 API BASE URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

const LRModal = ref(null)
const loginEmail = ref('')
const loginPassword = ref('')
const rememberMe = ref(false)

const name = ref('')
const email = ref('')
const emailExists = ref(false) //確認電子信箱是否重複
const password = ref('')
const confirmPassword = ref('')
const agree = ref(false)
const loginForm = ref(null)
const registerForm = ref(null)
const loginTabIndex = ref(0)
const registerTabIndex = ref(-1)

const loginPasswordType = ref('password')
const passwordType = ref('password')
const confirmPasswordType = ref('password')

const closeModal = () => {
  // 隱藏登入畫面
  store.hide()
}

const showLogin = () => {
  clearForm()
  if (LRModal.value && loginForm.value && registerForm.value) {
    LRModal.value.classList.remove('active')
    setTimeout(() => {
      loginForm.value.style.transform = 'translateX(0)'
      registerForm.value.style.transform = 'translateX(400px)'
      LRModal.value.style.height = '500px'
      loginTabIndex.value = 0
      registerTabIndex.value = -1
    }, 200)
  }
}

const showRegister = () => {
  clearForm()
  if (LRModal.value && loginForm.value && registerForm.value) {
    LRModal.value.classList.add('active')
    setTimeout(() => {
      loginForm.value.style.transform = 'translateX(-400px)'
      registerForm.value.style.transform = 'translateX(0)'
      LRModal.value.style.height = '600px'
      loginTabIndex.value = -1
      registerTabIndex.value = 0
    }, 200)
  }
}

//確認電子信箱是否重複
const checkEmail = async () => {
  const emailValue = email.value
  if (emailValue) {
    try {
      const response = await axios.get(`${apiUrl}/api/UserIdentity/CheckEmailExists`, {
        params: { email: email.value }
      })
      emailExists.value = !response.data
      const feedbackElement = $('#emailFeedback')
      if (response.data) {
        feedbackElement
          .text('此Email 可以使用')
          .removeClass('invalid-feedback')
          .addClass('valid-feedback')
        $('#email').removeClass('is-invalid').addClass('is-valid')
      } else {
        feedbackElement
          .text('該 Email 已被使用，請更換Email')
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

//cookie登入
// const submitLogin = async () => {
//   try {
//     // 將輸入的密碼進行加密
//     const hashedPassword = await hashPassword(loginPassword.value)

//     // 構建登入請求的資料
//     const loginData = {
//       email: loginEmail.value,
//       password: hashedPassword
//     }

//     // 發送 POST 請求到後端 API
//     const response = await axios.post(`${apiUrl}/api/UserIdentity/LoginCookie`, loginData)
//     console.log('Response received:', response)

//     // 檢查回應狀態和數據
//     if (response.status === 200 && response.data.includes('登入成功')) {
//       const message = response.data // 從回應中提取 message

//       console.log('Login successful:', message)

//       // 顯示登入成功訊息或進行頁面跳轉
//       alert(message) // 使用回應中的訊息
//       closeModal()
//     } else {
//       console.log('Unexpected response structure:', response)
//       alert('登入過程中發生錯誤：無法獲取 cookie')
//     }
//   } catch (error) {
//     // 處理錯誤情況，例如顯示錯誤消息
//     console.error('Error occurred during login:', error)
//     if (error.response && error.response.status === 400) {
//       alert('帳號或密碼錯誤')
//     } else {
//       alert('登入過程中發生錯誤')
//     }
//   }
// }

//Jwt登入
const submitLogin = () => {
  // 將輸入的密碼進行加密
  hashPassword(loginPassword.value)
    .then((hashedPassword) => {
      // 構建登入請求的資料
      const loginData = {
        email: loginEmail.value,
        password: hashedPassword
      }

      // 發送 POST 請求到後端 API
      return axios.post(`${apiUrl}/api/UserIdentity/JwtLogin`, loginData)
    })
    .then((response) => {
      console.log('IdentityModal:', response)

      // 檢查回應狀態和數據
      if (response.status === 200 && response.data.token) {
        const { token, message } = response.data // 從回應中提取 token 和 message

        // console.log('Login successful:', message)
        // console.log('Token:', token)

        // 儲存 token 到 Pinia store
        store.setToken(token)

        // 紀錄登入狀態
        store.Login()

        // 儲存至 SessionStorage ~ 記住登入(可關閉瀏覽器)
        sessionStorage.setItem('token', token)
        // localStorage.setItem('token', token);

        // 顯示登入成功訊息或進行頁面跳轉
        // alert(message) // 使用回應中的訊息

        toast.success(message, {
          theme: 'colored',
          autoClose: 1000,
          transition: toast.TRANSITIONS.ZOOM,
          position: toast.POSITION.BOTTOM_CENTER
        })

        closeModal()
      } else {
        console.log('Unexpected response structure:', response)
        // alert('登入過程中發生錯誤：無法獲取 token')
        toast.error('登入失敗：無法取得會員資料', {
          theme: 'dark',
          autoClose: 1000,
          transition: toast.TRANSITIONS.ZOOM,
          position: toast.POSITION.BOTTOM_CENTER
        })
      }
    })
    .catch((error) => {
      // 處理錯誤情況，例如顯示錯誤消息
      console.error('Error occurred during login:', error)
      if (error.response && error.response.status === 400) {
        // alert('帳號或密碼錯誤')
        toast.error('帳號或密碼錯誤', {
          theme: 'colored',
          autoClose: 1000,
          transition: toast.TRANSITIONS.ZOOM,
          // position: toast.POSITION.TOP_CENTER
          position: toast.POSITION.BOTTOM_CENTER
        })
      } else {
        // alert('登入過程中發生錯誤')
        toast.error('登入失敗，請重新登入', {
          theme: 'colored',
          autoClose: 1000,
          transition: toast.TRANSITIONS.ZOOM,
          position: toast.POSITION.BOTTOM_CENTER
        })
      }
    })
}

// JavaScript 實現 SHA-256 加密
async function hashPassword(password) {
  const encoder = new TextEncoder()
  const data = encoder.encode(password)
  const hash = await crypto.subtle.digest('SHA-256', data)
  return btoa(String.fromCharCode(...new Uint8Array(hash)))
}

//驗證第一部份表單是否有誤
const isFirstPartValid = computed(() => {
  return (
    name.value &&
    email.value &&
    password.value &&
    confirmPassword.value &&
    password.value === confirmPassword.value &&
    !emailExists.value
  )
})

//清空表單
const clearForm = () => {
  loginEmail.value = ''
  loginPassword.value = ''
  confirmPassword.value = ''
  rememberMe.value = false
  name.value = ''
  email.value = ''
  password.value = ''
  agree.value = false
}

const submitRegister = async () => {
  // 檢查表單是否有效
  if (!isFirstPartValid.value) {
    // alert('請填寫完整且正確的表單資料')
    toast.error('請填寫完整且正確的表單資料', {
        theme: 'colored',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.BOTTOM_CENTER
    })
    return
  }

  // 檢查密碼和確認密碼是否一致
  if (password.value !== confirmPassword.value) {
    // alert('密碼與確認密碼不一致')
      toast.error('密碼與確認密碼不一致', {
      theme: 'colored',
      autoClose: 1000,
      transition: toast.TRANSITIONS.ZOOM,
      position: toast.POSITION.BOTTOM_CENTER
    })
    return
  }

  // 構建註冊請求的資料
  const formData = new FormData()
  formData.append('Name', name.value)
  formData.append('Password', password.value)
  formData.append('ConfirmPassword', confirmPassword.value)
  formData.append('Email', email.value)

  try {
    // 發送 POST 請求到後端 API
    // const response = await axios.post(`${apiUrl}/api/MemberManagement/CreateMember`, formData) //後端使用的API，ViewModel有非必填欄位
    const response = await axios.post(`${apiUrl}/api/UserIdentity/CreateMember`, formData) //前端使用的API，ViewModel移除非必填欄位

    // 檢查回應狀態和數據
    if (response.status === 200 && response.data.success) {
      // alert(response.data.message)
        toast.success(response.data.message, {
        theme: 'colored',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.BOTTOM_CENTER
      })
      showLogin()
    } else {
      // alert(response.data.message || '註冊過程中發生錯誤')
        toast.error(response.data.message || '註冊失敗', {
        theme: 'colored',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.BOTTOM_CENTER
      })
    }
  } catch (error) {
    // 處理錯誤情況，例如顯示錯誤消息
    console.error('Error occurred during registration:', error)
    // alert('註冊過程中發生錯誤')
      toast.error('註冊失敗', {
      theme: 'colored',
      autoClose: 1000,
      transition: toast.TRANSITIONS.ZOOM,
      position: toast.POSITION.BOTTOM_CENTER
    })
  }
}

const togglePasswordVisibility = (field) => {
  if (field === 'loginPassword') {
    loginPasswordType.value = loginPasswordType.value === 'password' ? 'text' : 'password'
  } else if (field === 'password') {
    passwordType.value = passwordType.value === 'password' ? 'text' : 'password'
  } else if (field === 'confirmPassword') {
    confirmPasswordType.value = confirmPasswordType.value === 'password' ? 'text' : 'password'
  }
}

onMounted(() => {
  showLogin()
})
</script>

<style scoped>
.LRModal {
  position: fixed;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  z-index: 10000;
  width: 400px;
  height: 500px;
  background: rgba(0, 0, 0, 0.6);
  border: 2px solid rgba(255, 255, 255, 0.5);
  border-radius: 20px;
  backdrop-filter: blur(20px);
  box-shadow: 0 0 30px rgba(255, 255, 255, 0.1);
  display: flex;
  justify-content: center;
  align-items: center;
  overflow: hidden;
  transition:
    height 0.3s ease,
    transform 0.5s ease;
}

.LRModal .form-box {
  width: 100%;
  padding: 40px;
  position: absolute;
}

.LRModal .form-box.login {
  transition: transform 0.2s ease;
  transform: translateX(0);
}

.LRModal.active .form-box.login {
  transform: translateX(-400px);
}

.LRModal .form-box.register {
  transition: transform 0.2s ease;
  transform: translateX(400px);
}

.LRModal.active .form-box.register {
  transform: translateX(0);
}

.LRModal .icon-close {
  position: absolute;
  top: 0;
  right: 0;
  width: 45px;
  height: 45px;
  background: #162938;
  font-size: 2em;
  color: white;
  display: flex;
  justify-content: center;
  align-items: center;
  border-bottom-left-radius: 20px;
  cursor: pointer;
  z-index: 1000;
}

.form-box h2 {
  font-size: 2em;
  color: white;
  text-align: center;
}

.input-box {
  position: relative;
  width: 100%;
  height: 50px;
  border-bottom: 2px solid white;
  margin: 30px 0;
}

.input-box label {
  position: absolute;
  top: 50%;
  left: 5px;
  transform: translateY(-50%);
  font-size: 1em;
  color: white;
  font-weight: 500;
  pointer-events: none;
  transition: 0.5s;
}

.input-box input:focus ~ label,
.input-box input:valid ~ label {
  top: -5px;
}

.input-box input {
  width: 100%;
  height: 100%;
  background: transparent;
  border: none;
  outline: none;
  font-size: 1em;
  color: white;
  font-weight: 600;
  padding: 0 35px 0 5px;
}

.input-box .icon {
  position: absolute;
  right: 8px;
  color: white;
  font-size: 1.2em;
  line-height: 57px;
}

.remember-forgot {
  font-size: 0.9em;
  color: white;
  font-weight: 500;
  margin: -15px 0 15px;
  display: flex;
  justify-content: space-between;
}

.remember-forgot label input {
  accent-color: white;
  margin-right: 3px;
}

.remember-forgot a {
  color: white;
  text-decoration: none;
}

.remember-forgot a:hover {
  text-decoration: underline;
}

.btn {
  width: 100%;
  height: 45px;
  background: white;
  border: none;
  outline: none;
  border-radius: 6px;
  cursor: pointer;
  font-size: 1em;
  color: black;
  font-weight: 500;
}

.login-register {
  font-size: 0.9em;
  color: white;
  text-align: center;
  font-weight: 500;
  margin: 25px 0 10px;
}

.login-register p a {
  text-decoration: none;
  color: white;
  font-weight: 600;
}

.login-register p a:hover {
  text-decoration: underline;
}

.input-box .icon-eye {
  position: absolute;
  right: 35px; /* 調整眼睛圖標的位置 */
  color: white;
  font-size: 1.2em;
  line-height: 57px;
  cursor: pointer;
}
</style>
