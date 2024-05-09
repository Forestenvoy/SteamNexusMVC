<template>
  <section class="mx-2 mb-2">
    <div class="row">123</div>
    <div class="row">
      <CProgress color="info" variant="striped" animated :value="progress" class="p-0 me-5">
        <CProgressBar>25%</CProgressBar>
      </CProgress>
    </div>
  </section>
</template>
<script setup>
import { ref, onMounted } from 'vue'
import { CContainer, CProgress } from '@coreui/vue'

// 宣告 進度
const progress = ref(80)

// 在组件渲染后获取容器宽度
onMounted(() => {})
</script>
<style scoped></style>
