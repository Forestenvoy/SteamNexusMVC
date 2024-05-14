<template>
  <section class="mt-1 mt-lg-3 mx-3">
    <!-- 菜單管理 UI -->
    <CRow>
      <CCol sm="12" md="6" lg="5">
        <h2>Menu Manage System</h2>
        <div>
          <label style="color: grey !important">
            新增、編輯、刪除菜單，且可以自由上下架，並偵測異常菜單
          </label>
        </div>
      </CCol>
      <CCol sm="12" md="6" lg="4" class="d-flex justify-content-center align-items-end">
        <CFormInput
          type="text"
          placeholder="請輸入菜單關鍵字"
          class="me-2"
          style="width: 250px; height: 40px"
        />
        <!-- 搜尋按鈕 -->
        <CButton color="light" class="text-center p-0" style="width: 40px; height: 40px">
          <CIcon icon="cilSearch" style="width: 24px; height: 24px" />
        </CButton>
      </CCol>
      <CCol sm="12" md="6" lg="3" class="d-flex justify-content-end align-items-end">
        <CButton color="light" shape="rounded-0" class="border border-black" @click="Menu_Create()"
          >新增菜單</CButton
        >
      </CCol>
    </CRow>
    <CRow>
      <transition-group name="list">
        <menu-card
          v-for="menu in menuLists"
          :key="menu.id"
          :menuId="menu.id"
          :menuName="menu.name"
          :menuPrice="menu.totalPrice"
          :menuCount="menu.count"
          :menuStatus="menu.status"
          :menuActive="menu.active"
          @menu-delete="deleteCard"
        ></menu-card>
      </transition-group>
    </CRow>
  </section>
  <!-- Modal Start -->
  <!-- 新增、編輯 -->
  <!-- alignment="center" 置中 -->
  <!-- backdrop="static" 靜態 -->
  <CModal
    size="xl"
    alignment="center"
    backdrop="static"
    :visible="isModalVisible"
    @close="
      () => {
        isModalVisible = false
      }
    "
    aria-labelledby="MenuModal"
  >
    <menu-modal-body-c
      @create-result="presentResult"
      @modal-close="isModalVisible = false"
    ></menu-modal-body-c>
  </CModal>
  <!-- Modal End -->
</template>
<script setup>
// CCol 可以考慮 手機 sm 平板 md 桌機 lg
import { CRow, CCol } from '@coreui/vue'
import { CButton, CFormInput } from '@coreui/vue'
import { ref, onMounted } from 'vue'
import { toast } from 'vue3-toastify'
import 'vue3-toastify/dist/index.css'
import MenuModalBodyC from '@/components/backend/hardware/menu/create/MenuModalBodyC.vue'
import MenuCard from '@/components/backend/hardware/MenuCard.vue'

// 從環境變數取得 API BASE URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

let isModalVisible = ref(false)

let menuLists = ref([])

// 新增菜單 Modal 開啟
function Menu_Create() {
  isModalVisible.value = true
}

// 訊息結果
function presentResult(result, isSuccess, menuId) {
  if (isSuccess) {
    insertMenu(menuId)
  }

  toast.success(result, {
    theme: 'dark',
    autoClose: 2000,
    transition: toast.TRANSITIONS.ZOOM,
    position: toast.POSITION.TOP_CENTER
  })
}

// 成功的話 插入列表
function insertMenu(menuId) {
  getMenu(menuId)
}

// 獲取單一菜單資料
function getMenu(id) {
  fetch(`${apiUrl}/api/HardwareManage/GetMenu?MenuId=${id}`, { method: 'GET' })
    .then((response) => {
      if (!response.ok) {
        throw new Error('NetworkError')
      }
      return response.json()
    })
    .then((data) => {
      menuLists.value.push(data)
    })
    .catch((error) => {
      console.error(error.message)
    })
}
// 獲取菜單列表
function getMenuList() {
  fetch(`${apiUrl}/api/HardwareManage/GetMenuList`, { method: 'GET' })
    .then((response) => {
      if (!response.ok) {
        throw new Error('NetworkError')
      }
      return response.json()
    })
    .then((data) => {
      menuLists.value = data
    })
    .catch((error) => {
      console.error(error.message)
    })
}

// 刪除菜單(畫面)
function deleteCard(id) {
  menuLists.value = menuLists.value.filter((item) => item.id !== id)
}

onMounted(() => {
  // 獲取菜單列表
  getMenuList()
})
</script>
<style scoped>
.list-move, /* 对移动中的元素应用的过渡 */
.list-enter-active,
.list-leave-active {
  transition: all 0.5s cubic-bezier(0.55, 0, 0.1, 1);
}

.list-enter-from,
.list-leave-to {
  opacity: 0;
  transform: scaleY(0.01) translate(30px, 0);
}

/* 确保将离开的元素从布局流中删除
  以便能够正确地计算移动的动画。 */
.list-leave-active {
  position: absolute;
}
</style>
