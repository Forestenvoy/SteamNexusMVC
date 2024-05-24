<template>
  <section id="systemIntroduction">
    <CContainer>
      <!-- 標題 -->
      <CRow>
        <CCol class="text-center">
          <h1 data-aos="fade-up" data-aos-duration="1500" class="title display-3">系統特色</h1>
        </CCol>
      </CRow>
      <!-- 內容 -->
      <CRow>
        <!-- Effect Cards-->
        <CCol xs="12" md="6" class="px-5 d-flex align-items-center">
          <swiper
            :spaceBetween="30"
            :effect="'fade'"
            :centeredSlides="true"
            :autoplay="{
              delay: 2500,
              disableOnInteraction: false
            }"
            :pagination="{
              clickable: true
            }"
            :modules="modules"
            class="mySwiper"
          >
            <swiper-slide class="card-bg">
              <div class="card-body">
                <CRow>
                  <CCol>
                    <h2 class="text-center mb-3">我需要懂甚麼?</h2>
                  </CCol>
                </CRow>
                <CRow>
                  <CCol>
                    <h5 class="mb-3">
                      你不需要懂技術知識，不需要處理相容性問題，只需要大體了解電腦裡面有什麼零件，就可以來這裡配一張屬於你的電腦估價單
                    </h5>
                  </CCol>
                </CRow>
              </div>
            </swiper-slide>
            <swiper-slide class="card-bg">
              <div class="card-body">
                <CRow>
                  <CCol>
                    <h2 class="text-center mb-3">456?</h2>
                  </CCol>
                </CRow>
                <CRow>
                  <CCol>
                    <h5 class="mb-3">
                      你不需要懂技術知識，不需要處理相容性問題，只需要大體了解電腦裡面有什麼零件，就可以來這裡配一張屬於你的電腦估價單
                    </h5>
                  </CCol>
                </CRow>
              </div>
            </swiper-slide>
            <swiper-slide class="card-bg">
              <div class="card-body">
                <CRow>
                  <CCol>
                    <h2 class="text-center mb-3">789?</h2>
                  </CCol>
                </CRow>
                <CRow>
                  <CCol>
                    <h5 class="mb-3">
                      你不需要懂技術知識，不需要處理相容性問題，只需要大體了解電腦裡面有什麼零件，就可以來這裡配一張屬於你的電腦估價單
                    </h5>
                  </CCol>
                </CRow>
              </div>
            </swiper-slide>
          </swiper>
        </CCol>
        <!-- 圖片 -->
        <CCol xs="12" md="6">
          <img src="@/assets/images/builder/introduction.png" alt="JustAImage" />
        </CCol>
      </CRow>
    </CContainer>
  </section>
</template>

<script setup>
// AOS 滾動框架動畫
import AOS from 'aos'
import 'aos/dist/aos.css'

// vue
import { ref, onMounted } from 'vue'

// Core UI
import { CContainer, CRow, CCol } from '@coreui/vue'

// Import Swiper Vue.js components
import { Swiper, SwiperSlide } from 'swiper/vue'
// Import Swiper styles
import 'swiper/css'
import 'swiper/css/effect-fade'
import 'swiper/css/pagination'
// import required modules
import { Autoplay, EffectFade, Pagination } from 'swiper/modules'
const modules = ref([Autoplay, EffectFade, Pagination])

onMounted(() => {
  AOS.init({
    once: true // Animation happens only once
  })
})
</script>

<style>
.swiper-pagination-bullet-active {
  background-color: white;
}
</style>

<style scoped>
.mySwiper {
  height: 450px;
  padding-left: 50px;
  padding-right: 50px;
}

/* 區塊 */
#systemIntroduction {
  padding-top: 80px;
}

/* 圖片 */
img {
  width: 500px;
  height: 450px;
}

@media screen and (max-width: 768px) {
  .mySwiper {
    padding-left: 0px;
    padding-right: 0px;
  }

  img {
    display: none;
  }
}
.card-bg {
  background: linear-gradient(135deg, #fffffff5, #313131, #ffffff98);
  padding: 1px;
  border-radius: 1.2rem;
  box-shadow: 0px 1rem 1.5rem -0.9rem #000000e1;
  height: 450px;
}

.card-body {
  font-size: 1rem;
  color: #bec4cf;
  background: linear-gradient(135deg, #000000 0%, #313131 43%, #000000 100%);
  padding: 1.5rem;
  border-radius: 1.2rem;
  height: 100%;
}
</style>
