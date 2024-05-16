<template>
  <CNavbar expand="lg" color-scheme="dark">
    <CContainer fluid>
      <CNavbarBrand href="#" class="pt-2">
        <img
          class="mx-2"
          style="width: 30px; height: 30px; padding-bottom: 3px; margin-bottom: 13px"
          src="/img/logo.png"
        />
        <span
          class="text-uppercase"
          style="font-family: 'Oswald', sans-serif; font-weight: 500; font-size: 30px; color: white"
          >SteamNexus</span
        >
      </CNavbarBrand>
      <CNavbarToggler
        aria-label="Toggle navigation"
        aria-expanded="{visible}"
        @click="visible = !visible"
      />
      <CCollapse class="navbar-collapse" :visible="visible">
        <CNavbarNav class="mx-xl-auto">
          <CNavItem>
            <CNavLink href="#" @click="$router.push('/')"> 首頁 </CNavLink>
          </CNavItem>
          <CNavItem>
            <CNavLink href="#" @click="$router.push('/computerBuilder')"> 硬體估價 </CNavLink>
          </CNavItem>
          <CDropdown variant="nav-item" :popper="false">
            <CDropdownToggle color="secondary">會員中心</CDropdownToggle>
            <CDropdownMenu class="text-center">
              <CDropdownItem href="#" @click="$router.push('/userData')"
                >會員資料變更</CDropdownItem
              >
              <CDropdownItem href="#" @click="$router.push('/trackedGames')"
                >我的追蹤遊戲</CDropdownItem
              >
            </CDropdownMenu>
          </CDropdown>
        </CNavbarNav>
        <CNavbarNav class="me-0 me-lg-2 order-0">
          <CNavItem>
            <CNavLink href="#" @click="$router.push('/admin')"> 後台系統 </CNavLink>
          </CNavItem>
        </CNavbarNav>
        <CNavbarNav class="ms-0 ms-lg-2 mb-2 mb-lg-0 order-2">
          <CNavItem>
            <CNavLink href="#" active> 登入 </CNavLink>
          </CNavItem>
        </CNavbarNav>
        <CForm class="d-flex order-1">
          <CFormInput type="search" class="me-2" placeholder="Search" />
          <CButton color="light" variant="outline" @click.prevent="$router.push('/searchSystem')"
            >Search</CButton
          >
        </CForm>
      </CCollapse>
    </CContainer>
  </CNavbar>
</template>
<script setup>
import { ref } from 'vue'
import { CNavbar, CNavbarBrand, CNavbarToggler, CCollapse } from '@coreui/vue'

const visible = ref(false)
</script>
<style scoped>
.nav-item {
  padding: 1rem 2rem 1.15rem;
  text-transform: uppercase;
  cursor: pointer;
  color: #ebebeb;
  min-width: 80px;
  margin: auto;
}

.nav-item:hover {
  /* background-image: url('https://scottyzen.sirv.com/Images/v/button.png'); */
  background-color: black;
  background-size: 80% 80%;
  border-radius: 15px;
  color: #27262c;
  animation: spring 300ms ease-out;
  text-shadow: 0 -1px 0 white;
  font-weight: bold;
}
.nav-item:active {
  transform: translateY(4px);
}

@keyframes spring {
  15% {
    -webkit-transform-origin: center center;
    -webkit-transform: scale(1.2, 1.1);
  }
  40% {
    -webkit-transform-origin: center center;
    -webkit-transform: scale(0.95, 0.95);
  }
  75% {
    -webkit-transform-origin: center center;
    -webkit-transform: scale(1.05, 1);
  }
  100% {
    -webkit-transform-origin: center center;
    -webkit-transform: scale(1, 1);
  }
}
</style>
