<!-- <template>
  <Carousel :autoplay="true" :autoplayTimeout="9000" :loop="true">
    <Slide v-for="(slide, index) in slides" :key="index">
      <div class="slide-content">
        <a :href="slide.url">
        <img :src="slide.imagePath" :alt="'Slide ' + (index + 1)">
        <div class="description">{{ slide.description }}</div>
        </a>
      </div>
    </Slide>
    <template #addons>
      <Navigation />
      <Pagination />
    </template>
  </Carousel>
</template>

<script>
import { ref, onMounted } from 'vue';
import { Carousel, Slide, Navigation, Pagination } from 'vue3-carousel';

import 'vue3-carousel/dist/carousel.css';

export default {
  components: {
    Carousel,
    Slide,
    Pagination,
    Navigation,
  },
  setup() {
    const slides = ref([]);

    const fetchSlides = async () => {
      try {
        const response = await fetch('https://localhost:7135/api/Advertisement/GetAdSlides');
        const data = await response.json();
        slides.value = data
      } catch (error) {
        console.error('Failed to fetch slides:', error);
      }
    };

    const carouselSettings = ref({
      autoplay: true,
      autoplaySpeed: 5000,
      loop: true,
    });

    onMounted(() => {
      fetchSlides();
    });

    return {
      slides
    };
  }
};
</script>

<style>
.slide-content {
  text-align: center;
  background-color: black;
  justify-content: center;
  position: relative;
  overflow: hidden;
  weight: 100vh; /* 使滑塊高度占滿整個視窗高度 */
}

.slide-content img {
  display: block;
  margin: 0 auto;
  max-width: 100%;
  height: auto;
}

.description {
  margin-top: 5px;
  font-size: 16px;
  position: absolute;
  bottom: 10px; /* 調整說明文字距離底部的距離 */
  width: 100%;
  text-align: center;
  color: white;
  background-color: rgba(0, 0, 0, 0.5);
}
</style> -->
<template>
  <swiper :slidesPerView="1" :spaceBetween="0" :loop="true" :autoplay="true" :pagination="pagination" :navigation="true"
    :modules="modules">
    <swiper-slide v-for="(slide, index) in slides" :key="index"><a :href="slide.url"><img :src="slide.imagePath"
          :alt="slide.title">
        <div class="description">
          <p>{{ slide.description }}</p>
        </div>
      </a></swiper-slide>
  </swiper>
</template>
<script setup>
// Import Swiper Vue.js components
import { Swiper, SwiperSlide } from 'swiper/vue';

import { ref, onMounted } from 'vue';

// Import Swiper styles
import 'swiper/css';

import 'swiper/css/pagination';
import 'swiper/css/navigation';


// import required modules
import { Pagination, Navigation, Autoplay } from 'swiper/modules';

const modules = ref([Pagination, Navigation, Autoplay]);
const pagination = ref({
  clickable: true,
});
const slides = ref([]);

async function fetchSlides() {
  fetch('https://localhost:7135/api/Advertisement/GetAdSlides', {
    method: 'GET',
  })
    .then(response => response.json())
    .then(data => {
      console.log(data);
      slides.value = data;
    })
    .catch(error => {
      console.error('Failed to fetch slides:', error);
    });
}

onMounted(() => {
  fetchSlides();
});
</script>
<style scoped>
.swiper {
  /* 輪播範圍設定 */
  width: 1000px;
  height: 500px;
  margin: 0 auto;
  border-radius: 15px;

  /* margin-left: auto; */
  /* margin-right: auto; */
}

.swiper-slide {
  text-align: center;
  font-size: 18px;
  background: #fff;
  color: #000;

  /* Center slide text vertically */
  display: flex;
  justify-content: center;
  align-items: center;
}

.swiper-slide img {
  display: block;
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.description {
  margin-top: 5px;
  font-size: 24px;
  position: absolute;
  bottom: 0px;
  /* 調整說明文字距離底部的距離 */
  width: 100%;
  height: 100px;
  text-align: center;
  color: white;
  background-color: rgba(0, 0, 0, 0.5);
}

p {
  margin: 20px;
  text-align: center;
  justify-content: center;

}
</style>
