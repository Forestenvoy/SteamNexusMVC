import { createRouter, createWebHistory } from 'vue-router'

const BackendLayout = () => import('@/views/backend/BackendLayout.vue')

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    // 後台系統主頁面
    {
      path: '/admin',
      component: BackendLayout
    },
    // 自動將網域導向首頁
    {
      path: '/',
      redirect: '/admin'
    }
  ]
})

export default router
