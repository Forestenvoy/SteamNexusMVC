<template>
  <div class="LRModal">
    <span class="icon-close" @click="hideLogin">
      <i class="fa fa-times" aria-hidden="true"></i>
    </span>

    <div class="form-box login">
      <h2>Login</h2>
      <form @submit.prevent="login">
        <div class="input-box">
          <span class="icon"><i class="fas fa-envelope"></i></span>
          <input type="email" v-model="loginEmail" class="mx-2" required />
          <label>Email</label>
        </div>
        <div class="input-box">
          <span class="icon"><i class="fas fa-lock"></i></span>
          <input type="password" v-model="loginPassword" class="mx-2" required />
          <label>Password</label>
        </div>
        <div class="remember-forgot">
          <label><input type="checkbox" v-model="rememberMe" /> Remember me</label>
          <a href="#">Forgot password?</a>
        </div>
        <button type="submit" class="btn">Login</button>
        <div class="login-register">
          <p>Don't have an account?<a href="#" class="register-link"> Register</a></p>
        </div>
      </form>
    </div>

    <div class="form-box register">
      <h2>Registration</h2>
      <form @submit.prevent="register">
        <div class="input-box">
          <span class="icon"><i class="fa fa-user" aria-hidden="true"></i></span>
          <input type="text" v-model="registerName" class="mx-2" required />
          <label>Name</label>
        </div>
        <div class="input-box">
          <span class="icon"><i class="fas fa-envelope"></i></span>
          <input type="email" v-model="registerEmail" class="mx-2" required />
          <label>Email</label>
        </div>
        <div class="input-box">
          <span class="icon"><i class="fas fa-lock"></i></span>
          <input type="password" v-model="registerPassword" class="mx-2" required />
          <label>Password</label>
        </div>
        <div class="input-box">
          <span class="icon"><i class="fas fa-lock"></i></span>
          <input type="password" v-model="confirmPassword" class="mx-2" required />
          <label>Confirm Password</label>
        </div>
        <div class="remember-forgot">
          <label
            ><input type="checkbox" v-model="agreeToTerms" required /> I agree to the terms &
            conditions</label
          >
        </div>
        <button type="submit" class="btn">Register</button>
        <div class="login-register">
          <p>Already have an account?<a href="#" class="login-link"> Login</a></p>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'

const loginEmail = ref('')
const loginPassword = ref('')
const rememberMe = ref(false)
const registerName = ref('')
const registerEmail = ref('')
const registerPassword = ref('')
const confirmPassword = ref('')
const agreeToTerms = ref(false)

const hideLogin = () => {
  document.querySelector('.LRModal').style.display = 'none'
}

const login = () => {
  // 登入邏輯
}

const register = () => {
  // 註冊邏輯
}

onMounted(() => {
  const LRModal = document.querySelector('.LRModal')
  const loginLink = document.querySelector('.login-link')
  const registerLink = document.querySelector('.register-link')

  if (registerLink) {
    registerLink.addEventListener('click', () => {
      LRModal.classList.add('active')
      LRModal.style.height = '600px'
    })
  }

  if (loginLink) {
    loginLink.addEventListener('click', () => {
      LRModal.classList.remove('active')
      LRModal.style.height = '400px'
    })
  }
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
  background: transparent;
  border: 2px solid rgba(255, 255, 255, 0.5);
  border-radius: 20px;
  backdrop-filter: blur(20px);
  box-shadow: 0 0 30px rgba(255, 255, 255, 10);
  display: flex;
  justify-self: center;
  align-items: center;
  overflow: hidden;
  /* transform: scale(0); */
  transition: height 0.2s ease;
}

.LRModal .form-box {
  width: 100%;
  padding: 40px;
}

.LRModal .form-box.login {
  transition: transform 0.18s ease;
  transform: translateX(0);
}

.LRModal.active .form-box.login {
  transform: none;
  transform: translateX(-400px);
}

.LRModal .form-box.register {
  position: absolute;
  transition: none;
  transform: translate(400px);
}

.LRModal.active .form-box.register {
  transition: transform 0.18s ease;
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
