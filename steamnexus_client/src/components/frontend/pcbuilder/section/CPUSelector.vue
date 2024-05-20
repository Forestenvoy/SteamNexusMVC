<template>
  <!-- 過濾 -->
  <CRow>
    <CCol xs="12" md="6">
      <h3>廠牌</h3>
    </CCol>
    <CCol xs="12" md="6">
      <CForm @change="filter">
        <CFormCheck type="radio" inline label="全部" value="All" v-model="brand" />
        <CFormCheck type="radio" inline label="Intel" value="Intel" v-model="brand" />
        <CFormCheck type="radio" inline label="AMD" value="AMD" v-model="brand" />
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
      <h3>CPU</h3>
    </CCol>
    <CCol xs="12" md="6">
      <select name="CPU" class="form-select" v-model="selectedCPU" @change="selectedProduct">
        <option value="0" disabled selected hidden>---- 請選擇硬體 ----</option>
        <optgroup :label="groupName" v-for="(group, groupName) in SortGroups" :key="groupName">
          <option
            v-for="item in group"
            :value="item.id"
            :key="item.id"
            :data-price="item.price"
            :data-wattage="item.wattage"
            :data-recommend="item.recommend"
          >
            {{ item.productName }} {{ item.specification }} ,${{ item.price }}
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

// API URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

// Original Data : CPU Products (Classified)
const CPUGroups = ref({})

// filter ref
const brand = ref('All')
const min = ref('')
const max = ref('')
const selectedCPU = ref(0)

// Sort Data
const SortGroups = ref({})

// (Async) get CPU Data
const getData = async () => {
  console.log('Get CPU Data...')
  await fetch(`${apiUrl}/api/PcBuilder?type=10000`, {
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
    if (!CPUGroups.value[className]) {
      CPUGroups.value[className] = []
    }
    CPUGroups.value[className].push(item)
  })
  // 將資料存入 session storage
  sessionStorage.setItem('CPUList', JSON.stringify(CPUGroups.value))
  SortGroups.value = CPUGroups.value
}

// Filter
const filter = () => {
  filterByBrand()
  filterByPrice()
}

// Filter By Brand
const filterByBrand = () => {
  if (brand.value === 'All') {
    SortGroups.value = CPUGroups.value
  } else {
    // 過濾名稱內含有該品牌的資料
    const filteredData = Object.keys(CPUGroups.value)
      .filter((key) => key.includes(brand.value))
      .reduce((obj, key) => {
        obj[key] = CPUGroups.value[key]
        return obj
      }, {})
    SortGroups.value = filteredData
  }
}

// Filter By Price
const filterByPrice = () => {
  // 過濾最低價格
  if (min.value !== '') {
    const filteredGroups = Object.keys(SortGroups.value).reduce((result, key) => {
      const filteredProducts = SortGroups.value[key].filter((product) => product.price >= min.value)
      if (filteredProducts.length > 0) {
        result[key] = filteredProducts
      }
      return result
    }, {})
    SortGroups.value = filteredGroups
  }
  // 過濾最高價格
  if (max.value !== '') {
    const filteredGroups = Object.keys(SortGroups.value).reduce((result, key) => {
      const filteredProducts = SortGroups.value[key].filter((product) => product.price <= max.value)
      if (filteredProducts.length > 0) {
        result[key] = filteredProducts
      }
      return result
    }, {})
    SortGroups.value = filteredGroups
  }
}

onMounted(() => {
  // 檢查 session storage 是否有資料
  const storedData = sessionStorage.getItem('CPUList')
  if (storedData !== null && storedData !== undefined) {
    CPUGroups.value = JSON.parse(storedData)
    SortGroups.value = CPUGroups.value
  } else {
    getData()
  }
})
</script>

<style scoped>
/* 隱藏數字增減按鈕 */
input::-webkit-outer-spin-button,
input::-webkit-inner-spin-button {
  -webkit-appearance: none;
  margin: 0;
}

input[type='number'] {
  -webkit-appearance: textfield; /* Safari */
  -moz-appearance: textfield; /* Firefox */
  appearance: textfield;
}
</style>
