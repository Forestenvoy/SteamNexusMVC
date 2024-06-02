<template>
  <div ref="container" class="container p-2 mt-5">
    <div class="row">
      <div class="col-xl-2"></div>
      <div class="col-xl-8">
        <div class="input-group mb-3">
          <span class="input-group-text"><i class="fa-solid fa-magnifying-glass"></i></span>
          <!-- <span class="input-group-text">0.00</span> -->
          <input
            type="text"
            class="form-control"
            aria-label="Dollar amount (with dot and two decimal places)"
            placeholder="請輸入遊戲關鍵字"
            v-model="inputText"
            @change="inputChang"
          />
        </div>

        <div
          v-for="game in gameLozad"
          :key="game.gameId"
          class="mb-2 videoFather rounded"
          style="background-color: #0f1c27"
          @mouseover="showPopup(game.gameId)"
          @mouseleave="hidePopup"
        >
          <div
            v-if="isPopupVisible == game.gameId"
            style="background-color: #2a3741; z-index: 2; width: 30%"
            class="videoKid p-3 rounded"
            @mouseover="showPopup(game.gameId)"
          >
            <video
              v-if="game.videoPath != ''"
              :src="game.videoPath"
              autoplay
              class=""
              style="width: 100%"
            ></video>
            <span
              class="fs-4 d-flex justify-content-center align-items-center"
              v-else
              style="background-color: black; height: 150px; text-align: center"
              ><span>未提供遊戲影片</span></span
            >
            <span class="fs-4 nowrap d-block">{{ game.name }}</span>
            <div>
              <img
                :src="
                  game.ageRating == '18+'
                    ? 'https://www.gamerating.org.tw/Content/img/index_icon_05.jpg'
                    : game.ageRating == '15+'
                      ? 'https://www.gamerating.org.tw/Content/img/index_icon_04.jpg'
                      : game.ageRating == '12+'
                        ? 'https://www.gamerating.org.tw/Content/img/index_icon_03.jpg'
                        : game.ageRating == '6+'
                          ? 'https://www.gamerating.org.tw/Content/img/index_icon_02.jpg'
                          : 'https://www.gamerating.org.tw/Content/img/index_icon_01.jpg'
                "
                alt=""
                class="m-1"
                style="width: 20%"
              /><span class="fs-6">遊戲分級： {{ game.ageRating }}</span>
            </div>
            <span class="fs-6">{{ game.description }}</span>
          </div>
          <a
            href="#"
            @click.prevent="$router.push(`/game/${game.gameId}`)"
            target="_blank"
            class="row m-0 rounded"
            :style="{ backgroundColor: isPopupVisible === game.gameId ? '#2A3741' : '' }"
            style="color: white; text-decoration: none"
          >
            <div class="col-3 px-0 videoFather">
              <img :src="game.imagePath" style="width: 100%" class="rounded" alt="" />
            </div>
            <div class="col-6 d-flex flex-column justify-content-between">
              <span class="d-block mt-1 nowrap fs-4" style="">{{ game.name }}</span>
              <div
                v-for="tagGroup in TagGroupDataLozad.filter((tg) => tg.gameId === game.gameId)"
                :key="tagGroup.id"
                class="mb-3"
              >
                <a
                  href=""
                  v-for="tag in getTagNames(tagGroup.tags)"
                  :key="tag"
                  style="font-size: 12px; text-decoration: none; border: 1px solid white"
                  class="m-1 px-2 rounded tagClass"
                  >{{ tag }}</a
                >
                <!-- <button @click.capture.stop=""  v-for="tag in getTagNames(tagGroup.tags)" :key="tag" style="font-size: 12px;" class="m-1">{{tag}}</button> -->
              </div>
            </div>
            <div class="col-3 px-0 text-end d-flex align-items-center justify-content-end">
              <div v-if="game.originalPrice != game.currentPrice" class="d-flex">
                <span
                  class="px-2 h-100 my-auto me-4 fs-4"
                  style="background-color: #f3ae0b; color: #0f1c27"
                  >-{{ bb(game.originalPrice, game.currentPrice) }}%</span
                >
                <div>
                  <span class="d-block text-decoration-line-through me-3" style="font-size: 15px"
                    >NT.{{ game.originalPrice }}</span
                  >
                  <span class="fs-4 d-inline me-3" style="color: #f3ae0b"
                    >NT.{{ game.currentPrice }}</span
                  >
                </div>
              </div>
              <div v-else>
                <span class="fs-4 me-3">{{
                  game.currentPrice == 0 ? '免費' : 'NT.' + game.currentPrice
                }}</span>
              </div>
            </div>
          </a>
        </div>
      </div>
      <div ref="loading" class="loading"></div>
    </div>
    <div class="col-xl-2 fs-4 mx-auto" style="text-align: center">{{ loadingfont }}</div>
  </div>
</template>
<script setup>
import { ref, onMounted } from 'vue'
import lozad from 'lozad'

import 'vue3-carousel/dist/carousel.css'

import { useSearchStore } from '@/stores/search.js'
const searchStore = useSearchStore()

