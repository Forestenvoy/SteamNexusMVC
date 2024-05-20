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
  const socket = ref({})
  // getter
  const getSocket = computed(() => socket.value)
  // action
  const setSocket = (value) => {
    socket.value = value
  }

  return {
    getCurrentStep,
    setCurrentStep,
    prev,
    next,
    getProductList,
    addProduct,
    getSocket,
    setSocket
  }
})