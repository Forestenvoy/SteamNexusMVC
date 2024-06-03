<template>
  <div class="mt-5 container mb-2 text-white">
    <div class="position-absolute top-0 start-50 translate-middle-x" :style="ImageBackground"></div>
    <div style="margin-top: 200px">
      <div class="fs-6 row" data-aos="fade-in">
        <div class="col-xl-1"></div>
        <div class="col-xl-3 leftbox" style="height: 100%" data-aos="fade-right">
          <img :src="ImagePath" class="w-100" alt="" />

          <div class="p-3">
            <div v-if="tagShow" class="mt-3 tagShow">
              <span class="h7">熱門標籤：</span>
              <button v-for="tag in tagData" class="badge" :key="tag" @click="tagclick">
                {{ tag }}
              </button>
            </div>
            <div v-if="!tagShow" class="mt-3 tagShow" :key="tagShow">
              <span class="h7">熱門標籤： </span>
              <button v-for="tag in tagDataOpen" class="badge" :key="tag" @click="tagclick">
                {{ tag }}
              </button>
            </div>
            <button class="badge" @click="moreclick(tagShow)">...</button><br />
            <p class="mt-2">{{ Description }}</p>
          </div>
        </div>
        <div class="col-xl-7 p-1">
          <div class="d-flex">
            <span
              class="fs-1 shadow-sm ms-4 nowrap d-block"
              :key="Name"
              data-aos="fade-left"
              style="color: #f3ae0b; text-shadow: 2px 2px 4px #000000; width: 100%"
              >{{ Name }}
            </span>
            <!-- <button class="fontawesome fs-1 shadow-sm" @click="love">
              <i v-if="loveclick == true" class="fa-regular fa-heart"></i>
              <i v-else class="fa-solid fa-heart"></i>
            </button> -->
            <button
              v-show="loveclick == false"
              class="fontawesome fs-1 shadow-sm"
              @click="love(props.gameId)"
            >
              <!-- @click="love(gameId) 確保 gameId 是一個數字 -->
              <i v-show="loveclick == false" class="fa-regular fa-heart"></i>
            </button>
            <button
              v-show="loveclick == true"
              class="fontawesome fs-1 shadow-sm"
              @click="love(props.gameId)"
            >
              <!-- @click="love(gameId) 確保 gameId 是一個數字 -->
              <i style="color: red" class="fa-solid fa-heart"></i>
            </button>
          </div>

          <hr
            style="height: 2px; background-color: white; border: none; opacity: 1"
            class="mt-3 mb-4 ms-3"
            data-aos="fade-left"
          />
          <div class="d-flex justify-content-between" style="text-shadow: 2px 2px 4px #000000">
            <span class="ms-4 fs-1 fw-bold" data-aos="fade-left">歷史價格分析</span>
            <div class="d-flex" data-aos="fade-left">
              <span class="fs-5 my-auto">目前價格：</span>
              <span class="fs-2 mt-auto mb-auto" data-aos="fade-in">{{
                OriginalPrice == 0 ? '免費' : `NT.${OriginalPrice}`
              }}</span>
              <a :href="steamWeb" target="_blank" :title="steamWebtitle" class="mt-auto mb-auto"
                ><button
                  style="background-color: #f3ae0b; color: black; border: 0px; font-weight: 500"
                  type="button"
                  class="btn btn-primary ms-3"
                >
                  前往Steam購買
                </button></a
              >
            </div>
          </div>

          <!-- 圖表開始 -->
          <div data-aos="fade-left" data-aos-duration="500">
            <div class="hello ms-4 position-relative" ref="chartdiv">
              <span class="text-end d-block position-absolute bottom-0 end-0 pe-3">
                <span class="badge"
                  >目前在線人數
                  <span style="color: #f3ae0b; font-weight: 500">{{ playerData }}人</span></span
                >
                <span class="badge"
                  >三個月內最低價格
                  <span style="color: #f3ae0b; font-weight: 500"
                    >NT.{{ PricelowestData }}</span
                  ></span
                >
              </span>
            </div>
          </div>

          <!-- 圖表結束 -->
        </div>
        <div class="col-xl-1"></div>
      </div>
      <div class="fs-6 row" data-aos="fade-in">
        <div class="col-xl-1"></div>
        <div class="col-xl-3 leftbox" data-aos="fade-right">
          <div class="p-3">
            <span>名稱： {{ Name }}</span
            ><br />
            <span>原始價格： {{ OriginalPrice == 0 ? '免費' : `NT.${OriginalPrice}` }}</span
            ><br />
            <span>所有評論： {{ Comment }}（ {{ CommentNum }}人評論 ）</span><br />
            <span>開發商： {{ Publisher }}</span
            ><br />
            <span>發行日期： {{ ReleaseDate }}</span
            ><br />
            <div data-aos="fade-up">
              <img
                :src="
                  AgeRating == '18+'
                    ? 'https://www.gamerating.org.tw/Content/img/index_icon_05.jpg'
                    : AgeRating == '15+'
                      ? 'https://www.gamerating.org.tw/Content/img/index_icon_04.jpg'
                      : AgeRating == '12+'
                        ? 'https://www.gamerating.org.tw/Content/img/index_icon_03.jpg'
                        : AgeRating == '6+'
                          ? 'https://www.gamerating.org.tw/Content/img/index_icon_02.jpg'
                          : 'https://www.gamerating.org.tw/Content/img/index_icon_01.jpg'
                "
                alt=""
                class="w-25 mt-3 me-3"
              /><span>遊戲分級： {{ AgeRating }}</span
              ><br />
            </div>
            <div data-aos="fade-up">
              <table class="table table-hover mt-3">
                <thead>
                  <tr>
                    <th>語言支援</th>
                    <th class="text-center">介面</th>
                    <th class="text-center">完整語言</th>
                    <th class="text-center">字幕</th>
                  </tr>
                  <tr v-for="LT in LanguageTable" :key="LT.GameLanguageId">
                    <td>{{ LT.name }}</td>
                    <td class="text-center">{{ LT.support >= 1 ? '√' : '' }}</td>
                    <td class="text-center">
                      {{ LT.support == 2 ? '√' : LT.support == 1 ? '√' : '' }}
                    </td>
                    <td class="text-center">
                      {{ LT.support == 3 ? '√' : LT.support == 1 ? '√' : '' }}
                    </td>
                  </tr>
                </thead>
              </table>
            </div>
          </div>
        </div>
        <div class="col-xl-7 p-1">
          <span class="ms-3 fs-1 mt-4 fw-bold" data-aos="fade-left">遊玩門檻</span>
          <div class="d-flex ps-2">
            <div class="leftbox p-3 rounded m-1" style="width: 50%" data-aos="fade-left">
              <span class="fw-bold text-white fs-6">最低配備：</span><br />
              <div v-if="MinOriCpu != null" data-aos-duration="100">
                <span class="text-white">Cpu：</span>
                <span>{{ MinOriCpu }}</span>
              </div>
              <div v-if="MinOriGpu != null">
                <span class="text-white">Gpu：</span>
                <span>{{ MinOriGpu }}</span>
              </div>
              <div v-if="MinOriRam != null">
                <span class="text-white">記憶體：</span>
                <span>{{ MinOriRam }}</span>
              </div>
              <div v-if="MinStorage != null">
                <span class="text-white">儲存空間：</span>
                <span>{{ MinStorage }}</span>
              </div>
              <div v-if="MinOS != null">
                <span class="text-white">Windows系統：</span>
                <span>{{ MinOS }}</span>
              </div>
              <div v-if="MinNetwork != null">
                <span class="text-white">網路需求：</span>
                <span>{{ MinNetwork }}</span>
              </div>
              <div v-if="MinDirectX != null">
                <span class="text-white">DirectX：</span>
                <span>{{ MinDirectX }}</span>
              </div>
              <div v-if="MinAudio != null">
                <span class="text-white">音效：</span>
                <span>{{ MinAudio }}</span>
              </div>
              <div v-if="MinNote != null">
                <span class="text-white">備註：</span>
                <span>{{ MinNote }}</span>
              </div>
            </div>
            <div
              class="leftbox p-3 rounded m-1"
              style="width: 50%"
              data-aos="fade-left"
              data-aos-duration="500"
            >
              <span class="fw-bold text-white fs-6">最高配備：</span><br />
              <div v-if="RecOriCpu != null">
                <span class="text-white">Cpu：</span>
                <span>{{ RecOriCpu }}</span>
              </div>
              <div v-if="RecOriGpu != null">
                <span class="text-white">Gpu：</span>
                <span>{{ RecOriGpu }}</span>
              </div>
              <div v-if="RecOriRam != null">
                <span class="text-white">記憶體：</span>
                <span>{{ RecOriRam }}</span>
              </div>
              <div v-if="RecStorage != null">
                <span class="text-white">儲存空間：</span>
                <span>{{ RecStorage }}</span>
              </div>
              <div v-if="RecOS != null">
                <span class="text-white">Windows系統：</span>
                <span>{{ RecOS }}</span>
              </div>
              <div v-if="RecNetwork != null">
                <span class="text-white">網路需求：</span>
                <span>{{ RecNetwork }}</span>
              </div>
              <div v-if="RecDirectX != null">
                <span class="text-white">DirectX：</span>
                <span>{{ RecDirectX }}</span>
              </div>
              <div v-if="RecAudio != null">
                <span class="text-white">音效：</span>
                <span>{{ RecAudio }}</span>
              </div>
              <div v-if="RecNote != null">
                <span class="text-white">備註：</span>
                <span>{{ RecNote }}</span>
              </div>
            </div>
          </div>
          <span class="ms-4 fs-1 fw-bold" data-aos="fade-left">推薦遊戲</span>
          <!-- 推薦遊戲 -->
          <div data-aos="fade-left" data-aos-duration="500">
            <Carousel :items-to-show="4" class="ms-2">
              <Slide class="slide" v-for="slide in TagSamegamesName" :key="slide">
                <a
                  class="carousel__item"
                  style="text-decoration: none"
                  @mousedown="holdDown"
                  @mouseup="holdUp(slide.gameId)"
                >
                  <div>
                    <img width=" 100%" :src="slide.imagePath" alt="" />
                    <p height="45px" class="fs-7 nowrap">{{ slide.name }}</p>
                    <div v-if="slide.originalPrice != slide.currentPrice">
                      <p class="fs-6 text-decoration-line-through d-inline text-danger">
                        NT.{{ slide.originalPrice }}
                      </p>
                      <p class="fs-6 d-inline bg-danger ps-1 pe-1 ms-1">
                        NT.{{ slide.currentPrice }}
                      </p>
                    </div>
                    <div v-else>
                      <p class="fs-6">
                        {{ slide.currentPrice == 0 ? '免費' : 'NT.' + slide.currentPrice }}
                      </p>
                    </div>
                  </div></a
                >
              </Slide>
              <template #addons>
                <Navigation class="text-danger fw-bold" />
              </template>
            </Carousel>
          </div>

          <!-- carousel -->
        </div>
        <div class="col-xl-1"></div>
      </div>
    </div>
  </div>
