<template>
  <section id="ProductList">
    <CContainer class="py-5 d-flex flex-column align-items-center">
      <!-- 標題 -->
      <CRow>
        <CCol class="d-flex align-items-center justify-content-center">
          <h1 class="title display-3">產品列表</h1>
        </CCol>
      </CRow>
      <!-- 產品列表 -->
      <template v-for="product in builderStore.getProductList" :key="product.id">
        <CRow class="item">
          <CCol xs="12" md="10" class="d-flex align-items-center mb-3 mb-md-0">
            <!-- icon -->
            <img src="@/assets/images/builder/hardware.png" alt="icon" class="icon" />
            <!-- 名稱 -->
            <span class="name">{{ product.name }}</span>
          </CCol>
          <CCol
            xs="12"
            md="2"
            class="d-flex align-items-center justify-content-center justify-content-md-end"
          >
            <!-- 價格 -->
            <span class="price me-3 me-md-0">${{ product.price }}</span>
            <!-- 刪除按鈕 -->
            <button
              class="delete d-flex justify-content-center align-items-center"
              :data-id="product.id"
              @click="deleteProduct"
              v-if="builderStore.getMode == 'build'"
            >
              ✘
            </button>
          </CCol>
        </CRow>
        <div class="line"></div>
      </template>
      <!-- 下方列表 -->
      <CRow class="w-100 pt-5">
        <CCol xs="12" md="6"> </CCol>
        <!-- 瓦數 -->
        <CCol
          xs="12"
          md="3"
          class="d-flex justify-content-center justify-content-md-end align-items-center order-0 order-md-1 mb-3"
        >
          <span class="info h5"
            >總瓦數： <span class="wattage">{{ builderStore.totalWattage }}</span> W</span
          >
        </CCol>
        <!-- 價格 -->
        <CCol
          xs="12"
          md="3"
          class="d-flex justify-content-center justify-content-md-end align-items-center order-1 order-md-2 mb-3"
        >
          <span class="info h5"
            >總價格： <span class="price">${{ builderStore.totalPrice }}</span></span
          >
        </CCol>
      </CRow>
      <!-- 可玩遊戲比例核對 -->
      <CRow class="position-relative w-100" v-if="builderStore.getMode == 'build'">
        <CCol xs="12" class="d-flex justify-content-center">
          <button class="match" @click="matchGame">
            Match
            <div class="star-1">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                xml:space="preserve"
                version="1.1"
                style="
                  shape-rendering: geometricPrecision;
                  text-rendering: geometricPrecision;
                  image-rendering: optimizeQuality;
                  fill-rule: evenodd;
                  clip-rule: evenodd;
                "
                viewBox="0 0 784.11 815.53"
                xmlns:xlink="http://www.w3.org/1999/xlink"
              >
                <defs></defs>
                <g id="Layer_x0020_1">
                  <metadata id="CorelCorpID_0Corel-Layer"></metadata>
                  <path
                    class="fil0"
                    d="M392.05 0c-20.9,210.08 -184.06,378.41 -392.05,407.78 207.96,29.37 371.12,197.68 392.05,407.74 20.93,-210.06 184.09,-378.37 392.05,-407.74 -207.98,-29.38 -371.16,-197.69 -392.06,-407.78z"
                  ></path>
                </g>
              </svg>
            </div>
            <div class="star-2">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                xml:space="preserve"
                version="1.1"
                style="
                  shape-rendering: geometricPrecision;
                  text-rendering: geometricPrecision;
                  image-rendering: optimizeQuality;
                  fill-rule: evenodd;
                  clip-rule: evenodd;
                "
                viewBox="0 0 784.11 815.53"
                xmlns:xlink="http://www.w3.org/1999/xlink"
              >
                <defs></defs>
                <g id="Layer_x0020_1">
                  <metadata id="CorelCorpID_0Corel-Layer"></metadata>
                  <path
                    class="fil0"
                    d="M392.05 0c-20.9,210.08 -184.06,378.41 -392.05,407.78 207.96,29.37 371.12,197.68 392.05,407.74 20.93,-210.06 184.09,-378.37 392.05,-407.74 -207.98,-29.38 -371.16,-197.69 -392.06,-407.78z"
                  ></path>
                </g>
              </svg>
            </div>
            <div class="star-3">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                xml:space="preserve"
                version="1.1"
                style="
                  shape-rendering: geometricPrecision;
                  text-rendering: geometricPrecision;
                  image-rendering: optimizeQuality;
                  fill-rule: evenodd;
                  clip-rule: evenodd;
                "
                viewBox="0 0 784.11 815.53"
                xmlns:xlink="http://www.w3.org/1999/xlink"
              >
                <defs></defs>
                <g id="Layer_x0020_1">
                  <metadata id="CorelCorpID_0Corel-Layer"></metadata>
                  <path
                    class="fil0"
                    d="M392.05 0c-20.9,210.08 -184.06,378.41 -392.05,407.78 207.96,29.37 371.12,197.68 392.05,407.74 20.93,-210.06 184.09,-378.37 392.05,-407.74 -207.98,-29.38 -371.16,-197.69 -392.06,-407.78z"
                  ></path>
                </g>
              </svg>
            </div>
            <div class="star-4">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                xml:space="preserve"
                version="1.1"
                style="
                  shape-rendering: geometricPrecision;
                  text-rendering: geometricPrecision;
                  image-rendering: optimizeQuality;
                  fill-rule: evenodd;
                  clip-rule: evenodd;
                "
                viewBox="0 0 784.11 815.53"
                xmlns:xlink="http://www.w3.org/1999/xlink"
              >
                <defs></defs>
                <g id="Layer_x0020_1">
                  <metadata id="CorelCorpID_0Corel-Layer"></metadata>
                  <path
                    class="fil0"
                    d="M392.05 0c-20.9,210.08 -184.06,378.41 -392.05,407.78 207.96,29.37 371.12,197.68 392.05,407.74 20.93,-210.06 184.09,-378.37 392.05,-407.74 -207.98,-29.38 -371.16,-197.69 -392.06,-407.78z"
                  ></path>
                </g>
              </svg>
            </div>
            <div class="star-5">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                xml:space="preserve"
                version="1.1"
                style="
                  shape-rendering: geometricPrecision;
                  text-rendering: geometricPrecision;
                  image-rendering: optimizeQuality;
                  fill-rule: evenodd;
                  clip-rule: evenodd;
                "
                viewBox="0 0 784.11 815.53"
                xmlns:xlink="http://www.w3.org/1999/xlink"
              >
                <defs></defs>
                <g id="Layer_x0020_1">
                  <metadata id="CorelCorpID_0Corel-Layer"></metadata>
                  <path
                    class="fil0"
                    d="M392.05 0c-20.9,210.08 -184.06,378.41 -392.05,407.78 207.96,29.37 371.12,197.68 392.05,407.74 20.93,-210.06 184.09,-378.37 392.05,-407.74 -207.98,-29.38 -371.16,-197.69 -392.06,-407.78z"
                  ></path>
                </g>
              </svg>
            </div>
            <div class="star-6">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                xml:space="preserve"
                version="1.1"
                style="
                  shape-rendering: geometricPrecision;
                  text-rendering: geometricPrecision;
                  image-rendering: optimizeQuality;
                  fill-rule: evenodd;
                  clip-rule: evenodd;
                "
                viewBox="0 0 784.11 815.53"
                xmlns:xlink="http://www.w3.org/1999/xlink"
              >
                <defs></defs>
                <g id="Layer_x0020_1">
                  <metadata id="CorelCorpID_0Corel-Layer"></metadata>
                  <path
                    class="fil0"
                    d="M392.05 0c-20.9,210.08 -184.06,378.41 -392.05,407.78 207.96,29.37 371.12,197.68 392.05,407.74 20.93,-210.06 184.09,-378.37 392.05,-407.74 -207.98,-29.38 -371.16,-197.69 -392.06,-407.78z"
                  ></path>
                </g>
              </svg>
            </div>
          </button>
        </CCol>
        <transition name="fade">
          <span class="warning" v-if="check">請至少選擇 CPU、GPU 和 RAM </span>
        </transition>
      </CRow>
    </CContainer>
  </section>
