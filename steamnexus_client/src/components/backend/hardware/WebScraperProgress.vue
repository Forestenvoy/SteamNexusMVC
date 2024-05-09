<template>
  <section class="mx-2 mb-3">
    <div class="row mx-1 mb-2">
      <div class="h3 col-12 col-md-3 text-center text-md-end">
        <CSpinner />
      </div>
      <div class="h3 col-12 col-md-6 text-center">Web Scraper</div>
      <div class="h3 col-12 col-md-3 text-center text-md-start">{{ p_value }}%</div>
    </div>
    <div class="row mx-1">
      <CProgress
        :color="p_color"
        :variant="p_variant"
        :animated="p_animated"
        :value="p_value"
        class="p-0 me-5"
      >
      </CProgress>
    </div>
  </section>
</template>
<script setup>
import { ref, onMounted } from 'vue'
import { CProgress, CSpinner } from '@coreui/vue'

// 繫結進度條屬性
const p_color = ref('info')
const p_variant = ref('striped')
const p_animated = ref(true)
const p_value = ref(80)

// 在组件渲染后获取容器宽度
onMounted(() => {})
</script>
<style scoped>
section {
  border-radius: 10px;
  padding: 15px;
}

html[data-coreui-theme='light'] section {
  border: 1px solid #3c4242;
  background-color: #e6feff;
}

html[data-coreui-theme='dark'] section {
  border: 1px solid #dee2e6;
  background-color: #3c4242;
}
</style>
