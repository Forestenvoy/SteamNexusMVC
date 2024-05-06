<template>
  <div id="sidebar">
    <div class="sidebar-wrapper active">
      <div class="sidebar-header position-relative">
        <!-- logo start -->
        <div class="d-flex justify-content-between align-items-center">
          <div class="logo">
            <router-link to="/home">
              <img
                class="mx-2"
                style="width: 30px; height: 30px"
                src="/img/logo.png"
                id="logoImg"
              />
              <span
                class="text-uppercase"
                style="
                  font-family: 'Oswald', sans-serif;
                  font-weight: 500;
                  font-size: 30px;
                  color: white;
                "
                id="logoText"
                >SteamNexus</span
              >
            </router-link>
          </div>
          <!-- logo start -->
          <!-- 叉叉 按鈕 start -->
          <div class="sidebar-toggler x">
            <a href="#" class="sidebar-hide d-xl-none d-block"><i class="bi bi-x bi-middle"></i></a>
          </div>
          <!-- 叉叉 按鈕 end -->
        </div>
      </div>
      <!-- SideBar 選單 -->
      <div class="sidebar-menu">
        <ul class="menu">
          <li class="sidebar-title">後台系統</li>
          <li class="sidebar-item has-sub">
            <a href="" class="sidebar-link">
              <i class="bi bi-stack"></i>
              <span>會員管理</span>
            </a>
            <ul class="submenu">
              <li class="submenu-item">
                <a href="" class="submenu-link" id="MemberManageSystem">會員資料</a>
              </li>
              <li class="submenu-item">
                <a href="" class="submenu-link" id="RolesManageSystem">權限管理</a>
              </li>
            </ul>
          </li>
          <li class="sidebar-item has-sub">
            <a href="" class="sidebar-link">
              <i class="bi bi-stack"></i>
              <span>硬體管理</span>
            </a>
            <ul class="submenu">
              <li class="submenu-item">
                <a href="" class="submenu-link" id="HardwareProductManageSystem">產品管理</a>
              </li>
              <li class="submenu-item">
                <a href="" class="submenu-link">菜單管理</a>
              </li>
            </ul>
          </li>
          <li class="sidebar-item has-sub">
            <a href="" class="sidebar-link">
              <i class="bi bi-collection-fill"></i>
              <span>遊戲管理</span>
            </a>
            <ul class="submenu">
              <li class="submenu-item">
                <a href="" class="submenu-link" id="GamesManageSystem">資訊管理</a>
              </li>
            </ul>
          </li>
          <li class="sidebar-item">
            <a href="" class="sidebar-link" id="AdvertiseManageSystem">
              <i class="bi bi-grid-1x2-fill"></i>
              <span>廣告管理</span>
            </a>
          </li>
        </ul>
      </div>
      <!-- 滾動條 -->
      <div class="ps__rail-x" style="left: 0px; bottom: 0px">
        <div class="ps__thumb-x" tabindex="0" style="left: 0px; width: 0px"></div>
      </div>
      <div class="ps__rail-y" style="top: 0px; height: 791px; right: 0px">
        <div class="ps__thumb-y" tabindex="0" style="top: 0px; height: 344px"></div>
      </div>
    </div>
  </div>
</template>
<script>
export default {}
</script>
<style></style>
