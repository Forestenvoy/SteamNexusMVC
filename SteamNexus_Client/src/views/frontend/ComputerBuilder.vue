<template>
  <!-- 硬體匹配遊戲比例 -->
  <section class="box game-proportion"></section>
  <!-- 菜單系統 -->
  <section class="box menu-list">
    <menu-carousel></menu-carousel>
  </section>
  <!-- 估價單系統 -->
  <section class="box pc-builder"></section>
</template>

<script setup>
import MenuCarousel from '@/components/frontend/pcbuilder/MenuCarousel.vue'
</script>

<style scoped>
.box {
  background-color: #eeecec;
  border-radius: 20px;
  border: 2px solid #525252;
  margin-bottom: 20px;
  padding: 20px;
}

.game-proportion {
  width: 100%;
  height: 500px;
}

.menu-list {
  width: 100%;
  height: 300px;
}

.pc-builder {
  width: 100%;
  height: 300px;
}
</style>
