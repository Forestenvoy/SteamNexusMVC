<template>
  <div class="menu">
    <div class="lines"></div>
    <div class="imageBox">
      <img src="@/assets/images/builder/rocket.gif" alt="" />
    </div>
    <div class="content">
      <div class="details">
        <h2>$ {{ props.menu.totalPrice }}</h2>
        <h3>{{ props.menu.name }}</h3>
        <a href="#" @click.prevent="showProductList">觀看更多</a>
      </div>
    </div>
  </div>
</template>

<script setup>
// pinia
import { useBuilderStore } from '@/stores/builder.js'
const builderStore = useBuilderStore()

// API URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

const props = defineProps({
  menu: Object
})

// 展開產品清單
const showProductList = () => {
  getProductList()
}

// 獲取菜單產品資料
const getProductList = async () => {
  await fetch(`${apiUrl}/api/PcBuilder/GetProductList?menuId=${props.menu.id}`, {
    method: 'GET'
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
      builderStore.setProductList(data)
      calculateRatio()
    })
    .catch((error) => {
      console.error('Error:', error.message)
    })
}

// 發送產品ID 計算遊戲匹配比例
const calculateRatio = async () => {
  // post
  await fetch(`${apiUrl}/api/PcBuilder/CalculateRatio`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      pCpuId: builderStore.getCPUId,
      pGpuId: builderStore.getGPUId,
      pRamId: builderStore.getRAMId
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
      console.log(data)
      builderStore.setMinRatio(data.min)
      builderStore.setRecRatio(data.rec)
      builderStore.setMinCount(data.minCount)
      builderStore.setRecCount(data.recCount)
      builderStore.showMatch()
    })
    .catch((error) => {
      console.error('Error:', error.message)
    })
}
</script>

<style scoped>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

/* 卡片本體 */
.menu {
  position: relative;
  width: 350px;
  height: 180px;
  background-color: #313131;
  transition: 0.5s;
}

.menu {
  height: 450px;
}

/* 線條 */
.lines {
  position: absolute;
  inset: 0;
  background-color: #000;
  overflow: hidden;
}

.lines::before {
  content: '';
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  width: 600px;
  height: 120px;
  background: linear-gradient(transparent, antiquewhite, antiquewhite, antiquewhite, transparent);
  animation: animate 4s linear infinite;
  animation-play-state: paused;
}

.menu .lines::before {
  animation-play-state: running;
}

@keyframes animate {
  0% {
    transform: translate(-50%, -50%) rotate(0deg);
  }

  100% {
    transform: translate(-50%, -50%) rotate(360deg);
  }
}

.lines::after {
  content: '';
  position: absolute;
  inset: 3px;
  background-color: #292929;
}

/* 圖片 */

.imageBox {
  position: absolute;
  top: -60px;
  left: 50%;
  width: 150px;
  height: 150px;
  transform: translateX(-50%);
  background: #000;
  transition: 0.5s;
  z-index: 10;
  overflow: hidden;
  display: flex;
  justify-content: center;
  align-items: center;
}

.menu .imageBox {
  top: 25px;
  width: 200px;
  height: 200px;
}

.imageBox::before {
  content: '';
  position: absolute;
  top: 50%;
  left: 50%;
  width: 500px;
  height: 150px;
  transform: translate(-50%, -50%);
  background: linear-gradient(transparent, #ff0f0f, #ff0f0f, #ff0f0f, transparent);
  animation: animate2 6s linear infinite;
  animation-play-state: paused;
}

.menu .imageBox::before {
  animation-play-state: running;
}

@keyframes animate2 {
  0% {
    transform: translate(-50%, -50%) rotate(360deg);
  }

  100% {
    transform: translate(-50%, -50%) rotate(0deg);
  }
}

.imageBox::after {
  content: '';
  position: absolute;
  inset: 3px;
  background-color: #292929;
}

img {
  position: absolute;
  width: 100px;
  z-index: 1;
  filter: invert(1);
  opacity: 0.5;
  transition: 0.5s;
}

.menu img {
  opacity: 1;
}

/* 文字內容 */
.content {
  position: absolute;
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: flex-end;
  overflow: hidden;
}

.details {
  padding: 30px 20px;
  text-align: center;
  width: 100%;
  transition: 0.5s;
  transform: translateY(145px);
}

.menu .details {
  transform: translateY(0);
}

h2 {
  font-size: 1.5em;
  font-weight: 500;
  color: antiquewhite;
  line-height: 1.2em;
  margin-bottom: 25px;
}

h3 {
  color: #fff;
  opacity: 0;
  transition: 0.5s;
  margin-bottom: 35px;
}

a {
  display: inline-block;
  padding: 8px 15px;
  background-color: antiquewhite;
  color: #292929;
  font-weight: 500;
  text-decoration: none;
  opacity: 0;
  transition: 0.5s;
  margin-bottom: 10px;
}

.menu h3,
.menu a {
  opacity: 1;
}
</style>
