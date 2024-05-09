<template>
  <div>
    <AppSidebar />
    <div class="wrapper d-flex flex-column min-vh-100">
      <AppHeader />
      <div class="body flex-grow-1">
        <CContainer class="px-0">
          <transition-group name="systemContainer">
            <!-- 進度條元件 -->
            <transition name="p_slide">
              <web-scraper-progress
                class="p_element"
                :class="{ expand: scraperState }"
                v-if="scraperState"
              ></web-scraper-progress>
            </transition>
            <!-- 後台子系統 -->
            <router-view
              @update-one-hardware="UpdateOneHardware"
              @update-all-hardware="UpdateAllHardware"
            ></router-view>
          </transition-group>
        </CContainer>
      </div>
      <AppFooter />
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { CContainer } from '@coreui/vue'
import AppFooter from '@/components/backend/share/AppFooter.vue'
import AppHeader from '@/components/backend/share/AppHeader.vue'
import AppSidebar from '@/components/backend/share/AppSidebar.vue'
import WebScraperProgress from '@/components/backend/hardware/WebScraperProgress.vue'

// 從環境變數取得 API BASE URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

// 宣告進度條顯示狀態
const scraperState = ref(false)

// 單一零件更新
function UpdateOneHardware(hardwareId) {
  // 啟動進度條
  scraperState.value = true
  // 發送非同步POST請求 ==> 資料庫資料變更
  var data = {
    Type: hardwareId
  }
  fetch(`${apiUrl}/api/HardwareManage/UpdateHardwareOne`, {
    method: 'POST',
    body: JSON.stringify(data),
    headers: {
      'Content-Type': 'application/json'
    }
  })
    .then((response) => {
      // 此時 result 是一個請求結果的物件
      // 注意傳回值型態，字串用 text()，JSON 用 json()
      //  如過 HTTP 響應的狀態馬碼在 200 到 299 的範圍內 ==> .ok 會回傳 true
      if (!response.ok) {
        return response.text().then((errorMessage) => {
          throw new Error(errorMessage)
        })
      }
      return response.text()
    })
    .then((data) => {
      // 此時 data 為上一個 then 回傳的資料
      alert(data)
      scraperState.value = false
    })
    .catch((error) => {
      alert(error.message)
      scraperState.value = false
    })
}

// 所有零件更新
function UpdateAllHardware() {
  // 啟動進度條
  scraperState.value = true
  // 發送非同步POST請求 ==> 資料庫資料變更
  fetch(`${apiUrl}/api/HardwareManage/UpdateHardwareAll`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    }
  })
    .then((response) => {
      // 此時 result 是一個請求結果的物件
      // 注意傳回值型態，字串用 text()，JSON 用 json()
      //  如過 HTTP 響應的狀態馬碼在 200 到 299 的範圍內 ==> .ok 會回傳 true
      if (!response.ok) {
        return response.text().then((errorMessage) => {
          throw new Error(errorMessage)
        })
      }
      return response.text()
    })
    .then((data) => {
      // 此時 data 為上一個 then 回傳的資料
      alert(data)
      scraperState.value = false
    })
    .catch((error) => {
      alert(error)
      scraperState.value = false
    })
}
</script>

<style scoped>
.systemContainer-move {
  transition: all 1s ease;
}
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
