<template>
  <div>
    <AppSidebar />
    <div class="wrapper d-flex flex-column min-vh-100">
      <AppHeader />
      <div class="body flex-grow-1">
        <CContainer class="px-0">
          <!-- 進度條元件 -->
          <transition name="p_slide">
            <web-scraper-progress
              class="p_element"
              v-if="store.getState"
              :update-type="store.getType"
            ></web-scraper-progress>
          </transition>
          <!-- 後台子系統 -->
          <router-view></router-view>
        </CContainer>
      </div>
      <AppFooter />
    </div>
  </div>
</template>

<script setup>
import { CContainer } from '@coreui/vue'
import AppFooter from '@/components/backend/share/AppFooter.vue'
import AppHeader from '@/components/backend/share/AppHeader.vue'
import AppSidebar from '@/components/backend/share/AppSidebar.vue'
import WebScraperProgress from '@/components/backend/hardware/WebScraperProgress.vue'

// 使用 Pinia
import { useScraperStore } from '@/stores/scraper.js'
// 利用 store 去訪問狀態 ✨
const store = useScraperStore()

// // 從環境變數取得 API BASE URL
// const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

// // 檢測零件更新 是否有使用者正在操作
// function CheckScraperState() {
//   // 發送非同步POST請求 ==> 檢測是否有使用者在做更新操作
//   // 返回 fetch 的 Promise
//   return fetch(`${apiUrl}/api/HardwareManage/IsHardwareUpdate`, {
//     method: 'GET'
//   })
//     .then((response) => {
//       if (!response.ok) {
//         throw new Error('NetworkError')
//       }
//       return response.text()
//     })
//     .then((data) => {
//       return data
//     })
//     .catch((error) => {
//       throw error
//     })
// }

// // 單一零件更新
// function UpdateOneHardware(hardwareId, productType) {
//   // 檢測零件更新 是否有使用者正在操作
//   CheckScraperState()
//     .then((state) => {
//       // 在这里处理 state 的值
//       console.log(state)

//       if (state === 'false') {
//         // 啟動進度條
//         UpdateType.value = productType
//         scraperState.value = true
//         // 發送非同步POST請求 ==> 資料庫資料變更
//         var data = {
//           Type: hardwareId
//         }
//         // 確定無人操作，才進行更新
//         return fetch(`${apiUrl}/api/HardwareManage/UpdateHardwareOne`, {
//           method: 'POST',
//           body: JSON.stringify(data),
//           headers: {
//             'Content-Type': 'application/json'
//           }
//         }) // 返回 fetch 的 Promise
//       } else if (state === 'true') {
//         // 如果有人操作，則直接返回一個 resolved 的 Promise，避免執行後續的 .then() 方法
//         alert('請勿重複更新')
//         return Promise.resolve(null)
//       } else {
//         return Promise.resolve(null)
//       }
//     })
//     .then((response) => {
//       // 此時 response 是一個請求結果的物件
//       if (!response) {
//         scraperState.value = false
//         return null
//       }
//       // 注意傳回值型態，字串用 text()，JSON 用 json()
//       //  如過 HTTP 響應的狀態馬碼在 200 到 299 的範圍內 ==> .ok 會回傳 true
//       if (!response.ok) {
//         return response.text().then((errorMessage) => {
//           throw new Error(errorMessage)
//         })
//       }
//       return response.text()
//     })
//     .then((data) => {
//       // 此時 data 為上一個 then 回傳的資料
//       console.log(data)
//       scraperState.value = false
//     })
//     .catch((error) => {
//       alert(error.message)
//       scraperState.value = false
//     })
// }
</script>

<style scoped>
.p_slide-enter-active,
.p_slide-leave-active {
  transition: transform 1s ease;
}
.p_slide-enter-from {
  transform: translateY(-150%);
}
.p_slide-leave-to {
  transform: translateY(-150%);
}

@media (min-width: 1650px) {
  .container {
    max-width: 1400px;
  }
}
@media (min-width: 1750px) {
  .container {
    max-width: 1600px;
  }
}
</style>
