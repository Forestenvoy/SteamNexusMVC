import { ref, computed } from 'vue'
import { defineStore } from 'pinia'

export const useScraperStore = defineStore('scraper', () => {
  // 從環境變數取得 API BASE URL
  //   const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

  // 宣告進度條顯示狀態
  const state = ref(false)

  // 回傳進度條顯示狀態
  const getState = computed(() => state.value)

  // 設定進度條狀態
  const setState = (value) => {
    state.value = value
  }

  // 宣告零件更新類型
  const type = ref('')

  // 回傳零件更新類型
  const getType = computed(() => type.value)

  // 設定零件更新類型
  const setType = (value) => {
    type.value = value
  }

  // 宣告硬體 Id
  const hardwareId = ref(0)

  // 回傳硬體 Id
  const getHardwareId = computed(() => hardwareId.value)

  // 設定硬體 Id
  const setHardwareId = (value) => {
    hardwareId.value = value
  }

  return {
    state,
    getState,
    setState,
    type,
    getType,
    setType,
    hardwareId,
    getHardwareId,
    setHardwareId
  }
})
