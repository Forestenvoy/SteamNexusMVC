import { ref, computed } from 'vue'
import { defineStore } from 'pinia'

export const useIdentityStore = defineStore('identity', () => {
  // 宣告進度條顯示狀態
  const showLogin = ref(false)

  // 回傳登入畫面顯示狀態
  const getShowLogin = computed(() => showLogin.value)

  // 顯示登入畫面
  const show = () => {
    showLogin.value = true
  }

  // 隱藏登入畫面
  const hide = () => {
    showLogin.value = false
  }

  return { showLogin, getShowLogin, show, hide }
})
