<template>
  <div class="container p-2">
    <ad-carousel></ad-carousel>
    <div class="row">
      <div class="col-xl-1 "></div>
      <div class="col-xl-10 mt-3 ">
        <div class="row">
          <!-- 推薦遊戲 -->
        <div  data-aos="fade-left"  data-aos-duration="500">
          <Carousel  :items-to-show="5">
          <Slide v-for="slide in tags" :key="slide">
      <div class="carousel__item" :style="`background-image: url(${slide.img});background-size: cover;background-position: center;`" >{{ slide.name }}</div>
    </Slide>

          <template #addons>
            <Navigation class="text-danger fw-bold" />
          </template>
        </Carousel>
        </div>
       
        <!-- carousel -->
        </div>
        <div class="row">
          <div class="col-8">
            <div v-for="game in gameLozad" :key="game.gameId" class="mb-2 videoFather" style="background-color: #0f1c27;" >
              <div :id="game.gameId" v-if="isPopupVisible==game.gameId&&game.videoPath!=''" style="background-color:#0f1c27;width: 50%;" class="videoKid p-2">
                  <video :src="game.videoPath" class="w-100" autoplay></video>
                </div>  
              <a :href="`http://localhost:5173/game/${game.gameId}`" class="row m-0 " style="color: white;text-decoration: none;" @mouseover="showPopup(game.gameId)" >
              <div class="col-3 px-0 ">
                <img :src="game.imagePath " style="width: 100%;" class="" alt="">
                
              </div>
              <div class="col-6 d-flex flex-column justify-content-between">
                <span class="d-block mt-1 nowrap fs-3" style="">{{ game.name}}</span>
                <div v-for="tagGroup in TagGroupDataLozad.filter(tg => tg.gameId === game.gameId)" :key="tagGroup.id" class="mb-3">
                  <a href="" v-for="tag in getTagNames(tagGroup.tags)"  :key="tag" style="font-size: 16px;color: white;text-decoration: none; border:1px solid white" class="m-1 px-2 rounded" >{{tag}}</a>
                  <!-- <button @click.capture.stop=""  v-for="tag in getTagNames(tagGroup.tags)" :key="tag" style="font-size: 12px;" class="m-1">{{tag}}</button> -->
                </div>
              </div>
              <div class="col-3 px-0 text-end d-flex align-items-center justify-content-end" >
                <div v-if="game.originalPrice!=game.currentPrice" class="d-flex">
                  <span class="px-2 h-100 my-auto me-4 fs-4" style="background-color: #F3AE0B ; color: #0f1c27">-{{bb(game.originalPrice,game.currentPrice)}}%</span>
                  <div>
                    <span  class="d-block text-decoration-line-through me-3" style="font-size: 15px">NT.{{game.originalPrice}}</span>
                    <span  class="fs-4 d-inline me-3" style="color: #F3AE0B">NT.{{game.currentPrice}}</span>
                  </div>
                  
                </div>
              <div v-else>
                 <span  class="fs-4 me-3">{{game.currentPrice==0?"免費":"NT."+ game.currentPrice}}</span>
              </div>
              
              </div>
              </a>
              
          </div>
          </div>
          <div class="col-4" style="background-color:#0f1c27;">
          </div>
        </div>
          
      </div>
      <div class="col-xl-1 "></div>
      <div ref="loading" class="loading">Loading...</div>
    </div>
  </div>

  <div style="height: 500px; width: 100px"></div>
</template>
<script setup>
import AdCarousel from '@/components/frontend/home/AdCarousel.vue'
import  { ref, onMounted, onBeforeUnmount,reactive,watch , defineComponent } from 'vue';
import lozad from 'lozad';

import { Carousel, Navigation, Slide } from 'vue3-carousel'
import 'vue3-carousel/dist/carousel.css'
defineComponent({
  name: 'Basic',
  components: {
    Carousel,
    Slide,
    
    Navigation,
  },
})
var isPopupVisible=ref(0)
function showPopup(gameId){
  isPopupVisible.value=gameId
  var triggerElement = event.currentTarget;
    var rect = triggerElement.getBoundingClientRect();
  var popup = document.getElementById(gameId);
  
  console.log(popup);
}

function hidePopup(){
  
  isPopupVisible.value=null
}


