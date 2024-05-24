<template>
  <CNavbar expand="lg" color-scheme="dark" :class="{ sticky: isScrolled }">
    <CContainer fluid>
      <CNavbarBrand href="#" class="pt-2" @click="$router.push('/')">
        <img v-if="!isScrolled" class="mx-2 brand-img" src="/img/logo.png" />
        <img v-else class="mx-2 brand-img" src="/img/logo-dark.png" />
        <span class="text-uppercase brand-title">SteamNexus</span>
      </CNavbarBrand>

      <CNavbarToggler
        aria-label="Toggle navigation"
        aria-expanded="{visible}"
        @click="visible = !visible"
      />
      <!-- 使用者小型介面 -->
      <AccountDropdown class="order-4" v-if="store.getIsLogin" />
      <CCollapse class="navbar-collapse" :visible="visible">
        <CNavbarNav class="mx-xl-auto text-center">
          <CNavItem>
            <CNavLink class="nLink" href="#" @click="$router.push('/')"> 首頁 </CNavLink>
          </CNavItem>
          <CNavItem>
            <CNavLink class="nLink" href="#" @click="$router.push('/pc-builder')">
              硬體估價
            </CNavLink>
          </CNavItem>
        </CNavbarNav>
        <CNavbarNav class="me-0 me-lg-2 order-3 text-center" v-if="store.getUserRole === 'Admin'">
          <CNavItem>
            <CNavLink class="nLink" href="#" @click="$router.push('/admin')"> 後台系統 </CNavLink>
          </CNavItem>
        </CNavbarNav>
        <CNavbarNav class="ms-0 ms-lg-2 mb-2 mb-lg-0 order-5" v-if="!store.getIsLogin">
          <CNavItem>
            <CNavLink 
              class="text-center CNavLink"
              :class="{ nLink: canToggle, login_btn: !canToggle }"
              href="#"
              @click="showLogin"
              ><span> 登入 </span></CNavLink
            >
          </CNavItem>
        </CNavbarNav>
        <CForm class="d-flex order-4 pb-3 pb-lg-0">
          <input
            type="text"
            class="form-control me-2 input-search"
            placeholder="請輸入遊戲關鍵字"
          />
          <CButton style="border: 0px;"
            :color="buttonColor"
            variant="outline"
            @click.prevent="$router.push('/searchSystem')"
            ><i class="fa-solid fa-magnifying-glass"></i></CButton
          >
        </CForm>
      </CCollapse>
    </CContainer>
  </CNavbar>
  <section class="banner"></section>
</template>
<script setup>
import { ref, onMounted } from 'vue'
import { CNavbar, CNavbarBrand, CNavbarToggler, CCollapse } from '@coreui/vue'

import AccountDropdown from '@/components/frontend/share/AccountDropdown.vue'

// 使用 Pinia，利用 store 去訪問狀態
import { useIdentityStore } from '@/stores/identity.js'
const store = useIdentityStore()

const visible = ref(false)

const isScrolled = ref(false)
const buttonColor = ref('light')
const canToggle = ref(false)

// 偵測滾動事件
window.addEventListener('scroll', () => {
  if (window.scrollY > 0) {
    isScrolled.value = true
    buttonColor.value = 'dark'
  } else {
    isScrolled.value = false
    buttonColor.value = 'light'
  }
})

window.addEventListener('resize', () => {
  if (window.innerWidth > 991) {
    canToggle.value = false
  } else {
    canToggle.value = true
  }
})

// 顯示登入視窗
const showLogin = () => {
  store.show()
}

onMounted(() => {
  if (window.innerWidth > 991) {
    canToggle.value = false
  } else {
    canToggle.value = true
  }
})
</script>
<style scoped>

.navbar {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  transition: 0.6s;
  padding: 20px 100px;
  z-index: 10000;
}

@media screen and (max-width: 991px) {
  .navbar {
    background: #000;
  }
}

.sticky {
  padding: 5px 100px;
  background: #fff;
}

@media screen and (max-width: 1400px) {
  .navbar {
    padding: 10px 30px;
  }

  .sticky {
    padding: 0 30px;
  }
}

@media screen and (max-width: 576px) {
  .navbar {
    padding: 0px 0px;
  }
}

.navbar-brand {
  position: relative;
  letter-spacing: 2px;
  transition: 0.6s;
}

.brand-img {
  width: 30px;
  height: 30px;
  padding-bottom: 3px;
  margin-bottom: 13px;
}

.brand-title {
  font-family: 'Oswald', sans-serif;
  font-weight: 500;
  font-size: 30px;
  color: white;
}

.sticky .brand-title {
  color: #000;
}

.navbar-nav {
  position: relative;
}

.nav-item {
  position: relative;
  margin: 0 15px;
}

.nav-link {
  position: relative;
  font-size: 18px;
  text-decoration: none;
  color: #fff;
  letter-spacing: 2px;
  font-weight: 500px;
  transition: 0.6s;
}

.sticky .nav-link {
  color: #000;
}

.sticky input {
  background-color: #d6d6d6;
}

.sticky input::placeholder {
  color: #000;
}

.sticky .navbar-toggler {
  border-color: #d6d6d6;
}

.banner {
  position: relative;
  width: 100%;
  height: 10vh;
  background-image: linear-gradient(to bottom, #393939, #0000);
  background-size: cover;
}

/* 底線效果 */

.nLink::after {
  content: '';
  position: absolute;
  width: 100%;
  height: 0.175rem;
  left: 0;
  bottom: 0;
  background: #fff;
  
  transform: scale(0, 1);
  transition: transform 0.3s ease;
}

.nLink:hover::after {
  transform: scale(1, 1);
}

.sticky .nLink::after {
  background: #000;
}

/* 登入按鈕 */

.login_btn {
  font-weight: 700;
  color: #ffffff;
  border-radius: 10px;
  padding: 18px 34px;
  position: relative;
  background-color: transparent;
  transition: 0.3s ease-in-out;
  width: 80px;
  padding-top: 0;
  padding-bottom: 0;
}

.login_btn span {
  font-size: 25px;
  z-index: 1;
  
}

.login_btn::before {
  content: '';
  width: 0%;
  height: 100%;
  position: absolute;
  background-color:#F3AE0B;
  border-radius: 8px;
  top: 0;
  left: 0;
  z-index: -1;
  transition: 0.3s ease-in-out;
}


.login_btn:hover {
  color: #000;
}

.login_btn:hover::before {
  content: '';
  width: 100%;
  height: 100%;
  position: absolute;
  
}

.sticky .login_btn {
  color: #000;
  border: 1px solid #000;
}

.sticky .login_btn::before {
  background-color: #000;
}

.sticky .login_btn:hover {
  color: #fff;
}
</style>

<style>
.sticky .navbar-toggler-icon {
  background-image: url("data:image/svg+xml,<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 30 30'><path stroke='rgba(37, 43, 54, 0.75)' stroke-linecap='round' stroke-miterlimit='10' stroke-width='2' d='M4 7h22M4 15h22M4 23h22'/></svg>") !important;
}
</style>
