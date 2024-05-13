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
      <menu-card
        v-for="menu in menuLists"
        :key="menu.id"
        :menuId="menu.id"
        :menuName="menu.name"
        :menuPrice="menu.totalPrice"
        :menuCount="menu.count"
        :menuStatus="menu.status"
        :menuActive="menu.active"
      ></menu-card>
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
function presentResult(result) {
  toast.success(result, {
    theme: 'dark',
    autoClose: 2000,
    transition: toast.TRANSITIONS.ZOOM,
    position: toast.POSITION.TOP_CENTER
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

onMounted(() => {
  // 獲取菜單列表
  getMenuList()
})
</script>
<style></style>
