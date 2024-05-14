<template>
  <CModalHeader>
    <CModalTitle id="MenuModal" class="me-5">Edit Menu</CModalTitle>
    <CModalTitle class="error-message">{{ errorMessage }}</CModalTitle>
  </CModalHeader>
  <CModalBody>
    <CRow class="mb-3">
      <CCol
        xs="12"
        md="2"
        class="d-flex align-items-center justify-content-center justify-content-md-start mb-3 mb-md-0"
      >
        <label class="m-0 h5">ID</label>
      </CCol>
      <CCol xs="12" md="10">
        <label class="m-0 h5">{{ props.menuId }}</label>
      </CCol>
    </CRow>
    <CRow class="mb-3">
      <CCol
        xs="12"
        md="2"
        class="d-flex align-items-center justify-content-center justify-content-md-start mb-3 mb-md-0"
      >
        <label class="m-0 h5">Name</label>
      </CCol>
      <CCol xs="12" md="10">
        <input
          type="text"
          class="form-control"
          id="menuName"
          v-model="menuName"
          placeholder="請輸入菜單名稱"
          pattern="^[A-Za-z0-9\u4e00-\u9fa5]+$"
        />
      </CCol>
    </CRow>
    <select-list
      v-for="product in props.products"
      :key="product.id"
      ref="selectLists"
      :type="product.id"
      :type-name="product.name"
      :selected-id="product.selectedId"
      :ori-price="product.price"
      :ori-wattage="product.wattage"
      @product-selected="onProductSelected"
    ></select-list>
  </CModalBody>
  <CModalFooter>
    <CRow class="w-100">
      <CCol xs="6" class="d-flex align-items-center">
        <label class="h3 m-0 me-5">
          {{ totalPrice.toLocaleString('zh-TW', { style: 'currency', currency: 'TWD' }) }}
        </label>
        <label class="h3 m-0">{{ totalWattage }} 瓦 </label>
      </CCol>
      <CCol xs="6" class="text-end">
        <CButton color="primary" class="me-4"> 更新 </CButton>
        <CButton color="secondary" @click="modalClose"> 關閉 </CButton>
      </CCol>
    </CRow>
  </CModalFooter>
</template>

<script setup>
import { CModalHeader, CModalTitle, CModalBody, CModalFooter } from '@coreui/vue'
import { CRow, CCol, CButton } from '@coreui/vue'
import { ref, onBeforeMount } from 'vue'

import SelectList from '@/components/backend/hardware/menu/edit/SelectList.vue'

// 從環境變數取得 API BASE URL
// const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

const selectLists = ref([])

let errorMessage = ref('')

let props = defineProps({
  menuId: Number,
  menuName: String,
  menuPrice: Number,
  menuWattage: Number,
  products: Array
})

let totalPrice = ref(props.menuPrice)
let totalWattage = ref(props.menuWattage)

let menuName = ref(props.menuName)

const emit = defineEmits(['modal-close'])

// 互動視窗 關閉事件
function modalClose() {
  emit('modal-close')
}

// 產品選擇事件
function onProductSelected(value, price, wattage, oriPrice, oriWattage) {
  // 價格加總
  totalPrice.value = totalPrice.value - oriPrice + price
  // 瓦數加總
  totalWattage.value = totalWattage.value - oriWattage + wattage
}

onBeforeMount(() => {})
</script>

<style scoped>
.error-message {
  color: red;
}
</style>
