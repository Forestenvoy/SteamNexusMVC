<script setup>
import { RouterLink, RouterView } from 'vue-router'
import AdminSidebar from './components/AdminSidebar.vue'
import AdminBody from './views/AdminBody.vue'
</script>

<template>
  <AdminSidebar />
  <AdminBody />
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

<!-- <template>
    <AdminSidebar />
    <header>
      <img alt="Vue logo" class="logo" src="@/assets/logo.svg" width="125" height="125" />
  
      <div class="wrapper">
        <HelloWorld msg="You did it!" />
  
        <nav>
          <RouterLink to="/">Home</RouterLink>
          <RouterLink to="/about">About</RouterLink>
        </nav>
      </div>
    </header>
  
    <RouterView />
  </template> -->
