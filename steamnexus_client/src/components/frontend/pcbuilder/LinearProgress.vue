<template>
  <div class="d-flex justify-content-center">
    <section
      class="d-flex justify-content-center position-relative"
      style="height: 20px; width: 80%"
    >
      <svg width="100%" height="20">
        <!-- 繪製中間的白色虛線 -->
        <!-- stroke-width 屬性設定了線條的粗細 -->
        <!-- stroke-dasharray 屬性定義了線段和間隙的長度模式，第一個值表示線段長度，第二個值表示間隙長度 -->
        <line
          x1="10"
          y1="10"
          x2="100%"
          y2="10"
          stroke="#fff"
          stroke-width="4"
          stroke-dasharray="10 5"
        />

        <!-- 繪製實心的進度條部分 -->
        <line
          x1="10"
          y1="10"
          :x2="animationProgress"
          y2="10"
          stroke="#f3ae0b"
          stroke-width="4"
          stroke-dasharray="10 5"
        />
      </svg>
      <img
        src="@/assets/images/builder/airplane.png"
        alt="AirPlane"
        :style="{ left: animationProgress }"
      />
    </section>
  </div>
</template>

<script setup>
// vue
import { ref, watch } from 'vue'

// pinia
import { useBuilderStore } from '@/stores/builder.js'
const builderStore = useBuilderStore()

const animationProgress = ref('0%')

var timer = null

watch(
  () => builderStore.getCurrentStep,
  (newVal, oldVal) => {
    if (timer) clearInterval(timer)
    let start = oldVal * 11
    let end = newVal * 11
    if (newVal >= 0) {
      if (newVal > oldVal) {
        timer = setInterval(() => {
          animationProgress.value = `${start}%`
          start++
          if (start == end) clearInterval(timer)
        }, 50)
      } else {
        timer = setInterval(() => {
          animationProgress.value = `${start}%`
          start--
          if (start == end) clearInterval(timer)
        }, 50)
      }
    }
  }
)
</script>

<style scoped>
img {
  position: absolute;
  top: 0;
  width: 80px;
  height: 50px;
  transform: translateX(-50%);
  animation: updown 1.5s infinite;
}

@keyframes updown {
  0% {
    transform: translate(-50%, -20px);
  }
  50% {
    transform: translate(-50%, -10px);
  }
  100% {
    transform: translate(-50%, -20px);
  }
}

@media screen and (max-width: 576px) {
  img {
    width: 60px;
    height: 40px;
    animation: updown2 1.5s infinite;
  }

  @keyframes updown2 {
    0% {
      transform: translate(-50%, -15px);
    }
    50% {
      transform: translate(-50%, -8px);
    }
    100% {
      transform: translate(-50%, -15px);
    }
  }
}
</style>
