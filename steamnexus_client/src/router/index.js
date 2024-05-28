// Vue Router
import { createRouter, createWebHistory } from 'vue-router'

// 前台系統
const FrontendLayout = () => import('@/layouts/FrontendLayout.vue')
// 首頁
const HomePage = () => import('@/views/frontend/HomePage.vue')

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
  history: createWebHistory(import.meta.env.VITE_APP_VUE_BASE_URL),
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
      component: FrontendLayout,
      name: 'front',
      children: [
        // 首頁
        {
          path: '',
          name: 'Home',
          component: HomePage
        },
        // 硬體估價
        {
          path: 'pc-builder',
          name: 'ComputerBuilder',
          component: () => import('@/views/frontend/ComputerBuilder.vue')
        },
        // 會員資料變更
        {
          path: 'userData',
          name: 'UserData',
          component: () => import('@/views/frontend/UserData.vue')
        },
        // 我的追蹤遊戲
        {
          path: 'trackedGames',
          name: 'TrackedGames',
          component: () => import('@/views/frontend/TrackedGames.vue')
        },
        // 搜尋頁面
        {
          path: 'searchSystem',
          name: 'SearchSystem',
          component: () => import('@/views/frontend/SearchSystem.vue')
        },
        // 遊戲頁面
        {
          path: 'game/:gameId',
          name: 'Game',
          component: () => import('@/views/frontend/GameInformation.vue'),
          props: true
        }
      ]
    },
    // 無法辨別的路由，自動導向首頁
    {
      path: '/:notFound(.*)',
      redirect: '/'
    }
  ],
  scrollBehavior() {
    // 始終滾動到頂部
    return { top: 0 }
  }
})

export default router
