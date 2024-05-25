import { ref, computed } from 'vue'
import { defineStore } from 'pinia'

export const useBuilderStore = defineStore('builder', () => {
  // 階段
  // state
  const currentStep = ref(0)
  // getter
  const getCurrentStep = computed(() => currentStep.value)
  // action
  const setCurrentStep = (value) => {
    if (typeof value === 'number' && value >= 0) {
      currentStep.value = value
    } else {
      console.warn('Invalid Step value')
    }
  }
  const prev = () => {
    if (currentStep.value === 0) return
    currentStep.value--
  }
  const next = () => {
    currentStep.value++
  }

  // 產品清單
  // state
  const productList = ref([])
  // getter
  const getProductList = computed(() => productList.value)
  // action
  // 加入產品
  const addProduct = (type, product) => {
    // 風冷散熱器、水冷散熱器，兩者只能擇一
    if (type === 'AirCooler') {
      // 刪除水冷散熱器
      productList.value = productList.value.filter((p) => p.type !== 'LiquidCooler')
    }
    if (type === 'LiquidCooler') {
      // 刪除風冷散熱器
      productList.value = productList.value.filter((p) => p.type !== 'AirCooler')
    }
    // SSD、HDD 可重複
    if (type === 'SSD' || type === 'HDD') {
      // 加入新產品
      productList.value.push(product)
      return
    }
    // 檢查有沒有重複產品
    if (productList.value.find((p) => p.type === type)) {
      // 刪除重複產品
      productList.value = productList.value.filter((p) => p.type !== type)
      // 加入新產品
      productList.value.push(product)
      return
    } else {
      // 加入新產品
      productList.value.push(product)
    }
  }

  // CPU 腳位
  // state
  const socket = ref('')
  // getter
  const getSocket = computed(() => socket.value)
  // action
  const setSocket = (value) => {
    socket.value = value
  }

  // RAM SDRAM
  // state
  const memory = ref('')
  // getter
  const getMemory = computed(() => memory.value)
  // action
  const setMemory = (value) => {
    memory.value = value
  }

  return {
    getCurrentStep,
    setCurrentStep,
    prev,
    next,
    getProductList,
    addProduct,
    getSocket,
    setSocket,
    getMemory,
    setMemory
  }
})
