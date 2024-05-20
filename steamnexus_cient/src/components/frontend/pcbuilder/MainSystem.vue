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
        <component :is="currentStepComponent" />
      </CRow>
    </CContainer>
  </section>
</template>

<script setup>
// vue
import { computed } from 'vue'

// Core UI
import { CContainer, CRow, CCol } from '@coreui/vue'

// my components
import FirstQuestion from '@/components/frontend/pcbuilder/section/FirstQuestion.vue'
import CentralProcessingUnit from '@/components/frontend/pcbuilder/section/CentralProcessingUnit.vue'
import MotherBoard from '@/components/frontend/pcbuilder/section/MotherBoard.vue'
import GraphicsProcessingUnit from '@/components/frontend/pcbuilder/section/GraphicsProcessingUnit.vue'

// pinia
import { useBuilderStore } from '@/stores/builder.js'
const builderStore = useBuilderStore()

// computed
const currentStepComponent = computed(() => {
  switch (builderStore.getCurrentStep) {
    case 0:
      return FirstQuestion
    case 1:
      return CentralProcessingUnit
    case 2:
      return MotherBoard
    case 3:
      return GraphicsProcessingUnit
    default:
      return FirstQuestion
  }
})
</script>

<style scoped>
.content {
  transition: 1s;
}
</style>
