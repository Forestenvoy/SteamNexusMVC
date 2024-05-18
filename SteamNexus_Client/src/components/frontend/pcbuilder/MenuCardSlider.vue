<template>
  <div class="slide-container">
    <div class="slide-content">
      <h2 class="title">Menu</h2>
      <!-- swiper 設置 -->
      <!-- slidesPerView: 設置 slider 容器能夠同時顯示的 slides 數量 -->
      <!-- slidesPerGroup: 設置每組的 slides 數量 -->
      <!-- slidesPerGroupSkip: 在循環模式下設置跳過的 slides 數量。設置為 1，表示每次滑動時只跳過一個 slide。 -->
      <!-- grabCursor: 網頁滑鼠游標懸停時，滑鼠游標會變成手掌形狀，可用於控制滑動方向。 -->
      <!-- keyboard: 鍵盤控制滑動方向。enabled: 鍵盤控制滑動方向。 -->
      <!-- loop: 重複滑動。 -->
      <!-- spaceBetween: 設置兩個 slide 之間的間距。 -->
      <!-- navigation: 顯示左右切換按鈕。 -->
      <!-- pagination: 顯示分頁按鈕。 -->
      <!-- clickable: true => 可以點擊分頁按鈕 -->
      <!-- dynamicBullets: true => 可以動態生成分頁按鈕 -->
      <swiper
        :slidesPerView="1"
        :slidesPerGroup="1"
        :slidesPerGroupSkip="1"
        :grabCursor="true"
        :keyboard="{
          enabled: true
        }"
        :loop="true"
        :spaceBetween="30"
        :navigation="true"
        :pagination="{
          clickable: true,
          dynamicBullets: true
        }"
        :breakpoints="{
          // xs
          '576': {
            slidesPerView: 1
          },
          // sm
          '1000': {
            slidesPerView: 2
          },
          // md
          '1200': {
            slidesPerView: 3
          },
          // lg
          '1600': {
            slidesPerView: 4
          },
          // xl
          '1800': {
            slidesPerView: 5
          }
        }"
        :modules="modules"
        class="card-wrapper"
      >
        <swiper-slide class="swiper-card" v-for="menu in menuLists" :key="menu.id">
          <menu-card :menu="menu"></menu-card>
        </swiper-slide>
      </swiper>
    </div>
  </div>
</template>

<script setup>
// Import Swiper Vue.js components
import { Swiper, SwiperSlide } from 'swiper/vue'
// Import Swiper styles
import 'swiper/css'
import 'swiper/css/pagination'
import 'swiper/css/navigation'
// import required modules
import { Keyboard, Pagination, Navigation } from 'swiper/modules'
const modules = ref([Keyboard, Pagination, Navigation])

import MenuCard from '@/components/frontend/pcbuilder/MenuCard.vue'

import { ref, onMounted } from 'vue'

// 從環境變數取得 API BASE URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

const menuLists = ref([])

// 獲取上架的所有菜單
const getMenuLists = async () => {
  await fetch(`${apiUrl}/api/PcBuilder/GetMenuLists`, {
    method: 'GET'
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error('Network response was not ok')
      }
      return response.json()
    })
    .then((data) => {
      menuLists.value = data
    })
    .catch((error) => {
      console.error('Error:', error)
    })
}

onMounted(() => {
  getMenuLists()
})
</script>

<style>
.swiper {
  height: 400px;
}

.swiper-button-prev,
.swiper-button-next {
  color: black;
}

.swiper-pagination-bullet-active-main {
  background-color: black;
}
</style>

<style scoped>
.slide-container {
  width: 100%;
  height: 100%;
}

.slide-content {
  /* margin: 0 40px; */
  overflow: visible;
  border-radius: 25px;
}
</style>