var gameData=ref([])
var gameLozad=ref([])
let gameCount = -1;
let TagGroupCount= -1
const loading = ref(null);
var TagGroupData=ref([]);
var TagGroupDataLozad=ref([]);
var isGameDataLoaded=ref(false);
var tags=ref([{"img":"https://img.freepik.com/free-photo/man-neon-suit-sits-chair-with-neon-sign-that-says-word-it_188544-27011.jpg?t=st=1716706009~exp=1716709609~hmac=345cfeddc0f13b19e1184b3e34943052b5ceb18416c681523bf7d8ad64ed6a89&w=826"},
  {"img":"https://img.freepik.com/free-photo/soccer-players-action-professional-stadium_654080-1746.jpg?t=st=1716706887~exp=1716710487~hmac=5107994c4f6d01a61ea3e48ee8731c9756153a9139e52b4e4acdcae3bd971b80&w=900"},
  {"img":"https://img.freepik.com/free-photo/people-swallowed-by-tornadoes-illustration_456031-24.jpg?t=st=1716706769~exp=1716710369~hmac=d4d2abef6f2b5bb57eec38608c5146a48e0abe3582b2ad0daffb1d73128205ee&w=900"},
  {"img":"https://img.freepik.com/free-photo/alien-forest-illustration_456031-20.jpg?t=st=1716707061~exp=1716710661~hmac=db4db3cbbccc0516a5eb9c33205b05d5ed487dc3d44444a889b975819f786dbc&w=900"},
  {"img":"https://img.freepik.com/free-photo/rise-humanoids-with-advanced-headgear-generative-ai_8829-2877.jpg?t=st=1716707280~exp=1716710880~hmac=1ed87adc440dcfa4a50531542502375a8dfbaa3393decff4a409c84316ea5f69&w=360"},
  {"img":"https://img.freepik.com/free-photo/beautiful-overhead-shot-shanghai-city-skyline-with-tall-skyscrapers-river-side_181624-22128.jpg?t=st=1716707163~exp=1716710763~hmac=3627273b6c71ef3f85b5dc7c8ca6a2eee40f48d934d0ffa67624d04c38622e31&w=360"},
  {"img":"https://img.freepik.com/free-photo/still-life-map-with-dices_23-2149352348.jpg?t=st=1716706587~exp=1716710187~hmac=22281abe609a307ad70c95f5629d553f135c771465974cd762aa1628d19c9acc&w=900"},
  {"img":"https://img.freepik.com/free-photo/high-angle-gamer-playing-indoors_23-2149829144.jpg?t=st=1716707010~exp=1716710610~hmac=bf2672f8a7c52e3bd2d377b9723898868399dd51f13b254b73671298e3ebc728&w=900"},
  {"img":"https://img.freepik.com/free-photo/moment-city-was-hit-by-nuclear-bomb-digital-painting_456031-158.jpg?t=st=1716707394~exp=1716710994~hmac=b65a072be1b0ce889556c003ad878bdb99d142fd6f11af203855a1bb27e4255c&w=900"},
  {"img":"https://img.freepik.com/free-photo/professional-gamers-cafe-room-with-powerful-personal-computer-3d-illustration_1419-2792.jpg?t=st=1716707421~exp=1716711021~hmac=5cc106be9257ea19ad92c4233708549992f70b6e991855f1a04f68f47ecaa6ab&w=900"},
  {"img":"https://img.freepik.com/free-photo/close-up-arcade-game-with-rubber-ducks_23-2148253115.jpg?t=st=1716706540~exp=1716710140~hmac=3f8d3d7e900d8b9680c3f0078bba823b3052fac290c83630ef8f8c382c66bef5&w=900"},
  {"img":"https://img.freepik.com/free-photo/doomsday-scene-catastrophe-digital-illustration_456031-51.jpg?t=st=1716706798~exp=1716710398~hmac=e618824332e93ac21dc2a839a5f9dcb174ea7a2912e0fa717049b29ee070ada6&w=900"},
  {"img":"https://img.freepik.com/free-photo/digital-painting-ancient-warships-traveling-rough-seas_456031-6.jpg?t=st=1716707487~exp=1716711087~hmac=437fb91b8261170d14cde10ad75e75f84d1277401a70e8fdeb737eaa3f1b464c&w=900"},
  {"img":"https://img.freepik.com/free-photo/world-collapse-doomsday-scene-digital-painting_456031-63.jpg?t=st=1716706207~exp=1716709807~hmac=e3d82e190ad86b59e18597a27bbab88de10ff180c3aa3849313b21842fdc468b&w=900"},
  {"img":"https://img.freepik.com/free-photo/roulette-wheel-glimmers-amidst-bustling-casino-floor_157027-4472.jpg?t=st=1716707532~exp=1716711132~hmac=54497e5997dd3befa1b94595d52bcb8de32883a7484248e9700ad16e921c33a7&w=900"},
  {"img":"https://img.freepik.com/free-photo/person-facing-purgatory-world-illustration_456031-40.jpg?t=st=1716706408~exp=1716710008~hmac=b35f172579578e4614ab9cde22bf951dacea18e06599c68e422b01118426538b&w=900"},
  {"img":"https://img.freepik.com/free-photo/multi-ethnic-people-cheering-while-woman-winning-game-with-vr-glasses-controller-tv-console-workmates-enjoying-video-games-with-joystick-have-fun-with-activity-after-work_482257-36711.jpg?t=st=1716706498~exp=1716710098~hmac=8b5d5df8e0377db99468a56b328469b42e22517feea80ee67f5d367f8e375491&w=900"},
  {"img":"https://img.freepik.com/free-photo/close-up-bingo-game-elements_23-2149181810.jpg?t=st=1716707614~exp=1716711214~hmac=f2782220222f14d04f7b720d0138401a18c5964ed4b1bf2f101ac5dbb6415c18&w=900"},
  {"img":"https://img.freepik.com/free-photo/empty-dark-room-modern-futuristic-sci-fi-background-3d-illustration_35913-2349.jpg?t=st=1716707561~exp=1716711161~hmac=44ee17917ea55f832810ec084a0f0c8d403bf0199120adf6e63ebdd3acb99078&w=900"},
  {"img":"https://img.freepik.com/free-photo/village-background-asset-game-2d-futuristic-generative-ai_191095-2024.jpg?t=st=1716706922~exp=1716710522~hmac=1cf942d935599c06a5462eb59eb488c1ca891a5776546f020093dbb963198776&w=900"}
]);

