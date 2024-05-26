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
    // 關閉遊戲匹配系統
    showMatchSystem.value = false
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

  // 刪除產品
  const removeProduct = (id) => {
    // 關閉遊戲匹配系統
    showMatchSystem.value = false
    // 刪除產品
    productList.value = productList.value.filter((p) => p.id !== id)
  }

  // 總瓦數計算
  const totalWattage = computed(() => {
    let total = 0
    productList.value.forEach((p) => {
      total += Number(p.wattage)
    })
    return total
  })

  // 總價格計算
  const totalPrice = computed(() => {
    let total = 0
    productList.value.forEach((p) => {
      total += Number(p.price)
    })
    return total
  })

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

  // 顯示遊戲匹配系統
  // state
  const showMatchSystem = ref(false)
  // getter
  const isShowMatchSystem = computed(() => showMatchSystem.value)
  // action
  const showMatch = () => {
    showMatchSystem.value = true
  }
  const hideMatch = () => {
    showMatchSystem.value = false
  }

  return {
    getCurrentStep,
    setCurrentStep,
    prev,
    next,
    getProductList,
    addProduct,
    removeProduct,
    totalWattage,
    totalPrice,
    getSocket,
    setSocket,
    getMemory,
    setMemory,
    isShowMatchSystem,
    showMatch,
    hideMatch
  }
})
