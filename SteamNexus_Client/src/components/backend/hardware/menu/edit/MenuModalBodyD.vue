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
        <label class="m-0 h5">Menu Name</label>
      </CCol>
      <CCol xs="12" md="10">
        <label class="m-0 h5">{{ props.menuId }}</label>
      </CCol>
    </CRow>
    <select-list
      v-for="product in Products"
      :key="product.id"
      ref="selectLists"
      :type="product.id"
      :type-name="product.name"
      @product-selected="onProductSelected"
    ></select-list>
  </CModalBody>
  <CModalFooter>
    <CRow class="w-100">
      <CCol xs="6" class="d-flex align-items-center">
        <label class="h3 m-0 me-5"
          >$
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
import { ref } from 'vue'

import SelectList from '@/components/backend/hardware/menu/create/SelectList.vue'

// 從環境變數取得 API BASE URL
// const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

const selectLists = ref([])

let totalPrice = ref(0)
let totalWattage = ref(0)
let errorMessage = ref('')

let props = defineProps({
  menuId: Number
})

const emit = defineEmits(['modal-close'])

// 宣告產品分類清單
let Products = [
  {
    id: 10000,
    name: 'CPU'
  },
  {
    id: 10001,
    name: 'GPU'
  },
  {
    id: 10002,
    name: 'RAM'
  },
  {
    id: 10003,
    name: 'MotherBoard'
  },
  {
    id: 10004,
    name: 'SSD'
  },
  {
    id: 10005,
    name: 'HDD'
  },
  {
    id: 10006,
    name: 'AirCooler'
  },
  {
    id: 10007,
    name: 'LiquidCooler'
  },
  {
    id: 10008,
    name: 'CASE'
  },
  {
    id: 10009,
    name: 'PSU'
  },
  {
    id: 10010,
    name: 'OS'
  }
]

// 互動視窗 關閉事件
function modalClose() {
  emit('modal-close')
}

// 產品選擇事件
function onProductSelected(value, price, wattage) {
  // 價格加總
  totalPrice.value = totalPrice.value + price
  // 瓦數加總
  totalWattage.value = totalWattage.value + wattage
}
</script>

<style scoped>
.error-message {
  color: red;
}
</style>
