<template>
  <!-- 硬體匹配遊戲比例 -->
  <section class="box game-proportion"></section>
  <!-- 菜單系統 -->
  <section class="box menu-list">
    <menu-card-slider></menu-card-slider>
  </section>
  <!-- 估價單系統 -->
  <section class="box pc-builder"></section>
</template>

<script setup>
import MenuCardSlider from '@/components/frontend/pcbuilder/MenuCardSlider.vue'
</script>

<style scoped>
.box {
  background-color: #eeecec;
  border-radius: 20px;
  border: 2px solid #525252;
  margin-bottom: 20px;
  padding: 20px 20px;
}

.game-proportion {
  width: 100%;
  height: 500px;
}

.menu-list {
  width: 100%;
  height: 450px;
  display: flex;
  justify-content: center;
  align-items: center;
}

.pc-builder {
  width: 100%;
  height: 300px;
}
</style>
