<template>
  <CModalHeader>
    <CModalTitle id="MenuModal">Create Menu</CModalTitle>
  </CModalHeader>
  <CModalBody>
    <CRow class="mb-3">
      <CCol
        xs="12"
        md="2"
        class="d-flex align-items-center justify-content-center justify-content-md-start mb-3 mb-md-0"
      >
        <label for="CPU" class="m-0 h5">CPU</label>
      </CCol>
      <CCol xs="12" md="10">
        <select name="CPU" class="form-select">
          <optgroup v-for="(group, groupName) in cpuGroups" :label="groupName" :key="groupName">
            <option
              v-for="item in group"
              :value="item.productId"
              :key="item.productId"
              :data-price="item.price"
            >
              {{ item.productName }} {{ item.specification }} ,${{ item.price }}
            </option>
          </optgroup>
        </select>
      </CCol>
    </CRow>
    <CRow>
      <CCol xs="2" class="d-flex align-items-center">
        <label for="Motherboard" class="m-0 h5">Motherboard</label>
      </CCol>
      <CCol xs="10">
        <select name="Motherboard" class="form-select">
          <option value="Motherboard">Motherboard</option>
        </select>
      </CCol>
    </CRow>
  </CModalBody>
  <CModalFooter>
    <CButton color="secondary"> Close </CButton>
    <CButton color="primary">Save changes</CButton>
  </CModalFooter>
</template>

<script setup>
import { CModalHeader, CModalTitle, CModalBody, CModalFooter, CButton } from '@coreui/vue'
import { ref, onMounted } from 'vue'

// 從環境變數取得 API BASE URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

let cpuGroups = ref({})

// 取得全部產品資料
function getProducts(type) {
  fetch(`${apiUrl}/api/HardwareManage/GetProductData?Type=${type}`, { method: 'GET' })
    .then((response) => {
      return response.json()
    })
    .then((data) => {
      productClassify(data)
    })
    .catch((error) => {
      console.error('Error:', error)
    })
}

// 產品分類
function productClassify(data) {
  data.forEach((item) => {
    // 讀取 產品分類
    const Classification = item.componentClassificationName
    // 如果分類數組不存在，則建立
    if (!cpuGroups.value[Classification]) {
      cpuGroups.value[Classification] = []
    }
    // 將產品資料加入到對應的分類中
    cpuGroups.value[Classification].push(item)
  })
}

onMounted(() => {
  getProducts(10000)
})
</script>

<style></style>
