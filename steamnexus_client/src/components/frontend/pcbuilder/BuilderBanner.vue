<template>
  <section class="banner" data-aos="zoom-in-down" data-aos-duration="500">
    <CContainer>
      <CRow class="align-items-center">
        <CCol xs="12" md="6" xl="7" class="order-1 order-md-0">
          <h1 data-aos="fade-up" data-aos-duration="1500" class="text-center text-md-start mb-3">
            一般人想組台遊戲用桌上型電腦
          </h1>
          <h2
            data-aos="fade-up"
            data-aos-duration="1500"
            data-aos-delay="500"
            class="text-center text-md-start mb-3"
          >
            打開電腦硬體零售商的網站，看不懂 <CIcon icon="cilFrown" size="xl" />
          </h2>
          <CRow>
            <CCol xs="12" md="6" class="text-center text-md-start mb-2">
              <h4 data-aos="fade-up" data-aos-duration="1500" :data-aos-delay="aosDelay1">
                ▪️ 缺乏基礎知識
              </h4>
            </CCol>
            <CCol xs="12" md="6" class="text-center text-md-start mb-2">
              <h4 data-aos="fade-up" data-aos-duration="1500" :data-aos-delay="aosDelay1">
                ▪️ 相容性問題
              </h4>
            </CCol>
            <CCol xs="12" md="6" class="text-center text-md-start mb-2">
              <h4 data-aos="fade-up" data-aos-duration="1500" :data-aos-delay="aosDelay1">
                ▪️ 技術規格複雜
              </h4>
            </CCol>
            <CCol xs="12" md="6" class="text-center text-md-start mb-2">
              <h4 data-aos="fade-up" data-aos-duration="1500" :data-aos-delay="aosDelay1">
                ▪️ 品牌型號雜亂
              </h4>
            </CCol>
          </CRow>
          <h2
            data-aos="fade-up"
            data-aos-duration="1500"
            :data-aos-delay="aosDelay2"
            class="text-center text-md-start mb-3"
          >
            我該怎麼配單？產品是最新的嗎？
          </h2>
          <p data-aos="fade-up" data-aos-duration="1500" :data-aos-delay="aosDelay2" class="mb-3">
            SteamNexus 提供了人性化的配單系統，同時也透過定時爬蟲獲取原價屋最新的產品資料
          </p>
          <h2
            data-aos="fade-up"
            data-aos-duration="1500"
            :data-aos-delay="aosDelay2"
            class="text-center text-md-start mb-3"
          >
            配好了我可以玩多少遊戲？
          </h2>
          <p data-aos="fade-up" data-aos-duration="1500" :data-aos-delay="aosDelay2" class="mb-3">
            我們網站透過 Steam API
            外加爬蟲補助獲取了所有數位遊戲配備需求資料，一配完就可以匹配我們的資料庫，馬上知道能玩多少的遊戲
          </p>
          <div class="d-flex justify-content-center justify-content-md-start">
            <button
              @click="StartBuilder"
              data-aos="fade-up"
              data-aos-duration="1500"
              :data-aos-delay="aosDelay2"
              class="d-flex align-items-center"
            >
              你來對地方了，讓我們開始吧<i class="fa-solid fa-arrow-right"></i>
            </button>
          </div>
        </CCol>
        <CCol xs="12" md="6" xl="5" class="d-flex justify-content-center order-0 order-md-1 mb-4">
          <img
            src="@/assets/images/builder/banner.png"
            alt="Header-img"
            data-aos="fade-down"
            data-aos-duration="1500"
          />
        </CCol>
      </CRow>
    </CContainer>
  </section>
</template>

<script setup>
// AOS 滾動框架動畫
import AOS from 'aos'
import 'aos/dist/aos.css'

// vue module
import { ref, onMounted } from 'vue'

import { CContainer, CRow, CCol } from '@coreui/vue'

let aosDelay1 = ref(500)
let aosDelay2 = ref(1000)

function aosDelayChange() {
  if (window.innerWidth <= 768) {
    aosDelay1.value = 0
    aosDelay2.value = 0
  } else {
    aosDelay1.value = 500
    aosDelay2.value = 1000
  }
}

window.addEventListener('resize', aosDelayChange())

onMounted(() => {
  AOS.init({
    once: true // Animation happens only once
  })

  // 設定 AOS 動畫延遲
  aosDelayChange()
})
</script>

<style scoped>
.banner {
  margin-top: 0;
  padding: 80px 0 100px 0;
  background-image: url('@/assets/images/builder/banner-bg.jpg');
  background-position: top center;
  background-size: cover;
  background-repeat: no-repeat;
}

.banner::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5); /* 使用 rgba 设置透明度 */
  z-index: -1000;
}

.banner .container {
  z-index: 2;
}

.banner .tagline {
  font-weight: 700;
  letter-spacing: 0.8px;
  padding: 8px 10px;
  border-radius: 5px;
  background: linear-gradient(90.21deg, rgba(0, 0, 0, 0.5) -5.91%, rgba(87, 82, 107, 0.5) 111.58%);
  border: 1px solid rgba(255, 255, 255, 0.5);
  font-size: 20px;
  margin-bottom: 16px;
  display: inline-block;
}

.banner h1 {
  font-weight: 600;
  letter-spacing: 0.8px;
  line-height: 1;
  margin-bottom: 10px;
  display: block;
}

.banner h2 {
  font-weight: 500;
  letter-spacing: 0.8px;
  line-height: 1;
  margin-bottom: 10px;
  display: block;
  color: antiquewhite;
}

.banner p {
  color: rgba(255, 255, 255, 0.75);
  font-size: 18px;
  letter-spacing: 0.8px;
  line-height: 1.5em;
  width: 96%;
  text-align: justify;
  margin-bottom: 8px;
}

.banner button {
  color: #fff;
  background-color: transparent;
  border: none;
  font-weight: 700;
  font-size: 20px;
  letter-spacing: 0.8px;
  display: flex;
  align-items: center;
  padding-left: 0;
}

.banner button svg {
  font-size: 25px;
  margin-left: 10px;
  transition: 0.3s ease-in-out;
  line-height: 1;
}

.banner button:hover svg {
  margin-left: 25px;
}

.banner img {
  width: 450px;
  height: 450px;
  animation: updown 3s linear infinite;
}

@media screen and (max-width: 768px) {
  .banner img {
    width: 400px;
    height: 400px;
    animation: none;
  }
}

@keyframes updown {
  0% {
    transform: translateY(-20px);
  }
  50% {
    transform: translateY(20px);
  }
  100% {
    transform: translateY(-20px);
  }
}
</style>
