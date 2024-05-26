<template>
  <CRow class="block">
    <CCol xs="12" md="6" class="text-center"
      ><h3>我時間寶貴，我不想一步一步選擇</h3>
      <a href="#menuSlider" class="button-80">觀看現成的菜單</a>
    </CCol>
    <CCol xs="12" md="6" class="text-center"
      ><h3>我等不及了，快點開始吧</h3>
      <button class="button-80" @click="next">GO</button>
    </CCol>
  </CRow>
</template>

<script setup>
// pinia
import { useBuilderStore } from '@/stores/builder.js'
const builderStore = useBuilderStore()

const next = () => {
  builderStore.next()
}
</script>

<style scoped>
/* 自定義 */
.block {
  padding: 50px 0 50px 0;
}

h3 {
  margin-bottom: 50px;
}

@import '@/assets/styles/builder.css';
</style>
