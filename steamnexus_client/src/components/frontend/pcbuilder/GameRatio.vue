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
          <CRow>
            <CCol xs="12" class="mb-5">
              <h2 class="text-center mb-3">滿足最低配備</h2>
              <div class="ratio-card">
                <div class="rating">
                  <h2>
                    <span class="counter">{{ startMin }}</span
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
                      backgroundColor: i <= targetMin ? '#0f0' : '#4f4d4d', // 根據進度設置顏色
                      boxShadow: i <= targetMin ? '0 0 15px #0f0' : '0 0 15px #4f4d4d'
                    }"
                  ></div>
                </div>
              </div>
            </CCol>
            <!-- 最低配備 遊戲列表 -->
            <CCol xs="12" class="d-flex justify-content-center">
              <button class="GameList" @click="openGameListM">遊戲列表</button>
            </CCol>
          </CRow>
        </CCol>
        <CCol xs="12" md="6">
          <CRow>
            <CCol xs="12" class="mb-5">
              <h2 class="text-center mb-3">滿足建議配備</h2>
              <div class="ratio-card">
                <div class="rating">
                  <h2>
                    <span class="counter">{{ startRec }}</span
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
                      backgroundColor: i <= targetRec ? '#0f0' : '#4f4d4d', // 根據進度設置顏色
                      boxShadow: i <= targetRec ? '0 0 15px #0f0' : '0 0 15px #4f4d4d'
                    }"
                  ></div>
                </div>
              </div>
            </CCol>
            <!-- 建議配備 遊戲列表 -->
            <CCol xs="12" class="d-flex justify-content-center">
              <button class="GameList" @click="openGameListR">遊戲列表</button>
            </CCol>
          </CRow>
        </CCol>
      </CRow>
    </CContainer>
  </section>
  <!-- 遊戲列表互動視窗 -->
  <CModal
    alignment="center"
    size="lg"
    :visible="visibleGameList"
    @close="
      () => {
        visibleGameList = false
        keyword = ''
        currentPage = 1
      }
    "
    aria-labelledby="GameListLabel"
    style="z-index: 100000"
  >
    <CModalHeader>
      <CModalTitle id="GameListLabel">遊戲列表</CModalTitle>
    </CModalHeader>
    <!-- 列表內容 -->
    <CModalBody>
      <CRow class="mb-3">
        <CCol xs="12" md="6" class="d-flex justify-content-center align-items-center">
          <div class="input-wrapper">
            <input
              class="input-box text-center"
              type="text"
              placeholder="&#x1F50D;&#xFE0E;  請輸入關鍵字"
              v-model="keyword"
            />
            <span class="underline"></span>
          </div>
          <button class="btn-search ms-2" @click="filter">搜尋</button>
        </CCol>
        <CCol xs="12" md="6" class="d-flex justify-content-center align-items-center">
          <!-- 頁數 -->
          <label for="gamePage" class="pe-2 h5">頁數 : </label>
          <select
            name="gamePage"
            class="form-select page"
            v-model="currentPage"
            @change="pageChange"
          >
            <option v-for="page in totalPages" :key="page" :value="page">
              {{ page }}
            </option>
          </select>
          <span class="mx-3">/</span>
          <span>{{ totalPages }}</span>
        </CCol>
      </CRow>
      <CRow>
        <CCol xs="12">
          <template v-for="game in gameList" :key="game.gameId">
            <CRow class="item">
              <CCol xs="12" class="d-flex justify-content-center align-items-center mb-3 mb-md-0">
                <a class="name" @click="handleGameClick(game.gameId)">{{ game.name }}</a>
              </CCol>
            </CRow>
            <div class="line"></div>
          </template>
        </CCol>
      </CRow>
    </CModalBody>
    <CModalFooter>
      <CButton
        color="secondary"
        @click="
          () => {
            visibleGameList = false
            keyword = ''
            currentPage = 1
          }
        "
      >
        關閉
      </CButton>
    </CModalFooter>
  </CModal>
</template>

<script setup>
// AOS 滾動框架動畫
import AOS from 'aos'
import 'aos/dist/aos.css'

// vue
import { ref, onMounted, watch } from 'vue'

// vue router
import { useRouter } from 'vue-router'
const router = useRouter()

// Core UI
import { CContainer, CRow, CCol } from '@coreui/vue'

// pinia
import { useBuilderStore } from '@/stores/builder.js'
const builderStore = useBuilderStore()

// 最低配備 ~ 目標進度值
let targetMin = ref(0)
// 最低配備 ~  起始值
let startMin = ref(0)
// 建議配備 ~ 目標進度值
const targetRec = ref(0)
// 建議配備 ~ 起始值
const startRec = ref(0)

// API URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

// 遊戲列表互動視窗
const visibleGameList = ref(false)
// 關鍵字
const keyword = ref('')
// 頁數
const totalPages = ref(0)
const currentPage = ref(1)
// 遊戲列表
const gameList = ref([])

