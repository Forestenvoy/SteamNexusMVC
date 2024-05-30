<template>
  <router-view></router-view>
</template>

<script setup>
import { onBeforeMount, onMounted } from 'vue'
import { useColorModes } from '@coreui/vue'

import { useThemeStore } from '@/stores/theme.js'

import { useIdentityStore } from '@/stores/identity.js'
const authStore = useIdentityStore()

const { isColorModeSet, setColorMode } = useColorModes('coreui-free-vue-admin-template-theme')
const currentTheme = useThemeStore()

onBeforeMount(() => {
  const urlParams = new URLSearchParams(window.location.href.split('?')[1])
  let theme = urlParams.get('theme')

  if (theme !== null && theme.match(/^[A-Za-z0-9\s]+/)) {
    theme = theme.match(/^[A-Za-z0-9\s]+/)[0]
  }

  if (theme) {
    setColorMode(theme)
    return
  }

  if (isColorModeSet()) {
    return
  }

  setColorMode(currentTheme.theme)
})

onMounted(() => {
  const loadingAnimation = document.getElementById('AnimationContainer')
  loadingAnimation.classList.add('animate__animated', 'animate__fadeOut')
  setTimeout(() => {
    loadingAnimation.remove()
    const animationCSS = document.querySelector('link[href="/css/webAnimation.css"]')
    animationCSS.remove()
    const animationJS = document.querySelector('script[src="/js/webAnimation.js"]')
    animationJS.remove()
  }, 2100)

  // 從 session storage 獲取 token
  const token = sessionStorage.getItem('token')

  if (token) {
    // 儲存 token 到 Pinia store
    authStore.setToken(token)
    // 紀錄登入狀態
    authStore.Login()
  }
})
</script>

<style lang="scss">
// 字體變更
@import url('https://fonts.googleapis.com/css2?family=Poppins:ital,wght@0,100;0,200;0,300;0,400;0,500;0,600;0,700;0,800;0,900;1,100;1,200;1,300;1,400;1,500;1,600;1,700;1,800;1,900&display=swap');
@import url('https://fonts.googleapis.com/css?family=Noto+Sans+TC&display=swap');
// cwTeXYen (Chinese Traditional) 圓體字體
@import url(https://fonts.googleapis.com/earlyaccess/cwtexyen.css);
$font-family-sans-serif: 'Poppins', 'cwTeXYen';

// 深色模式
// sidebar、header、footer 背景色
$body-bg-dark: #313131;
// body 背景色
$dark-bg-subtle-dark: #000000;

// 取消 toggle icon 避免 CSP
$navbar-light-toggler-icon-bg: none;
$navbar-dark-toggler-icon-bg: none;
$header-toggler-icon-bg: none;
$header-toggler-hover-icon-bg: none;

// Import Main styles for this application
@import 'styles/style';
</style>

<style>
/* 滾動條樣式 */
::-webkit-scrollbar {
  width: 12px;
}

::-webkit-scrollbar-track {
  border-radius: 8px;
  background-color: transparent;
  box-shadow: inset 0 0 6px rgba(0, 0, 0, 0.3);
}

::-webkit-scrollbar-thumb {
  border-radius: 8px;
  background-color: #363636;
}
</style>
