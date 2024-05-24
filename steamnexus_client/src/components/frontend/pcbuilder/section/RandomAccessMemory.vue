<template>
  <!-- 過濾 -->
  <CRow>
    <CCol xs="12" md="6">
      <h3>廠牌</h3>
    </CCol>
    <CCol xs="12" md="6">
      <CForm @change="filter">
        <CFormCheck type="radio" inline label="全部" value="All" v-model="brand" />
        <CFormCheck type="radio" inline label="UMAX" value="UMAX" v-model="brand" />
        <CFormCheck type="radio" inline label="威剛" value="威剛" v-model="brand" />
        <CFormCheck type="radio" inline label="金士頓" value="金士頓" v-model="brand" />
        <CFormCheck type="radio" inline label="美光" value="美光" v-model="brand" />
        <CFormCheck type="radio" inline label="科賦" value="科賦" v-model="brand" />
        <CFormCheck type="radio" inline label="十銓" value="十銓" v-model="brand" />
        <CFormCheck type="radio" inline label="芝奇" value="芝奇" v-model="brand" />
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
  <CRow>
    <CCol xs="12" md="6">
      <h3>容量</h3>
    </CCol>
    <CCol xs="12" md="6">
      <CForm @input="filter">
        <label></label>
        <input type="number" placeholder="請輸入最低價格" v-model="smin" />
        <label>以上 ~ </label>
        <input type="number" placeholder="請輸入最高價格" v-model="smax" />
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
            :data-size="item.size"
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

// filter ref
const brand = ref('All')
const min = ref('')
const max = ref('')
const smin = ref('')
const smax = ref('')

// (Async) get RAM Data
const getData = async () => {
  console.log('Get RAM Data...')
  await fetch(`${apiUrl}/api/PcBuilder/GetRAMData`, {
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
  filterByMemory()
}

// Filter By Memory
const filterByMemory = () => {
  const filteredData = Object.keys(RAMGroups.value)
    .filter((key) => key.includes(builderStore.getMemory))
    .reduce((obj, key) => {
      obj[key] = RAMGroups.value[key]
      return obj
    }, {})
  RAMGroups.value = filteredData
  SortGroups.value = RAMGroups.value
}

// Filter
const filter = () => {
  filterByBrand()
  filterByPrice()
  filterBySize()
}

// Filter By Brand
const filterByBrand = () => {
  if (brand.value !== 'All') {
    // 過濾名稱內含有該品牌的資料
    const filteredGroups = Object.keys(RAMGroups.value).reduce((result, key) => {
      const filteredProducts = RAMGroups.value[key].filter((product) =>
        product.name.includes(brand.value)
      )
      if (filteredProducts.length > 0) {
        result[key] = filteredProducts
      }
      return result
    }, {})
    SortGroups.value = filteredGroups
  } else {
    SortGroups.value = RAMGroups.value
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

// Filter By Size
const filterBySize = () => {
  // 過濾最低價格
  if (smin.value !== '') {
    const filteredGroups = Object.keys(SortGroups.value).reduce((result, key) => {
      const filteredProducts = SortGroups.value[key].filter((product) => product.size >= smin.value)
      if (filteredProducts.length > 0) {
        result[key] = filteredProducts
      }
      return result
    }, {})
    SortGroups.value = filteredGroups
  }
  // 過濾最高價格
  if (smax.value !== '') {
    const filteredGroups = Object.keys(SortGroups.value).reduce((result, key) => {
      const filteredProducts = SortGroups.value[key].filter((product) => product.size <= smax.value)
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
    type: 'RAM',
    id: Id,
    name: Name,
    price: Price,
    wattage: Wattage
  }
  // 加入至產品清單
  builderStore.addProduct('RAM', Product)
}

onMounted(() => {
  // 檢查 session storage 是否有資料
  const storedData = sessionStorage.getItem('RAMList')
  if (storedData !== null && storedData !== undefined) {
    RAMGroups.value = JSON.parse(storedData)
    SortGroups.value = RAMGroups.value
    filterByMemory()
  } else {
    getData()
  }
})
</script>

<style scope>
@import '@/assets/styles/builder.css';
</style>
