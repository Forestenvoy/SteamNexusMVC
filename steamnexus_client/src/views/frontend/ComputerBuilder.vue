<template>
  <!-- 英雄橫幅旗幟 -->
  <hero-banner></hero-banner>
  <!-- 組裝之旅 -->
  <build-journey v-if="builderStore.getMode === 'build'"></build-journey>
  <!-- 菜單系統 -->
  <menu-slider v-if="builderStore.getMode === 'menu'"></menu-slider>
  <!-- 子系統切換 -->
  <switch-mode></switch-mode>
  <!-- 產品列表 -->
  <product-list v-if="showProductList"></product-list>
  <!-- 遊戲匹配系統  -->
  <game-ratio v-if="builderStore.isShowMatchSystem"></game-ratio>
  <!-- 系統介紹 -->
  <!-- <system-introduction></system-introduction> -->
</template>

<script setup>
// AOS 滾動框架動畫
import AOS from 'aos'
import 'aos/dist/aos.css'

// vue 核心模組
import { computed, onMounted } from 'vue'

// vue router
import { onBeforeRouteLeave } from 'vue-router'

// pinia
import { useBuilderStore } from '@/stores/builder.js'
const builderStore = useBuilderStore()

// 頁面元件
import HeroBanner from '@/components/frontend/pcbuilder/HeroBanner.vue'
import SwitchMode from '@/components/frontend/pcbuilder/SwitchMode.vue'
import BuildJourney from '@/components/frontend/pcbuilder/BuildJourney.vue'
import MenuSlider from '@/components/frontend/pcbuilder/MenuSlider.vue'
import ProductList from '@/components/frontend/pcbuilder/ProductList.vue'
import GameRatio from '@/components/frontend/pcbuilder/GameRatio.vue'

// import SystemIntroduction from '@/components/frontend/pcbuilder/SystemIntroduction.vue'

// 產品列表是否顯示
const showProductList = computed(() => {
  if (builderStore.getProductList.length > 0) {
    return true
  }
  return false
})

onMounted(() => {
  AOS.init()
})

onBeforeRouteLeave(() => {
  builderStore.hideMatch()
  builderStore.clearProductList()
  builderStore.setMode('build')
})
</script>

<style scoped>

</style>
