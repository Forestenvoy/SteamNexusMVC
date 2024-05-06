<template>
  <div id="main" class="layout-navbar navbar-fixed mainbody position-relative">
    <header>
      <nav class="navbar navbar-expand navbar-light navbar-top" id="pageNav">
        <div class="container-fluid">
          <a href="#" class="burger-btn d-block d-xl-none pe-4">
            <i class="bi bi-justify fs-3"></i>
          </a>
          <h2 class="p-0 d-none d-md-block" id="SystemTitle">SteamNexus Admin Panel</h2>
          <div class="collapse navbar-collapse" id="navbarSupportedContent">
            <ul class="navbar-nav ms-auto mb-lg-0">
              <li class="nav-item dropdown me-3">
                <a
                  class="nav-link active dropdown-toggle text-gray-600"
                  href="#"
                  data-bs-toggle="dropdown"
                  data-bs-display="static"
                  aria-expanded="false"
                >
                  <i class="bi bi-bell bi-sub fs-4"></i>
                  <span class="badge badge-notification bg-danger">5</span>
                </a>
                <ul
                  class="dropdown-menu dropdown-center dropdown-menu-sm-end notification-dropdown"
                  aria-labelledby="dropdownMenuButton"
                >
                  <li class="dropdown-header">
                    <h6>通知</h6>
                  </li>
                  <li class="dropdown-item notification-item">
                    <a class="d-flex align-items-center" href="#">
                      <div class="notification-icon bg-primary"></div>
                      <div class="notification-text ms-4">
                        <p class="notification-title font-bold">遊戲價格更新</p>
                        <p class="notification-subtitle font-thin text-sm">555筆變動</p>
                      </div>
                    </a>
                  </li>
                  <li class="dropdown-item notification-item">
                    <a class="d-flex align-items-center" href="#">
                      <div class="notification-icon bg-success"></div>
                      <div class="notification-text ms-4">
                        <p class="notification-title font-bold">硬體零件更新</p>
                        <p class="notification-subtitle font-thin text-sm">CPU已更新</p>
                      </div>
                    </a>
                  </li>
                  <li>
                    <p class="text-center py-2 mb-0"><a href="#">See all notification</a></p>
                  </li>
                </ul>
              </li>
            </ul>
            <div class="dropdown">
              <a href="#" data-bs-toggle="dropdown" aria-expanded="false">
                <div class="user-menu d-flex">
                  <div class="user-name text-end me-3">
                    <h6 class="mb-0 text-gray-600">王小明</h6>
                    <p class="mb-0 text-sm text-gray-600">系統管理員</p>
                  </div>
                  <div class="user-img d-flex align-items-center">
                    <div class="avatar avatar-md">
                      <img src="/mazer/assets/compiled/jpg/2.jpg" />
                    </div>
                  </div>
                </div>
              </a>
              <ul
                class="dropdown-menu dropdown-menu-end"
                aria-labelledby="dropdownMenuButton"
                style="min-width: 11rem"
              >
                <li>
                  <a class="dropdown-item" href="#">
                    <i class="icon-mid bi bi-person me-2"></i>
                    個人資訊
                  </a>
                </li>
                <li>
                  <a class="dropdown-item" href="#">
                    <i class="icon-mid bi bi-gear me-2"></i>
                    設定
                  </a>
                </li>
                <li>
                  <a class="dropdown-item" href="#">
                    <i class="icon-mid bi bi-box-arrow-left me-2"></i>
                    登出
                  </a>
                </li>
              </ul>
            </div>
          </div>
        </div>
      </nav>
    </header>
    <div id="main-content" class="wrapperdiv">
      <div class="page-heading">
        <!-- router-view -->
      </div>
    </div>
  </div>
</template>
<script>
export default {}
</script>
<style></style>
