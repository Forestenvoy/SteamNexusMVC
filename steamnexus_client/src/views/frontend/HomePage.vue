<template>
  <div class="container p-2">
    <ad-carousel></ad-carousel>
    <div class="row">
      <div class="col-xl-1 "></div>
      <div class="col-xl-10 mt-4 d-flex flex-wrap justify-content-between">
        
          <div v-for="game in gameLozad" :key="game.gameId" class="mb-2" style="width: 49.5%;background-color: #0f1c27;">
            <a :href="`http://localhost:5173/game/${game.gameId}`" class="row m-0" style="color: white;text-decoration: none;">
              <div class="col-3 px-0">
                <img :src="game.imagePath " style="width: 100%;" alt="">
              </div>
              <div class="col-6 ">
                <span class="d-block mt-1 nowrap">{{ game.name}}</span>
              </div>
              <div class="col-3 px-0 text-end d-flex align-items-center justify-content-end" >
                <div v-if="game.originalPrice!=game.currentPrice" class="">
                  <span  class="d-block fs-6 text-decoration-line-through text-danger me-2">NT.{{game.originalPrice}}</span>
                  <span  class="fs-6 d-inline bg-danger ps-1 pe-1 ms-1 me-2">NT.{{game.currentPrice}}</span>
                </div>
              <div v-else>
                 <span  class="fs-6 me-2">{{game.currentPrice==0?"免費":"NT."+ game.currentPrice}}</span>
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

var gameData=ref([])
var gameLozad=ref([])
let gameCount = -1;
const loading = ref(null);

const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

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
      loadMoreGames()
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

const observer = lozad();

onMounted(() => {
  GetGameData();
  observer.observe();
  const loadingElement = loading.value; 
  const io = new IntersectionObserver((entries) => {
    entries.forEach((entry) => {
      if (entry.isIntersecting) {
        loadMoreGames();
        observer.observe();
      }
    });
  });

  io.observe(loadingElement);
});
</script>
<style>
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