var isPopupVisible = ref(0)
function showPopup(gameId) {
  isPopupVisible.value = gameId
  console.log(this)
}

function hidePopup() {
  isPopupVisible.value = null
}

var gameData = ref([])
var gameLozad = ref([])
// let gameCount = -1
let TagGroupCount = -1
const loading = ref(null)
const container = ref(null)
var TagGroupData = ref([])
var TagGroupDataLozad = ref([])
var isGameDataLoaded = ref(false)
var num = 0
var isholdClick = false
var inputText = ref(searchStore.getKeyword)
var loadingfont = ref('')
// var loadingif = false

var tags = ref([])

const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

function inputChang() {
  num = 0
  gameData.value = []
  if (inputText.value == '') {
    GetGameData()
    gameLozad.value = []
    isholdClick = false
  } else {
    inputChangSeach()
    isholdClick = true
    gameLozad.value = []
  }
}

function inputChangSeach() {
  gameData.value = []
  fetch(`${apiUrl}/api/GamesManagement/inputChangSeach?input=${inputText.value}&num=${num}`, {
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
      if (val == '') {
        loadingfont.value = '查無此遊戲'
      } else {
        loadingfont.value = ''
      }
      gameData.value = val
      console.log(val)
    })
    .then(() => {
      loadMoreGames()
    })
    .catch((error) => {
      console.error('Error:', error)
    })
}

function bb(a, b) {
  return Math.round((1 - b / a) * 100)
}

function GetGameData() {
  gameData.value = []
  fetch(`${apiUrl}/api/GamesManagement/GetGameData?num=${num}`, {
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
      gameData.value = val
      isGameDataLoaded.value = true
    })
    .then(() => {
      // console.log('1-2')
      loadMoreGames()
    })
    .catch((error) => {
      console.error('Error:', error)
    })
}

function getTagNames(tags) {
  var tag = []
  for (let i = 0; i < 5; i++) {
    tag.push(tags[i].name)
  }
  return tag
}

function AllGameTagData() {
  fetch(`${apiUrl}/api/GamesManagement/AllGameTagData`, {
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
      TagGroupData.value = val
    })
    .then(() => {
      loadMoreTagGroup()
    })
    .catch((error) => {
      console.error('Error:', error)
    })
}

function TagsData() {
  fetch(`${apiUrl}/api/GamesManagement/TagsData`, {
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
      val.forEach((element, i) => {
        tags.value[i].name = element.name
        tags.value[i].tagId = element.tagId
      })
      // console.log(val)
    })
    .catch((error) => {
      console.error('Error:', error)
    })
}

const loadMoreGames = () => {
  console.log('把API遊戲傳進前端')
  for (let i = 0; i < gameData.value.length; i++) {
    gameLozad.value.push(gameData.value[i])
    // gameCount++
    num++
  }

  console.log(gameData.value)
  console.log(gameLozad.value)
  console.log(num)
}

const loadMoreTagGroup = () => {
  for (let i = 0; i < 20; i++) {
    TagGroupDataLozad.value.push(TagGroupData.value[TagGroupCount + 1])
    TagGroupCount++
  }
}

const observer = lozad()

onMounted(() => {
  console.log('Hi search')
  AllGameTagData()
  inputChang()
  TagsData()
  observer.observe()
  const loadingElement = loading.value
  const io = new IntersectionObserver((entries) => {
    entries.forEach((entry) => {
      if (entry.isIntersecting) {
        if (isGameDataLoaded.value == true) {
          if (num >= 20) {
            if (isholdClick == true) {
              inputChangSeach()
              console.log('isholdClick==true')
            } else {
              GetGameData()
              console.log('isholdClick==false')
            }
          }
          loadMoreTagGroup()
        }
      }
    })
  })
  io.observe(loadingElement)
})
</script>
<style scoped>
.btn:hover {
  background-color: rgba(255, 255, 255, 0.6) !important;
  color: black;
}
.videoFather {
  position: relative;
}
.videoKid {
  position: absolute;
  right: 0;
  /* left: 100%; */
  /* top: 100%; */
}
.carousel__item {
  cursor: pointer;
  min-height: 150px;
  width: 100%;
  background-color: black;
  color: var(--vc-clr-white);
  font-size: 20px;
  font-weight: 500;
  border-radius: 8px;
  display: flex;
  justify-content: center;
  align-items: center;
}

.carousel__item:hover {
  background-color: rgba(255, 255, 255, 0.6);
}

.carousel__slide {
  padding: 10px;
}

.carousel__prev,
.carousel__next {
  box-sizing: content-box;
  color: #f3ae0b;
  font-weight: bolder;
  stroke: #f3ae0b;
  stroke-width: 5;
  z-index: 2px;
  opacity: 1;
}
a:hover {
  background-color: #2a3741;
}

.tagClass {
  color: wheat;
}

.tagClass:hover {
  background-color: rgba(255, 255, 255, 0.6);
  color: #2a3741;
  font-weight: bold;
}
.nowrap {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  /* margin: 10px; */
  /* text-align: left; */
}
</style>
