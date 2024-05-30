import { createApp } from 'vue'
import { createPinia } from 'pinia'
import App from './App.vue'
import router from './router/index.js'
// 導入身分驗證
import { useIdentityStore } from '@/stores/identity.js'

import CoreuiVue from '@coreui/vue'
import CIcon from '@coreui/icons-vue'
import { iconsSet as icons } from '@/assets/icons'

import 'jquery'

const app = createApp(App)
const pinia = createPinia()
app.use(pinia)
app.use(router)
app.use(CoreuiVue)
app.provide('icons', icons)
app.component('CIcon', CIcon)

// 路由守衛
router.beforeEach((to, from, next) => {
  const identityStore = useIdentityStore()
  // 如果事後台
  if (to.path.startsWith('/admin')) {
    // 驗證是否登入管理員
    if (identityStore.getUserRole === 'Admin') {
      next()
    } else {
      next('/')
    }
  } else {
    console.log('Hello Front')
    next()
  }
})

app.mount('#app')
