<template>
  <!-- 選單 -->
  <CRow>
    <CCol xs="12" md="6">
      <h3>HDD</h3>
    </CCol>
    <CCol xs="12" md="6">
      <select name="HDD" class="form-select" v-model="selectedHDD" @change="selectedProduct">
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

// Original Data : HDD Products (Classified)
const HDDGroups = ref({})

// Sort Data
const SortGroups = ref({})

// Selected Product
const selectedHDD = ref(0)

// (Async) get HDD Data
const getData = async () => {
  console.log('Get HDD Data...')
  await fetch(`${apiUrl}/api/PcBuilder?type=10005`, {
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
    if (!HDDGroups.value[className]) {
      HDDGroups.value[className] = []
    }
    HDDGroups.value[className].push(item)
  })
  // 將資料存入 session storage
  sessionStorage.setItem('HDDList', JSON.stringify(HDDGroups.value))
  SortGroups.value = HDDGroups.value
}

onMounted(() => {
  // 檢查 session storage 是否有資料
  const storedData = sessionStorage.getItem('HDDList')
  if (storedData !== null && storedData !== undefined) {
    HDDGroups.value = JSON.parse(storedData)
    SortGroups.value = HDDGroups.value
  } else {
    getData()
  }
})
</script>

<style scoped>
@import '@/assets/styles/builder.css';
</style>