</template>

<script setup>
// 使用 Pinia，利用 store 去訪問狀態，JWT使用者資訊
import { useIdentityStore } from '@/stores/identity.js'
const authStore = useIdentityStore()

import { ref, onMounted, onBeforeUnmount, reactive, watch, defineComponent } from 'vue'
import * as am5 from '@amcharts/amcharts5'
import * as am5xy from '@amcharts/amcharts5/xy'

import am5locales_zh_Hant from '@amcharts/amcharts5/locales/zh_Hant'
import Dark from '@amcharts/amcharts5/themes/Dark'
import AOS from 'aos'
import 'aos/dist/aos.css'
import axios from 'axios'

const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

var AppId = ref('')
var Name = ref('')
var OriginalPrice = ref('')
var CurrentPrice = ref('')
var AgeRating = ref('')
var ReleaseDate = ref('')
var Publisher = ref('')
var Description = ref('')
var ImagePath = ref('')
var VideoPath = ref('')
var Comment = ref('')
var CommentNum = ref('')
var steamWeb = ref('')
var steamWebtitle = ref('')

var MinCPUId = ref('')
var MinGPUId = ref('')
var MinRAM = ref('')
var MinOS = ref('')
var MinDirectX = ref('')
var MinNetwork = ref('')
var MinStorage = ref('')
var MinAudio = ref('')
var MinNote = ref('')
var MinOriCpu = ref('')
var MinOriGpu = ref('')
var MinOriRam = ref('')

