// Vue Router
import { createRouter, createWebHistory } from 'vue-router'

// 前台系統
const FrontendLayout = () => import('@/layouts/FrontendLayout.vue')

// 後台系統
const BackendLayout = () => import('@/layouts/BackendLayout.vue')
// 會員資料管理
const MemberManage = () => import('@/views/backend/MemberManage.vue')
// 權限管理
const RoleManage = () => import('@/views/backend/RoleManage.vue')
// 硬體產品管理
const ProductManage = () => import('@/views/backend/ProductManage.vue')
// 硬體菜單管理
const MenuManage = () => import('@/views/backend/MenuManage.vue')
// 遊戲資料管理
const GameManage = () => import('@/views/backend/GameManage.vue')
// 廣告資料管理
const AdManage = () => import('@/views/backend/AdManage.vue')

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    // 後台系統(巢狀路由)
    {
      path: '/admin',
      component: BackendLayout,
      name: 'admin',
      children: [
        // 會員資料管理
        {
          path: 'memberManage',
          name: 'MemberManage',
          component: MemberManage
        },
        // 權限管理
        {
          path: 'roleManage',
          name: 'RoleManage',
          component: RoleManage
        },
        // 硬體產品管理
        {
          path: 'productManage',
          name: 'ProductManage',
          component: ProductManage
        },
        // 硬體菜單管理
        {
          path: 'menuManage',
          name: 'MenuManage',
          component: MenuManage
        },
        // 遊戲資料管理
        {
          path: 'gameManage',
          name: 'GameManage',
          component: GameManage
        },
        // 廣告資料管理
        {
          path: 'adManage',
          name: 'AdManage',
          component: AdManage
        }
      ]
    },
    // 前台系統
    {
      path: '/',
      component: FrontendLayout
    },
    // 無法辨別的路由，自動導向首頁
    {
      path: '/:notFound(.*)',
      redirect: '/'
    }
  ]
})

export default router
