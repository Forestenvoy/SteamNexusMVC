<template>
  <div class="container p-2">
    <ad-carousel></ad-carousel>
    <div class="row">
      <div class="col-xl-1 "></div>
      <div class="col-xl-10 mt-3 ">
        <!-- 推薦遊戲 -->
        <div  data-aos="fade-left"  data-aos-duration="500">
          <Carousel  :items-to-show="4">
          <Slide v-for="slide in 10" :key="slide">
      <div class="carousel__item">{{ slide }}</div>
    </Slide>

          <template #addons>
            <Navigation class="text-danger fw-bold" />
          </template>
        </Carousel>
        </div>
        
        <!-- carousel -->
          <div v-for="game in gameLozad" :key="game.gameId" class="mb-2" style="background-color: #0f1c27;">
            
              <a :href="`http://localhost:5173/game/${game.gameId}`" class="row m-0" style="color: white;text-decoration: none;">
              <div class="col-3 px-0">
                <img :src="game.imagePath " style="width: 100%;" alt="">
              </div>
              <div class="col-6 d-flex flex-column justify-content-between">
                <span class="d-block mt-1 nowrap fs-3" style="">{{ game.name}}</span>
                <div v-for="tagGroup in TagGroupDataLozad.filter(tg => tg.gameId === game.gameId)" :key="tagGroup.id" class="mb-3">
                  <a href="" v-for="tag in getTagNames(tagGroup.tags)"  :key="tag" style="font-size: 16px;color: white;text-decoration: none; border:1px solid white" class="m-1 px-1 rounded" >{{tag}}</a>
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

var gameData=ref([])
var gameLozad=ref([])
let gameCount = -1;
let TagGroupCount= -1
const loading = ref(null);
var TagGroupData=ref([]);
var TagGroupDataLozad=ref([]);
var isGameDataLoaded=ref(false);

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

  console.log(tag);
  return tag
}

function TagData(){
  fetch(`${apiUrl}/api/GamesManagement/TagData`, {
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
  TagData();
  GetGameData();
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
.carousel__item {
  min-height: 200px;
  width: 100%;
  background-color: aqua;
  color: var(--vc-clr-white);
  font-size: 20px;
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
