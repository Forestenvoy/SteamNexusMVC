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
  if (to.path.startsWith('/admin')) {
    // 後台驗證是否登入管理員
    if (identityStore.getUserRole === 'Admin') {
      next()
    } else {
      next('/')
    }
  } else if (to.path.startsWith('/userData') || to.path.startsWith('/trackedGames')) {
    // 會員驗證是否登入會員
    if (identityStore.getUserRole === 'Member' || identityStore.getUserRole === 'Admin') {
      next()
    } else {
      next('/')
    }
  } else {
    next()
  }
})

app.mount('#app')