var RecCPUId = ref('')
var RecGPUId = ref('')
var RecRAM = ref('')
var RecOS = ref('')
var RecDirectX = ref('')
var RecNetwork = ref('')
var RecStorage = ref('')
var RecAudio = ref('')
var RecNote = ref('')
var RecOriCpu = ref('')
var RecOriGpu = ref('')
var RecOriRam = ref('')

var tagData = ref([])
var tagDataOpen = ref([])
var tagShow = ref(true)
var LanguageTable = ref([])
var TagSamegamesName = ref([])
// var loveclick = ref('')
var loveclick = ref(false)

var PricelowestData = ref('')
var playerData = ref('')
// var slideUrl=ref("")

import { Carousel, Navigation, Slide } from 'vue3-carousel'
import 'vue3-carousel/dist/carousel.css'
defineComponent({
  components: {
    Carousel,
    Slide,
    Navigation
  },
  data: () => ({
    // carousel settings
    settings: {
      itemsToShow: 1,
      snapAlign: 'start'
    }
  })
})
var timeStart = ref('')
var timeEnd = ref('')

function getTimeNow() {
  var now = new Date()
  return now.getTime()
}

function holdDown() {
  timeStart.value = getTimeNow()
}

function holdUp(id) {
  timeEnd.value = getTimeNow()
  //如果此時檢測到的時間與第一次獲取的時間差有1000毫秒
  if (timeEnd.value - timeStart.value < 100) {
    location.href = `http://localhost:5173/game/${id}`
    console.log(timeStart.value)
    console.log(timeEnd.value)
  }
}

