<template>
  <div class="container-fluid mt-5">
    <div class="row">
      <div class="col-12 col-md-3 mb-4" v-for="(item, index) in items" :key="index">
        <div class="card">
          <div class="face face1">
            <div class="content">
              <img :src="item.imagePath" alt="Game Image" class="img-fluid" />
            </div>
          </div>
          <div class="face face2">
            <div class="content">
              <p>{{ item.description }}</p>
              <a href="#" type="button" @click="untrack(item.gameTrackingId)">取消追蹤</a>
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

// 特殊吐司
import { toast } from 'vue3-toastify'
import 'vue3-toastify/dist/index.css'

// 從環境變數取得 API BASE URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL
const token = ''

const items = ref([])

//const items = [
//  {
//    imagePath: 'https://cdn.cloudflare.steamstatic.com/steam/apps/553850/header.jpg?t=1709666906'
//  }
//]

//取得全部追蹤列表
// const TrackingList = async () => {
//   const token = store.getToken
//   console.log('JWT Token:', token) // 輸出 JWT Token
//   try {
//     const response = await axios.get(`${apiUrl}/api/GameTracking/GameTracking`, {
//       headers: {
//         Authorization: `Bearer ${store.token}`
//       }
//     })
//     items.value = response.data
//   } catch (error) {
//     console.error('Error fetching tracking list:', error)
//   }
// }

//取得會員追蹤列表
// const fetchTrackingList = async () => {
//   const token = store.getToken
//   console.log('JWT Token:', token) // 輸出 JWT Token

//   axios
//     .get(`${apiUrl}/api/GameTracking/GameTrack`, {
//       headers: {
//         Authorization: `Bearer ${token}`,
//         'Content-Type': 'application/json'
//       }
//     })
//     .then((response) => {
//       console.log('Tracking Data:', response.data) // 輸出追蹤數據
//       items.value = response.data
//     })
//     .catch((error) => {
//       console.error('Failed to fetch tracking data:', error)
//     })
// }

//取得會員追蹤列表
const fetchTrackingList = async () => {
  store.getToken
  console.log('JWT Token:', store.getToken) // 輸出 JWT Token

  try {
    const response = await axios.get(`${apiUrl}/api/GameTracking/GameTrackForId`, {
      headers: {
        Authorization: `Bearer ${store.getToken}`,
        'Content-Type': 'application/json'
      }
    })
    console.log('Tracking Data:', response.data) // 輸出追蹤數據
    items.value = response.data

    // 檢查是否有數據
    if (items.value.length === 0) {
      toast.success('目前沒有任何追蹤資料', {
        theme: 'colored',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.BOTTOM_CENTER
      })
    }
  } catch (error) {
    console.error('Failed to fetch tracking data:', error)

    // 檢查是否為 404 錯誤
    if (error.response && error.response.status === 404) {
      toast.error('未找到追蹤資料', {
        theme: 'colored',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.BOTTOM_CENTER
      })
    } else {
      toast.error('無法獲取追蹤資料，請稍後再試', {
        theme: 'colored',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.BOTTOM_CENTER
      })
    }
  }
}

//取消追蹤
const untrack = async (gameTrackingId) => {
  const token = store.getToken
  console.log('JWT Token:', token) // 輸出 JWT Token

  // 使用 confirm 方法讓使用者確認是否要取消追蹤
  const confirmation = confirm('您確定要取消追蹤這個遊戲嗎？')

  if (confirmation) {
    try {
      await axios.delete(`${apiUrl}/api/GameTracking/DeleteGameTracking/${gameTrackingId}`, {
        headers: {
          Authorization: `Bearer ${token}`
        }
      })
      // 更新追蹤列表
      await fetchTrackingList()
    } catch (error) {
      console.error('Error untracking game:', error)
    }
  } else {
    console.log('取消追蹤操作已取消')
  }
}

onMounted(() => {
  // TrackingList()
  fetchTrackingList()
})
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
  /* background: #333; */
  z-index: 1;
  transform: translateY(100px);
}

.card:hover .face1 {
  transform: translateY(0);
  box-shadow:;
  /* inset 0 0 60px whitesmoke, */
  /* inset 20px 0 80px #f0f, */
  /* inset -20px 0 80px #0ff, */
  /* inset 20px 0 300px #f0f, */
  /* inset -20px 0 300px #0ff; */
  /* 0 0 50px #fff, */
  /* -10px 0 80px #f0f, */
  /* 10px 0 80px #0ff; */
}

.face1 .content {
  /* opacity: 0.2; */
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
  opacity: 0;
}

.card:hover .face2 {
  transform: translateY(0);
  opacity: 1;
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
