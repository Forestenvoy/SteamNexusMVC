<template>
  <!-- Main Banner -->
  <builder-banner></builder-banner>
  <!-- 菜單系統 -->
  <menu-slider></menu-slider>
  <!-- 電腦配單系統 -->
  <main-system></main-system>
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
import MenuSlider from '@/components/frontend/pcbuilder/MenuSlider.vue'
import MainSystem from '@/components/frontend/pcbuilder/MainSystem.vue'
import GameRatio from '@/components/frontend/pcbuilder/GameRatio.vue'

onMounted(() => {
  AOS.init()
})
</script>

<style scoped></style>
