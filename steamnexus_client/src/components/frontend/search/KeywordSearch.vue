<template>
  <div>
    <input v-model="searchQuery" @input="performSearch" placeholder="Search"/>
    <ul>
      <li v-for="result in searchResults" :key="result.item.id">{{ result.item.name }}</li>
    </ul>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import Fuse from 'fuse.js';

// 初始化資料
const searchQuery = ref('');
const searchResults = ref([]);

// 初始化 Fuse.js
let fuse;

onMounted(() => {
  fuse = new Fuse(items, {
    keys: ['name'],
    threshold: 0.3,  // 0 至 1，越小越嚴格
  });
});

// 搜索功能
const performSearch = () => {
  searchResults.value = fuse.search(searchQuery.value);
};
</script>

<style scoped>
/* 添加任何需要的樣式 */
</style>
