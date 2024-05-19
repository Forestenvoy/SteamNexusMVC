<template>
  <div class="LRModal" ref="LRModal" :style="{ display: showLogin ? 'block' : 'none' }">
    <span class="icon-close" @click="closeModal">
      <i class="fa fa-times" aria-hidden="true"></i>
    </span>

    <div class="form-box login" ref="loginForm">
      <h2>Login</h2>
      <form @submit.prevent="submitLogin">
        <!-- 信箱 -->
        <div class="input-box">
          <span class="icon"><i class="fas fa-envelope"></i></span>
          <input type="text" class="mx-2" required v-model="loginemail" />
          <label>Email</label>
        </div>
        <!-- 密碼 -->
        <div class="input-box">
          <span class="icon"><i class="fas fa-lock"></i></span>
          <input type="password" class="mx-2" required v-model="loginpassword" />
          <label>Password</label>
        </div>
        <!-- 記住我 -->
        <div class="remember-forgot">
          <label><input type="checkbox" v-model="rememberMe" /> Remember me</label>
          <!-- 忘記密碼 -->
          <a href="#">Forgot password?</a>
        </div>
        <!-- 登入按鈕 -->
        <button type="submit" class="btn">Login</button>
        <!-- 註冊按鈕 -->
        <div class="login-register">
          <p>Don't have an account?<a href="#" @click.prevent="showRegister"> Register</a></p>
        </div>
      </form>
    </div>

    <div class="form-box register" ref="registerForm">
      <h2>Registration</h2>
      <form @submit.prevent="submitRegister">
        <!-- 名字 -->
        <div class="input-box">
          <span class="icon"><i class="fa fa-user" aria-hidden="true"></i></span>
          <input type="text" class="mx-2" required v-model="registername" />
          <label>Name</label>
        </div>
        <!-- 信箱 -->
        <div class="input-box">
          <span class="icon"><i class="fas fa-envelope"></i></span>
          <input type="text" class="mx-2" required v-model="registeremail" />
          <label>Email</label>
        </div>
        <!-- 密碼 -->
        <div class="input-box">
          <span class="icon"><i class="fas fa-lock"></i></span>
          <input type="password" class="mx-2" required v-model="registerpassword" />
          <label>Password</label>
        </div>
        <!-- 確認密碼 -->
        <div class="input-box">
          <span class="icon"><i class="fas fa-lock"></i></span>
          <input type="password" class="mx-2" required v-model="confirmPassword" />
          <label>Confirm Password</label>
        </div>
        <!-- 註冊前確認規定同意書 -->
        <div class="remember-forgot">
          <label><input type="checkbox" required /> I agree to the terms & conditions</label>
        </div>
        <!-- 註冊按鈕 -->
        <button type="submit" class="btn">Register</button>
        <!-- 登入按鈕 -->
        <div class="login-register">
          <p>Already have an account?<a href="#" @click.prevent="showLoginModel"> Login</a></p>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'

const LRModal = ref(null)
const loginemail = ref('')
const loginpassword = ref('')
const confirmPassword = ref('')
const rememberMe = ref(false)
const registername = ref('')
const registeremail = ref('')
const registerpassword = ref('')
const loginForm = ref(null)
const registerForm = ref(null)
const showLogin = ref(true)

const closeModal = () => {
  if (LRModal.value) {
    LRModal.value.style.display = 'none'
  }
}

const showLoginModel = () => {
  if (LRModal.value && loginForm.value && registerForm.value) {
    LRModal.value.classList.remove('active')
    setTimeout(() => {
      loginForm.value.style.transform = 'translateX(0)'
      registerForm.value.style.transform = 'translateX(400px)'
      LRModal.value.style.height = '500px'
    }, 200)
  }
}

const showRegister = () => {
  if (LRModal.value && loginForm.value && registerForm.value) {
    LRModal.value.classList.add('active')
    setTimeout(() => {
      loginForm.value.style.transform = 'translateX(-400px)'
      registerForm.value.style.transform = 'translateX(0)'
      LRModal.value.style.height = '600px'
    }, 200)
  }
}

const submitLogin = () => {
  // 處理登入表單提交
}

const submitRegister = () => {
  // 處理註冊表單提交
}

onMounted(() => {
  showLogin() // 預設顯示登入表單
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
    transform 0.3s ease;
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
</style>
