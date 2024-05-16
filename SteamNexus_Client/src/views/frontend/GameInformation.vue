<template>
  <div>我是遊戲頁面</div>
  <div>ID : {{ props.gameId }}</div>
</template>
<script setup>
let props = defineProps({
  gameId: Number
})
</script>
<style></style>
