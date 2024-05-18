<template>
  <div>
    <div class="wrapper d-flex flex-column min-vh-100">
      <app-header></app-header>
      <div class="body flex-grow-1 position-relative">
        <router-view></router-view>
      </div>
      <app-footer></app-footer>
    </div>
  </div>
</template>
<script setup>
import AppHeader from '@/components/frontend/share/AppHeader.vue'
import AppFooter from '@/components/frontend/share/AppFooter.vue'
</script>

<style scoped></style>
