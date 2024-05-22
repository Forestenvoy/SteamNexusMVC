<script setup>
import { RouterLink } from 'vue-router'
import { useSidebarStore } from '@/stores/sidebar.js'

const sidebar = useSidebarStore()
</script>

<template>
  <CSidebar
    class="border-end"
    position="fixed"
    :unfoldable="sidebar.unfoldable"
    :visible="sidebar.visible"
    @visible-change="(value) => sidebar.toggleVisible(value)"
  >
    <CSidebarHeader class="border-bottom">
      <!-- Logo -->
      <router-link to="/" style="text-decoration: none">
        <!--  v-slot="{ href, navigate }" -->
        <!-- v-bind="$attrs" as="a" :href="href" @click="navigate" -->
        <CSidebarBrand>
          <img
            class="mx-2 sidebar-brand-narrow"
            style="width: 30px; height: 30px; padding-bottom: 3px"
            src="/img/logo.png"
          />
          <img
            class="mx-2 sidebar-brand-full"
            style="width: 30px; height: 30px; padding-bottom: 3px; margin-bottom: 13px"
            src="/img/logo.png"
          />
          <span
            class="text-uppercase sidebar-brand-full"
            style="
              font-family: 'Oswald', sans-serif;
              font-weight: 500;
              font-size: 30px;
              color: white;
            "
            >SteamNexus</span
          >
        </CSidebarBrand>
      </router-link>
      <!-- 側邊欄關閉按鈕(X) -- 手機板專用 -->
      <CCloseButton class="d-lg-none" dark @click="sidebar.toggleVisible()" />
    </CSidebarHeader>
    <!-- 側邊欄選單 -->
    <CSidebarNav>
      <CNavTitle>Admin Dashboard</CNavTitle>
      <CNavGroup>
        <template #togglerContent>
          <CIcon customClassName="nav-icon" icon="cil-people" /> 會員管理
        </template>
        <CNavItem href="#" @click="$router.push('/admin/memberManage')">
          <span class="nav-icon"><span class="nav-icon-bullet"></span></span> 會員資料
        </CNavItem>
        <CNavItem href="#" @click="$router.push('/admin/roleManage')">
          <span class="nav-icon"><span class="nav-icon-bullet"></span></span> 權限管理
        </CNavItem>
      </CNavGroup>
      <CNavGroup>
        <template #togglerContent>
          <CIcon customClassName="nav-icon" icon="cil-laptop" /> 硬體管理
        </template>
        <CNavItem href="#" @click="$router.push('/admin/productManage')">
          <span class="nav-icon"><span class="nav-icon-bullet"></span></span> 產品管理
        </CNavItem>
        <CNavItem href="#" @click="$router.push('/admin/menuManage')">
          <span class="nav-icon"><span class="nav-icon-bullet"></span></span> 菜單管理
        </CNavItem>
      </CNavGroup>
      <CNavItem href="#" @click="$router.push('/admin/gameManage')">
        <CIcon customClassName="nav-icon" icon="cilGamepad" /> 遊戲管理
      </CNavItem>
      <CNavItem href="#" @click="$router.push('/admin/adManage')">
        <CIcon customClassName="nav-icon" icon="cilSpeech" /> 公告管理
      </CNavItem>
    </CSidebarNav>
    <!-- 側邊欄摺疊按鈕(底部) -->
    <CSidebarFooter class="border-top d-none d-lg-flex">
      <CSidebarToggler @click="sidebar.toggleUnfoldable()" />
    </CSidebarFooter>
  </CSidebar>
</template>
