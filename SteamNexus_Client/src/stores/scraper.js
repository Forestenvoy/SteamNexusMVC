import { ref } from 'vue'
import { defineStore } from 'pinia'

export const useScraperStore = defineStore('scraper', () => {
  // 從環境變數取得 API BASE URL
  //   const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

  // 宣告進度條顯示狀態
  const state = ref(false)

  // 宣告零件更新類型
  const type = ref('')

  // 顯示進度條進度
  const view = () => {
    console.log(state.value)
  }

  // 啟用進度條
  const start = () => {
    state.value = true
  }

  // 關閉進度條
  const stop = () => {
    state.value = false
  }

  return { state, type, start, stop, view }
})
