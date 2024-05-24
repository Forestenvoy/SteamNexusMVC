<template>
  <!-- 過濾 -->
  <CRow>
    <CCol xs="12" md="6">
      <h3>廠牌</h3>
    </CCol>
    <CCol xs="12" md="6">
      <CForm @change="filter">
        <CFormCheck type="radio" inline label="全部" value="All" v-model="brand" />
        <CFormCheck type="radio" inline label="君主" value="君主" v-model="brand" />
        <CFormCheck type="radio" inline label="ID-COOLING" value="ID-COOLING" v-model="brand" />
        <CFormCheck type="radio" inline label="be quiet!" value="be quiet!" v-model="brand" />
        <CFormCheck type="radio" inline label="美洲獅" value="美洲獅" v-model="brand" />
        <CFormCheck type="radio" inline label="利民" value="利民" v-model="brand" />
        <CFormCheck type="radio" inline label="九州風神" value="九州風神" v-model="brand" />
        <CFormCheck type="radio" inline label="銀欣" value="銀欣" v-model="brand" />
        <CFormCheck type="radio" inline label="酷碼" value="酷碼" v-model="brand" />
        <CFormCheck type="radio" inline label="鎌刀" value="鎌刀" v-model="brand" />
        <CFormCheck type="radio" inline label="喬思伯" value="喬思伯" v-model="brand" />
        <CFormCheck type="radio" inline label="貓頭鷹" value="貓頭鷹" v-model="brand" />
        <CFormCheck type="radio" inline label="快睿" value="快睿" v-model="brand" />
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
      <h3>AirCooler</h3>
    </CCol>
    <CCol xs="12" md="6">
      <select
        name="AirCooler"
        class="form-select"
        v-model="selectedAirCooler"
        @change="selectedProduct"
      >
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
</template>

<script setup>
// vue
import { ref, onMounted, watch } from 'vue'

// core UI
import { CRow, CCol, CForm, CFormCheck } from '@coreui/vue'

// pinia
import { useBuilderStore } from '@/stores/builder.js'
const builderStore = useBuilderStore()

// API URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

// Original Data : AirCooler Products (Classified)
const AirCoolerGroups = ref({})

// Sort Data
const SortGroups = ref({})

// props
const props = defineProps(['defaultSelected'])

// emits
const emits = defineEmits(['selected'])

// Selected Product
const selectedAirCooler = ref(props.defaultSelected)

// filter ref
const brand = ref('All')
const min = ref('')
const max = ref('')

// (Async) get AirCooler Data
const getData = async () => {
  console.log('Get AirCooler Data...')
  await fetch(`${apiUrl}/api/PcBuilder?type=10006`, {
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
    if (!AirCoolerGroups.value[className]) {
      AirCoolerGroups.value[className] = []
    }
    AirCoolerGroups.value[className].push(item)
  })
  // 將資料存入 session storage
  sessionStorage.setItem('AirCoolerList', JSON.stringify(AirCoolerGroups.value))
  SortGroups.value = AirCoolerGroups.value
}

// Filter
const filter = () => {
  filterByBrand()
  filterByPrice()
}

// Filter By Brand
const filterByBrand = () => {
  if (brand.value === 'All') {
    SortGroups.value = AirCoolerGroups.value
  } else {
    // 過濾名稱內含有該品牌的資料
    const filteredData = Object.keys(AirCoolerGroups.value)
      .filter((key) => key.includes(brand.value))
      .reduce((obj, key) => {
        obj[key] = AirCoolerGroups.value[key]
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

// 產品選擇事件
const selectedProduct = (event) => {
  const Id = event.target.value
  const endIndex = event.target.options[event.target.selectedIndex].text.indexOf(',')
  const Name = event.target.options[event.target.selectedIndex].text.slice(0, endIndex)
  const Price = event.target.options[event.target.selectedIndex].getAttribute('data-price')
  const Wattage = event.target.options[event.target.selectedIndex].getAttribute('data-wattage')
  const Product = {
    type: 'AirCooler',
    id: Id,
    name: Name,
    price: Price,
    wattage: Wattage
  }
  // 加入至產品清單
  builderStore.addProduct('AirCooler', Product)
  emits('selected', selectedAirCooler.value)
}

// 監看 props.defaultSelected 是否有變動
watch(
  () => props.defaultSelected,
  (newValue) => {
    selectedAirCooler.value = newValue
  }
)

onMounted(() => {
  // 檢查 session storage 是否有資料
  const storedData = sessionStorage.getItem('AirCoolerList')
  if (storedData !== null && storedData !== undefined) {
    AirCoolerGroups.value = JSON.parse(storedData)
    SortGroups.value = AirCoolerGroups.value
  } else {
    getData()
  }
})
</script>

<style scoped>
@import '@/assets/styles/builder.css';
</style>
