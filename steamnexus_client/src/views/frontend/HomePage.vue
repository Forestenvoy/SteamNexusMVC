<template>
  <div class="container p-2">
    <ad-carousel class="mb-2 mt-3"></ad-carousel>
    <div class="row">
      <div class="col-xl-2"></div>
      <div class="col-xl-8 mt-3">
        <span class="fs-2 text-center d-block">依類別瀏覽</span>
        <!-- 推薦遊戲 -->
        <div data-aos="fade-left" data-aos-duration="500">
          <Carousel :items-to-show="5">
            <Slide v-for="slide in tags" :key="slide">
              <div
                class="carousel__item d-flex align-items-end p-3 fs-3"
                :style="`background-image: linear-gradient(to bottom, rgba(0, 0, 0,0)50%, rgba(0, 0, 0, 1) 80% ),url(${slide.img});background-size: cover;background-position: center;`"
                @mousedown="holdDown"
                @mouseup="holdUp(slide.tagId, slide.name)"
              >
                <span style="">{{ slide.name }}</span>
              </div>
            </Slide>

            <template #addons>
              <Navigation class="fw-bold" />
            </template>
          </Carousel>
        </div>

        <!-- carousel -->
        <div v-if="gameLozad.value != []">
          <div class="mb-2 videoFather">
            <span class="fs-2 text-center d-block">{{ hotTitle }}</span>
            <button
              v-if="btnopen == true"
              class="videoKid fs-6 translate-middle px-2 rounded btn"
              style="
                background-color: rgba(0, 0, 0, 0);
                border: 1px solid white;
                top: 50%;
                right: -50px;
              "
              @click="hotClick"
            >
              回到熱門排行
            </button>
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
      <div class="col-xl-2"></div>
    </div>
  </div>
</template>
<script setup>
import AdCarousel from '@/components/frontend/home/AdCarousel.vue'
import { ref, onMounted, defineComponent } from 'vue'
import lozad from 'lozad'

import { Carousel, Navigation, Slide } from 'vue3-carousel'
import 'vue3-carousel/dist/carousel.css'

defineComponent({
  components: {
    Carousel,
    Slide,

    Navigation
  }
})
var isPopupVisible = ref(0)
function showPopup(gameId) {
  isPopupVisible.value = gameId
  console.log(this)
}

function hidePopup() {
  isPopupVisible.value = null
}
var btnopen = ref(false)
var hotTitle = ref('熱門遊戲')
var gameData = ref([])
var gameLozad = ref([])
// let gameCount = -1
let TagGroupCount = -1
const loading = ref(null)
var TagGroupData = ref([])
var TagGroupDataLozad = ref([])
var isGameDataLoaded = ref(false)
var num = 0
var isholdClick = false
var holdUptagId = ''

