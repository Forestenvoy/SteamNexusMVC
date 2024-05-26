<template>
  <div class="container-fluid mt-5">
    <div class="row">
      <div class="col-12 col-md-3 mb-4" v-for="(item, index) in items" :key="index">
        <div class="card">
          <div class="face face1">
            <div class="content">
              <i :class="item.icon"></i>
              <h3>{{ item.title }}</h3>
            </div>
          </div>
          <div class="face face2">
            <div class="content">
              <p>Lorem ipsum dolor sit amet consectetur adipisicing elit.</p>
              <a href="#" type="button">取消追蹤</a>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
// 使用 Pinia，利用 store 去訪問狀態
import { useIdentityStore } from '@/stores/identity.js'
const store = useIdentityStore()

import { ref, onMounted } from 'vue'
import axios from 'axios'

// 從環境變數取得 API BASE URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

const items = [
  { title: 'Windows', icon: 'fab fa-windows' },
  { title: 'Apple', icon: 'fab fa-apple' },
  { title: 'Android', icon: 'fab fa-android' },
  { title: 'Windows', icon: 'fab fa-windows' },
  { title: 'Apple', icon: 'fab fa-apple' },
  { title: 'Android', icon: 'fab fa-android' },
  { title: 'Windows', icon: 'fab fa-windows' },
  { title: 'Apple', icon: 'fab fa-apple' },
  { title: 'Android', icon: 'fab fa-android' }
]

onMounted(() => {})
</script>

<style scoped>
.container-fluid {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
  width: 100%;
}

.row {
  width: 100%;
}

.card {
  perspective: 1000px;
  background-color: transparent;
  border: none;
  width: 100%;
}

.face {
  width: 100%;
  height: 200px;
  transition: 0.4s;
  position: relative;
  display: flex;
  justify-content: center;
  align-items: center;
}

.face1 {
  background: #333;
  z-index: 1;
  transform: translateY(100px);
}

.card:hover .face1 {
  transform: translateY(0);
  box-shadow:
    inset 0 0 60px whitesmoke,
    inset 20px 0 80px #f0f,
    inset -20px 0 80px #0ff,
    inset 20px 0 300px #f0f,
    inset -20px 0 300px #0ff,
    /* 0 0 50px #fff, */
    /* -10px 0 80px #f0f, */
    /* 10px 0 80px #0ff; */
}

.face1 .content {
  opacity: 0.2;
  transition: 0.5s;
  text-align: center;
}

.card:hover .face1 .content {
  opacity: 1;
}

.face1 .content i {
  font-size: 3em;
  color: white;
}

.face1 .content h3 {
  font-size: 1em;
  color: white;
}

.face2 {
  background: whitesmoke;
  box-shadow: 0 20px 50px rgba(0, 0, 0, 0.8);
  transform: translateY(-100px);
  padding: 20px;
  box-sizing: border-box;
}

.card:hover .face2 {
  transform: translateY(0);
}

.face2 .content p {
  font-size: 10pt;
  color: #333;
}

.face2 .content a {
  text-decoration: none;
  color: black;
  outline: 1px dashed #333;
  padding: 10px;
  display: inline-block;
  transition: 0.5s;
}

.face2 .content a:hover {
  background: #333;
  color: whitesmoke;
  box-shadow: inset 0 0 10px rgba(0, 0, 0, 0.5);
}
</style>
