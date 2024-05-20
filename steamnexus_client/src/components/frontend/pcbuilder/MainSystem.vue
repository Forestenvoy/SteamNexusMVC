<template>
  <section class="main-system mx-1 mx-md-5 mb-5" id="mainSystem">
    <CContainer>
      <!-- 標題 -->
      <CRow>
        <CCol class="text-center">
          <h1 class="title display-3">開始你的組裝之旅</h1>
        </CCol>
      </CRow>
      <!-- 進度條 -->
      <!-- 核心系統 -->
      <CRow class="content">
        <FirstQuestion v-if="builderStore.getCurrentStep === 0" />
        <CPUSelector v-else-if="builderStore.getCurrentStep === 1" />
      </CRow>
    </CContainer>
  </section>
</template>

<script setup>
// Core UI
import { CContainer, CRow, CCol } from '@coreui/vue'

// my components
import FirstQuestion from '@/components/frontend/pcbuilder/section/FirstQuestion.vue'
import CPUSelector from '@/components/frontend/pcbuilder/section/CPUSelector.vue'

// pinia
import { useBuilderStore } from '@/stores/builder.js'
const builderStore = useBuilderStore()
</script>

<style scoped>
.content {
  transition: 1s;
}
</style>
