<template>
  <!-- Main Banner -->
  <builder-banner></builder-banner>
  <!-- 菜單系統 -->
  <!-- <section class="box menu-list" data-aos="fade-up">
    <menu-card-slider></menu-card-slider>
  </section> -->
  <!-- 估價單系統 -->
  <!-- <section class="box pc-builder" data-aos="fade-up"></section> -->
  <!-- Hardware Matching Game Ratio -->
  <game-ratio></game-ratio>
</template>

<script setup>
// AOS 滾動框架動畫
import AOS from 'aos'
import 'aos/dist/aos.css'

// vue 核心模組
import { onMounted } from 'vue'

// 頁面元件
import BuilderBanner from '@/components/frontend/pcbuilder/BuilderBanner.vue'
// import MenuCardSlider from '@/components/frontend/pcbuilder/MenuCardSlider.vue'
import GameRatio from '@/components/frontend/pcbuilder/GameRatio.vue'

onMounted(() => {
  AOS.init()
})
</script>

<style scoped></style>