var ImageBackground = reactive({
  'background-size': 'cover',
  width: '100%',
  height: '500px',
  'mask-image': 'linear-gradient(to bottom, rgba(0, 0, 0, 0.7), rgba(0, 0, 0, 0))',
  'z-index': '-1',
  'margin-top': '45px'
})

watch(
  ImagePath,
  (newPath) => {
    ImageBackground['background-image'] = `url(${newPath})`
  },
  { immediate: true }
)

let props = defineProps({
  gameId: Number
})

const chartdiv = ref(null)
let root = null

function tagclick(event) {
  console.log(event.target.innerText)
}

function moreclick() {
  tagShow.value = !tagShow.value
}

//function love(){
//  loveclick.value=false
//}

//新增遊戲追蹤
const love = async (gameId) => {
  // 確保 gameId 是一個數字
  gameId = Number(gameId)
  loveclick.value = !loveclick.value
  console.log(loveclick.value)
  // if (isNaN(gameId)) {
  //   console.error('Game ID 必須為數字')
  //   return
  // }

  // 從 store 中取得 JWT Token
  // 假設 getToken 是一個屬性
  console.log('JWT Token:', authStore) // 輸出 JWT Token

  try {
    const response = await axios.post(
      `${apiUrl}/api/GameTracking/AddGameTracking`,
      { GameId: gameId }, // 這是請求的資料
      {
        headers: {
          Authorization: `Bearer ${authStore}`,
          'Content-Type': 'application/json'
        }
      }
    )
    console.log('新增遊戲追蹤成功:', response.data)
    alert('新增遊戲追蹤成功！') // 提示用戶成功新增
    // loveclick.value = !loveclick.value // 更新 loveclick 的值（若需要）
  } catch (error) {
    if (error.response) {
      // 伺服器返回了狀態碼但不在 2xx 範圍內
      if (error.response.status === 409) {
        console.log('該遊戲已經在追蹤列表中')
        alert('該遊戲已經在追蹤列表中') // 提示用戶已經存在於追蹤列表中
      } else {
        console.error('新增遊戲追蹤失敗:', error.response.data)
        alert('新增遊戲追蹤失敗：請先登入會員在新增') // 提示用戶失敗原因
      }
    } else if (error.request) {
      // 請求已發送但未收到回應
      console.error('未收到伺服器回應:', error.request)
      alert('新增遊戲追蹤失敗：' + error.response.data) // 提示用戶失敗原因
    } else {
      // 發生了一些其他的錯誤
      console.error('請求失敗:', error.message)
      alert('新增遊戲追蹤失敗：' + error.response.data) // 提示用戶失敗原因
    }
  }
}