const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

function bb(a,b){
  return Math.round((1-(b/a))*100)
}

function GetGameData(){
  fetch(`${apiUrl}/api/GamesManagement/IndexJson`, {
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
      gameData=val
      isGameDataLoaded.value = true;
    }).then(()=>{
      loadMoreGames();
    })
    .catch((error) => {
      alert(error)
    })
    .finally(() => {
      // 异步操作完成后启用按钮
      $(this).prop('disabled', false)
    })
}

function getTagNames(tags){
  var tag=[]
  for (let i = 0; i < 6; i++) {
    tag.push(tags[i].name)
  }
  // tags.forEach(element => {
  //   tag.push(element.name)
  // });

  return tag
}

function AllGameTagData(){
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
      TagGroupData=val;
      console.log('1-1');
    }).then(()=>{
      loadMoreTagGroup();
      console.log('1-2');
    })
    .catch((error) => {
      alert(error)
    })
    .finally(() => {
      // 异步操作完成后启用按钮
      $(this).prop('disabled', false)
    })
}

function TagsData(){
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
      val.forEach((element,i)=> {
        console.log(tags.value[i].img);
        tags.value[i].name=element.name;
      });
      // tags.value=val
      console.log(tags.value);
    })
    .catch((error) => {
      alert(error)
    })
    .finally(() => {
      // 异步操作完成后启用按钮
      $(this).prop('disabled', false)
    })
}


const loadMoreGames = () => {
  for (let i = 0; i < 20; i++) {
    gameLozad.value.push(gameData[gameCount + 1])
    gameCount++;
  }
};

const loadMoreTagGroup = () => {
  for (let i = 0; i < 20; i++) {
    TagGroupDataLozad.value.push(TagGroupData[TagGroupCount + 1])
    TagGroupCount++;
  }
};

const observer = lozad();

onMounted(() => {
  AllGameTagData();
  GetGameData();
  TagsData();
  observer.observe();
  const loadingElement = loading.value; 
  const io = new IntersectionObserver((entries) => {
    entries.forEach((entry) => {
      if (entry.isIntersecting) {
        if(isGameDataLoaded.value==true){
          loadMoreGames();
          loadMoreTagGroup();
          observer.observe();
        }
        
      }
    });
  });

io.observe(loadingElement);
});
</script>
<style>
.videoFather{
  position: relative;
}
.videoFather{
  position: relative;
}

.videoKid{
  position: absolute;
  right: -370px;
}

.carousel__item {
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

.carousel__slide {
  padding: 10px;
}

.carousel__prev,
.carousel__next {
  box-sizing: content-box;
  border: 5px solid white;
} 
a:hover{
  background-color: rgba(255, 255, 255,0.1)
}
.nowrap {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  /* margin: 10px; */
  /* text-align: left; */
}
</style>
