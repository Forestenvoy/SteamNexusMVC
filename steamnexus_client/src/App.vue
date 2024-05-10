<template>
  <router-view></router-view>
</template>

<script setup>
import { onBeforeMount, onMounted } from 'vue'
import { useColorModes } from '@coreui/vue'

import { useThemeStore } from '@/stores/theme.js'

const { isColorModeSet, setColorMode } = useColorModes('coreui-free-vue-admin-template-theme')
const currentTheme = useThemeStore()

onBeforeMount(() => {
  const urlParams = new URLSearchParams(window.location.href.split('?')[1])
  let theme = urlParams.get('theme')

  if (theme !== null && theme.match(/^[A-Za-z0-9\s]+/)) {
    theme = theme.match(/^[A-Za-z0-9\s]+/)[0]
  }

  if (theme) {
    setColorMode(theme)
    return
  }

  if (isColorModeSet()) {
    return
  }

  setColorMode(currentTheme.theme)
})

onMounted(() => {
  const loadingAnimation = document.getElementById('AnimationContainer')
  loadingAnimation.classList.add('animate__animated', 'animate__fadeOut')
  setTimeout(() => {
    loadingAnimation.remove()
    const animationCSS = document.querySelector('link[href="/css/webAnimation.css"]')
    animationCSS.remove()
    const animationJS = document.querySelector('script[src="/js/webAnimation.js"]')
    animationJS.remove()
  }, 2100)
})
</script>

<style lang="scss">
// Import Main styles for this application
@import 'styles/style';
</style>
