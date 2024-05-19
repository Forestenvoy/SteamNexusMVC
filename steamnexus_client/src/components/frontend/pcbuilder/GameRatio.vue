<template>
  <section class="game-ratio">
    <CContainer>
      <!-- 標題 -->
      <CRow>
        <CCol class="text-center">
          <h1 class="title">Playable Game Ratio</h1>
        </CCol>
      </CRow>
      <!-- 說明 -->
      <CRow>
        <CCol>
          <p>
            Lorem ipsum dolor sit amet consectetur adipisicing elit. Doloremque provident quibusdam,
            aliquam odio nostrum dolorem sint natus aliquid veritatis tempora voluptas voluptatibus
            omnis eveniet voluptatum accusantium fugit molestias modi saepe?
          </p>
        </CCol>
      </CRow>
      <!-- 圓形進度條 -->
      <CRow>
        <CCol xs="12" md="6">
          <h2 class="text-center mb-3">最低配備</h2>
          <div class="ratio-box">
            <div class="rating">
              <h2>
                <span class="counter">{{ valueMin }}</span
                ><sup class="percent">%</sup>
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
          <h2 class="text-center mb-3">建議配備</h2>
          <div class="ratio-box">
            <div class="rating">
              <h2>
                <span class="counter">{{ valueMax }}</span
                ><sup class="percent">%</sup>
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
// vue
import { ref, onMounted } from 'vue'

// Core UI
import { CContainer, CRow, CCol } from '@coreui/vue'

// 進度條進度
let progressMin = ref(70)
let valueMin = ref(0)
const progressMax = ref(100)
const valueMax = ref(0)

// 數字特效
const NumberCounter = (target, counter) => {
  if (counter.value < target) {
    counter.value++
    setTimeout(() => NumberCounter(target, counter), 25)
  }
}

onMounted(() => {
  NumberCounter(progressMin.value, valueMin)
  NumberCounter(progressMax.value, valueMax)
})
</script>

<style scoped>
/* 標題 */
.game-ratio .title {
  font-family: 'Oswald', sans-serif;
  font-size: calc(1.525rem + 3.3vw);
  font-weight: 300;
  line-height: 1.2;
}

/* 圓形進度條 */

.ratio-box {
  display: flex;
  justify-content: center;
  align-items: center;
  position: relative;
  height: 200px;
  padding: 0;
}

.ratio-box .rating {
  position: relative;
  width: 100%;
  height: 100%;
}

.ratio-box .rating .block {
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

.ratio-box h2 {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  color: #fff;
  font-size: 1.5em;
  font-weight: 500;
  text-align: center;
  padding: 0;
}

.counter {
  font-size: 2.5em;
  font-weight: 700;
}

.percent {
  font-size: 1.5em;
  font-weight: 300;
  vertical-align: middle;
}
</style>