</template>

<script setup>
// vue
import { ref } from 'vue'

// Core UI
import { CContainer, CRow, CCol } from '@coreui/vue'

// pinia
import { useBuilderStore } from '@/stores/builder.js'
const builderStore = useBuilderStore()

// API URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

// 檢驗
let check = ref(false)

// 刪除產品
const deleteProduct = (event) => {
  // 取得 id
  const id = event.target.getAttribute('data-id')
  // 刪除產品
  builderStore.removeProduct(id)
}

// 遊戲匹配
const matchGame = () => {
  // 取得 產品列表
  const products = builderStore.getProductList

  // 檢驗是否至少有選擇 CPU、GPU、RAM
  const hasCPU = products.some((product) => product.type === 'CPU')
  const hasGPU = products.some((product) => product.type === 'GPU')
  const hasRAM = products.some((product) => product.type === 'RAM')

  if (!hasCPU || !hasGPU || !hasRAM) {
    check.value = true
    // fade
    setTimeout(() => {
      // 文字淡出
      check.value = false
    }, 2000)
    return
  }
  // 遊戲核對
  calculateRatio()
}

// 發送產品ID 計算遊戲匹配比例
const calculateRatio = async () => {
  // post
  await fetch(`${apiUrl}/api/PcBuilder/CalculateRatio`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      pCpuId: builderStore.getCPUId,
      pGpuId: builderStore.getGPUId,
      pRamId: builderStore.getRAMId
    })
  })
    .then((response) => {
      if (!response.ok) {
        return response.text().then((errorMessage) => {
          throw new Error(errorMessage)
        })
      }
      return response.json()
    })
    .then((data) => {
      console.log(data)
      builderStore.setMinRatio(data.min)
      builderStore.setRecRatio(data.rec)
      builderStore.setMinCount(data.minCount)
      builderStore.setRecCount(data.recCount)
      builderStore.showMatch()
    })
    .catch((error) => {
      console.error('Error:', error.message)
    })
}
</script>

