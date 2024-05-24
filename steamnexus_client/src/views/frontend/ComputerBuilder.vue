<template>
  <!-- 開頭動畫 -->
  <opening-animation></opening-animation>
  <!-- 菜單系統 -->
  <menu-slider></menu-slider>
  <!-- 電腦配單系統 -->
  <main-system></main-system>
  <!-- Hardware Matching Game Ratio -->
  <game-ratio></game-ratio>
  <!-- 系統介紹 -->
  <system-introduction></system-introduction>
</template>

<script setup>
// AOS 滾動框架動畫
import AOS from 'aos'
import 'aos/dist/aos.css'

// vue 核心模組
import { onMounted } from 'vue'

// 頁面元件
import OpeningAnimation from '@/components/frontend/pcbuilder/OpeningAnimation.vue'
import MenuSlider from '@/components/frontend/pcbuilder/MenuSlider.vue'
import MainSystem from '@/components/frontend/pcbuilder/MainSystem.vue'
import GameRatio from '@/components/frontend/pcbuilder/GameRatio.vue'
import SystemIntroduction from '@/components/frontend/pcbuilder/SystemIntroduction.vue'
onMounted(() => {
  AOS.init()
})
</script>

<style scoped></style>
