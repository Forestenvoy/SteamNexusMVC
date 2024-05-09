<template>
  <section class="mx-2 mb-3">
    <div class="row mx-1 mb-2">
      <div class="h3 col-12 col-md-3 text-center text-md-end">
        <CSpinner />
      </div>
      <div class="h3 col-12 col-md-6 text-center">Web Scraper</div>
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

// 從環境變數取得 API BASE URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

// 繫結進度條屬性
const p_color = ref('info')
const p_variant = ref('striped')
const p_animated = ref(true)
const p_value = ref(10)

// 宣告 EventSource 物件
let source = null

onMounted(() => {
  // 建立連線
  source = new EventSource(`${apiUrl}/api/HardwareManage/UpdateMessage`)
  // 連線成功 觸發 onopen 事件
  // onmessage 事件是預設用來接收 Server 端回傳的結果(data)
  source.onmessage = (event) => {
    console.log(event.data)
  }
})

onUnmounted(() => {
  source.close()
  console.log('我關閉了')
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
  background-color: #3c4242;
}
</style>
