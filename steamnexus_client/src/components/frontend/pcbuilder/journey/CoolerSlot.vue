<template>
  <CRow>
    <!-- 左側 -->
    <CCol xs="12" md="6" class="mb-5 mb-md-0 d-flex flex-column justify-content-center">
      <!-- 切換 ~ 風冷、水冷 -->
      <div class="d-flex justify-content-center align-items-center mb-4">
        <input id="checkbox_toggle" type="checkbox" class="check" v-model="toggle" />
        <div class="checkbox">
          <label class="slide" for="checkbox_toggle">
            <label class="toggle" for="checkbox_toggle"></label>
            <label class="text" for="checkbox_toggle">風冷</label>
            <label class="text" for="checkbox_toggle">水冷</label>
          </label>
        </div>
      </div>
      <!-- 標題 -->
      <h3 class="title text-center">{{ !toggle ? '風冷散熱器' : '水冷散熱器' }}</h3>
      <!-- 圖片 -->
      <div class="d-flex justify-content-center align-items-center">
        <img
          src="@/assets/images/builder/aircooler.png"
          alt="Motherboard"
          class="image"
          v-if="!toggle"
        />
        <img
          src="@/assets/images/builder/liquidcooler.png"
          alt="Motherboard"
          class="image"
          v-else
        />
      </div>
    </CCol>
    <!-- 右側 -->
    <CCol xs="12" md="6" class="d-flex flex-column justify-content-center">
      <!-- 風冷散熱器 -->
      <div v-show="!toggle">
        <air-cooler :default-selected="A" @selected="updateA"></air-cooler>
      </div>
      <!-- 水冷散熱器 -->
      <div v-show="toggle">
        <liquid-cooler :default-selected="L" @selected="updateL"></liquid-cooler>
      </div>
      <!-- 換頁 -->
      <CRow class="mb-3">
        <CCol xs="6" class="d-flex justify-content-center">
          <!-- 上一頁 -->
          <button class="back-btn" @click="builderStore.prev()">
            <svg
              width="34"
              height="34"
              viewBox="0 0 74 74"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
            >
              <circle cx="37" cy="37" r="35.5" stroke="black" stroke-width="3"></circle>
              <path
                d="M49 37C49 36.1716 48.3284 35.5 47.5 35.5V38.5C48.3284 38.5 49 37.8284 49 37ZM24.9393 35.9393C24.3536 36.5251 24.3536 37.4749 24.9393 38.0607L34.4853 47.6066C35.0711 48.1924 36.0208 48.1924 36.6066 47.6066C37.1924 47.0208 37.1924 46.0711 36.6066 45.4853L28.1213 37L36.6066 28.5147C37.1924 27.9289 37.1924 26.9792 36.6066 26.3934C36.0208 25.8076 35.0711 25.8076 34.4853 26.3934L24.9393 35.9393ZM49 38.5H26V35.5H49V38.5Z"
                fill="black"
              ></path>
            </svg>
            <span>Back</span>
          </button>
        </CCol>
        <CCol xs="6" class="d-flex justify-content-center">
          <!-- 下一頁 -->
          <button class="next-btn" @click="builderStore.next()">
            <span>Next</span>
            <svg
              width="34"
              height="34"
              viewBox="0 0 74 74"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
            >
              <circle cx="37" cy="37" r="35.5" stroke="black" stroke-width="3"></circle>
              <path
                d="M25 35.5C24.1716 35.5 23.5 36.1716 23.5 37C23.5 37.8284 24.1716 38.5 25 38.5V35.5ZM49.0607 38.0607C49.6464 37.4749 49.6464 36.5251 49.0607 35.9393L39.5147 26.3934C38.9289 25.8076 37.9792 25.8076 37.3934 26.3934C36.8076 26.9792 36.8076 27.9289 37.3934 28.5147L45.8787 37L37.3934 45.4853C36.8076 46.0711 36.8076 47.0208 37.3934 47.6066C37.9792 48.1924 38.9289 48.1924 39.5147 47.6066L49.0607 38.0607ZM25 38.5L48 38.5V35.5L25 35.5V38.5Z"
                fill="black"
              ></path>
            </svg>
          </button>
        </CCol>
      </CRow>
    </CCol>
  </CRow>
</template>

<script setup>
import AirCooler from '@/components/frontend/pcbuilder/journey/AirCooler.vue'
import LiquidCooler from '@/components/frontend/pcbuilder/journey/LiquidCooler.vue'

// vue
import { ref } from 'vue'

// pinia
import { useBuilderStore } from '@/stores/builder.js'
const builderStore = useBuilderStore()

// 風冷、水冷 切換開關
const toggle = ref(false)

const A = ref(0)
const L = ref(0)

const updateA = (newValue) => {
  A.value = newValue
  if (newValue !== 0) {
    L.value = 0
  }
}

const updateL = (newValue) => {
  L.value = newValue
  if (newValue !== 0) {
    A.value = 0
  }
}
</script>

<style scoped>
/* 標題 */
.title {
  font-size: 2.5rem;
  font-weight: 700;
  letter-spacing: 2px;
  margin-bottom: 40px;
}

/* 圖片 */
.image {
  width: 300px;
  height: 300px;
}

@media screen and (max-width: 576px) {
  .image {
    width: 200px;
    height: 200px;
  }
}

/* 風冷、水冷 切換 */
.checkbox {
  width: 223px;
  height: 60px;
  background-color: #d0d0d0;
  border-radius: 30px;
  position: relative;
  color: black;
  overflow: hidden;
}

#checkbox_toggle {
  display: none;
}

.checkbox .toggle {
  width: 100px;
  height: 50px;
  position: absolute;
  border-radius: 30px;
  left: 5px;
  cursor: pointer;
  background: linear-gradient(40deg, #ff0080, #ff8c00 70%);
  transition: 0.4s;
  box-shadow:
    0px 0px 3px rgb(255, 255, 20),
    0px 0px 5px rgb(255, 255, 20);
}

.checkbox .slide {
  width: 230px;
  height: 60px;
  display: flex;
  align-items: center;
  justify-content: space-around;
  cursor: pointer;
}

.checkbox .slide .text {
  font-size: 16px;
  font-weight: 700;
  z-index: 100;
  cursor: pointer;
}

.check:checked + .checkbox .slide .toggle {
  transform: translateX(113px);
  background: linear-gradient(40deg, #8983f7, #a3dafb 70%);
  box-shadow:
    -0px -0px 10px #8983f7,
    -0px -0px 3px #8983f7;
}

.check:checked + .checkbox .slide {
  background-color: #0a1929;
  color: #fff;
}
/* 上一頁、下一頁 */
button {
  cursor: pointer;
  font-weight: 700;
  font-family: Helvetica, 'sans-serif';
  transition: all 0.2s;
  padding: 10px 20px;
  border-radius: 100px;
  color: #000;
  background: #f3ae0b;
  border: 1px solid transparent;
  display: flex;
  align-items: center;
  font-size: 15px;
}

button:hover {
  background: #f3ae0b;
}

.back-btn > svg {
  width: 34px;
  margin-right: 10px;
  transition: transform 0.3s ease-in-out;
}

.next-btn > svg {
  width: 34px;
  margin-left: 10px;
  transition: transform 0.3s ease-in-out;
}

.back-btn:hover svg {
  transform: translateX(-5px);
}

.next-btn:hover svg {
  transform: translateX(5px);
}

button:active {
  transform: scale(0.95);
}
</style>