var tags = ref([
  {
    img: 'https://img.freepik.com/free-photo/man-neon-suit-sits-chair-with-neon-sign-that-says-word-it_188544-27011.jpg?t=st=1716706009~exp=1716709609~hmac=345cfeddc0f13b19e1184b3e34943052b5ceb18416c681523bf7d8ad64ed6a89&w=826'
  },
  {
    img: 'https://img.freepik.com/free-photo/soccer-players-action-professional-stadium_654080-1746.jpg?t=st=1716706887~exp=1716710487~hmac=5107994c4f6d01a61ea3e48ee8731c9756153a9139e52b4e4acdcae3bd971b80&w=900'
  },
  {
    img: 'https://img.freepik.com/free-photo/people-swallowed-by-tornadoes-illustration_456031-24.jpg?t=st=1716706769~exp=1716710369~hmac=d4d2abef6f2b5bb57eec38608c5146a48e0abe3582b2ad0daffb1d73128205ee&w=900'
  },
  {
    img: 'https://img.freepik.com/free-photo/alien-forest-illustration_456031-20.jpg?t=st=1716707061~exp=1716710661~hmac=db4db3cbbccc0516a5eb9c33205b05d5ed487dc3d44444a889b975819f786dbc&w=900'
  },
  {
    img: 'https://img.freepik.com/free-photo/rise-humanoids-with-advanced-headgear-generative-ai_8829-2877.jpg?t=st=1716707280~exp=1716710880~hmac=1ed87adc440dcfa4a50531542502375a8dfbaa3393decff4a409c84316ea5f69&w=360'
  },
  {
    img: 'https://img.freepik.com/free-photo/beautiful-overhead-shot-shanghai-city-skyline-with-tall-skyscrapers-river-side_181624-22128.jpg?t=st=1716707163~exp=1716710763~hmac=3627273b6c71ef3f85b5dc7c8ca6a2eee40f48d934d0ffa67624d04c38622e31&w=360'
  },
  {
    img: 'https://img.freepik.com/free-photo/still-life-map-with-dices_23-2149352348.jpg?t=st=1716706587~exp=1716710187~hmac=22281abe609a307ad70c95f5629d553f135c771465974cd762aa1628d19c9acc&w=900'
  },
  {
    img: 'https://img.freepik.com/free-photo/high-angle-gamer-playing-indoors_23-2149829144.jpg?t=st=1716707010~exp=1716710610~hmac=bf2672f8a7c52e3bd2d377b9723898868399dd51f13b254b73671298e3ebc728&w=900'
  },
  {
    img: 'https://img.freepik.com/free-photo/moment-city-was-hit-by-nuclear-bomb-digital-painting_456031-158.jpg?t=st=1716707394~exp=1716710994~hmac=b65a072be1b0ce889556c003ad878bdb99d142fd6f11af203855a1bb27e4255c&w=900'
  },
  {
    img: 'https://img.freepik.com/free-photo/professional-gamers-cafe-room-with-powerful-personal-computer-3d-illustration_1419-2792.jpg?t=st=1716707421~exp=1716711021~hmac=5cc106be9257ea19ad92c4233708549992f70b6e991855f1a04f68f47ecaa6ab&w=900'
  },
  {
    img: 'https://img.freepik.com/free-photo/close-up-arcade-game-with-rubber-ducks_23-2148253115.jpg?t=st=1716706540~exp=1716710140~hmac=3f8d3d7e900d8b9680c3f0078bba823b3052fac290c83630ef8f8c382c66bef5&w=900'
  },
  {
    img: 'https://img.freepik.com/free-photo/doomsday-scene-catastrophe-digital-illustration_456031-51.jpg?t=st=1716706798~exp=1716710398~hmac=e618824332e93ac21dc2a839a5f9dcb174ea7a2912e0fa717049b29ee070ada6&w=900'
  },
  {
    img: 'https://img.freepik.com/free-photo/digital-painting-ancient-warships-traveling-rough-seas_456031-6.jpg?t=st=1716707487~exp=1716711087~hmac=437fb91b8261170d14cde10ad75e75f84d1277401a70e8fdeb737eaa3f1b464c&w=900'
  },
  {
    img: 'https://img.freepik.com/free-photo/world-collapse-doomsday-scene-digital-painting_456031-63.jpg?t=st=1716706207~exp=1716709807~hmac=e3d82e190ad86b59e18597a27bbab88de10ff180c3aa3849313b21842fdc468b&w=900'
  },
  {
    img: 'https://img.freepik.com/free-photo/roulette-wheel-glimmers-amidst-bustling-casino-floor_157027-4472.jpg?t=st=1716707532~exp=1716711132~hmac=54497e5997dd3befa1b94595d52bcb8de32883a7484248e9700ad16e921c33a7&w=900'
  },
  {
    img: 'https://img.freepik.com/free-photo/person-facing-purgatory-world-illustration_456031-40.jpg?t=st=1716706408~exp=1716710008~hmac=b35f172579578e4614ab9cde22bf951dacea18e06599c68e422b01118426538b&w=900'
  },
  {
    img: 'https://img.freepik.com/free-photo/multi-ethnic-people-cheering-while-woman-winning-game-with-vr-glasses-controller-tv-console-workmates-enjoying-video-games-with-joystick-have-fun-with-activity-after-work_482257-36711.jpg?t=st=1716706498~exp=1716710098~hmac=8b5d5df8e0377db99468a56b328469b42e22517feea80ee67f5d367f8e375491&w=900'
  },
  {
    img: 'https://img.freepik.com/free-photo/close-up-bingo-game-elements_23-2149181810.jpg?t=st=1716707614~exp=1716711214~hmac=f2782220222f14d04f7b720d0138401a18c5964ed4b1bf2f101ac5dbb6415c18&w=900'
  },
  {
    img: 'https://img.freepik.com/free-photo/empty-dark-room-modern-futuristic-sci-fi-background-3d-illustration_35913-2349.jpg?t=st=1716707561~exp=1716711161~hmac=44ee17917ea55f832810ec084a0f0c8d403bf0199120adf6e63ebdd3acb99078&w=900'
  },
  {
    img: 'https://img.freepik.com/free-photo/village-background-asset-game-2d-futuristic-generative-ai_191095-2024.jpg?t=st=1716706922~exp=1716710522~hmac=1cf942d935599c06a5462eb59eb488c1ca891a5776546f020093dbb963198776&w=900'
  }
])

