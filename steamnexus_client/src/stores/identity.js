import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
// 解析 JWT Token
import { jwtDecode } from 'jwt-decode'

export const useIdentityStore = defineStore('identity', () => {
  // 是否顯示登入互動視窗
  // state
  const showLogin = ref(false)
  // getter
  const getShowLogin = computed(() => showLogin.value)
  // actions
  const show = () => {
    // 顯示登入畫面
    showLogin.value = true
  }
  const hide = () => {
    // 隱藏登入畫面
    showLogin.value = false
  }

  // JWT Token
  // state
  const token = ref('')
  // getter
  const getToken = computed(() => token.value)
  // actions
  const setToken = (value) => {
    token.value = value
  }

  // 使用者是否登入
  // state
  const isLogin = ref(false)
  // getter
  const getIsLogin = computed(() => isLogin.value)
  // actions
  const Login = () => {
    // 登入
    isLogin.value = true
  }
  const Logout = () => {
    // 登出
    isLogin.value = false
    token.value = ''
  }

  // 使用者權限
  // state
  const userRole = ref('')
  // getter
  const getUserRole = computed(() => {
    if (token.value) {
      try {
        const decodedToken = jwtDecode(token.value)
        userRole.value =
          decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
      } catch (error) {
        console.error('Invalid token:', error)
        userRole.value = ''
      }
    } else {
      userRole.value = ''
    }
    return userRole.value
  })

  // 使用者大頭照
  // state
  const userPhoto = ref('')
  // getter
  const getUserPhoto = computed(() => {
    if (token.value) {
      try {
        const decodedToken = jwtDecode(token.value)
        userPhoto.value =
          decodedToken['image']
      } catch (error) {
        console.error('Invalid token:', error)
        userPhoto.value = ''
      }
    } else {
      userPhoto.value = ''
    }
    return userPhoto.value
  })




  return { getShowLogin, show, hide, getToken, setToken, getUserRole,getUserPhoto , getIsLogin, Login, Logout }
})
