<template>
  <CContainer>
    <CRow>
      <CCol xs="12">
        <swiper
          :slidesPerView="1"
          :spaceBetween="0"
          :autoplay="true"
          :pagination="pagination"
          :navigation="true"
          :modules="modules"
        >
          <swiper-slide v-for="(slide, index) in slides" :key="index"
            ><a :href="slide.url"
              ><img :src="slide.imagePath" :alt="slide.title" />
              <div class="description">
                <p>{{ slide.description }}</p>
              </div>
            </a></swiper-slide
          >
        </swiper>
      </CCol>
    </CRow>
  </CContainer>
</template>
<script setup>
// Import Swiper Vue.js components
import { Swiper, SwiperSlide } from 'swiper/vue'

import { ref, onMounted } from 'vue'

// Import Swiper styles
import 'swiper/css'

import 'swiper/css/pagination'
import 'swiper/css/navigation'

// import required modules
import { Pagination, Navigation, Autoplay } from 'swiper/modules'

// Core UI
import { CContainer, CRow, CCol } from '@coreui/vue'
// 從環境變數取得 API BASE URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

const modules = ref([Pagination, Navigation, Autoplay])
const pagination = ref({
  clickable: true
})
const slides = ref([])

async function fetchSlides() {
  fetch(`${apiUrl}/api/Advertisement/GetAdSlides`, {
    method: 'GET'
  })
    .then((response) => response.json())
    .then((data) => {
      console.log(data)
      slides.value = data
    })
    .catch((error) => {
      console.error('Failed to fetch slides:', error)
    })
}

onMounted(() => {
  fetchSlides()
})
</script>
<style scoped>
.swiper {
  /* 輪播範圍設定 */
  max-width: 1000px;
  margin: 0 auto;
  border-radius: 15px;

  /* margin-left: auto; */
  /* margin-right: auto; */
}

.swiper-slide {
  text-align: center;
  font-size: 18px;
  background: #fff;
  color: #000;

  /* Center slide text vertically */
  display: flex;
  justify-content: center;
  align-items: center;
}

.swiper-slide img {
  display: block;
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.description {
  margin-top: 5px;
  font-size: 24px;
  position: absolute;
  bottom: 0px;
  /* 調整說明文字距離底部的距離 */
  width: 100%;
  height: 100px;
  text-align: center;
  color: white;
  background-color: rgba(0, 0, 0, 0.5);
}

p {
  margin: 20px;
  text-align: center;
  justify-content: center;
}
</style>
