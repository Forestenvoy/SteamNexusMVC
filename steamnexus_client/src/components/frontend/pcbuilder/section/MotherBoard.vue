<template>
  <!-- 過濾 -->
  <CRow>
    <CCol xs="12" md="6">
      <h3>廠牌</h3>
    </CCol>
    <CCol xs="12" md="6">
      <CForm @change="filter">
        <CFormCheck type="radio" inline label="全部" value="All" v-model="brand" />
        <CFormCheck type="radio" inline label="華碩" value="華碩" v-model="brand" />
        <CFormCheck type="radio" inline label="技嘉" value="技嘉" v-model="brand" />
        <CFormCheck type="radio" inline label="微星" value="微星" v-model="brand" />
        <CFormCheck type="radio" inline label="華擎" value="華擎" v-model="brand" />
        <CFormCheck type="radio" inline label="NZXT" value="NZXT" v-model="brand" />
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
      <h3>主機板</h3>
    </CCol>
    <CCol xs="12" md="6">
      <select name="CPU" class="form-select" v-model="selectedMB" @change="selectedProduct">
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

// pinia
import { useBuilderStore } from '@/stores/builder.js'
const builderStore = useBuilderStore()

// API URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

// Original Data : CPU Products (Classified)
const MBGroups = ref({})

// Sort Data
const SortGroups = ref({})

// Selected Product
const selectedMB = ref(0)

// filter ref
const brand = ref('All')
const min = ref('')
const max = ref('')

// (Async) get MB Data
const getData = async () => {
  console.log('Get CPU Data...')
  await fetch(`${apiUrl}/api/PcBuilder?type=10003`, {
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
    if (!MBGroups.value[className]) {
      MBGroups.value[className] = []
    }
    MBGroups.value[className].push(item)
  })
  // 將資料存入 session storage
  sessionStorage.setItem('MBList', JSON.stringify(MBGroups.value))
  SortGroups.value = MBGroups.value
}

// Filter By Socket
const filterBySocket = () => {
  const filteredData = Object.keys(MBGroups.value)
    .filter((key) => key.includes(builderStore.getSocket))
    .reduce((obj, key) => {
      obj[key] = MBGroups.value[key]
      return obj
    }, {})
  MBGroups.value = filteredData
  SortGroups.value = MBGroups.value
}

// Filter
const filter = () => {
  filterByBrand()
  filterByPrice()
}

// Filter By Brand
const filterByBrand = () => {
  if (brand.value === 'All') {
    SortGroups.value = MBGroups.value
  } else {
    // 過濾名稱內含有該品牌的資料
    const filteredGroups = Object.keys(MBGroups.value).reduce((result, key) => {
      const filteredProducts = MBGroups.value[key].filter((product) =>
        product.name.includes(brand.value)
      )
      if (filteredProducts.length > 0) {
        result[key] = filteredProducts
      }
      return result
    }, {})
    SortGroups.value = filteredGroups
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

// 產品選擇事件
const selectedProduct = (event) => {
  const Id = event.target.value
  const endIndex = event.target.options[event.target.selectedIndex].text.indexOf(',')
  const Name = event.target.options[event.target.selectedIndex].text.slice(0, endIndex)
  const Price = event.target.options[event.target.selectedIndex].getAttribute('data-price')
  const Wattage = event.target.options[event.target.selectedIndex].getAttribute('data-wattage')
  const Product = {
    type: 'MB',
    id: Id,
    name: Name,
    price: Price,
    wattage: Wattage
  }
  // 加入至產品清單
  builderStore.addProduct('MB', Product)
}

onMounted(() => {
  // 檢查 session storage 是否有資料
  const storedData = sessionStorage.getItem('MBList')
  if (storedData !== null && storedData !== undefined) {
    MBGroups.value = JSON.parse(storedData)
    SortGroups.value = MBGroups.value
  } else {
    getData()
  }
  filterBySocket()
})
</script>

<style scoped>
@import '@/assets/styles/builder.css';
</style>
