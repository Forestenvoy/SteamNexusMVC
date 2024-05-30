import { ref, computed } from 'vue'
import { defineStore } from 'pinia'

export const useSearchStore = defineStore('search', () => {
  // 關鍵字
  // state
  const keyword = ref('')
  // getter
  const getKeyword = computed(() => keyword.value)
  // action
  const setKeyword = (value) => {
    keyword.value = value
  }

  return { getKeyword, setKeyword }
})
