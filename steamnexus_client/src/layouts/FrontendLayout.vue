<template>
  <div>
    <div class="wrapper d-flex flex-column min-vh-100">
      <app-header></app-header>
      <div class="body flex-grow-1">
        <CContainer class="px-5 pt-4 position-relative" fluid>
          <router-view></router-view>
        </CContainer>
      </div>
      <app-footer></app-footer>
    </div>
  </div>
</template>
<script setup>
import { CContainer } from '@coreui/vue'
import AppHeader from '@/components/frontend/share/AppHeader.vue'
import AppFooter from '@/components/frontend/share/AppFooter.vue'
</script>

<style scoped></style>
