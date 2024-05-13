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
    <!-- 菜單列表 -->
    <CRow>
      <CCol
        sm="12"
        md="6"
        lg="4"
        xl="3"
        style="height: 350px"
        class="d-flex justify-content-center align-items-center"
      >
        <menu-card></menu-card>
      </CCol>
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
import { ref } from 'vue'

import MenuModalBodyC from '@/components/backend/hardware/menu/create/MenuModalBodyC.vue'
import MenuCard from '@/components/backend/hardware/MenuCard.vue'

let isModalVisible = ref(false)

// 新增菜單 Modal 開啟
function Menu_Create() {
  isModalVisible.value = true
}

// 訊息結果
function presentResult(result) {
  console.log(result)
}
</script>
<style></style>
