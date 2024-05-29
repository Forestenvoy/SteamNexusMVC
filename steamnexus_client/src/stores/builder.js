import { ref, computed } from 'vue'
import { defineStore } from 'pinia'

export const useBuilderStore = defineStore('builder', () => {
  // 模式
  // state
  const mode = ref('build')
  // getter
  const getMode = computed(() => mode.value)
  // action
  const setMode = (value) => {
    if (typeof value === 'string') {
      mode.value = value
    } else {
      console.warn('Invalid Mode value')
    }
  }

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
  // 提供清單
  const setProductList = (value) => {
    productList.value = value
  }
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

  // CPUId
  // getter
  const getCPUId = computed(() => {
    const id = Number(productList.value.find((p) => p.type === 'CPU').id)
    if (id) return id
    else return 0
  })

  // GPUId
  // getter
  const getGPUId = computed(() => {
    const id = Number(productList.value.find((p) => p.type === 'GPU').id)
    if (id) return id
    else return 0
  })

  // RAMId
  // getter
  const getRAMId = computed(() => {
    const id = Number(productList.value.find((p) => p.type === 'RAM').id)
    if (id) return id
    else return 0
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

  // 最低配備比例
  // state
  const minRatio = ref(0)
  // getter
  const getMinRatio = computed(() => minRatio.value)
  // action
  const setMinRatio = (value) => {
    minRatio.value = value
  }
  // 最低配備遊戲數量
  // state
  const minCount = ref(0)
  // getter
  const getMinCount = computed(() => minCount.value)
  // action
  const setMinCount = (value) => {
    minCount.value = value
  }

  // 最低、建議 配備模式辨別
  // state
  const ratioMode = ref('')
  // getter
  const getRatioMode = computed(() => ratioMode.value)
  // action
  const setRatioMode = (value) => {
    ratioMode.value = value
  }

  // 建議配備比例
  const recRatio = ref(0)
  // getter
  const getRecRatio = computed(() => recRatio.value)
  // action
  const setRecRatio = (value) => {
    recRatio.value = value
  }

  // 建議配備遊戲數量
  const recCount = ref(0)
  // getter
  const getRecCount = computed(() => recCount.value)
  // action
  const setRecCount = (value) => {
    recCount.value = value
  }

  // 顯示遊戲匹配系統
  // state
  const showMatchSystem = ref(false)
  // getter
  const isShowMatchSystem = computed(() => showMatchSystem.value)
  // action
  const showMatch = () => {
    // 開啟遊戲匹配系統
    showMatchSystem.value = true
  }
  const hideMatch = () => {
    // 關閉遊戲匹配系統
    showMatchSystem.value = false
    minRatio.value = 0
    recRatio.value = 0
    minCount.value = 0
    recCount.value = 0
  }

  // 清空產品清單
  const clearProductList = () => {
    productList.value = []
    currentStep.value = 0
  }

  return {
    getMode,
    setMode,
    getCurrentStep,
    setCurrentStep,
    prev,
    next,
    getProductList,
    setProductList,
    addProduct,
    removeProduct,
    totalWattage,
    totalPrice,
    getCPUId,
    getGPUId,
    getRAMId,
    getSocket,
    setSocket,
    getMemory,
    setMemory,
    getMinRatio,
    setMinRatio,
    getMinCount,
    setMinCount,
    getRecRatio,
    setRecRatio,
    getRecCount,
    setRecCount,
    getRatioMode,
    setRatioMode,
    isShowMatchSystem,
    showMatch,
    hideMatch,
    clearProductList
  }
})
