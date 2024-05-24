<script setup>
import { onMounted, ref } from 'vue'
import { useColorModes } from '@coreui/vue'

import AccountDropdown from '@/components/frontend/share/AccountDropdown.vue'
import { useSidebarStore } from '@/stores/sidebar.js'

const headerClassNames = ref('mb-4 p-0')
const { colorMode, setColorMode } = useColorModes('coreui-free-vue-admin-template-theme')
const sidebar = useSidebarStore()

onMounted(() => {
  document.addEventListener('scroll', () => {
    if (document.documentElement.scrollTop > 0) {
      headerClassNames.value = 'mb-4 p-0 shadow-sm'
    } else {
      headerClassNames.value = 'mb-4 p-0'
    }
  })
})
</script>

<template>
  <CHeader position="sticky" :class="headerClassNames">
    <CContainer class="border-bottom px-4" fluid id="app-header">
      <!-- 側邊欄展開按鈕 -->
      <input
        type="checkbox"
        id="menu_checkbox"
        @click="sidebar.toggleVisible()"
        style="margin-inline-start: -14px"
      />
      <label for="menu_checkbox" class="menu_btn">
        <div></div>
        <div></div>
        <div></div>
      </label>
      <!-- 通知系統(待製作) -->
      <CHeaderNav class="ms-auto">
        <CNavItem>
          <CNavLink href="#">
            <CIcon icon="cil-bell" size="lg" />
          </CNavLink>
        </CNavItem>
      </CHeaderNav>
      <CHeaderNav>
        <!-- 垂直線 -->
        <li class="nav-item py-1">
          <div class="vr h-100 mx-2 text-body text-opacity-75"></div>
        </li>
        <!-- 日夜模式切換 -->
        <CDropdown variant="nav-item" placement="bottom-end">
          <CDropdownToggle :caret="false">
            <CIcon v-if="colorMode === 'dark'" icon="cil-moon" size="lg" />
            <CIcon v-else-if="colorMode === 'light'" icon="cil-sun" size="lg" />
            <CIcon v-else icon="cil-contrast" size="lg" />
          </CDropdownToggle>
          <CDropdownMenu>
            <CDropdownItem
              :active="colorMode === 'light'"
              class="d-flex align-items-center"
              component="button"
              type="button"
              @click="setColorMode('light')"
            >
              <CIcon class="me-2" icon="cil-sun" size="lg" /> Light
            </CDropdownItem>
            <CDropdownItem
              :active="colorMode === 'dark'"
              class="d-flex align-items-center"
              component="button"
              type="button"
              @click="setColorMode('dark')"
            >
              <CIcon class="me-2" icon="cil-moon" size="lg" /> Dark
            </CDropdownItem>
            <CDropdownItem
              :active="colorMode === 'auto'"
              class="d-flex align-items-center"
              component="button"
              type="button"
              @click="setColorMode('auto')"
            >
              <CIcon class="me-2" icon="cil-contrast" size="lg" /> Auto
            </CDropdownItem>
          </CDropdownMenu>
        </CDropdown>
        <!-- 垂直線 -->
        <li class="nav-item py-1">
          <div class="vr h-100 mx-2 text-body text-opacity-75"></div>
        </li>
        <!-- 使用者小型介面 -->
        <AccountDropdown />
      </CHeaderNav>
    </CContainer>
  </CHeader>
</template>

<style scoped>
/* 漢堡 icon */
#menu_checkbox {
  display: none;
}

.menu_btn {
  display: block;
  width: 30px;
  height: 30px;
  margin-right: 10px;
  cursor: pointer;
}

.menu_btn div {
  position: relative;
  top: 0;
  height: 6px;
  background-color: #fff;
  margin-bottom: 6px;
  transition:
    0.3s ease transform,
    0.3s ease top,
    0.3s ease width,
    0.3s ease right;
  border-radius: 2px;
}

.menu_btn div:first-child {
  transform-origin: 0;
}

.menu_btn div:last-child {
  margin-bottom: 0;
  transform-origin: 30px;
}

.menu_btn div:nth-child(2) {
  right: 0;
  width: 30px;
}
</style>
