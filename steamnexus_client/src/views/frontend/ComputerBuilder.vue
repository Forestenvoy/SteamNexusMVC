<template>
  <!-- 英雄橫幅旗幟 -->
  <hero-banner></hero-banner>
  <!-- 組裝之旅 -->
  <build-journey></build-journey>
  <!-- 菜單系統 -->
  <!-- <menu-slider></menu-slider> -->
  <!-- 產品列表 -->
  <product-list></product-list>
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
import HeroBanner from '@/components/frontend/pcbuilder/HeroBanner.vue'
import BuildJourney from '@/components/frontend/pcbuilder/BuildJourney.vue'

import ProductList from '@/components/frontend/pcbuilder/ProductList.vue'

// import MenuSlider from '@/components/frontend/pcbuilder/MenuSlider.vue'
import GameRatio from '@/components/frontend/pcbuilder/GameRatio.vue'
import SystemIntroduction from '@/components/frontend/pcbuilder/SystemIntroduction.vue'
onMounted(() => {
  AOS.init()
})
</script>

<style scoped></style>
