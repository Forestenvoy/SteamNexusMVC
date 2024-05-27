<template>
  <section class="banner mb-5 mt-0 mt-md-3">
    <CContainer>
      <CRow class="align-items-center">
        <CCol xs="12" md="6" xl="7" class="order-1 order-md-0 text-center">
          <h1 class="display-1 mb-5" :data-aos="animation" data-aos-duration="1000">Game PC</h1>
          <h1 class="display-1 mb-5" :data-aos="animation" data-aos-duration="1000">Builder</h1>
          <div class="d-flex justify-content-center align-items-center">
            <a class="button" href="#buildJourney" :data-aos="animation" data-aos-duration="1000">
              開始
              <svg fill="currentColor" viewBox="0 0 24 24" class="icon">
                <path
                  clip-rule="evenodd"
                  d="M12 2.25c-5.385 0-9.75 4.365-9.75 9.75s4.365 9.75 9.75 9.75 9.75-4.365 9.75-9.75S17.385 2.25 12 2.25zm4.28 10.28a.75.75 0 000-1.06l-3-3a.75.75 0 10-1.06 1.06l1.72 1.72H8.25a.75.75 0 000 1.5h5.69l-1.72 1.72a.75.75 0 101.06 1.06l3-3z"
                  fill-rule="evenodd"
                ></path>
              </svg>
            </a>
          </div>
        </CCol>
        <CCol xs="12" md="6" xl="5" class="d-flex justify-content-center order-0 order-md-1 mb-5">
          <img src="@/assets/images/builder/computer.png" alt="Header-img" />
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

// Core UI
import { CContainer, CRow, CCol } from '@coreui/vue'

let animation = ref('zoom-in')

function aosDelayChange() {
  if (window.innerWidth <= 768) {
    animation.value = 'fade-up'
  } else {
    animation.value = 'zoom-in'
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
@import url('https://fonts.googleapis.com/css2?family=Kaushan+Script&display=swap');

.banner {
  margin-top: 0;
  padding: 80px 0 0 0;
  height: 85vh;
  background-image: url('@/assets/images/builder/banner-bg.jpg');
  background-position: top center;
  background-size: cover;
  background-repeat: no-repeat;
}

.banner h1 {
  font-family: 'Kaushan Script', cursive;
  font-weight: 600;
  letter-spacing: 0.8px;
  line-height: 1;
  display: block;
}

/* 圖片 */
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

@media screen and (max-width: 576px) {
  .banner img {
    width: 200px;
    height: 200px;
    animation: none;
  }

  .banner {
    margin-top: 0;
    padding: 30px 0 50px 0;
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

/* 特殊按鈕 */
.button {
  position: relative;
  transition: all 0.3s ease-in-out;
  box-shadow: 0px 10px 20px rgba(0, 0, 0, 0.2);
  padding-block: 0.5rem;
  padding-inline: 1.25rem;
  background-color: #f3ae0b;
  border-radius: 9999px;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #000;
  gap: 10px;
  font-weight: bold;
  border: 3px solid #ffffff4d;
  outline: none;
  overflow: hidden;
  font-size: 15px;
  text-decoration: none;
  cursor: pointer;
}

.icon {
  width: 24px;
  height: 24px;
  transition: all 0.3s ease-in-out;
}

.button:hover {
  transform: scale(1.05);
  border-color: #fff9;
}

.button:hover .icon {
  transform: translate(4px);
}

.button:hover::before {
  animation: shine 1.5s ease-out infinite;
}

.button::before {
  content: '';
  position: absolute;
  width: 100px;
  height: 100%;
  background-image: linear-gradient(
    120deg,
    rgba(255, 255, 255, 0) 30%,
    rgba(255, 255, 255, 0.8),
    rgba(255, 255, 255, 0) 70%
  );
  top: 0;
  left: -100px;
  opacity: 0.6;
}

@keyframes shine {
  0% {
    left: -100px;
  }

  60% {
    left: 100%;
  }

  to {
    left: 100%;
  }
}
</style>