// 數字特效
const NumberCounter = (target, counter) => {
  if (counter.value < target) {
    counter.value++
    NumberCounter(target, counter)
  }
}

// watch 目標進度值
watch(
  () => builderStore.getMinRatio,
  (newValue) => {
    startMin.value = 0
    targetMin.value = newValue
    NumberCounter(targetMin.value, startMin)
  }
)
watch(
  () => builderStore.getRecRatio,
  (newValue) => {
    startRec.value = 0
    targetRec.value = newValue
    NumberCounter(targetRec.value, startRec)
  }
)

// 打開最低配備遊戲選單
const openGameListM = () => {
  visibleGameList.value = true
  builderStore.setRatioMode('min')
  getGameList()
  totalPages.value = Math.floor(builderStore.getMinCount / 10) + 1
}
// 打開建議配備遊戲選單
const openGameListR = () => {
  visibleGameList.value = true
  builderStore.setRatioMode('rec')
  getGameList()
  totalPages.value = Math.floor(builderStore.getRecCount / 10) + 1
}

// 獲取遊戲列表內容
const getGameList = async () => {
  // post
  await fetch(
    `${apiUrl}/api/PcBuilder/GetGameList?mode=${builderStore.getRatioMode}&page=${currentPage.value}&keyword=${keyword.value}`,
    {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        pCpuId: builderStore.getCPUId,
        pGpuId: builderStore.getGPUId,
        pRamId: builderStore.getRAMId
      })
    }
  )
    .then((response) => {
      if (!response.ok) {
        return response.text().then((errorMessage) => {
          throw new Error(errorMessage)
        })
      }
      return response.json()
    })
    .then((data) => {
      gameList.value = data
      console.log(data)
    })
    .catch((error) => {
      console.error('Error:', error.message)
    })
}

// 關鍵字搜尋
const filter = () => {
  // 切回至第一頁
  currentPage.value = 1
  reCalculatePage()
  getGameList()
}

// 重新計算頁數
const reCalculatePage = async () => {
  await fetch(`${apiUrl}/api/PcBuilder/calculateQuantityByKeyword`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      pCpuId: builderStore.getCPUId,
      pGpuId: builderStore.getGPUId,
      pRamId: builderStore.getRAMId,
      keyword: keyword.value,
      mode: builderStore.getRatioMode
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
      totalPages.value = Math.floor(data.count / 10) + 1
    })
    .catch((error) => {
      console.error('Error:', error.message)
    })
}

// 頁數搜尋
const pageChange = () => {
  getGameList()
}

// 遊戲頁面跳轉
const handleGameClick = (gameId) => {
  if (visibleGameList.value) {
    visibleGameList.value = false
  }
  router.push(`/game/${gameId}`)
}

onMounted(() => {
  AOS.init({
    once: true // Animation happens only once
  })
  targetMin.value = builderStore.getMinRatio
  targetRec.value = builderStore.getRecRatio
  NumberCounter(targetMin.value, startMin)
  NumberCounter(targetRec.value, startRec)
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

/* 遊戲列表按鈕 */
.GameList {
  background:
    linear-gradient(140.14deg, #f3ae0b 15.05%, #c58a00 114.99%) padding-box,
    linear-gradient(142.51deg, #ff9465 8.65%, #ddb608 88.82%) border-box;
  border-radius: 7px;
  border: 2px solid transparent;

  text-shadow: 1px 1px 1px #00000040;
  box-shadow: 8px 8px 20px 0px #45090059;

  padding: 10px 40px;
  line-height: 20px;
  cursor: pointer;
  transition: all 0.3s;
  color: #000;
  font-size: 18px;
  font-weight: 500;
}

.GameList:hover {
  box-shadow: none;
  opacity: 80%;
}

/* 關鍵字 */
.input-wrapper {
  position: relative;
  width: 180px;
}

.input-box {
  font-size: 16px;
  padding: 5px 0;
  border: none;
  border-bottom: 2px solid #ccc;
  color: #fff;
  width: 100%;
  background-color: transparent;
  transition: border-color 0.3s ease-in-out;
}

.underline {
  position: absolute;
  bottom: 0;
  left: 0;
  width: 180px;
  height: 2px;
  background-color: #f3ae0b;
  transform: scaleX(0);
  transition: transform 0.3s ease-in-out;
}

.input-box:focus {
  border-color: #a47609;
  outline: none;
}

.input-box:focus + .underline {
  transform: scaleX(1);
}

/* 頁數 */
.page {
  width: 80px;
  text-align: center;
}

/* 項目 */
.item {
  width: 100%;
  padding-top: 10px;
  padding-bottom: 10px;
  margin-top: 8px;
}

/* 名稱 */
.name {
  font-size: 20px;
  font-weight: 500;
  color: #fff;
  cursor: pointer;
  text-decoration: none;
}

/* 線 */
.line {
  width: 100%;
  height: 2px;
  background: linear-gradient(to right, #313131, rgba(255, 255, 255), #313131);
  margin-top: 5px;
}
</style>