const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

var timeStart = ref('')
var timeEnd = ref('')

function getTimeNow() {
  var now = new Date()
  return now.getTime()
}

function holdDown() {
  num = 0
  timeStart.value = getTimeNow()
}

function holdUp(tagId, name) {
  holdUptagId = tagId
  isholdClick = true
  timeEnd.value = getTimeNow()
  //如果此時檢測到的時間與第一次獲取的時間差有1000毫秒
  if (timeEnd.value - timeStart.value < 100) {
    gameLozad.value = []
    tagsameFatch(holdUptagId)
    btnopen.value = true
    hotTitle.value = `${name}相關遊戲`
    console.log(timeStart.value)
    console.log(timeEnd.value)
  }
}

function tagsameFatch(tagId) {
  gameData.value = []
  console.log(gameLozad.value)
  // console.log("gameData是0");
  console.log(`FatchNum:${num}`)
  fetch(`${apiUrl}/api/GamesManagement/GetGameSameData?tagId=${tagId}&tagNum=${num}`, {
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
      // console.log(gameData.value)
    })
    .then(() => {
      loadMoreGames()
      // console.log(gameData.value)
    })
    .catch((error) => {
      console.error('Error:', error)
    })
}

function hotClick() {
  num = 0
  isholdClick = false
  gameLozad.value = []
  btnopen.value = false
  hotTitle.value = `熱門排行`
  // gameCount = -1
  GetGameData()
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
      // gameLozad.value = []

      gameData.value = val
      isGameDataLoaded.value = true
      // console.log(gameData.value)
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
  for (let i = 0; i < tags.length; i++) {
    if(i>=5){
      break;
    }
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
  for (let i = 0; i < 20; i++) {
    gameLozad.value.push(gameData.value[i])
    // gameCount++
    num++
    // console.log(gameCount)
  }
}

const loadMoreTagGroup = () => {
  for (let i = 0; i < 20; i++) {
    TagGroupDataLozad.value.push(TagGroupData.value[TagGroupCount + 1])
    TagGroupCount++
    console.log(TagGroupData.value[TagGroupCount + 1]);
  }
}

const observer = lozad()

onMounted(() => {
  AllGameTagData()
  GetGameData()
  TagsData()
  observer.observe()
  const loadingElement = loading.value
  const io = new IntersectionObserver((entries) => {
    entries.forEach((entry) => {
      if (entry.isIntersecting) {
        if (isGameDataLoaded.value == true) {
          if (isholdClick == true) {
            console.log('isholdClick==true')
          } else {
            GetGameData()
            console.log('isholdClick==false')
          }

          // loadMoreGames()
          loadMoreTagGroup()
          observer.observe()
          // console.log('123')
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
