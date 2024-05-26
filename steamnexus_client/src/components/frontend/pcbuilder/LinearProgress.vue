<template>
  <section class="d-flex justify-content-center">
    <svg width="90%" height="20">
      <!-- 繪製中間的白色虛線 -->
      <!-- stroke-width 屬性設定了線條的粗細 -->
      <!-- stroke-dasharray 屬性定義了線段和間隙的長度模式，第一個值表示線段長度，第二個值表示間隙長度 -->
      <line
        x1="10"
        y1="10"
        x2="100%"
        y2="10"
        stroke="#fff"
        stroke-width="4"
        stroke-dasharray="10 5"
      />

      <!-- 繪製實心的進度條部分 -->
      <line
        x1="10"
        y1="10"
        :x2="progress"
        y2="10"
        stroke="#f3ae0b"
        stroke-width="4"
        stroke-dasharray="10 5"
      />
    </svg>
  </section>
</template>

<script setup>
// vue
import { computed } from 'vue'

// pinia
import { useBuilderStore } from '@/stores/builder.js'
const builderStore = useBuilderStore()

let progress = computed(() => `${builderStore.getCurrentStep}0%`)
</script>

<style scoped>

</style>
