<template>
  <section class="game-ratio mx-1 mx-md-5 mb-5" data-aos="zoom-in" data-aos-duration="1000">
    <CContainer>
      <!-- 標題 -->
      <CRow class="mb-3">
        <CCol class="text-center">
          <h1 class="title display-3">可玩的遊戲比例</h1>
        </CCol>
      </CRow>
      <!-- 說明 -->
      <CRow class="mb-3">
        <CCol>
          <p class="description text-start text-md-center">
            SteamNexus 會定期更新資料庫內的遊戲需求配備資料，確保你匹配的數據是最新的
          </p>
        </CCol>
      </CRow>
      <!-- 圓形進度條 -->
      <CRow>
        <CCol xs="12" md="6" class="mb-5 mb-md-0">
          <h2 class="text-center mb-3">滿足最低配備</h2>
          <div class="ratio-card">
            <div class="rating">
              <h2>
                <span class="counter">{{ valueMin }}</span
                ><span class="percent">%</span>
              </h2>
              <!-- 生成進度條 -->
              <div
                class="block"
                v-for="i in 100"
                :key="i"
                :style="{
                  transform: `rotate(${i * 3.6}deg)`,
                  animationDelay: `${i / 40}s`,
                  backgroundColor: i <= progressMin ? '#0f0' : '#4f4d4d', // 根據進度設置顏色
                  boxShadow: i <= progressMin ? '0 0 15px #0f0' : '0 0 15px #4f4d4d'
                }"
              ></div>
            </div>
          </div>
        </CCol>
        <CCol xs="12" md="6">
          <h2 class="text-center mb-3">滿足建議配備</h2>
          <div class="ratio-card">
            <div class="rating">
              <h2>
                <span class="counter">{{ valueMax }}</span
                ><span class="percent">%</span>
              </h2>
              <!-- 生成進度條 -->
              <div
                class="block"
                v-for="i in 100"
                :key="i"
                :style="{
                  transform: `rotate(${i * 3.6}deg)`,
                  animationDelay: `${i / 40}s`,
                  backgroundColor: i <= progressMax ? '#0f0' : '#4f4d4d', // 根據進度設置顏色
                  boxShadow: i <= progressMax ? '0 0 15px #0f0' : '0 0 15px #4f4d4d'
                }"
              ></div>
            </div>
          </div>
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

// 進度條進度
let progressMin = ref(70)
let valueMin = ref(0)
const progressMax = ref(50)
const valueMax = ref(0)

// 數字特效
const NumberCounter = (target, counter) => {
  if (counter.value < target) {
    counter.value++
    setTimeout(() => NumberCounter(target, counter), 25)
  }
}

onMounted(() => {
  AOS.init({
    once: true // Animation happens only once
  })
  NumberCounter(progressMin.value, valueMin)
  NumberCounter(progressMax.value, valueMax)
})
</script>

<style scoped>
/* 容器 */
.game-ratio {
  position: relative;
  padding: 50px 50px 50px 50px;
  background-color: #313131;
  border-radius: 25px;
}

/* 標題 */
.title {
  font-family: 'Oswald', sans-serif;
}

/* 說明 */
.description {
  font-size: 20px;
  font-weight: 300;
  line-height: 1.2;
  color: #fff;
}

/* 圓形進度條 */
.ratio-card {
  display: flex;
  justify-content: center;
  align-items: center;
  position: relative;
  height: 200px;
  padding: 0;
}

.rating {
  position: relative;
  width: 100%;
  height: 100%;
}

.block {
  position: absolute;
  width: 2px;
  height: 15px;
  background-color: #4f4d4d;
  left: 50%;
  transform-origin: 50% 100px;
  opacity: 0;
  animation: animate 0.1s linear forwards;
}

@keyframes animate {
  to {
    opacity: 1;
  }
}

.ratio-card h2 {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  color: #fff;
  font-size: 1.5em;
  font-weight: 500;
  text-align: center;
  padding: 0;
  display: flex;
  align-items: center;
}

.counter {
  font-size: 2em;
  font-weight: 700;
}

.percent {
  font-size: 1.5em;
  font-weight: 300;
}
</style>
