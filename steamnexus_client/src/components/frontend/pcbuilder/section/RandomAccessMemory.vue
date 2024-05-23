<template>
  <!-- 過濾 -->
  <CRow>
    <CCol xs="12" md="6">
      <h3>廠牌</h3>
    </CCol>
    <CCol xs="12" md="6">
      <CForm @change="filter">
        <CFormCheck type="radio" inline label="全部" value="All" v-model="brand" />
        <CFormCheck type="radio" inline label="Intel" value="INTEL" v-model="brand" />
        <CFormCheck type="radio" inline label="AMD" value="AMD" v-model="brand" />
        <CFormCheck type="radio" inline label="NVIDIA" value="NVIDIA" v-model="brand" />
      </CForm>
    </CCol>
  </CRow>
  <CRow>
    <CCol xs="12" md="6">
      <h3>價格</h3>
    </CCol>
    <CCol xs="12" md="6">
      <CForm @input="filter">
        <label>$</label>
        <input type="number" placeholder="請輸入最低價格" v-model="min" />
        <label>以上 ~ $</label>
        <input type="number" placeholder="請輸入最高價格" v-model="max" />
        <label>以下</label>
      </CForm>
    </CCol>
  </CRow>
  <!-- 選單 -->
  <CRow>
    <CCol xs="12" md="6">
      <h3>RAM</h3>
    </CCol>
    <CCol xs="12" md="6">
      <select name="RAM" class="form-select" v-model="selectedRAM" @change="selectedProduct">
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
  <!-- 上一頁、下一頁 -->
  <CRow>
    <CCol xs="12" md="6">
      <button class="button-80" @click="builderStore.prev()">上一頁</button>
    </CCol>
    <CCol xs="12" md="6">
      <button class="button-80" @click="builderStore.next()">下一頁</button>
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

// Original Data : CPU Products (Classified)
const RAMGroups = ref({})

// Sort Data
const SortGroups = ref({})

// Selected Product
const selectedRAM = ref(0)

// (Async) get RAM Data
const getData = async () => {
  console.log('Get RAM Data...')
  await fetch(`${apiUrl}/api/PcBuilder?type=10002`, {
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
    if (!RAMGroups.value[className]) {
      RAMGroups.value[className] = []
    }
    RAMGroups.value[className].push(item)
  })
  // 將資料存入 session storage
  sessionStorage.setItem('RAMList', JSON.stringify(RAMGroups.value))
  SortGroups.value = RAMGroups.value
}

onMounted(() => {
  // 檢查 session storage 是否有資料
  const storedData = sessionStorage.getItem('RAMList')
  if (storedData !== null && storedData !== undefined) {
    RAMGroups.value = JSON.parse(storedData)
    SortGroups.value = RAMGroups.value
  } else {
    getData()
  }
})
</script>

<style scope>
@import '@/assets/styles/builder.css';
</style>
