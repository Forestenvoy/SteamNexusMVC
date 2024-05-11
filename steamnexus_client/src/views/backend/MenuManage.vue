<template>
  <CContainer class="mt-1 mt-lg-3 mx-3">
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
      <CCol sm="12" md="6" lg="4" class="text-center">
        <CButton color="primary">新增菜單</CButton>
      </CCol>
      <CCol sm="12" md="6" lg="3">789</CCol>
    </CRow>
    <!-- 菜單列表 -->
    <CRow> 123 </CRow>
  </CContainer>
</template>
<script setup>
// CCol 可以考慮 手機 sm 平板 md 桌機 lg
import { CContainer, CRow, CCol } from '@coreui/vue'
import { CButton } from '@coreui/vue'
</script>
<style></style>
