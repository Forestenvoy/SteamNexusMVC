<template>
  <section id="buildJourney">
    <CContainer>
      <!-- 標題 -->
      <CRow class="mb-3">
        <CCol class="text-center">
          <h1 class="title display-3">開始你的組裝之旅</h1>
        </CCol>
      </CRow>
      <!-- 組裝線性進度條 -->
      <CRow class="mb-3">
        <CCol>
          <LinearProgress />
        </CCol>
      </CRow>
      <!-- 核心區塊 -->
      <CRow class="mb-3 system py-5">
        <CCol>
          <component :is="currentStepComponent" />
        </CCol>
      </CRow>
    </CContainer>
  </section>
</template>

<script setup>
// vue
import { computed } from 'vue'
// Core UI
import { CContainer, CRow, CCol } from '@coreui/vue'
// MyComponents
import LinearProgress from '@/components/frontend/pcbuilder/LinearProgress.vue'
import CentralProcessingUnit from '@/components/frontend/pcbuilder/section/CentralProcessingUnit.vue'
import MotherBoard from '@/components/frontend/pcbuilder/section/MotherBoard.vue'

// pinia
import { useBuilderStore } from '@/stores/builder.js'
const builderStore = useBuilderStore()

// computed
const currentStepComponent = computed(() => {
  switch (builderStore.getCurrentStep) {
    case 0:
      return CentralProcessingUnit
    case 1:
      return MotherBoard
    default:
      return CentralProcessingUnit
  }
})
</script>

<style scoped>
#buildJourney {
  padding-top: 80px;
}
.container {
  background-color: #202020;
}

.system {
  background-color: #4d4c4c;
}
</style>
