<template>
  <side-bar></side-bar>
  <main-body></main-body>
  <footer>
    <div class="footer clearfix mb-0 text-muted ml-xl-300">
      <div class="float-start">
        <p>2024 SteamNexus</p>
      </div>
      <div class="float-end">
        <p>Crafted with by 139 全端 第三組</p>
      </div>
    </div>
  </footer>
</template>
<script setup>
import SideBar from '@/components/Backend/sidebar/SideBar.vue'
import MainBody from '@/components/Backend/mainbody/MainBody.vue'
import { onMounted, onBeforeMount } from 'vue'

// 動態重新加載指定的 JavaScript 文件 (Mazer UI)
function reloadScripts(paths) {
  paths.forEach((path) => {
    // 查找舊的相同路徑的 script 標籤
    const oldScripts = document.querySelector(`script[src="${path}"]`)
    // 移除舊有的 script 標籤
    if (oldScripts) {
      oldScripts.remove()
    }
    // 建立新的 script 標籤
    const script = document.createElement('script')
    script.type = 'text/javascript'
    script.src = path
    // 加載新的 script 標籤
    document.body.appendChild(script)
  })
}

onBeforeMount(() => {
  reloadScripts([
    '/mazer/assets/static/js/components/dark.js',
    '/mazer/assets/static/js/initTheme.js',
    '/mazer/assets/extensions/perfect-scrollbar/perfect-scrollbar.min.js',
    '/mazer/assets/compiled/js/app.js'
  ])
})
onMounted(() => {})
</script>
<style></style>
