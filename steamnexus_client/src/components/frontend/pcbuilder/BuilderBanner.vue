<template>
  <section class="banner" data-aos="zoom-in-down" data-aos-duration="500">
    <CContainer>
      <CRow class="align-items-center">
        <CCol xs="12" md="6" xl="7">
          <h1 data-aos="fade-right" data-aos-duration="1200">一般人想組台遊戲用桌上型電腦</h1>
          <h2 data-aos="fade-right" data-aos-duration="1400">
            打開電腦硬體零售商的網站，看不懂 <CIcon icon="cilFrown" size="xl" />
          </h2>
          <h3>- 缺乏硬體基礎知識 -</h3>
          <p>
            電腦硬體種類繁多，包括中央處理器、圖形處理器、主機板、記憶體、儲存設備、散熱系統、電源供應器。且每種硬體都有許多技術參數，如處理器的核心數量和時脈速度，顯示卡的顯存大小和架構，記憶體的容量和頻率等。理解這些參數以及它們之間的相互影響需要一定的技術知識。
          </p>
          <h3>- 配件兼容性複雜 -</h3>
          <p>
            選擇不相容的組件會導致系統無法正常工作或性能受限。像是主機板和中央處理器必須匹配特定的插槽類型、電源必須提供足夠的功率以支撐所有硬體等。
          </p>
          <h3>- 科技日新月異 -</h3>
          <p>
            每種硬體都有許多品牌和型號，需要仔細挑選和匹配。但技術更新速度很快，新產品不斷推出，市場上存在大量的訊息和選擇。
          </p>
          <h2>我該怎麼配單？配好了可以玩多少遊戲？</h2>
          <p>
            SteamNexus 提供了人性化的配單系統，同時可以讓你匹配我們網站內的數位遊戲資料，一配完就可以知道能玩多少的遊戲
          </p>
          <button @click="StartBuilder" data-aos="fade-right" data-aos-duration="2500">
            你來對地方了，讓我們開始吧<i class="fa-solid fa-arrow-right"></i>
          </button>
        </CCol>
        <CCol xs="12" md="6" xl="5" class="d-flex justify-content-center">
          <img
            src="@/assets/images/builder/banner.png"
            alt="Header-img"
            data-aos="fade-left"
            data-aos-duration="2500"
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
import { onMounted } from 'vue'

import { CContainer, CRow, CCol } from '@coreui/vue'

onMounted(() => {
  AOS.init()
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
  font-weight: 700;
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
