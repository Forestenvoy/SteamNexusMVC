<template>
  <!-- 選單 -->
  <CRow>
    <CCol xs="12" md="6">
      <h3>SSD</h3>
    </CCol>
    <CCol xs="12" md="6">
      <select name="SSD" class="form-select" v-model="selectedSSD" @change="selectedProduct">
        <option :value="0" disabled selected hidden>---- 請選擇硬體 ----</option>
        <optgroup :label="groupName" v-for="(group, groupName) in SortGroups" :key="groupName">
          <option
            v-for="item in group"
            :value="item.id"
            :key="item.id"
            :data-price="item.price"
            :data-wattage="item.wattage"
            :data-recommend="item.recommend"
          >
            {{ item.name }} {{ item.specification }} ,${{ item.price }}
          </option>
        </optgroup>
      </select>
    </CCol>
  </CRow>
</template>

<script setup>
// vue
import { ref, onMounted } from 'vue'

// core UI
import { CRow, CCol, CForm, CFormCheck } from '@coreui/vue'

// pinia
import { useBuilderStore } from '@/stores/builder.js'
const builderStore = useBuilderStore()

// API URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

// Original Data : SSD Products (Classified)
const SSDGroups = ref({})

// Sort Data
const SortGroups = ref({})

// Selected Product
const selectedSSD = ref(0)

// (Async) get SSD Data
const getData = async () => {
  console.log('Get SSD Data...')
  await fetch(`${apiUrl}/api/PcBuilder?type=10004`, {
    method: 'GET'
  })
    .then((response) => {
      if (!response.ok) {
        return response.text().then((errorMessage) => {
          throw new Error(errorMessage)
        })
      }
      return response.json()
    })
    .then((data) => {
      // 分類
      classification(data)
    })
    .catch((error) => {
      console.error('Error:', error.message)
    })
}

// Classification
const classification = (data) => {
  data.forEach((item) => {
    const className = item.classification
    if (!SSDGroups.value[className]) {
      SSDGroups.value[className] = []
    }
    SSDGroups.value[className].push(item)
  })
  // 將資料存入 session storage
  sessionStorage.setItem('SSDList', JSON.stringify(SSDGroups.value))
  SortGroups.value = SSDGroups.value
}

onMounted(() => {
  // 檢查 session storage 是否有資料
  const storedData = sessionStorage.getItem('SSDList')
  if (storedData !== null && storedData !== undefined) {
    SSDGroups.value = JSON.parse(storedData)
    SortGroups.value = SSDGroups.value
  } else {
    getData()
  }
})
</script>

<style scoped>
@import '@/assets/styles/builder.css';
</style>
