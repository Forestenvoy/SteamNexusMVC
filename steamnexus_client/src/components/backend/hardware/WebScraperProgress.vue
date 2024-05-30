<template>
  <section class="mx-2 mb-3">
    <div class="row mx-1 mb-2">
      <div class="h3 col-12 col-md-3 text-center text-md-end">
        <CSpinner v-if="p_state === 0" />
        <CIcon icon="cilCheckAlt" style="width: 38px; height: 38px; color: #00ff00" v-else />
      </div>
      <div class="h3 col-12 col-md-6 text-center">{{ p_text }}{{ p_dot }}</div>
      <div class="h3 col-12 col-md-3 text-center text-md-start">{{ p_value }}%</div>
    </div>
    <div class="row mx-1">
      <CProgress
        :color="p_color"
        :variant="p_variant"
        :animated="p_animated"
        :value="p_value"
        class="p-0 me-5"
      >
      </CProgress>
    </div>
  </section>
</template>
<script setup>
import { ref, onMounted, onUnmounted } from 'vue'
import { CProgress, CSpinner } from '@coreui/vue'

// 使用 Pinia
import { useScraperStore } from '@/stores/scraper.js'
// 利用 store 去訪問狀態 ✨
const store = useScraperStore()

// 拿身份驗證
import { useIdentityStore } from '@/stores/identity.js'
const authStore = useIdentityStore()

// 從環境變數取得 API BASE URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

// 建立 Props 判斷
const props = defineProps(['updateType'])
// 繫結進度條屬性
let p_text = ref('請求發送中')
let p_color = ref('info')
let p_variant = ref('striped')
let p_animated = ref(true)
let p_value = ref(0)
let p_dot = ref('')
let p_state = ref(0)

// 宣告 EventSource 物件
let source = null
let sourceData = ''
let resultStr = '爬蟲啟動中'
var intervalID = null

// 單一零件更新
function UpdateOneHardware() {
  fetch(`${apiUrl}/api/HardwareManage/UpdateHardwareOne`, {
    method: 'POST',
    body: JSON.stringify({
      Type: store.getHardwareId
    }),
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${authStore.getToken}`
    }
  })
    .then((response) => {
      if (!response) {
        store.setState(false)
        return null
      }
      if (!response.ok) {
        return response.text().then((errorMessage) => {
          throw new Error(errorMessage)
        })
      }
      return response.text()
    })
    .then((data) => {
      // 此時 data 為上一個 then 回傳的資料
      console.log(data)
      store.setState(false)
    })
    .catch((error) => {
      alert(error.message)
      store.setState(false)
    })
}

// 所有零件更新
function UpdateAllHardware() {
  fetch(`${apiUrl}/api/HardwareManage/UpdateHardwareAll`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${authStore.getToken}`
    }
  })
    .then((response) => {
      if (!response.ok) {
        return response.text().then((errorMessage) => {
          throw new Error(errorMessage)
        })
      }
      return response.text()
    })
    .then((data) => {
      console.log(data)
      store.setState(false)
    })
    .catch((error) => {
      alert(error)
      store.setState(false)
    })
}
onMounted(() => {
  if (props.updateType === 'All') {
    UpdateAllHardware()
  } else {
    UpdateOneHardware()
  }
  // 建立連線
  source = new EventSource(`${apiUrl}/api/HardwareManage/UpdateMessage`)
  // onmessage 事件是預設用來接收 Server 端回傳的結果(data)
  source.onmessage = (event) => {
    sourceData = event.data
    p_text.value = sourceData

    if (sourceData === '爬蟲啟動中') {
      p_color.value = 'info'
      p_value.value = 0
      p_variant.value = 'striped'
      p_animated.value = true
      p_state.value = 0
    }

    if (sourceData === '更新成功') {
      p_color.value = 'success'
      p_value.value = 100
      p_variant.value = undefined
      p_animated.value = false
      p_state.value = 1
      p_dot.value = ''
      // 清除計時器
      clearInterval(intervalID)
      return
    }
    // 進度條數值更新
    if (props.updateType === 'All') {
      switch (sourceData) {
        case '爬蟲啟動中':
          p_value.value = 0
          break
        case 'CPU 解析中':
          p_value.value = 4
          break
        case 'CPU 資料更新完成':
          p_value.value = 8
          break
        case 'GPU 解析中':
          p_value.value = 12
          break
        case 'GPU 資料更新完成':
          p_value.value = 16
          break
        case 'RAM 解析中':
          p_value.value = 20
          break
        case 'RAM 資料更新完成':
          p_value.value = 24
          break
        case 'Motherboard 解析中':
          p_value.value = 28
          break
        case 'Motherboard 資料更新完成':
          p_value.value = 35
          break
        case 'SSD 解析中':
          p_value.value = 39
          break
        case 'SSD 資料更新完成':
          p_value.value = 43
          break
        case 'HDD 解析中':
          p_value.value = 47
          break
        case 'HDD 資料更新完成':
          p_value.value = 52
          break
        case 'Air Cooler 解析中':
          p_value.value = 56
          break
        case 'Air Cooler 資料更新完成':
          p_value.value = 60
          break
        case 'LiquidCooler 解析中':
          p_value.value = 64
          break
        case 'LiquidCooler 資料更新完成':
          p_value.value = 68
          break
        case 'CASE 解析中':
          p_value.value = 72
          break
        case 'CASE 資料更新完成':
          p_value.value = 77
          break
        case 'PSU 解析中':
          p_value.value = 82
          break
        case 'PSU 資料更新完成':
          p_value.value = 87
          break
        case 'OS 解析中':
          p_value.value = 92
          break
        case 'OS 資料更新完成':
          p_value.value = 96
          break
      }
    } else {
      if (resultStr !== sourceData) {
        resultStr = sourceData
        p_value.value += 33
      }
    }
  }

  // 建立計時器
  intervalID = setInterval(function () {
    p_dot.value = p_dot.value === ' . . .' ? '' : p_dot.value + ' .'
  }, 200)
})

onUnmounted(() => {
  // 關閉連線
  source.close()
})
</script>
<style scoped>
section {
  border-radius: 10px;
  padding: 15px;
}

html[data-coreui-theme='light'] section {
  border: 1px solid #3c4242;
  background-color: #e6feff;
}

html[data-coreui-theme='dark'] section {
  border: 1px solid #dee2e6;
  background-color: #2d2f2f;
}
</style>