//拿取遊戲資料
function getData() {
  fetch(`${apiUrl}/api/GamesManagement/GetEditJSON?id=${props.gameId}`, {
    method: 'GET',
    headers: {
      Authorization: `Bearer ${authStore.getToken}` // 確保 token 正確傳遞
    }
  })
    .then((response) => {
      // 確保請求是否成功
      if (!response.ok) {
        throw new Error('Network response was not ok')
      }
      // 解析 html
      return response.json()
    })
    .then((val) => {
      console.log(val)
      AppId.value = val.appId
      Name.value = val.name
      OriginalPrice.value = val.originalPrice
      AgeRating.value = val.ageRating
      ReleaseDate.value = val.releaseDate.substring(0, 10)
      Publisher.value = val.publisher
      Description.value = val.description
      ImagePath.value = val.imagePath
      VideoPath.value = val.videoPath
      Comment.value = val.comment
      CommentNum.value = val.commentNum
      CurrentPrice.value = val.currentPrice
      steamWeb.value = `https://store.steampowered.com/app/${val.appId}`
      steamWebtitle.value = `前往${val.name}的Steam網頁`
    })
    .catch((error) => {
      alert(error)
    })
}

function GetTagGroup() {
  fetch(`${apiUrl}/api/GamesManagement/GetTagGroup?id=${props.gameId}`, {
    method: 'GET'
  })
    .then((response) => {
      // 確保請求是否成功
      if (!response.ok) {
        throw new Error('Network response was not ok')
      }
      // 解析 html
      return response.json()
    })
    .then((val) => {
      tagDataOpen.value = []
      tagData.value = []
      console.log(val)
      for (var i = 0; i < 5; i++) {
        tagData.value.push(val[i])
      }
      for (var s = 0; s < val.length; s++) {
        tagDataOpen.value.push(val[s])
      }
    })
    .catch((error) => {
      alert(error)
    })
}

function GetLanguageGroup() {
  fetch(`${apiUrl}/api/GamesManagement/GetLanguageGroup?id=${props.gameId}`, {
    method: 'GET'
  })
    .then((response) => {
      // 確保請求是否成功
      if (!response.ok) {
        throw new Error('Network response was not ok')
      }
      // 解析 html
      return response.json()
    })
    .then((val) => {
      LanguageTable.value = []
      for (var i = 0; i < val.length; i++) {
        LanguageTable.value.push(val[i])
      }
    })
    .catch((error) => {
      alert(error)
    })
}

function getMinReqData() {
  fetch(`${apiUrl}/api/GamesManagement/GetMinReqData?id=${props.gameId}`, {
    method: 'GET'
  })
    .then((response) => {
      // 確保請求是否成功
      if (!response.ok) {
        throw new Error('Network response was not ok')
      }
      // 解析 html
      return response.json()
    })
    .then((val) => {
      MinCPUId.value = val.cpuId
      MinGPUId.value = val.gpuId
      MinRAM.value = val.ram
      MinOS.value = val.os
      MinDirectX.value = val.directX
      MinNetwork.value = val.network
      MinStorage.value = val.storage
      MinAudio.value = val.audio
      MinNote.value = val.note
      MinOriCpu.value = val.oriCpu
      MinOriGpu.value = val.oriGpu
      MinOriRam.value = val.oriRam
      console.log(val)
    })
    .catch((error) => {
      alert(error)
    })
}

function getRecReqData() {
  fetch(`${apiUrl}/api/GamesManagement/GetRecReqData?id=${props.gameId}`, {
    method: 'GET'
  })
    .then((response) => {
      // 確保請求是否成功
      if (!response.ok) {
        throw new Error('Network response was not ok')
      }
      // 解析 html
      return response.json()
    })
    .then((val) => {
      RecCPUId.value = val.cpuId
      RecGPUId.value = val.gpuId
      RecRAM.value = val.ram
      RecOS.value = val.os
      RecDirectX.value = val.directX
      RecNetwork.value = val.network
      RecStorage.value = val.storage
      RecAudio.value = val.audio
      RecNote.value = val.note
      RecOriCpu.value = val.oriCpu
      RecOriGpu.value = val.oriGpu
      RecOriRam.value = val.oriRam
      console.log(val)
    })
    .catch((error) => {
      alert(error)
    })
}
function GetGameTagSameData() {
  fetch(`${apiUrl}/api/GamesManagement/GetGameTagSameData?id=${props.gameId}`, {
    method: 'GET'
  })
    .then((response) => {
      // 確保請求是否成功
      if (!response.ok) {
        throw new Error('Network response was not ok')
      }
      // 解析 html
      return response.json()
    })
    .then((val) => {
      TagSamegamesName.value = []
      console.log(val)
      for (var i = 0; i < val.length; i++) {
        TagSamegamesName.value.push({
          gameId: val[i].gameId,
          name: val[i].name,
          imagePath: val[i].imagePath,
          originalPrice: val[i].originalPrice,
          currentPrice: val[i].currentPrice
        })
      }
      // TagSamegamesName=val.
      console.log(TagSamegamesName)
    })
    .catch((error) => {
      alert(error)
    })
}

