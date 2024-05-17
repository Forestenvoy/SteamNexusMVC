<template>
  <div class="LRModal">
    <div class="form-box login">
      <h2>Login</h2>
      <form action="#">
        <!-- 信箱 -->
        <div class="input-box">
          <span class="icon"><i class="fas fa-envelope"></i></span>
          <input type="text" class="mx-2" required />
          <label>Email</label>
        </div>
        <!-- 密碼 -->
        <div class="input-box">
          <span class="icon"><i class="fas fa-lock"></i></span>
          <input type="text" class="mx-2" required />
          <label>Password</label>
        </div>
        <!-- 記住我 -->
        <div class="remember-forgot">
          <label><input type="checkbox" /> Remember me</label>
          <!-- 忘記密碼 -->
          <a href="#">Forgot password?</a>
        </div>
        <!-- 登入按鈕 -->
        <button type="submit" class="btn">Login</button>
        <!-- 註冊按鈕 -->
        <div class="login-register">
          <p>Don't have an account?<a href="#" class="register-link">Register</a></p>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup></script>

<style scoped>
.LRModal {
  position: fixed;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  z-index: 10000;
  width: 400px;
  height: 400px;
  background: transparent;
  border: 2px solid rgba(255, 255, 255, 0.5);
  border-radius: 20px;
}
</style>
