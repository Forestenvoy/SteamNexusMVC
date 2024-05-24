<template>
  <!-- 風冷散熱器 -->
  <air-cooler :default-selected="A" @selected="updateA"></air-cooler>
  <!-- 水冷散熱器 -->
  <liquid-cooler :default-selected="L" @selected="updateL"></liquid-cooler>
  <!-- 上一頁、下一頁 -->
  <CRow>
    <CCol xs="12" md="6">
      <button class="button-80" @click="builderStore.prev()">上一頁</button>
    </CCol>
    <CCol xs="12" md="6">
      <button class="button-80" @click="builderStore.next()">下一頁</button>
    </CCol>
  </CRow>
</template>

<script setup>
import AirCooler from '@/components/frontend/pcbuilder/section/AirCooler.vue'
import LiquidCooler from '@/components/frontend/pcbuilder/section/LiquidCooler.vue'

// vue
import { ref } from 'vue'

// pinia
import { useBuilderStore } from '@/stores/builder.js'
const builderStore = useBuilderStore()

const A = ref(0)
const L = ref(0)

const updateA = (newValue) => {
  A.value = newValue
  if (newValue !== 0) {
    L.value = 0
  }
}

const updateL = (newValue) => {
  L.value = newValue
  if (newValue !== 0) {
    A.value = 0
  }
}
</script>

<style scoped></style>
