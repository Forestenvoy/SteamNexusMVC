<template>
  <section class="banner" data-aos="fade-up">
    <CContainer>
      <CRow class="align-items-center">
        <CCol xs="12" md="6" xl="7">
          <span class="tagline">歡迎來到硬體構建系統</span>
          <h1>我想組一台遊戲機</h1>
          <h1>但硬體看不懂？</h1>
          <p>
            Lorem ipsum dolor sit, amet consectetur adipisicing elit. Voluptatem delectus ex
            quibusdam cumque obcaecati corrupti harum ipsum ipsa quaerat vel, nostrum iste aut
            similique? Suscipit vel unde accusamus quibusdam maxime.
          </p>
          <button @click="StartBuilder">Let's Start <i class="fa-solid fa-arrow-right"></i></button>
        </CCol>
        <CCol xs="12" md="6" xl="5" class="d-flex justify-content-center">
          <img src="@/assets/images/builder/banner.png" alt="Header-img" />
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
  margin-bottom: 20px;
  display: block;
}

.banner p {
  color: rgba(255, 255, 255, 0.75);
  font-size: 18px;
  letter-spacing: 0.8px;
  line-height: 1.5em;
  width: 96%;
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