<style scoped>
.Container {
  width: 100%;
}

/* icon */
.icon {
  width: 80px;
  height: 80px;
  margin-right: 15px;
}

/* 名稱 */
.name {
  display: inline-block;
  max-width: 100%;
  white-space: nowrap;
  overflow: auto;
  padding-right: 20px;
  -ms-overflow-style: none; /* IE 和 Edge */
  scrollbar-width: none; /* Firefox */
}

.name::-webkit-scrollbar {
  display: none;
}

.item {
  width: 100%;
  padding-top: 10px;
  padding-bottom: 10px;
  margin-top: 8px;
}

/* 錯誤訊息 */
.warning {
  width: 100%;
  position: absolute;
  text-align: center;
  bottom: -65%;
  left: 50%;
  transform: translateX(-50%);
  font-size: 20px;
  color: #ff0000;
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.5s ease-in-out;
}

.fade-enter, .fade-leave-to /* .fade-leave-active in <2.1.8 */ {
  opacity: 0;
}

/* 瓦數 */
.wattage {
  color: #f3ae0b;
}

/* 價格 */
.price {
  color: #00ff00;
}

/* 刪除按鈕 */
.delete {
  width: 30px;
  height: 30px;
  padding: 5px 5px;
  margin-left: 10px;
  border-radius: 0;
  cursor: pointer;
  font-size: 20px;
  background-color: transparent;
  color: #ff0000;
}

