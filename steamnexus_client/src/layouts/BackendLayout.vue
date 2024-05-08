<template>
  <div>
    <AppSidebar />
    <div class="wrapper d-flex flex-column min-vh-100">
      <AppHeader />
      <div class="body flex-grow-1">
        <CContainer class="px-0">
          <router-view></router-view>
        </CContainer>
      </div>
      <AppFooter />
    </div>
  </div>
</template>
<script setup>
import { CContainer } from '@coreui/vue'
import AppFooter from '@/components/backend/share/AppFooter.vue'
import AppHeader from '@/components/backend/share/AppHeader.vue'
import AppSidebar from '@/components/backend/share/AppSidebar.vue'
</script>
<style scoped>
@media (min-width: 1750px) {
  .container {
    max-width: 1700px;
  }
}
</style>