function GetGamePricelowestData() {
  fetch(`${apiUrl}/api/GamesManagement/GetGamePricelowestData?id=${props.gameId}`, {
    method: 'GET'
  })
    .then((response) => {
      // 確保請求是否成功
      if (!response.ok) {
        throw new Error('Network response was not ok')
      }
      // 解析 html
      return response.json()
    })
    .then((val) => {
      console.log(val)
      PricelowestData.value = val.lowestPrice
    })
    .catch((error) => {
      alert(error)
    })
}
function GetGamePeopleData() {
  fetch(`${apiUrl}/api/GamesManagement/GetGamePeopleData?id=${props.gameId}`, {
    method: 'GET'
  })
    .then((response) => {
      // 確保請求是否成功
      if (!response.ok) {
        throw new Error('Network response was not ok')
      }
      // 解析 html
      return response.json()
    })
    .then((val) => {
      console.log(val)
      playerData.value = val.players
    })
    .catch((error) => {
      alert(error)
    })
}

onMounted(() => {
  AOS.init()
  getData()
  GetTagGroup()
  GetLanguageGroup()
  getMinReqData()
  getRecReqData()
  GetGameTagSameData()
  GetGamePricelowestData()
  GetGamePeopleData()

  fetch(`${apiUrl}/api/GamesManagement/GetLineChartData?id=${props.gameId}`, {
    method: 'GET'
  })
    .then((response) => {
      // 確保請求是否成功
      if (!response.ok) {
        throw new Error('Network response was not ok')
      }
      // 解析 html
      return response.json()
    })
    .then((data) => {
      // 将日期字符串转换为Date对象
      data.forEach(function (item) {
        item.date = new Date(item.date).getTime()
      })

      if (root) {
        root = null
      }

      root = am5.Root.new(chartdiv.value)
      root.setThemes([Dark.new(root)])
      root.locale = am5locales_zh_Hant

      let chart = root.container.children.push(
        am5xy.XYChart.new(root, {
          panX: true,
          panY: true,
          wheelX: 'panX',
          wheelY: 'zoomX',
          pinchZoomX: true,
          paddingLeft: 0,
          height: am5.percent(90),
          layout: am5.GridLayout.new(root, {})
        })
      )

      var cursor = chart.set(
        'cursor',
        am5xy.XYCursor.new(root, {
          behavior: 'none'
        })
      )
      cursor.lineY.set('visible', false)

      // Create Y-axis
      var yAxis = chart.yAxes.push(
        am5xy.ValueAxis.new(root, {
          renderer: am5xy.AxisRendererY.new(root, {
            pan: 'zoom'
          })
        })
      )

      // Create X-Axis
      var xAxis = chart.xAxes.push(
        am5xy.DateAxis.new(root, {
          // groupData: true,
          // groupIntervals: [{ timeUnit: "month", count: 0.1 }],
          maxDeviation: 0,
          baseInterval: {
            timeUnit: 'day',
            count: 1
          },
          renderer: am5xy.AxisRendererX.new(root, {
            minorGridEnabled: true,
            minGridDistance: 200,
            minorLabelsEnabled: true
          }),
          tooltip: am5.Tooltip.new(root, {})
        })
      )

      xAxis.get('renderer').labels.template.setAll({
        fill: '#FFF'
      })

      xAxis.setAll({
        background: am5.Rectangle.new(root, {
          fill: '#313131'
        }),
        tooltipDateFormat: 'yyyy年MM月dd日',
        periodChangeDateFormats: {
          day: 'dd', // 設置在日期改變時的格式
          month: 'yyyy年' // 設置在月份改變時的格式
        }
      })

      // 設置標籤樣式
      xAxis.get('renderer').labels.template.adapters.add('fontSize', function (fontSize, target) {
        if (target.dataItem && target.dataItem.get('value')) {
          var date = new Date(target.dataItem.get('value'))
          if (date.getMonth() === 0 && date.getDate() === 1) {
            // 如果是“年”的標籤
            return 14 // 設置“年”的標籤大小
          } else {
            return 10 // 設置其他標籤大小
          }
        }
        return fontSize
      })

      chart.zoomOutButton.get('background').setAll({
        fill: am5.color(0x000000),
        fillOpacity: 1,
        stroke: am5.color(0xf3ae0b)
      })
      root.interfaceColors.set('primaryButtonHover', am5.color(0xf3ae0b))
      root.interfaceColors.set('primaryButtonDown', am5.color(0x000000))

      var series1 = chart.series.push(
        am5xy.StepLineSeries.new(root, {
          name: 'Series',
          xAxis: xAxis,
          yAxis: yAxis,
          valueYField: 'price',
          valueXField: 'date'
        })
      )

      // 增大感應範圍
      series1.setAll({
        snapTooltip: true // 使工具提示對齊最近的點
      })

      series1.data.setAll(data)

      // chart.set("scrollbarX", am5.Scrollbar.new(root, {
      //   orientation: "horizontal",
      //   minHeight: 3,

      // }));

      //設定點點樣式
      series1.bullets.push(function () {
        return am5.Bullet.new(root, {
          stacked: 'up',
          sprite: am5.Circle.new(root, {
            radius: 4,
            fill: series1.get('fill')
          })
        })
      })

      series1.set('fill', am5.color(0xf3ae0b))
      series1.set('stroke', am5.color(0xf3ae0b))

      //設定線條粗度
      series1.strokes.template.set('strokeWidth', 2)

      //設定線上框框的圖式--start
      let tooltip = am5.Tooltip.new(root, {
        getFillFromSprite: false,
        labelText: "{date.formatDate('yyyy年MM月dd日')}\nNT.{price}"
      })

      tooltip.get('background').setAll({
        fill: am5.color(0xffffff),
        fillOpacity: 1
      })
      series1.set('tooltip', tooltip)
      //設定線上框框的圖式--end

      //設定下面圖式--start
      let legend = chart.children.push(
        am5.Legend.new(root, {
          y: am5.percent(110),
          x: am5.percent(10)
        })
      )
      legend.data.setAll(chart.series.values)
      //設定下面圖式--end
      // Add cursor
      chart.set('cursor', am5xy.XYCursor.new(root, {}))
      console.log(data)
    })
    .catch((error) => {
      alert(error)
    })
})