@media screen and (max-width: 576px) {
  .delete {
    width: 20px;
    height: 20px;
    font-size: 16px;
    margin-left: 0;
  }
}

/* 線 */
.line {
  width: 100%;
  height: 2px;
  background: linear-gradient(to right, #000, rgba(255, 255, 255), #000);
  margin-top: 5px;
}

/* 匹配按鈕 */
.match {
  position: relative;
  padding: 12px 35px;
  background: #f3ae0b;
  font-size: 17px;
  font-weight: 500;
  color: #181818;
  border: 3px solid #f3ae0b;
  border-radius: 8px;
  box-shadow: 0 0 0 #ba8509;
  transition: all 0.3s ease-in-out;
  cursor: pointer;
}

.star-1 {
  position: absolute;
  top: 20%;
  left: 20%;
  width: 25px;
  height: auto;
  filter: drop-shadow(0 0 0 #fffdef);
  z-index: -5;
  transition: all 1s cubic-bezier(0.05, 0.83, 0.43, 0.96);
}

.star-2 {
  position: absolute;
  top: 45%;
  left: 45%;
  width: 15px;
  height: auto;
  filter: drop-shadow(0 0 0 #fffdef);
  z-index: -5;
  transition: all 1s cubic-bezier(0, 0.4, 0, 1.01);
}

.star-3 {
  position: absolute;
  top: 40%;
  left: 40%;
  width: 5px;
  height: auto;
  filter: drop-shadow(0 0 0 #fffdef);
  z-index: -5;
  transition: all 1s cubic-bezier(0, 0.4, 0, 1.01);
}

.star-4 {
  position: absolute;
  top: 20%;
  left: 40%;
  width: 8px;
  height: auto;
  filter: drop-shadow(0 0 0 #fffdef);
  z-index: -5;
  transition: all 0.8s cubic-bezier(0, 0.4, 0, 1.01);
}

.star-5 {
  position: absolute;
  top: 25%;
  left: 45%;
  width: 15px;
  height: auto;
  filter: drop-shadow(0 0 0 #fffdef);
  z-index: -5;
  transition: all 0.6s cubic-bezier(0, 0.4, 0, 1.01);
}

.star-6 {
  position: absolute;
  top: 5%;
  left: 50%;
  width: 5px;
  height: auto;
  filter: drop-shadow(0 0 0 #fffdef);
  z-index: -5;
  transition: all 0.8s ease;
}

.match:hover {
  background: transparent;
  color: #f3ae0b;
  box-shadow: 0 0 25px #ba8509;
}

.match:hover .star-1 {
  position: absolute;
  top: -80%;
  left: -30%;
  width: 25px;
  height: auto;
  filter: drop-shadow(0 0 10px #fffdef);
  z-index: 2;
}

.match:hover .star-2 {
  position: absolute;
  top: -25%;
  left: 10%;
  width: 15px;
  height: auto;
  filter: drop-shadow(0 0 10px #fffdef);
  z-index: 2;
}

.match:hover .star-3 {
  position: absolute;
  top: 55%;
  left: 25%;
  width: 5px;
  height: auto;
  filter: drop-shadow(0 0 10px #fffdef);
  z-index: 2;
}

.match:hover .star-4 {
  position: absolute;
  top: 30%;
  left: 80%;
  width: 8px;
  height: auto;
  filter: drop-shadow(0 0 10px #fffdef);
  z-index: 2;
}

.match:hover .star-5 {
  position: absolute;
  top: 25%;
  left: 115%;
  width: 15px;
  height: auto;
  filter: drop-shadow(0 0 10px #fffdef);
  z-index: 2;
}

.match:hover .star-6 {
  position: absolute;
  top: 5%;
  left: 60%;
  width: 5px;
  height: auto;
  filter: drop-shadow(0 0 10px #fffdef);
  z-index: 2;
}

.fil0 {
  fill: #fffdef;
}
</style>
