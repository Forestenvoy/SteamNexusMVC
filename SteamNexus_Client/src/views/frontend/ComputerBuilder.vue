<template>
  <!-- Main Banner -->
  <builder-banner></builder-banner>
  <!-- 硬體匹配遊戲比例 -->
  <!-- <section class="box game-proportion" data-aos="fade-up"></section> -->
  <!-- 菜單系統 -->
  <!-- <section class="box menu-list" data-aos="fade-up">
    <menu-card-slider></menu-card-slider>
  </section> -->
  <!-- 估價單系統 -->
  <!-- <section class="box pc-builder" data-aos="fade-up"></section> -->
</template>

<script setup>
// AOS 滾動框架動畫
import AOS from 'aos'
import 'aos/dist/aos.css'

// vue 核心模組
import { onMounted } from 'vue'

// 頁面元件
import BuilderBanner from '@/components/frontend/pcbuilder/BuilderBanner.vue'
import MenuCardSlider from '@/components/frontend/pcbuilder/MenuCardSlider.vue'

onMounted(() => {
  AOS.init()
})
</script>

<style scoped>
.box {
  background-color: #eeecec;
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