onBeforeUnmount(() => {
  if (root) {
    root.dispose()
  }
})
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
th,
td {
  border-bottom: 1px solid #ddd;
}

.fontawesome {
  background-color: rgba(0, 0, 0, 0);
  border: 0px solid black;
}
.nowrap {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  /* margin: 10px; */
  /* text-align: left; */
}

.carousel__item {
  height: 180px;
  padding: 10px;
  width: 100%;
  background-color: #0f1c27;
  color: var(--vc-clr-white);
  font-size: 20px;
  border-radius: 8px;
  /* display: flex;
  Flex-direction:column;
  align-items: center;
  justify-content:Space-between; */
}

.carousel__slide {
  width: 100%;
  padding: 2.5px;
}

.carousel__prev,
.carousel__next {
  box-sizing: content-box;
  border: 5px solid white;
}
.fs-7 {
  color: gray;
  font-size: 15px;
}

.rounded span {
  color: gray;
}
.leftbox {
  background: #313131; /* fallback for old browsers */
}
.hello {
  width: 100%;
  height: 300px;
}

.slidename {
  height: 45px;
}

.badge {
  font-size: 13px;
  font-weight: 300;
  border: 2px solid gray;
  margin: 2px;
  background-color: rgba(0, 0, 0, 0);
}

.tagShow {
  display: inline;
}

.h7 {
  font-weight: 500;
}
h3 {
  font-weight: bold;
}
p {
  font-weight: normal;
}

.row > * {
  padding-left: 0;
  padding-right: 0;
}
</style>
