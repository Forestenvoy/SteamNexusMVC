import { createRouter, createWebHistory } from 'vue-router'

import FrontendLayout from '@/views/frontend/FrontendLayout.vue'

// const BackendLayout = () => import('@/views/backend/BackendLayout.vue')
import BackendLayout from '@/views/backend/BackendLayout.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    // 後台系統主頁面
    {
      path: '/admin',
      component: BackendLayout,
      name: 'BackendLayout'
    },
    // 前台系統首頁
    {
      path: '/home',
      component: FrontendLayout
    },
    // 自動將網域導向首頁
    {
      path: '/',
      redirect: '/home'
    }
  ]
})

export default router
