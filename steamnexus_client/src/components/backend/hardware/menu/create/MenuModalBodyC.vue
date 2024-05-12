<template>
  <CModalHeader>
    <CModalTitle id="MenuModal">Create Menu</CModalTitle>
  </CModalHeader>
  <CModalBody>
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
        <label class="h3 m-0"
          >$ {{ totalPrice.toLocaleString('zh-TW', { style: 'currency', currency: 'TWD' }) }}</label
        >
      </CCol>
      <CCol xs="6" class="text-end">
        <CButton color="primary" class="me-4"> 建立 </CButton>
        <CButton color="secondary" @click="$emit('modal-close')"> 關閉 </CButton>
      </CCol>
    </CRow>
  </CModalFooter>
</template>

<script setup>
import { CModalHeader, CModalTitle, CModalBody, CModalFooter } from '@coreui/vue'
import { CRow, CCol, CButton } from '@coreui/vue'
import { ref } from 'vue'

import SelectList from '@/components/backend/hardware/menu/create/SelectList.vue'

const selectLists = ref([])
let totalPrice = ref(0)

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
</script>

<style></style>
