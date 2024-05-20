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

  return { currentStep, getCurrentStep, setCurrentStep }
})
